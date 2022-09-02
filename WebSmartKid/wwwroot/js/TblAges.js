
var _AgeId = 0;
function filltableTblAges(data) {
    $('#tableTblAges').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +    
            "<td>" + item.ageName + "</td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteTblAges(" + item.ageId + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updateTblAges(" + item.ageId + ")'  data-toggle='modal' data-target='#TblAgesModal'>تعديل</button></td></tr>";
        $('#tableTblAges').append(rows); 
    });
}

function deleteTblAges(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "TblAges/Delete", object1, RefreshTblAges);
    }
}
function RefreshTblAges() {
    call_ajax("GET", "TblAges/GetAll", null, filltableTblAges);
}

function updateTblAges(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "TblAges/GetById", object1, setdataTblAges);
    _AgeId = id;
}

function setdataTblAges(data) {
    $("#AgeName").val(data.ageName); 
}
function aftersaveTblAges() { 
    $('#tableTblAges').empty();
    $("#AgeName").val('');  
    _AgeId = 0;
    call_ajax("GET", "TblAges/GetAll", null, filltableTblAges);
}