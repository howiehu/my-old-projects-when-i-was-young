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
    public class SelectEquipmentDetailController : Controller
    {
        private IEquipmentDetailRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public SelectEquipmentDetailController(IEquipmentDetailRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(int page = 1)
        {

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

        public ActionResult Select(Guid id)
        {
            Equipment equipment = (Equipment)Session["Equipment"];

            equipment.EquipmentDetailId = id;
            equipment.EquipmentDetail = this.repository.EquipmentDetails.FirstOrDefault(m => m.Id == id);

            return RedirectToAction("ContinueEdit", "Equipment");
        }
    }
}
