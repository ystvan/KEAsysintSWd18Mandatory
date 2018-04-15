using KEAWebApp.Models;
using KEAWebApp.Models.ViewModels;
using KEAWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KEAWebApp.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ISensorHttpClient _sensorHttpClient;
        public LogsController(ISensorHttpClient sensorHttpClient)
        {
            _sensorHttpClient = sensorHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var apiClient = await _sensorHttpClient.GetClient();

            using (apiClient)
            {
                try
                {
                    var response = await apiClient.GetAsync("api/Logs");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        var logFiles = JsonConvert.DeserializeObject<List<Log>>(result);
                        var viewModel = new ShowLogViewModel(logFiles);

                        return View(viewModel);
                    }
                    throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException(e.Message);
                }
                catch (ArgumentNullException n)
                {
                    throw new ArgumentNullException(n.Message);
                }
            }
                        
        }
    }
}