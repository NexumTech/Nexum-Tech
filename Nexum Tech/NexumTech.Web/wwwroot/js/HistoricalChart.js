$(document).ready(function () {
    const ctx = document.getElementById('historicalChart').getContext('2d');
    let historicalChart;

    // Inicializa o gr�fico com dados padr�o
    initializeChart();

    // Manipula o envio do formul�rio de filtro
    $('#filterForm').submit(function (event) {
        event.preventDefault(); // Impede o envio padr�o do formul�rio

        // Captura os valores dos campos de filtro
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        // Atualiza o gr�fico com os novos filtros
        updateChartWithFilters(startDate, endDate);
    });

    // Fun��o para inicializar o gr�fico com dados padr�o
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
    }

    function updateChartWithFilters(dateFrom, dateTo) {
        $.ajax({
            type: 'POST',
            url: '/HistoricalChart/GetHistoricalTemperature',
            data: {
                dateFrom: dateFrom,
                dateTo: dateTo
            },
            success: function (response) {
                let temperatures = response.temperatureRecords.map(record => record.attrValue);
                let timestamps = response.temperatureRecords.map(record => record.recvTime);

                historicalChart.data.labels = timestamps.map(timestamp => {
                    let date = new Date(timestamp);
                    return date.toLocaleTimeString();
                });

                historicalChart.data.datasets[0].data = temperatures;
                historicalChart.update();
            },
            error: function (xhr, status, error) {
                console.log('Erro: ' + error);
            }
        });
    }
});
