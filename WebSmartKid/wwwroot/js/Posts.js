
var _PostId = 0;
function filltablePosts(data) {
    $('#tablePosts').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +   
            "<td>" + item.url + "</td>"+
            "<td>" + item.title + "</td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deletePosts(" + item.postId + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updatePosts(" + item.postId + ")'  data-toggle='modal' data-target='#PostsModal'>تعديل</button></td></tr>";
        $('#tablePosts').append(rows); 
    });
}

function deletePosts(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Posts/Delete", object1, RefreshPosts);
    }
}
function RefreshPosts() {
    call_ajax("GET", "Posts/GetAll", null, filltablePosts);
}

function updatePosts(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Posts/GetById", object1, setdataPosts);
    _PostId = id;
}

function setdataPosts(data) {
    $("#Url").val(data.url);
    $("#Title").val(data.title);
}
function aftersavePosts() { 
    $('#tablePosts').empty();
    $("#Url").val(''); 
    $("#Title").val(''); 
    _PostId = 0;
    call_ajax("GET", "Posts/GetAll", null, filltablePosts);
}