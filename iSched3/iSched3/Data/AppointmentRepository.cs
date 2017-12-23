using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using iSched3.Helpers;
using iSched3.Models;
using Newtonsoft.Json;
using Syncfusion.SfSchedule.XForms;

namespace iSched3.Data
{
    public class AppointmentRepository : IAppointmentsRepository
    {
        private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://ischedwebapi.azurewebsites.net/api/") };

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            try
            {
                if (!String.IsNullOrEmpty(Settings.Token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                }
                var response = await _client.GetAsync("appointments");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<Appointment> appointments = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(json);
                    return appointments;
                }
                
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

            return null;
        }

        public async Task<bool> AddAsync(string name, string comments, DateTime startTime, DateTime endTime, bool isAllDay, bool isRecurrence)
        {
            var model = new Appointment()
            {
                Name = name,
                Comments = comments,
                StartTime = startTime,
                EndTime = endTime,
                IsAllDay = isAllDay,
                IsRecurrence = isRecurrence
            };

            try
            {
                if (!String.IsNullOrEmpty(Settings.Token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                }

                var json = JsonConvert.SerializeObject(model);

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PostAsync("appointments", content);
                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
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

        public async Task<bool> EditAsync(int id, string name, string comments, DateTime startTime, DateTime endTime, bool isAllDay, bool isRecurrence)
        {
            var model = new Appointment()
            {
                Id = id,
                Name = name,
                Comments = comments,
                StartTime = startTime,
                EndTime = endTime,
                IsAllDay = isAllDay,
                IsRecurrence = isRecurrence
            };

            try
            {
                if (!String.IsNullOrEmpty(Settings.Token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                }

                var json = JsonConvert.SerializeObject(model);

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _client.PutAsync($"appointments/{model.Id}", content);
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
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

        public async Task<bool> DeleteAsync(int id)
        {
            if (!String.IsNullOrEmpty(Settings.Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
            }

            try
            {
                var response = await _client.DeleteAsync($"appointments/{id}");
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

    }
}
