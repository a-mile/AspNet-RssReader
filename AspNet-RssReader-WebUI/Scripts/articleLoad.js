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
    var sourceName = $('#sourceName').val();
    var sortOrder = $('#sortOrder').val();

    $.ajax({
        type: 'GET',
        url: '/Article/GetArticles',
        data: { "sourceName": sourceName, "page": pageIndex, "sortOrder": sortOrder },
        dataType: 'html',
        success: function (data) {
            if (data != null) {
                $("#feed").append(data);
                pageIndex++;
            }
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}