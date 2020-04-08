$('#OptedOut').on('ifChecked', function (event) { optedOutChanged(true); });

$('#OptedOut').on('ifUnchecked', function (event) { optedOutChanged(false); });

$('#CannotFollowup').on('ifChecked', function (event) { cannotFollowupChanged(true); });

$('#CannotFollowup').on('ifUnchecked', function (event) { cannotFollowupChanged(false); });

function optedOutChanged(checked) {
    if (checked) {
        $('#Tabs').hide();
    } else {
        $('#Tabs').show();
    }
}

function cannotFollowupChanged(checked) {
    if (checked) {
        $('#CannotFollowUpText').show();
    } else {
        $('#CannotFollowUpText').hide();
    }
}

$(".forcompleteness").each(function () {
    if ($(this).val() == "") {
        $(this).addClass("forcompleteness_required");
    }
});

$(function () {
    $(document).on('change', '#FractureSideQuick', function () {
        $('#FractureSideTopField').hide();
        $('#FractureSideMsg').show();
        $('#FractureSide').val($('#FractureSideQuick').val());
    });
});

$(function () {
    $(document).on('change', '#Medicare', function () {
        if (country == "New Zealand") {
            if ($(this).val() != "" && !is_nz_nhi($(this).val().toUpperCase())) {
                BootstrapDialog.show({
                    title: 'Invalid NHI number',
                    message: $(this).val().toUpperCase() + ' is an invalid NHI number.',
                    type: BootstrapDialog.TYPE_DANGER,
                    buttons: [{
                        label: 'Close',
                        action: function (dialogItself) {
                            dialogItself.close();
                        }
                    }]
                });
                $(this).val('');
            } else {
                $(this).val($(this).val().toUpperCase());
                if ($('#EventNumber').val() == '') {
                    $('#EventNumber').val($('#Medicare').val());
                }
            }
        } else {
            if ($(this).val() != "" && !is_medicare($(this).val())) {
                BootstrapDialog.show({
                    title: 'Invalid Medicare number',
                    message: $(this).val() + ' is an invalid Medicare number.',
                    type: BootstrapDialog.TYPE_DANGER,
                    buttons: [{
                        label: 'Close',
                        action: function (dialogItself) {
                            dialogItself.close();
                        }
                    }]
                });
                $(this).val('');
            };
        }
    })
});

// Description: Returns true if a Medicare passes checksum validation, otherwise returns false.
function is_medicare(MN) {
    var medicareNumber;
    var pattern;
    var length;
    var matches;
    var base;
    var checkDigit;
    var total;
    var multipliers;
    var isValid;

    pattern = /^(\d{8})(\d)/;
    medicareNumber = MN.toString().replace(/ /g, '');
    length = 11;

    if (medicareNumber.length === length) {
        matches = pattern.exec(medicareNumber);
        if (matches) {
            base = matches[1];
            checkDigit = matches[2];
            total = 0;
            multipliers = [1, 3, 7, 9, 1, 3, 7, 9];

            for (var i = 0; i < multipliers.length; i++) {
                total += base[i] * multipliers[i];
            }

            isValid = (total % 10) === Number(checkDigit);
        } else {
            isValid = false;
        }
    } else {
        isValid = false;
    }

    return isValid;
};

// Description: Returns true if a New Zealand NHI passes checksum validation, otherwise returns false.
function is_nz_nhi(NHI) {
    var passTest = false;
    // Validation steps 1 and 2
    if (/^[A-HJ-NP-Z]{3}[0-9]{4}$/.test(NHI)) {
        var checkValue = 0;
        // Init alpha conversion table omitting O and I. A=1~Z=24
        var aplhaTable = "ABCDEFGHJKLMNPQRSTUVWXYZ".split('');
        for (i = 0; i < 3; i++) {
            // Convert each letter to numeric value from table above
            var letterNumeric = aplhaTable.indexOf(NHI[i]) + 1
            // Multiply numeric value by 7-i and add to the checkvalue
            checkValue += letterNumeric * (7 - i);
        }
        for (i = 3; i < 6; i++) {
            // Multiply first three numbers by 7-i and add to the checkvalue
            checkValue += NHI[i] * (7 - i);
        }
        // Apply modulus 11 to the sum of the above numbers (checkValue)
        var checkSum = checkValue % 11;
        // If the checksum is not 0, subtract the checksum from 11
        // to create checkdigit. Mod 10 to convert 10 to 0.
        if (checkSum !== 0) {
            if (NHI[6] == (11 - checkSum) % 10) {
                // if the last digit of the NHI matches the checkdigit, NHI is valid
                passTest = true;
            }
        }
    }
    return passTest;
}

