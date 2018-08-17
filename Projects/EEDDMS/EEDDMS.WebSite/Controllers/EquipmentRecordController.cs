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
    public class EquipmentRecordController : Controller
    {
        private IEquipmentRecordRepository repository;

        public EquipmentRecordController(IEquipmentRecordRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult RecordsByEquipment(Guid id)
        {
            ViewData["EquipmentId"] = id;
            return View(this.repository.EquipmentRecords.Where(e => e.EquipmentId == id).OrderBy(e => e.StartDate).Include(e => e.Equipment).Include(e => e.Location).Include(e => e.Unit));
        }
    }
}
