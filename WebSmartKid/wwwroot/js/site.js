//const baseUrl = "/";
function call_ajax(method, url, object, call_back_func) {
    var userToken = getCookie("token2");
    mouseevent("progress");
    $('.progress').fadeIn();
    $.ajax({
        method: method,
        url: "/" + url,
        cache: true, async: true,
        data: object,
        headers: {
            'Authorization': `Bearer ${userToken}`,
        },
        success: (respons) => {
            //   /  debugger;
            mouseevent("default");
            $('.preloader').fadeOut();
            if (respons.success) {
                if (typeof (call_back_func) == 'function') {
                    if (respons.data != undefined)
                        call_back_func(respons.data);
                    else
                        call_back_func();
                }
                else if (typeof (call_back_func) == 'string'
                    && call_back_func == 'return') {
                    return respons.data;
                }
                if (respons.msg != null && respons.msg != undefined) 
                md.showNotification(respons.msg);
            }
            else {
                if (respons.msg != null && respons.msg != undefined) 
                    md.showNotification(respons.msg);
            }

        },
        error: (e) => {
            mouseevent("default");
            $('.progress').fadeOut();
            md.showNotification('حدث خطأ عند الأتصال');
        }
    });
} 

function call_Action(url) {
    var userToken = getCookie("token2");
    mouseevent("progress");
    $.ajax({
        method: 'GET',
        url: "/" + url,
        cache: true, async: true,
        headers: {
            'Authorization': `Bearer ${userToken}`,
        },
        success: (respons) => {
            $('.loader').fadeIn();
            mouseevent("default");
            var from = respons.indexOf("<!-- CUT FROM HERE -->");
            document.body.innerHTML = respons.substring(from, respons.length - 18);
            window.scrollTo(0, 0);
            var mydiv = document.getElementById("mydiv");
            var scripts = mydiv.getElementsByTagName("script");

            for (var i = 0; i < scripts.length; i++) { 
                eval(scripts[i].innerText);
            }

            call_ajax("GET", "Admin/GetPermissionForLayout", null, filllayout);
            $('.loader').fadeOut();
        }
    });

}
function mouseevent(type) {
    $("body").css("cursor", type);
    //type =progress ,default
}
function setCookie(cname, cvalue, exdays) {
    //debugger;
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    //  debugger;
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}