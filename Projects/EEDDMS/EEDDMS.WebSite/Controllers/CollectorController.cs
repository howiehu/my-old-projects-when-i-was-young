using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using System.Data.Entity;
using EEDDMS.WebSite.Helpers;
using EEDDMS.WebSite.Models;

namespace EEDDMS.WebSite.Controllers
{
    public class CollectorController : Controller
    {
        private ICollectorRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public CollectorController(ICollectorRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(int page = 1)
        {

            Session.Remove("Collector");
            CollectorListViewModel viewModel = new CollectorListViewModel
            {
                Collectors = repository.Collectors
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Collectors.Count()
                }
            };
            viewModel.Collectors.AsQueryable().Include(p => p.Equipment).Load();
            return View(viewModel);
        }

        public ViewResult Edit(Guid id)
        {
            Collector item = this.repository.Collectors.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                Session["Collector"] = item;

                Collector sessionItem = (Collector)Session["Collector"];

                return View(sessionItem);
            }
            else
            {
                return View(item);
            }
        }

        public ViewResult ContinueEdit()
        {
            Collector collector = (Collector)Session["Collector"];

            return View("Edit", collector);
        }

        [HttpPost]
        public ActionResult Edit(Collector collector)
        {

            //if (collector.EquipmentId == Guid.Empty)
            //{
            //    ModelState.AddModelError("EquipmentDetailId", "请选择设备信息");
            //}

            if (ModelState.IsValid)
            {
                this.repository.SaveCollector(collector);
                TempData["message"] = string.Format("{0} 保存成功", collector.CollectorNo);

                return RedirectToAction("Index");
            }
            else
            {
                Collector sessionItem = (Collector)Session["Collector"];

                collector.Equipment = sessionItem.Equipment;

                return View(collector);
            }
        }

        public ViewResult Create()
        {
            Collector item = new Collector();

            Session["Collector"] = item;

            return View("Edit", item);
        }

        public ActionResult Delete(Guid id)
        {
            Collector item = this.repository.Collectors.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteCollector(item);
                TempData["message"] = string.Format("{0} 已被删除", item.CollectorNo);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            Collector item = this.repository.Collectors.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        public ActionResult SelectEquipment()
        {
            return RedirectToAction("Index", "SelectEquipment");
        }

        public ActionResult RemoveEquipment()
        {
            Collector sessionItem = (Collector)Session["Collector"];

            sessionItem.EquipmentId = null;
            sessionItem.Equipment = null;

            return RedirectToAction("ContinueEdit");
        }


        [HttpPost]
        public ViewResult QueryCollector(string collectorNo, string state, string startproductionDate, string endproductionDate, string startToUseDate, string endstartToUseDate, string EquipmentNo, int page = 1)
        {
            var source = this.repository.Collectors.Include(p => p.Equipment);
            if (!string.IsNullOrEmpty(collectorNo))
            {
                source = source.Where(p => p.CollectorNo.Contains(collectorNo));
            }
            if (!string.IsNullOrEmpty(state) && state != "3")
            {
                int intstate = 0;
                int.TryParse(state, out intstate);
                source = source.Where(p => p.State == intstate);
            }
            if (!string.IsNullOrEmpty(startproductionDate))
            {

                if (MVCHelper.Regex(startproductionDate))
                {
                    DateTime date = DateTime.Parse(startproductionDate);
                    source = source.Where(p => p.ProductionDate >= date);
                }
                else
                {
                    ModelState.AddModelError("startproductionDate", "请输入正确的日期格式");
                }
            }
            if (!string.IsNullOrEmpty(endproductionDate))
            {

                if (MVCHelper.Regex(endproductionDate))
                {
                    DateTime date = DateTime.Parse(endproductionDate);
                    source = source.Where(p => p.ProductionDate <= date);
                }
                else
                {
                    ModelState.AddModelError("endproductionDate", "请输入正确的日期格式");
                }
            }
            if (!string.IsNullOrEmpty(startToUseDate))
            {

                if (MVCHelper.Regex(startToUseDate))
                {
                    DateTime date = DateTime.Parse(startToUseDate);
                    source = source.Where(p => p.StartToUseDate >= date);
                }
                else
                {
                    ModelState.AddModelError("startToUseDate", "请输入正确的日期格式");
                }
            }
            if (!string.IsNullOrEmpty(endstartToUseDate))
            {

                if (MVCHelper.Regex(endstartToUseDate))
                {
                    DateTime date = DateTime.Parse(endstartToUseDate);
                    source = source.Where(p => p.StartToUseDate <= date);
                }
                else
                {
                    ModelState.AddModelError("startToUseDate", "请输入正确的日期格式");
                }
            }
            if (!string.IsNullOrEmpty(EquipmentNo))
            {
                source = source.Where(p => p.Equipment.EquipmentNo.Contains(EquipmentNo));
            }

            CollectorListViewModel viewModel = new CollectorListViewModel
            {
                Collectors = source
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = source.Count()
                }
            };
            viewModel.Collectors.AsQueryable().Include(p => p.Equipment).Load();
            if (ModelState.IsValid)
            {
                TempData["cache"] = viewModel;
                return View("Index", viewModel);
            }
            if (TempData["cache"] == null)
            {
                TempData["cache"] = viewModel;
            }
            return View("Index", TempData["cache"]);

        }
    }
}
