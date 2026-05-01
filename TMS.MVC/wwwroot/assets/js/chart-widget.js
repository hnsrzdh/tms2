(function ($) {
  "use strict";
  /*Line chart*/
  var optionslinechart = {
    chart: {
      toolbar: {
        show: false,
      },
      height: 200,
      type: "area",
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: 3,
      curve: "smooth",
    },
    xaxis: {
      show: false,
      type: "datetime",
      categories: [
        "1394-09-19T00:00:00",
        "1394-09-19T01:30:00",
        "1394-09-19T02:30:00",
        "1394-09-19T03:30:00",
        "1394-09-19T04:30:00",
        "1394-09-19T05:30:00",
        "1394-09-19T06:30:00",
        "1394-09-19T07:30:00",
        "1394-09-19T08:30:00",
        "1394-09-19T09:30:00",
        "1394-09-19T10:30:00",
      ],
      labels: {
        show: false,
      },
      axisBorder: {
        show: false,
      },
    },
    yaxis: {
      labels: {
        show: false,
      },
    },
    grid: {
      show: false,
      padding: {
        left: -10,
        top: -25,
        right: -60,
        bottom: -40,
      },
    },
    fill: {
      opacity: 0.2,
    },
    colors: [AdmiroAdminConfig.primary],
    series: [
      {
                  name: "مقدار",

        data: [70, 60, 82, 80, 60, 90, 70, 120, 50, 60, 0],
      },
    ],
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
    responsive: [
      {
        breakpoint: 576,
        options: {
          chart: {
            height: 100,
          }
        }
      }
    ]
  };

  var chartlinechart = new ApexCharts(
    document.querySelector("#chart-widget1"),
    optionslinechart
  );

  chartlinechart.render();

  /*Line chart2*/
  var optionslinechart2 = {
    chart: {
      toolbar: {
        show: false,
      },
      height: 200,
      type: "area",
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: 3,
      curve: "smooth",
    },
    xaxis: {
      show: false,
      type: "datetime",
      categories: [
        "1394-09-19T00:00:00",
        "1394-09-19T01:30:00",
        "1394-09-19T02:30:00",
        "1394-09-19T03:30:00",
        "1394-09-19T04:30:00",
        "1394-09-19T05:30:00",
        "1394-09-19T06:30:00",
        "1394-09-19T07:30:00",
        "1394-09-19T08:30:00",
        "1394-09-19T09:30:00",
        "1394-09-19T10:30:00",
      ],
      labels: {
        show: false,
      },
      axisBorder: {
        show: false,
      },
    },
    yaxis: {
      show: false
    },
    grid: {
      show: false,
      padding: {
        left: -10,
        top: -25,
        right: -60,
        bottom: -40,
      },
    },
    fill: {
      opacity: 0.2,
    },
    colors: [AdmiroAdminConfig.secondary],
    series: [
      {
        name: "مقدار",
        data: [70, 60, 82, 80, 60, 90, 70, 120, 50, 60, 0],
      },
    ],
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
    responsive: [
      {
        breakpoint: 576,
        options: {
          chart: {
            height: 100,
          }
        }
      }
    ]
  };

  var chartlinechart2 = new ApexCharts(
    document.querySelector("#chart-widget2"),
    optionslinechart2
  );
  chartlinechart2.render();

  /*Line chart3*/
  var optionslinechart3 = {
    chart: {
      toolbar: {
        show: false,
      },
      height: 200,
      type: "area",
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: 3,
      curve: "smooth",
    },
    xaxis: {
      show: false,
      type: "datetime",
      categories: [
        "1394-09-19T00:00:00",
        "1394-09-19T01:30:00",
        "1394-09-19T02:30:00",
        "1394-09-19T03:30:00",
        "1394-09-19T04:30:00",
        "1394-09-19T05:30:00",
        "1394-09-19T06:30:00",
        "1394-09-19T07:30:00",
        "1394-09-19T08:30:00",
        "1394-09-19T09:30:00",
        "1394-09-19T10:30:00",
      ],
      labels: {
        show: false,
      },
      axisBorder: {
        show: false,
      },
    },
    yaxis: {
      show: false
    },
    grid: {
      show: false,
      padding: {
        left: -10,
        top: -25,
        right: -60,
        bottom: -40,
      },
    },
    fill: {
      opacity: 0.2,
    },
    colors: ["#3eb95f"],
    series: [
      {
                  name: "مقدار",

        data: [70, 60, 82, 80, 60, 90, 70, 120, 50, 60, 0],
      },
    ],
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
    responsive: [
      {
        breakpoint: 576,
        options: {
          chart: {
            height: 100,
          }
        }
      }
    ]
  };

  var chartlinechart3 = new ApexCharts(
    document.querySelector("#chart-widget3"),
    optionslinechart3
  );
  chartlinechart3.render();

  // column chart
  var optionscolumnchart = {
    series: [
      {
        name: "سود",
        data: [100, 50, 25, 50, 30, 50, 70],
      },
      {
        name: "درامد",
        data: [70, 20, 55, 45, 35, 110, 85],
      },
      {
        name: "بازگشتی",
        data: [85, 55, 100, 35, 90, 60, 80],
      },
    ],
    chart: {
      type: "bar",
      height: 380,
      toolbar: {
        show: false,
      },
    },
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: "30%",
        endingShape: "rounded",
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      show: true,
      width: 1,
      colors: ["transparent"],
      curve: "smooth",
      lineCap: "butt",
    },
    xaxis: {
      categories: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر'],
      floating: false,
      axisTicks: {
        show: false,
      },
      axisBorder: {
        color: "#C4C4C4",
      },
    },
    yaxis: {
      title: {
        text: "دلار",
        style: {
          fontSize: "14px",
          fontFamily: "Roboto, sans-serif",
          fontWeight: 500,
        },
      },
    },
    colors: [AdmiroAdminConfig.secondary, "#3eb95f", AdmiroAdminConfig.primary],
    fill: {
      type: "gradient",
      gradient: {
        shade: "light",
        type: "vertical",
        shadeIntensity: 0.1,
        inverseColors: false,
        opacityFrom: 1,
        opacityTo: 0.9,
        stops: [0, 100],
      },
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return val;
        },
      },
    },
    responsive: [
      {
        breakpoint: 576,
        options: {
          chart: {
            height: 200,
          }
        }
      }
    ]
  };
  var chartcolumnchart = new ApexCharts(
    document.querySelector("#chart-widget4"),
    optionscolumnchart
  );
  chartcolumnchart.render();

  // product chart
  var optionsproductchart = {
    chart: {
      height: 320,
      type: "area",
      toolbar: {
        show: false,
      },
    },
    stroke: {
      curve: "smooth",
      width: 0,
    },
    series: [
      {
        name: "تیم 1",
        data: [50, 120, 90, 100, 70, 95, 40, 55, 0],
      },
      {
        name: "تیم 2",
        data: [35, 60, 40, 90, 70, 110, 90, 120, 0],
      },
    ],
    fill: {
      colors: [AdmiroAdminConfig.primary, AdmiroAdminConfig.secondary],
      type: "gradient",
      gradient: {
        shade: "light",
        type: "vertical",
        shadeIntensity: 0.4,
        inverseColors: false,
        opacityFrom: 0.9,
        opacityTo: 0.8,
        stops: [0, 100],
      },
    },
    dataLabels: {
      enabled: false,
    },
    grid: {
      borderColor: "rgba(196,196,196, 0.3)",
      padding: {
        top: 0,
        right: -120,
        bottom: 10,
      },
    },
    colors: [AdmiroAdminConfig.primary, AdmiroAdminConfig.secondary],
    labels: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر'],
    markers: {
      size: 0,
    },
    xaxis: {
      axisTicks: {
        show: false,
      },
      axisBorder: {
        color: "rgba(196,196,196, 0.3)",
      },
    },
    yaxis: [
      {
        title: {
          text: "نمودار",
        },
      },
    ],
    tooltip: {
      shared: true,
      intersect: false,
      y: {
        formatter: function (y) {
          if (typeof y !== "undefined") {
            return y.toFixed(0) ;
          }
          return y;
        },
      },
    },
  };

  var chartproductchart = new ApexCharts(
    document.querySelector("#chart-widget6"),
    optionsproductchart
  );
  chartproductchart.render();

  // Turnover chart
  var optionsturnoverchart = {
    chart: {
      toolbar: {
        show: false,
      },
      height: 300,
      type: "area",
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: 3,
      curve: "smooth",
    },
    xaxis: {
      categories: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی'],
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
    },
    grid: {
      borderColor: "rgba(196,196,196, 0.3)",
      padding: {
        top: -20,
        right: -55,
        bottom: 0,
      },
    },
    fill: {
      opacity: 0.2,
    },
    colors: [AdmiroAdminConfig.primary],
    series: [
      {
        data: [70, 60, 82, 80, 60, 90, 70, 120, 50, 0],
      },
    ],
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
  };

  var chartturnoverchart = new ApexCharts(
    document.querySelector("#chart-widget7"),
    optionsturnoverchart
  );
  chartturnoverchart.render();

  // Crypto price chart
  var optionscryptopricechart = {
    chart: {
      toolbar: {
        show: false,
      },
      height: 400,
      type: "area",
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: 3,
      curve: "smooth",
    },
    xaxis: {
      categories: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
      'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
      'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
      'فروردین'],
      tickAmount: 5,
      tickPlacement: "between",
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
      tooltip: {
        enabled: false,
      },
    },
    grid: {
      borderColor: "rgba(196,196,196, 0.3)",
      padding: {
        top: -20,
        right: -16,
        bottom: 0,
      },
    },
    fill: {
      opacity: 0.2,
    },
    colors: [AdmiroAdminConfig.primary],
    series: [
      {
        data: [
          20, 120, 15, 100, 120, 60, 150, 70, 100, 80, 105, 20, 70, 60, 10, 12,
          10, 130, 60, 80, 40, 140, 110, 150, 30, 75, 20, 45, 15, 130, 10, 30,
          15, 110, 65, 130, 0,
        ],
      },
    ],
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
    responsive: [
      {
        breakpoint: 576,
        options: {
          chart: {
            height: 200,
          }
        }
      }
    ]
  };
  var chartcryptopricechart = new ApexCharts(
    document.querySelector("#chart-crypto"),
    optionscryptopricechart
  );
  chartcryptopricechart.render();

  // Crypto annotation chart

  var series = {
    monthDataSeries1: {
      prices: [8107, 8300, 8260, 8400, 8350, 8500, 8350],
      dates: [
        "13 تیر 2017",
        "14 تیر 2017",
        "15 تیر 2017",
        "16 تیر 2017",
        "17 تیر 2017",
        "20 تیر 2017",
        "21 تیر 2017",
      ],
    },
  };


  // user chart
  function generateData(baseval, count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = Math.floor(Math.random() * (750 - 1 + 1)) + 1;
      var y =
        Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;
      var z = Math.floor(Math.random() * (75 - 15 + 1)) + 15;

      series.push([x, y, z]);
      baseval += 86400000;
      i++;
    }
    return series;
  }
 


  var trigoStrength = 3;
  var iteration = 11;

  function getRandom() {
    var i = iteration;
    return (
      (Math.sin(i / trigoStrength) * (i / trigoStrength) +
        i / trigoStrength +
        1) *
      (trigoStrength * 2)
    );
  }

  function getRangeRandom(yrange) {
    return (
      Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min
    );
  }

  function generateMinuteWiseTimeSeries(baseval, count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = baseval;
      var y =
        (Math.sin(i / trigoStrength) * (i / trigoStrength) +
          i / trigoStrength +
          1) *
        (trigoStrength * 2);

      series.push([x, y]);
      baseval += 300000;
      i++;
    }
    return series;
  }

  function getNewData(baseval, yrange) {
    var newTime = baseval + 300000;
    return {
      x: newTime,
      y: Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min,
    };
  }



  var optionsCircle = {
    chart: {
      type: "radialBar",
      height: 355,
      offsetY: 10,
      offsetX: 20,
    },
    plotOptions: {
      radialBar: {
        size: undefined,
        inverseOrder: false,
        hollow: {
          margin: 10,
          size: "30%",
          background: "transparent",
        },
        track: {
          show: true,
          background: "#f2f2f2",
          strokeWidth: "10%",
          opacity: 1,
          margin: 3,
        },
      },
    },
    series: [90, 63, 50],
    labels: ["مهارت 01", "مهارت 02", "مهارت 03"],
    legend: {
      show: true,
      fontSize: "16px",
      fontFamily: "Roboto, sans-serif",
      fontWeight: 500,
      labels: {
        colors: "#2C323F",
      },
      markers: {
        width: 86,
        height: 18,
        radius: 3,
      },
    },
    colors: [AdmiroAdminConfig.secondary, AdmiroAdminConfig.primary, "#3eb95f"],
    responsive: [
      {
        breakpoint: 767,
        options: {
          title: {
            style: {
              fontSize: "16px",
            },
          },
        },
      },
      {
        breakpoint: 420,
        options: {
          chart: {
            offsetY: 0,
            offsetX: 0,
          },
          legend: {
            position: 'bottom'
          },
        },
      },
    ],
  };

  var chartCircle = new ApexCharts(
    document.querySelector("#circlechart"),
    optionsCircle
  );
  chartCircle.render();

  var optionsProgress1 = {
    chart: {
      height: 70,
      type: "bar",
      stacked: true,
      sparkline: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "15%",
        colors: {
          backgroundBarColors: [AdmiroAdminConfig.primary],
          backgroundBarOpacity: 0.2,
        },
      },
    },
    colors: [AdmiroAdminConfig.primary],
    stroke: {
      width: 0,
    },
    fill: {
      colors: [AdmiroAdminConfig.primary],
      type: "gradient",
      gradient: {
        gradientToColors: [AdmiroAdminConfig.primary],
      },
    },
    series: [
      {
        name: "ارسال شده",
        data: [44],
      },
    ],
    title: {
      floating: true,
      offsetX: -10,
      offsetY: 5,
      text: "ارسال شده",
      style: {
        fontSize: "18px",
        fontFamily: "Roboto, sans-serif",
        fontWeight: 500,
      },
    },
    subtitle: {
      floating: true,
      align: "right",
      offsetY: 0,
      text: "44%",
      style: {
        fontSize: "14px",
      },
    },
    tooltip: {
      enabled: false,
    },
    xaxis: {
      categories: ["ارسال شده"],
    },
    yaxis: {
      max: 100,
    },
    fill: {
      opacity: 1,
    },
    responsive: [{
      breakpoint: 767,
      options: {
        title: {
          style: {
            fontSize: "16px",
          },
        },
      },
    }]
  };

  var chartProgress1 = new ApexCharts(
    document.querySelector("#progress1"),
    optionsProgress1
  );
  chartProgress1.render();

  var optionsProgress2 = {
    chart: {
      height: 70,
      type: "bar",
      stacked: true,
      sparkline: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "15%",
        colors: {
          backgroundBarColors: [AdmiroAdminConfig.secondary],
          backgroundBarOpacity: 0.2,
          backgroundBarRadius: 10,
        },
      },
    },
    colors: [AdmiroAdminConfig.secondary],
    stroke: {
      width: 0,
    },
    series: [
      {
        name: "بسته بندی",
        data: [40],
      },
    ],
    title: {
      floating: true,
      offsetX: -10,
      offsetY: 5,
      text: "بسته بندی",
      style: {
        fontSize: "18px",
        fontFamily: "Roboto, sans-serif",
        fontWeight: 500,
      },
    },
    subtitle: {
      floating: true,
      align: "right",
      offsetY: 0,
      text: "44%",
      style: {
        fontSize: "14px",
      },
    },
    tooltip: {
      enabled: false,
    },
    xaxis: {
      categories: ["Process 2"],
    },
    yaxis: {
      max: 100,
    },
    fill: {
      colors: [AdmiroAdminConfig.secondary],
      type: "gradient",
      gradient: {
        inverseColors: false,
        gradientToColors: [AdmiroAdminConfig.secondary],
      },
    },
    responsive: [{
      breakpoint: 767,
      options: {
        title: {
          style: {
            fontSize: "16px",
          },
        },
      },
    }]
  };

  var chartProgress2 = new ApexCharts(
    document.querySelector("#progress2"),
    optionsProgress2
  );
  chartProgress2.render();

  var optionsProgress3 = {
    chart: {
      height: 70,
      type: "bar",
      stacked: true,
      sparkline: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "15%",
        colors: {
          backgroundBarColors: ["#a927f9"],
          backgroundBarOpacity: 0.2,
          backgroundBarRadius: 10,
        },
      },
    },
    colors: ["#a927f9"],
    stroke: {
      width: 0,
    },
    series: [
      {
        name: "در حال پردازش",
        data: [50],
      },
    ],
    fill: {
      colors: ["#a927f9"],
      type: "gradient",
      gradient: {
        gradientToColors: ["#a927f9"],
      },
    },
    title: {
      floating: true,
      offsetX: -10,
      offsetY: 5,
      text: "در حال پردازش",
      style: {
        fontSize: "18px",
        fontFamily: "Roboto, sans-serif",
        fontWeight: 500,
      },
    },
    subtitle: {
      floating: true,
      align: "right",
      offsetY: 0,
      text: "50%",
      style: {
        fontSize: "14px",
      },
    },
    tooltip: {
      enabled: false,
    },
    xaxis: {
      categories: ["در حال پردازش"],
    },
    yaxis: {
      max: 100,
    },
    responsive: [{
      breakpoint: 767,
      options: {
        title: {
          style: {
            fontSize: "16px",
          },
        },
      },
    }]
  };

  var chartProgress3 = new ApexCharts(
    document.querySelector("#progress3"),
    optionsProgress3
  );
  chartProgress3.render();

  var optionsProgress4 = {
    chart: {
      height: 70,
      type: "bar",
      stacked: true,
      sparkline: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "15%",
        colors: {
          backgroundBarColors: ["#F8D62B"],
          backgroundBarOpacity: 0.2,
          backgroundBarRadius: 10,
        },
      },
    },
    colors: ["#F8D62B"],
    stroke: {
      width: 0,
    },
    series: [
      {
        name: "تحویل شده",
        data: [60],
      },
    ],
    fill: {
      colors: ["#F8D62B"],
      type: "gradient",
      gradient: {
        gradientToColors: ["#F8D62B"],
      },
    },
    title: {
      floating: true,
      offsetX: -10,
      offsetY: 5,
      text: "تحویل شده",
      style: {
        fontSize: "18px",
        fontFamily: "Roboto, sans-serif",
        fontWeight: 500,
      },
    },
    subtitle: {
      floating: true,
      align: "right",
      offsetY: 0,
      text: "60%",
      style: {
        fontSize: "14px",
      },
    },
    tooltip: {
      enabled: false,
    },
    xaxis: {
      categories: ["تحویل شده"],
    },
    yaxis: {
      max: 100,
    },
    responsive: [{
      breakpoint: 767,
      options: {
        title: {
          style: {
            fontSize: "16px",
          },
        },
      },
    }]
  };

  var chartProgress4 = new ApexCharts(
    document.querySelector("#progress4"),
    optionsProgress4
  );
  chartProgress4.render();

  var optionsProgress5 = {
    chart: {
      height: 70,
      type: "bar",
      stacked: true,
      sparkline: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "15%",
        colors: {
          backgroundBarColors: ["#3eb95f"],
          backgroundBarOpacity: 0.2,
          backgroundBarRadius: 10,
        },
      },
    },
    colors: ["#3eb95f"],
    stroke: {
      width: 0,
    },
    series: [
      {
        name: "درانتظار",
        data: [74],
      },
    ],
    fill: {
      colors: ["#3eb95f"],
      type: "gradient",
      gradient: {
        gradientToColors: ["#3eb95f"],
      },
    },
    title: {
      floating: true,
      offsetX: -10,
      offsetY: 5,
      text: "در انتظار",
      style: {
        fontSize: "18px",
        fontFamily: "Roboto, sans-serif",
        fontWeight: 500,
      },
    },
    subtitle: {
      floating: true,
      align: "right",
      offsetY: 0,
      text: "74%",
      style: {
        fontSize: "14px",
      },
    },
    tooltip: {
      enabled: false,
    },
    xaxis: {
      categories: ["در انتظار"],
    },
    yaxis: {
      max: 100,
    },
    responsive: [{
      breakpoint: 767,
      options: {
        title: {
          style: {
            fontSize: "16px",
          },
        },
      },
    }]
  };

  var chartProgress5 = new ApexCharts(
    document.querySelector("#progress5"),
    optionsProgress5
  );
  chartProgress5.render();

  window.setInterval(function () {
    iteration++;

    chartColumn.updateSeries([
      {
        data: [
          ...chartColumn.w.config.series[0].data,
          [chartColumn.w.globals.maxX + 210000, getRandom()],
        ],
      },
    ]),
      chartLine.updateSeries([
        {
          data: [
            ...chartLine.w.config.series[0].data,
            [chartLine.w.globals.maxX + 300000, getRandom()],
          ],
        },
        {
          data: [
            ...chartLine.w.config.series[1].data,
            [chartLine.w.globals.maxX + 300000, getRandom()],
          ],
        },
      ]);

    chartCircle.updateSeries([
      getRangeRandom({ min: 10, max: 100 }),
      getRangeRandom({ min: 10, max: 100 }),
      getRangeRandom({ min: 10, max: 100 }),
    ]);

    var p1Data = getRangeRandom({ min: 10, max: 100 });
    chartProgress1.updateOptions({
      series: [
        {
          data: [p1Data],
        },
      ],
      subtitle: {
        text: p1Data + "%",
      },
    });

    var p2Data = getRangeRandom({ min: 10, max: 100 });
    chartProgress2.updateOptions({
      series: [
        {
          data: [p2Data],
        },
      ],
      subtitle: {
        text: p2Data + "%",
      },
    });

    var p3Data = getRangeRandom({ min: 10, max: 100 });
    chartProgress3.updateOptions({
      series: [
        {
          data: [p3Data],
        },
      ],
      subtitle: {
        text: p3Data + "%",
      },
    });

    var p4Data = getRangeRandom({ min: 10, max: 100 });
    chartProgress4.updateOptions({
      series: [
        {
          data: [p4Data],
        },
      ],
      subtitle: {
        text: p4Data + "%",
      },
    });

    var p5Data = getRangeRandom({ min: 10, max: 100 });
    chartProgress5.updateOptions({
      series: [
        {
          data: [p5Data],
        },
      ],
      subtitle: {
        text: p5Data + "%",
      },
    });
  }, 3000);
})(jQuery);