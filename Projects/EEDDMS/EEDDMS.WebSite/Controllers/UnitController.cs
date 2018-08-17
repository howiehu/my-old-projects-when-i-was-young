using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Models;

namespace EEDDMS.WebSite.Controllers
{
    public class UnitController : Controller
    {
        private IUnitRepository repository;

        public UnitController(IUnitRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index()
        {
            return View(new UnitTree(this.repository).UnitTreeNodeNodes);
        }

        public ViewResult Edit(Guid id)
        {
            Unit item = this.repository.Units.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Unit unit)
        {
            if (ModelState.IsValid)
            {
                this.repository.SaveUnit(unit);
                TempData["message"] = string.Format("{0} 保存成功", unit.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(unit);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Unit());
        }

        public ViewResult CreateChild(Guid? parentId)
        {
            return View("Edit", new Unit() { ParentId = parentId });
        }

        public ActionResult Delete(Guid id)
        {
            Unit item = this.repository.Units.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteUnit(item);
                TempData["message"] = string.Format("{0} 已被删除", item.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            Unit item = this.repository.Units.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        public ActionResult MoveUp(Guid id)
        {
            this.repository.MoveUpAndDownUnit(id, false);

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(Guid id)
        {
            this.repository.MoveUpAndDownUnit(id, true);

            return RedirectToAction("Index");
        }
    }
}
