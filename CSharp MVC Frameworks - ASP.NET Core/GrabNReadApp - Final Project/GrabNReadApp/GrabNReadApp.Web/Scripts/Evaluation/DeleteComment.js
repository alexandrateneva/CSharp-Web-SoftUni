function deleteComment(e) {
    e.preventDefault();

    var commentId = event.target.id;
    var currentComment = event.target.parentNode.parentNode;

    $.ajax({
        type: 'POST',
        url: '/Evaluation/Comments/Delete',
        data: { commentId },
        success: function (response) {
            if (response.authorize === "Failed") {
                location.href = "/Identity/Account/Login";
            }
            else if (response.commentValidation === "Failed") {
                document.getElementById("validation").textContent = response.commentValidationMsg;
            }
            else {
                currentComment.style.display = 'none';
            }
        }
    });
}