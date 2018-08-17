using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Controllers;
using EEDDMS.WebSite.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EEDDMS.Tests
{


    /// <summary>
    ///这是 EquipmentClassControllerTest 的测试类，旨在
    ///包含所有 EquipmentClassControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class EquipmentClassControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_EquipmentClasses()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();

            EquipmentClass class1 = new EquipmentClass { Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1", SN = 0 };

            EquipmentClass class2 = new EquipmentClass { Id = Guid.Parse("5ac60e18-f424-4523-82b2-d5ef2a61aa8c"), Name = "C2", SN = 1 };

            EquipmentClass class3 = new EquipmentClass { Id = Guid.Parse("6aa0ce18-3ff5-4036-b55e-c4a5aaa9a472"), Name = "C3", SN = 2 };

            EquipmentClass class11 = new EquipmentClass
            {
                Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"),
                Name = "C11",
                SN = 0,
                ParentId = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"),
                Parent = class1
            };

            EquipmentClass class111 = new EquipmentClass
            {
                Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"),
                Name = "C111",
                SN = 0,
                ParentId = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"),
                Parent = class11
            };

            EquipmentClass class112 = new EquipmentClass
            {
                Id = Guid.Parse("fe0d4c55-23e9-42ab-9a81-865f18fc7cf9"),
                Name = "C112",
                SN = 1,
                ParentId = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"),
                Parent = class11
            };

            class11.Children.Add(class111);
            class11.Children.Add(class112);
            class1.Children.Add(class11);

            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[]{
                class1,
                class11,
                class111,
                class112,
                class2,
                class3
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClassTreeNode[] result = ((IEnumerable<EquipmentClassTreeNode>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 6);
            Assert.AreEqual("C1", result[0].EquipmentClass.Name);
            Assert.AreEqual("C11", result[1].EquipmentClass.Name);
            Assert.AreEqual("C111", result[2].EquipmentClass.Name);
            Assert.AreEqual("C112", result[3].EquipmentClass.Name);
            Assert.AreEqual("C2", result[4].EquipmentClass.Name);
            Assert.AreEqual("C3", result[5].EquipmentClass.Name);

            Assert.AreEqual(0, result[0].LevelNumber);
            Assert.AreEqual(1, result[1].LevelNumber);
            Assert.AreEqual(2, result[2].LevelNumber);
            Assert.AreEqual(2, result[3].LevelNumber);
            Assert.AreEqual(0, result[4].LevelNumber);
            Assert.AreEqual(0, result[5].LevelNumber);

            Assert.AreEqual("1", result[0].WbsNumber);
            Assert.AreEqual("1.1", result[1].WbsNumber);
            Assert.AreEqual("1.1.1", result[2].WbsNumber);
            Assert.AreEqual("1.1.2", result[3].WbsNumber);
            Assert.AreEqual("2", result[4].WbsNumber);
            Assert.AreEqual("3", result[5].WbsNumber);

            Assert.AreEqual(NodeSiblingPosition.First, result[0].IsFirstOrLastNode);
            Assert.AreEqual(NodeSiblingPosition.Only, result[1].IsFirstOrLastNode);
            Assert.AreEqual(NodeSiblingPosition.First, result[2].IsFirstOrLastNode);
            Assert.AreEqual(NodeSiblingPosition.Last, result[3].IsFirstOrLastNode);
            Assert.AreEqual(NodeSiblingPosition.Middle, result[4].IsFirstOrLastNode);
            Assert.AreEqual(NodeSiblingPosition.Last, result[5].IsFirstOrLastNode);
        }

        [TestMethod]
        public void Can_Show_EquipmentClass_Details()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();
            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[]{
                new EquipmentClass{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1"},
                new EquipmentClass{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "C2"},
                new EquipmentClass{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "C3"}
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClass c1 = target.Details(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as EquipmentClass;
            EquipmentClass c2 = target.Details(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as EquipmentClass;
            EquipmentClass c3 = target.Details(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as EquipmentClass;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), c1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), c2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), c3.Id);
        }

        [TestMethod]
        public void Can_Edit_EquipmentClass()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();
            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[]{
                new EquipmentClass{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1"},
                new EquipmentClass{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "C2"},
                new EquipmentClass{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "C3"}
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClass c1 = target.Edit(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9")).ViewData.Model as EquipmentClass;
            EquipmentClass c2 = target.Edit(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7")).ViewData.Model as EquipmentClass;
            EquipmentClass c3 = target.Edit(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826")).ViewData.Model as EquipmentClass;

            Assert.AreEqual(Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), c1.Id);
            Assert.AreEqual(Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), c2.Id);
            Assert.AreEqual(Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), c3.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_EquipmentClass()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();
            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[]{
                new EquipmentClass{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1"},
                new EquipmentClass{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "C2"},
                new EquipmentClass{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "C3"}
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClass result = (EquipmentClass)target.Edit(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826")).ViewData.Model;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClass item = new EquipmentClass { Name = "C1" };

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipmentClass(item));

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            EquipmentClass item = new EquipmentClass { Name = "C1" };

            target.ModelState.AddModelError("error", "error");

            ActionResult result = target.Edit(item);

            mock.Verify(m => m.SaveEquipmentClass(It.IsAny<EquipmentClass>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_EquipmentClasses()
        {
            EquipmentClass item = new EquipmentClass { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "Test" };

            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();
            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[] {
                new EquipmentClass{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1"},
                item,
                new EquipmentClass{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "C3"}
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            target.Delete(item.Id);

            mock.Verify(m => m.DeleteEquipmentClass(item));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_EquipmentClasses()
        {
            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();
            mock.Setup(m => m.EquipmentClasses).Returns(new EquipmentClass[]{
                new EquipmentClass{ Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), Name = "C1"},
                new EquipmentClass{ Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "C2"},
                new EquipmentClass{ Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), Name = "C3"}
            }.AsQueryable());

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            target.Delete(Guid.Parse("3ecfd3ad-0000-457c-96f8-7eb884ab6826"));

            mock.Verify(m => m.DeleteEquipmentClass(It.IsAny<EquipmentClass>()), Times.Never());
        }

        [TestMethod]
        public void Can_Move_Up_EquipmentClass()
        {
            EquipmentClass item = new EquipmentClass { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "Test" };

            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            target.MoveUp(item.Id);

            mock.Verify(m => m.MoveUpAndDownEquipmentClass(item.Id, false));
        }

        [TestMethod]
        public void Can_Move_Down_EquipmentClass()
        {
            EquipmentClass item = new EquipmentClass { Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), Name = "Test" };

            Mock<IEquipmentClassRepository> mock = new Mock<IEquipmentClassRepository>();

            EquipmentClassController target = new EquipmentClassController(mock.Object);

            target.MoveDown(item.Id);

            mock.Verify(m => m.MoveUpAndDownEquipmentClass(item.Id, true));
        }
    }
}
