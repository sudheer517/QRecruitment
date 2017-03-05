import { Component, AfterViewInit, ViewEncapsulation, ElementRef, Renderer } from '@angular/core';

@Component({
    selector: "[appc-tests]",
    templateUrl: "./tests.component.html",
    styleUrls: ["./tests.component.scss"],
})
export class TestsComponent implements AfterViewInit{
    locationChartOptions: Object;
    branchChartOptions: Object;
    teamChartOptions: Object;
    constructor(private el: ElementRef, private renderer: Renderer) {
    }

    ngOnInit(){
       this.changeBackground();
    }

    changeBackground(){
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("questions-tests-background");
    }

    ngAfterViewInit(){
        this.setHighchartsColors();
    }

    setHighchartsColors(){
        const Highcharts = require('highcharts');

        Highcharts.setOptions({
        colors: ['#6D7991', '#DD6600', '#61AEE1', '#EE2525', '#E8C54F', '#009E4A' ]
        });
    }
    setHighchartsOptionsForLocationChart(){
        this.locationChartOptions = {
            chart: { type: 'pie', backgroundColor: '#F5F4F4' },
            title: { text : ''},
            plotOptions: {
            pie: {
                dataLabels: {
                    enabled: false,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true
                // startAngle: -90,
                // endAngle: 90,
                }
            },
            series: [
                { 
                    innerSize: '50%',
                    data: [
                        ['Firefox',   10.38],
                        ['IE',       56.33],
                        ['Chrome', 24.03],
                        ['Safari',    4.77],
                        ['Opera',     0.91],
                        {
                            name: 'Proprietary or Undetectable',
                            y: 0.2,
                            dataLabels: {
                                enabled: false
                            }
                        }
                    ] 
                }
            ]
        };
    }

    setHighchartsOptionsForBranchChart(){
        this.branchChartOptions = {
            chart: { type: 'pie', backgroundColor: '#F5F4F4' },
            title: { text : ''},
            plotOptions: {
            pie: {
                dataLabels: {
                    enabled: false,
                    distance: -50,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true,
                startAngle: -90,
                endAngle: 90,
                center: ['50%', '75%']
                }
            },
            series: [
                { 
                    innerSize: '50%',
                    data: [
                        ['Firefox',   10.38],
                        ['IE',       56.33],
                        ['Chrome', 24.03],
                        ['Safari',    4.77],
                        ['Opera',     0.91],
                        {
                            name: 'Proprietary or Undetectable',
                            y: 0.2,
                            dataLabels: {
                                enabled: false
                            }
                        }
                    ] 
                }
            ]
        };
    }

    setHighchartsOptionsForTeamChart(){
        this.teamChartOptions = {
            chart: { type: 'column', backgroundColor: '#F5F4F4' },
            title: { text : ''},
            // subtitle: {
            //     text: 'Click the columns to view drilldown by month'
            // },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Total number of tests'
                }
            },
            legend: {
                enabled: true
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.1f}%'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            },
            series: [{
                name: 'Brands',
                colorByPoint: true,
                data: [{
                    name: 'Microsoft Internet Explorer',
                    y: 56.33,
                    drilldown: 'Microsoft Internet Explorer'
                }, {
                    name: 'Chrome',
                    y: 24.03,
                    drilldown: 'Chrome'
                }, {
                    name: 'Firefox',
                    y: 10.38,
                    drilldown: 'Firefox'
                }, {
                    name: 'Safari',
                    y: 4.77,
                    drilldown: 'Safari'
                }, {
                    name: 'Opera',
                    y: 0.91,
                    drilldown: 'Opera'
                }, {
                    name: 'Proprietary or Undetectable',
                    y: 0.2,
                    drilldown: null
                }]
            }],
            drilldown: {
                series: [{
                    name: 'Microsoft Internet Explorer',
                    id: 'Microsoft Internet Explorer',
                    data: [
                        [
                            'v11.0',
                            24.13
                        ],
                        [
                            'v8.0',
                            17.2
                        ],
                        [
                            'v9.0',
                            8.11
                        ],
                        [
                            'v10.0',
                            5.33
                        ],
                        [
                            'v6.0',
                            1.06
                        ],
                        [
                            'v7.0',
                            0.5
                        ]
                    ]
                }, {
                    name: 'Chrome',
                    id: 'Chrome',
                    data: [
                        [
                            'v40.0',
                            5
                        ],
                        [
                            'v41.0',
                            4.32
                        ],
                        [
                            'v42.0',
                            3.68
                        ],
                        [
                            'v39.0',
                            2.96
                        ],
                        [
                            'v36.0',
                            2.53
                        ],
                        [
                            'v43.0',
                            1.45
                        ],
                        [
                            'v31.0',
                            1.24
                        ],
                        [
                            'v35.0',
                            0.85
                        ],
                        [
                            'v38.0',
                            0.6
                        ],
                        [
                            'v32.0',
                            0.55
                        ],
                        [
                            'v37.0',
                            0.38
                        ],
                        [
                            'v33.0',
                            0.19
                        ],
                        [
                            'v34.0',
                            0.14
                        ],
                        [
                            'v30.0',
                            0.14
                        ]
                    ]
                }, {
                    name: 'Firefox',
                    id: 'Firefox',
                    data: [
                        [
                            'v35',
                            2.76
                        ],
                        [
                            'v36',
                            2.32
                        ],
                        [
                            'v37',
                            2.31
                        ],
                        [
                            'v34',
                            1.27
                        ],
                        [
                            'v38',
                            1.02
                        ],
                        [
                            'v31',
                            0.33
                        ],
                        [
                            'v33',
                            0.22
                        ],
                        [
                            'v32',
                            0.15
                        ]
                    ]
                }, {
                    name: 'Safari',
                    id: 'Safari',
                    data: [
                        [
                            'v8.0',
                            2.56
                        ],
                        [
                            'v7.1',
                            0.77
                        ],
                        [
                            'v5.1',
                            0.42
                        ],
                        [
                            'v5.0',
                            0.3
                        ],
                        [
                            'v6.1',
                            0.29
                        ],
                        [
                            'v7.0',
                            0.26
                        ],
                        [
                            'v6.2',
                            0.17
                        ]
                    ]
                }, {
                    name: 'Opera',
                    id: 'Opera',
                    data: [
                        [
                            'v12.x',
                            0.34
                        ],
                        [
                            'v28',
                            0.24
                        ],
                        [
                            'v27',
                            0.17
                        ],
                        [
                            'v29',
                            0.16
                        ]
                    ]
                }]
            }
        }
    }
}