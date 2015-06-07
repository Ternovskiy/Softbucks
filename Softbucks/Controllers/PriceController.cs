using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModul;

namespace Softbucks.Controllers
{
[Authorize]
    public class PriceController : Controller
    {
        public PriceController(IRepository repository)
        {
            Repository = repository;
        }

        public IRepository Repository { get; set; }
        


        public ActionResult Main()
        {
            return PartialView();
        }

        public ActionResult SearchPrice(string name, int pageSize, int? page)
        {
            int countP=0;
            var p = Repository.GetPrice(name, pageSize, page ?? 1,ref countP);
            var pg = (page ?? 1);

            ViewBag.pageNumber = pg;
            ViewBag.pageSize = pageSize;
            ViewBag.pageCount = countP;
            return PartialView(p);
        }

        public ActionResult DetalsMerchandise(int idMerch)
        {
            var r = new Random();
            ViewBag.img = @"/Content/img/"+r.Next(1,5)+".jpg";
            return PartialView(Repository.GerMerchandise(idMerch));
        }

        [AllowAnonymous]
        public ActionResult DetalsMerchandiseModal(int idMerch)
        {
            var r = new Random();
            ViewBag.img = @"/Content/img/" + r.Next(1, 5) + ".jpg";
            return PartialView(Repository.GerMerchandise(idMerch));
        }
    }
}