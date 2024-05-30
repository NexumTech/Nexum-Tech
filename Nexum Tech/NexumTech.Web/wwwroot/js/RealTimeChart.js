let temperatures = [];
let timeStamps = [];
let isLoopActive = true;
let currentDevice;
let currentMaxTemp;
let currentMinTemp;

$(document).ready(function () {
    let currentSelectedCompany = $('#currentCompanyId').text();

    getDevices(currentSelectedCompany);

    $('#companiesList').on('click', 'a', function (event) {
        event.preventDefault();

        var companyId = $(this).data('company-id');
        getDevices(companyId);

        currentDevice = undefined;
        resetChart();
    });

    function getDevices(companyId) {
        $.ajax({
            type: 'GET',
            url: '/RealTimeChart/GetDevices',
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

                if (data.length == 0) {
                    var listItem = $('<li>').append($('<button>', {
                        class: 'dropdown-item',
                        type: 'button',
                        text: 'Nenhum dispositivo encontrado para essa empresa',
                        disabled: true,
                    }));

                    $('#devicesList').append(listItem);
                }

                $('#devicesList').on('click', 'button.dropdown-item', function () {
                    var deviceName = $(this).text();
                    $('#selectedDevice').html('<i class="fa-solid fa-mobile-screen mx-1"></i> &nbsp' + deviceName);
                    resetChart();
                    currentDevice = deviceName;
                });
            },
        });
    }

    $(document).on('input', '#txtMinTemp, #txtMaxTemp', function () {
        var minValue = parseInt($(this).val());
        var maxValue = parseInt($('#txtMaxTemp').val());

        if (minValue > maxValue) {
            $(this).val(maxValue);
        } else if (this.value < 0) {
            this.value = 0;
        } else if (this.value > 80) {
            this.value = 80;
        }
    });

    $('#btnTempNotification').click(function () {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-outline-success mx-2",
                denyButton: "btn btn-outline-danger mx-2"
            },
            buttonsStyling: false,
        });

        swalWithBootstrapButtons.fire({
            title: 'Temperature limits notification',
            html:
                '<div class="row mt-4 mb-2">' +
                '<div class="col-md-6 d-flex justify-content-center">' +
                '<input type="number" id="txtMinTemp" min="0" max="80" class="swal2-input" placeholder="Min">' +
                '</div>' +
                '<div class="col-md-6 d-flex justify-content-center">' +
                '<input type="number" id="txtMaxTemp" min="0" max="80" class="swal2-input" placeholder="Max">' +
                '</div>' +
                '</div>',
            showDenyButton: true,
            confirmButtonText: 'Notify',
            denyButtonText: 'Cancel',
            focusConfirm: false,
            preConfirm: () => {
                return [
                    $('#txtMinTemp').val(),
                    $('#txtMaxTemp').val()
                ]
            }
        }).then((result) => {
            if (result.isConfirmed) {
                if (!isNaN(result.value[0]) && !isNaN(result.value[1]) && result.value[0] !== null && result.value[1] !== null && result.value[0] !== "" && result.value[1] !== "") {
                    currentMinTemp = parseFloat(result.value[0]);
                    currentMaxTemp = parseFloat(result.value[1]);

                    $('#btnTempNotification').tooltip('dispose').tooltip({
                        title: `<b id='lblMinTemp' class='text-success'>Min: ${currentMinTemp}\u2103 </b> <br> <b id='lblMaxTemp' class='text-danger'>Max: ${currentMaxTemp}\u2103 </b>`,
                        html: true,
                        trigger: 'hover'
                    });
                }
            }
        });
    });

    function addTemperatureAndTime(temperature, timestamp) {
        checkDifferentDates();
        checkTemperatureParameters();

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

    function checkTemperatureParameters() {
        var currentTemp = parseFloat($('#realTemp').text());

        if (currentTemp < currentMinTemp || currentTemp > currentMaxTemp)
            $('#temperatureAlertIcon').removeClass('d-none');
        else
            $('#temperatureAlertIcon').addClass('d-none');
    }

    function fetchTemperatureData() {
        if (currentDevice != undefined) {
            $.ajax({
                type: 'POST',
                global: false,
                url: '/RealTimeChart/GetRealTemperature',
                data: {
                    deviceName: currentDevice,
                },
                success: function (response) {
                    addTemperatureAndTime(response.value, response.metadata.timeInstant.value);
                },
                error: function (xhr, status, error) {
                    console.log('Erro: ' + error);
                }
            });
        }
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
