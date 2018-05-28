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
                text = AddLotContent(text, $(item)[0].Id, $(item)[0].Name, $(item)[0].Price);
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
                    text = AddLotContent(text, $(item)[0].Id, $(item)[0].Name, $(item)[0].Price);
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
                        text = AddLotContent(text, $(item)[0].Id, $(item)[0].Name, $(item)[0].Price);
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
                        text = AddLotContent(text, $(item)[0].Id, $(item)[0].Name, $(item)[0].Price);
                    }
                });
            }
            if ($(text).length <= 2) {
                text = "<h3>No results<\/h3>";
            }
            $("#description").html(text);
        });
}

function AddLotContent(text, id, name, price) {
    text += "<div class='search_lot' style='background: transparent url(" + "img/nophoto.png" + "); background-repeat: no-repeat; margin-left: 30px'>" +
        "<div class='search_lot_title' style='' onclick='GetLot(" + id + ")'><a href='#' title=" + name + ">" + name + "<\/a><\/div>" +
        "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>4 дн.<\/strong><\/span><\/div>" +
        "<div class='search_lot_price'><b>" + price + "<\/b>грн.<\/div>" +
        "<div class='search_lot_buynow'>купить сейчас<\/div>"/* + "<button onclick='RemoveLot(" + id + ")'>Remove</button>"*/ + "<\/div ><br/><hr><br/>";

    return text;
}

function AddTradeContent(text, id, name, price, days) {
    text += "<div class='search_lot' style='background: transparent url(" + "img/nophoto.png" + "); background-repeat: no-repeat; margin-left: 30px'>" +
        "<div class='search_lot_title' style='' onclick='GetLot(" + id + ")'><a href='#' title=" + name + ">" + name + "<\/a><\/div>" +
        "<div class='search_lot_timetoend'><span><strong><\/strong><span class='toend'>До окончания: <\/span><strong>" + days + " дн.<\/strong><\/span><\/div>" +
        "<div class='search_lot_price'><b>" + price + "<\/b> грн.<\/div>" +
        "<\/div ><br/><hr><br/>";

    return text;
}

