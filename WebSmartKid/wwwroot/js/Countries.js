
var _countryId = 0;
function filltableCountries(data) {
    $('#tableCountries').empty();
    $.each(data, function (i, item) {
        var rows = "<tr>" +    
            "<td>" + item.countryName + "</td>"
            + "<td> <button type='button' class='btn btn-danger' onclick='deleteCountries(" + item.countryId + ")'  >حذف</button>" +
            "  |  <button type='button' class='btn btn-primary' onclick='updateCountries(" + item.countryId + ")'  data-toggle='modal' data-target='#CountriesModal'>تعديل</button></td></tr>";
        $('#tableCountries').append(rows); 
    });
}

function deleteCountries(id) {
    var result = confirm("هل تريد الحذف؟!");
    if (result == true) {
        var object1 = {
            Id: id,
        }
        call_ajax("DELETE", "Countries/Delete", object1, RefreshCountries);
    }
}
function RefreshCountries() {
    call_ajax("GET", "Countries/GetAll", null, filltableCountries);
}

function updateCountries(id) {
    var object1 = {
        Id: id
    };
    call_ajax("GET", "Countries/GetById", object1, setdataCountries);
    _countryId = id;
}

function setdataCountries(data) {
    $("#CountryName").val(data.countryName); 
}
function aftersaveCountries() { 
    $('#tableCountries').empty();
    $("#CountryName").val('');  
    _countryId = 0;
    call_ajax("GET", "Countries/GetAll", null, filltableCountries);
}