using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Moq;

namespace EEDDMS.Tests
{


    /// <summary>
    ///这是 ManufacturerControllerTest 的测试类，旨在
    ///包含所有 ManufacturerControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class ManufacturerControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_Manufacturers()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[]{
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new Manufacturer{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer[] result = ((IEnumerable<Manufacturer>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("M1", result[0].Name);
            Assert.AreEqual("M2", result[1].Name);
            Assert.AreEqual("M3", result[2].Name);
        }

        [TestMethod]
        public void Can_Show_Manufacturer_Details()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[]{
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new Manufacturer{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer m1 = target.Details(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Manufacturer;
            Manufacturer m2 = target.Details(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as Manufacturer;
            Manufacturer m3 = target.Details(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as Manufacturer;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Can_Edit_Manufacturer()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[]{
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new Manufacturer{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer m1 = target.Edit(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as Manufacturer;
            Manufacturer m2 = target.Edit(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as Manufacturer;
            Manufacturer m3 = target.Edit(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as Manufacturer;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), m1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), m2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), m3.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Manufacturer()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[]{
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new Manufacturer{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer result = (Manufacturer)target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer item = new Manufacturer { Name = "M1" };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveManufacturer(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();

            ManufacturerController target = new ManufacturerController(mock.Object);

            Manufacturer item = new Manufacturer { Name = "M1" };

            target.ModelState.AddModelError("error", "error");

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveManufacturer(It.IsAny<Manufacturer>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Manufacturers()
        {
            Manufacturer item = new Manufacturer { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "Test" };

            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[] {
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                item,
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteManufacturer(item));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_Manufacturers()
        {
            Mock<IManufacturerRepository> mock = new Mock<IManufacturerRepository>();
            mock.Setup(m => m.Manufacturers).Returns(new Manufacturer[]{
                new Manufacturer{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "M1"},
                new Manufacturer{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "M2"},
                new Manufacturer{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "M3"}
            }.AsQueryable());

            ManufacturerController target = new ManufacturerController(mock.Object);

            target.Delete(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826"));

            mock.Verify(m => m.DeleteManufacturer(It.IsAny<Manufacturer>()), Times.Never());
        }
    }
}
