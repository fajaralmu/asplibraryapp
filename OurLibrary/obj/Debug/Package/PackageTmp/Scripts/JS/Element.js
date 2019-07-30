function ToggleFormAdd(id) {
    let element = document.getElementById(id);
    if (element.style.display == "block") {
        element.style.display ="none";
    } else {
        element.style.display = "block";
    }
}

function SetTextInput(input, textId) {
    let element = document.getElementById(textId);
    element.value = input;
}

function HideElement(id) {
    let element = document.getElementById(id);
    element.style.display = "none";
}

function ShowElement(id) {
    let element = document.getElementById(id);
    element.style.display = "block";
}

function serverClock() {

    timeReq("/Web/Api/Info", document.querySelectorAll('.clock')[0]);
}

function timeReq(url, clock_label) {
    var request = new XMLHttpRequest();
    request.open("POST", url, true);
    request.onreadystatechange = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            if (this.responseText != null) {
                let response_time = this.responseText;
                response_time = JSON.parse(response_time);
                clock_label.innerHTML = "Server Time:" + response_time.data;
            } else {
                clock_label.innerHTML = "Server Error";
            }

        } else {
            clock_label.innerHTML = "Server Error";
        }
    }
    request.send();
}
