using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestHomes.Domain.Abstract;
using RestHomes.Domain.Entities;
using RestHomes.Models;

namespace RestHomes.Controllers
{
    [Authorize]
    public class TimeSheetController : Controller
    {
        private IDBRepository repository;
        public TimeSheetController(IDBRepository dbRep)
        {
            repository = dbRep;
        }
        public ViewResult List()
        {
            DateTime dt = GetDate();//new DateTime(2015, 9, 2);
            List<DateTime> days = new List<DateTime>();
            for (int i = 0; i < 14; i++)
            {
                days.Add(dt.AddDays(i));
            }
            DateTime dtS = days[0];
            DateTime dtE = days[13];

            ViewBag.AllowEdit = HttpContext.User.IsInRole("Administrators");

            var allW = repository.Workers.Where(w => w.isDeleted != true)
                .Select(w => new Workers2Hrs
                {
                    worker = w,
                    hours2days = days.Select(dday => w.TimeSheets.OrderBy(ts => ts.Day)
                        .Where(ts => ts.Day == dday && ts.Hours.HasValue)
                        .Select(ts => new HrsKey { key = ts.Position.Name, hrs = ts.Hours.Value }))
                }).ToArray();
            //int allC = allWorkers.Count();
            var allW2Grp = allW.Select(hh => new Workers2HrsGr
            {
                worker = hh.worker,
                hours2days = hh.hours2days.Select(t => t.Select(t2 =>
                {
                    string key2 = "";
                    if (t2.key.Contains("Leave"))
                    {
                        key2 = t2.key[0].ToString().ToLower();
                    }
                    return new HrsKey { key = key2, hrs = t2.hrs };
                }).GroupBy(r => r.key))
            }
            ).ToArray();            
            /*
            TimeSheetModel allWorkers = new TimeSheetModel{
                workers = repository.Workers.Where(w => w.isDeleted != true)
                   .Select(w => new Workers2Hours
                   {
                       worker = w,
                       hours2days = days.Select(dday => w.TimeSheets.OrderBy(ts => ts.Day)
                           .Where(ts => ts.Day == dday && ts.Hours.HasValue).Select(ts => ts.Hours).Sum() ?? 0.0),
                       signed = !w.TimeSheets.Where(ts => ts.Day >= dtS && ts.Day <= dtE).Any(ts => ts.isSigned != true),
                       
                   }),
                days = days
            };
            */

            return View(new TimeSheetModel { workers = allW2Grp, days = days });
        }
        [HttpPost]
        public ActionResult List(DateTime dtFirst)
        {
            DateTime dt =dtFirst;//new DateTime(2015, 9, 2);
            List<DateTime> days = new List<DateTime>();
            for (int i = 0; i < 14; i++)
            {
                days.Add(dt.AddDays(i));
            }
            DateTime dtS = days[0];
            DateTime dtE = days[13];

            return RedirectToAction("List");
        }

        [HttpPost]
        public RedirectToRouteResult SetDate(DateTime startdate)
        {
            if (ModelState.IsValid)
            {
                Session["UserDateTime"] = startdate;
            }
            return RedirectToAction("List");
        }

        private DateTime GetDate()
        {
            var dateTime = Session["UserDateTime"];
            if (dateTime == null)
            {
                DateTime dt = DateTime.Today;
                while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                Session["UserDateTime"] = dt;
                return dt;
            }
            return (DateTime)dateTime;
        }
	}
}