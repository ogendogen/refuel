var ctx = document.getElementById('myChart').getContext('2d');
$.myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: 'Spalanie',
            data: [],
            borderWidth: 2,
            borderColor: "lime",
            borderBackground: "lime",
            fill: false
        }]
    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: false
                }
            }]
        },
        responsive: false
    }
});

function getGeneralData() {

    var urlParts = window.location.href.split("/");
    var vehicleId = urlParts[urlParts.length - 1];

    $.get("/api/refuelstats/GetVehicleData", { vehicleId: vehicleId }, function (data) {

        if (data["message"] == "ok") {
            $("#priceFor100Km").text(data["priceFor100Km"]);
            $("#averageCombustion").text(data["averageCombustion"]);
            $("#totalCosts").text(data["totalCosts"]["totalCosts"]);
        }
        else {
            alert("error: " + data["message"]);
        }
    });
}

$(document).ready(function () {
    getGeneralData();
});

function handleFuelType() {
    var fuelType = $("#fuelsList").val();
    getCombustionChart(fuelType);
}

function getCombustionChart(fuelType) {

    var urlParts = window.location.href.split("/");
    var vehicleId = urlParts[urlParts.length - 1];

    $.get("/api/refuelstats/GetVehicleFuelStats", { vehicleId: vehicleId, fuelType: fuelType }, function (data) {

        if (data["message"] == "ok") {

            while ($.myChart.data.labels.length > 0) {
                $.myChart.data.labels.pop();
            }

            $.myChart.data.datasets.forEach((dataset) => {
                dataset.data.pop();
            });

            for (var i = 0; i < data["refuelsDataForCharts"].length; i++) {
                $.myChart.data.labels.push(data["refuelsDataForCharts"][i]["refuelDate"].replace("T", "\r\n"));

                var dataset = $.myChart.data.datasets[0];
                dataset.data.push(data["refuelsDataForCharts"][i]["combustion"]);
            }

            $.myChart.update();
        }
        else {
            alert("error: " + data["message"]);
        }

    });
}