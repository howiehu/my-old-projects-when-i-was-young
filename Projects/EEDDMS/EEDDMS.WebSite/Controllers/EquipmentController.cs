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
    public class EquipmentController : Controller
    {

        private IEquipmentRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public EquipmentController(IEquipmentRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(int page = 1)
        {
        
            Session.Remove("Equipment");
            EquipmentListViewModel viewModel = new EquipmentListViewModel
            {
                Equipments = repository.Equipments
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Equipments.Count()
                }
            };
            viewModel.Equipments.AsQueryable().Include(p => p.EquipmentDetail).Load();
            return View(viewModel);
        }

        public ViewResult Edit(Guid id)
        {
            Equipment item = this.repository.Equipments.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                Session["Equipment"] = item;

                Equipment sessionItem = (Equipment)Session["Equipment"];

                return View(sessionItem);
            }
            else
            {
                return View(item);
            }
        }

        public ViewResult ContinueEdit()
        {
            Equipment equipment = (Equipment)Session["Equipment"];

            return View("Edit", equipment);
        }

        [HttpPost]
        public ActionResult Edit(Equipment equipment)
        {

            if (equipment.EquipmentDetailId == Guid.Empty)
            {
                ModelState.AddModelError("EquipmentDetailId", "请选择设备信息");
            }

            if (ModelState.IsValid)
            {
                this.repository.SaveEquipment(equipment);
                TempData["message"] = string.Format("{0} 保存成功", equipment.EquipmentNo);

                return RedirectToAction("Index");
            }
            else
            {
                Equipment sessionItem = (Equipment)Session["Equipment"];

                equipment.EquipmentDetail = sessionItem.EquipmentDetail;

                return View(equipment);
            }
        }

        public ViewResult Create()
        {
            Equipment item = new Equipment();

            Session["Equipment"] = item;

            return View("Edit", item);
        }

        public ActionResult Delete(Guid id)
        {
            Equipment item = this.repository.Equipments.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteEquipment(item);
                TempData["message"] = string.Format("{0} 已被删除", item.EquipmentNo);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            Equipment item = this.repository.Equipments.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        public ActionResult SelectEquipmentDetail()
        {
            return RedirectToAction("Index", "SelectEquipmentDetail");
        }


        [HttpPost]
        public ViewResult QueryEquipment(string equipmentNo, string equipmentDetailName, string equipmentDetailType, string startproductionDate, string endproductionDate, string startToUseDate, string endstartToUseDate, string state, string health, int page = 1)
        {
            IQueryable<Equipment> source = this.repository.Equipments.Include(p => p.EquipmentDetail);



            if (!string.IsNullOrEmpty(equipmentNo))
            {
                source = source.Where(p => p.EquipmentNo.Contains(equipmentNo));
            }
            if (!string.IsNullOrEmpty(equipmentDetailName))
            {
                source = source.Where(p => p.EquipmentDetail.Name.Contains(equipmentDetailName));
            }
            if (!string.IsNullOrEmpty(equipmentDetailType))
            {
                source = source.Where(p => p.EquipmentDetail.Type.Contains(equipmentDetailType));
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
                    ModelState.AddModelError("productionDate", "请输入正确的日期格式");
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
                    ModelState.AddModelError("productionDate", "请输入正确的日期格式");
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
            if (!string.IsNullOrEmpty(state) && state != "3")
            {
                int intstate = 0;
                int.TryParse(state, out intstate);
                source = source.Where(p => p.State == intstate);
            }

            if (!string.IsNullOrEmpty(health))
            {
                source = source.Where(p => p.Health.Contains(health));
            }

            EquipmentListViewModel viewModel = new EquipmentListViewModel
            {
                Equipments = source
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
            viewModel.Equipments.AsQueryable().Include(p => p.EquipmentDetail).Load();

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
