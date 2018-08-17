using System;
using System.Collections.Generic;
using System.Linq;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using EEDDMS.WebSite.Models;
namespace EEDDMS.Tests
{
    /// <summary>
    ///这是 LocationControllerTest 的测试类，旨在
    ///包含所有 LocationControllerTest 单元测试
    [TestClass]
    public class LocationControllerTest
    {

        /// <summary>
        /// 显示所有地域信息列表
        /// </summary>
        [TestMethod]
        public void Index_Contains_All_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            IQueryable<Location> locations = new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Locations).Returns(locations);
            LocationController target = new LocationController(mock.Object);
            LocationsListViewModel result = ((LocationsListViewModel)target.Index().Model);
            Assert.AreEqual(result.Locations.Count(), 3);
            Assert.AreEqual(locations.ToArray()[0].Name, result.Locations.ToArray()[0].Name);
            Assert.AreEqual(locations.ToArray()[1].Name, result.Locations.ToArray()[1].Name);
            Assert.AreEqual(locations.ToArray()[2].Name, result.Locations.ToArray()[2].Name);
        }

        /// <summary>
        /// 显示地域信息详细信息
        /// </summary>
        [TestMethod]
        public void Can_Show_Locations_Details()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            mock.Setup(m => m.Locations).Returns(new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable());
            ;
            LocationController target = new LocationController(mock.Object);
            Location L1 = target.Details(g1).ViewData.Model as Location;
            Location L2 = target.Details(g2).ViewData.Model as Location;
            Location L3 = target.Details(g3).ViewData.Model as Location;
            Assert.AreEqual(g1, L1.Id);
            Assert.AreEqual(g2, L2.Id);
            Assert.AreEqual(g3, L3.Id);
        }


        /// <summary>
        /// 编辑地域信息
        /// </summary>
        [TestMethod]
        public void Can_Edit_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            mock.Setup(m => m.Locations).Returns(new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable());
            ;
            LocationController target = new LocationController(mock.Object);

            Location l1 = target.Edit(g1).ViewData.Model as Location;
            Location l2 = target.Edit(g2).ViewData.Model as Location;
            Location l3 = target.Edit(g3).ViewData.Model as Location;

            Assert.AreEqual(g1, l1.Id);
            Assert.AreEqual(g2, l2.Id);
            Assert.AreEqual(g3, l3.Id);
        }

        /// <summary>
        /// 不能编辑不存在的地域信息
        /// </summary>
        [TestMethod]
        public void Cannot_Edit_Nonexistent_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            mock.Setup(m => m.Locations).Returns(new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable());
            LocationController target = new LocationController(mock.Object);

            Manufacturer result = (Manufacturer)target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model;

            Assert.IsNull(result);
        }


        /// <summary>
        /// 可以保存
        /// </summary>
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();

            LocationController target = new LocationController(mock.Object);

            Location item = new Location { Name = "L1" };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveLocation(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();

            LocationController target = new LocationController(mock.Object);

            Location item = new Location { Name = "L1" };

            target.ModelState.AddModelError("error", "error");

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveLocation(It.IsAny<Location>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        /// <summary>
        /// 可以正常删除 
        /// </summary>
        [TestMethod]
        public void Can_Delete_Valid_Locations()
        {
            Location item = new Location { Id = Guid.NewGuid(), Name = "Test" };

            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            mock.Setup(m => m.Locations).Returns(new Location[] {
                new Location{ Id = Guid.NewGuid(), Name = "L1"},
                item,
                new Location{ Id =  Guid.NewGuid(), Name = "L3"}
            }.AsQueryable());

            LocationController target = new LocationController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteLocation(item));
        }

        /// <summary>
        /// 不能删除不存在的地域信息
        /// </summary>
        [TestMethod]
        public void Cannot_Delete_Invalid_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            mock.Setup(m => m.Locations).Returns(new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable());
            LocationController target = new LocationController(mock.Object);
            target.Delete(Guid.Parse("50ad0522-20ba-4134-8dd1-18c70023733e"));
            mock.Verify(m => m.DeleteLocation(It.IsAny<Location>()), Times.Never());
        }

        /// <summary>
        /// 查询参数为空
        /// </summary>
        [TestMethod]
        public void Query_Empty_Params_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            IQueryable<Location> locations = new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Locations).Returns(locations);
            ;
            LocationController target = new LocationController(mock.Object);
            LocationsListViewModel result = ((LocationsListViewModel)target.QueryLocation("").Model);
            Assert.AreEqual(result.Locations.Count(), 3);
            Assert.AreEqual("L3", result.Locations.ToArray()[0].Name);
            Assert.AreEqual("L1", result.Locations.ToArray()[1].Name);
            Assert.AreEqual("L2", result.Locations.ToArray()[2].Name);
        }

        /// <summary>
        /// 查询参数不为空
        /// </summary>
        [TestMethod]
        public void Query_NotEmpty_Params_Locations()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ILocationRepository> mock = new Mock<ILocationRepository>();
            IQueryable<Location> locations = new Location[]{
                new Location{ Id = g1, Name = "L1"},
                new Location{ Id = g2, Name = "L2"},
                new Location{ Id =  g3, Name = "L3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Locations).Returns(locations);
            ;
            LocationController target = new LocationController(mock.Object);
            LocationsListViewModel result = ((LocationsListViewModel)target.QueryLocation("L2").Model);
            Assert.AreEqual(result.Locations.Count(), 3);
            Assert.AreEqual("L2", result.Locations.ToArray()[0].Name);
        }
    }
}
