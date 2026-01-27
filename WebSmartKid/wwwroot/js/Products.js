// ====================================
// متغيرات عامة
// ====================================
var _ProductsId = 0;
var ProductsId2 = 0;
var selectallsastus = false;

// Cache DOM elements for better performance
var $cache = {
    tableProducts: null,
    tableAge: null,
    tableImage: null,
    nameInput: null,
    detailInput: null,
    priceInput: null,
    countInput: null,
    discountPercentageInput: null,
    categoriesIdInput: null,
    urlInput: null,
    imageInput: null,
    imageUrlInput: null,
    isDiscountCheckbox: null,
    searchNameInput: null,
    searchCategoryInput: null,
    containImageCheckbox: null,
    indexElement: null,
    productModal: null,
    imageModal: null,
    ageModal: null
};

// Initialize cache on document ready
function initializeCache() {
    $cache.tableProducts = $('#tableProducts');
    $cache.tableAge = $('#tableAge');
    $cache.tableImage = $('#tableImage');
    $cache.nameInput = $("#Name");
    $cache.detailInput = $("#Detail");
    $cache.priceInput = $("#Price");
    $cache.countInput = $("#Count");
    $cache.discountPercentageInput = $("#DiscountPercentage");
    $cache.categoriesIdInput = $("#CategoriesId");
    $cache.urlInput = $("#Url");
    $cache.imageInput = $("#imageca");
    $cache.imageUrlInput = $("#ImageUrl");
    $cache.isDiscountCheckbox = $("#IsDiscountca");
    $cache.searchNameInput = $("#namese");
    $cache.searchCategoryInput = $("#CategoriesIdse");
    $cache.containImageCheckbox = $('#ConatinImage');
    $cache.indexElement = $("#indexid");
    $cache.productModal = $('#ProductsModal');
    $cache.imageModal = $('#ImageModal');
    $cache.ageModal = $('#AgeModal');
}

