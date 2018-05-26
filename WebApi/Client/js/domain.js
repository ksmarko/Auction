function GetActiveTrades() {
    SendRequest("http://localhost:49351/api/profile/trades/active");
    $('#content-header').text("Действующие");
}

function GetWinLots() {
    SendRequest("http://localhost:49351/api/profile/trades/win");
    $('#content-header').text("Выиграшные");
}

function GetLoseLots() {
    SendRequest("http://localhost:49351/api/profile/trades/lose");
    $('#content-header').text("Завершенные");
}

function GetActiveLots() {
    SendRequest("http://localhost:49351/api/profile/lots/active");
    $('#content-header').text("В продаже");
}

function GetMyLots() {
    SendRequest("http://localhost:49351/api/profile/lots");
    $('#content-header').text("Мои лоты");
}

function SendRequest(url) {
    var tokenKey = "tokenInfo";
    $.ajax({
        type: 'GET',
        url: url,
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var text = "";
            $.each(data, function (key, item) {
                text += "<div class='search_lot' style='background: transparent url(" + "images/DDR2.jpg" + "); background-repeat: no-repeat'>" +
                    "<div class='search_lot_title' style='' onclick='GetLot(" + $(item)[0].Id + ")'><a href='#' title=" + $(item)[0].Name + ">" + $(item)[0].Name + "</a></div>" +
                    "<div class='search_lot_timetoend'><span><strong></strong><span class='toend'>До окончания: </span><strong>4 дн.</strong></span></div>" +
                    "<div class='search_lot_price'><b>" + $(item)[0].Price + "</b>грн.</div>" +
                    "</div ><br/><hr><br/>";
            });
            $("#lots-content").html(text);
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function GetLotsForCategory(id) {
    $.getJSON("http://localhost:49351/api/categories/" + id + "/lots")
        .done(function (data) {
            var text = "";
            if ($(data).length <= 0) {
                text = "<h3>No results<\/h3>";
            }
            else {
                $.each(data, function (key, item) {
                    text += "<div class='search_lot' style='background: transparent url(" + "images/DDR2.jpg" + "); background-repeat: no-repeat'>" +
                        "<div class='search_lot_title' style='' onclick='GetLot(" + $(item)[0].Id + ")'><a href='#' title=" + $(item)[0].Name + ">" + $(item)[0].Name + "<\/a><\/div>" +
                        "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>4 дн.<\/strong><\/span><\/div>" +
                        "<div class='search_lot_price'><b>" + $(item)[0].Price + "<\/b>грн.<\/div>" +
                        "<div class='search_lot_buynow'>купить сейчас<\/div><\/div ><br/><hr><br/>";
                });
            }
            $("#description").html(text);
        });
}

function GetLot(id) {
    $.getJSON("http://localhost:49351/api/lots/" + id)
        .done(function (data) {
            location.href = 'lot.html';
            $(document).getElementById("lot-name").val($(data).Name);
        });
}

function FilterLots() {
    $.getJSON("http://localhost:49351/api/lots/")
        .done(function (data) {
            var text = "";
            if ($(data).length <= 0) {
                text = "<h3>No results<\/h3>";
            }
            else {
                var from = $("#price-from").val() ? $("#price-from").val() : 0;
                var to = $("#price-to").val() ? $("#price-to").val() : 9999999999999;
                $.each(data, function (key, item) {
                    if ($(item)[0].Price >= from && $(item)[0].Price <= to) {
                        text += "<div class='search_lot' style='background: transparent url(" + "images/DDR2.jpg" + "); background-repeat: no-repeat'>" +
                            "<div class='search_lot_title' style='' onclick='GetLot(" + $(item)[0].Id + ")'><a href='#' title=" + $(item)[0].Name + ">" + $(item)[0].Name + "<\/a><\/div>" +
                            "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>4 дн.<\/strong><\/span><\/div>" +
                            "<div class='search_lot_price'><b>" + $(item)[0].Price + "<\/b>грн.<\/div>" +
                            "<div class='search_lot_buynow'>купить сейчас<\/div><\/div ><br/><hr><br/>";
                    }
                });
            }
            $("#description").html(text);
        });
}

function SearchLots() {
    $.getJSON("http://localhost:49351/api/lots/")
        .done(function (data) {
            var text = "";
            if ($(data).length <= 0) {
                text = "<h3>No results<\/h3>";
            }
            else {
                var term = $("#search-lots").val().toLowerCase();
                $.each(data, function (key, item) {
                    if ($(item)[0].Name.toLowerCase().includes(term) || $(item)[0].Description.toLowerCase().includes(term)) {
                        text += "<div class='search_lot' style='background: transparent url(" + "images/DDR2.jpg" + "); background-repeat: no-repeat'>" +
                            "<div class='search_lot_title' style='' onclick='GetLot(" + $(item)[0].Id + ")'><a href='#' title=" + $(item)[0].Name + ">" + $(item)[0].Name + "<\/a><\/div>" +
                            "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>4 дн.<\/strong><\/span><\/div>" +
                            "<div class='search_lot_price'><b>" + $(item)[0].Price + "<\/b>грн.<\/div>" +
                            "<div class='search_lot_buynow'>купить сейчас<\/div><\/div ><br/><hr><br/>";
                    }
                });
            }
            if ($(text).length <= 2) {
                text = "<h3>No results<\/h3>";
            }
            $("#description").html(text);
        });
}

function GetMainLots() {
    $.getJSON("http://localhost:49351/api/lots")
        .done(function (data) {
            var text = "";
            if ($(data).length <= 0) {
                text = "<h3>No results<\/h3>";
            }
            else {
                var counter = 0;
                $.each(data, function (key, item) {
                    if (counter >= 10) {
                        return;
                    }
                    counter++;
                    text += "<div class='search_lot' style='background: transparent url(" + "images/DDR2.jpg" + "); background-repeat: no-repeat'>" +
                        "<div class='search_lot_title' style='' onclick='GetLot(" + $(item)[0].Id + ")'><a href='#' title=" + $(item)[0].Name + ">" + $(item)[0].Name + "<\/a><\/div>" +
                        "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>4 дн.<\/strong><\/span><\/div>" +
                        "<div class='search_lot_price'><b>" + $(item)[0].Price + "<\/b>грн.<\/div>" +
                        "<div class='search_lot_buynow'>купить сейчас<\/div><\/div ><br/><hr><br/>";
                });
            }
            $("#description").html(text);
        });
}

function GetMainCats() {
    $.getJSON("http://localhost:49351/api/home")
        .done(function (data) {
            var text = "";
            $.each(data, function (key, item) {
                text += "<div onclick='GetLotsForCategory(" + $(item)[0].Id + ")' style='display: inline-block;cursor: pointer; width: 200px; padding-right:10px;'><a class='sp_href cat-name' style= '' >" + $(item)[0].Name + "<\/a ><\/div > <br style='line-height:25px;'>";
            });
            $("#cats-list-my").html(text);
        });
}