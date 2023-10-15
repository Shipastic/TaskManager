using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services
{
    public class UsersRequestService
    {
        private const string HOST = "http://localhost:40464/api/";

        private string _userControllerUrl = HOST + "users";

        private string GetDataByUrl(HttpMethod method, string url, AuthToken token, string userName = null, string password = null)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method.Method;

            if (userName != null && password != null)
            {
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);
            }
            else
            {
                if (token != null)
                {
                    request.Headers.Add("Authorization", "Bearer " + token.access_token);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseStr = reader.ReadToEnd();
                result = responseStr;
            }
            return result;
        }

        private HttpStatusCode SendDataByUrl(HttpMethod method, string url, AuthToken token, string data)
        {
            HttpResponseMessage result = new();
            HttpClient client = new();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            if (method == HttpMethod.Post)
            {
                result = client.PostAsync(url, content).Result;
            }
            if (method == HttpMethod.Patch)
            {
                result = client.PatchAsync(url, content).Result;
            }

            return result.StatusCode;
        }

        private HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            HttpResponseMessage result = new();
            HttpClient client = new();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            result = client.DeleteAsync(url).Result;
            return result.StatusCode;
        }

        public AuthToken GetToken(string userName, string password)
        {
            string url = HOST + "account/token";
            string resultStr = GetDataByUrl(HttpMethod.Post, url, null, userName, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
            return token;
        }

        public HttpStatusCode CreateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Post, _userControllerUrl, token, userJson);
            return result;
        }

        public List<UserModel> GetAllUsers(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _userControllerUrl, token);
            List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(response);
            return users; 
        }

        public HttpStatusCode DeleteUser(AuthToken token, int userId)
        {
            var result = DeleteDataByUrl(_userControllerUrl + $"/{userId}", token);
            return result;
        }

        public HttpStatusCode CreateMultipleUsers(AuthToken token, List<UserModel> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = SendDataByUrl(HttpMethod.Post, _userControllerUrl + "/all", token, userJson);
            return result;
        }

        public HttpStatusCode UpdateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Patch, _userControllerUrl + $"/{user.Id}", token, userJson);
            return result;
        }
    }
}
