var defaultColorsForDoughnutPiePolar = [
    {
        color: "#3d79c2",
        highlight: "#3467A5",
    },
    {
        color: "#8b30b6",
        highlight: "#6E2690",
    },
    {
        color: "#dc709a",
        highlight: "#D2477D",
    },
    {
        color: "#52b5e0",
        highlight: "#26a3d9",
    },
         {
             color: "#5b50af",
             highlight: "#51479B",
         },
         {
             color: "#46BFBD",
             highlight: "#5AD3D1",
         },
         {
             color: "#FDB45C",
             highlight: "#FFC870",
         },
         {
             color: "#949FB1",
             highlight: "#A8B3C5",
         },
         {
             color: "#4D5360",
             highlight: "#616774",
         },
        {
            color: "#003300",
            highlight: "#335C33",
        },
        {
            color: "#FFB2B2",
            highlight: "#FFCCCC",
        },
        {
            color: "#990033",
            highlight: "#B5476C",
        },
        {
            color: "#FFFF00",
            highlight: "#FFFF66",
        },
        {
            color: "#990099",
            highlight: "#C266C2",
        },

        {
            color: "#00FF00",
            highlight: "#4DFF4D",
        },
        {
            color: "#FFFFCC",
            highlight: "#FFFFF0",
        },
        {
            color: "#000066",
            highlight: "#4D4D94",
        },
        {
            color: "#FF0066",
            highlight: "#FF66A3",
        },
        {
            color: "#993300",
            highlight: "#AD5C33",
        },
        {
            color: "#FF3300",
            highlight: "#FF5C33",
        },
        {
            color: "#5C5C3D",
            highlight: "#7D7D64",
        },
        {
            color: "#00FFFF",
            highlight: "#99FFFF",
        },
        {
            color: "#5C005C",
            highlight: "#853385",
        },
        {
            color: "#E6E68A",
            highlight: "#FFFF99",
        },
        {
            color: "#000000",
            highlight: "#191919",
        },
        {
            color: "rgba(220,220,220,0.5)",
            highlight: "rgba(220,220,220,0.75)",
        },
        {
            color: "rgba(151,187,205,0.5)",
            highlight: "rgba(151,187,205,0.75)",
        },
        {
            color: "#FF0066",
            highlight: "#FF66A3",
        },
        {
            color: "#993300",
            highlight: "#AD5C33",
        },
        {
            color: "#FF3300",
            highlight: "#FF5C33",
        },
        {
            color: "#5C5C3D",
            highlight: "#7D7D64",
        },
        {
            color: "#00FFFF",
            highlight: "#99FFFF",
        },
        {
            color: "#5C005C",
            highlight: "#853385",
        },
        {
            color: "#E6E68A",
            highlight: "#FFFF99",
        },
        {
            color: "#000000",
            highlight: "#191919",
        },
        {
            color: "rgba(220,220,220,0.5)",
            highlight: "rgba(220,220,220,0.75)",
        },
        {
            color: "rgba(151,187,205,0.5)",
            highlight: "rgba(151,187,205,0.75)",
        },
];

var defaultColorsForBarLineRadar = [
    {
        lightColor: "#3d79c2",
        darkColor: "#3467A5",
    },
    {
        lightColor: "#8b30b6",
        darkColor: "#6E2690",
    },
    {
        lightColor: "#dc709a",
        darkColor: "#D2477D",
    },
    {
        lightColor: "#52b5e0",
        darkColor: "#2DA5DA",
    },
    {
        lightColor: "#5b50af",
        darkColor: "#51479B",
    },
];

function RenderChartOnClick(divChartId, title, chartType, dataSource, depth, dataSourceServiceUrl, arrayOfColors, breadCrumbClassName, fontFamily, emptyDataMsg) {

    var chartId = "chart" + divChartId;

    if (title != null && title != '')
        $('#' + divChartId).append("<ul class='" + breadCrumbClassName + "'></ul><canvas id='chart" + divChartId + "' width='400' height='200' />");
    else
        $('#' + divChartId).append("<ul style='display:none;' class='" + breadCrumbClassName + "'></ul><canvas id='chart" + divChartId + "' width='400' height='200' />");

    RenderChart(chartId, divChartId, title, chartType, dataSource, depth, dataSourceServiceUrl, arrayOfColors, fontFamily, emptyDataMsg);
}

