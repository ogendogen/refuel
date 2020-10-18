var ctxCombustion = document.getElementById('combustionChart').getContext('2d');
$.combustionChart = new Chart(ctxCombustion, {
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

var ctxPrice = document.getElementById('priceChart').getContext('2d');
$.priceChart = new Chart(ctxPrice, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: 'Cena',
            data: [],
            borderWidth: 2,
            borderColor: "red",
            borderBackground: "red",
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
            $("#totalCosts").text(data["totalCosts"]);

            var dataPerFuelType = data["dataPerFuelType"];

            var fuelsCosts = dataPerFuelType["costsPerFuelType"];
            var fuelCostsRow = $("#petrolTypesCosts");

            var fuelsCombustion = dataPerFuelType["averageCombustionPerFuelType"];
            var fuelsCombustionRow = $("#petrolTypesCombustion");

            var fuelsPriceFor100Km = dataPerFuelType["priceFor100KmPerFuelType"];
            var fuelsPriceFor100KmRow = $("#petrolTypesPriceFor100Km");

            $("#petrolTypesHeader").children().each(function () {

                if (fuelsCosts[this.id] != null) {
                    fuelCostsRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>" + fuelsCosts[this.id].toFixed(2) + "</td>");
                } else {
                    if (this.id !== "") {
                        fuelCostsRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>0.00</td>");
                    }
                }

                if (fuelsCombustion[this.id] != null) {
                    fuelsCombustionRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>" + fuelsCombustion[this.id].toFixed(2) + "</td>");
                } else {
                    if (this.id !== "") {
                        fuelsCombustionRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>0.00</td>");
                    }
                }

                if (fuelsPriceFor100Km[this.id] != null) {
                    fuelsPriceFor100KmRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>" + fuelsPriceFor100Km[this.id].toFixed(2) + "</td>");
                } else {
                    if (this.id !== "") {
                        fuelsPriceFor100KmRow.append("<td class='table-warning text-center' id='costs_" + this.id + "'>0.00</td>");
                    }
                }

            });
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

    var urlParts = window.location.href.split("/");
    var vehicleId = urlParts[urlParts.length - 1];

    $.get("/api/refuelstats/GetVehicleFuelStats", { vehicleId: vehicleId, fuelType: fuelType }, function (data) {

        if (data["message"] == "ok") {

            getCombustionChart(data);
            getPriceChart(data);
        }
        else {
            alert("error: " + data["message"]);
        }

    });
}

function getCombustionChart(data) {
    while ($.combustionChart.data.labels.length > 0) {
        $.combustionChart.data.labels.pop();
    }

    $.combustionChart.data.datasets.forEach((dataset) => {
        dataset.data.pop();
    });

    for (var i = 0; i < data["refuelsDataForCharts"].length; i++) {
        $.combustionChart.data.labels.push(data["refuelsDataForCharts"][i]["refuelDate"].replace("T", "\r\n"));

        var dataset = $.combustionChart.data.datasets[0];
        dataset.data.push(data["refuelsDataForCharts"][i]["combustion"]);
    }

    $.combustionChart.update();
}

function getPriceChart(data) {
    while ($.priceChart.data.labels.length > 0) {
        $.priceChart.data.labels.pop();
    }

    $.priceChart.data.datasets.forEach((dataset) => {
        dataset.data.pop();
    });

    for (var i = 0; i < data["refuelsDataForCharts"].length; i++) {
        $.priceChart.data.labels.push(data["refuelsDataForCharts"][i]["refuelDate"].replace("T", "\r\n"));

        var dataset = $.priceChart.data.datasets[0];
        dataset.data.push(data["refuelsDataForCharts"][i]["price"]);
    }

    $.priceChart.update();
}