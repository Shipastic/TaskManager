using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Common.Models;
using System.Net;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        private AuthToken _token;
        private DesksRequestService _service;

        public DesksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "apolda25");
            _service = new DesksRequestService();
        }

        [TestMethod()]
        public void GetDesksTest()
        {
            var desks = _service.GetDesks(_token);

            Console.WriteLine(desks.Count);

            Assert.AreNotEqual(Array.Empty<DeskModel>(), desks);
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            var desk = _service.GetDeskById(_token, 1);

            Assert.AreNotEqual(null, desk);
        }

        [TestMethod()]
        public void GetProjectDesksTest()
        {
            var desks = _service.GetProjectDesks(_token, 1);
            Assert.AreNotEqual(0, desks);
        }

        [TestMethod()]
        public void CreateDeskTest()
        {
            var desk = new DeskModel("Test Desk", "Simple desk for tests of service", true, new string[] { "New", "Done" });
            desk.ProjectId = 3;
            desk.AdminId = 1;

            var result = _service.CreateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateDeskTest()
        {
            var desk = new DeskModel("Test Desk", "Simple desk for tests of service", true, new string[] { "New", "In Progress", "Done" });
            desk.ProjectId = 3;
            desk.AdminId = 1;
            desk.Id = 5;

            var result = _service.UpdateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteDeskByIdTest()
        {
            var result = _service.DeleteDeskById(_token, 5);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}