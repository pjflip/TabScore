﻿var isSubmitted = false;

function onFullPageLoad() {
    if (numTricks != -1) {
        var diffTricks = numTricks - contractLevel - 6;
        if (diffTricks < 0) {
            document.getElementById('minus' + (-diffTricks).toString()).className = "btn btn-warning btn-lg m-1 px-0";
        }
        else if (diffTricks == 0) {
            document.getElementById('equals').className = "btn btn-warning btn-lg m-1 px-0";
        }
        else {
            document.getElementById('plus' + diffTricks.toString()).className = "btn btn-warning btn-lg m-1 px-0";
        }
        document.getElementById("OKButton").disabled = false;
    }
}

function resetButtons() {
    for (var i = 1; i < contractLevel + 7; i++) {
        document.getElementById('minus' + i.toString()).className = "btn btn-minus btn-lg m-1 px-0";
    }
    document.getElementById('equals').className = "btn btn-primary btn-lg m-1 px-0";
    for (var i = 1; i < 8 - contractLevel; i++) {
        document.getElementById('plus' + i.toString()).className = "btn btn-plus btn-lg m-1 px-0";
    }
}

function setMinusTricks(n) {
    numTricks = contractLevel + 6 - n;
    resetButtons();
    document.getElementById('minus' + n.toString()).className = "btn btn-warning btn-lg m-1 px-0";
    document.getElementById("OKButton").disabled = false;
}

function setPlusTricks(n) {
    numTricks = contractLevel + 6 + n;
    resetButtons();
    document.getElementById('plus' + n.toString()).className = "btn btn-warning btn-lg m-1 px-0";
    document.getElementById("OKButton").disabled = false;
}

function setEqualsTricks() {
    numTricks = contractLevel + 6;
    resetButtons();
    document.getElementById('equals').className = "btn btn-warning btn-lg m-1 px-0";
    document.getElementById("OKButton").disabled = false;
}

$(document).on('touchstart click', '#OKButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&numTricks=' + numTricks;
    }
});

$(document).on('touchstart click', '#BackButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlBackButtonClick;
    }
});