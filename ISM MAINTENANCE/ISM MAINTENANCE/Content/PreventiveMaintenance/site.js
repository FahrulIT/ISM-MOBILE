
//$('.datepicker').datepicker();

var _vLoading = "#idLoading"; //khusus buat ambil modal untuk loading
var _vSch = $('#scheduleDate').val();

$("#spareParts").on('change', function () {

    $.ajax({
        type: 'GET',
        url: 'getUnitPrice',
        data: {
            itemName: $(this).val()
        },
        success: function (data) {

            if (data == null) {

                var itemCode = data.ItemCode;
                $("#itemCode").val(itemCode);

                var price = parseFloat(data.PRICE);
                $("#unitPrice").val(price.toFixed(2));
                $("#quantity").val(0);
            } else {

                var itemCode = data.ItemCode;
                $("#itemCode").val(itemCode);

                var price = parseFloat(data.PRICE);
                $("#unitPrice").val(price.toFixed(2));
                $("#quantity").val(0);
            }
        }
    });
});

function myFunction(e, deptId, mcId, mtcDate, blockNo, loomNo) {

    var conf = confirm('Are you sure?');
    if (conf) {

        var table = $('#SparepareDetailTable').DataTable();
        var correntRow = $(e).closest('tr').find('td');
        var itemCode = $(correntRow).find('input')[0].defaultValue;

        $.ajax({
            type: 'GET',
            url: 'delDetail',
            data: {
                itemCode: itemCode,
                dept: deptId,
                mc: mcId,
                mtc: mtcDate,
                block: blockNo,
                loom: loomNo
            },
            success: function (data) {

                if (data == null) {

                    var removingRow = $(e).closest('tr');
                    table.row(removingRow).remove().draw();
                } else {

                    var removingRow = $(e).closest('tr');
                    table.row(removingRow).remove().draw();
                }
            }
        });
    }
}

function checkMonth() {
    var machine = $('#mtcNo').val();
    var maintenance = $('#mtc_action_name').val();

    if (machine == '' || maintenance == '') {

        alert('Machine No or Maintenance Action not yet input');

        return false;
    } else {

        var start = $('#mtcStart').val();
        var finish = $('#mtcFinish').val();

        //validate month start and finish maintenance
        var mStart = start.substring(5, 3);
        var mFinish = finish.substring(5, 3);

        if (mStart == mFinish) {

            //validate date start and finish maintenance
            var dStart = start.substring(0, 2);
            var dFinish = finish.substring(0, 2);

            if (dFinish == dStart) {

                //validate time start and finish maintenance
                var tStart = start.substring(11);
                var tFinish = finish.substring(11);

                if (tFinish < tStart) {

                    alert('Input time on Start Maintenance or Finish Maintenance not correct');

                    return false;
                } else {

                    return true;
                }
            } else if (dFinish < dStart) {

                alert('Input date on Start Maintenance or Finish Maintenance not correct');

                return false;
            } else {

                return true;
            }
        } else {

            alert('Input month on Start Maintenance between Finish Maintenance must be same');

            return false;
        }
    }
}

function deleteRow(r) {
    //var i = r.parentNode.parentNode.rowIndex;
    //document.getElementById("SparepareDetailTable").deleteRow(i);

    //$(r).closest("tr").remove().draw();

    var dt = $('#SparepareDetailTable').DataTable();
    dt.row($(r).parents('tr')).remove().draw();
}

$("#mtcNo").select2({ width: '100%' });
//$("#Fullname").select2({ width: '100%' });
$("#mtc_action_name").select2({ width: '100%' });
$("#spareParts").select2({ width: '100%', dropdownParent: $("#SparepartModal") });

