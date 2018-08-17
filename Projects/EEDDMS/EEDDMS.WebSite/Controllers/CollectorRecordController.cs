using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using System.Data.Entity;
using EEDDMS.WebSite.Helpers;

namespace EEDDMS.WebSite.Controllers
{
    public class CollectorRecordController : Controller
    {
        private ICollectorRecordRepository repository;

        public CollectorRecordController(ICollectorRecordRepository repo)
        {
            this.repository = repo;
        }

        //public ViewResult Index(Guid id)
        //{
        //    return View(this.repository.CollectorRecords.Where(c => c.CollectorId == id).Include(c => c.Collector).Include(c => c.Equipment));
        //}

        public ViewResult RecordsByCollector(Guid id)
        {
            ViewData["CollectorId"] = id;
            return View(this.repository.CollectorRecords.Where(c => c.CollectorId == id).OrderBy(c => c.StartDate).Include(c => c.Collector).Include(c => c.Equipment));
        }
    }
}
