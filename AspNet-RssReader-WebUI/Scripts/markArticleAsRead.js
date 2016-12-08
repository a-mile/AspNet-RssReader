function markArticleAsRead(link, id, item) {
    $.ajax({
        type: 'GET',
        url: '/Article/MarkArticleAsRead',
        data: { "articleId": id},
        dataType: 'text',
        success: function() {
            $(item).addClass("read");
        },
        error: function () {
            alert("Error while marking as read!");
        }
    });
    window.open(link, '_blank');
}