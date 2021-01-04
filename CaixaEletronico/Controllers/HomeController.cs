using CaixaEletronico.Class;
using CaixaEletronico.Data;
using CaixaEletronico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CaixaEletronico.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext db;

        public HomeController(DBContext context, ILogger<HomeController> logger)
        {
            db = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        public PartialViewResult Caixa()
        {
            return PartialView("Views/Home/Components/Caixa/Caixa.cshtml", db.Notas.ToList());
        }

        [HttpGet]
        public PartialViewResult Saque()
        {
            ViewBag.Sucesso = null;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult Saque(Saque saque)
        {
            if (saque.Valor.HasValue)
            {
                int valorInicial = saque.Valor.Value;
                int valorRestante = saque.Valor.Value;
                var caixa = db.Notas.OrderByDescending(x => x.Valor).ToList();

                List<ListNotas> notasNecessarias = new List<ListNotas>(); //Lista contendo as notas a movimentar

                foreach (var x in caixa)
                {
                    int qtdNotas = 0;

                    if (x.Quantidade > 0)
                    {
                        qtdNotas = (int)Math.Floor(valorRestante / Convert.ToDecimal(x.Valor));

                        if ((x.Quantidade - qtdNotas) < 0)
                        {
                            qtdNotas = x.Quantidade;
                        }

                        notasNecessarias.Add(new ListNotas()
                        {
                            Valor = x.Valor,
                            Quantidade = qtdNotas,
                            ValorTotal = x.Valor * qtdNotas
                        });
                        valorRestante -= (x.Valor * qtdNotas);
                    }

                }
                int valorTotalDisponivel = notasNecessarias.Sum(x => x.ValorTotal);
                string mensagem;
                mensagem = $"Valor [{valorInicial:C2}] não disponivél.";
                if (valorTotalDisponivel < valorInicial && valorTotalDisponivel > 0)
                {
                    mensagem += $" Tente[{ valorTotalDisponivel:C2}]";
                    int totalCaixa = caixa.Sum(x => x.Quantidade * x.Valor);
                    foreach (var x in caixa.OrderBy(x => x.Valor))
                    {
                        int notas = notasNecessarias.Where(a => a.Valor == x.Valor).FirstOrDefault().Quantidade;
                        if ((x.Quantidade - notas) > 0)
                        {
                            mensagem += $" ou [{valorTotalDisponivel + x.Valor:C2}]";
                        }

                    }
                    mensagem += ".";

                    ModelState.AddModelError("Valor", mensagem);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        foreach (var x in notasNecessarias)
                        {
                            if (x.Quantidade > 0)
                            {
                                Notas notas = db.Notas.Where(a => a.Valor == x.Valor).FirstOrDefault();

                                notas.Quantidade += -x.Quantidade;

                                db.SaveChanges();
                            }

                        }
                        ViewBag.Sucesso = $"Saque do valor [{valorTotalDisponivel:C2}] realizado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Sucesso = null;
                        ModelState.AddModelError("Valor", "Ocorreu um erro ao salvar os dados.");
                    }


                    return PartialView(saque);
                }
            }





            return PartialView(saque);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
