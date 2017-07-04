using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytory.IRepo;
using Repozytory.Models;
using Repozytory.Models.Views;


namespace PortalOgloszen.Controllers
{
    public class KategorieController : Controller
    {
        private readonly IKategoriaRepo _repo;
        private readonly IOgloszenieRepo _repoOgloszenie;
        private readonly IOgloszenieKategoriaRepo _repoOgloszenieKategoria;

        public KategorieController(IKategoriaRepo repo, IOgloszenieRepo repoOgloszenie, IOgloszenieKategoriaRepo repoOgloszenieKategoria)
        {
            _repo = repo;
            _repoOgloszenie = repoOgloszenie;
            _repoOgloszenieKategoria = repoOgloszenieKategoria;
        }

        public ActionResult Index()
        {
            return View(_repo.PobierzKategorie());
        }

        public ActionResult PokazOgloszenia(int? KategoriaId)
        {

            if (KategoriaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OgloszeniaZKategoriiViewModels ogloszeniaZKategoriiViewModels = new OgloszeniaZKategoriiViewModels()
            {
                Kategoria = _repo.GetKategoriaById((int) KategoriaId),
                Ogloszenia = _repo.PobierzOgloszeniaZKategorii((int) KategoriaId).ToList()
            };

            return View(ogloszeniaZKategoriiViewModels);
        }

        [Authorize]
        public ActionResult EdytujKategorieOgloszenia(int idOgloszenia, int idKategorii)
        {

            if (idOgloszenia < 0 || idKategorii < 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ogloszenie = _repoOgloszenie.GetOgloszenieById(idOgloszenia);
            var kategorie = _repo.PobierzKategorie();

            OgloszenieKategoriaViewModels ogloszenieKategoria = new OgloszenieKategoriaViewModels()
            {
                Ogloszenie = ogloszenie,
                Kategoria = kategorie,
                IdKategoriiDoEdycji = idKategorii
            };

            return View(ogloszenieKategoria);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EdytujKategorieOgloszenia(OgloszenieKategoriaViewModels ogloszenieKategoriaViewModel, int noweIdKategorii, int IdKategoriiDoEdycji)
        {
            if (noweIdKategorii < 0)
                ModelState.AddModelError("noweIdKategorii", "Musisz wybrać kategorię");

            if (ModelState.IsValid)
            {
                try
                {
                    _repoOgloszenie.Aktualizuj(ogloszenieKategoriaViewModel.Ogloszenie);
                    _repoOgloszenie.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Wystąpił błąd podczas aktualizowania ogłoszenie. Spróbuj ponownie.";
                    return View();
                }

                if (noweIdKategorii != IdKategoriiDoEdycji)
                {
                    try
                    {
                        OgloszenieKategoria ogloszenieKategoria = _repoOgloszenieKategoria.PobierzOgloszenieKategoria(ogloszenieKategoriaViewModel.IdKategoriiDoEdycji, ogloszenieKategoriaViewModel.Ogloszenie.OgloszenieId);
                        ogloszenieKategoria.OgloszenieId = ogloszenieKategoriaViewModel.Ogloszenie.OgloszenieId;
                        ogloszenieKategoria.KategoriaId = noweIdKategorii;
                        _repoOgloszenieKategoria.Aktualizuj(ogloszenieKategoria);
                        _repoOgloszenieKategoria.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Wystąpił błąd podczas aktualizowania kategorii. Spróbuj ponownie.";
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", "Kategorie");
        }
    }
}
