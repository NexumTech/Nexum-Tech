$(document).ready(function () {
    const ctx = document.getElementById('historicalChart').getContext('2d');
    let historicalChart;

    initializeChart();

    $('#btnApply').on('click', function (event) {
        event.preventDefault();

        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();

        if (!startDate || !endDate) {
            var lblWarning = $('#lblWarning').text();
            var textWarning = $('#textWarning').text();

            Swal.fire({
                title: lblWarning,
                text: textWarning,
                icon: 'warning'
            });
            return; // Não prossiga com o envio do formulário
        }

        const userStartDate = new Date(startDate + 'T00:00:00-03:00'); // Data de início selecionada pelo usuário em UTC-3
        const userEndDate = new Date(endDate + 'T23:59:59-03:00'); // Data de fim selecionada pelo usuário em UTC-3

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

    function exportChartAsImage(chart) {
        const imgData = chart.toBase64Image();
        downloadFile(imgData, 'NexumHistoricalChart.png');
    }

    function exportChartAsPDF(chart) {
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
        const pdfBlob = doc.output('blob');
        downloadFile(pdfBlob, 'NexumHistoricalChart.pdf');
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
        downloadFile(encodedUri, 'NexumHistoricalChart.csv');
    }

    function downloadFile(fileData, fileName) {
        fetch(fileData)
            .then(response => response.blob())
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = fileName;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a);
            })
            .catch(error => console.error('Error downloading file:', error));
    }
});
