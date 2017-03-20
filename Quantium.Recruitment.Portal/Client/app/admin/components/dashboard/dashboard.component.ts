import { Component, AfterViewInit, ViewEncapsulation, Renderer, OnInit } from '@angular/core';

@Component({
    selector: "[appc-dashboard]",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.scss"],
    // encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements AfterViewInit, OnInit{
    locationChartOptions: Object;
    branchChartOptions: Object;
    teamChartOptions: Object;
    constructor(private renderer: Renderer) {
        
    }
    ngOnInit(){
        this.changeBackground();
    }

    changeBackground(){
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("dashboard-background");
    }
    ngAfterViewInit(){
        
        this.setHighchartsColors();
        
        this.setHighchartsOptionsForLocationChart();

        this.setHighchartsOptionsForBranchChart();

        this.setHighchartsOptionsForTeamChart();
    }

    setHighchartsColors(){
        const Highcharts = require('highcharts');

        Highcharts.setOptions({
        colors: ['#6D7991', '#DD6600', '#61AEE1', '#EE2525', '#E8C54F', '#009E4A' ]
        });
    }
    setHighchartsOptionsForBranchChart(){
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
                        ['Mechanical',   10.38],
                        ['Chemical',       56.33],
                        ['Metallurgy', 24.03],
                        ['Other',    4.77],
                        ['Computer science',     0.91],
                        {
                            name: 'MBA',
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

    setHighchartsOptionsForLocationChart(){
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
                        ['Hyderabad',   10.38],
                        ['Other',       56.33],
                        ['Bangalore', 24.03],
                        ['Mumbai',    4.77],
                        ['Delhi',     0.91],
                        {
                            name: 'Chennai',
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
                name: 'Teams',
                colorByPoint: true,
                data: [{
                    name: 'Retail',
                    y: 56.33,
                    drilldown: 'Retail'
                }, {
                    name: 'Media',
                    y: 24.03,
                    drilldown: 'Media'
                }, {
                    name: 'Web automation',
                    y: 10.38,
                    drilldown: 'Web automation'
                }, {
                    name: 'Checkout',
                    y: 4.77,
                    drilldown: 'Checkout'
                }, {
                    name: 'Wow',
                    y: 0.91,
                    drilldown: 'Wow'
                }, {
                    name: 'Other',
                    y: 0.2,
                    drilldown: null
                }]
            }],
            drilldown: {
                series: [{
                    name: 'Retail',
                    id: 'Retail',
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
                    name: 'Media',
                    id: 'Media',
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
                    name: 'Web automation',
                    id: 'Web automation',
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
                    name: 'Checkout',
                    id: 'Checkout',
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
                    name: 'Wow',
                    id: 'Wow',
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