// Set Max and Min Values based on Start Date
$(function () {
    $('#ArrivalDateTime').datetimepicker();
    $('#ArrivalDateTime').data("DateTimePicker").setMaxDate(moment());

    $('#DepartureDateTime').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#DepartureDateTime').data("DateTimePicker").setMaxDate(moment());

    $('#SurgeryDateTime').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#SurgeryDateTime').data("DateTimePicker").setMaxDate(moment());

    $('#WardDischargeDate').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#WardDischargeDate').data("DateTimePicker").setMaxDate(moment());

    $('#HospitalDischargeDate').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#HospitalDischargeDate').data("DateTimePicker").setMaxDate(moment());

    $('#FollowupDate30').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#FollowupDate30').data("DateTimePicker").setMaxDate(moment());

    $('#FollowupDate120').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });
    $('#FollowupDate120').data("DateTimePicker").setMaxDate(moment());

    $("#ArrivalDateTime").on("dp.hide", function (e) {
        if ($('#DepartureDateTime').val() == '')
        {
            $('#DepartureDateTime').val($("#ArrivalDateTime").val());
        };
        $('#DepartureDateTime').data("DateTimePicker").setMinDate(e.date.add(-1, 'days'));
        $('#SurgeryDateTime').data("DateTimePicker").setMinDate(e.date);
    });
});

$(document).ready(function () {
    
    $("input").focus(function () {
        $(this).parent().addClass("curFocus")
    });
    $("input").blur(function () {
        $(this).parent().removeClass("curFocus")
    });
    CalculateAge();
    CalculateFollowups();
    AdjustForYear();
});

