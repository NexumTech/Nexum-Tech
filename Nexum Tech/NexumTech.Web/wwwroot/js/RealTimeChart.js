let temperatures = [];
let timeStamps = [];

$(document).ready(function () {
    function addTemperatureAndTime(temperature, timestamp) {
        temperatures.push(temperature);
        timeStamps.push(timestamp);

        updateChart();
        updateRealTemp(temperature);
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

    $('#btnReload').on('click', function () {
        resetChart();
    });

    function loop() {
        fetchTemperatureData();
    }

    setInterval(loop, 500);
});
