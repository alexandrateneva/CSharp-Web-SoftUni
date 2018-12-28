(function () {
    Array.from(document.getElementsByClassName("rating-star")).forEach(
        function (element, index, array) {
            if (element.id <= averageRating) {
                element.classList.add("average");
            }
            if (element.id <= userVote) {
                element.classList.add("selected");
            }
        }
    );
})();