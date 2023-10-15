using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin", "apolda25");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "apolda25");

            UserModel userTest = new UserModel("Ivan", "Petrov", "petrov_i@mail.ru", "trololo", UserStatus.User, "88005555555");

            var result = service.CreateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetAllUsersTests()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "apolda25");

            var result = service.GetAllUsers(token);

            Console.WriteLine(result.Count);

            Assert.AreNotEqual(Array.Empty<UserModel>(), result.ToArray());
        }

        [TestMethod()]
        public void DeleteUserTests()
        { 
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "apolda25");
             
            var result = service.DeleteUser(token, 26);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateUserTests()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "apolda25");

            UserModel userTest = new UserModel("Sidr", "Egorov", "egorov_s@gmail.com", "qwerty123", UserStatus.User, "+794044123");

            userTest.Id = 26;

            var result = service.UpdateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultipleUsersTests()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "apolda25");

            List<UserModel> usersTest = new()
                                             {
                                                 new UserModel ("Ivan", "Petrov", "petrov_i@mail.ru", "trololo", UserStatus.User, "88005555555"),
                                                 new UserModel("Petr", "Ivanov", "ivanov_p@mail.ru", "qwerty123", UserStatus.Editor, "+79341237744"),
                                                 new UserModel("Sidr", "Egorov", "egorov_s@mail.ru", "qwerty123", UserStatus.User, "+344044123")
                                             };

            var result = service.CreateMultipleUsers(token, usersTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

    }
}
