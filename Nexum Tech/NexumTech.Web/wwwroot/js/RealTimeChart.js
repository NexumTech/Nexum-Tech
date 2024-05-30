let temperatures = [];
let timeStamps = [];
let isLoopActive = true;

$(document).ready(function () {
    function addTemperatureAndTime(temperature, timestamp) {
        checkDifferentDates();

        temperatures.push(temperature);
        timeStamps.push(timestamp);

        if (isLoopActive) {
            updateChart();
            updateRealTemp(temperature);
        }
    }

    function updateChart() {
        realTimeChart.data.labels = timeStamps.map(timestamp => {
            let date = new Date(timestamp);
            return date.toLocaleTimeString();
        });

        realTimeChart.data.datasets[0].data = temperatures;
        realTimeChart.update();
    }

    function updateRealTemp(temperature) {
        $('#realTemp').text(temperature);
    }

    function fetchTemperatureData() {
        $.ajax({
            type: 'POST',
            global: false,
            url: '/RealTimeChart/GetRealTemperature',
            success: function (response) {
                addTemperatureAndTime(response.value, response.metadata.timeInstant.value);
            },
            error: function (xhr, status, error) {
                console.log('Erro: ' + error);
            }
        });
    }

    function resetChart() {
        temperatures = [];
        timeStamps = [];
        realTimeChart.data.labels = [];
        realTimeChart.data.datasets[0].data = [];
        realTimeChart.update();
    }

    const ctx = document.getElementById('realTimeChart').getContext('2d');
    const realTimeChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: 'Temp ( ' + String.fromCharCode(176) + 'C )',
                data: [],
                backgroundColor: 'rgba(2, 104, 255, 0.2)',
                borderColor: 'rgba(2, 104, 255, 1)',
                borderWidth: 1,
                pointRadius: 0
            }]
        },
        options: {
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

    $('#exportImage').on('click', function () {
        exportChartAsImage(realTimeChart);
    });

    $('#exportPDF').on('click', function () {
        exportChartAsPDF(realTimeChart);
    });

    $('#exportCSV').on('click', function () {
        exportChartAsCSV(realTimeChart);
    });

    $('#realTemp').on('click', function () {
        resetChart();
    });

    function exportChartAsImage(chart) {
        const a = document.createElement('a');
        a.href = chart.toBase64Image();
        a.download = 'NexumRealTimeChart.png';
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

        doc.save('NexumRealTimeChart.pdf');
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
        a.download = 'NexumRealTimeChart.csv';
        a.click();
    }

    function checkDifferentDates(response) {
        if (timeStamps.length >= 3) {
            const currentDate = new Date(timeStamps[timeStamps.length - 1]).toISOString().split('T')[1];
            const lastDate = new Date(timeStamps[timeStamps.length - 2]).toISOString().split('T')[1];

            if (currentDate !== lastDate) {
                isLoopActive = true;
            }
            else {
                isLoopActive = false;
            }
        }
    }

    setInterval(fetchTemperatureData, 1000);
});
