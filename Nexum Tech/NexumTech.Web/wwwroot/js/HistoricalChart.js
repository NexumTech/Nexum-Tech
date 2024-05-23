$(document).ready(function () {
    function fetchHistoricalData() {
        $.ajax({
            type: 'POST',
            url: '/HistoricalChart/GetHistoricalTemperature',
            success: function (response) {
                let historicalTemperatures = response.temperatureRecords.map(record => record.attrValue);
                let historicalTimeStamps = response.temperatureRecords.map(record => record.recvTime);

                updateHistoricalChart(historicalTemperatures, historicalTimeStamps);
            },
            error: function (xhr, status, error) {
                console.log('Error:', error);
            }
        });
    }

    function updateHistoricalChart(temperatures, timestamps) {
        historicalChart.data.labels = [];

        timestamps.forEach(function (timestamp) {
            let date = new Date(timestamp);
            historicalChart.data.labels.push(date.toLocaleTimeString());
        });

        historicalChart.data.datasets[0].data = temperatures;
        historicalChart.update();
    }

    const ctx = document.getElementById('historicalChart').getContext('2d');
    const historicalChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: 'Temperature ( ' + String.fromCharCode(176) + 'C )',
                data: [],
                backgroundColor: 'rgba(16, 60, 190, 0.2)',
                borderColor: 'rgba(16, 60, 190, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: false
                }
            }
        }
    });

    fetchHistoricalData();
});
