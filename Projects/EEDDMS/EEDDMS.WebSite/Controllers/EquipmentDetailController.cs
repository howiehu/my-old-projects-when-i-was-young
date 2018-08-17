using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using System.Data.Entity;
using EEDDMS.WebSite.Models;
using EEDDMS.WebSite.Helpers;

namespace EEDDMS.WebSite.Controllers
{
    public class EquipmentDetailController : Controller
    {
        private IEquipmentDetailRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public EquipmentDetailController(IEquipmentDetailRepository repo)
        {
            this.repository = repo;
        }

 
        public ViewResult Index(int page = 1)
        {

            Session.Remove("EquipmentDetail");
            EquipmentDetailListViewModel viewModel = new EquipmentDetailListViewModel
            {
                EquipmentDetails = repository.EquipmentDetails
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.EquipmentDetails.Count()
                }
            };
            viewModel.EquipmentDetails.AsQueryable().Include(p => p.EquipmentClass).Include(p => p.Manufacturer).Load();
            return View(viewModel);
        }

        public ViewResult Edit(Guid id)
        {
            EquipmentDetail item = this.repository.EquipmentDetails.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                Session["EquipmentDetail"] = item;

                EquipmentDetail sessionItem = (EquipmentDetail)Session["EquipmentDetail"];

                return View(sessionItem);
            }
            else
            {
                return View(item);
            }
        }

        public ViewResult ContinueEdit()
        {
            EquipmentDetail equipmentDetail = (EquipmentDetail)Session["EquipmentDetail"];

            return View("Edit", equipmentDetail);
        }

        [HttpPost]
        public ActionResult Edit(EquipmentDetail equipmentDetail)
        {

            if (equipmentDetail.ManufacturerId == Guid.Empty)
            {
                ModelState.AddModelError("ManufacturerId", "请选择设备制造商");
            }
            if (equipmentDetail.EquipmentClassId == Guid.Empty)
            {
                ModelState.AddModelError("EquipmentClassId", "请选择设备分类");
            }

            if (ModelState.IsValid)
            {
                this.repository.SaveEquipmentDetail(equipmentDetail);
                TempData["message"] = string.Format("{0} 保存成功", equipmentDetail.Name);

                return RedirectToAction("Index");
            }
            else
            {
                EquipmentDetail sessionItem = (EquipmentDetail)Session["EquipmentDetail"];

                equipmentDetail.Manufacturer = sessionItem.Manufacturer;
                equipmentDetail.EquipmentClass = sessionItem.EquipmentClass;

                return View(equipmentDetail);
            }
        }

        public ViewResult Create()
        {
            EquipmentDetail item = new EquipmentDetail();

            Session["EquipmentDetail"] = item;

            return View("Edit", item);
        }

        public ActionResult Delete(Guid id)
        {
            EquipmentDetail item = this.repository.EquipmentDetails.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteEquipmentDetail(item);
                TempData["message"] = string.Format("{0} 已被删除", item.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            EquipmentDetail item = this.repository.EquipmentDetails.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        public ActionResult SelectManufacturer()
        {
            return RedirectToAction("Index", "SelectManufacturer");
        }

        public ActionResult SelectEquipmentClass()
        {
            return RedirectToAction("Index", "SelectEquipmentClass");
        }
        [HttpPost]
        public ViewResult QueryEquipmentDetail(string name, string type, string designLife, string manufacturerName, string equipmentClassName, int page = 1)
        {
            IQueryable<EquipmentDetail> source = this.repository.EquipmentDetails;
            //Session.Remove("EquipmentDetail");
            //如果名称不为空
            if (!string.IsNullOrEmpty(name))
            {
                source = this.repository.EquipmentDetails.Where(p => p.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(type))
            {
                source = this.repository.EquipmentDetails.Where(p => p.Type.Contains(type));
            }
            if (!string.IsNullOrEmpty(designLife))
            {
                int intDesignLife = 0;
                int.TryParse(designLife, out intDesignLife);
                if (intDesignLife != 0)
                {
                    source = this.repository.EquipmentDetails.Where(p => p.DesignLife == intDesignLife);
                }
            }
            if (!string.IsNullOrEmpty(manufacturerName))
            {
                source = this.repository.EquipmentDetails.Where(p => p.Manufacturer.Name.Contains(manufacturerName));
            }
            if (!string.IsNullOrEmpty(equipmentClassName))
            {

                source = this.repository.EquipmentDetails.Where(p => p.EquipmentClass.Name.Contains(type));
            }
            source = source.Include(e => e.Manufacturer).Include(e => e.EquipmentClass);

            EquipmentDetailListViewModel viewModel = new EquipmentDetailListViewModel
            {
                EquipmentDetails = source
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
            viewModel.EquipmentDetails.AsQueryable().Include(p => p.EquipmentClass).Include(p => p.Manufacturer).Load();

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
