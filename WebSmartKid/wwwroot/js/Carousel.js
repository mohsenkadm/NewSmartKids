
var _CarouseId = 0;
function filltableCarousel(data) {
    $('#tableCarousel').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsShow" + item.carouseId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" + 
            "<td><img src='" + item.image + "' alt='' border=3 height=50 width=50></img></td>" +
            "<td>" + item.image + "</td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteCarousel(" + item.carouseId + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updateCarousel(" + item.carouseId + ")'  data-toggle='modal' data-target='#CarouselModal'>تعديل</button></td></tr>";
        $('#tableCarousel').append(rows);
        $('#IsShow' + item.carouseId).attr('checked', item.isShow);
    });
}

function deleteCarousel(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Carousel/Delete", object1, RefreshCarousel);
    }
}
function RefreshCarousel() {
    call_ajax("GET", "Carousel/GetAll", null, filltableCarousel);
}

function updateCarousel(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Carousel/GetById", object1, setdataCarousel);
    _CarouseId = id;
}

function setdataCarousel(data) {
    if (data.isShow === true) {
        $("#IsShowca").prop("checked", true);
    }
    else {
        $("#IsShowca").prop("checked", false);
    }
}
function aftersaveCarousel(Carousel) {
    var fileUpload = $("#imageca").get(0);
    var files = fileUpload.files;
    var data = new FormData();
    if (files.length > 0) {
        data.append(files[0].name, files[0]);

        var userToken = getCookie("token2");
        $.ajax({
            type: "POST", url: "/Carousel/UploadFile/" + Carousel.carouseId, contentType: false, processData: false,
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
    $('#tableCarousel').empty();
    $("#imageca").val('');
    $("#IsShowca").prop("checked", false);
    _CarouseId = 0;
    call_ajax("GET", "Carousel/GetAll", null, filltableCarousel);
}