function CalculateAge() {
    if ($('#StartDate').val() != '' && $('#DOB').val() != '') {
        var startDate = parseDateTime($('#StartDate').val());
        var dob = parseDate($('#DOB').val());
        var age = Math.floor((startDate - dob) / (24 * 3600 * 1000) / 365);
        $('#AgeText').text('Age at admission: ' + age);
        if (age > 110 || age < 50) {
            BootstrapDialog.show({
                title: 'Age Warning',
                message: age + ' looks like an invalid age.<br /> <br />Please check the following:<br /><br />' +
                    'Date of Birth: ' + formatDate(dob) + '<br />' +
                    'Admission Date / time: ' + formatDateTime(startDate),
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    }
    else if ($('#DOB').val() != '') {
        var startDate = Date.now();
        var dob = parseDate($('#DOB').val());
        var age = Math.floor((startDate - dob) / (24 * 3600 * 1000) / 365);
        $('#AgeText').text('Age at admission: ' + age);
        if (age > 110 || age < 50) {
            BootstrapDialog.show({
                title: 'Age Warning',
                message: age + ' looks like an invalid age.<br /> <br /> Please check the following:<br /><br />' +
                    'Date of Birth: ' + formatDate(dob) + '<br />' +
                    'Admission Date / time: ' + formatDateTime(startDate) + ' (assuming now)',
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    }
    if ($('#DOB').val() == "") { $('#AgeText').text(''); }
}

function CalculateTimeToSurgery() {
    if ($('#StartDate').val() != '' && $('#SurgeryDateTime').val() != '') {
        var startDate = parseDateTime($('#StartDate').val());
        var surgeryDate = parseDateTime($('#SurgeryDateTime').val());
        var timetoSurgery = Math.floor((surgeryDate - startDate) / (3600 * 1000));
        $('#SurgeryText').text('Time to surgery: ' + timetoSurgery + ' hours');
        if (timetoSurgery < 1) {
            BootstrapDialog.show({
                title: 'Time To Surgery - WARNING ONLY',
                message: 'The Time to Surgery of <strong>' + timetoSurgery + ' hours</strong> looks wrong.<br /> <br /> Please check the following: <br /><br />' +
                    'Admission Date / Time: (' + formatDateTime(startDate) + ')<br />' +
                    'Surgery Date / Time: &nbsp; &nbsp; (' + formatDateTime(surgeryDate) + ').',
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
        if (timetoSurgery > 96) {
            BootstrapDialog.show({
                title: 'Time To Surgery - WARNING ONLY',
                message: 'The Time to Surgery of <strong>' + timetoSurgery + ' hours</strong> is greater than the 96 hour warning level.<br /> <br /> Please check the following:<br /><br />' +
                    'Admission Date / Time: (' + formatDateTime(startDate) + ')<br />' +
                    'Surgery Date / Time: &nbsp; &nbsp; (' + formatDateTime(surgeryDate) + ').',
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    }
    else if ($('#StartDate').val() == '') {
        BootstrapDialog.show({
            title: 'Time To Surgery - WARNING ONLY',
            message: 'The Time to Surgery cannot be calculated.<br /> <br />Please add the Admission Date/Time.',
            type: BootstrapDialog.TYPE_DANGER,
            buttons: [{
                label: 'Close',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });
    }
}

function CalculateFollowups() {
    if ($('#StartDate').val() != '') {
        var startDate = Date.parse($('#StartDate').val());
        if (startDate.getFullYear() <= 2018) {
            $('#ExpectedFollowup30String').val(formatDate(startDate.addDays(30)));
        }
        startDate = Date.parse($('#StartDate').val());
        $('#ExpectedFollowup120String').val(formatDate(startDate.addDays(120)));
    }
}

function AdjustForYear() {
    if ($('#StartDate').val() != '') {
        startDate = Date.parse($('#StartDate').val());
        if (startDate.getFullYear() <= 2018) {
            $('#TAB6').show();
            $('#malnutrition').hide();
            $('#Weight120').show();
            $('#FirstDayWalking').hide();
        }
        if (startDate.getFullYear() > 2018) {
            $('#TAB6').hide();
            $('#malnutrition').show();
            $('#Weight120').show();
            $('#FirstDayWalking').hide();
        }
        if (startDate.getFullYear() > 2019) {
            $('#Weight120').hide();
            $('#FirstDayWalking').show();
        }
    }
    else
    {
        $('#Weight120').hide();
        $('#FirstDayWalking').show();
    }
}

function AdmissionChange() {
    $('#StartDate').val($('#ArrivalDateTime').val());
    if ($('#AdmissionViaED').val() == '1') {
        $('#ED').show();
        $('#inpatient').hide();
        $('#transfer').hide();
        $('#StartDate').val($('#ArrivalDateTime').val());
    } else if ($('#AdmissionViaED').val() == '2') {
        $('#ED').show();
        $('#inpatient').hide();
        $('#transfer').show();
        $('#StartDate').val($('#TransferDateTime').val());
    } else if ($('#AdmissionViaED').val() == '3') {
        $('#ED').hide();
        $('#inpatient').show();
        $('#transfer').hide();
        $('#StartDate').val($('#InHospFractureDateTime').val());
    } else {
        $('#ED').show();
        $('#inpatient').hide();
        $('#transfer').hide();
        $('#StartDate').val($('#ArrivalDateTime').val());
        echo("No Admission Via ED value");
    }
    CalculateAge();
    CalculateFollowups();
    AdjustForYear();
   

    if ($('#DepartureDateTime').val() != "" && $('#ArrivalDateTime').val() != "") {
        var startDate = parseDateTime($('#ArrivalDateTime').val());
        var endDate = parseDateTime($('#DepartureDateTime').val());
        var EDTime = Math.floor((endDate - startDate) / (3600 * 1000));
        $('#EDText').text('Time in ED: ' + EDTime + ' hours');
        if (startDate > endDate) {
            BootstrapDialog.show({
                title: 'Time in ED - WARNING ONLY',
                message: 'The ED Arrival Date is greater than the ED Departure Date.<br /><br />Please check the following:<br /><br />' +
                    'Admission Date/Time: ' + formatDateTime(startDate) + '<br />' +
                    'Departure Date/time: ' + formatDateTime(endDate),
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
        else if (EDTime > 8) {
            BootstrapDialog.show({
                title: 'Time in ED Warning',
                message: 'The time in ED of <strong>' + EDTime + ' hours</strong> is greater than the 8 hour warning limit.<br /><br />Please check the following:<br /><br />' +
                    'Admission Date/Time: ' + formatDateTime(startDate) + '<br />' +
                    'Departure Date/time: ' + formatDateTime(endDate),
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    }
}

function DepartureChange() {
    if ($('#DepartureDateTime').val() != "" && $('#ArrivalDateTime').val() != "") {
        var startDate = parseDateTime($('#ArrivalDateTime').val());
        var endDate = parseDateTime($('#DepartureDateTime').val());
        var EDTime = Math.floor((endDate - startDate) / (3600 * 1000));
        $('#EDText').text('Time in ED: ' + EDTime + ' hours');
        if (startDate > endDate) {
            BootstrapDialog.show({
                title: 'Time in ED - WARNING ONLY',
                message: 'The ED Arrival Date is greater than the ED Departure Date.<br /><br />Please check the following:<br /><br />' +
                    'Admission Date/Time: ' + formatDateTime(startDate) + '<br />' +
                    'Departure Date/time: ' + formatDateTime(endDate),
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
        else if (EDTime > 8) {
            BootstrapDialog.show({
                title: 'Time in ED - WARNING ONLY',
                message: 'The time in ED of <strong>' + EDTime + ' hours</strong> is greater than the 8 hour warning level.<br /> <br />Please check the following:<br /><br />' +
                    'Admission Date/Time: ' + formatDateTime(startDate) + '<br />' +
                    'Departure Date/time: ' + formatDateTime(endDate),
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    }
    if ($('#DepartureDateTime').val() == "") { $('#EDText').text('');}
}

function UpdateDeath() {
    if ($('#StartDate').val() != '') {
        var startDate = parseDateTime($('#StartDate').val());
        if (startDate.getFullYear() > 2019) {
            if ($('#DischargeDest').val() == '6' || $('#DischargeResidence').val() == '3' || $('#Survival120').val() == '0') {
                $('#DeceasedDate').show(); $('#DeceasedWarning').show(); $('#DeceasedWarning120').show(); $('#120DaySurvival').hide();
            } else {
                $('#DeceasedDate').hide(); $('#DeceasedWarning').hide(); $('#DeceasedWarning120').hide(); $('#120DaySurvival').show();
            }
        }
        else {
            if ($('#DischargeDest').val() == '6' || $('#DischargeResidence').val() == '3' || $('#Survival120').val() == '0') {
                $('#DeceasedDate').hide(); $('#DeceasedWarning').show(); $('#DeceasedWarning120').show(); $('#120DaySurvival').hide();
            } else {
                $('#DeceasedDate').hide(); $('#DeceasedWarning').hide(); $('#DeceasedWarning120').hide(); $('#120DaySurvival').show();
            }
        }
    } else {
        if ($('#Survival120').val() == '0') {
            $('#DeceasedWarning').show(); $('#DeceasedWarning120').show(); $('#120DaySurvival').hide();
        } else {
            $('#DeceasedWarning').hide(); $('#DeceasedWarning120').hide(); $('#120DaySurvival').show();
        }
    }
}

function formatDate(d) {
    var m_names = new Array("Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec");

    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    return (parseInt(curr_date) < 10 ? ('0' + curr_date) : curr_date) + "-" + m_names[curr_month] + "-" + curr_year;
}

function formatDateTime(d) {
    var m_names = new Array("Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec");

    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var curr_hours = d.getHours();
    var curr_min = d.getMinutes();
    return (parseInt(curr_date) < 10 ? ('0' + curr_date) : curr_date) + "-" + m_names[curr_month] + "-" + curr_year + " " + curr_hours + ":" + curr_min.padLeft();
}

function calcALengthOfStay() {
    AdmissionChange();
    if ($('#StartDate').val() != '' && $('#WardDischargeDate').val() != '') {
        var startDate = parseDateTime($('#StartDate').val());
        var wDischargeDate = parseDate($('#WardDischargeDate').val());
        var dischargeLOS = Math.floor((wDischargeDate - startDate) / (24 * 3600 * 1000));
        $('#AcuteLOSText').text('Acute Length of Stay: ' + dischargeLOS + ' days');
        $('#WLengthofStay').val(dischargeLOS.toFixed(0));
        if (dischargeLOS < 1 || dischargeLOS > 14) {
            BootstrapDialog.show({
                title: 'Acute Length of Stay - WARNING ONLY',
                message: 'The Acute Length of Stay of <strong>' + dischargeLOS + ' days</strong> is outside the warning level of < 1 and > 14 days.<br /> <br /> Please check the following: <br /><br />' +
                    'Admission Date / Time: &nbsp; (' + formatDateTime(startDate) + ')<br />' +
                    'Acute Discharge Date / Time: (' + formatDate(wDischargeDate) + ').',
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    };
    if ($('#StartDate').val() == '' || $('#WardDischargeDate').val() == '') { $('#AcuteLOSText').text(''); };
}

function calcHLengthOfStay() {
    AdmissionChange();
    if ($('#StartDate').val() != '' && $('#HospitalDischargeDate').val() != '') {
        var startDate = parseDateTime($('#StartDate').val());
        var hDischargeDate = parseDate($('#HospitalDischargeDate').val());
        var dischargeLOS = Math.floor((hDischargeDate - startDate) / (24 * 3600 * 1000));
        $('#HospitalLOSText').text('Hospital Length of Stay: ' + dischargeLOS + ' days');
        $('#HLengthofStay').val(dischargeLOS.toFixed(0));
        if (dischargeLOS < 1 || dischargeLOS > 35) {
            BootstrapDialog.show({
                title: 'Hospital Length of Stay - WARNING ONLY',
                message: 'The Hospital Length of Stay of <strong>' + dischargeLOS + ' days</strong> is outside the warning levels of < 1 and > 35 days.<br /> <br /> Please check the following: <br /><br />' +
                    'Admission Date / Time:&nbsp; (' + formatDateTime(startDate) + ')<br />' +
                    'Hospital Discharge Date: (' + formatDate(hDischargeDate) + ').',
                type: BootstrapDialog.TYPE_DANGER,
                buttons: [{
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }
    };
    if ($('#StartDate').val() == '' || $('#HospitalDischargeDate').val() == '') { $('#HospitalLOSText').text(''); };
}

$('.date-only').datetimepicker({
    format: 'dd-MMM-yyyy',
    pickTime: false
});

$('.date-time').datetimepicker({
    format: 'dd-MMM-yyyy HH:mm',
    pickTime: true
});

function parseDate(s) {
    var months = {
        jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5,
        jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11
    };
    var p = s.split('-');
    return new Date(p[2], months[p[1].toLowerCase()], p[0]);
};

function parseDateTime(s) {
    var months = {
        jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5,
        jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11
    };
    var dt = s.replace(/\./g, ':').split(' ');
    var d = dt[0].split('-');
    var t = dt[1].split(':');
    return new Date(d[2], months[d[1].toLowerCase()], d[0], t[0], t[1], 0, 0);
};

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

jQuery("#cancel-btn").on("click", function (event) {
    event.preventDefault();

    var data = { 'page': jQuery("#Page").val(), 'search': jQuery("#FilterSearch").val() };
    window.location.href = "/patient?" + EncodeQueryData(data);
})

jQuery("#qcancel-btn").on("click", function (event) {
    event.preventDefault();

    var data = { 'page': jQuery("#Page").val(), 'search': jQuery("#FilterSearch").val() };
    window.location.href = "/quality?" + EncodeQueryData(data);
})

jQuery("#delete-btn").on("click", function (event) {
    event.preventDefault();
    jQuery("#DeleteItemId").val(jQuery(this).attr("item-id"));
});

jQuery("#con-delete-btn").on("click", function (event) {
    event.preventDefault();
    var data = { 'page': jQuery("#Page").val(), 'search': jQuery("#FilterSearch").val() };
    window.location.href = "/patient/delete/" + jQuery("#DeleteItemId").val() + "?" + EncodeQueryData(data);
});

jQuery("#qcon-delete-btn").on("click", function (event) {
    event.preventDefault();
    var data = { 'page': jQuery("#Page").val(), 'search': jQuery("#FilterSearch").val() };
    window.location.href = "/quality/delete/" + jQuery("#DeleteItemId").val() + "?" + EncodeQueryData(data);
});

$('.btnNext').click(function () {
    if ($('#StartDate').val() != '') {
        var startDate = Date.parse($('#StartDate').val());
        if (startDate.getFullYear() > 2018 && $('.nav-tabs > .active').next('li').find('a').attr('href') == '#tab_6') {
            $('.nav-tabs > .active').next('li').find('a').trigger('click');
            $('.nav-tabs > .active').next('li').find('a').trigger('click');
        }
        else {
            $('.nav-tabs > .active').next('li').find('a').trigger('click');
        }
    }
    else {
        $('.nav-tabs > .active').next('li').find('a').trigger('click');
    }
});

$('.btnPrevious').click(function () {
    if ($('#StartDate').val() != '') {
        var startDate = Date.parse($('#StartDate').val());
        if (startDate.getFullYear() > 2018 && $('.nav-tabs > .active').prev('li').find('a').attr('href') == '#tab_6') {
            $('.nav-tabs > .active').prev('li').find('a').trigger('click');
            $('.nav-tabs > .active').prev('li').find('a').trigger('click');
        }
        else {
            $('.nav-tabs > .active').prev('li').find('a').trigger('click');
        }
    }
    else {
        $('.nav-tabs > .active').prev('li').find('a').trigger('click');
    }
});

$(function () {
    var tooltips = $("[title]").tooltip({
        position: {
            my: "left top",
            at: "right+5 top-5"
        }
    });
});

$('.my_tooltip').tooltip({ html: true });