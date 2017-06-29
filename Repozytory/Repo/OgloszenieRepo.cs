using System;
using System.Linq;
using Repozytory.IRepo;
using Repozytory.Models;
using System.Data.Entity;

namespace Repozytory.Repo
{
    public class OgloszenieRepo : IOgloszenieRepo
    {
        private readonly IOgloszeniaContext _db;
        
        public OgloszenieRepo(IOgloszeniaContext db)
        {
            _db = db;
        }

        public IQueryable<Ogloszenie> PobierzOgloszenia()
        {
            return _db.Ogloszenia.Include(u => u.Uzytkownik);
        }
    }
}