using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repozytory.Models
{
    public class Ogloszenie
    {
        [Display(Name = "Id:")] // using System.ComponentModel.DataAnnotations;
        public int OgloszenieId { get; set; }

        [Display(Name = "Treść ogłoszenia:")]
        [MaxLength(500)]
        public string Tresc { get; set; }

        [Display(Name = "Tytuł ogłoszenia:")]
        [MaxLength(72)]
        public string Tytul { get; set; }

        [Display(Name = "Data dodania:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DataDodania { get; set; }

        public string UzytkownikId { get; set; }

        public virtual ICollection<OgloszenieKategoria> OgloszenieKategoria { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }

    }
}