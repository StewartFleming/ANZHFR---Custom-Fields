
var ANZHFR = {};

ANZHFR.Loader = CLoader;



/**************************************************** Field Error Methods***********************************************************/
function SetErrorMessage(fieldId, message) {

    jQuery(".field-validation-valid").each(function (i) {
        var t = jQuery(this).attr("data-valmsg-for");
        if (t == fieldId) {
            //alert(message);
            jQuery(this).removeClass("field-validation-valid");
            jQuery(this).addClass("field-validation-error");
            jQuery("#" + fieldId).addClass("input-validation-error");

            jQuery(this).html('<span for="' + fieldId + '" generated="true" class="">' + message + '</span>');

        }
    });
}

function RemoveErrorMessage(fieldId) {

    jQuery(".field-validation-error").each(function (i) {
        var t = jQuery(this).attr("data-valmsg-for");
        if (t == fieldId) {
            //alert(message);
            jQuery(this).removeClass("field-validation-error");
            jQuery(this).addClass("field-validation-valid");
            jQuery(this).html('');
            jQuery("#" + fieldId).removeClass("input-validation-error");
        }
    });
}
/**************************************************** Field Error Methods end ***********************************************************/

function GetReturnPageNumber(isLastChk) {

    var currentPage = jQuery(".pagination .active a").html();
    
    if (isLastChk) {
        var rows = jQuery(".table tbody tr").length;

        if (currentPage > 1 && rows == 1) {
            currentPage = currentPage - 1;
        }
    }

    if (isNaN(currentPage) || currentPage == "" || currentPage == null)
        currentPage = 1;

    return currentPage;
}
/***************************************************************************************************************/

function EncodeQueryData(data) {
    var ret = [];
    for (var d in data)
        ret.push(encodeURIComponent(d) + "=" + encodeURIComponent(data[d]));
    return ret.join("&");
}


String.format = function () {
	var s = arguments[0];
	for (var i = 0; i < arguments.length - 1; i++) {
		var reg = new RegExp("\\{" + i + "\\}", "gm");
		s = s.replace(reg, arguments[i + 1]);
	}
	return s;
};







function loadSampleData(a, b) {
	$('.label-width100 .editor-field').each(function (index, el) {
		$(this).find('input, textarea').val($('.label-width100 .editor-label:eq(' + index + ') label').html().substring(0, 3) + " " + a);
	});

	$('.label-width100 select').val(b);
	$('.label-width100 input[type=number]').val(b);
}