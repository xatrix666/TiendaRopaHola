document.title = "TiendaRopa API";

$(function () {
    var interval = setInterval(function () {
        var jsonLink = $('a[href="/swagger/v1/swagger.json"] .url');
        if (jsonLink.length > 0) {            
            jsonLink.text("JSON - TiendaRopaHola").css({
                "color": "#FFA500",
                "font-family": "Comic Sans MS, cursive, sans-serif",
                "font-size": "12px"
            });
            clearInterval(interval);
        }
    }, 1);
});
