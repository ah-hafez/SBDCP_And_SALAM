$(function(){        
    /* reportrange */
    if($("#reportrange").length > 0){   
        $("#reportrange").daterangepicker({                    
            ranges: {
               'Today': [moment(), moment()],
               'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
               'Last 7 Days': [moment().subtract(6, 'days'), moment()],
               'Last 30 Days': [moment().subtract(29, 'days'), moment()],
               'This Month': [moment().startOf('month'), moment().endOf('month')],
               'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            opens: 'left',
            buttonClasses: ['btn btn-default'],
            applyClass: 'btn-small btn-primary',
            cancelClass: 'btn-small',
            format: 'MM.DD.YYYY',
            separator: ' to ',
            startDate: moment().subtract('days', 29),
            endDate: moment()            
          },function(start, end) {
              $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        });
        
        $("#reportrange span").html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
    }
    /* end reportrange */
    

    
    /* Donut dashboard chart */
    Morris.Donut({
        element: 'dashboard-donut-1',
        data: [
            {label: "Returned", value: 2513},
            {label: "New", value: 764},
            {label: "Registred", value: 311}
        ],
        colors: ['#2DA5DA', '#3467A5', '#6E2690'],
        resize: true
    });
    /* END Donut dashboard chart */
    
    /* Bar dashboard chart */
    Morris.Bar({
        element: 'dashboard-bar-1',
        data: [
            { y: 'Oct 10', a: 75, b: 35, c: 25 },
            { y: 'Oct 11', a: 64, b: 26, c: 35 },
            { y: 'Oct 12', a: 78, b: 39, c: 45 },
            { y: 'Oct 13', a: 82, b: 34, c: 55 },
            { y: 'Oct 14', a: 86, b: 39, c: 65 },
            { y: 'Oct 15', a: 94, b: 40, c: 75 },
            { y: 'Oct 16', a: 96, b: 41, c: 85 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b', 'c'],
        labels: ['New Users', 'Returned', 'Returned'],
        barColors: ['#2DA5DA', '#3467A5', '#D2477D'],
        gridTextSize: '10px',
        hideHover: true,
        resize: true,
        gridLineColor: '#E5E5E5'
    });
    /* END Bar dashboard chart */
    
    /* Line dashboard chart */
    Morris.Line({
      element: 'dashboard-line-1',
      data: [
        { y: '2014-10-10', a: 2,b: 4},
        { y: '2014-10-11', a: 4,b: 6},
        { y: '2014-10-12', a: 7,b: 10},
        { y: '2014-10-13', a: 5,b: 7},
        { y: '2014-10-14', a: 6,b: 9},
        { y: '2014-10-15', a: 9,b: 12},
        { y: '2014-10-16', a: 18,b: 20}
      ],
      xkey: 'y',
      ykeys: ['a','b'],
      labels: ['Sales','Event'],
      resize: true,
      hideHover: true,
      xLabels: 'day',
      gridTextSize: '10px',
      lineColors: ['#2DA5DA', '#6E2690'],
      gridLineColor: '#E5E5E5'
    });   
    /* EMD Line dashboard chart */
    
    /* Vector Map */
    var jvm_wm = new jvm.WorldMap({container: $('#dashboard-map-seles'),
                                    map: 'world_mill_en', 
                                    backgroundColor: '#EFF0F1',                                      
                                    regionsSelectable: true,
                                    regionStyle: {selected: {fill: '#B64645'},
                                                    initial: {fill: '#5435AE'}},
                                    markerStyle: {initial: {fill: '#2E8BEF',
                                                   stroke: '#2E8BEF'}},
                                    markers: [{latLng: [50.27, 30.31], name: 'Kyiv - 1'},                                              
                                              {latLng: [52.52, 13.40], name: 'Berlin - 2'},
                                              {latLng: [48.85, 2.35], name: 'Paris - 1'},                                            
                                              {latLng: [51.51, -0.13], name: 'London - 3'},                                                                                                      
                                              {latLng: [40.71, -74.00], name: 'New York - 5'},
                                              {latLng: [35.38, 139.69], name: 'Tokyo - 12'},
                                              {latLng: [37.78, -122.41], name: 'San Francisco - 8'},
                                              {latLng: [28.61, 77.20], name: 'New Delhi - 4'},
                                              {latLng: [39.91, 116.39], name: 'Beijing - 3'}]
                                });    
    /* END Vector Map */

    
    /*$(".x-navigation-minimize").on("click",function(){
        setTimeout(function(){
            rdc_resize();
        },200);    
    });
    */
    
});

