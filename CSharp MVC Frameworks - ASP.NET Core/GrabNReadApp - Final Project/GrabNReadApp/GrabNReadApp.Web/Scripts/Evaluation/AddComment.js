function addComment(e) {
    e.preventDefault();

    var content = document.getElementById("content").value;
    var author = document.getElementById("username").value;
    var bookId = Number(document.getElementById("bookId").value);

    document.getElementById("validation").textContent = "";

    $.ajax({
        type: 'POST',
        url: '/Evaluation/Comments/Create',
        data: { content, bookId },
        success: function (response) {
            if (response.authorize === "Failed") {
                location.href = "/Identity/Account/Login";
            }
            else if (response.bookValidation === "Failed") {
                document.getElementById("validation").textContent = "There is no book with this id.";
            }
            else if (response.contentValidation === "Failed") {
                document.getElementById("validation").textContent = "Comment must contain between 5 and 500 symbols.";
            }
            else {
                var comment = '<blockquote class="blockquote">' +
                    `<p class="break-text mb-0">${content}` +
                    `<button id="${response.commentId}" onclick="deleteComment(event)" class="btn btn-sm btn-danger float-right">Delete</button>` +
                    '</p><footer class="blockquote-footer">by ' +
                    `<cite title="Comment Author">${author}</cite>` +
                    '</footer></blockquote>';

                var d = document.createElement('div');
                d.innerHTML = comment;

                var comments = document.getElementById("newComment");
                comments.insertBefore(d.firstChild, comments.firstChild);
            }
        },
        error: function (response) {
            alert("Аn error occurred while adding a comment.");
        }
    });

    document.getElementById("content").value = "";
}