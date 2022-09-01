
var _PersonId = 0;
function filltableUsers(data) {
    $('#tableUsers').empty();
    if (data.length === 0) {  
        md.showNotification('لا توجد معلومات');
        return;
    } 
    $.each(data, function (i, item) {
        var rows = "<tr>" + 
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsDelete" + item.personId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" +
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsActive" + item.personId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" +
            "<td>" + item.password + "</td>" +   
            "<td>" + item.phone + "</td>" +   
            "<td>" + item.name + "</td>"
            + "<td> <button type='button' rel='tooltip' title='' class='btn btn-white btn-link btn-sm'  onclick='deleteUsers(" + item.personId + ")'  ><i class='material-icons'>delete</i> </button>" +
            "  |  <button type='button'  rel='tooltip' title='' class='btn btn-white btn-link btn-sm' onclick='updateUsers(" + item.personId + ")'  data-toggle='modal' data-target='#UsersModal'><i class='material-icons'>edit</i> </button></td></tr>";
        $('#tableUsers').append(rows);
        $('#IsActive' + item.personId).attr('checked', item.isActive);
        $('#IsDelete' + item.personId).attr('checked', item.isDeleted);
    });
}

function deleteUsers(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Persons/Delete", object1, RefreshUsers);
    }
}
function RefreshUsers() {
    var obj = { Name:''}
    call_ajax("GET", "Persons/GetUsersAll", obj, filltableUsers);
}

function updateUsers(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Persons/GetById", object1, setdataUsers);
    _PersonId = id;
}

function setdataUsers(data) {  
    $("#Name").val(data.name);   
    $("#Phone").val(data.phone);   
    $("#Password").val(data.password);  
    if (data.isActive === true) {
        $("#IsActive").prop("checked", true);
    }
    else {
        $("#IsActive").prop("checked", false);
    }
    if (data.isDeleted === true) {
        $("#IsDeleted").prop("checked", true);
    }
    else {
        $("#IsDeleted").prop("checked", false);
    }  
}
function aftersaveUsers() {
    $('#tableUsers').empty();
     
    $("#Name").val('');
    $("#Email").val('');
    $("#Phone").val('');  
    $("#Password").val('');
    $("#IsActive").prop("checked", true);
    $("#IsDeleted").prop("checked", true);
    _PersonId = 0;
    RefreshUsers();
}