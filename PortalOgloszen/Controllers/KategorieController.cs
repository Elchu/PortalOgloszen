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

        public KategorieController(IKategoriaRepo repo)
        {
            _repo = repo;
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
                Kategoria = _repo.GetKategoriaById((int)KategoriaId),
                Ogloszenia = _repo.PobierzOgloszeniaZKategorii((int)KategoriaId).ToList()
            };

            return View(ogloszeniaZKategoriiViewModels);
        }


    }
}
