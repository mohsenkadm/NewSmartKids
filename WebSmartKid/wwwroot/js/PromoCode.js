// ====================================
// متغيرات عامة
// ====================================
var _PromoCodeId = 0;
var _CurrentPromoCodeId = 0;

// ====================================
// دوال عرض البيانات
// ====================================

/**
 * ملء جدول البرومو كودات
 * @param {Array} data - قائمة البرومو كودات
 */
function fillTablePromoCode(data) {
    $('#tablePromoCode').empty();
    $.each(data, function (i, item) {
        var statusBadge = item.isActive 
            ? '<span class="badge badge-success">نشط</span>' 
            : '<span class="badge badge-danger">غير نشط</span>';
        
        var toggleBtn = item.isActive 
            ? `<button type='button' class='btn btn-sm btn-warning' onclick='toggleActive(${item.promoCodeId}, false)'>إيقاف</button>`
            : `<button type='button' class='btn btn-sm btn-success' onclick='toggleActive(${item.promoCodeId}, true)'>تفعيل</button>`;

        var date = new Date(item.createdDate);
        var formattedDate = date.toLocaleDateString('ar-EG');

        var rows = "<tr>" +
            "<td>" + formattedDate + "</td>" +
            "<td>" + statusBadge + "</td>" +
            "<td>" + item.amount + " IQD</td>" +
            "<td>" + item.name + "</td>" +
            "<td><button type='button' class='btn btn-sm btn-info' onclick='showUsage(" + item.promoCodeId + ")' data-toggle='modal' data-target='#UsageModal'>" + (item.usedCount || 0) + "</button></td>" +
            "<td>" +
            toggleBtn + " | " +
            "<button type='button' class='btn btn-sm btn-primary' onclick='updatePromoCode(" + item.promoCodeId + ")' data-toggle='modal' data-target='#PromoCodeModal'>تعديل</button> | " +
            "<button type='button' class='btn btn-sm btn-danger' onclick='deletePromoCode(" + item.promoCodeId + ")'>حذف</button>" +
            "</td></tr>";
        $('#tablePromoCode').append(rows);
    });
}

/**
 * ملء جدول سجل الاستخدامات
 * @param {Array} data - قائمة الاستخدامات
 */
function fillTableUsage(data) {
    $('#tableUsage').empty();
    if (data.length === 0) {
        $('#tableUsage').append('<tr><td colspan="4">لا توجد استخدامات لهذا البرومو كود</td></tr>');
        return;
    }
    
    $.each(data, function (i, item) {
        var date = new Date(item.usedDate);
        var formattedDate = date.toLocaleDateString('ar-EG') + ' ' + date.toLocaleTimeString('ar-EG');
        
        var rows = "<tr>" +
            "<td>" + formattedDate + "</td>" +
            "<td>" + item.userName + "</td>" +
            "<td>" + item.phone + "</td>" +
            "<td>" + item.amount + " IQD</td>" +
            "</tr>";
        $('#tableUsage').append(rows);
    });
}

// ====================================
// دوال العمليات (CRUD)
// ====================================

/**
 * حذف برومو كود
 * @param {number} id - معرف البرومو كود
 */
function deletePromoCode(id) {
    var result = confirm("هل تريد حذف هذا البرومو كود؟ سيتم حذف جميع سجلات الاستخدام أيضاً!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "PromoCode/Delete", object1, refreshPromoCode);
    }
}

/**
 * تعديل برومو كود
 * @param {number} id - معرف البرومو كود
 */
function updatePromoCode(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "PromoCode/GetById", object1, setDataPromoCode);
    _PromoCodeId = id;
}

/**
 * تعيين بيانات البرومو كود في النموذج
 * @param {Object} data - بيانات البرومو كود
 */
function setDataPromoCode(data) {
    $("#Name").val(data.name);
    $("#Amount").val(data.amount);
    $("#IsActiveca").prop("checked", data.isActive);
}

/**
 * تفعيل/إيقاف البرومو كود
 * @param {number} id - معرف البرومو كود
 * @param {boolean} isActive - الحالة المطلوبة
 */