$('#scheduleDate').on('change', function () {

    var url = 'getDataMachine';
    var sch = this.value;

    if (_vSch != sch) {

        _vSch = sch;

        $.ajax({
            type: 'GET',
            url: '' + url + '',
            data: {
                sch: '' + sch + ''
            },
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {

                $('#mtcNo option').remove();
                $('#mtcNo').append('<option selected="selected" value="">-- PLEASE SELECT --</option>');

                for (let i = 0; i < result.length; i++) {

                    $('#mtcNo').append('<option selected="selected" value="' + result[i].mtc + '">' + result[i].mtc + '</option>');
                }

                document.getElementById('mtcNo').selectedIndex = 0;
                $('#mtcNo').change();
            },
            error: function (e) {
                if (typeof funcCallError === 'undefined') {
                    alert(e);
                }
                else {
                    alert(funcCallError());
                }
            }
        });
    }
});

//function setMachine(txt) {

//    var url = 'getDataMachine';
//    var sch = txt.value;
   
//    getDataMachine(url, sch)
//}

//function getDataMachine(url, sch) {

//    $(_vLoading).modal('show');

//    $.ajax({
//        type: 'GET',
//        url: '' + url + '',
//        data: {
//            sch: '' + sch + ''
//        },
//        dataType: 'json',
//        contentType: 'application/json',
//        success: function (result) {

//            $('#mtcNo option').remove();
//            $('#mtcNo').append('<option selected="selected" value="">-- PLEASE SELECT --</option>');

//            for (let i = 0; i < result.length; i++) {

//                $('#mtcNo').append('<option selected="selected" value="' + result[i].mtc + '">' + result[i].mtc + '</option>');
//            }

//            document.getElementById('mtcNo').selectedIndex = 0;
//            $('#mtcNo').change();

//            $(_vLoading).modal('hide');
//        },
//        error: function (e) {
//            if (typeof funcCallError === 'undefined') {
//                alert(e);

//                $(_vLoading).modal('hide');
//            }
//            else {
//                alert(funcCallError());

//                $(_vLoading).modal('hide');
//            }
//        }
//    });
//}

$(document).ready(function () {

    $("#GetSparepart").click(function () {

        let counter = 0;
        var spareParts = $('#spareParts').val();
        var itemCode = $('#itemCode').val();
        var price = $('#unitPrice').val();
        var quantity = $('#quantity').val();
        var duplicate = "";

        if (spareParts != '') {
            if (quantity == 0 || quantity == "") {

                alert('Cannot input zero or blank on quantity spare part');
            } else {
                $('#SparepareDetailTable tbody tr td input').each(function () {
                    var cellValue = $(this)[0].defaultValue;
                    if (cellValue == itemCode) {
                        duplicate = "true";
                    }
                });

                if (duplicate == "true") {

                    alert("Duplicate data");
                } else {

                    var oTableAction = $('#SparepareDetailTable').DataTable();
                    counter = oTableAction.rows().count() + 1;

                    var htmlNo = '<tr><td style="text-align:center;">' + (counter) + '</td>';
                    var htmlItem = '<td style="width:60%;">' + spareParts + '<input type="hidden" name="Detail_2' + '[' + String(counter) + '].item_name" value="' + itemCode + '" class="form-control" />' + '</td>';
                    var htmlPrice = '<td><input type="text" name="Detail_2' + '[' + String(counter) + '].price" value="' + price + '" class="form-control text-center" />' + '</td>';
                    var htmlQuantity = '<td><input type="text" name="Detail_2' + '[' + String(counter) + '].quantity" value="' + quantity + '" class="form-control text-center" />' + '</td>';

                    //var htmlAction = '<td style="text-align:center"><button class="btn btn-danger" id="btnAction" name="btnAction" onclick="return this.parentNode.parentNode.remove();">Delete</button></td></tr>';
                    var htmlAction = '<td style="text-align:center"><button class="btn btn-danger" id="btnAction" name="btnAction" onclick="deleteRow(this)">Delete</button></td></tr>';

                    oTableAction.row.add([htmlNo, htmlItem, htmlPrice, htmlQuantity, htmlAction]).draw(false);

                    // clear input on modal
                    //document.getElementById('spareParts').selectedIndex = 1;
                    //$('spareParts').change();
                    $('#unitPrice').val('');
                    $('#quantity').val('');
                }
            }
        } else {

            alert('Input Spare Part not correct');
        }
    });

});