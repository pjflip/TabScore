var playerID = "";
var isSubmitted = false;

function addNumber(e) {
    if (playerID == "Unknown") {
        playerID = "";
    }
    playerID = playerID + e;
    document.getElementById('playerNumberBox').value = playerID;
    document.getElementById("OKButton").disabled = false;
}

function unknown() {
    playerID = "Unknown";
    document.getElementById('playerNumberBox').value = playerID;
    document.getElementById("OKButton").disabled = false;
}

function clearplayerNumber() {
    playerID = ""
    document.getElementById('playerNumberBox').value = "";
    document.getElementById("OKButton").disabled = true;
}

function clearLastEntry() {
    if (playerID == "Unknown") {
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

$(document).on('touchstart', '#OKButton:enabled', function () {
    if (playerID == "Unknown") playerID = "0";
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&playerID=' + playerID;
    }
});