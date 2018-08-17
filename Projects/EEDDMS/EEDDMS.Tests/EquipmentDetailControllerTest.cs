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
    ///这是 EquipmentDetailControllerTest 的测试类，旨在
    ///包含所有 EquipmentDetailControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class EquipmentDetailControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_EquipmentDetails()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[]{
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new EquipmentDetail{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["EquipmentDetail"]).Returns(new EquipmentDetail());

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            target.ControllerContext = mockContext.Object;

            EquipmentDetailListViewModel result = (EquipmentDetailListViewModel)target.Index().ViewData.Model;

            Assert.AreEqual(result.EquipmentDetails.Count(), 3);
            Assert.AreEqual("M2", result.EquipmentDetails.ToArray()[0].Name);
            Assert.AreEqual("M3", result.EquipmentDetails.ToArray()[1].Name);
            Assert.AreEqual("M1", result.EquipmentDetails.ToArray()[2].Name);
        }

        [TestMethod]
        public void Can_Show_EquipmentDetail_Details()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[]{
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new EquipmentDetail{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            EquipmentDetail m1 = target.Details(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as EquipmentDetail;
            EquipmentDetail m2 = target.Details(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as EquipmentDetail;
            EquipmentDetail m3 = target.Details(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as EquipmentDetail;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Can_Edit_EquipmentDetail()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[]{
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                //new EquipmentDetail{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                //new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["EquipmentDetail"]).Returns(
                new EquipmentDetail { Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1" }
                );

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            //controllerContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(_testEmail);
            //controllerContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            //controllerContext.SetupGet(p => p.HttpContext.Response.Cookies).Returns(new HttpCookieCollection());

            //controllerContext.Setup(p => p.HttpContext.Request.Form.Get("ReturnUrl")).Returns("sample-return-url");
            //controllerContext.Setup(p => p.HttpContext.Request.Params.Get("q")).Returns("sample-search-term");

            target.ControllerContext = mockContext.Object;


            EquipmentDetail m1 = target.Edit(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as EquipmentDetail;
            //EquipmentDetail m2 = target.Edit(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as EquipmentDetail;
            //EquipmentDetail m3 = target.Edit(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as EquipmentDetail;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            //Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            //Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_EquipmentDetail()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[]{
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new EquipmentDetail{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            EquipmentDetail result = target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model as EquipmentDetail;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            EquipmentDetail item = new EquipmentDetail
            {
                Name = "M1",
                EquipmentClassId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"),
                ManufacturerId = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")
            };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipmentDetail(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            EquipmentDetail item = new EquipmentDetail
            {
                Name = "M1",
                EquipmentClassId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"),
                ManufacturerId = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")
            };

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["EquipmentDetail"]).Returns(
                new EquipmentDetail
                {
                    Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"),
                    Name = "M1",
                    EquipmentClassId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"),
                    ManufacturerId = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")
                });

            target.ModelState.AddModelError("error", "error");

            target.ControllerContext = mockContext.Object;

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipmentDetail(It.IsAny<EquipmentDetail>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_EquipmentDetails()
        {
            EquipmentDetail item = new EquipmentDetail { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "Test" };

            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[] {
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                item,
                new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteEquipmentDetail(item));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_EquipmentDetails()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();

            mock.Setup(m => m.EquipmentDetails).Returns(new EquipmentDetail[]{
                new EquipmentDetail{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new EquipmentDetail{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new EquipmentDetail{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            target.Delete(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826"));

            mock.Verify(m => m.DeleteEquipmentDetail(It.IsAny<EquipmentDetail>()), Times.Never());
        }

        [TestMethod]
        public void Can_Continue_Edit()
        {
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);

            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Session["EquipmentDetail"]).Returns(new EquipmentDetail { Name = "M1" });

            target.ControllerContext = mockContext.Object;

            EquipmentDetail m1 = target.ContinueEdit().ViewData.Model as EquipmentDetail;

            Assert.AreEqual("M1", m1.Name);
        }
        [TestMethod]
        public void Query_Empty_Params_EquipmentDetails()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            IQueryable<EquipmentDetail> equipmentDetails = new EquipmentDetail[]{
                new  EquipmentDetail{ Id = g1,  Name = "E1"},
                new  EquipmentDetail{ Id = g2, Name = "E2"},
                new  EquipmentDetail{ Id = g3, Name = "E3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.EquipmentDetails).Returns(equipmentDetails);

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);
            EquipmentDetailListViewModel result = ((EquipmentDetailListViewModel)target.QueryEquipmentDetail("", "", "", "", "").Model);
            Assert.AreEqual(result.EquipmentDetails.Count(), 3);
            Assert.AreEqual("E3", result.EquipmentDetails.ToArray()[0].Name);
            Assert.AreEqual("E1", result.EquipmentDetails.ToArray()[1].Name);
            Assert.AreEqual("E2", result.EquipmentDetails.ToArray()[2].Name);
        }
        [TestMethod]
        public void Query_NotEmpty_Params_EquipmentDetails()
        {
            Guid g1 = Guid.Parse("81fd22ff-d39d-45e0-b10b-ce80439fee8b");
            Guid g2 = Guid.Parse("cf544533-0039-4061-8551-de13915aff6a");
            Guid g3 = Guid.Parse("27442b3a-1b18-438e-9f20-2225d48ee39e");
            Mock<IEquipmentDetailRepository> mock = new Mock<IEquipmentDetailRepository>();
            IQueryable<EquipmentDetail> equipmentDetails = new EquipmentDetail[]{
                new  EquipmentDetail{ Id = g1,  Name = "E1"},
                new  EquipmentDetail{ Id = g2, Name = "E2"},
                new  EquipmentDetail{ Id = g3, Name = "E3"}
            }.AsQueryable().OrderBy(p => p.Id).Take(4);
            mock.Setup(m => m.EquipmentDetails).Returns(equipmentDetails);

            EquipmentDetailController target = new EquipmentDetailController(mock.Object);
            EquipmentDetailListViewModel result = ((EquipmentDetailListViewModel)target.QueryEquipmentDetail("E2","","","","").Model);
            Assert.AreEqual(result.EquipmentDetails.Count(), 1);
            Assert.AreEqual("E2", result.EquipmentDetails.ToArray()[0].Name);

        }
    }
}
