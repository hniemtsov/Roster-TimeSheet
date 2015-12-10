using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestHomes.Domain.Abstract;
using RestHomes.Models;
using RestHomes.Domain.Entities;

namespace RestHomes.Controllers
{
    [Authorize]
    public class WorkerController : Controller
    {
        private IDBRepository repository;
        public WorkerController(IDBRepository dbRep)
        {
            repository = dbRep;
        }
        public ActionResult List()
        {
            return View(repository.Workers.Where(w=>w.isDeleted!=true));
        }

        public ViewResult Positions2Worker(int IDw, string returnUrl)
        {
            Worker worker = repository.GetWorker(IDw);
            IEnumerable<Position> members = worker.Positions.ToArray();
            IEnumerable<Position> nonMembers = repository.Positions.Where(p => p.isDeleted != true).ToArray().Except(members).ToArray();
            return View(new WorkerEditModel { worker = worker, Members = members, NonMembers = nonMembers, ReturnUrl = returnUrl });
        }
        [HttpPost]
        public RedirectResult Positions2Worker(ManyModificationModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                repository.AddPositionsToWorker(model.modId, model.IdsToAdd);
                repository.RemPositionsFromWorker(model.modId, model.IdsToDelete);
            }

            return Redirect(returnUrl);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateWorkerModel model)
        {
            if (ModelState.IsValid)
            {
                Worker newWorker = new Worker { FirstName = model.firstName, 
                                               SecondName = model.lastName, 
                                               NickName = model.nickName,
                                               SickHours = model.sickHours,
                                               LeaveHours = model.leaveHours,
                                               isDeleted = false };
                repository.SaveWorker(newWorker);
                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
	}
}