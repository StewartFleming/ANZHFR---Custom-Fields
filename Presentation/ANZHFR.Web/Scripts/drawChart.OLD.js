﻿function drawChart(chartdiv, reportType, month, year) {
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

            //var options = {
            //    bars: 'vertical',
            //    bar: { groupWidth: '80%' },
            //    vAxis: {
            //        viewWindow: {
            //            min: 0,
            //            max: maxValue
            //        },
            //        format: '#.##'
            //    },
            //    height: 400
            //};

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
                    format: '#.##'
                },
                height: 400
            };

            if (reportType == 'length-of-stay2' || reportType == 'total-entered2') {
                var chart = new google.visualization.LineChart(document.getElementById(chartdiv))
                chart.draw(dataTable, options);
            }
            else {
                //var chart = new google.visualization.ColumnChart(document.getElementById(chartdiv))
                var chart = new google.visualization.LineChart(document.getElementById(chartdiv))
                chart.draw(dataTable, options);
            }
        }
        else {
            document.getElementById(chartdiv).innerHTML = "<p><strong>Records are not available.</strong></p>";
        }
    });
}