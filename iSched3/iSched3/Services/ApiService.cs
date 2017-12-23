using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using iSched3.Helpers;
using iSched3.Models;
using Newtonsoft.Json;

namespace iSched3.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client = new HttpClient{BaseAddress = new Uri("http://ischedwebapi.azurewebsites.net/api/") };

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var model = new LoginBindingModel()
            {
                UserName = userName,
                Password = password
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _client.PostAsync("identity/token", content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var tokenString = await response.Content.ReadAsStringAsync();
                    TokenModel token = JsonConvert.DeserializeObject<TokenModel>(tokenString);
                    Settings.Username = model.UserName;
                    Settings.PassSecret = model.Password;
                    Settings.Token = token.Token;
                    Settings.TokenExpiration = token.Expiration;
                }

                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                if (ex is ObjectDisposedException)
                {
                    Debug.WriteLine("Ignore this ex, it's a temporary xamarin bug");
                } else if (ex is System.Net.Http.HttpRequestException)
                {
                    Debug.WriteLine("Request ex: " + ex);
                } else if (ex is Exception)
                {
                    Debug.WriteLine(ex);
                }
                
            }

            return false;
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            var model = new RegistrationBindingModel()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Password = password
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _client.PostAsync("identity/create", content);
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                if (ex is ObjectDisposedException)
                {
                    Debug.WriteLine("Ignore this ex, it's a temporary xamarin bug");
                }
                else if (ex is System.Net.Http.HttpRequestException)
                {
                    Debug.WriteLine("Request ex: " + ex);
                }
                else if (ex is Exception)
                {
                    Debug.WriteLine(ex);
                }
            }

            return false;
        }

        public async Task<bool> RenewAccessToken()
        {
            if (string.IsNullOrWhiteSpace(Settings.Token))
                return false;

            var timeLeft = (Settings.TokenExpiration - DateTime.UtcNow).TotalMinutes;

            if (timeLeft < 0.5 || double.IsNaN(timeLeft))
            {
                var model = new LoginBindingModel()
                {
                    UserName = Settings.Username,
                    Password = Settings.PassSecret
                };

                var json = JsonConvert.SerializeObject(model);

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                try
                {
                    var response = await _client.PostAsync("identity/token", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Access Token Renewed!");
                        var tokenString = await response.Content.ReadAsStringAsync();
                        TokenModel token = JsonConvert.DeserializeObject<TokenModel>(tokenString);
                        Settings.Token = token.Token;
                        Settings.TokenExpiration = token.Expiration;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
                
            return true;
        }
    }
}