function RenderChart(chartId, divChartId, title, chartType, dataSource, depth, dataSourceServiceUrl, arrayOfColors, fontFamily, emptyDataMsg) {
    
    var checkDataSource = $.parseJSON(dataSource);

    var check = 0;

    for (var i = 0; i < checkDataSource.length; i++) {

        check += checkDataSource[i].value
    }

    if (check == 0) {

        $('#' + divChartId).html(emptyDataMsg);
        $('#' + divChartId).addClass("no_data");
        return false;
    }

    if (arrayOfColors != '' && arrayOfColors != null) {

        arrayOfColors = $.parseJSON(arrayOfColors);
    }
    depth = $.parseJSON(depth);

    //Use the dataSource array to create new array that will be used in chart function
    var chartData = DataSourceManipulation(chartType, dataSource, arrayOfColors);

    window.onload = new function () {

        //First link in the breadcrumb
        $('#' + divChartId + ' ul').append("<li><a id='a" + chartId + "'name='0' >" + title + "</a></li>");
        $(function () {
            $("#a" + chartId).click(function () {
                $.ajax({
                    type: "GET",
                    url: dataSourceServiceUrl,
                    success: function (response) {

                        $('#' + divChartId + ' canvas').each(function (index, value) {

                            if (this.id == chartId) {
                                $('#' + this.id).show();
                            }
                            else {
                                $('#' + this.id).remove();
                                $('#a' + this.id).parent().remove();
                            }
                        });
                    }
                });
            });
        });

        var chart;
        var ctx = document.getElementById(chartId).getContext("2d");
        var drillDownCtx = document.getElementById(chartId);

        //Initialize the main chart 
        switch (chartType) {

            case "doughnut":

                chart = new Chart(ctx).Doughnut(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>",
                });

                window.myDoughnut = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {

                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;

            case "pie":

                chart = new Chart(ctx).Pie(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>",
                });

                window.myPie = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {
                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;

            case "polararea":

                chart = new Chart(ctx).PolarArea(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>",
                });

                window.PolarArea = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {
                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;

            case "bar":

                chart = new Chart(ctx).Bar(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>"
                });

                window.myBar = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {
                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;

            case "line":

                chart = new Chart(ctx).Line(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>",
                });

                window.myLine = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {
                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;

            case "radar":

                chart = new Chart(ctx).Radar(chartData, {
                    tooltipFontFamily: fontFamily,
                    tooltipTitleFontFamily: fontFamily,
                    scaleFontFamily: fontFamily,
                    responsive: true,
                    tooltipTemplate: "<%=label%>: <%=value%>",
                });

                window.myRadar = chart;

                if (depth != 1) {
                    drillDownCtx.onclick = function (evt) {
                        var count = 1;
                        DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, chartId, arrayOfColors, fontFamily);
                    };
                }
                break;
        }
    }
}

