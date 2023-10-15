var playerID = "";
var isSubmitted = false;

function addNumber(e) {
    if (playerID == stringUnknown) {
        playerID = "";
    }
    playerID = playerID + e;
    document.getElementById('playerNumberBox').value = playerID;
    document.getElementById("OKButton").disabled = false;
}

function unknown() {
    playerID = stringUnknown;
    document.getElementById('playerNumberBox').value = playerID;
    document.getElementById("OKButton").disabled = false;
}

function clearplayerNumber() {
    playerID = ""
    document.getElementById('playerNumberBox').value = "";
    document.getElementById("OKButton").disabled = true;
}

function clearLastEntry() {
    if (playerID == stringUnknown) {
        playerID = "";
        document.getElementById("OKButton").disabled = true;
    }
    else {
        if (playerID.length > 0) {
            playerID = playerID.substr(0, playerID.length - 1);
            if (playerID == "") document.getElementById("OKButton").disabled = true;
        }
    }
    document.getElementById('playerNumberBox').value = playerID;
}

$(document).on('click', '#OKButton:enabled', function () {
    if (playerID == stringUnknown) playerID = "0";
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&playerID=' + playerID;
    }
});