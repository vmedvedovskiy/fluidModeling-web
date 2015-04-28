(function () {
    var data = null;
    var graph = null;

    google.load("visualization", "1");

    // Set callback to run when API is loaded
    google.setOnLoadCallback(drawVisualization);

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
            data: JSON.stringify({
                step: 0.05,
                xBounds: {
                    low: -2,
                    high: 2
                },
                yBounds: {
                    low: -1,
                    high: 1
                }
            }),
            success: function (result, status, xhr) {
                var pointsLength = result.length;
                for (var i = 0; i < pointsLength; ++i) {
                    data.addRow([result[i].X, result[i].Y, result[i].Z]);
                }

                // create some nice looking data with sin/cos
                var steps = 25;  // number of datapoints will be steps*steps
                var axisMax = 314;
                axisStep = axisMax / steps;


                // specify options
                options = {
                    width: "400px",
                    height: "400px",
                    style: "surface",
                    showPerspective: true,
                    showGrid: true,
                    showShadow: false,
                    keepAspectRatio: true,
                    verticalRatio: 0.5
                };

                // Instantiate our graph object.
                graph = new links.Graph3d(document.getElementById('mygraph'));

                // Draw our graph with the created data and options
                graph.draw(data, options);
            },
            dataType: 'json',
            contentType: 'application/json'
        });
    }
})();