function DataSourceManipulation(chartType, dataSource, arrayOfColors) {

    var colorsForDoughnutPiePolar = defaultColorsForDoughnutPiePolar;
    var colorsForBarLineRadar = defaultColorsForBarLineRadar;

    if (arrayOfColors != '' && arrayOfColors != null) {

        colorsForDoughnutPiePolar = arrayOfColors;
        colorsForBarLineRadar = arrayOfColors;
    }

    dataSource = $.parseJSON(dataSource);

    if (chartType == "doughnut" || chartType == "pie" || chartType == "polararea") {
        var chartData = [];

        var chartDataColor;
        var chartDataHighlight;
        var colorsForDoughnutPiePolarMax = colorsForDoughnutPiePolar.length - 1;
        var color = 0;

        for (var i = 0; i < dataSource.length; i++) {

            //if the colors in colorsForDoughnutPiePolar array lower than the values entered
            if (color > colorsForDoughnutPiePolarMax) {
                color = 0;
            }
            if (arrayOfColors != '' && arrayOfColors != null) {

                chartDataColor = colorsForDoughnutPiePolar[color];
                chartDataHighlight = colorsForDoughnutPiePolar[color];
            }
            else {

                chartDataColor = colorsForDoughnutPiePolar[color].color;
                chartDataHighlight = colorsForDoughnutPiePolar[color].highlight;
            }
            chartData.push({
                id: dataSource[i].id,
                value: dataSource[i].value,
                color: chartDataColor,
                highlight: chartDataHighlight,
                label: dataSource[i].label
            });
            color++;
        }

        return chartData;
    }
    else if (chartType == "bar" || chartType == "line" || chartType == "radar") {

        var ids = [];

        for (var i = 0; i < dataSource.length; i++) {
            ids.push(dataSource[i].id);
        }

        var labels = [];

        for (var i = 0; i < dataSource.length; i++) {
            labels.push(dataSource[i].label);
        }

        var maxLength = 0

        for (var i = 0; i < dataSource.length; i++) {
            if (dataSource[i].value.length > maxLength) {
                maxLength = dataSource[i].value.length;
            }
        }

        var datasets = [];

        var colorsForBarLineRadarMax = colorsForBarLineRadar.length - 1;
        var color = 0;
        for (var i = 0; i < maxLength; i++) {

            //If the colors in colorsForBarLineRadar array lower than the values entered
            if (color > colorsForBarLineRadarMax) {
                color = 0;
            }

            var data = [];
            for (var y = 0; y < dataSource.length; y++) {

                //If the number of values in one of dataSourse items is lower than the max length 
                if (dataSource[y].value[i] == null) {
                    dataSource[y].value[i].value = 0;
                }
                data.push(dataSource[y].value[i].value);

            }

            if (arrayOfColors != '' && arrayOfColors != null) {

                lightColor = colorsForBarLineRadar[color];
                darkColor = colorsForBarLineRadar[color];
            }
            else {

                lightColor = colorsForBarLineRadar[color].lightColor;
                darkColor = colorsForBarLineRadar[color].darkColor;
            }

            if (chartType == "bar") {
                datasets.push({
                    label: dataSource[0].value[i].label,
                    fillColor: darkColor,
                    highlightFill: lightColor,
                    data: data
                });
            }
            else {
                datasets.push({
                    label: dataSource[0].value[i].label,
                    fillColor: "rgba(220,220,220,0)",
                    strokeWidth: 100,
                    strokeColor: darkColor,
                    pointColor: darkColor,
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: darkColor,
                    data: data
                });
            }
            color++;
        }

        var chartData = {
            ids: ids,
            labels: labels,
            datasets: datasets
        }
        return chartData;
    }
}

