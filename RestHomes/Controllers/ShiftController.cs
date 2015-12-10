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
    public class ShiftController : Controller
    {
        private IDBRepository repository;
        public ShiftController(IDBRepository dbRep)
        {
            repository = dbRep;
        }
        public ActionResult List()
        {
            return View(repository.Shifts.Where(sh=>sh.isDeleted!=true));
        }
        public ViewResult Positions2Shift(int IDs, string returnUrl)
        {
            Shift shift = repository.GetShift(IDs);
            IEnumerable<Position> members = shift.Positions.ToArray();
            IEnumerable<Position> nonMembers = repository.Positions.Where(p => p.isDeleted != true).ToArray().Except(members).ToArray();
            return View(new ShiftEditModel { shift = shift, Members = members, NonMembers = nonMembers, ReturnUrl = returnUrl });
        }
        [HttpPost]
        public ActionResult Positions2Shift(ManyModificationModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                repository.AddPositionsToShift(model.modId, model.IdsToAdd);
                repository.RemPositionsFromShift(model.modId, model.IdsToDelete);
            }
            return Redirect(returnUrl);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateShiftModel model)
        {
            if (ModelState.IsValid)
            {
                Shift newShift = new Shift { Start = model.Start, End = model.End, Hours = model.Hours, isDeleted = false };
                repository.SaveShift(newShift);
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Delete(int posId, string returnUrl)
        {
            repository.DeleteShift(posId);
            return Redirect(returnUrl);
        }
	}
}