// Utility function to escape HTML (prevent XSS)
function escapeHtml(text) {
    if (!text) return '';
    var map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.toString().replace(/[&<>"']/g, function(m) { return map[m]; });
}

// Get ConatinImage status
function getContainImageStatus() {
    return $cache.containImageCheckbox.is(":checked");
}

// ====================================
// دوال عرض البيانات
// ====================================

/**
 * ملء جدول المنتجات
 * @param {Array} data - قائمة المنتجات
 */
function filltableProducts(data) {
    if (!data || data.length === 0) {
        $cache.tableProducts.html('<tr><td colspan="9" class="text-center">لا توجد منتجات</td></tr>');
        return;
    }

    // Use array for better performance than string concatenation
    var rows = [];
    
    $.each(data, function (i, item) {
        var productId = item.productsId;
        var isDiscountChecked = item.isDiscount ? 'checked' : '';
        
        rows.push(
            '<tr data-product-id="' + productId + '">' +
            '<td>' +
            '<div class="form-check">' +
            '<label class="form-check-label">' +
            '<input class="form-check-input" type="checkbox" id="IsDiscount' + productId + '" ' + isDiscountChecked + '>' +
            '<span class="form-check-sign">' +
            '<span class="check"></span>' +
            '</span>' +
            '</label>' +
            '</div>' +
            '</td>' + 
            '<td>' + escapeHtml(item.discountPercentage) + '</td>' +
            '<td>' + escapeHtml(item.categoriesName) + '</td>' +
            '<td>' + escapeHtml(item.count) + '</td>' +
            '<td>' + escapeHtml(item.price) + '</td>' +
            '<td>' + (item.url ? escapeHtml(item.url) : '') + '</td>' +
            '<td>' + escapeHtml(item.detail) + '</td>' +
            '<td>' + escapeHtml(item.name) + '</td>' +
            '<td>' + 
            '<button type="button" class="btn btn-info btn-sm btn-show-image" data-id="' + productId + '" data-toggle="modal" data-target="#ImageModal">عرض الصور</button> | ' +
            '<button type="button" class="btn btn-danger btn-sm btn-delete-product" data-id="' + productId + '">حذف</button> | ' +
            '<button type="button" class="btn btn-primary btn-sm btn-update-product" data-id="' + productId + '" data-toggle="modal" data-target="#ProductsModal">تعديل</button> | ' +
            '<button type="button" class="btn btn-primary btn-sm btn-get-age" data-id="' + productId + '" data-toggle="modal" data-target="#AgeModal">العمر</button>' +
            '</td>' +
            '</tr>'
        );
    });
    
    $cache.tableProducts.html(rows.join(''));
}

/**
 * ملء جدول الأعمار
 * @param {Array} data - قائمة الأعمار
 */
function filltableAge(data) {
    if (!data || data.length === 0) {
        $cache.tableAge.html('<tr><td colspan="2" class="text-center">لا توجد أعمار</td></tr>');
        return;
    }

    var rows = [];
    var productId = data[0].productsId;
    var selectAllChecked = selectallsastus ? 'checked' : '';
    
    rows.push(
        '<tr>' +
        '<td><input type="checkbox" id="salactall" class="select-all-age" data-product-id="' + productId + '" ' + selectAllChecked + ' /></td>' +
        '<td>تحديد الكل</td>' +
        '</tr>'
    );
    
    $.each(data, function (i, item) {
        var ageChecked = item.state ? 'checked' : '';
        rows.push(
            '<tr>' +
            '<td><input type="checkbox" id="state' + item.id + '" class="age-checkbox" data-id="' + item.id + '" ' + ageChecked + ' /></td>' +
            '<td>' + escapeHtml(item.ageName) + '</td>' +
            '</tr>'
        );
    });
    
    $cache.tableAge.html(rows.join(''));
}

/**
 * ملء جدول الصور
 * @param {Array} data - قائمة الصور
 */
function filltableImage(data) {
    if (!data || data.length === 0) {
        $cache.tableImage.html('<tr><td colspan="2" class="text-center">لا توجد صور</td></tr>');
        return;
    }

    var rows = [];
    
    $.each(data, function (i, item) {
        var imagePath = escapeHtml(item.imagePath);
        rows.push(
            '<tr>' +
            '<td><img src="' + imagePath + '" alt="صورة المنتج" border="3" height="50" width="50" loading="lazy"></td>' +
            '<td><button type="button" class="btn btn-danger btn-sm btn-delete-image" data-id="' + item.imageId + '">حذف</button></td>' +
            '</tr>'
        );
    });
    
    $cache.tableImage.html(rows.join(''));
}

// ====================================
// دوال العمليات (CRUD)
// ====================================

/**
 * حذف منتج
 * @param {number} id - معرف المنتج
 */
function deleteProducts(id) {
    if (!confirm("هل تريد الحذف؟!")) {
        return;
    }
    
    var object1 = { Id: id };
    call_ajax("DELETE", "Products/Delete", object1, function(response) {
        if (response && response.success !== false) {
            md.showNotification("تم الحذف بنجاح");
            RefreshProducts();
        } else {
            md.showNotification("حدث خطأ أثناء الحذف");
        }
    });
}

/**
 * تعديل منتج
 * @param {number} id - معرف المنتج
 */
function updateProducts(id) {
    if (!confirm("سيتم تحديث جدول الاعمار ايضا اذا قمت بعملية التعديل؟!")) {
        return;
    }
    
    _ProductsId = id;
    var object1 = { Id: id };
    call_ajax("GET", "Products/GetById", object1, setdataProducts);
}

/**
 * تعيين بيانات المنتج في النموذج
 * @param {Object} data - بيانات المنتج
 */
function setdataProducts(data) {
    if (!data) {
        md.showNotification("حدث خطأ في جلب بيانات المنتج");
        return;
    }
    
    $cache.isDiscountCheckbox.prop("checked", data.isDiscount === true);
    $cache.nameInput.val(data.name || '');
    $cache.detailInput.val(data.detail || '');
    $cache.priceInput.val(data.price || '');
    $cache.countInput.val(data.count || '');
    $cache.discountPercentageInput.val(data.discountPercentage || '');
    $cache.categoriesIdInput.val(data.categoriesId).change();
    $cache.urlInput.val(data.url || '');
}

/**
 * حفظ المنتج مع الصور أو رابط صورة
 * @param {Object} productData - بيانات المنتج
 */
function saveProductWithFiles(productData) {
    var formData = new FormData();
    
    // إضافة بيانات المنتج
    formData.append("ProductsId", productData.ProductsId);
    formData.append("Name", productData.Name);
    formData.append("Detail", productData.Detail);
    formData.append("Price", productData.Price);
    formData.append("Count", productData.Count);
    formData.append("CategoriesId", productData.CategoriesId);
    formData.append("DiscountPercentage", productData.DiscountPercentage);
    formData.append("IsDiscount", productData.IsDiscount);
    formData.append("Url", productData.Url || "");
    
    // إضافة الصور
    var imageInput = $cache.imageInput.get(0);
    var imageFiles = imageInput.files;
    if (imageFiles && imageFiles.length > 0) {
        for (var i = 0; i < imageFiles.length; i++) {
            formData.append("images", imageFiles[i]);
        }
    }

    // إضافة رابط الصورة إن وجد
    var imageUrl = $cache.imageUrlInput.val();
    if (imageUrl && imageUrl.trim() !== "") {
        formData.append("imageUrl", imageUrl.trim());
    }
    
    var userToken = getCookie("token2");
    
    $.ajax({
        type: "POST",
        url: "/Products/PostWithFiles",
        data: formData,
        contentType: false,
        processData: false,
        headers: {
            'Authorization': `Bearer ${userToken}`,
        },
        success: function (response) {
            if (response && response.success !== false) {
                md.showNotification("تم حفظ المنتج والصور بنجاح");
                clearProductForm();
                $cache.productModal.modal('hide');
                RefreshProducts();
            } else {
                md.showNotification("عذرا: " + (response.message || response.msg || "حدث خطأ"));
            }
        },
        error: function (xhr, status, error) {
            console.error("Error saving product:", error);
            md.showNotification("عذرا حدث خطأ اثناء الحفظ");
        },
    });
}

/**
 * مسح نموذج المنتج
 */
function clearProductForm() {
    $cache.nameInput.val('');
    $cache.detailInput.val('');
    $cache.priceInput.val('');
    $cache.countInput.val('');
    $cache.discountPercentageInput.val('');
    $cache.imageInput.val('');
    $cache.imageUrlInput.val('');
    $cache.urlInput.val('');
    $cache.isDiscountCheckbox.prop("checked", false);
    _ProductsId = 0;
}

/**
 * حذف صورة أو فيديو
 * @param {number} id - معرف الصورة
 */
function deleteImage(id) {
    if (!confirm("هل تريد الحذف؟!")) {
        return;
    }
    
    var object1 = { Id: id };
    call_ajax("DELETE", "Products/DeleteImage", object1, function(response) {
        if (response && response.success !== false) {
            md.showNotification("تم حذف الصورة بنجاح");
            RefreshImage();
        } else {
            md.showNotification("حدث خطأ أثناء حذف الصورة");
        }
    });
}

// ====================================
// دوال الأعمار
// ====================================

/**
 * جلب الأعمار للمنتج
 * @param {number} id - معرف المنتج
 */
function GetAge(id) {
    var object1 = { Id: id };
    call_ajax("GET", "TblAges/GetProductAndAge", object1, filltableAge);
}

/**
 * تعيين عمر للمنتج
 * @param {boolean} state - حالة الاختيار
 * @param {number} id - معرف العمر
 */
function SetProductAndAge(state, id) {
    var object = { State: state, Id: id };
    call_ajax("POST", "TblAges/SetProductAndAge", object, null);
}

/**
 * تحديد/إلغاء تحديد جميع الأعمار
 * @param {boolean} state - حالة الاختيار
 * @param {number} id - معرف المنتج
 */
function SetSalactAllProductAndAge(state, id) {
    var object = { State: state, ProductsId: id };
    selectallsastus = object.State;
    call_ajax("POST", "TblAges/SetSalactAllProductAndAge", object, function() {
        GetAge(id);
    });
}

// ====================================
// دوال التحديث والعرض
// ====================================

/**
 * تحديث قائمة المنتجات
 */
function RefreshProducts() {
    var count = $cache.indexElement.text();
    if (count > 1) {
        var obj = { 
            Name: $cache.searchNameInput.val() || '', 
            CategoriesId: $cache.searchCategoryInput.val() || 0, 
            index: count, 
            ConatinImage: getContainImageStatus()
        };
        call_ajax("GET", "Products/GetAll", obj, filltableProducts);
    }
}

/**
 * تحديث قائمة الصور
 */
function RefreshImage() {
    var obj = { Id: ProductsId2 };
    call_ajax("GET", "Products/GetImagesByProductsId", obj, filltableImage);
}

/**
 * عرض صور المنتج
 * @param {number} id - معرف المنتج
 */
function ShowImage(id) {
    ProductsId2 = id;
    RefreshImage();
}

// ====================================
// Event Delegation for Dynamic Buttons
// ====================================

/**
 * Initialize event delegation for dynamically created buttons
 */
function initializeEventDelegation() {
    // Event delegation for product table buttons
    $cache.tableProducts.on('click', '.btn-show-image', function(e) {
        e.preventDefault();
        var id = $(this).data('id');
        ShowImage(id);
    });
    
    $cache.tableProducts.on('click', '.btn-delete-product', function(e) {
        e.preventDefault();
        var id = $(this).data('id');
        deleteProducts(id);
    });
    
    $cache.tableProducts.on('click', '.btn-update-product', function(e) {
        e.preventDefault();
        var id = $(this).data('id');
        updateProducts(id);
    });
    
    $cache.tableProducts.on('click', '.btn-get-age', function(e) {
        e.preventDefault();
        var id = $(this).data('id');
        GetAge(id);
    });
    
    // Event delegation for image table
    $cache.tableImage.on('click', '.btn-delete-image', function(e) {
        e.preventDefault();
        var id = $(this).data('id');
        deleteImage(id);
    });
    
    // Event delegation for age table
    $cache.tableAge.on('click', '.age-checkbox', function() {
        var id = $(this).data('id');
        var state = $(this).is(':checked');
        SetProductAndAge(state, id);
    });
    
    $cache.tableAge.on('click', '.select-all-age', function() {
        var productId = $(this).data('product-id');
        var state = $(this).is(':checked');
        SetSalactAllProductAndAge(state, productId);
    });
}

// ====================================
// Validation Functions
// ====================================

/**
 * Validate product form
 * @returns {Object} validation result
 */
function validateProductForm() {
    var name = $cache.nameInput.val();
    var detail = $cache.detailInput.val();
    var price = $cache.priceInput.val();
    var categoryId = $cache.categoriesIdInput.val();
    
    if (!name || name.trim() === "") {
        return { valid: false, message: "رجاءا ادخل الاسم" };
    }
    
    if (!detail || detail.trim() === "") {
        return { valid: false, message: "رجاءا ادخل التفاصيل" };
    }
    
    if (!price || price.trim() === "" || isNaN(price) || parseFloat(price) < 0) {
        return { valid: false, message: "رجاءا ادخل السعر بشكل صحيح" };
    }
    
    if (!categoryId || categoryId.trim() === "0" || categoryId === 0) {
        return { valid: false, message: "رجاءا اختر الصنف" };
    }
    
    return { valid: true };
}

/**
 * Get product data from form
 * @returns {Object} product data
 */
function getProductDataFromForm() {
    var isDiscount = $cache.isDiscountCheckbox.is(":checked");
    
    return {
        ProductsId: _ProductsId,
        Name: $cache.nameInput.val().trim(),
        Detail: $cache.detailInput.val().trim(),
        Price: $cache.priceInput.val(),
        Count: $cache.countInput.val() || 0,
        CategoriesId: $cache.categoriesIdInput.val(),
        DiscountPercentage: $cache.discountPercentageInput.val() || 0,
        IsDiscount: isDiscount,
        Url: $cache.urlInput.val().trim()
    };
}

// ====================================
// Modal Event Handlers
// ====================================

/**
 * Handle modal shown event to clear form for new product
 */
function handleProductModalShown() {
    if (_ProductsId === 0) {
        clearProductForm();
    }
}

/**
 * Handle modal hidden event to reset form
 */
function handleProductModalHidden() {
    clearProductForm();
}