function DrillDown(chart, chartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily) {

    var chartIdHide = chartId;
    var drillDownchartId;
    var lastChar = chartId.substr(chartId.length - 1);
    var lastId = chartId;

    //Create an id for the new canvas
    if (chartId == mainChart) {

        //If the function call for the first time
        drillDownchartId = chartId + "1";
        lastChar = 1;
    }
    else if (!isNaN(lastChar)) {

        lastChar = parseInt(lastChar) + 1;
        chartId = chartId.substring(0, chartId.length - 1);
        drillDownchartId = chartId + lastChar.toString();
    }

    switch (chartType) {
        case "doughnut":

            var activePoints = chart.getSegmentsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].id, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {

                        if (data != null && data != "") {

                            if (count != depth) {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).Doughnut(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).Doughnut(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }
            break;

        case "pie":

            var activePoints = chart.getSegmentsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].id, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {

                        if (data != null && data != "") {

                            if (count != depth) {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).Pie(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).Pie(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }

            break;

        case "polararea":

            var activePoints = chart.getSegmentsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].id, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {
                        if (data != null && data != "") {

                            if (count != depth) {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).PolarArea(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).PolarArea(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }

            break;

        case "bar":

            var activePoints = chart.getBarsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].dataId, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {

                        if (data != null && data != "") {

                            if (count != depth) {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).Bar(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).Bar(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }

            break;

        case "line":

            var activePoints = chart.getPointsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].dataId, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {

                        if (data != null && data != "") {

                            if (count != depth) {

                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).Line(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).Line(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }
            break;

        case "radar":

            var activePoints = chart.getPointsAtEvent(evt);
            if (activePoints.length != 0) {

                $('#' + chartIdHide).hide();

                //Create new canvas and new link in the breadcrumb 
                AppendCanvas(drillDownchartId, divChartId, activePoints[0].label, activePoints[0].dataId, lastChar, mainChart);

                //Create a string has the ids of the segments that the user clicked
                var drillDownIds = DrillDownIds(divChartId);
                var count = drillDownIds[0];
                var itemId = drillDownIds[1];

                var drillDownItems;

                //Bring the new data from dataSourceServiceUrl
                $.ajax({
                    url: dataSourceServiceUrl,
                    type: 'post',
                    dataType: 'text',
                    error: function (response) {
                        alert("error");
                    },
                    success: function (data) {

                        if (data != null && data != "") {

                            if (count != depth) {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                var drillDownCtxNew = document.getElementById(drillDownchartId);

                                var drillDownChart = new Chart(drillDownCtxNew.getContext("2d")).Radar(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });

                                window.drillDown = drillDownChart;

                                drillDownCtxNew.onclick = function (evt) {
                                    DrillDown(drillDownChart, drillDownchartId, divChartId, chartType, evt, dataSourceServiceUrl, depth, mainChart, arrayOfColors, fontFamily);
                                };
                            }
                            else {
                                drillDownItems = data;
                                var drillDownChartData = DataSourceManipulation(chartType, drillDownItems, arrayOfColors);

                                window.drillDown = new Chart(document.getElementById(drillDownchartId).getContext("2d")).Radar(drillDownChartData, {
                                    tooltipFontFamily: fontFamily,
                                    tooltipTitleFontFamily: fontFamily,
                                    scaleFontFamily: fontFamily,
                                    responsive: true,
                                });
                            }
                        }
                    },
                    data: "id=" + itemId
                });
            }
            break;
    }
}

function AppendCanvas(drillDownchartId, divChartId, activePointLabel, activePointsId, lastChar, mainChart) {

    $('#' + divChartId + ' ul').append("<li><a id='a" + drillDownchartId + "'name='" + activePointsId.toString() + "' >" + activePointLabel + "</a></li>");
    $('#' + divChartId).append("<canvas id='" + drillDownchartId + "' width='400' height='200' />");

    //On the link click remove all canvas elements that after the selected canvas, hide the canvas elements that before it, and show the selected one
    $(function () {
        $('#' + divChartId + " #a" + drillDownchartId).click(function () {
            $.ajax({
                type: "GET",
                error: function (response) {
                    alert("error");
                },
                success: function (response) {
                    $('#' + divChartId + ' canvas').each(function (index, value) {

                        if (this.id != mainChart) {
                            var IdLastChar = (this.id).substr((this.id).length - 1);
                            if (!isNaN(lastChar)) {
                                if (IdLastChar > lastChar) {
                                    $('#' + this.id).remove();
                                    $('#a' + this.id).parent().remove();
                                }
                            }
                            else {
                                if (IdLastChar > 1) {
                                    $('#' + this.id).remove();
                                    $('#a' + this.id).parent().remove();
                                }
                            }
                        }
                    });
                    $('#' + divChartId + ' canvas').hide();
                    $('#' + drillDownchartId).show();
                }
            });
        });
    });
}

function DrillDownIds(divChartId) {

    var count = 0;
    var itemId = null;

    //bring the selected values from the name attribute of <a> tags
    $('#' + divChartId + ' ul li a').each(function (index, value) {
        count++;
        if (itemId != null) {
            itemId = itemId + "," + this.name;
        }
        else {
            itemId = this.name;
        }
    });

    return [count, itemId];
}
