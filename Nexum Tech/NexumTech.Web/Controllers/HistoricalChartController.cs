﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Controllers
{
    public class HistoricalChartController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public HistoricalChartController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoricalTemperature()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                var response = await _httpService.CallMethod<HistoricalApiResponse>(_appSettingsUI.Fiware.ApiFiwareHistoricalChartURL, HttpMethod.Get, token, headers: headers, urlFiware: _appSettingsUI.Fiware.ApiFiwareHistoricalChartURL);

                var temperatureRecords = response.ContextResponses[0].ContextElement.Attributes[0].Values.Select(v => new TemperatureRecord
                {
                    Id = v.Id,
                    RecvTime = v.RecvTime,
                    AttrName = v.AttrName,
                    AttrType = v.AttrType,
                    AttrValue = v.AttrValue
                }).ToList();

                var viewModel = new HistoricalChartViewModel
                {
                    TemperatureRecords = temperatureRecords
                };

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class HistoricalApiResponse
        {
            public List<ContextResponse> ContextResponses { get; set; }
        }

        public class ContextResponse
        {
            public ContextElement ContextElement { get; set; }
        }

        public class ContextElement
        {
            public List<Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            public string Name { get; set; }
            public List<Value> Values { get; set; }
        }

        public class Value
        {
            public string Id { get; set; }
            public DateTime RecvTime { get; set; }
            public string AttrName { get; set; }
            public string AttrType { get; set; }
            public double AttrValue { get; set; }
        }

    }
}
