$(document).ready(function () {
    let currentDevice;
    const ctx = document.getElementById('historicalChart').getContext('2d');
    let historicalChart;

    let currentSelectedCompany = $('#currentCompanyId').text();

    getDevices(currentSelectedCompany);

    $('#companiesList').on('click', 'a', function (event) {
        event.preventDefault();

        var companyId = $(this).data('company-id');
        getDevices(companyId);
    });

    function getDevices(companyId) {
        $.ajax({
            type: 'GET',
            url: '/HistoricalChart/GetDevices',
            data: {
                companyId: companyId,
            },
            success: function (data) {
                $('#devicesList').empty();

                data.forEach(function (device) {
                    var listItem = $('<li>').append($('<button>', {
                        class: 'dropdown-item',
                        type: 'button',
                        text: device.name
                    }));

                    $('#devicesList').append(listItem);
                });

                $('#devicesList').on('click', 'button.dropdown-item', function () {
                    var deviceName = $(this).text();
                    $('#selectedDevice').html('<i class="fa-solid fa-mobile-screen mx-1"></i> &nbsp' + deviceName);
                    currentDevice = deviceName;
                });
            },
        });
    }

    initializeChart();

    $('#filterForm').submit(function (event) {
        event.preventDefault();

        const userStartDate = new Date($('#startDate').val() + 'T00:00:00-03:00'); 
        const userEndDate = new Date($('#endDate').val() + 'T23:59:59-03:00'); 

        const apiStartDate = new Date(userStartDate.getTime() + userStartDate.getTimezoneOffset() * 60000).toISOString();
        const apiEndDate = new Date(userEndDate.getTime() + userEndDate.getTimezoneOffset() * 60000).toISOString();

        fetchAllHistoricalData(apiStartDate, apiEndDate);
    });

    $('#exportImage').on('click', function () {
        exportChartAsImage(historicalChart);
    });

    $('#exportPDF').on('click', function () {
        exportChartAsPDF(historicalChart);
    });

    $('#exportCSV').on('click', function () {
        exportChartAsCSV(historicalChart);
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
                    limit: limit,
                    deviceName: currentDevice,
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

    function exportChartAsImage(chart) {
        const a = document.createElement('a');
        a.href = chart.toBase64Image();
        a.download = 'NexumHistoricalChart.png';
        a.click();
    }

    function exportChartAsPDF(chart) {
        debugger;
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF('landscape');
        const imgData = chart.toBase64Image();

        const pageWidth = doc.internal.pageSize.getWidth();
        const pageHeight = doc.internal.pageSize.getHeight();
        const imgProps = chart.canvas;
        const imgWidth = imgProps.width;
        const imgHeight = imgProps.height;
        const aspectRatio = imgWidth / imgHeight;

        const pdfWidth = pageWidth - 20;
        const pdfHeight = pdfWidth / aspectRatio;

        doc.addImage(imgData, 'PNG', 10, 10, pdfWidth, pdfHeight);

        doc.save('NexumHistoricalChart.pdf');
    }

    function exportChartAsCSV(chart) {
        let csvContent = 'data:text/csv;charset=utf-8,';
        const labels = chart.data.labels.join(',');
        csvContent += labels + '\n';

        chart.data.datasets.forEach((dataset) => {
            const dataString = dataset.data.join(',');
            csvContent += dataset.label + ',' + dataString + '\n';
        });

        const encodedUri = encodeURI(csvContent);
        const a = document.createElement('a');
        a.href = encodedUri;
        a.download = 'NexumHistoricalChart.csv';
        a.click();
    }
});