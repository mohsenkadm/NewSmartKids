const connection = new signalR.HubConnectionBuilder().withUrl("/Signalr")
    .configureLogging(signalR.LogLevel.Information).build();
connection.start({ withCredentials: false }).catch(err => console.error(err.toString()));
connection.on('OnGetMessage', data => {
    md.showNotification(data);
    RefreshChat();
});
connection.on('GetOrder', data => {
    md.showNotification(data);
    RefreshOrders();
}); 
const baseUrl = "/"; 
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


function RefreshChat() {

    call_ajax("GET", "Chat/GetMessageChatForWeb", null, GetMessagechat);
}

function Print(method, url, object) {
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
            md.showNotification('تم تصدير ملف طباعة');
        },
        error: (e) => {
            mouseevent("default");
            $('.progress').fadeOut();
            md.showNotification('حدث خطأ عند الأتصال');
        }
    });
}

function GetMessagechat(data) {
    if (data.length === 0) {
        toust.error("لا توجد رسائل الى الان");
        return;
    }

    var userName = "";
    var rows = "";
    $('#messges').empty();
    $.each(data, function (i, item) {
        userName = item.name;
        if (item.isOwner === false) {
            rows = "<div class=' l'>"
                + "<div class='dl col'   > <label style='color: #fff;'> " + item.messageText + "</label> </div>" +
                " <span style='float:left;'  class='date'  >" + item.date + "/تاريخ الارسال </span>"+
                "</div>" ;
        }
        else {
            rows = "<div class='row r' >" +
                " <div class='row dr'  > <label class='col' style='color: #fff;' >" + item.messageText + "</label> "+
            " <span class='date'   style = 'float:right;' > " + item.date + " / تاريخ الارسال </span >  </div > " +
                
                "</div>" ;
        }

        $('#messges').append(rows);
    });

    $('#userchatinfoid').empty();
    var rows1 = "<h3  >" + userName + " </h3>";
    $('#userchatinfoid').append(rows1);
    window.scrollTo(0, document.body.scrollHeight);
}


function GetMessageList() {
    call_ajax("GET", "Chat/GetMessageList", null, messageList);
};

// Beautiful Message List for Modal
function messageList(data) {
    const container = $('#messageList');
    
    // Clear loading state
    container.empty();
    
    // Check if no messages
    if (!data || data.length === 0) {
        const emptyHtml = `
            <div class="message-list-empty">
                <div class="message-list-empty-icon">
                    <i class="material-icons" style="font-size: 80px;">chat_bubble_outline</i>
                </div>
                <h3 class="message-list-empty-title">لا توجد محادثات</h3>
                <p class="message-list-empty-text">لم يتم العثور على أي محادثات حتى الآن</p>
            </div>
        `;
        container.html(emptyHtml);
        return;
    }
    
    // Render message list items
    data.forEach((item, index) => {
        const userName = item.name || 'مستخدم';
        const userInitial = userName.charAt(0).toUpperCase();
        const messagePreview = item.messageText ? 
            (item.messageText.length > 50 ? item.messageText.substring(0, 50) + '...' : item.messageText) : 
            'لا توجد رسائل';
        const timeAgo = formatMessageTime(item.date || item.dateShow);
        const userId = item.userReciverId || item.userId || 0;
        
        const itemHtml = `
            <div class="message-list-item" onclick="openChat(${userId})" style="animation-delay: ${index * 0.05}s">
                <div class="message-list-item-avatar">
                    ${userInitial}
                </div>
                <div class="message-list-item-content">
                    <div class="message-list-item-name">${escapeHtmlSafe(userName)}</div>
                    <div class="message-list-item-message">${escapeHtmlSafe(messagePreview)}</div>
                </div>
                <div class="message-list-item-time">
                    <i class="material-icons" style="font-size: 16px;">access_time</i>
                    <span>${timeAgo}</span>
                </div>
            </div>
        `;
        
        container.append(itemHtml);
    });
}

// Open chat for specific user
function openChat(userId) {
    $('#messagemodel').modal('hide');
    if (userId && userId > 0) {
        call_Action('Home/Chat?UserReciverId=' + userId);
    } else {
        call_Action('Home/Chat');
    }
}

// Format message time
function formatMessageTime(dateString) {
    if (!dateString) return '';
    
    try {
        const date = new Date(dateString);
        const now = new Date();
        const diff = now - date;
        const minutes = Math.floor(diff / 60000);
        const hours = Math.floor(diff / 3600000);
        const days = Math.floor(diff / 86400000);
        
        if (minutes < 1) return 'الآن';
        if (minutes < 60) return `منذ ${minutes} د`;
        if (hours < 24) return `منذ ${hours} س`;
        if (days < 7) return `منذ ${days} يوم`;
        
        return date.toLocaleDateString('ar-EG', { 
            month: 'short', 
            day: 'numeric' 
        });
    } catch (e) {
        return '';
    }
}

// Safe HTML escape for modal
function escapeHtmlSafe(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