function toggleActive(id, isActive) {
    var message = isActive ? "هل تريد تفعيل هذا البرومو كود؟" : "هل تريد إيقاف هذا البرومو كود؟";
    var result = confirm(message);
    if (result == true) {
        var object = {
            Id: id,
            IsActive: isActive
        };
        call_ajax("POST", "PromoCode/ToggleActive", object, refreshPromoCode);
    }
}

/**
 * عرض سجل استخدامات البرومو كود
 * @param {number} id - معرف البرومو كود
 */
function showUsage(id) {
    _CurrentPromoCodeId = id;
    var obj = { PromoCodeId: id };
    call_ajax("GET", "PromoCode/GetPromoCodeUsage", obj, fillTableUsage);
}

/**
 * مسح نموذج البرومو كود
 */
function clearPromoCodeForm() {
    $("#Name").val('');
    $("#Amount").val('');
    $("#IsActiveca").prop("checked", true);
    _PromoCodeId = 0;
}

// ====================================
// دوال التحديث
// ====================================

/**
 * تحديث قائمة البرومو كودات
 */
function refreshPromoCode() {
    var count = $("#indexid").text();
    var isActive = $("#IsActivese").val();
    if (isActive === "") {
        isActive = null;
    }
    var obj = {
        Name: $("#namese").val(),
        IsActive: isActive,
        Index: count
    };
    call_ajax("GET", "PromoCode/GetAll", obj, fillTablePromoCode);
}

// ====================================
// Event Handlers
// ====================================

$(document).ready(function () {
    // تحميل البيانات الأولية
    var obj = { Name: '', IsActive: null, Index: 1 };
    call_ajax("GET", "PromoCode/GetAll", obj, fillTablePromoCode);
    $("#indexid").text(1);
});

$("#save").on("click", (e) => {
    e.preventDefault();

    var isActive = false;
    if ($('#IsActiveca').is(":checked")) {
        isActive = true;
    }

    var object = {
        PromoCodeId: _PromoCodeId,
        Name: $("#Name").val(),
        Amount: $("#Amount").val(),
        IsActive: isActive,
    };

    if (object.Name === "" || object.Name.trim() === "") {
        md.showNotification("رجاءا ادخل اسم البرومو كود");
        return;
    }
    if (object.Amount === "" || object.Amount <= 0) {
        md.showNotification("رجاءا ادخل المبلغ بشكل صحيح");
        return;
    }

    call_ajax("POST", "PromoCode/Post", object, afterSavePromoCode);
});

function afterSavePromoCode(data) {
    clearPromoCodeForm();
    refreshPromoCode();
    $('#PromoCodeModal').modal('hide');
}

$("#search-btn").on("click", (e) => {
    e.preventDefault();
    var isActive = $("#IsActivese").val();
    if (isActive === "") {
        isActive = null;
    }
    var obj = {
        Name: $("#namese").val(),
        IsActive: isActive,
        Index: 1
    };
    call_ajax("GET", "PromoCode/GetAll", obj, fillTablePromoCode);
    $("#indexid").text(1);
});

$("#next").on("click", (e) => {
    e.preventDefault();
    var count = parseInt($("#indexid").text());
    count++;
    var isActive = $("#IsActivese").val();
    if (isActive === "") {
        isActive = null;
    }
    var obj = {
        Name: $("#namese").val(),
        IsActive: isActive,
        Index: count
    };
    call_ajax("GET", "PromoCode/GetAll", obj, fillTablePromoCode);
    $("#indexid").text(count);
});

$("#prev").on("click", (e) => {
    e.preventDefault();
    var count = parseInt($("#indexid").text());
    if (count > 1) {
        count--;
        var isActive = $("#IsActivese").val();
        if (isActive === "") {
            isActive = null;
        }
        var obj = {
            Name: $("#namese").val(),
            IsActive: isActive,
            Index: count
        };
        call_ajax("GET", "PromoCode/GetAll", obj, fillTablePromoCode);
        $("#indexid").text(count);
    }
});

// مسح النموذج عند إغلاق Modal
$('#PromoCodeModal').on('hidden.bs.modal', function () {
    clearPromoCodeForm();
});
