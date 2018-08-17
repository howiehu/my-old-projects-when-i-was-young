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
    ///这是 EquipmentControllerTest 的测试类，旨在
    ///包含所有 EquipmentControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class EquipmentControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_Equipments()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            IQueryable<Equipment> equipments = new Equipment[]{
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"},
                new Equipment{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), EquipmentNo = "M2"},
                new Equipment{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), EquipmentNo = "M3"}
            }.AsQueryable().OrderBy(p => p.Id);
            mock.Setup(m => m.Equipments).Returns(equipments);

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Equipment"]).Returns(new Equipment());

            EquipmentController target = new EquipmentController(mock.Object);

            target.ControllerContext = mockContext.Object;

            EquipmentListViewModel result = (EquipmentListViewModel)target.Index().ViewData.Model;

            Assert.AreEqual(result.Equipments.Count(), 3);
            Assert.AreEqual(equipments.ToArray()[0].EquipmentNo, result.Equipments.ToArray()[0].EquipmentNo);
            Assert.AreEqual(equipments.ToArray()[1].EquipmentNo, result.Equipments.ToArray()[1].EquipmentNo);
            Assert.AreEqual(equipments.ToArray()[2].EquipmentNo, result.Equipments.ToArray()[2].EquipmentNo);
        }

        [TestMethod]
        public void Can_Show_Equipment_Details()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            mock.Setup(m => m.Equipments).Returns(new Equipment[]{
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"},
                new Equipment{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), EquipmentNo = "M2"},
                new Equipment{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), EquipmentNo = "M3"}
            }.AsQueryable());

            EquipmentController target = new EquipmentController(mock.Object);

            Equipment m1 = target.Details(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Equipment;
            Equipment m2 = target.Details(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as Equipment;
            Equipment m3 = target.Details(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as Equipment;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Can_Edit_Equipment()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            mock.Setup(m => m.Equipments).Returns(new Equipment[]{
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"}
            }.AsQueryable());

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Equipment"]).Returns(
                new Equipment { Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1" }
                );

            EquipmentController target = new EquipmentController(mock.Object);

            target.ControllerContext = mockContext.Object;


            Equipment m1 = target.Edit(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Equipment;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Equipment()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            mock.Setup(m => m.Equipments).Returns(new Equipment[]{
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"},
                new Equipment{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), EquipmentNo = "M2"},
                new Equipment{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), EquipmentNo = "M3"}
            }.AsQueryable());

            EquipmentController target = new EquipmentController(mock.Object);

            Equipment result = target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model as Equipment;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();

            EquipmentController target = new EquipmentController(mock.Object);

            Equipment item = new Equipment
            {
                EquipmentNo = "M1",
                EquipmentDetailId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
            };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipment(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();

            EquipmentController target = new EquipmentController(mock.Object);

            Equipment item = new Equipment
            {
                EquipmentNo = "M1",
                EquipmentDetailId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
            };

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Equipment"]).Returns(
                new Equipment
                {
                    Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"),
                    EquipmentNo = "M1",
                    EquipmentDetailId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")
                });

            target.ModelState.AddModelError("error", "error");

            target.ControllerContext = mockContext.Object;

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipment(It.IsAny<Equipment>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Equipments()
        {
            Equipment item = new Equipment { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), EquipmentNo = "Test" };

            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            mock.Setup(m => m.Equipments).Returns(new Equipment[] {
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"},
                item,
                new Equipment{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), EquipmentNo = "M3"}
            }.AsQueryable());

            EquipmentController target = new EquipmentController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteEquipment(item));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_Equipments()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            mock.Setup(m => m.Equipments).Returns(new Equipment[]{
                new Equipment{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), EquipmentNo = "M1"},
                new Equipment{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), EquipmentNo = "M2"},
                new Equipment{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), EquipmentNo = "M3"}
            }.AsQueryable());

            EquipmentController target = new EquipmentController(mock.Object);

            target.Delete(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826"));

            mock.Verify(m => m.DeleteEquipment(It.IsAny<Equipment>()), Times.Never());
        }

        [TestMethod]
        public void Can_Continue_Edit()
        {
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();

            EquipmentController target = new EquipmentController(mock.Object);

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["Equipment"]).Returns(new Equipment { EquipmentNo = "M1" });

            target.ControllerContext = mockContext.Object;

            Equipment m1 = target.ContinueEdit().ViewData.Model as Equipment;

            Assert.AreEqual("M1", m1.EquipmentNo);
        }

        [TestMethod]
        public void Query_Empty_Params_Equipments()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            IQueryable<Equipment> equipments = new Equipment[]{
                new  Equipment{ Id = g1,  EquipmentNo = "E1"},
                new  Equipment{ Id = g2, EquipmentNo = "E2"},
                new  Equipment{ Id = g3, EquipmentNo = "E3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Equipments).Returns(equipments);

            EquipmentController target = new EquipmentController(mock.Object);
            EquipmentListViewModel result = ((EquipmentListViewModel)target.QueryEquipment("", "", "", "", "", "", "", "", "").Model);
            Assert.AreEqual(result.Equipments.Count(), 3);
            Assert.AreEqual("E3", result.Equipments.ToArray()[0].EquipmentNo);
            Assert.AreEqual("E1", result.Equipments.ToArray()[1].EquipmentNo);
            Assert.AreEqual("E2", result.Equipments.ToArray()[2].EquipmentNo);
        }

        [TestMethod]
        public void Query_NotEmpty_Params_Equipments()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<IEquipmentRepository> mock = new Mock<IEquipmentRepository>();
            IQueryable<Equipment> equipments = new Equipment[]{
                new  Equipment{ Id = g1,  EquipmentNo = "E1"},
                new  Equipment{ Id = g2, EquipmentNo = "E2"},
                new  Equipment{ Id = g3, EquipmentNo = "E3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.Equipments).Returns(equipments);
            EquipmentController target = new EquipmentController(mock.Object);
            EquipmentListViewModel result = ((EquipmentListViewModel)target.QueryEquipment("E2", "", "", "", "", "", "", "", "").Model);
            Assert.AreEqual(result.Equipments.Count(), 1);
            Assert.AreEqual("E2", result.Equipments.ToArray()[0].EquipmentNo);
        }
    }
}
