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
    public class PositionController : Controller
    {
        private IDBRepository repository;
        public PositionController(IDBRepository dbRep)
        {
            repository = dbRep;
        }
        public ActionResult ListWorkers()
        {
            return View(repository.Positions.Where(p=>p.isDeleted!=true).ToArray());
        }
        public ActionResult ListShifts()
        {
             return View(repository.Positions.Where(p=>p.isDeleted != true).ToArray());
        }
        [ChildActionOnly]
        public MvcHtmlString GetUserName(int id)
        {

            return new MvcHtmlString("dsd");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreatePositionModel model)
        {
            if (ModelState.IsValid)
            {
                Position newPos = new Position { Name = model.Name, Description = model.Description, isDeleted=false };
                repository.SavePosition(newPos);
                return RedirectToAction("ListWorkers");
            }
            else
            {
                return View(model);
            }
        }

        public ViewResult Workers2Position(int IDp, string returnUrl)
        {
            Position pos = repository.GetPosition(IDp);
            IEnumerable<Worker> members = pos.Workers.ToArray();//repository.Workers.Where(
            IEnumerable<Worker> nonMembers = repository.Workers.Where(w => w.isDeleted != true).ToArray().Except(members).ToArray();
            return View(new PositionEditModel { Pos = pos, Members = members, NonMembers = nonMembers, ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Workers2Position(ManyModificationModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                repository.AddWorkersToPosition(model.modId, model.IdsToAdd);
                repository.RemWorkersFromPosition(model.modId, model.IdsToDelete);
            }
            
            return Redirect(returnUrl);
        }
        public ViewResult Shifts2Position(int IDp, string returnUrl)
        {
            Position pos = repository.GetPosition(IDp);
            IEnumerable<Shift> members = pos.Shifts.ToArray();//repository.Workers.Where(
            IEnumerable<Shift> nonMembers = repository.Shifts.Where(sh => sh.isDeleted != true).ToArray().Except(members).ToArray();
            return View(new PositionShiftsEditModel { Pos = pos, Members = members, NonMembers = nonMembers, ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Shifts2Position(ManyModificationModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                repository.AddShiftsToPosition(model.modId, model.IdsToAdd);
                repository.RemShiftsFromPosition(model.modId, model.IdsToDelete);
            }
            return Redirect(returnUrl);

            //return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Delete(int posId, string returnUrl)
        {
            repository.DeletePosition(posId);
            return Redirect(returnUrl);
        }
	}
}