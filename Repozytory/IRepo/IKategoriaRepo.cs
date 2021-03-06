﻿using System.Collections.Generic;
using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IKategoriaRepo
    {
        IEnumerable<Kategoria> PobierzKategorie();
        IEnumerable<Ogloszenie> PobierzOgloszeniaZKategorii(int idKategorii);
        Kategoria GetKategoriaById(int idKategorii);
        void SaveChanges();
    }
}