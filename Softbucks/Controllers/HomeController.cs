using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModul;
using Microsoft.AspNet.Identity;

namespace Softbucks.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public HomeController(IRepository repository)
        {
            Repository = repository;
        }

        public IRepository Repository { get; set; }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var dropTypeProduct = Repository.GetAllTypeProducts
                   .Select(c => new SelectListItem
                   {
                       Value = c.Id.ToString(),
                       Text = c.Name
                   }).ToList();
            dropTypeProduct.Add(new SelectListItem{Selected = true,Text = "Все продукты",Value = "-1"});
            ViewBag.DropTypeProduct = dropTypeProduct;
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult HeadMain()
        {
            var m = Repository.Db.Merchandises;

            return PartialView(m.OrderByDescending(p => p.Orders.Sum(o=>o.Count)).ToList());
        }

        [ChildActionOnly]
        public ActionResult AccordionMenu()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MyCofeAccordion()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ProductSearch(string name, int typeProduct, int prPage, int? page)
        {
            var pr = Repository.GetProductInType(typeProduct);
            if (name!="")
            {
                pr = pr.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            var pg=(page ?? 1);
            
            ViewBag.pageNumber = pg;
            ViewBag.pageSize = prPage;
            ViewBag.pageCount = pr.Count();

            pr = pr.Skip((pg - 1)*prPage);
            pr=pr.Take(prPage);

            return PartialView(pr);
        }

        
        public ActionResult ViewMyMerchandise(int? productId)
        {

            var pr = productId ?? -1;
            if (pr!=-1)
            {
                Repository.UserProductForMerchandiseAdd(User.Identity.GetUserId(), pr);
            }
            return PartialView(Repository.GetUserProductForMerchandise(this.User.Identity.GetUserId()));
        }
        
        public ActionResult MyMerchandiseKick(int productId)
        {
            Repository.UserProductForMerchandiseKick(User.Identity.GetUserId(), productId);
            return PartialView("ViewMyMerchandise",Repository.GetUserProductForMerchandise(this.User.Identity.GetUserId()));
        }
        
        public ActionResult MyMerchandiseKickAll()
        {
            Repository.UserProductForMerchandiseKickAll(User.Identity.GetUserId());
            return PartialView("ViewMyMerchandise", Repository.GetUserProductForMerchandise(this.User.Identity.GetUserId()));
        }


        public ActionResult AddViewMerchandiseForOrder(int? merchandiseId)
        {
            var mr = merchandiseId ?? -1;
            if (mr != -1)
            {
                Repository.AddMerchandiseForUserOrder(User.Identity.GetUserId(), mr);
            }
            ViewBag.m = merchandiseId;
            return PartialView(Repository.GetOrderUser(User.Identity.GetUserId()));
        }


        public ActionResult AddMyMerchandiseForUserOrder(string nameMerch)
        {
            Repository.AddMyMerchandiseForUserOrder(User.Identity.GetUserId(),nameMerch);
            return PartialView("AddViewMerchandiseForOrder", Repository.GetOrderUser(User.Identity.GetUserId()));
        }

        public ActionResult OrderMini()
        {
            var or = Repository.GetOrderUser(User.Identity.GetUserId());
            
            if (or!= null)
            {
                ViewBag.OrderCount = or.Sum(p => p.Count);
                ViewBag.OrderPrice =or.Sum(p => p.Count*p.Merchandises.MerchandiseProducts.Sum(s => s.Products.Price*s.CountProduct));
            }
            return PartialView();
        }
        
        public ActionResult OrderMerchAdd(int idOrder)
        {
            Repository.AddMerchandiseToOrder(idOrder);
            return PartialView("AddViewMerchandiseForOrder", Repository.GetOrderUser(User.Identity.GetUserId()));
        }

        public ActionResult OrderMerchKick(int idOrder)
        {
            Repository.KickMerchandiseToOrder(idOrder);
            return PartialView("AddViewMerchandiseForOrder", Repository.GetOrderUser(User.Identity.GetUserId()));
        }


        public ActionResult BuyOrder()
        {
            Repository.BuyOrderUser(User.Identity.GetUserId());
            return PartialView();
        }
    }
}