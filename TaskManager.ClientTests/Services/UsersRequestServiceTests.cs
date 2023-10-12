using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("_", "_");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
        }
    }
}
