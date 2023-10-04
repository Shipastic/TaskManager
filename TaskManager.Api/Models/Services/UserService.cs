using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TaskManager.Api.Models.Abstractions;
using TaskManager.Api.Models.Data;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models.Services
{
    public class UserService : AbstractionService,  ICommonService<UserModel>
    {
        private readonly ApplicationContext _db;
        public UserService(ApplicationContext db)
        {
            _db = db;
        }

        public Tuple<string, string> GetUserLoginPassFromBasicAuth(HttpRequest  request)
        {
            string userName = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();
            if(authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(":");
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return new Tuple<string, string>(userName, userPass);
        }

        public User GetUser(string logn, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == logn && u.Password == password);
            return user;
        }

        public User GetUser(string login)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login);
            return user;
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUSer = GetUser(username, password);
            if(currentUSer != null)
            {
                currentUSer.LastLoginDate = DateTime.Now;
                _db.Users.Update(currentUSer);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUSer.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUSer.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        public bool Create(UserModel model)
        {
            try
            {
                User newUser = new User(model.FirstName, model.LastName, model.Email, model.Password, model.Status, model.Phone, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return true;
            }  
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int id, UserModel model)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForUpdate != null)
            {

                 DoAction(delegate ()
                {
                    userForUpdate.FirstName = model.FirstName;
                    userForUpdate.LastName = model.LastName;
                    userForUpdate.Email = model.Email;
                    userForUpdate.Password = model.Password;
                    userForUpdate.Status = model.Status;
                    userForUpdate.Phone = model.Phone;
                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                });
            }
            return false;
        }

        public bool Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                 DoAction(delegate ()
                 {
                     _db.Users.Remove(user);
                     _db.SaveChanges();
                 });
            }
            return false;
        }
        public bool CreateMultipleUsers(List<UserModel> userModels)
        {
            return DoAction(delegate ()
            {
                var newUsers = userModels.Select(u => new User(u));
                _db.Users.AddRange(newUsers);
                _db.SaveChangesAsync();
            });
        }

        public UserModel Get(int id)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            return userForUpdate?.ToDto();
        }

        public IEnumerable<UserModel> GetAllByIds(List<int> userIds)
        {
            foreach (int id in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == id).ToDto();
                yield return user;
            }
        }
    }
}