function RemoveLot(id) {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'DELETE',
        url: "http://localhost:49351/api/lots/remove/" + id,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            alert(data);
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function GetMainLots() {
    $.getJSON("http://localhost:49351/api/trades")
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
                    text = AddTradeContent(text, $(item)[0].Lot.Id, $(item)[0].Lot.Name, $(item)[0].Lot.Price, $(item)[0].DaysLeft);
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

function LoadOptions() {
    $.getJSON("http://localhost:49351/api/home")
        .done(function (data) {
            var text = "";
            $.each(data, function (key, item) {
                text += "<option value=" + $(item)[0].Id + ">" + $(item)[0].Name + "</option >";
            });
            $("#cat0").html(text);
        });
}

function AddLot() {
    var tokenKey = "tokenInfo";
    var data = {
        Name: $('#lot-name').val(),
        Description: $('#lot-descr').val(),
        Price: $('#lot-price').val(),
        TradeDuration: $('#trade-duration').val(),
        Category: $('#cat0').val()
    };

    $.ajax({
        type: 'POST',
        url: "http://localhost:49351/api/lots/create",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            alert(data);
            location.href = 'onsale.html';
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function GetUserRole() {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'GET',
        url: "http://localhost:49351/api/users/current",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var text = "<ul>";

            if (data == "admin") {
                text += "<li><a href='users.html'> Управление пользователями</a ></li><li style='list-style: none; display: inline'><div class='arrow'></div></li>";
            }
            if (data == "moderator" || data == "admin") {
                text += "<li><a href='lotsmanagement.html'> Управление лотами</a></li><li style='list-style: none; display: inline'><div class='arrow'></div></li>";
            }

            text += "<li><a href='purchase.html'> Мой кабинет</a></li><li style='list-style: none; display: inline'><div class='arrow'></div></li>";
            text += "<li><a onclick='Logout()' href='login.html'> Выход</a></li></ul>";

            $("#menu-btns").html(text);
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function FillUsers() {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'GET',
        url: "http://localhost:49351/api/users",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var text = "<table class='table'><tr><th>E-mail</th><th>Role</th><th>Set role</th><th></th></tr>";

            $.each(data, function (key, item) {
                text += "<tr><td>" + $(item)[0].Name + "</td><td>" + $(item)[0].Role + "</td><form method=POST><td><select class='form-control user-role' id=" + $(item)[0].Id + ">";
                text += "</select></td><td><button type='submit' onclick='EditRoles(" + '"' + $(item)[0].Id + '"' + ")' class='btn btn-success'>Save</button></td></form></tr>";
            });

            text += "</table>";
            $("#content-users").html(text);
            GetRoles();
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function EditRoles(id) {
    var tokenKey = "tokenInfo";
    var data = {
        UserId: id,
        Role: $("#" + id).val()
    };

    $.ajax({
        type: 'POST',
        url: "http://localhost:49351/api/users/edit",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            FillUsers();
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function GetRoles() {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'GET',
        url: "http://localhost:49351/api/roles",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var text = "";

            $.each(data, function (key, item) {
                text += "<option>" + item + "</option>";
            });

            $(".user-role").html(text);
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function GetNonverifiedLots() {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'GET',
        url: "http://localhost:49351/api/lots",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var text = "";

            $.each(data, function (key, item) {
                if (!$(item)[0].IsVerified) {
                    text = AddLotFullContent(text, $(item)[0].Id, $(item)[0].Name, $(item)[0].Price, $(item)[0].Description, $(item)[0].Category, $(item)[0].Creator);
                }
            });

            $("#content-lots-mng").html(text);
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function AddLotFullContent(text, id, name, price, description, category, user) {
    text += "<div class='search_lot' style='background: transparent url(" + "img/nophoto.png" + "); background-repeat: no-repeat; margin-left: 30px'>" +
        "<div class='search_lot_title' style=''><a href='#' title=" + name + ">" + name + "<\/a><\/div>" +
        "<div class='search_lot_price'><b>" + price + "<\/b>грн.<\/div>" +
        "<div style='margin-left: 215px; margin-top: 50px;'>" + 'Category: ' + category + "<\/b><\/div>" +
        "<div style='margin-left: 215px; margin-top: 20px;'>" + 'Creator: ' + user.substring(0, user.indexOf("@")) + "<\/b><\/div>" +
        "<div style='margin-left: 215px; margin-top: 20px; width: 700px;'>" + description + "<\/b><\/div>" +
        "<input onclick='VerifyLot(" + id + ")' style='margin-left: 1000px; margin-top: 0px;' class='green_button' type='submit' value='Verify and start'>" + 
        "<\/div><br/><hr><br/>";

    return text;
}

function VerifyLot(id) {
    var tokenKey = "tokenInfo";

    $.ajax({
        type: 'PUT',
        url: "http://localhost:49351/api/lots/" + id + "/verify",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            alert(data);
            GetNonverifiedLots();
        },
        fail: function (data) {
            alert(data);
        }
    });
}

function Logout() {
    sessionStorage.removeItem("tokenInfo");
    $.ajax({
        type: 'POST',
        url: "http://localhost:49351/api/Account/Logout",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        fail: function (data) {
        }
    });
}

function Authorize() {
    var text = "<ul>";
    if (sessionStorage.getItem("tokenInfo")) {

        text += "<li><a href='purchase.html'> Мой кабинет</a></li><li style='list-style: none; display: inline'><div class='arrow'></div></li>";
        text += "<li><a onclick='Logout()' href='index.html'> Выход</a></li></ul>";

    }
    else {
        text += "<li><a href='login.html'> Войти</a></li><li style='list-style: none; display: inline'><div class='arrow'></div></li>";
        text += "<li><a href='register.html'> Регистрация</a></li></ul>";
    }
    $("#menu-btns-main").html(text);
}

function showPass() {
    var type = $('.pass').attr('type') == "text" ? "password" : 'text';
    $('.pass').prop('type', type);
}

function passCheck() {
    $.getJSON('check_account.html', { pass: $('#pass').val(), login: $('#user_login').val(), email: $('#user_email').val() }, function (list) {
        var valid = ""
        $.each(list, function (i) {
            valid = this
        });
        if (valid == 0) {
            $('#pass').css('border-bottom', '3px solid red')
            $('#pchv_error').show()
        } else {
            $('#pass').css('border-bottom', '1px solid #d0d0d0')
            $('#pchv_error').hide()
        }
    });
};

function check_email() {
    if (!$('#user_email').val().match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/g)) {
        $('#user_email').css('border-bottom', '3px solid red')
        $('#ech_error').show()
        return false;
    } else {
        $('#user_email').css('border-bottom', '1px solid #d0d0d0')
        $('#ech_error').hide()
        return true;
    }
}

function check_pass() {
    if (!$('#pass').val().match(/^(?=.*\d)(?=.*[a-zA-Z]).{6,32}$/g)) {
        $('#pass').css('border-bottom', '3px solid red')
        $('#pch_error').show()
    } else {
        $('#pass').css('border-bottom', '1px solid #d0d0d0')
        $('#pch_error').hide()
        passCheck()
    }
}

function check_pass2() {
    if ($('#pass2').val() != $('#pass').val()) {
        $('#pass2').css('border-bottom', '3px solid red')
        $('#ppch_error').show()
    } else {
        $('#pass2').css('border-bottom', '1px solid #d0d0d0')
        $('#ppch_error').hide()
        passCheck()
    }
}