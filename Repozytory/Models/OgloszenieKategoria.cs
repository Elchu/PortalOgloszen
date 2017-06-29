using System.ComponentModel.DataAnnotations;

namespace Repozytory.Models
{
    public class OgloszenieKategoria
    {
        [Key]
        public int OgloszenieKategoriaId { get; set; }

        public int KategoriaId { get; set; }

        public int OgloszenieId { get; set; }

        public virtual Kategoria Kategoria { get; set; }

        public virtual Ogloszenie Ogloszenie { get; set; }
    }
}