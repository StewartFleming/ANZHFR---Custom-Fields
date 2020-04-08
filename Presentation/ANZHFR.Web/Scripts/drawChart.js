function drawChart(chartdiv, reportType, month, year) {
    drawChart(chartdiv, reportType, month, year, false)
}

function drawChart(chartdiv, reportType, month, year, exportChart) {
    var dataTable = new google.visualization.DataTable();

    $.ajax({
        type: "POST",
        url: "/reports/GetReportData",
        data: {
            reportType: reportType,
            month: month,
            year: year
        },
        dataType: "json"
    }).done(function (data, textStatus, jqXHR) {
        if (data != null) {
            xTitle = data.xTitle;
            yTitle = data.yTitle;
            chartTitle = data.ChartTitle;
            dataTable.addColumn('string', '');
            for (var i = 0; i < data.Legends.length; i++) {
                dataTable.addColumn('number', data.Legends[i]);
            }

            if (data.Description != null && data.Description != "") {
                $("#divDescriptionArea").show();
                $("#divDescription").text(data.Description);
            } else {
                $("#divDescriptionArea").hide();
            }

            var maxValue = 10;
            if (data.Labels != null) {
                for (var i = 0; i < data.Labels.length; i++) {
                    var array = [];
                    array.push(data.Labels[i]);
                    for (var a = 0; a < data.Data.length; a++) {
                        array.push(parseFloat(data.Data[a][i]));

                        if (parseFloat(data.Data[a][i]) > maxValue) {
                            maxValue = parseFloat(data.Data[a][i]);
                        }
                    }
                    dataTable.addRow(array);
                }
            }
            else {
                //For handling "Length of Stay" report
                var array = [];
                array.push("Length of Stay");
                for (var a = 0; a < data.Data.length; a++) {
                    array.push(parseFloat(data.Data[a]));
                }
                dataTable.addRow(array);
            }

            var ticks;
            if (reportType == 'length-of-stay' || reportType == 'acute-length-of-stay') {
                maxValue = 30;
            }
            else if (reportType == 'time-to-surgery') {
                maxValue = 72;
                ticks = [0, 24, 48, 72];
            }
            else if (reportType != 'total-entered') {
                maxValue = 100;
            }

            var options = {
                bars: 'vertical',
                title: chartTitle,
                hAxis: { title: xTitle },
                vAxis: {
                    title: yTitle,
                    viewWindow: {
                        min: 0,
                        max: maxValue
                    },
                    format: '#.##',
                    ticks: ticks
                },
                height: 400
            };

            if (reportType == 'age' || reportType == 'sex' || reportType == 'fracture-type' || reportType == 'discharge-destination' || reportType == 'reason-for-delay') {
                var chart = new google.visualization.ColumnChart(document.getElementById(chartdiv))
                chart.draw(dataTable, options);
            }
            else {
                var chart = new google.visualization.LineChart(document.getElementById(chartdiv))
                chart.draw(dataTable, options);
            }

            if (exportChart) {
                var imgUri = chart.getImageURI();
                download(imgUri, chartTitle + ".png", "image/png");
            }
        }
        else {
            document.getElementById(chartdiv).innerHTML = "<p><strong>Records are not available.</strong></p>";
            $("#divDescriptionArea").hide();
        }
    });

    function downloadURI(fileContents, name) {
        var link = document.createElement("a");
        link.download = name;
        link.href = 'data:,' + fileContents;;
        link.click();
    }
}