function vote(event) {
    var voteValue = event.id;
    $.ajax({
        type: 'POST',
        url: '/Evaluation/Ratings/Vote',
        data: { bookId, voteValue },
        success: function (response) {
            if (response.authorize === "Failed") {
                location.href = "/Identity/Account/Login";
            }
            else if (response.voteValidation === "Failed") {
                document.getElementById("ratingValidation").textContent = "Your vote must have value from 1 to 5.";
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
        },
        error: function (response) {
            alert("Аn error occurred while rating a book.");
        }
    });
}