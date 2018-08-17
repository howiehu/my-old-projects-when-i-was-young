using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using EEDDMS.WebSite.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EEDDMS.Tests
{


    /// <summary>
    ///这是 CollectorRecordControllerTest 的测试类，旨在
    ///包含所有 CollectorRecordControllerTest 单元测试
    ///</summary>
    [TestClass()]
    public class CollectorRecordControllerTest
    {
        [TestMethod]
        public void Can_View_Records_By_Collector_Id()
        {
            Mock<ICollectorRecordRepository> mock = new Mock<ICollectorRecordRepository>();
            mock.Setup(m => m.CollectorRecords).Returns(new CollectorRecord[]{
                new CollectorRecord{ 
                    Id = Guid.Parse("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9"), 
                    CollectorId = Guid.Parse("12345678-d0de-497f-af17-4c5e218243e7"),
                    StartDate = DateTime.Parse("2010-02-01"), 
                    EndDate = DateTime.Parse("2010-02-20")
                },
                new CollectorRecord{
                    Id = Guid.Parse("197d1bc4-d0de-497f-af17-4c5e218243e7"), 
                    CollectorId = Guid.Parse("12345678-d0de-497f-af17-4c5e218243e7"),
                    StartDate = DateTime.Parse("2010-03-01"), 
                    EndDate = null
                },
                new CollectorRecord{
                    Id = Guid.Parse("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826"), 
                    CollectorId = Guid.Parse("12345678-d0de-497f-af17-4c5e218243e7"),
                    StartDate = DateTime.Parse("2010-01-01"), 
                    EndDate = DateTime.Parse("2010-01-20")
                }
            }.AsQueryable());

            CollectorRecordController target = new CollectorRecordController(mock.Object);

            CollectorRecord[] result = ((IEnumerable<CollectorRecord>)target.RecordsByCollector(Guid.Parse("12345678-d0de-497f-af17-4c5e218243e7")).ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("3ecfd3ad-c1d3-457c-96f8-7eb884ab6826", result[0].Id.ToString());
            Assert.AreEqual("8dc960c5-af4d-41d6-9e3a-12d5a4747cb9", result[1].Id.ToString());
            Assert.AreEqual("197d1bc4-d0de-497f-af17-4c5e218243e7", result[2].Id.ToString());
        }
    }
}
