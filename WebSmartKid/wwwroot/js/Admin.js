
var _AdminId = 0;
function filltableAdmin(data) {
    $('#tableAdmin').empty();
    if (data.length === 0) {  
        md.showNotification('لا توجد معلومات');
        return;
    } 
    $.each(data, function (i, item) {
        var rows = "<tr>" +  
            "<td>" + item.password + "</td>" +   
            "<td>" + item.adminNo + "</td>" +   
            "<td>" + item.adminName + "</td>"
            + "<td> <button type='button' rel='tooltip' title='' class='btn btn-white btn-link btn-sm'  onclick='deleteAdmin(" + item.adminId + ")'  ><i class='material-icons'>delete</i> </button>" +
            "  |  <button type='button'  rel='tooltip' title='' class='btn btn-white btn-link btn-sm' onclick='updateAdmin(" + item.adminId + ")'  data-toggle='modal' data-target='#AdminModal'><i class='material-icons'>edit</i> </button>"+
        "  |  <button type='button'  rel='tooltip' title='' class='btn btn-white btn-link btn-sm' onclick='GetPermissionUser(" + item.adminId + ")'  data-toggle='modal' data-target='#PermissionModal'>صلاحية </button></td></tr>";
        $('#tableAdmin').append(rows); 
    });
}



function filllayout(data) {
    $('#nav').empty();
    $.each(data, function (i, item) {

        var rows = "";
        rows = "<li class='nav-item '>"+
            "<a class='nav-link' onclick =\"call_Action('Home/" + item.controlName + "')\");' >"+
              "<i class='material-icons'>person</i>"+
            "<p>"+item.permissionName +"</p>"+
            "</a>"+
          "</li>"; 
        $('#nav').append(rows);
    });

    var lastrow = "<li class='nav-item '>" +
                    "<a class='nav-link' onclick ='logut()' >" +
                    "<i class='material-icons'>person</i>" +
                    "<p>تسجيل خروج</p>" +
                    "</a>" +
                    "</li>"; 
    $('#nav').append(lastrow);
}


function GetPermissionUser(id) {
    var object = {
        UserId: id,
    };
    call_ajax("GET", "Admin/GetPermissionUser", object, filltablePermission);
}
function filltablePermission(data) {
    $('#tablePermission').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +
            "<td><input type='checkbox' id='state" + item.permissionId + "' onclick='changestate(this," + item.permissionId + ")' /></td>" +
            "<td>" + item.permissionName + "</td></tr>";
        $('#tablePermission').append(rows);
        $('#state' + item.permissionId).attr('checked', item.state);
    });
}

function changestate(state, id) {
    var object = {
        State: state.checked, PermissionId: id,
    };
    call_ajax("POST", "Admin/changestate", object, null);
}

function deleteAdmin(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Admin/Delete", object1, RefreshAdmin);
    }
}
function RefreshAdmin() {
    var obj = { Name:''}
    call_ajax("GET", "Admin/GetAll", obj, filltableAdmin);
}

function updateAdmin(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Admin/GetById", object1, setdataAdmin);
    _AdminId = id;
}

function setdataAdmin(data) {  
    $("#AdminName").val(data.adminName);   
    $("#AdminNo").val(data.adminNo);   
    $("#Password").val(data.password);  
    
}
function aftersaveAdmin() {
    $('#tableAdmin').empty();
     
    $("#AdminName").val(''); 
    $("#AdminNo").val(''); 
    $("#Password").val(''); 
    _AdminId = 0;
    RefreshAdmin();
}


function logut() {
    call_ajax("Get", "Admin/Logout", null, afterlogout);
};
function afterlogout() {
    document.cookie = "token2= ; expires = Thu, 01 Jan 1970 00:00:00 GMT";
    call_Action("Home/Login")

}