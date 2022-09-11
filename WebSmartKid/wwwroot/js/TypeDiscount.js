
var _Id = 0;
function filltableTypeDiscount(data) {
    $('#tableTypeDiscount').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +    
            "<td>" + item.price + "</td>"+
            "<td>" + item.typeDis + "</td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteTypeDiscount(" + item.id + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updateTypeDiscount(" + item.id + ")'  data-toggle='modal' data-target='#TypeDiscountModal'>تعديل</button></td></tr>";
        $('#tableTypeDiscount').append(rows); 
    });
}

function deleteTypeDiscount(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "TypeDiscount/Delete", object1, RefreshTypeDiscount);
    }
}
function RefreshTypeDiscount() {
    call_ajax("GET", "TypeDiscount/GetAll", null, filltableTypeDiscount);
}

function updateTypeDiscount(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "TypeDiscount/GetById", object1, setdataTypeDiscount);
    _Id = id;
}

function setdataTypeDiscount(data) {
    $("#Price").val(data.price); 
}
function aftersaveTypeDiscount() { 
    $('#tableTypeDiscount').empty();
    $("#Price").val('');  
    _Id = 0;
    call_ajax("GET", "TypeDiscount/GetAll", null, filltableTypeDiscount);
}