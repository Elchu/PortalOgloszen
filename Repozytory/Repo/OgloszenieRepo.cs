﻿using System;
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

        public IQueryable<Ogloszenie> PobierzOgloszenia()
        {
            return _db.Ogloszenia.Include(u => u.Uzytkownik);
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

        public IQueryable<Kategoria> PobierzKategorie()
        {
            return _db.Kategorie;
        }
    }
}