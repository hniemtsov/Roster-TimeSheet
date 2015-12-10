using RestHomes.Domain.Abstract;
using RestHomes.Domain.Entities;
using RestHomes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Data.Entity;

namespace RestHomes.Controllers
{
    [Authorize]
    public class RosterController : Controller
    {
        private IDBRepository repository;
        public RosterController(IDBRepository dbRep)
        {
            repository = dbRep;
        }

        public ViewResult List()
        {
            DateTime dt = GetDate();//new DateTime(2015, 11, 30);
            List<DateTime> days = new List<DateTime>();    
            for (int i = 0; i < 14; i++)
            {
                days.Add(dt.AddDays(i));
            }
            DateTime dtS = days[0];
            DateTime dtE = days[13];

            //string mystr = days.ElementAt(0).ToString("MM/dd/yyyy");
            ViewBag.AllowEdit = HttpContext.User.IsInRole("Administrators");
            //repository.Workers.Select(w=>new {w.}
            RosterModel model = new RosterModel
            {
                positions = repository.Positions
                        .Union(repository.TimeSheets
                                  .Where(ts => ((ts.Day <= dtE) &&
                                                (ts.Day >= dtS) &&                                      // if signed than it doesn't matter
                                                ((ts.Position.isDeleted == false) || (ts.isSigned == true)))) // whether it was deleted or not
                                  .Select(ts => ts.Position))
                        .Select(p => new Position2Shifts
                        {
                            position = p,
                            shifts = p.Shifts
                            .Union(repository.TimeSheets
                                        .Where(ts => ( ((ts.Day <= dtE) &&
                                                        (ts.Day >= dtS) && 
                                                        (ts.IDp == p.IDp)) &&
                                                        ((ts.Shift.isDeleted == false) || (ts.isSigned == true))))
                                        .Select(ts => ts.Shift))
                            .Select(sh => new Shift2Days
                            {
                                shift = sh,
                                worker2days = days.Select(dday => repository.TimeSheets.OrderBy(ts=>ts.Day)
                                                     .Where(ts => ((ts.Day == dday) && (ts.IDs == sh.IDs) && (ts.IDp == p.IDp)))
                                                     .Select(ts => ts.Worker))
                            })
                        }),
                days = days
            };
            return View(model);
        }

        [HttpPost]
        //public RedirectToRouteResult List(IList<WorkersByDaysForShiftId> shifts)
        public RedirectToRouteResult List(RosterWorkersByDaysForShiftId shifts)
        {
            DateTime dt = shifts.dtFirst;
            foreach (var shift in shifts.wbdfs)
            {
                //IList<int> df = new List<int>();
                IList<List<int>> fortNight = new List<List<int>>();
                fortNight.Add(shift.day_0);  fortNight.Add(shift.day_1);
                fortNight.Add(shift.day_2);  fortNight.Add(shift.day_3);
                fortNight.Add(shift.day_4);  fortNight.Add(shift.day_5);
                fortNight.Add(shift.day_6);  fortNight.Add(shift.day_7);
                fortNight.Add(shift.day_8);  fortNight.Add(shift.day_9);
                fortNight.Add(shift.day_10); fortNight.Add(shift.day_11);
                fortNight.Add(shift.day_12); fortNight.Add(shift.day_13);
                repository.SaveToTS(shift.posId, shift.shiftId, fortNight, dt);             
            }

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
                DateTime dt = DateTime.Today;//.Now;
                while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                Session["UserDateTime"] = dt;
                return dt;
            }
            return (DateTime)dateTime;
        }
       
/*
        public ViewResult List()
        {
            RosterShiftListViewModel model = new RosterShiftListViewModel
            {
                RShifts = from b in repository.Shifts
                          group b by b.Position,
                firstDay = new DateTime(2015,9,7)
            };
            return View(model);
        }

        [ChildActionOnly]
        public string Time(DateTime day, int positionID, int shiftID, int kidx, int iidx)
        {
            var query2 = (from worker in repository.Workers
                          from position in worker.Positions
                          where position.IDp == positionID
                          select worker).ToArray();

            var query3 = (from ts in repository.TimeSheets
                          where ts.Day == day && ts.IDs == shiftID
                          select new { workerName = ts.Worker.NickName, workerID = ts.Worker.IDw }).ToArray();
            
            StringBuilder selectStr = new StringBuilder();
            
            selectStr.Append(@"<select id=""fave"" name=""[" + kidx+"]."+"workerID["+iidx+@"]"">");
            if (query3.Count() == 0)
            {
                selectStr.Append(@"<option value="""" selected label=""empty""></option>");
                foreach (var item in query2)
                {
                    selectStr.Append(@"<option value=""" + item.NickName + @""" label=""" + item.NickName + @""">" + item.NickName + @"</option>");
                }
            }
            else
            {
                selectStr.Append(@"<option value="""" label=""empty""></option>");
                foreach (var item in query2)
                {
                    if (item.IDw == query3[0].workerID)
                    {
                        selectStr.Append(@"<option value=""" + item.IDw + @""" selected label=""" + item.NickName + @""">" + item.NickName + @"</option>");
                    }
                    else
                    {
                        selectStr.Append(@"<option value=""" + item.IDw + @""" label=""" + item.NickName + @""">" + item.NickName + @"</option>");
                    }
                }
            }
            selectStr.Append(@"</select>");
            return selectStr.ToString();
        }
 */
    }
}
