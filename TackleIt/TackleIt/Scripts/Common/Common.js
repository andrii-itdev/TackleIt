
var menuHidden = true;

function MenuButtonClick() {
    if (menuHidden)
        RevealMenu();
    else HideMenu();
}

function HideMenu() {
    var menuobj = document.getElementById("menu");
    menuobj.style.setProperty("visibility", "hidden");

    var menubtn = document.getElementById("menuBtn");

    //menubtm.style.border.setProperty("bottom", "3px solid gray");
    menubtn.style.borderBottom = "3px solid gray";

    menubtn.style.setProperty("border-bottom-left-radius", "16px");
    menubtn.style.setProperty("border-bottom-right-radius", "16px");


    menuHidden = true;
}

function RevealMenu() {
    var menuobj = document.getElementById("menu");
    menuobj.style.setProperty("visibility", "visible");

    var menubtn = document.getElementById("menuBtn");

    //menubtm.style.border.setProperty("bottom", "0px solid gray");
    menubtn.style.borderBottom = "0px solid gray";

    menubtn.style.setProperty("border-bottom-left-radius", "0px");
    menubtn.style.setProperty("border-bottom-right-radius", "0px");

    //$("#menu").show("slow");

    menuHidden = false;
}


function SetResponseValue(value) {
    var ans = document.getElementById("Status");
    ans.setAttribute("value", value);
    $("#ansForm").submit();
}

function ChangeFormsAction() {
    var ans = document.getElementById("ansForm");
    ans.setAttribute("action", "/Proposition/PostOne");
}