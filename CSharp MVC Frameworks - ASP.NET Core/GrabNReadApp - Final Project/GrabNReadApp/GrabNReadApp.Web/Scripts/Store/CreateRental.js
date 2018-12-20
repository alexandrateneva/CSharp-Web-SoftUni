dateValidation = (pricePerDay) => {
    $(document).ready(function () {
        $(function () {
            var today = new Date();
            var tomorrow = new Date();
            tomorrow.setDate(today.getDate() + 1);

            $('#startDate').prop('min',
                function () {
                    return today.toJSON().split('T')[0];
                });
            $('#endDate').prop('min',
                function () {
                    return tomorrow.toJSON().split('T')[0];
                });
        });

        $('.dateRent').on('input', function () {
            var startDate = $('#startDate').val();
            var endDate = $('#endDate').val();

            var date1 = new Date(startDate);
            var date2 = new Date(endDate);
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
            var totalSum = 0;
            if (startDate < endDate) {
                totalSum = pricePerDay * diffDays;
                if (diffDays > 30) {
                    totalSum = pricePerDay * 30;
                    $("#dateValidation").text('You can rent the book for a maximum period of 30 days.');
                }
                else {
                    $("#dateValidation").text('');
                }
            }
            else {
                $("#dateValidation").text('End date must be greater than the start date.');
            }
            $("#totalSum").text(totalSum.toFixed(2) + ' BGN');
        });
    });
}
