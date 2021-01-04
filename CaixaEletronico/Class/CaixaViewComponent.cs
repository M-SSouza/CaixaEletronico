using CaixaEletronico.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CaixaEletronico.Class
{

    public class CaixaViewComponent : ViewComponent
    {
        private readonly DBContext db;

        public CaixaViewComponent(DBContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            return View("Caixa", db.Notas.ToList());
        }
    }
}
