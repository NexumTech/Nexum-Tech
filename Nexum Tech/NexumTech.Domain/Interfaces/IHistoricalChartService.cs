﻿using NexumTech.Infra.ViewModels;

namespace NexumTech.Web.Services
{
    public interface IHistoricalChartService
    {
        Task<List<HistoricalChartViewModel.TemperatureRecord>> GetHistoricalTemperature(string dateFrom, string dateTo, string deviceName, int hOffset = 0, int hLimit = 100);
    }
}