using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Common.Models;
using TaskManager.Client.Models;
using System.Net;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class ProjectsRequestServiceTests
    {
        private AuthToken _token;

        private ProjectsRequestService _service;
        public ProjectsRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "apolda25");
            _service = new ProjectsRequestService();
        }

        [TestMethod()]
        public void GetProjectsTest()
        {
            var projects = _service.GetProjects(_token);

            Console.WriteLine(projects.Count);

            Assert.AreNotEqual(Array.Empty<ProjectModel>(), projects);
        }

        [TestMethod()]
        public void GetProjectByIdTest()
        {
            var project = _service.GetProjectById(_token, 3);

            Assert.AreNotEqual(null, project);
        }

        [TestMethod()]
        public void CreateProjectTest()
        {
            ProjectModel project = new ProjectModel("Test project", "New test project from tests", ProjectStatus.InProgress);
            project.AdminId = 1;

            var result = _service.CreateProject(_token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateProjectTest()
        {
            ProjectModel project = new ProjectModel("Test project updated", "New test project from tests. Update v2", ProjectStatus.Suspended);
            project.Id = 7;
            var result = _service.UpdateProject(_token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var result = _service.DeleteProject(_token, 7);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void AddUsersToProjectTest()
        {
            var result = _service.AddUsersToProject(_token, 6, new List<int>{24, 25});
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void RemoveUsersFromProjectTest()
        {
            var result = _service.RemoveUsersFromProject(_token, 6, new List<int> { 24 });
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}