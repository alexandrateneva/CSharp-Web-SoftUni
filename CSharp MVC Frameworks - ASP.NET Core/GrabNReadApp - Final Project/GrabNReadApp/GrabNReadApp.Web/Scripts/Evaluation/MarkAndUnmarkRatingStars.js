function markStar(event) {
    var lastIndex = event.id;
    Array.from(document.getElementsByClassName("rating-star")).forEach(
        function (element, index, array) {
            if (element.id <= lastIndex) {
                element.classList.add("marked");
            }
        }
    );
}

function unmarkStar(event) {
    Array.from(document.getElementsByClassName("rating-star")).forEach(
        function (element, index, array) {
            element.classList.remove("marked");
        }
    );
}