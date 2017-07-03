using System;
using System.Collections.Generic;
using System.Linq;
using Repozytory.IRepo;
using Repozytory.Models;
using System.Data.Entity;

namespace Repozytory.Repo
{
    public class OgloszenieRepo : IOgloszenieRepo
    {
        private readonly IPortalOgloszenContext _db;
        
        public OgloszenieRepo(IPortalOgloszenContext db)
        {
            _db = db;
        }

        public IEnumerable<Ogloszenie> PobierzOgloszenia(string orderSort)
        {
            var ogloszenia = _db.Ogloszenia.Include(u => u.Uzytkownik);
            switch (orderSort)
            {
                case "DataDodaniaDesc":
                    ogloszenia = ogloszenia.OrderByDescending(d => d.DataDodania);
                    break;
                default:
                    ogloszenia = ogloszenia.OrderBy(d => d.DataDodania);
                    break;
            }

            return ogloszenia.ToList();
        }

        public Ogloszenie GetOgloszenieById(int id)
        {
            return _db.Ogloszenia.Find(id);
        }

        public void DodajOgloszenie(Ogloszenie ogloszenie)
        {
            _db.Ogloszenia.Add(ogloszenie);
        }

        public void UsunOgloszenie(int id)
        {
            UsunOgloszeniaZKategoriiOgloszen(id);
            Ogloszenie ogloszenie = GetOgloszenieById(id);
            _db.Ogloszenia.Remove(ogloszenie);
        }

        public void Aktualizuj(Ogloszenie ogloszenie)
        {
            _db.Entry(ogloszenie).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        private void UsunOgloszeniaZKategoriiOgloszen(int idOgloszenia)
        {
            IQueryable<OgloszenieKategoria> listaOgloszen = _db.OgloszenieKategoria.Where(o => o.OgloszenieId == idOgloszenia);

            foreach (OgloszenieKategoria item in listaOgloszen)
            {
                _db.OgloszenieKategoria.Remove(item);
            }
        }

        public void DodajOgloszenieDoKategorii(int ogloszenieId, int kategoriaId)
        {
            OgloszenieKategoria ogloszenieKategoria = new OgloszenieKategoria()
            {
                OgloszenieId = ogloszenieId,
                KategoriaId = kategoriaId
            };

            _db.OgloszenieKategoria.Add(ogloszenieKategoria);
        }

        public IEnumerable<Kategoria> PobierzKategorie()
        {
            return _db.Kategorie.AsNoTracking().ToList();
        }

        public IEnumerable<Ogloszenie> PobierzOgloszeniaUzytkownikaPoId(string userId, string orderSort)
        {
            return PobierzOgloszenia(orderSort).Where(u => u.UzytkownikId == userId).ToList();
        }
    }
}