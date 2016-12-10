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
    var sortingBy = $('#sortingBy').val();
    var sortingOrder = $('#sortingOrder').val();

    $.ajax({
        type: 'GET',
        url: '/Article/GetArticles',
        data: { "sourceName": sourceName, "page": pageIndex, "sortingBy":sortingBy, "sortingOrder": sortingOrder },
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    $("#feed").append(                                                
                       "<div class='item border blur " + data[i].Read +
                       "'onclick=\"markArticleAsRead('" + data[i].Link +"',"+ data[i].Id + ",this);\">" +
                           "<div class='item-image center border-bottom'>" +
                               "<img src='" + data[i].ImageUrl + "'>" +
                            "</div>" +
                            "<div>" + 
                                "<div class='item-info border-bottom'>" +
                                    "<div class='item-date'>" +
                                        "<p>" + data[i].PublicationTime + "</p>" +
                                    "</div>" + 
                                    "<div class='item-source'>" + 
                                        "<p>" + data[i].SourceName + "</p>" +
                                    "</div>" + 
                                "</div>" + 
                                "<div class='item-description'>" + 
                                    "<h3>" + data[i].Title + "</h3>" +
                                    "<p>" + data[i].Description + "</p>" +
                                "</div>" + 
                            "</div>" + 
                        "</div>"                                                       
                    );
                }
                pageIndex++;
            }
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}