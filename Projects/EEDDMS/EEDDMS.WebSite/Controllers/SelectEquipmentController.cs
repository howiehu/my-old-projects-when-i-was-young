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
    public class SelectEquipmentController : Controller
    {
        private IEquipmentRepository repository;
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        public SelectEquipmentController(IEquipmentRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(int page = 1)
        {
     
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

        public ActionResult Select(Guid id)
        {
            Collector collector = (Collector)Session["Collector"];

            collector.EquipmentId = id;
            collector.Equipment = this.repository.Equipments.FirstOrDefault(m => m.Id == id);

            return RedirectToAction("ContinueEdit", "Collector");
        }
    }
}
