var isSubmitted = false;

var pollRanking = new XMLHttpRequest();
pollRanking.onload = pollRankingListener;
setTimeout(function () {
    pollRanking.open('get', urlPollRanking, true);
    pollRanking.send();
}, 10000);

function pollRankingListener() {
    rankingList = JSON.parse(this.responseText);
    var new_tbody = document.createElement("tbody");
    for (var i = 0; i < rankingList.length; i++) {
        var row = new_tbody.insertRow(i);
        if (rankingList[i].PairNo == pairNS) row.className = "table-success";
        if (rankingList[i].PairNo == pairEW) row.className = "table-warning";
        var cellRank = row.insertCell(0);
        var cellPairNumber = row.insertCell(1);
        var cellScore = row.insertCell(2);
        cellRank.innerHTML = rankingList[i].Rank;
        cellPairNumber.innerHTML = rankingList[i].PairNo;
        cellScore.innerHTML = rankingList[i].Score + "%";
    }
    var old_tbody = document.getElementById("tableBody");
    old_tbody.parentNode.replaceChild(new_tbody, old_tbody);
    new_tbody.id = "tableBody";
    setTimeout(function () {
        pollRanking.open('get', urlPollRanking, true);
        pollRanking.send();
    }, 10000);
}

$(document).on('click', '#OKButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        if (finalRankingList) {
            location.href = urlEndScreen;
        }
        else {
            location.href = urlShowMove;
        }
    }
});

$(document).on('click', '#BackButton:enabled', function () {
    if (!isSubmitted) {
        isSubmitted = true;
        location.href = urlShowBoards;
    }
});