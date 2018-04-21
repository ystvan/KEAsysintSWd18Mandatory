using KEAWebApp.Models;
using KEAWebApp.Models.Dtos;
using KEAWebApp.Models.ViewModels;
using KEAWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace KEAWebApp.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ISensorHttpClient _sensorHttpClient;

        public DataController(ISensorHttpClient sensorHttpClient)
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
                    var response = await apiClient.GetAsync("api/SensorDatas");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        //json serializer settings
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Culture = CultureInfo.CurrentCulture,
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateParseHandling = DateParseHandling.DateTime,
                            Error = delegate (object sender, ErrorEventArgs args)
                            {
                                args.ErrorContext.Handled = true;
                            }
                        };

                        var measurements = JsonConvert.DeserializeObject<List<Data>>(result, settings);
                        var viewModel = new ShowDataViewModel(measurements);

                        return View(viewModel);
                    }
                    else
                    {
                        var m = new ApiResponseViewModel();
                        m.Content = response.Headers.ToString();
                        m.Message = response.ToString();
                        m.StatusCode = response.StatusCode.ToString();

                        return RedirectToAction("ShowOtherStatusCodesFromApi", m);
                    }
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateDataViewModel measurement)
        {            
            if (!ModelState.IsValid)
                return View(measurement);
            if (ModelState.IsValid)
            {
                var modelForCreation = new SensorDto {

                    Id = measurement.LocationIdentifier,
                    Light = measurement.Light,
                    Temperature = measurement.Temperature,
                    TimeStamp = DateTime.Now.ToLongTimeString()
                };

                var serializedModel = JsonConvert.SerializeObject(modelForCreation);

                var apiClient = await _sensorHttpClient.GetClient();

                using (apiClient)
                {
                    try
                    {
                        var response = await apiClient.PostAsync("api/SensorDatas",
                        new StringContent(serializedModel, System.Text.Encoding.Unicode, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var m = new ApiResponseViewModel();
                            m.Content = response.Headers.ToString();
                            m.Message = response.ToString();
                            m.StatusCode = response.StatusCode.ToString();
                            
                            return RedirectToAction("ShowOtherStatusCodesFromApi", m );
                            
                        }

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
            return View(measurement);
        }


        [HttpGet]
        public IActionResult ShowOtherStatusCodesFromApi(ApiResponseViewModel msgFromApi)
        {
            return View(msgFromApi);
        }
    }
}