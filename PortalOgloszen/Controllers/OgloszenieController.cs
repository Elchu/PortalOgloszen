﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Repozytory.IRepo;
using Repozytory.Models;
using PagedList;
using Repozytory.Models.Views;

namespace PortalOgloszen.Controllers
{
    public class OgloszenieController : Controller
    {
        private readonly IOgloszenieRepo _repoOgloszenie;
        private readonly IKategoriaRepo _repoKategoria;


        public OgloszenieController(IOgloszenieRepo repoOgloszenie, IKategoriaRepo repoKategoria)
        {
            _repoOgloszenie = repoOgloszenie;
            _repoKategoria = repoKategoria;
        }

        public ActionResult Index(int? page, string orderSort = "DataDodaniaDesc")
        {
            int aktualnaStrona = page ?? 1;
            int naStronie = 5;

            //potrzebne do stronicowania, zeby zachowac aktualne sortowanie
            ViewBag.CurrentSort = orderSort;
            ViewBag.DataSortowaniaRosnaco = orderSort == "DataDodaniaDesc" ? "DataDodaniaAsc" : "DataDodaniaDesc";

            var ogloszenia = _repoOgloszenie.PobierzOgloszenia(orderSort);
            return View(ogloszenia.ToPagedList<Ogloszenie>(aktualnaStrona, naStronie));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repoOgloszenie.GetOgloszenieById((int) id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            return View(ogloszenie);
        }

        public ActionResult Create()
        {
            ViewBag.KategoriaId = new SelectList(_repoOgloszenie.PobierzKategorie(), "KategoriaId", "Nazwa");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie, int? KategoriaId)
        {
            if (!KategoriaId.HasValue)
                ModelState.AddModelError("KategoriaId", "Musisz wybrać kategorię");

            if (ModelState.IsValid)
            {
                ogloszenie.UzytkownikId = User.Identity.GetUserId();
                ogloszenie.DataDodania = DateTime.Now;
                try
                {
                    _repoOgloszenie.DodajOgloszenie(ogloszenie);
                    _repoOgloszenie.SaveChanges();
                    _repoOgloszenie.DodajOgloszenieDoKategorii(ogloszenie.OgloszenieId, (int)KategoriaId);
                    _repoOgloszenie.SaveChanges();
                    return RedirectToAction("MojeOgloszenia");
                }
                catch (Exception)
                {
                    return View(ogloszenie);
                }
            }
            ViewBag.KategoriaId = new SelectList(_repoOgloszenie.PobierzKategorie(), "KategoriaId", "Nazwa");
            return View(ogloszenie);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repoOgloszenie.GetOgloszenieById((int) id);

            if (ogloszenie == null)
            {
                return HttpNotFound();
            }

            if (ogloszenie.UzytkownikId != User.Identity.GetUserId() && !(User.IsInRole("Admin") || User.IsInRole("Pracownik")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(ogloszenie);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OgloszenieId,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie, int? KategoriaId)
        {
            if (!KategoriaId.HasValue)
                ModelState.AddModelError("KategoriaId", "Musisz wybrać kategorię");

            if (ModelState.IsValid)
            {
                try
                {
                    _repoOgloszenie.Aktualizuj(ogloszenie);
                    _repoOgloszenie.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = true;
                    return View(ogloszenie);
                }

                ViewBag.Error = false;
                return View(ogloszenie);
            }

            return View(ogloszenie);
        }

        [Authorize]
        public ActionResult Delete(int? id, bool? error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ogloszenie ogloszenie = _repoOgloszenie.GetOgloszenieById((int) id);

            if (ogloszenie == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() != ogloszenie.UzytkownikId || !(User.IsInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (error != null)
                ViewBag.Error = true;

            return View(ogloszenie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repoOgloszenie.UsunOgloszenie(id);
            try
            {
                _repoOgloszenie.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new {id = id, error = true});
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MojeOgloszenia(int? page, string orderSort)
        {
            int aktualnaStrona = page ?? 1;
            int naStronie = 5;
            string userId = User.Identity.GetUserId();

            if (String.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.CurrentSort = orderSort;
            ViewBag.DataSortowaniaRosnaco = orderSort == "DataDodaniaDesc" ? "DataDodaniaAsc" : "DataDodaniaDesc";

            var listaOgloszenUzytkownika = _repoOgloszenie.PobierzOgloszeniaUzytkownikaPoId(userId, orderSort);

            return View(listaOgloszenUzytkownika.ToPagedList<Ogloszenie>(aktualnaStrona, naStronie));
        }

        public ActionResult SzczegolyOgloszenia(int idOgloszenia, int idKategorii)
        {
            if (idOgloszenia < 0 || idKategorii < 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ogloszenie = _repoOgloszenie.GetOgloszenieById(idOgloszenia);
            var kategorie = _repoKategoria.PobierzKategorie();

            OgloszenieKategoriaViewModels ogloszenieKategoria = new OgloszenieKategoriaViewModels()
            {
                Ogloszenie = ogloszenie,
                Kategoria = kategorie,
                IdKategoriiDoEdycji = idKategorii
            };

            return View(ogloszenieKategoria);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
