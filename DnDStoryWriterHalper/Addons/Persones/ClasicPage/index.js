window.onload = async function () {
    setIDs();
    await CefSharp.BindObjectAsync("dnd");

    window.setContent = (parameters) => {
        parameters = parameters.split("\'").join("\"");
        let a = JSON.parse(parameters);
        for (var k in a) {
            if(a[k] == 'on')
            document.getElementsByName(k)[0].checked = true
            document.getElementsByName(k)[0].value = a[k];
        }
    }
}

function send() {
    var a = new Object();
    Array.from((new FormData(document.getElementsByTagName("form")[0])).entries()).forEach(e => a[e[0]] = e[1])
    window.dnd.updateContent(JSON.stringify(a).split('\"').join("'"));
}



function setIDs() {
    let n = 0;
    Array.from(document.getElementsByTagName("input")).forEach(elem => elem.name = "i-" + (n++));
    Array.from(document.getElementsByTagName("textarea")).forEach(elem => elem.name = "t-" + (n++));

    Array.from(document.getElementsByTagName("input")).forEach(elem => elem.oninput = send);
    Array.from(document.getElementsByTagName("textarea")).forEach(elem => elem.oninput = send);
}
