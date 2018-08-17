using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using EEDDMS.WebSite.Models;
using EEDDMS.WebSite.Helpers;

namespace EEDDMS.WebSite.Controllers
{
    public class SelectManufacturerController : Controller
    {
        private IManufacturerRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public SelectManufacturerController(IManufacturerRepository repo)
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

        public ActionResult Select(Guid id)
        {
            EquipmentDetail equipmentDetail = (EquipmentDetail)Session["EquipmentDetail"];

            equipmentDetail.ManufacturerId = id;
            equipmentDetail.Manufacturer = this.repository.Manufacturers.FirstOrDefault(m => m.Id == id);

            return RedirectToAction("ContinueEdit", "EquipmentDetail");
        }
    }
}
