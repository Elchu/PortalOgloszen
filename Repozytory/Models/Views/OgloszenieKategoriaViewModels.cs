using System.Collections.Generic;

namespace Repozytory.Models.Views
{
    public class OgloszenieKategoriaViewModels
    {
        public int IdKategoriiDoEdycji { get; set; }
        public IEnumerable<Kategoria> Kategoria { get; set; }
        public Ogloszenie Ogloszenie { get; set; }
    }
}