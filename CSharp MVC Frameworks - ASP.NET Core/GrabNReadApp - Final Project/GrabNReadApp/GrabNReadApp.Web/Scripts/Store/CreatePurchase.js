countValidation = (price) => {
    $(document).ready(function () {
        $('#bookCount').on('input', function () {
            var bookCount = $("#bookCount").val();
            var totalSum = price * bookCount;
            if (bookCount <= 0) {
                totalSum = 0;
            }
            else if (bookCount > 20) {
                totalSum = price * 20;
            }

            $("#totalSum").text(totalSum.toFixed(2) + ' BGN');
        });
    });
}
