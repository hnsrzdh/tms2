(function ($) {
  google.charts.load("current", { packages: ["corechart", "bar"] });
  google.charts.load("current", { packages: ["line"] });
  google.charts.load("current", { packages: ["corechart"] });
  google.charts.setOnLoadCallback(drawBasic);
  function drawBasic() {
    if ($("#column-chart1").length > 0) {
      var a = google.visualization.arrayToDataTable([
          ["سال", "فروش", "هزینه‌ها", "سود"],
          ["1390", 1e3, 400, 250],
          ["1391", 1170, 460, 300],
          ["1392", 660, 1120, 400],
          ["1393", 1030, 540, 450],
        ]),
        b = {
          chart: {
            title: "عملکرد شرکت",
            subtitle: "فروش, هزینه‌ها و سود: 1390-1395",
          },
          bars: "vertical",
          vAxis: {
            format: "decimal",
          },
          height: 400,
          width: "100%",
          colors: [
            AdmiroAdminConfig.primary,
            AdmiroAdminConfig.secondary,
            "#3eb95f",
          ],
        },
        c = new google.charts.Bar(document.getElementById("column-chart1"));
      c.draw(a, google.charts.Bar.convertOptions(b));
    }
    if ($("#column-chart2").length > 0) {
      var a = google.visualization.arrayToDataTable([
          ["سال", "فروش", "هزینه‌ها", "سود"],
          ["1390", 1e3, 400, 250],
          ["1391", 1170, 460, 300],
          ["1392", 660, 1120, 400],
          ["1393", 1030, 540, 450],
        ]),
        b = {
          chart: {
            title: "عملکرد شرکت",
            subtitle: "فروش, هزینه‌ها و سود: 1390-1395",
          },
          bars: "horizontal",
          vAxis: {
            format: "decimal",
          },
          height: 400,
          width: "100%",
          colors: [
            AdmiroAdminConfig.primary,
            AdmiroAdminConfig.secondary,
            "#3eb95f",
          ],
        },
        c = new google.charts.Bar(document.getElementById("column-chart2"));
      c.draw(a, google.charts.Bar.convertOptions(b));
    }
    if ($("#pie-chart1").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["وظیفه", "Hours per Day"],
        ["کار", 5],
        ["خوردن", 10],
        ["رفت و آمد", 15],
        ["تلویزیون تماشا کنید", 20],
        ["خواب", 25],
      ]);
      var options = {
        title: "فعالیت‌های روزانه من",
        width: "100%",
        height: 300,
        colors: [
          "#e74b2b",
          "#3eb95f",
          "#ea9200",
          AdmiroAdminConfig.secondary,
          AdmiroAdminConfig.primary,
        ],
      };
      var chart = new google.visualization.PieChart(
        document.getElementById("pie-chart1")
      );
      chart.draw(data, options);
    }
    if ($("#pie-chart2").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["وظیفه", "Hours per Day"],
        ["کار", 5],
        ["خوردن", 10],
        ["رفت و آمد", 15],
        ["تلویزیون تماشا کنید", 20],
        ["خواب", 25],
      ]);
      var options = {
        title: "فعالیت‌های روزانه من",
        is3D: true,
        width: "100%",
        height: 300,
        colors: [
          "#e74b2b",
          "#ea9200",
          "#3eb95f",
          AdmiroAdminConfig.secondary,
          AdmiroAdminConfig.primary,
        ],
      };
      var chart = new google.visualization.PieChart(
        document.getElementById("pie-chart2")
      );
      chart.draw(data, options);
    }
    if ($("#pie-chart3").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["وظیفه", "Hours per Day"],
        ["کار", 2],
        ["خوردن", 2],
        ["رفت و آمد", 11],
        ["تلویزیون تماشا کنید", 2],
        ["خواب", 7],
      ]);
      var options = {
        title: "فعالیت‌های روزانه من",
        pieHole: 0.4,
        width: "100%",
        height: 300,
        colors: [
          "#e74b2b",
          "#ea9200",
          "#3eb95f",
          AdmiroAdminConfig.secondary,
          AdmiroAdminConfig.primary,
        ],
      };
      var chart = new google.visualization.PieChart(
        document.getElementById("pie-chart3")
      );
      chart.draw(data, options);
    }
    if ($("#pie-chart4").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["Language", "بلندگوها"],
        ["Assamese", 13],
        ["Bengali", 83],
        ["Bodo", 1.4],
        ["Dogri", 2.3],
        ["Gujarati", 46],
        ["Hindi", 300],
        ["Kannada", 38],
        ["Kashmiri", 5.5],
        ["Konkani", 5],
        ["Maithili", 20],
        ["Malayalam", 33],
        ["Manipuri", 1.5],
        ["Marathi", 72],
        ["Nepali", 2.9],
        ["Oriya", 33],
        ["Punjabi", 29],
        ["Sanskrit", 0.01],
        ["Santhali", 6.5],
        ["Sindhi", 2.5],
        ["Tamil", 61],
        ["Telugu", 74],
        ["Urdu", 52],
      ]);
      var options = {
        title: "کاربرد زبان هندی",
        legend: "none",
        width: "100%",
        height: 300,
        pieSliceText: "label",
        slices: {
          4: { offset: 0.2 },
          12: { offset: 0.3 },
          14: { offset: 0.4 },
          15: { offset: 0.5 },
        },
        colors: [
          "#dc3545",
          AdmiroAdminConfig.primary,
          AdmiroAdminConfig.secondary,
          "#3eb95f",
          "#ea9200",
          "#e74b2b",
          "#dc3545",
          AdmiroAdminConfig.primary,
          "#e74b2b",
          "#3eb95f",
          AdmiroAdminConfig.primary,
          AdmiroAdminConfig.secondary,
          "#3eb95f",
          AdmiroAdminConfig.primary,
          "#ea9200",
          "#e74b2b",
          AdmiroAdminConfig.primary,
          AdmiroAdminConfig.primary,
          "#ea9200",
          AdmiroAdminConfig.secondary,
          AdmiroAdminConfig.primary,
          "#3eb95f",
        ],
      };
      var chart = new google.visualization.PieChart(
        document.getElementById("pie-chart4")
      );
      chart.draw(data, options);
    }
    if ($("#line-chart").length > 0) {
      var data = new google.visualization.DataTable();
      data.addColumn("number", "ماه");
      data.addColumn("number", "نگهبانان کهکشان");
      data.addColumn("number", "انتقام‌جویان");
      data.addColumn("number", "تبدیل‌شوندگان: عصر انقراض");
      data.addRows([
        [1, 37.8, 80.8, 41.8],
        [2, 30.9, 10.5, 32.4],
        [3, 40.4, 57, 25.7],
        [4, 11.7, 18.8, 10.5],
        [5, 20, 17.6, 10.4],
        [6, 8.8, 13.6, 7.7],
        [7, 7.6, 12.3, 9.6],
        [8, 12.3, 29.2, 10.6],
        [9, 16.9, 42.9, 14.8],
        [10, 12.8, 30.9, 11.6],
        [11, 5.3, 7.9, 4.7],
        [12, 6.6, 8.4, 5.2],
      ]);
      var options = {
        chart: {
          title: "درآمد گیشه در دو هفته اول اکران",
          subtitle: "به میلیون دلار",
        },
        colors: [AdmiroAdminConfig.primary, AdmiroAdminConfig.secondary, "#3eb95f"],
        height: 500,
        width: "100%",
      };
      var chart = new google.charts.Line(document.getElementById("line-chart"));
      chart.draw(data, google.charts.Line.convertOptions(options));
    }
    if ($("#combo-chart").length > 0) {
      var data = google.visualization.arrayToDataTable([
        [
          "Month",
          "Bolivia",
          "Ecuador",
          "Madagascar",
          "Papua",
          "Rwanda",
          "Average",
        ],
        ["2004/05", 165, 938, 522, 998, 450, 614.6],
        ["2005/06", 135, 1120, 599, 1268, 288, 682],
        ["2006/07", 157, 1167, 587, 807, 397, 623],
        ["2007/08", 139, 1110, 615, 968, 215, 609.4],
        ["2008/09", 136, 691, 629, 1026, 366, 569.6],
      ]);
      var options = {
        title: "Monthly Coffee Production by Country",
        vAxis: { title: "Cups" },
        hAxis: { title: "Month" },
        seriesType: "bars",
        series: { 5: { type: "line" } },
        height: 500,
        width: "100%",
        colors: [
          AdmiroAdminConfig.primary,
          AdmiroAdminConfig.secondary,
          "#3eb95f",
          "#ea9200",
          "#e74b2b",
        ],
      };
      var chart = new google.visualization.ComboChart(
        document.getElementById("combo-chart")
      );
      chart.draw(data, options);
    }
    if ($("#area-chart1").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["سال", "فروش", "هزینه‌ها"],
        ["1394", 1000, 400],
        ["1395", 1170, 460],
        ["1396", 660, 1120],
        ["1397", 1030, 540],
      ]);
      var options = {
        title: "عملکرد شرکت",
        hAxis: { title: "سال", titleTextStyle: { color: "#333" } },
        vAxis: { minValue: 0 },
        width: "100%",
        height: 400,
        colors: [AdmiroAdminConfig.primary, AdmiroAdminConfig.secondary],
      };
      var chart = new google.visualization.AreaChart(
        document.getElementById("area-chart1")
      );
      chart.draw(data, options);
    }
    if ($("#area-chart2").length > 0) {
      var data = google.visualization.arrayToDataTable([
        ["سال", "ماشین‌ها", "کامیون‌ها", "پهپادها", "سگوی‌ها"],
        ["1413", 100, 400, 2000, 400],
        ["1414", 500, 700, 530, 800],
        ["1415", 2000, 1000, 620, 120],
        ["1416", 120, 201, 2501, 540],
      ]);
      var options = {
        title: "عملکرد شرکت",
        hAxis: { title: "سال", titleTextStyle: { color: "#333" } },
        vAxis: { minValue: 0 },
        width: "100%",
        height: 400,
        colors: [
          AdmiroAdminConfig.primary,
          AdmiroAdminConfig.secondary,
          "#3eb95f",
          "#e74b2b",
        ],
      };
      var chart = new google.visualization.AreaChart(
        document.getElementById("area-chart2")
      );
      chart.draw(data, options);
    }
    if ($("#bar-chart2").length > 0) {
      var a = google.visualization.arrayToDataTable([
          [
            "Element",
            "تراکم",
            {
              role: "style",
            },
          ],
          ["مس", 10, "#ea9200"],
          ["نقره", 12, "#e74b2b"],
          ["طلا", 14, "#f73164"],
          ["پلاتینیوم", 16, "color: #308e87"],
        ]),
        d = new google.visualization.DataView(a);
      d.setColumns([
        0,
        1,
        {
          calc: "stringify",
          sourceColumn: 1,
          type: "string",
          role: "annotation",
        },
        2,
      ]);
      var b = {
          title: "چگالی فلزات گرانبها، بر حسب گرم بر سانتی‌متر مکعب",
          width: "100%",
          height: 400,
          bar: {
            groupWidth: "95%",
          },
          legend: {
            position: "none",
          },
        },
        c = new google.visualization.BarChart(
          document.getElementById("bar-chart2")
        );
      c.draw(d, b);
    }
  }
  // Gantt chart
  google.charts.load("current", { packages: ["gantt"] });
  google.charts.setOnLoadCallback(drawChart);

  function daysToMilliseconds(days) {
    return days * 24 * 60 * 60 * 1000;
  }

  function drawChart() {
    var data = new google.visualization.DataTable();
    data.addColumn("string", "Task ID");
    data.addColumn("string", "Task Name");
    data.addColumn("string", "Resource");
    data.addColumn("date", "Start Date");
    data.addColumn("date", "End Date");
    data.addColumn("number", "Duration");
    data.addColumn("number", "Percent Complete");
    data.addColumn("string", "Dependencies");

    data.addRows([
      [
        "Research",
        "Find sources",
        null,
        new Date(2015, 0, 1),
        new Date(2015, 0, 5),
        null,
        100,
        null,
      ],
      [
        "Write",
        "Write paper",
        "write",
        null,
        new Date(2015, 0, 9),
        daysToMilliseconds(3),
        25,
        "Research,Outline",
      ],
      [
        "Cite",
        "Create bibliography",
        "write",
        null,
        new Date(2015, 0, 7),
        daysToMilliseconds(1),
        20,
        "Research",
      ],
      [
        "Complete",
        "Hand in paper",
        "complete",
        null,
        new Date(2015, 0, 10),
        daysToMilliseconds(1),
        0,
        "Cite,Write",
      ],
      [
        "Outline",
        "Outline paper",
        "write",
        null,
        new Date(2015, 0, 6),
        daysToMilliseconds(1),
        100,
        "Research",
      ],
    ]);

    var options = {
      height: 275,
      gantt: {
        criticalPathEnabled: false, // Critical path arrows will be the same as other arrows.
        arrow: {
          angle: 100,
          width: 5,
          color: "#3eb95f",
          radius: 0,
        },

        palette: [
          {
            color: AdmiroAdminConfig.primary,
            dark: AdmiroAdminConfig.secondary,
            light: "#047afb",
          },
        ],
      },
    };
    var chart = new google.visualization.Gantt(
      document.getElementById("gantt_chart")
    );

    chart.draw(data, options);
  }
  // word tree
  google.charts.load("current1", { packages: ["wordtree"] });
  google.charts.setOnLoadCallback(drawChart1);

  function drawChart1() {
    var data = google.visualization.arrayToDataTable([
      ["Phrases"],
      ["cats are better than dogs"],
      ["cats eat kibble"],
      ["cats are better than hamsters"],
      ["cats are awesome"],
      ["cats are people too"],
      ["cats eat mice"],
      ["cats meowing"],
      ["cats in the cradle"],
      ["cats eat mice"],
      ["cats in the cradle lyrics"],
      ["cats eat kibble"],
      ["cats for adoption"],
      ["cats are family"],
      ["cats eat mice"],
      ["cats are better than kittens"],
      ["cats are evil"],
      ["cats are weird"],
      ["cats eat mice"],
    ]);

    var options = {
      wordtree: {
        format: "implicit",
        word: "cats",
      },
    };
    var chart = new google.visualization.WordTree(
      document.getElementById("wordtree_basic")
    );
    chart.draw(data, options);
  }
})(jQuery);
