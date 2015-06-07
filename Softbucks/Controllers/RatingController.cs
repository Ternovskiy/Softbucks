using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModul;
using Microsoft.AspNet.Identity;

namespace Softbucks.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        public RatingController(IRepository repository)
        {
            Repository = repository;
        }

        public IRepository Repository { get; set; }

        public ActionResult GetOrdered(int? page)
        {
            int pageSize = 10;
            var or = Repository.GetOrderedUser(User.Identity.GetUserId());
            ViewBag.pageNumber = page??1;
            ViewBag.pageSize = pageSize;
            ViewBag.pageCount = or.Count();
            int pr = page ?? 1 ;
            or = or.Skip(pageSize*(pr-1)).Take(pageSize);
            return PartialView(or);
        }

        public ActionResult SetRating(int idMerch)
        {
            var r = Repository.GetRatingsForUser(User.Identity.GetUserId(), idMerch);
            if (r == null)
            {
                Repository.SetRating(User.Identity.GetUserId(), idMerch, 0, "");
                r = Repository.GetRatingsForUser(User.Identity.GetUserId(), idMerch);
            }
                
            return PartialView(r);
        }

        public ActionResult GetComments(int idMerch)
        {
            return PartialView(Repository.GetCommentForMerch(idMerch));
        }

        public ActionResult SetRatingValue(int idMerch,int value, string comment)
        {
            Repository.SetRating(User.Identity.GetUserId(), idMerch, value, comment);

            return PartialView("GetComments", Repository.GetCommentForMerch(idMerch));
        }
    }
}