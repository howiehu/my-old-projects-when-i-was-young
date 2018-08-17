using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Models;
using EEDDMS.WebSite.Helpers;

namespace EEDDMS.WebSite.Controllers
{
    public class ManufacturerController : Controller
    {
        private IManufacturerRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public ManufacturerController(IManufacturerRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(int page = 1)
        {
         
            ManufacturerListViewModel viewModel = new ManufacturerListViewModel
            {
                Manufacturers = repository.Manufacturers
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Manufacturers.Count()
                }
            };
            return View(viewModel);
        }

        public ViewResult Edit(Guid id)
        {
            Manufacturer item = this.repository.Manufacturers.FirstOrDefault(i => i.Id == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                this.repository.SaveManufacturer(manufacturer);
                TempData["message"] = string.Format("{0} 保存成功", manufacturer.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(manufacturer);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Manufacturer());
        }

        public ActionResult Delete(Guid id)
        {
            Manufacturer item = this.repository.Manufacturers.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                this.repository.DeleteManufacturer(item);
                TempData["message"] = string.Format("{0} 已被删除", item.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Details(Guid id)
        {
            Manufacturer item = this.repository.Manufacturers.FirstOrDefault(i => i.Id == id);

            return View(item);
        }
        [HttpPost]
        public ViewResult QueryManufacturer(string name, int page = 1)
        {
            IQueryable<Manufacturer> source = this.repository.Manufacturers;
            //如果名称不为空
            if (!string.IsNullOrEmpty(name))
            {
                source = this.repository.Manufacturers.Where(p => p.Name.Contains(name));
            }
            ManufacturerListViewModel viewModel = new ManufacturerListViewModel
            {
                Manufacturers = source
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
