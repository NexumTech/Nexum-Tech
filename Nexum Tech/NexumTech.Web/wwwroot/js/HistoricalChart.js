$(document).ready(function () {
    const ctx = document.getElementById('historicalChart').getContext('2d');
    let historicalChart;

    initializeChart();

    $('#filterForm').submit(function (event) {
        event.preventDefault();

        const userStartDate = new Date($('#startDate').val() + 'T00:00:00-03:00'); // Data de início selecionada pelo usuário em UTC-3
        const userEndDate = new Date($('#endDate').val() + 'T23:59:59-03:00'); // Data de fim selecionada pelo usuário em UTC-3

        const apiStartDate = new Date(userStartDate.getTime() + userStartDate.getTimezoneOffset() * 60000).toISOString();
        const apiEndDate = new Date(userEndDate.getTime() + userEndDate.getTimezoneOffset() * 60000).toISOString();

        fetchAllHistoricalData(apiStartDate, apiEndDate);
    });

    function initializeChart() {
        historicalChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Temp ( ' + String.fromCharCode(176) + 'C )',
                    data: [],
                    backgroundColor: 'rgba(2, 104, 255, 0.2)',
                    borderColor: 'rgba(2, 104, 255, 1)',
                    borderWidth: 1,
                    pointRadius: 0,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: false
                    }
                },
                plugins: {
                    legend: {
                        onClick: null
                    }
                }
            }
        });

        $(window).resize(function () {
            historicalChart.resize();
        });
    }

    function updateChartWithFilters(temperatures, timestamps) {
        historicalChart.data.labels = timestamps.map(timestamp => {
            let date = new Date(timestamp);
            return formatDate(date);
        });

        historicalChart.data.datasets[0].data = temperatures;
        historicalChart.update();
    }

    function formatDate(date) {
        const options = { day: 'numeric', month: 'numeric', hour: '2-digit', minute: '2-digit' };
        return date.toLocaleString('pt-BR', options);
    }

    function fetchAllHistoricalData(dateFrom, dateTo) {
        let allTemperatures = [];
        let allTimeStamps = [];
        let offset = 0;
        const limit = 100;

        function fetchPaginatedData() {
            $.ajax({
                type: 'POST',
                url: '/HistoricalChart/GetHistoricalTemperature',
                data: {
                    dateFrom: dateFrom,
                    dateTo: dateTo,
                    hOffset: offset,
                    limit: limit
                },
                success: function (response) {
                    const records = response.temperatureRecords;
                    if (records.length > 0) {
                        allTemperatures = allTemperatures.concat(records.map(record => record.attrValue));
                        allTimeStamps = allTimeStamps.concat(records.map(record => record.recvTime));
                        offset += limit;
                        fetchPaginatedData();
                    } else {
                        updateChartWithFilters(allTemperatures, allTimeStamps);
                    }
                },
                error: function (xhr, status, error) {
                    console.log('Error:', error);
                }
            });
        }

        fetchPaginatedData();
    }
});
