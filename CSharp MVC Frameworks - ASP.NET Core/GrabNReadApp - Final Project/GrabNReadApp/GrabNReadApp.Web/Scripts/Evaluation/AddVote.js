function vote(event) {
    var validationToken = document.getElementsByName("__RequestVerificationToken")[0].value;
    var voteValue = event.id;

    $.ajax({
        type: 'POST',
        url: '/Evaluation/Ratings/Vote',
        data: { bookId, voteValue, __RequestVerificationToken: validationToken },
        success: function (response) {
            if (response.authorize === "Failed") {
                location.href = "/Identity/Account/Login";
            }
            else if (response.bookValidation === "Failed") {
                document.getElementById("ratingValidation").textContent = response.bookValidationMsg;
            }
            else if (response.voteValidation === "Failed") {
                document.getElementById("ratingValidation").textContent = response.voteValidationMsg;
            }
            else {
                var lastIndex = event.id;
                var averageRating = response.averageRating;
                Array.from(document.getElementsByClassName("rating-star")).forEach(
                    function (element, index, array) {
                        if (element.id <= lastIndex) {
                            element.classList.add("selected");
                        } else {
                            element.classList.remove("selected");
                        }

                        if (element.id <= averageRating) {
                            element.classList.add("average");
                        } else {
                            element.classList.remove("average");
                        }
                    }
                );
            }
        }
    });
}