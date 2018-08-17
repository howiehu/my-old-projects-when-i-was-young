using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using EEDDMS.WebSite.Models;

namespace EEDDMS.WebSite.Controllers
{
    public class SelectEquipmentClassController : Controller
    {
        private IEquipmentClassRepository repository;

        public SelectEquipmentClassController(IEquipmentClassRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index()
        {
            return View(new EquipmentClassTree(this.repository).EquipmentClassTreeNodes);
        }

        public ActionResult Select(Guid id)
        {
            EquipmentDetail equipmentDetail = (EquipmentDetail)Session["EquipmentDetail"];

            equipmentDetail.EquipmentClassId = id;
            equipmentDetail.EquipmentClass = this.repository.EquipmentClasses.FirstOrDefault(c => c.Id == id);

            return RedirectToAction("ContinueEdit", "EquipmentDetail");
        }
    }
}
