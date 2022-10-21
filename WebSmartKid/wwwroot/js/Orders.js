
function filltableOrders(data) {
    $('#tableOrders').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +  
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsDone" + item.orderId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" + 
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsApporve" + item.orderId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" + 
            "<td>" +
            "<div class='form-check'>" +
            "<label class='form-check-label'>" +
            "<input class='form-check-input' type='checkbox' id='IsCancel" + item.orderId + "'>" +
            "<span class='form-check-sign'>" +
            "<span class='check'></span>" +
            "</span>" +
            "</label>" +
            "</div>" +
            "</td>" +
            "<td>" + item.netAmount + "</td>" +
            "<td>" + item.totalDiscount + "</td>" +
            "<td>" + item.total + "</td>" +
            "<td>" + item.address + "</td>" + 
            "<td>" + item.countryName + "</td>" + 
            "<td>" + item.phone + "</td>" +
            "<td>" + item.details + "</td>" +
            "<td>" + item.name + "</td>" +
            "<td>" + item.orderDate + "</td>" +
            "<td>" + item.orderNo + "</td>"
            + "<td> <button type='button' class='btn btn-primary' onclick='SetIsDone(" + item.orderId + ")'  >انتهاء الطلب</button></td>"
            + "<td> <button type='button' class='btn btn-info' onclick='PrintOrder(" + item.orderId + ")'  >طباعة الطلب</button></td>"
            + " <td>  <button type='button' class='btn btn-warning' onclick='SetIsCancel(" + item.orderId + ")'  >الغاء</button></td>"
            + " <td>  <button type='button' class='btn btn-success' onclick='SetIsApporve(" + item.orderId + ")'  >موافقة</button></td>"
            + " <td>  <button type='button' class='btn btn-danger' onclick='deleteOrders(" + item.orderId + ")'  >حذف</button></td>" +
            "  <td>  <button type='button' class='btn btn-info' onclick='OrderDetail(" + item.orderId + ")'  data-toggle='modal' data-target='#OrdersDetailsModal'>تفاصيل الطلب</button></td></tr>";
        $('#tableOrders').append(rows); 
        $('#IsDone' + item.orderId).attr('checked', item.isDone);
        $('#IsApporve' + item.orderId).attr('checked', item.isApporve);
        $('#IsCancel' + item.orderId).attr('checked', item.isCancel);
    });
}

function PrintOrder(id) {
    var obj = {
        Id: id
    }
    window.open("/Orders/Print?Id=" + id);
    //Print("GET", "Orders/Print", obj);
}
function deleteOrders(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Orders/Delete", object1, RefreshOrders);
    }
}
function SetIsCancel(id) {
    var object1 = {
        Id: id,
    }
    call_ajax("DELETE", "Orders/SetIsCancel", object1, RefreshOrders);
}
function SetIsApporve(id) {
    var object1 = {
        Id: id,
    }
    call_ajax("Post", "Orders/SetIsApporve", object1, RefreshOrders);
}
function SetIsDone(id) {
    var object1 = {
        Id: id,
    }
    call_ajax("Post", "Orders/SetIsDone", object1, RefreshOrders);
} 
var _id = 0;
function OrderDetail(id) {
    var object1 = {
        Id: id,
    }
    _id = id;
    call_ajax("Get", "Orders/GetOrdersWithDetailAll", object1, SetDataOrderDetail);
}

function SetDataOrderDetail(data) {
    $('#tableOrdersDetail').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +
            "<td> " + item.count + "</td> " +  
            "<td> " + item.price + "</td> " +  
            "<td>" + item.detail + "</td>" + 
            "<td>" + item.prodName + "</td>" +
            "<td><img src='" + item.image + "' alt='' border=3 height=50 width=50></img></td>" 
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteOrderDetail(" + item.orderDetailId + ")'  >حذف</button></td></tr>";
        $('#tableOrdersDetail').append(rows);
    });
}


function deleteOrderDetail(id) {
    var object1 = {
        Id: id,
    }
    call_ajax("DELETE", "Orders/DeleteDetails", object1, RefreshOrderDetail);
}
function RefreshOrderDetail() {
    OrderDetail(_id);
}
function RefreshOrders() {
    var obj = {
        OrderNo: '', UserName: '', datefrom: $("#datefrom").val(),
        dateto: $("#dateto").val(),
    }
    call_ajax("GET", "Orders/GetAll", obj, filltableOrders);
}
