$(document).ready(function () {
    const ctx = document.getElementById('historicalChart').getContext('2d');
    let historicalChart;

    // Inicializa o gráfico com dados padrão
    initializeChart();

    // Manipula o envio do formulário de filtro
    $('#filterForm').submit(function (event) {
        event.preventDefault(); // Impede o envio padrão do formulário

        // Captura os valores dos campos de filtro
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        // Atualiza o gráfico com os novos filtros
        updateChartWithFilters(startDate, endDate);
    });

    // Função para inicializar o gráfico com dados padrão
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
