using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EEDDMS.WebSite.Models;

namespace EEDDMS.Tests
{


    /// <summary>
    ///这是 CollectorControllerTest 的测试类，旨在
    ///包含所有 CollectorControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class CollectorControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_Collectors()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            IQueryable<Collector> collectors = new Collector[]{
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"),  CollectorNo = "M1"},
                new Collector{ Id =  Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"),  CollectorNo = "M2"},
                new Collector{ Id =Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), CollectorNo = "M3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Collectors).Returns(collectors);

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Collector"]).Returns(new Collector());

            CollectorController target = new CollectorController(mock.Object);

            target.ControllerContext = mockContext.Object;

            CollectorListViewModel result = (CollectorListViewModel)target.Index().ViewData.Model;

            Assert.AreEqual(result.Collectors.Count(), 3);
            Assert.AreEqual(collectors.ToArray()[0].CollectorNo, result.Collectors.ToArray()[0].CollectorNo);
            Assert.AreEqual(collectors.ToArray()[1].CollectorNo, result.Collectors.ToArray()[1].CollectorNo);
            Assert.AreEqual(collectors.ToArray()[2].CollectorNo, result.Collectors.ToArray()[2].CollectorNo);
        }

        [TestMethod]
        public void Can_Show_Collector_Details()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            mock.Setup(m => m.Collectors).Returns(new Collector[]{
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1"},
                new Collector{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), CollectorNo = "M2"},
                new Collector{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), CollectorNo = "M3"}
            }.AsQueryable());

            CollectorController target = new CollectorController(mock.Object);

            Collector m1 = target.Details(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Collector;
            Collector m2 = target.Details(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as Collector;
            Collector m3 = target.Details(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as Collector;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Can_Edit_Collector()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            mock.Setup(m => m.Collectors).Returns(new Collector[]{
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1"}
            }.AsQueryable());

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Collector"]).Returns(
                new Collector { Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1" }
                );

            CollectorController target = new CollectorController(mock.Object);

            target.ControllerContext = mockContext.Object;


            Collector m1 = target.Edit(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Collector;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Collector()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            mock.Setup(m => m.Collectors).Returns(new Collector[]{
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1"},
                new Collector{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), CollectorNo = "M2"},
                new Collector{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), CollectorNo = "M3"}
            }.AsQueryable());

            CollectorController target = new CollectorController(mock.Object);

            Collector result = target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model as Collector;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();

            CollectorController target = new CollectorController(mock.Object);

            Collector item = new Collector
            {
                CollectorNo = "M1",
                EquipmentId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
            };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveCollector(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();

            CollectorController target = new CollectorController(mock.Object);

            Collector item = new Collector
            {
                CollectorNo = "M1",
                EquipmentId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
            };

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Collector"]).Returns(
                new Collector
                {
                    Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"),
                    CollectorNo = "M1",
                    EquipmentId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
                });

            target.ModelState.AddModelError("error", "error");

            target.ControllerContext = mockContext.Object;

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveCollector(It.IsAny<Collector>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Collectors()
        {
            Collector item = new Collector { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), CollectorNo = "Test" };

            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            mock.Setup(m => m.Collectors).Returns(new Collector[] {
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1"},
                item,
                new Collector{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), CollectorNo = "M3"}
            }.AsQueryable());

            CollectorController target = new CollectorController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteCollector(item));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_Collectors()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            mock.Setup(m => m.Collectors).Returns(new Collector[]{
                new Collector{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), CollectorNo = "M1"},
                new Collector{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), CollectorNo = "M2"},
                new Collector{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), CollectorNo = "M3"}
            }.AsQueryable());

            CollectorController target = new CollectorController(mock.Object);

            target.Delete(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826"));

            mock.Verify(m => m.DeleteCollector(It.IsAny<Collector>()), Times.Never());
        }

        [TestMethod]
        public void Can_Continue_Edit()
        {
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();

            CollectorController target = new CollectorController(mock.Object);

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Collector"]).Returns(new Collector { CollectorNo = "M1" });

            target.ControllerContext = mockContext.Object;

            Collector m1 = target.ContinueEdit().ViewData.Model as Collector;

            Assert.AreEqual("M1", m1.CollectorNo);
        }

        /// <summary>
        /// 查询参数为空
        /// </summary>
        [TestMethod]
        public void Query_Empty_Params_Collectors()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            IQueryable<Collector> collectors = new Collector[]{
                new Collector{ Id = g1, CollectorNo = "M1"},
                new Collector{ Id = g2, CollectorNo = "M2"},
                new Collector{ Id = g3, CollectorNo = "M3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Collectors).Returns(collectors);
            ;
            CollectorController target = new CollectorController(mock.Object);
            CollectorListViewModel result = ((CollectorListViewModel)target.QueryCollector("", "", "", "", "", "", "").Model);
            Assert.AreEqual(result.Collectors.Count(), 3);
            Assert.AreEqual("M3", result.Collectors.ToArray()[0].CollectorNo);
            Assert.AreEqual("M1", result.Collectors.ToArray()[1].CollectorNo);
            Assert.AreEqual("M2", result.Collectors.ToArray()[2].CollectorNo);
        }

        /// <summary>
        /// 查询参数不为空
        /// </summary>
        [TestMethod]
        public void Query_NotEmpty_Params_Collectors()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<ICollectorRepository> mock = new Mock<ICollectorRepository>();
            IQueryable<Collector> collectors = new Collector[]{
                new Collector{ Id = g1,  CollectorNo = "M1"},
                new Collector{ Id = g2,  CollectorNo = "M2"},
                new Collector{ Id =  g3, CollectorNo = "M3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Collectors).Returns(collectors);
            ;
            CollectorController target = new CollectorController(mock.Object);
            CollectorListViewModel result = ((CollectorListViewModel)target.QueryCollector("M2", "", "", "", "", "", "").Model);
            Assert.AreEqual(result.Collectors.Count(), 3);
            Assert.AreEqual("M2", result.Collectors.ToArray()[0].CollectorNo);

        }
    }
}
