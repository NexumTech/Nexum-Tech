let temperatures = [];
let timeStamps = [];

$(document).ready(function () {
    function addTemperatureAndTime(temperature, timestamp) {
        temperatures.push(temperature);
        timeStamps.push(timestamp);

        updateChart();
    }

    function updateChart() {
        realTimeChart.data.labels = [];

        timeStamps.forEach(function (timestamp) {
            let date = new Date(timestamp);
            realTimeChart.data.labels.push(date.toLocaleTimeString());
        });

        realTimeChart.data.datasets[0].data = temperatures;
        realTimeChart.update();
    }

    function fetchTemperatureData() {
        $.ajax({
            type: 'POST',
            global: false,
            url: '/historicalChart/GetHistoricalTemperature',
            success: function (response) {
                addTemperatureAndTime(response.value, response.metadata.timeInstant.value);
            },
            error: function (xhr, status, error) {
                console.log('Erro:' + error);
            }
        });
    }

    const ctx = document.getElementById('historicalChart').getContext('2d');
    const realTimeChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: 'Temperatura ( ' + String.fromCharCode(176) + 'C )',
                data: [],
                backgroundColor: 'rgba(16, 60, 190, 0.2)',
                borderColor: 'rgba(16, 60, 190, 1)',
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

    fetchTemperatureData();
});