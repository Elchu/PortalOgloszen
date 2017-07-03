using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repozytory.Models.Views
{
    public class OgloszeniaZKategoriiViewModels
    {
        public Kategoria Kategoria { get; set; }
        public List<Ogloszenie> Ogloszenia { get; set; }

    }
}