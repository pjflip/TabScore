var suit = "";
var card = "";
var isSubmitted = false;

function onFullPageLoad() {
    if (leadCard == "SKIP") {
        suit = "SKIP";
        if (document.getElementById('skip')) {
            document.getElementById('skip').className = "btn btn-warning btn-lg m-1";
        }
        document.getElementById("OKButton").disabled = false;
    }
    else if (leadCard != "") {
        suit = leadCard.substr(0, 1);
        card = leadCard.substr(1, 1);
        document.getElementById('s' + suit).className = "btn btn-warning btn-lg m-1 p-1";
        document.getElementById('c' + card).className = "btn btn-warning btn-lg m-1";
        document.getElementById("OKButton").disabled = false;
    }
}

function setCard(c) {
    card = c;
    if (suit == "SKIP") suit = "";
    document.getElementById('cA').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cK').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cQ').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cJ').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cT').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c9').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c8').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c7').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c6').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c5').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c4').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c3').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c2').className = "btn btn-primary btn-lg m-1";
    if (document.getElementById('skip')) {
        document.getElementById('skip').className = "btn btn-outline-danger btn-lg m-1";
    }
    document.getElementById('c' + card).className = "btn btn-warning btn-lg m-1";
    if (suit == "") {
        document.getElementById("OKButton").disabled = true;
    }
    else {
        document.getElementById("OKButton").disabled = false;
    }
}

function setSuit(s) {
    suit = s;
    document.getElementById('sS').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sH').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sD').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sC').className = "btn btn-suit btn-lg m-1 p-1";
    if (document.getElementById('skip')) {
        document.getElementById('skip').className = "btn btn-outline-danger btn-lg m-1";
    }
    document.getElementById('s' + suit).className = "btn btn-warning btn-lg m-1 p-1";
    if (card == "") {
        document.getElementById("OKButton").disabled = true;
    }
    else {
        document.getElementById("OKButton").disabled = false;
    }
}

function setSkip() {
    suit = "SKIP";
    card = "";
    document.getElementById('cA').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cK').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cQ').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cJ').className = "btn btn-primary btn-lg m-1";
    document.getElementById('cT').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c9').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c8').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c7').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c6').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c5').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c4').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c3').className = "btn btn-primary btn-lg m-1";
    document.getElementById('c2').className = "btn btn-primary btn-lg m-1";
    document.getElementById('sS').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sH').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sD').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('sC').className = "btn btn-suit btn-lg m-1 p-1";
    document.getElementById('skip').className = "btn btn-warning btn-lg m-1";
    document.getElementById("OKButton").disabled = false;
}

$(document).on('click', '#OKButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&card=' + suit + card;
    }
});

$(document).on('click', '#BackButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlEnterContract;
    }
});