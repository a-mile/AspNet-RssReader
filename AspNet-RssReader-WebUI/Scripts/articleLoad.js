var pageSize = 9;
var pageIndex = 1;

$(document).ready(function () {
    GetArticles();

    $(window).scroll(function () {
        if ($(window).scrollTop() ==
           $(document).height() - $(window).height()) {
            GetArticles();
        }
    });
});

function GetArticles() {
    $.ajax({
        type: 'GET',
        url: '/Article/Articles',
        data: {"page": pageIndex},
        dataType: 'html',
        success: function (data) {
            if (data != null) {
                $("#content").append(data);
                pageIndex++;
            }
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}