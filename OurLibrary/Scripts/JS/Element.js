function ToggleFormAdd(id) {
    let element = document.getElementById(id);
    if (element.style.display == "block") {
        element.style.display ="none";
    } else {
        element.style.display = "block";
    }
}

function HideElement(id) {
    let element = document.getElementById(id);
    element.style.display = "none";
}

function ShowElement(id) {
    let element = document.getElementById(id);
    element.style.display = "block";
}