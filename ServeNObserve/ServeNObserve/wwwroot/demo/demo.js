demo = {
  initPickColor: function() {
    $('.pick-class-label').click(function() {
      var new_class = $(this).attr('new-class');
      var old_class = $('#display-buttons').attr('data-class');
      var display_div = $('#display-buttons');
      if (display_div.length) {
        var display_buttons = display_div.find('.btn');
        display_buttons.removeClass(old_class);
        display_buttons.addClass(new_class);
        display_div.attr('data-class', new_class);
      }
    });
  },

  initDashboardPageCharts: function(labels,incomeData,outcomeData) {

    chartColor = "#FFFFFF";

    // General configuration for the charts with Line gradientStroke
    gradientChartOptionsConfiguration = {
      maintainAspectRatio: false,
      legend: {
        display: false
      },
      tooltips: {
        bodySpacing: 4,
        mode: "nearest",
        intersect: 0,
        position: "nearest",
        xPadding: 10,
        yPadding: 10,
        caretPadding: 10
      },
      responsive: 1,
      scales: {
        yAxes: [{
          display: 0,
          gridLines: 0,
          ticks: {
            display: false
          },
          gridLines: {
            zeroLineColor: "transparent",
            drawTicks: false,
            display: false,
            drawBorder: false
          }
        }],
        xAxes: [{
          display: 0,
          gridLines: 0,
          ticks: {
            display: false
          },
          gridLines: {
            zeroLineColor: "transparent",
            drawTicks: false,
            display: false,
            drawBorder: false
          }
        }]
      },
      layout: {
        padding: {
          left: 0,
          right: 0,
          top: 15,
          bottom: 15
        }
      }
    };
    var canvas = document.getElementById('bigDashboardChart');
    var ctx = canvas.getContext("2d");

    var incomeGradientFill = ctx.createLinearGradient(0, 200, 0, 50);
    incomeGradientFill.addColorStop(0, "rgba(128, 182, 244, 0)");
    incomeGradientFill.addColorStop(1, "rgba(0, 255, 0, 0.24)");


    var outcomeGradientFill = ctx.createLinearGradient(0, 200, 0, 50);
    outcomeGradientFill.addColorStop(0, "rgba(255, 0, 0, 0.24)");
    outcomeGradientFill.addColorStop(1, "rgba(128, 182, 244, 0)");

    let delayed;
    var myChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: "Gelir",
          borderColor: chartColor,
          backgroundColor: incomeGradientFill,
          borderWidth: 2,
          data: incomeData
        },{
          label: "Gider",
          borderColor: chartColor,
          backgroundColor: outcomeGradientFill,
          borderWidth: 2,
          data: outcomeData
        }]
      },
      options: {
        layout: {
          padding: {
            left: 20,
            right: 20,
            top: 0,
            bottom: 0
          }
        },
        maintainAspectRatio: false,
        tooltips: {
          backgroundColor: '#fff',
          titleFontColor: '#333',
          bodyFontColor: '#666',
          bodySpacing: 4,
          xPadding: 12,
          mode: "nearest",
          intersect: 0,
          position: "nearest"
        },
        legend: {
          position: "bottom",
          fillStyle: "#FFF",
          display: false
        },
        scales: {
          yAxes: [{
            ticks: {
              fontColor: "rgba(255,255,255,0.4)",
              fontStyle: "bold",
              beginAtZero: true,
              maxTicksLimit: 5,
              padding: 10
            },
            gridLines: {
              drawTicks: true,
              drawBorder: false,
              display: true,
              color: "rgba(255,255,255,0.1)",
              zeroLineColor: "transparent"
            }

          }],
          xAxes: [{
            gridLines: {
              zeroLineColor: "transparent",
              display: false,

            },
            ticks: {
              padding: 10,
              fontColor: "rgba(255,255,255,0.4)",
              fontStyle: "bold"
            }
          }]
        },
      }
    });

    canvas.onclick = function(evt){
      var activePoints = myChart.getElementsAtEvent(evt);
      if (activePoints[1]) 
      {
        var chartData = activePoints[1]['_chart'].config.data;

        var idx = activePoints[1]['_index'];

        var value = chartData.datasets[1].data[idx];

        if(value == 0)
          return;

        var label = chartData.labels[idx];

        //alert(label);
      }
    };

  },

  initGoogleMaps: function() {
    var myLatlng = new google.maps.LatLng(40.748817, -73.985428);
    var mapOptions = {
      zoom: 13,
      center: myLatlng,
      scrollwheel: false, //we disable de scroll over the map, it is a really annoing when you scroll through page
      styles: [{
        "featureType": "water",
        "elementType": "geometry",
        "stylers": [{
          "color": "#e9e9e9"
        }, {
          "lightness": 17
        }]
      }, {
        "featureType": "landscape",
        "elementType": "geometry",
        "stylers": [{
          "color": "#f5f5f5"
        }, {
          "lightness": 20
        }]
      }, {
        "featureType": "road.highway",
        "elementType": "geometry.fill",
        "stylers": [{
          "color": "#ffffff"
        }, {
          "lightness": 17
        }]
      }, {
        "featureType": "road.highway",
        "elementType": "geometry.stroke",
        "stylers": [{
          "color": "#ffffff"
        }, {
          "lightness": 29
        }, {
          "weight": 0.2
        }]
      }, {
        "featureType": "road.arterial",
        "elementType": "geometry",
        "stylers": [{
          "color": "#ffffff"
        }, {
          "lightness": 18
        }]
      }, {
        "featureType": "road.local",
        "elementType": "geometry",
        "stylers": [{
          "color": "#ffffff"
        }, {
          "lightness": 16
        }]
      }, {
        "featureType": "poi",
        "elementType": "geometry",
        "stylers": [{
          "color": "#f5f5f5"
        }, {
          "lightness": 21
        }]
      }, {
        "featureType": "poi.park",
        "elementType": "geometry",
        "stylers": [{
          "color": "#dedede"
        }, {
          "lightness": 21
        }]
      }, {
        "elementType": "labels.text.stroke",
        "stylers": [{
          "visibility": "on"
        }, {
          "color": "#ffffff"
        }, {
          "lightness": 16
        }]
      }, {
        "elementType": "labels.text.fill",
        "stylers": [{
          "saturation": 36
        }, {
          "color": "#333333"
        }, {
          "lightness": 40
        }]
      }, {
        "elementType": "labels.icon",
        "stylers": [{
          "visibility": "off"
        }]
      }, {
        "featureType": "transit",
        "elementType": "geometry",
        "stylers": [{
          "color": "#f2f2f2"
        }, {
          "lightness": 19
        }]
      }, {
        "featureType": "administrative",
        "elementType": "geometry.fill",
        "stylers": [{
          "color": "#fefefe"
        }, {
          "lightness": 20
        }]
      }, {
        "featureType": "administrative",
        "elementType": "geometry.stroke",
        "stylers": [{
          "color": "#fefefe"
        }, {
          "lightness": 17
        }, {
          "weight": 1.2
        }]
      }]
    };


    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    var marker = new google.maps.Marker({
      position: myLatlng,
      title: "Hello World!"
    });

    // To add the marker to the map, call setMap();
    marker.setMap(map);
  },

  initDashboardOutcome: function (mainText,labels,outcome) 
  {
      $('.outcomeData').text(`${mainText}`);
    var e = document.getElementById("barChartSimpleGradientsNumbers").getContext("2d");
    
    gradientFill = e.createLinearGradient(0, 170, 0, 50);
    gradientFill.addColorStop(0, "rgba(128, 182, 244, 0)");
    gradientFill.addColorStop(1, hexToRGB('#2CA8FF', 0.6));
    
    var a = {
      type: "bar",
      data: {
        labels: labels,
        datasets: [{
          backgroundColor: gradientFill,
          borderColor: "#2CA8FF",
          pointBorderColor: "#FFF",
          pointBackgroundColor: "#2CA8FF",
          pointBorderWidth: 2,
          pointHoverRadius: 4,
          pointHoverBorderWidth: 1,
          pointRadius: 4,
          fill: true,
          borderWidth: 1,
          data: outcome
        }]
      },
      options: {
        maintainAspectRatio: false,
        legend: {
          display: false
        },
        tooltips: {
          bodySpacing: 4,
          mode: "nearest",
          intersect: 0,
          position: "nearest",
          xPadding: 10,
          yPadding: 10,
          caretPadding: 10
        },
        responsive: 1,
        scales: {
          yAxes: [{
            gridLines: 0,
            gridLines: {
              zeroLineColor: "transparent",
              drawBorder: false
            }
          }],
          xAxes: [{
            ticks: {
              padding: 10,
              fontColor: "rgba(90,90,90,1)",
              fontStyle: "bold"
            },
            gridLines: {
              zeroLineColor: "transparent",
              drawTicks: false,
              display: false,
              drawBorder: false
            }
          }]
        },
        layout: {
          padding: {
            left: 0,
            right: 0,
            top: 15,
            bottom: 15
          }
        }
      }
    };
    new Chart(e, a);
  }
};




