using System.Linq;
using CaixaEletronico.Models;

namespace CaixaEletronico.Data
{
    public class DbInitializer
    {

        public static void Initialize(DBContext db)
        {
            db.Database.EnsureCreated();

            if (db.Notas.Any())
            {
                return;   // Ja existe o banco
            }

            var notas = new Notas[]
            {
                new Notas{Valor=2,Quantidade=8,Imagem="../lib/images/cedula2.jpg"},
                new Notas{Valor=5,Quantidade=7,Imagem="../lib/images/cedula5.jpg"},
                new Notas{Valor=10,Quantidade=6,Imagem="../lib/images/cedula10.jpg"},
                new Notas{Valor=20,Quantidade=5,Imagem="../lib/images/cedula20.jpg"},
                new Notas{Valor=50,Quantidade=4,Imagem="../lib/images/cedula50.jpg"},
                new Notas{Valor=100,Quantidade=3,Imagem="../lib/images/cedula100.jpg"},
                new Notas{Valor=200,Quantidade=2,Imagem="../lib/images/cedula200.jpg"},
            };
            foreach (Notas n in notas)
            {
                db.Notas.Add(n);
            }
            db.SaveChanges();
        }
    }
}
