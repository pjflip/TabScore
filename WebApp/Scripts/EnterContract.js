var isSubmitted = false;

function onFullPageLoad() {
    if (level == -1) {
        setSkip();
    }
    else if (level == 0) {
        setPass();
    }
    else if (level > 0) {
        document.getElementById('n' + level.toString()).className = "btn btn-lg btn-warning m-1";
        document.getElementById('s' + suit).className = "btn btn-lg btn-warning m-1 p-1";
        document.getElementById('d' + NSEW).className = "btn btn-lg btn-warning m-1";
        if (double == 'x') {
            document.getElementById('X').className = "btn btn-lg btn-warning m-1";
        }
        else if (double == 'xx') {
            document.getElementById('XX').className = "btn btn-lg btn-warning m-1";
        }
        if (suit != "" && NSEW != "") document.getElementById("OKButton").disabled = false;
    }
}

function clearNumbers() {
    document.getElementById('n1').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n2').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n3').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n4').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n5').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n6').className = "btn btn-lg btn-primary m-1";
    document.getElementById('n7').className = "btn btn-lg btn-primary m-1";
    document.getElementById("OKButton").disabled = true;
}

function clearSuits() {
    document.getElementById('sNT').className = "btn btn-lg btn-suit m-1 p-1";
    document.getElementById('sS').className = "btn btn-lg btn-suit m-1 p-1";
    document.getElementById('sH').className = "btn btn-lg btn-suit m-1 p-1";
    document.getElementById('sD').className = "btn btn-lg btn-suit m-1 p-1";
    document.getElementById('sC').className = "btn btn-lg btn-suit m-1 p-1";
    document.getElementById("OKButton").disabled = true;
}

function clearNSEWs() {
    document.getElementById('dN').className = "btn btn-lg btn-secondary m-1";
    document.getElementById('dS').className = "btn btn-lg btn-secondary m-1";
    document.getElementById('dE').className = "btn btn-lg btn-secondary m-1";
    document.getElementById('dW').className = "btn btn-lg btn-secondary m-1";
    document.getElementById("OKButton").disabled = true;
}

function setLevel(n) {
    level = n;
    document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
    document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
    clearNumbers();
    document.getElementById('n' + level.toString()).className = "btn btn-lg btn-warning m-1";
    if (suit != "" && NSEW != "") document.getElementById("OKButton").disabled = false;
}

function setSkip() {
    level = -1;
    suit = "";
    double = "";
    NSEW = "";
    document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
    clearNumbers();
    clearSuits();
    clearNSEWs();
    document.getElementById('X').className = "btn btn-lg btn-double m-1";
    document.getElementById('XX').className = "btn btn-lg btn-redouble m-1";
    document.getElementById('skip').className = "btn btn-lg btn-warning m-1";
    document.getElementById("OKButton").disabled = false;
}

function setPass() {
    level = 0;
    suit = "";
    double = "";
    NSEW = "";
    document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
    clearNumbers();
    clearSuits();
    clearNSEWs();
    document.getElementById('X').className = "btn btn-lg btn-double m-1";
    document.getElementById('XX').className = "btn btn-lg btn-redouble m-1";
    document.getElementById('pass').className = "btn btn-lg btn-warning m-1";
    document.getElementById("OKButton").disabled = false;
}

function setSuit(s) {
    suit = s;
    clearSuits();
    document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
    document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
    document.getElementById('s' + suit).className = "btn btn-lg btn-warning m-1 p-1";
    if (level > 0 && NSEW != "") document.getElementById("OKButton").disabled = false;
}

function setX() {
    if (double != 'x') {
        double = "x";
        document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
        document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
        document.getElementById('X').className = "btn btn-lg btn-warning m-1";
        document.getElementById('XX').className = "btn btn-lg btn-redouble m-1";
    }
    else {
        double = "";
        document.getElementById('X').className = "btn btn-lg btn-double m-1";
    }
}

function setXX() {
    if (double != 'xx') {
        double = "xx";
        document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
        document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
        document.getElementById('XX').className = "btn btn-lg btn-warning m-1";
        document.getElementById('X').className = "btn btn-lg btn-double m-1";
    }
    else {
        double = "";
        document.getElementById('XX').className = "btn btn-lg btn-redouble m-1";
    }
}

function setNSEW(d) {
    NSEW = d;
    clearNSEWs();
    document.getElementById('pass').className = "btn btn-lg btn-pass m-1";
    document.getElementById('skip').className = "btn btn-lg btn-outline-danger m-1";
    document.getElementById('d' + NSEW).className = "btn btn-lg btn-warning m-1";
    if (level > 0 && suit != "") document.getElementById("OKButton").disabled = false;
}

$(document).on('touchstart', '#OKButton:enabled', function () {
    if (level == 0) {
        if (!isSubmitted) {
            isSubmitted = true;
            location.href = urlOKButtonPass;
        }
    }
    else if (level == -1) {
        if (!isSubmitted) {
            isSubmitted = true;
            location.href = urlOKButtonSkip;
        }
    }
    else {
        if (!isSubmitted) {
            isSubmitted = true;
            location.href = urlOKButtonContract + '&contractLevel=' + level.toString() + '&contractSuit=' + suit + '&contractX=' + double + '&declarerNSEW=' + NSEW;
        }
    }
});

$(document).on('touchstart', '#BackButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlShowBoards;
    }
});