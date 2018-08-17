using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        private List<Map> GetMaps()
        {
            List<Map> maps = new List<Map>();
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "挖掘机",
                Longitude = decimal.Parse(118.56 + ""),
                Latitude = decimal.Parse(40.23 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "推土机",
                Longitude = decimal.Parse(118.46 + ""),
                Latitude = decimal.Parse(40.13 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "起重机",
                Longitude = decimal.Parse(118.36 + ""),
                Latitude = decimal.Parse(40.03 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "塔机",
                Longitude = decimal.Parse(118.26 + ""),
                Latitude = decimal.Parse(40.33 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "装载机",
                Longitude = decimal.Parse(118.66 + ""),
                Latitude = decimal.Parse(40.43 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "平地机",
                Longitude = decimal.Parse(118.76 + ""),
                Latitude = decimal.Parse(40.53 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "自卸卡车",
                Longitude = decimal.Parse(118.86 + ""),
                Latitude = decimal.Parse(40.63 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });
            maps.Add(new Map
            {
                Id = Guid.NewGuid(),
                Name = "铲运机",
                Longitude = decimal.Parse(116.183771 + ""),
                Latitude = decimal.Parse(39.778 + ""),
                PictureHeight = 120,
                PictureWidth = 120
            });

            return maps;
        }

        public JsonResult GetEquipmentsByName(string equipmentName)
        {
            List<Map> maps = GetMaps();
            if (!string.IsNullOrEmpty(equipmentName))
            {
                var result = from m in maps
                             where m.Name.Contains(equipmentName)
                             select m.Name;

                return Json(result);
            }
            var rst = maps.Select(p => p.Name);
            return Json(rst);
        }

        public JsonResult GetEquipmentByName(string equipmentName)
        {
            List<Map> maps = GetMaps();
            if (!string.IsNullOrEmpty(equipmentName))
            {
                var result = maps.Where(p => p.Name == equipmentName);
                return Json(result);
            }
            return null;
        }
    }
}
