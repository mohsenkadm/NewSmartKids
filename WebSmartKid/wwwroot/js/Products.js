
var _ProductsId = 0;
var ProductsId2 = 0;
var selectallsastus = false;
function filltableProducts(data) {
    $('#tableProducts').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +  
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsDiscount" + item.productsId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" + 
            "<td>" + item.discountPercentage + "</td>" +
            "<td>" + item.categoriesName + "</td>" +
            "<td>" + item.count + "</td>" +
            "<td>" + item.price + "</td>" +
            "<td>" + item.detail + "</td>" +
            "<td>" + item.name + "</td>" +
             "<td> <button type='button' class='btn btn-info' onclick='ShowImage(" + item.productsId + ")'  data-toggle='modal' data-target='#ImageModal' >عرض الصور</button>"
            +" | <button type = 'button' class='btn btn-danger' onclick = 'deleteProducts(" + item.productsId + ")' > حذف</button > " +
            "  |  <button type='button' class='btn btn-primary' onclick='updateProducts(" + item.productsId + ")'  data-toggle='modal' data-target='#ProductsModal'>تعديل</button>"+
        "  |  <button type='button' class='btn btn-primary' onclick='GetAge(" + item.productsId + ")'  data-toggle='modal' data-target='#AgeModal'>العمر</button></td></tr>";
        $('#tableProducts').append(rows);
        $('#IsDiscount' + item.productsId).attr('checked', item.isDiscount);
    });
}


function GetAge(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "TblAges/GetProductAndAge", object1, filltableAge);
}

 
function filltableAge(data) {
    $('#tableAge').empty();
    var rows = "<tr>" +
        "<td><input type='checkbox'  id='salactall' onclick='SetSalactAllProductAndAge(this," + data[0].productsId + ")'   /></td>" +
        "<td>تحديد الكل</td></tr>";
    $('#tableAge').append(rows); 
    $.each(data, function (i, item) {
        var rows = "<tr>" +
            "<td><input type='checkbox'    id='state" + item.id + "' onclick='SetProductAndAge(this," + item.id + ")' /></td>" +
            "<td>" + item.ageName + "</td></tr>";
        $('#tableAge').append(rows);
        $('#state' + item.id).attr('checked', item.state);
    });
    $('#salactall').attr('checked', selectallsastus);
}  
function SetProductAndAge(state, id) {
    var object = {
        State: state.checked, Id: id,
    };
    call_ajax("POST", "TblAges/SetProductAndAge", object, null);
}
function SetSalactAllProductAndAge(state, id) {
    var object = {
        State: state.checked, ProductsId: id,
    };
      selectallsastus =object.State;
    call_ajax("POST", "TblAges/SetSalactAllProductAndAge", object, GetAge);
}

function deleteProducts(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Products/Delete", object1, RefreshProducts);
    }
}
function RefreshProducts() {
    var ConatinImage = false;
    if ($('#ConatinImage').is(":checked")) {
        ConatinImage = true;
    }
    var count = $("#indexid").text();
    if (count > 1) {
        var obj = { Name: $("#namese").val(), CategoriesId: $("#CategoriesIdse").val(), index: count, ConatinImage: ConatinImage }
        call_ajax("GET", "Products/GetAll", obj, filltableProducts);
    }
}

function updateProducts(id) {
    var result = confirm("سيتم تحديث جدول الاعمار ايضا اذا قمت بعملية التعديل؟!");
    if (result == true) {
        var object1 = {
            Id: id
        };
        call_ajax("GET", "Products/GetById", object1, setdataProducts);
        _ProductsId = id;
    }
}

function setdataProducts(data) {
    if (data.isDiscount === true) {
        $("#IsDiscountca").prop("checked", true);
    }
    else {
        $("#IsDiscountca").prop("checked", false);
    }
    $("#Name").val(data.name);  
    $("#Detail").val(data.detail);  
    $("#Price").val(data.price);  
    $("#Count").val(data.count);  
    $("#DiscountPercentage").val(data.discountPercentage);  
    $("#CategoriesId").val(data.categoriesId).change();
}
function aftersaveProducts(data) { 
    var input = $("#imageca").get(0);
    var files = input.files;
    var formData = new FormData();
    if (files.length > 0) {
        var userToken = getCookie("token2");
        for (var i = 0; i != files.length; i++) {
            formData.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST", url: "/Products/UploadFile/" + data.productsId, async: false, contentType: false, processData: false,
            data: formData,
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
    $('#tableProducts').empty();
    $("#Name").val('');  
    $("#Detail").val('');  
    $("#Price").val('');  
    $("#Count").val('');  
    $("#DiscountPercentage").val('');  
    _ProductsId = 0;
    RefreshProducts();
}

function filltableImage(data) {
    $('#tableImage').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +
            "<td><img src='" + item.imagePath + "' alt='' border=3 height=50 width=50></img></td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteImage(" + item.imageId + ")'  >حذف</button></tr>";
        $('#tableImage').append(rows);
    });
}

function RefreshImage() {
    var obj = { Id: ProductsId2 }
    call_ajax("GET", "Products/GetImagesByProductsId", obj, filltableImage);
}

function ShowImage(id) {
    ProductsId2 = id;
    RefreshImage();
}

function deleteImage(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Products/DeleteImage", object1, RefreshImage);
    }
}