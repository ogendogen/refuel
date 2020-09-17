$(document).ready(function () {
    console.log("ready!");
});

function calculateValuesForUser() {
    var kilometers = $("input[name='Input.Kilometers']").val();
    var price = $("input[name='Input.PricePerLiter']").val();
    var liters = $("input[name='Input.Liters']").val();

    if (kilometers !== "" && price !== "" && liters !== "") {

        var d_kilometers = new Decimal(kilometers);
        var d_price = new Decimal(price);
        var d_liters = new Decimal(liters);

        var combustion = calculateCombustion(d_kilometers, d_liters);
        var totalPrice = calculateTotalPrice(d_price, d_liters);

        $("input[name='Input.Combustion']").val(combustion);
        $("input[name='Input.TotalPrice']").val(totalPrice);
    }
}

function calculateCombustion(d_kilometers, d_liters) {
    return d_liters.mul(100).div(d_kilometers).toDecimalPlaces(2).toString();
}

function calculateTotalPrice(d_price, d_liters) {
    return d_price.mul(d_liters).toDecimalPlaces(2).toString();
}