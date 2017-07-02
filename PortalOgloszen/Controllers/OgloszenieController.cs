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

namespace PortalOgloszen.Controllers
{
    public class OgloszenieController : Controller
    {
        private readonly IOgloszenieRepo _repo;

        public OgloszenieController(IOgloszenieRepo repo)
        {
            _repo = repo;

        }

        public ActionResult Index(int? page)
        {
            int aktualnaStrona = page ?? 1;
            int naStronie = 5;
            var ogloszenia = _repo.PobierzOgloszenia();
            return View(ogloszenia.ToPagedList<Ogloszenie>(aktualnaStrona, naStronie));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int) id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            return View(ogloszenie);
        }

        public ActionResult Create()
        {
            ViewBag.KategoriaId = new SelectList(_repo.PobierzKategorie(), "KategoriaId", "Nazwa");
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
                    _repo.DodajOgloszenie(ogloszenie);
                    _repo.SaveChanges();
                    _repo.DodajOgloszenieDoKategorii(ogloszenie.OgloszenieId, (int)KategoriaId);
                    _repo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(ogloszenie);
                }
            }
            ViewBag.KategoriaId = new SelectList(_repo.PobierzKategorie(), "KategoriaId", "Nazwa");
            return View(ogloszenie);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int) id);

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
        public ActionResult Edit([Bind(Include = "OgloszenieId,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Aktualizuj(ogloszenie);
                    _repo.SaveChanges();
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

            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int) id);

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
            _repo.UsunOgloszenie(id);
            try
            {
                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new {id = id, error = true});
            }

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
