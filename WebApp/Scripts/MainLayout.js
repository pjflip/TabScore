$(function () {
    $("body").css("padding-top", $("#headerRow").height().toString() + "px");
    $("body").css("padding-bottom", ($("#footerRow").height() + 10).toString() + "px");
    if ("getBattery" in navigator) {
        navigator.getBattery().then(function (battery) {
            $("#battery").css("display", "block");
            $("#header").attr("class", "col-10 my-auto pr-0");
            var batteryLevel = battery.level * 100;
            if (batteryLevel > 87.5) {
                $("#bl").attr("class", "fa fa-battery-full");
            }
            else if (batteryLevel > 62.5) {
                $("#bl").attr("class", "fa fa-battery-three-quarters");
            }
            else if (batteryLevel > 37.5) {
                $("#bl").attr("class", "fa fa-battery-half");
            }
            else if (batteryLevel > 12.5) {
                $("#bl").attr("class", "fa fa-battery-quarter");
            }
            else {
                $("#bl").attr("class", "fa fa-battery-empty");
            }
        });
    }
    setTimerValue(timerSeconds);
    if (timerSeconds >= 0) startTimer(timerSeconds);
    if (typeof onFullPageLoad == 'function') {
        onFullPageLoad();
    }
});

function startTimer(timeSeconds) {
    var timerInterval = setInterval(function () {
        setTimerValue(--timeSeconds);
        if (timeSeconds <= 0) clearInterval(timerInterval);
    }, 1000);
}

function setTimerValue(timeSeconds) {
    if (timeSeconds <= 0) {
        $("#timerValue").text(" 00:00");
        $("#timerButton").attr("class", "btn btn-danger my-2");
    }
    else {
        minutes = parseInt(timeSeconds / 60, 10);
        seconds = parseInt(timeSeconds % 60, 10);
        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;
        $("#timerValue").text(" " + minutes + ":" + seconds);
        if (timeSeconds <= 60) {
            $("#timerButton").attr("class", "btn btn-warning my-2");
        }
    }
}
