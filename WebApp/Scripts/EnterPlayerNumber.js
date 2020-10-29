var playerNumber = "";
var isSubmitted = false;

function addNumber(e) {
    if (playerNumber == "Unknown") {
        playerNumber = "";
    }
    playerNumber = playerNumber + e;
    document.getElementById('playerNumberBox').value = playerNumber;
    document.getElementById("OKButton").disabled = false;
}

function unknown() {
    playerNumber = "Unknown";
    document.getElementById('playerNumberBox').value = playerNumber;
    document.getElementById("OKButton").disabled = false;
}

function clearplayerNumber() {
    playerNumber = ""
    document.getElementById('playerNumberBox').value = "";
    document.getElementById("OKButton").disabled = true;
}

function clearLastEntry() {
    if (playerNumber == "Unknown") {
        playerNumber = "";
        document.getElementById("OKButton").disabled = true;
    }
    else {
        if (playerNumber.length > 0) {
            playerNumber = playerNumber.substr(0, playerNumber.length - 1);
            if (playerNumber == "") document.getElementById("OKButton").disabled = true;
        }
    }
    document.getElementById('playerNumberBox').value = playerNumber;
}

$(document).on('touchstart', '#OKButton:enabled', function () {
    if (playerNumber == "Unknown") playerNumber = "0";
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&playerNumber=' + playerNumber;
    }
});