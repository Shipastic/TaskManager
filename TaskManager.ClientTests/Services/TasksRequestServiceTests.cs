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
    public class TasksRequestServiceTests
    {
        private AuthToken _token;
        private TasksRequestService _service;

        public TasksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "apolda25");
            _service = new TasksRequestService();
        }

        [TestMethod()]
        public void GetTasksTest()
        {
            var tasks = _service.GetTasks(_token);

            Console.WriteLine(tasks.Count);

            Assert.AreNotEqual(0, tasks);
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            var task = _service.GetTaskById(_token, 1);

            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public void GetTasksByDeskTest()
        {
            var tasks = _service.GetTasksByDesk(_token, 1);

            Assert.AreNotEqual(0, tasks);
        }

        [TestMethod()]
        public void CreateTaskTest()
        {
            var task = new TaskModel("Task #1", "Task from unit test", DateTime.Now, DateTime.Now, "New");
            task.DeskId = 6;
            task.ExecutorId = 1;
            var result = _service.CreateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            var task = new TaskModel("Task #1 v2.0", "Task updated from unit test", DateTime.Now, DateTime.Now, "In Progress");
            task.Id = 3;
            task.ExecutorId = 2;
            var result = _service.UpdateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteTaskByIdTest()
        {
            var result = _service.DeleteTaskById(_token, 4);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}