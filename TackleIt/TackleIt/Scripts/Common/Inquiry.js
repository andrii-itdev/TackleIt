
var StarsNumb = 0;

function RateStars(N)
{
    CleanAll();
    for (var i = 1; i < N + 1; i++) {
        var item = document.getElementById(("star" + i.toString()));
        item.style.color = "Red";
    }
}

function CleanAll() {
    for (var i = 1; i < 6; i++) {
        document.getElementById("star" + i).style.color = "gold";
    }
}

function ResetStars()
{
    CleanAll();
    if (StarsNumb > 0) {
        RateStars(StarsNumb)
    }
}

function SetStars(N) {
    StarsNumb = N;
    var ans = document.getElementById("Answer");
    ans.setAttribute("value", 1.0 - N/5.0 + 0.2);
}

function btnYeah() {
    var ans = document.getElementById("Answer");
    ans.setAttribute("value", "0.0");
    $("#ansForm").submit();
}

function btnNope() {
    var ans = document.getElementById("Answer");
    ans.setAttribute("value", "1.0");
    $("#ansForm").submit();
}