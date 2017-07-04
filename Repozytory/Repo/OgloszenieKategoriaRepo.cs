using System;
using System.Data.Entity;
using Repozytory.IRepo;
using Repozytory.Models;
using System.Linq;

namespace Repozytory.Repo
{
    public class OgloszenieKategoriaRepo : IOgloszenieKategoriaRepo
    {
        private readonly IPortalOgloszenContext _db; 

        public OgloszenieKategoriaRepo(IPortalOgloszenContext db)
        {
            _db = db;
        }

        public void Aktualizuj(OgloszenieKategoria ogloszenieKategoria)
        {
            _db.Entry(ogloszenieKategoria).State = EntityState.Modified;
        }

        public OgloszenieKategoria PobierzOgloszenieKategoria(int idKategorii, int idOgloszenia)
        {
            return _db.OgloszenieKategoria.First(k => k.KategoriaId == idKategorii && k.OgloszenieId == idOgloszenia);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}