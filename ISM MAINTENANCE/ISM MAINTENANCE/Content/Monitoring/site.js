
$('#periodFrom').datetimepicker({
    format: 'd-m-Y',
    timepicker: false,
    datepicker: true
});

$('#periodTo').datetimepicker({
    format: 'd-m-Y',
    timepicker: false,
    datepicker: true
});

$("#troubleName").dropdownchecklist({ firstItemChecksAll: true });

document.getElementById('graphicType').selectedIndex = 0;
$('#graphicType').change();
document.getElementById('troubleName').selectedIndex = 0;
$('#troubleName').change();
//document.getElementById('typeOfChart').selectedIndex = 1;
//$('#typeOfChart').change();
document.getElementById('maintenanceType').selectedIndex = 0;
$('#maintenanceType').change();

var title = 'Total Trouble ( Machine )';
var chartName = 'myChart';

var myChart = new Chart($('#' + chartName), {
    type: 'bar',
    options: {
        //responsive: true, 
        //maintainAspectRatio: false,
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

var newLegendClickHandlerTemp = function (e, legendItem) {

    alert('click');
}

function getRandomRgb() {

    var num = Math.round(0xffffff * Math.random());
    var r = num >> 16;
    var g = num >> 8 & 255;
    var b = num & 255;
    return 'rgb(' + r + ', ' + g + ', ' + b + ', 100)';
}

$('#graphicType').on('change', function () {

    var grap = $(this).val();

    vBlock = 'block';
    vNone = 'none';

    if (grap == 'Trend' || grap == 'Progress') {

        document.getElementById('labelTrouble').style.display = vNone;
        document.getElementById('valueTrouble').style.display = vNone;
    } else {

        document.getElementById('labelTrouble').style.display = vBlock;
        document.getElementById('valueTrouble').style.display = vBlock;
    }

    if (grap == 'Progress') {

        $('#maintenanceType').val('Preventive');
        $('#maintenanceType').change();

        document.getElementById('maintenanceType').disabled = true;
    } else {

        document.getElementById('maintenanceType').selectedIndex = 0;
        $('#maintenanceType').change();

        document.getElementById('maintenanceType').disabled = false;
    }
});

$('#periodFrom').on('change', function () {

    var url = 'getDataTrouble';
    var from = $('#periodFrom').val();
    var to = $('#periodTo').val();
    var trouble = 'trouble';
    //var chk = $("input[type='radio'][name='flexRadioDefault']:checked").val();

    var dailyTrend = $("#graphicType").val();
    var main = $("#maintenanceType").val();
    if (dailyTrend == '') { main = main; } else { main = main + '_' + dailyTrend; }

    getDataTrouble(url, from, to, trouble, main)
});

$('#periodTo').on('change', function () {

    var url = 'getDataTrouble';
    var from = $('#periodFrom').val();
    var to = $('#periodTo').val();
    var trouble = 'trouble';
    //var chk = $("input[type='radio'][name='flexRadioDefault']:checked").val();

    var dailyTrend = $("#graphicType").val();
    var main = $("#maintenanceType").val();
    if (dailyTrend == '') { main = main; } else { main = main + '_' + dailyTrend; }

    getDataTrouble(url, from, to, trouble, main)
});

//$('#radioQuality').on('click', function () {

//    var url = 'getDataTrouble';
//    var from = $('#periodFrom').val();
//    var to = $('#periodTo').val();
//    var trouble = 'trouble';
//    var chk = $(this).val();

//    getDataTrouble(url, from, to, trouble, chk)
//});

//$('#radioMachine').on('click', function () {

//    var url = 'getDataTrouble';
//    var from = $('#periodFrom').val();
//    var to = $('#periodTo').val();
//    var trouble = 'trouble';
//    var chk = $(this).val();

//    getDataTrouble(url, from, to, trouble, chk)
//});

$('#maintenanceType').on('change', function () {

    var url = 'getDataTrouble';
    var from = $('#periodFrom').val();
    var to = $('#periodTo').val();
    var trouble = 'trouble';
    var main = $(this).val();

    if (main == 'Machine' || main == 'Quality') {

        document.getElementById('typeChart').innerText = 'Trouble Name (*)';
    } else {

        document.getElementById('typeChart').innerText = 'Spare Part Name (*)';
    }

    var dailyTrend = $("#graphicType").val();
    if (dailyTrend == '') { main = main; } else { main = main + '_' + dailyTrend; }

    getDataTrouble(url, from, to, trouble, main)
});

function getDataTrouble(url, from, to, trouble, chk) {
    $.ajax({
        type: 'GET',
        url: '' + url + '',
        data: {
            periodFrom: '' + from + '',
            periodTo: '' + to + '',
            trouble: trouble,
            chk: chk
        },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {

            $('#troubleName option').remove();
            $('#troubleName').append('<option selected="selected" value="ALL">ALL</option>');

            for (let i = 0; i < result.length; i++) {

                $('#troubleName').append('<option selected="selected" value="' + result[i].troubleId + '">' + result[i].troubleName + '</option>');
            }

            $("#troubleName").dropdownchecklist("destroy");
            $("#troubleName").dropdownchecklist({ firstItemChecksAll: true });

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

$('#showChart').on('click', function () {

    var url = 'getDataChart';
    var from = $('#periodFrom').val();
    var to = $('#periodTo').val();
    const troubleName = $('#troubleName').val();
    let trouble = troubleName.toString();
    //var chk = $("input[type='radio'][name='flexRadioDefault']:checked").val();

    var dailyTrend = $("#graphicType").val();
    var chk = $("#maintenanceType").val();
    if (dailyTrend == '') { chk = chk; } else { chk = chk + '_' + dailyTrend; }

    getDataChart(url, from, to, trouble, chk, createChart)
});

function getDataChart(url, from, to, trouble, chk, funcCallBack, funcCallError) {
    $.ajax({
        type: 'GET',
        url: '' + url + '',
        data: {
            periodFrom: '' + from + '',
            periodTo: '' + to + '',
            trouble: trouble,
            chk: chk
        },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            funcCallBack(result);
        },
        error: function (e) {
            if (typeof funcCallError === 'undefined') {
                alert(e);
            }
            else {
                funcCallError();
            }
        }
    });
}

function createChart(data) {
    var type = 'bar' //$('#typeOfChart').val();

    //const datas = data;
    //let exDatas = datas.length;

    if (typeof data.data[0] == 'undefined') {

        alert('Data Not Found');
    } else {

        if (data.result != "") {

            addMultipleDataChart(myChart, data.data, type);
        } else {

            alert(data.msg);
        }
    }
}

// khusus all item unit
function addMultipleDataChart(myChart, data, type) {
    var bordColors = "";
    var bolFill = false;

    if (type == 'line') {
        bolFill = false;
        bordColors = 'navy';
    } else {
        bolFill = false;
        bordColors = 'white';
    }

    let urut = data[0].urut;
    let space = (urut / 100) * 5;
    let leftMax = parseInt(urut) + Math.ceil(parseFloat(space));
    var cTitle = '';

    var grap = $('#graphicType').val();
    if (grap == 'Progress') {

        leftMax = 100;
        cTitle = '%';
    }

    const opsi = {
        scales: {
            y: {
                beginAtZero: true,
                max: leftMax,
                title: {
                    display: true,
                    text: cTitle                    
                }
            }
        }
    };

    myChart.config.options = opsi
    myChart.config.type = type;

    var dataChart = {
        labels: [],
        datasets: []
    };

    var totTrouble = data[0].countTrouble;
    var mesin = 1;

    $.each(data, function (idx, itm) {

        dataChart.labels.push(Object.values(itm)[0]);
    });

    //var jsonData = data;
    let num = 0;
    let legend = 0;

    //untuk ambil jumlah unit nya / sesuai yg ada pada kotak kategori (co : total mesin ada 6)
    let unit = parseInt(data[0].countTrouble) + 1;
    let cumulative = unit + parseInt(data[0].countTrouble);

    for (var obj in data) {

        num = num + 1;

        // looping ini hanya untuk mengambil jumlah kolom dan nama kolom nya
        if (data.hasOwnProperty(obj)) {
            for (var prop in data[obj]) {

                legend = legend + 1;

                //num = 1 hanya untuk mengambil nama kolom nya
                if (num == 1) {

                    if (legend > 1) {

                        if (legend <= unit) {

                            var dataSet = {
                                type: type,
                                label: prop,
                                data: [],
                                backgroundColor: [],
                                borderColor: [],
                                //borderWidth: 2,
                                fill: bolFill
                            }

                            //insert values & color
                            //ini untuk looping isi data nya dengan index row dan nama kolom dari looping pertama

                            for (var key in data) {
                                if (data.hasOwnProperty(key)) {

                                    dataSet.data.push(data[key][prop]);
                                }
                            }

                            dataSet.backgroundColor.push(getRandomRgb());
                            dataSet.borderColor.push(getRandomRgb());

                            dataChart.datasets.push(dataSet);
                            myChart.data = dataChart;
                            myChart.update();
                        }
                    }
                }
            }
        }
    }
}