var isSubmitted = false;

function onFullPageLoad() {
    if (numTricks != '-1') {
        document.getElementById('n' + numTricks).className = "btn btn-warning btn-lg m-1 px-0";
        document.getElementById("OKButton").disabled = false;
    }
}

function setNumTricks(n) {
    numTricks = n;
    document.getElementById('n0').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n1').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n2').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n3').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n4').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n5').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n6').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n7').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n8').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n9').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n10').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n11').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n12').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n13').className = "btn btn-primary btn-lg m-1 px-0";
    document.getElementById('n' + numTricks).className = "btn btn-warning btn-lg m-1 px-0";
    document.getElementById("OKButton").disabled = false;
}

$(document).on('click', '#OKButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlOKButtonClick + '&numTricks=' + numTricks;
    }
});

$(document).on('click', '#BackButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlBackButtonClick;
    }
});