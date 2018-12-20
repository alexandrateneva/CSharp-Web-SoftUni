showFinalSumWithDelivery = (totalSum) => {
    $(document).ready(function () {
        $('input[name=delivery]').change(function () {
            var value = $('input[name=delivery]:checked').val();
            var finalSum = totalSum + Number(value);
            $("#totalSum").text(finalSum.toFixed(2) + ' BGN');
        });
    });
}
