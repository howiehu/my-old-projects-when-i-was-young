using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Concrete;
using EEDDMS.Domain.Abstract;
using EEDDMS.WebSite.Models;
using EEDDMS.WebSite.Helpers;
using DevExpress.Web.Mvc;

namespace EEDDMS.WebSite.Controllers
{
    public class LocationController : Controller
    {
        //从配置文件中读取每页显示的默认条数
        int pageSize = MVCHelper.FromConfigQueryPageSize();
        private ILocationRepository repository;

        public LocationController(ILocationRepository repo)
        {
            this.repository = repo;
        }


        //
        // GET: /Location/

        public ViewResult Index(int page = 1)
        {
            //从配置文件中读取每页显示的默认条数
            int pageSize = MVCHelper.FromConfigQueryPageSize();
            LocationsListViewModel viewModel = new LocationsListViewModel
            {
                Locations = repository.Locations
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Locations.Count()
                }
            };
            return View(viewModel);

        }

        [HttpPost]
        public ViewResult Search(string queryString)
        {
            if (queryString.Split("\"".ToCharArray()).ToList().Count - 1 > 2)
            {
                TempData["message"] = "搜索字符中不能包含两个以上的引号";
                return View("Index", repository.Locations);
            }
            if (repository.QuickQuery(queryString) == null)
            {
                return View("Index", repository.Locations);
            }
            else
            {
                return View("Index", repository.QuickQuery(queryString));
            }
        }

        // GET: /Location/Details

        public ViewResult Details(Guid id)
        {
            Location location = repository.Locations.Where(p => p.Id == id).First();
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            var model = new Location();
            return PartialView("LocationForm", model);
            //  return View("Edit", new Location());
        }


        [OutputCache(Duration = 0)]
        public ActionResult List(int page = 1)
        {
            //从配置文件中读取每页显示的默认条数
            int pageSize = MVCHelper.FromConfigQueryPageSize();
            LocationsListViewModel viewModel = new LocationsListViewModel
            {
                Locations = repository.Locations
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Locations.Count()
                }
            };

            return PartialView(viewModel);
        }


        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                repository.SaveLocation(location);
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Location/Edit/5

        public ViewResult Edit(Guid id)
        {
            Location location = repository.Locations.FirstOrDefault(p => p.Id == id);

            return View(location);

        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                repository.SaveLocation(location);
                return RedirectToAction("Index");
            }
            return View(location);
        }


        public ActionResult Delete(Guid id)
        {

            Location item = this.repository.Locations.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                this.repository.DeleteLocation(item);
                TempData["message"] = string.Format("{0} 已被删除", item.Name);
            }

            return RedirectToAction("Index");


        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewResult QueryLocation(string name, int page = 1)
        {

            IQueryable<Location> source = null;
            if (!string.IsNullOrEmpty(name))
            {
                source = repository.Locations
                 .Where(p => p.Name.Contains(name))
                 .OrderBy(p => p.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize);
            }
            else
            {
                source = repository.Locations
               .OrderBy(p => p.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
            }
            LocationsListViewModel viewModel = new LocationsListViewModel
            {
                Locations = source,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Locations.Where(p => p.Name.Contains(name)).Count()
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



        //////////////////////////
        public ActionResult InlineEditing()
        {
            return View("InlineEditing", repository.Locations.ToList());
        }


        [ValidateInput(false)]
        public ActionResult InlineEditingPartial()
        {
            return PartialView("InlineEditingPartial", repository.Locations.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult InlineEditingAddNewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] Location location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repository.SaveLocation(location);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("InlineEditingPartial", repository.Locations.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult InlineEditingUpdatePartial([ModelBinder(typeof(DevExpressEditorsBinder))]Location location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repository.SaveLocation(location);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("InlineEditingPartial", repository.Locations.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult InlineEditingDeletePartial(Guid? Id)
        {
            if (Id != null)
            {
                try
                {
                    repository.DeleteLocation(repository.Locations.Where(p => p.Id == Id).FirstOrDefault());
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("InlineEditingPartial", repository.Locations.ToList());
        }
    }

}