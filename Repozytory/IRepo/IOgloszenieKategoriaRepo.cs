using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszenieKategoriaRepo
    {
        OgloszenieKategoria PobierzOgloszenieKategoria(int idKategorii, int idOgloszenia);
        void Aktualizuj(OgloszenieKategoria ogloszenieKategoria);
        void SaveChanges();
    }
}