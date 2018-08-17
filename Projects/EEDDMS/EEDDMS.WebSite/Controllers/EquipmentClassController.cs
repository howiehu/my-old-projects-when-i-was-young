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
    public class EquipmentClassController : Controller
    {
        private IEquipmentClassRepository repository;

        public EquipmentClassController(IEquipmentClassRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index()
        {
            return View(new EquipmentClassTree(this.repository).EquipmentClassTreeNodes);
        }

        public ViewResult Edit(Guid id)
        {
            EquipmentClass item = this.repository.EquipmentClasses.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(EquipmentClass equipmentClass)
        {
            if (ModelState.IsValid)
            {
                this.repository.SaveEquipmentClass(equipmentClass);
                TempData["message"] = string.Format("{0} 保存成功", equipmentClass.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(equipmentClass);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new EquipmentClass());
        }

        public ViewResult CreateChild(Guid? parentId)
        {
            return View("Edit", new EquipmentClass() { ParentId = parentId });
        }

        public ActionResult Delete(Guid id)
        {
            EquipmentClass item = this.repository.EquipmentClasses.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteEquipmentClass(item);
                TempData["message"] = string.Format("{0} 已被删除", item.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            EquipmentClass item = this.repository.EquipmentClasses.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        public ActionResult MoveUp(Guid id)
        {
            this.repository.MoveUpAndDownEquipmentClass(id, false);

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(Guid id)
        {
            this.repository.MoveUpAndDownEquipmentClass(id, true);

            return RedirectToAction("Index");
        }
    }
}
