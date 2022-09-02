
var _categoriesId = 0;
function filltableCategories(data) {
    $('#tableCategories').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" + 
            "<td><img src='" + item.image + "' alt='' border=3 height=50 width=50></img></td>" +
            "<td>" + item.categoriesName + "</td>" 
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteCategories(" + item.categoriesId + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updateCategories(" + item.categoriesId + ")'  data-toggle='modal' data-target='#CategoriesModal'>تعديل</button></td></tr>";
        $('#tableCategories').append(rows); 
    });
}

function deleteCategories(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Categories/Delete", object1, RefreshCategories);
    }
}
function RefreshCategories() {
    call_ajax("GET", "Categories/GetAll", null, filltableCategories);
}

function updateCategories(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Categories/GetById", object1, setdataCategories);
    _categoriesId = id;
}

function setdataCategories(data) {
    $("#CategoriesName").val(data.categoriesName);
}
function aftersaveCategories(Categories) {
    var fileUpload = $("#imageca").get(0);
    var files = fileUpload.files;
    var data = new FormData();
    if (files.length > 0) {
        data.append(files[0].name, files[0]);

        var userToken = getCookie("token2");
        $.ajax({
            type: "POST", url: "/Categories/UploadFile/" + Categories.categoriesId, contentType: false, processData: false,
            data: data, async: false,
            headers: {
                'Authorization': `Bearer ${userToken}`,
            },
            success: function (message) {
                md.showNotification("تم  تحميل الصورة بنجاح");
            },
            error: function () {
                md.showNotification("عذرا حدث خطا اثناء  تحميل الصورة");
            },
        });
    }
    $('#tableCategories').empty();
    $("#CategoriesName").val(''); 
    _categoriesId = 0;
    call_ajax("GET", "Categories/GetAll", null, filltableCategories);
}