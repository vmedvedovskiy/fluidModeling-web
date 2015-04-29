(function () {
    var data = null;
    var graph = null;

    google.load("visualization", "1");

    // Set callback to run when API is loaded
    google.setOnLoadCallback(drawVisualization);

    var querySettings = {
        step: 0.1,
        xBounds: {
            low: -5,
            high: 5
        },
        yBounds: {
            low: 0,
            high: 1
        },
        M: 5
    }

    // Called when the Visualization API is loaded.
    function drawVisualization() {
        // Create and populate a data table.
        data = new google.visualization.DataTable();
        data.addColumn('number', 'x');
        data.addColumn('number', 'y');
        data.addColumn('number', 'value');

        $.ajax({
            type: 'POST',
            url: 'http://localhost:9000/api/sphere/points',
            data: JSON.stringify(querySettings),
            success: function (result, status, xhr) {
                var chart3d = document.createElement('DIV'),
                    contourChart = document.createElement('DIV');

                create3DChart(chart3d, result);
                // createContourChart(contourChart, result);

                document.body.appendChild(chart3d);
                document.body.appendChild(contourChart);
            },
            dataType: 'json',
            contentType: 'application/json'
        });
    }

    function create3DChart(node, result) {
        var pointsLength = result.length;
        for (var i = 0; i < pointsLength; ++i) {
            data.addRow([result[i].X, result[i].Y, result[i].Z]);
        }

        // specify options
        options = {
            width: "100%",
            height: "800px",
            style: "surface",
            showPerspective: true,
            showGrid: true,
            showShadow: false,
            keepAspectRatio: true,
            verticalRatio: 0.5,
            xMax: querySettings.xBounds.high,
            xMin: querySettings.xBounds.low,
            yMax: querySettings.yBounds.high,
            yMin: querySettings.yBounds.low,
            valueMin: 0
        };

        // Instantiate our graph object.
        graph = new links.Graph3d(node);
        // Draw our graph with the created data and options
        graph.draw(data, options);
    }

    function createContourChart(node, result) {
        var margin = { top: 20, right: 20, bottom: 30, left: 30 },
            width = 960 - margin.left - margin.right,
            height = 500 - margin.top - margin.bottom;

        var x = d3.scale.linear()
            .range([0, width]);

        var y = d3.scale.linear()
            .range([height, 0]);

        var color = d3.scale.linear()
            .domain([0.02, 0.05, 0.07, 0.1, 0.2, 0.5, 1, 2, 3, 5, 10, 20])
            .range(["#0a0", "#6c0", "#ee0", "#eb4", "#eb9", "#fff"]);

        var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom")
            .ticks(20);

        var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left");

        var svg = d3.select(node).append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
          .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        var dx = Math.floor((querySettings.xBounds.high - querySettings.xBounds.low) / querySettings.step) + 1,
            dy = Math.floor((querySettings.yBounds.high - querySettings.yBounds.low) / querySettings.step) + 1;

        x.domain([0, dx]);
        y.domain([0, dy]);

        var data = {},
            isoscaleX = [],
            isoscaleY = [];

        for (var i = 0; i < dx; i++) {
            isoscaleX[i] = querySettings.xBounds.low + i * querySettings.step;
        }

        for (var i = 0; i < dy; i++) {
            isoscaleY[i] = querySettings.yBounds.low + i * querySettings.step;
        }

        result.map(function (item) {
            if (!data[item.Y]) {
                data[item.Y] = {};
            }

            data[item.Y][item.X] = item.Z;
        });

        svg.selectAll(".isoline")
            .data(color.domain().map(isoline))
          .enter().append("path")
            .datum(function (d) { return d3.geom.contour(d).map(transform); })
            .attr("class", "isoline")
            .attr("d", function (d) { return "M" + d.join("L") + "Z"; })
            .style("fill", function (d, i) { return color.range()[i]; });

        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);

        svg.append("g")
            .attr("class", "y axis")
            .call(yAxis);

        function isoline(min) {
            return function (x, y) {
                if (!isoscaleY[y] || !isoscaleX[x]) {
                    return;
                }

                var i = +isoscaleY[y].toFixed(2),
                    j = +isoscaleX[x].toFixed(2);

                return x >= 0 && y >= 0 && x < dx && y < dy
                    && data[i]
                    && data[i][j]
                    && data[i][j] >= min;
            };
        }

        function transform(point) {
            return [point[0] * width / dx, point[1] * height / dy];
        }
    }
})();