var nercholidays = [new Date(2015, 1, 1), new Date(2015, 5, 25), new Date(2015, 7, 4), new Date(2015, 11, 26), new Date(2015, 12, 25),
    new Date(2016, 1, 1), new Date(2016, 5, 30), new Date(2016, 7, 4), new Date(2016, 11, 24), new Date(2016, 12, 25),
    new Date(2017, 1, 1), new Date(2017, 5, 29), new Date(2017, 7, 4), new Date(2017, 11, 23), new Date(2017, 12, 25),
    new Date(2018, 1, 1), new Date(2018, 5, 28), new Date(2018, 7, 4), new Date(2018, 11, 22), new Date(2018, 12, 25),
    new Date(2019, 1, 1), new Date(2019, 5, 27), new Date(2019, 7, 4), new Date(2019, 11, 28), new Date(2019, 12, 25),
    new Date(2020, 1, 1), new Date(2020, 5, 25), new Date(2020, 7, 4), new Date(2020, 11, 26), new Date(2020, 12, 25)];

var layoutFormat = '{"controlid" : null, "locationx" : null, "locationy" : null, "width" : null, "height" : null, "dock" : null, "locations" : [ ], "dates" : [ ] , "children" : [ ]}"'

var app = angular.module('dashboardsApp', ['jqwidgets']).run(function ($rootScope, $timeout) {
    $rootScope.dateformat = {
        formatString: 'MM/dd/yyyy',
    };

    $rootScope.numberformat = {
        digits: 4,
        decimalDigits: 2,
        inputMode: 'simple',
    };

    $rootScope.incrementsecond = function () {
        try {
            $rootScope.pjmtime.setSeconds($rootScope.pjmtime.getSeconds() + 1);
            $rootScope.misotime.setSeconds($rootScope.misotime.getSeconds() + 1);
            $rootScope.ercottime.setSeconds($rootScope.ercottime.getSeconds() + 1);
            $rootScope.caisotime.setSeconds($rootScope.caisotime.getSeconds() + 1);
            $rootScope.spptime.setSeconds($rootScope.spptime.getSeconds() + 1);
            $rootScope.nyisotime.setSeconds($rootScope.nyisotime.getSeconds() + 1);
            $rootScope.isonetime.setSeconds($rootScope.isonetime.getSeconds() + 1);
        }
        catch (err) {
            console.log(err.toString());
        }
    }

    $rootScope.server = {};
    $rootScope.port = 0;
    $rootScope.sessionid = '00000000-0000-0000-0000-000000000000';
});

app.filter('time24hr', function myDateFormat($filter) {
    return function (text) {
        var tempdate = new Date(text.replace(/-/g, "/"));
        return $filter('date')(tempdate, "HH:mm:ss");
    }
});

app.controller('mainController', ['$scope', '$rootScope', '$compile', '$http', '$interval', function ($scope, $rootScope, $compile, $http, $interval) {
    $scope.dockmanager = {};
    $scope.pushchannel = {};
    

    $scope.adddNewControl = function (controlID) {
        try {
            var market = '';
            var title = ''
            var controllerName = '';

            var id = parseInt(controlID);
            var marketid = id >>> 16;
            if (marketid == 1) {  // PJM
                market = "PJM";
            }
            else if (marketid == 2) { // MISO
                market = "MISO";
            }
            else if (marketid == 3) { // ERCOT
                market = "ERCOT";
            }
            else if (marketid == 4) { // CAISO
                market = "CAISO";
            }
            else if (marketid == 5) { // SPP
                market = "SPP";
            }
            else if (marketid == 6) { // NYISO
                market = "NYISO";
            }
            else if (marketid == 7) { // ISO-NE
                market = "ISO-NE";
            }

            var controllerid = id & 0xffff;
            if (controllerid == 1) {
                title = market + " Day Ahaed LMP";
            }
            else if (controllerid == 2) {
                title = market + " Balday Monitor";
            }
            else if (controllerid == 3) {
                title = market + " Tick Monitor";
            }
            else if (controllerid == 4) {
                title = market + " Dispatch Monitor";
            }
            else if (controllerid == 5) {
                title = market + " Day Ahaed LMP";
            }
            else if (controllerid == 6) {
                title = market + " Day Ahaed LMP";
            }
            else if (controllerid == 7) {
                title = market + " Day Ahaed LMP";
            }

            $scope.updateControl(controlID, title);
        }
        catch (err) {
            alert(err.toString());
        }
    }

    $scope.initialize = function (server, port) {
        try {
            $rootScope.server = server;
            $rootScope.port = port;
            $rootScope.sessionid = document.getElementById("cssmenu").getAttribute("session");

            // Convert a div to the dock manager.  Panels can then be docked on to it
            var divDockManager = document.getElementById("dock-manager");
            var dockManager = new dockspawn.DockManager(divDockManager);
            dockManager.initialize();
            // Let the dock manager element fill in the entire screen
            var onResized = function (e) {
                dockManager.resize(window.innerWidth - (divDockManager.clientLeft + divDockManager.offsetLeft), window.innerHeight - (divDockManager.clientTop + divDockManager.offsetTop));
            }
            window.onresize = onResized;
            onResized(null);

            dockmanager = dockManager;

            try {
                if ("WebSocket" in window) {
                    pushchannel = new WebSocket("ws://" + server + ":" + port + "/service/subscribe");

                    pushchannel.onopen = function () {
                        console.log('Sending ' + $rootScope.sessionid);
                        pushchannel.send($rootScope.sessionid);
                    
                    };

                    pushchannel.onmessage = function (event) {
                        try {
                            if (event.data != 'ack' && event.data != 'nak') {
                                $rootScope.$broadcast('data-received', $.parseJSON(event.data));
                            }
                            else {
                                console.log('Message received: ' + event.data);
                            }
                        }
                        catch (err) {
                            console.log(err.toString());
                        }
                    };

                    pushchannel.onclose = function () {
                        console.log('WebSocket connection closed');
                        history.go(0);
                    };
                }
                else {
                    alert("The browser doesn't support server push. Real time data will not be received.");
                }
            }
            catch (err) {
                console.log(err.toString());
            }

            try {
                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/pjm").
                    then(function (response) {
                        console.log('PJM: ' + response.data);
                        $rootScope.pjmtime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);``
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/miso").
                    then(function (response) {
                        console.log('MISO: ' + response.data);
                        $rootScope.misotime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/ercot").
                    then(function (response) {
                        console.log('ERCOT: ' + response.data);
                        $rootScope.ercottime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/spp").
                    then(function (response) {
                        console.log('SPP: ' + response.data);
                        $rootScope.spptime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/caiso").
                    then(function (response) {
                        console.log('CAISO: ' + response.data);
                        $rootScope.caisotime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/nyiso").
                    then(function (response) {
                        console.log('NYISO: ' + response.data);
                        $rootScope.nyisotime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/markettime/iso-ne").
                    then(function (response) {
                        console.log('ISO-NE: ' + response.data);
                        $rootScope.isonetime = new Date(response.data);
                    }, function (response) {
                        console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                    });

                $interval($rootScope.incrementsecond, 1000);
                $interval(function () {
                    if (pushchannel != null) {
                        pushchannel.send("ping");
                    }
                }, 25000);
            }
            catch (err) {
                console.log(err.toString());
            }
        }
        catch (err) {
            alert(err.toString());
        }
    }

    $scope.updateControl = function (controlID, title) {
        try {
            $http.post("http://" + $rootScope.server + ":" + $rootScope.port + "/home/getcontrol", controlID).
                then(function (response) {
                    var controlnode = document.createElement("div");
                    controlnode.innerHTML = response.data;

                    var outline = new dockspawn.PanelContainer(controlnode, dockmanager, title);
                    var documentNode = dockmanager.context.model.documentManagerNode;
                    dockmanager.dockLeft(documentNode, outline, 0.15);

                    $compile(controlnode)($scope);
                }, function (response) {
                    console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                });
        }
        catch (err) {
            alert(err.toString());
        }
    }

    $scope.loadlayout = function () {
        try {
            $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/loadlayout/" + String(controlID)).
                then(function (response) {

                }, function (response) {
                    console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                });
        }
        catch (err) {
            alert(err.toString());
        }
    }

    $scope.savelayout = function () {

    }
}]);

app.controller('loginController', ['$scope', '$http', function ($scope, $http) {
    $scope.username = '';

    $scope.loginScreen = true;
    $scope.forgotScreen = false;
    $scope.confirmScreen = false;

    $scope.logout = function (sessionid, url) {
        var request = new XMLHttpRequest();
        request.onload = function () {
        }

        request.open("POST", url, false);
        request.setRequestsHeader("Content-Type", "application/json")
        request.send("{'sessionid': '" + sessionid + "'}");
    }
}]);

app.controller('baldayMonitorController', ['$scope', function ($scope) {
    var datapointregistration = [];

    $scope.initialize = function (market) {
        try {
            $scope.market = market;
            $scope.onpeak = true;
            $scope.offpeak = false;
            $scope.selecteddates = [];
            $scope.textboxdate = new Date();
            $scope.useheatmap = true;
            $scope.onpeakhours = getonpeakhours(market);

            $scope.listsettings = {
                source: $scope.selecteddates,
                width: 100,
                height: 60,
                renderer: function (value) {
                    console.log(value.toString());
                    return getformatteddate(value);
                },
            };

            $scope.griddata = new Array();

            var sample = ['-20', '-10', '0', '5', '10', '15', '20', '25', '30', '35', '40', '45', '50', '55', '60', '65', '70', '80', '90', '100', '200', '400', '500', '550', '650', '850'];
            for (var j = 0; j < 10; j++) {
                var row = {};
                for (var i = 1; i <= 24; i++) {
                    row["he" + i] = parseFloat(sample[i]);
                }

                $scope.griddata[j] = row;
            }

            var sources = {
                localdata: $scope.griddata,
                datafields:
                    [
                        { name: 'date', type: 'number' },
                        { name: 'he1', type: 'number' },
                        { name: 'he2', type: 'number' },
                        { name: 'he2p', type: 'number' },
                        { name: 'he3', type: 'number' },
                        { name: 'he4', type: 'number' },
                        { name: 'he5', type: 'number' },
                        { name: 'he6', type: 'number' },
                        { name: 'he7', type: 'number' },
                        { name: 'he8', type: 'number' },
                        { name: 'he9', type: 'number' },
                        { name: 'he10', type: 'number' },
                        { name: 'he11', type: 'number' },
                        { name: 'he12', type: 'number' },
                        { name: 'he13', type: 'number' },
                        { name: 'he14', type: 'number' },
                        { name: 'he15', type: 'number' },
                        { name: 'he16', type: 'number' },
                        { name: 'he17', type: 'number' },
                        { name: 'he18', type: 'number' },
                        { name: 'he19', type: 'number' },
                        { name: 'he20', type: 'number' },
                        { name: 'he21', type: 'number' },
                        { name: 'he22', type: 'number' },
                        { name: 'he23', type: 'number' },
                        { name: 'he24', type: 'number' }
                    ],
                datatype: "array"
            };

            var heatmapRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
<<<<<<< .mine
||||||| .r93
                var forecolor = '#000000';
                var backcolor = '#ed0a01';

=======
                var forecolor = '#000000';
                var backcolor = '#ffffff';

>>>>>>> .r109
                var setbackground = function (element, color) {
                    try {
                        $(element).closest('div').css('background-color', color);
                    }
                    catch (err) {
                        console.log(err.toString());
                    }
                }

                try {
                    var forecolor = '#000000';
                    var backcolor = '#ffffff';
                    if ($scope.useheatmap) {
<<<<<<< .mine
                        backcolor = getbackcolor(value);
                        forecolor = getforecolor(value);
                        
                    }
||||||| .r93
                        if (value < -10) { backcolor = "#64019a" }
                        else if (value < 0) { backcolor = '#12108d' }
                        else if (value < 6) { backcolor = '#0201ff' }
                        else if (value < 14) { backcolor = '#3533ff' }
                        else if (value < 20) { backcolor = '#067dff' }
                        else if (value < 26) { backcolor = '#0ea8ff' }
                        else if (value < 30) { backcolor = '#11c8ff' }
                        else if (value < 34) { backcolor = '#14ebff' }
                        else if (value < 38) { backcolor = '#2bf5e7' }
                        else if (value < 46) { backcolor = '#67f7a2' }
                        else if (value < 50) { backcolor = '#87fb80' }
                        else if (value < 56) { backcolor = '#a6fd60' }
                        else if (value < 62) { backcolor = '#c3fe40' }
                        else if (value < 68) { backcolor = '#e2ff19' }
                        else if (value < 76) { backcolor = '#fbfe00' }
                        else if (value < 82) { backcolor = '#f9ed01' }
                        else if (value < 90) { backcolor = '#fcdb02' }
                        else if (value < 100) { backcolor = '#f9c802' }
                        else if (value < 115) { backcolor = '#f9b700' }
                        else if (value < 125) { backcolor = '#f6a300' }
                        else if (value < 150) { backcolor = '#f88f00' }
                        else if (value < 200) { backcolor = '#f27f00' }
                        else if (value < 250) { backcolor = '#f36c0a' }
                        else if (value < 300) { backcolor = '#f05900' }
                        else if (value < 400) { backcolor = '#ef4802' }
                        else if (value < 500) { backcolor = '#f0350c' }
                        else if (value < 600) { backcolor = '#ee2201' }
                        else if (value < 800) { backcolor = '#ee1100' }
=======
                        backcolor = getbackgroundcolor(value);
                        foreground = getforegroundcolor(value);
                    }
>>>>>>> .r109

                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + ";color:" + forecolor + ";background-color:" + backcolor + "'>" + value.toFixed(2) + "</div>";
                }
                catch (err) {
                    console.log(err.toString());
                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + ";color:" + forecolor + ";background-color:" + backcolor + "'></div>";
                }
            }

            $scope.tablesettings =
            {
                width: 1225,
                selectionmode: 'multiplecellsextended',
                sortable: false,
                source: sources,
                columns: [
                  { text: 'Date', datafield: 'date', width: 100, cellsalign: 'center', align: 'center' },
                  { text: 'HE01', datafield: 'he1', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE02', datafield: 'he2', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE02+', datafield: 'he2p', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2', hidden: true },
                  { text: 'HE03', datafield: 'he3', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE04', datafield: 'he4', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE05', datafield: 'he5', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE06', datafield: 'he6', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE07', datafield: 'he7', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE08', datafield: 'he8', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE09', datafield: 'he9', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE10', datafield: 'he10', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE11', datafield: 'he11', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE12', datafield: 'he12', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE13', datafield: 'he13', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE14', datafield: 'he14', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE15', datafield: 'he15', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE16', datafield: 'he16', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE17', datafield: 'he17', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE18', datafield: 'he18', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE19', datafield: 'he19', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE20', datafield: 'he20', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE21', datafield: 'he21', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE22', datafield: 'he22', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE23', datafield: 'he23', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' },
                  { text: 'HE24', datafield: 'he24', width: 45, cellsalign: 'center', align: 'center', cellsrenderer: heatmapRenderer, cellsformat: 'f2' }
                ]
            };

            $scope.onAddDateClick = function () {
                try {
                    if ($scope.selecteddates.indexOf($scope.textboxdate) < 0) {
                        $scope.selecteddates.push($scope.textboxdate);
                    }
                }
                catch (err) {
                    console.log(err.toString());
                }
            }

            $scope.onRemoveDateClick = function () {
                try {
                    var index = $scope.selecteddates.indexOf($scope.textboxdate);
                    if (index >= 0) {
                        $scope.selecteddates.splice(index, 1);
                    }
                }
                catch (err) {
                    console.log(err.toString());
                }
            }

            $scope.onoffpeakclick = function () {
                $("#baldayGrid").jqxGrid("beginupdate");

                var showonpeak = true;
                var showoffpeak = true;
                try {
                    if ($scope.onpeak) {

                    }
                    else {
                        showonpeak = false;
                    }

                    if ($scope.offpeak) {

                    }
                    else {
                        showoffpeak = false;
                    }

                    for (var i = 1; i <= 24; i++) {
                        var hour = 'HE';
                        if (i >= 10) {
                            hour = hour + i;
                        }
                        else {
                            hour = hour + '0' + i;
                        }

                    }
                }
                catch (err) {
                    console.log(err.toString());
                }

                $("#baldayGrid").jqxGrid("endupdate");
            }

            var refreshTable = function () {
                try {
                    $("#baldayGrid").jqxGrid('updatebounddata');
                }
                catch (err) {
                    console.log(err.toString());
                }
            };

            $scope.refreshTable = refreshTable;

            $scope.$watch('selecteddates', function () {
                try {
                    $("#baldayGrid").jqxGrid('beginupdate');
                    $("#baldayGrid").jqxGrid('showcolumn', 'HE02+');
                    $("#baldayGrid").jqxGrid('endupdate');
                }
                catch (err) {
                    alert(err.toString());
                }
            });

            $scope.$on('data-received', function (event, arg) {
                try {
                    console.log('control received data ' + JSON.stringify(arg));
                    if (arg.market.toLowerCase() == $scope.market.toLowerCase()) {

                    }
                }
                catch (err) {
                    console.log(err.toString());
                }
            });
        }
        catch (err) {
            console.log(err.toString());
        }
    };
}]);

app.controller('tickMonitorController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
    var datapointregistration = [];
    var timezone = null;

    $scope.initialize = function (market) {
        try {        
            $scope.market = market;
            $scope.selectedlocation = null;
            $scope.current = 1.0,
            $scope.previous = 0.0,
            $scope.delta = 0.0,
            $scope.updatedtime = Date();
            $scope.usecramer = true;
            $scope.dispatchenabled = true;

            if (market == 'caiso') {
                timezone = 'PPT';
            }
            else if (market == 'miso') {
                timezone = 'CST';
            }
            else if (market == 'ercot' || market == 'spp') {
                timezone = 'CPT';
            }
            else {
                timezone = "EPT";
            }

            $scope.onlocationchanged = function () {
            }

            $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/registerdatapoint/" + $rootScope.sessionid.substring(1, 37) + "/0x0001000000000020/no").
                then(function (response) {

                    $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/registerdatapoint/" + $rootScope.sessionid.substring(1, 37) + "/0x0001000000000020/no").
                        then(function (response) {
                            console.log("Data point registration for Tick Monitor was successful");

                            $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/marketdatabyrange/" + $rootScope.sessionid.substring(1, 37) + "/0x0001000000000001/2015-01-01/2015-01-01").
                                then(function (response) {
                                    console.log('PJM: ' + response.data);
                                }, function (response) {
                                    console.log("Server time returned nothing");
                                });

                        }, function (response) {
                            console.log("Data point registration for Tick Monitor was NOT successful");
                        });
                }, function (response) {
                    console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                });
        }
        catch (err) {
            console.log(err.toString);
        }
    };

    $scope.$on('data-received', function (event, arg) {
        try {
            if (datapointregistration.indexOf(arg.datapoint) >= 0) {

                //$scope.previous = $scope.current;
                //$scope.current = parseFloat(arg.value);
                //$scope.delta = Math.round((current - previous) / current * 10000.0) / 100.0;
            }
        }
        catch (err) {
            console.log(err.toString());
        }
    });
}]);

app.controller('dispatchController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
    $scope.market = 'PJM';

    var locationcount = 19;
    var slotcount = 21;
    var columninfo = [];

    $scope.initialize = function () {
        try {
            for (var i = 0; i <= slotcount; i++) {
                columninfo.push($.parseJSON('{ "time": null, "created": null }'));
            }

            $scope.griddata = [];
            for (var i = 1; i <= locationcount; i++) {
                $scope.griddata.push([i, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '']);
            }

            $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/registerdatapoint/" + $rootScope.sessionid.substring(1, 37) + "/0x0001000000000020/no").
                then(function (response) {
                    console.log("Data point registration for Dispatch Monitor was successful");
                }, function (response) {
                    console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                });

            $http.get("http://" + $rootScope.server + ":" + $rootScope.port + "/service/marketdatalatest/" + $rootScope.sessionid.substring(1, 37)  + "/0x0001000000000020/" + slotcount).
                then(function (response) {
                    try {
                        var dataset = $.parseJSON(response.data);
                        var startindex = 0;
                        var dataindex = 0;

                        for (var i = 0; i < columninfo.length; i++) {
                            if (columninfo[i].time == null) {
                                break;
                            }
                            else {
                                var reference = dataset[dataindex];
                                var timevalues = reference.time.split(":");
                                var created = new Data(reference.created);
                                var time = new Date(reference.date).setHours(parseInt(timevalues[0]), parseInt(timevalues[1]), parseInt(timevalues[2]));
                                if (columninfo[i].time > time) {
                                    startindex = i + 1;
                                }
                                else if (columninfo[i].time == time && columninfo[i].created < created) {
                                    dataindex = dataindex + locationcount;
                                }
                                else {
                                    break;
                                }
                            }
                        }

                        for (var i = dataindex; i < dataset.length; i = i + locationcount) {
                            var firstpoint = dataset[i];
                            var timevalues = firstpoint.time.split(":");
                            columninfo[startindex].time = new Date(firstpoint.date).setHours(parseInt(timevalues[0]), parseInt(timevalues[1]), parseInt(timevalues[2]));
                            columninfo[startindex].created = new Date(firstpoint.created);

                            for (var j = 0; j < locationcount; j++) {
                                var location = dataset[i + j].location;
                                var stacknumber = 0;
                                var separatorindex = location.indexOf(":");
                                if (separatorindex >= 0) {
                                    stacknumber = parseInt(location.substring(0, separatorindex), 10);
                                    location = location.substring(separatorindex + 1, location.length)
                                }
                                else {
                                    stacknumber = parseInt(location, 10);
                                    location = '';
                                }

                                var targetrow = $scope.griddata[stacknumber - 1];
                                targetrow[startindex + 1] = location + ":" + dataset[i + j].value;
                            }

                            startindex = startindex + 1;
                        }

                        $("#dispatchGrid").jqxGrid('updatebounddata');
                    }
                    catch (err) {
                        console.log(err.toString());
                    }
                }, function (response) {
                    console.log("Web server returned error: status:" + response.status + " message: " + response.statusText);
                });

<<<<<<< .mine
            var dispatchRenderer = function (row, columnfield, cell, defaulthtml, columnproperties) {
||||||| .r93
            var dispatchRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                var forecolor = '#000000';
                var backcolor = '#ed0a01';

=======
            var dispatchRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                var forecolor = '#000000';
                var backcolor = '#ffffff';

>>>>>>> .r109
                var zonename = '';
                var price = '0.0';

                var setbackground = function (element, color) {
                    try {
                        $(element).closest('div').css('background-color', color);
                    }
                    catch (err) {
                        console.log(err.toString());
                    }
                }

<<<<<<< .mine
                try {
                    var backcolor = getbackcolor(cell.value);
                    var forecolor = getforecolor(cell.value);

                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + ";color:" + forecolor + ";background-color:" + backcolor + "'>" + cell.value.toFixed(2) + "</div>";
||||||| .r93
                try {
                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + "'>" + value.toFixed(2) + "</div>";
=======
                if (value != null) {
                    console.log(JSON.stringify(value));

                    try {
                        var separatorindex = value.indexOf(':');
                        zonename = value.substring(0, separatorindex);

                        price = value.substring(separatorindex + 1, value.length);

                        backcolor = getbackgroundcolor(parseFloat(price));
                        foreground = getforegroundcolor(parseFloat(price));

                        return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + ";color:" + forecolor + ";background-color:" + backcolor + "'>" + zonename + "<br/>" + price + "</div>";
                    }
                    catch (err) {
                        console.log(err.toString());
                        return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + "'>" + zonename + "<br/>" + price + "</div>";
                    }
>>>>>>> .r109
                }
<<<<<<< .mine
                catch (err) {
                    console.log(err.toString());
                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + ";color:" + forecolor + ";background-color:" + backcolor + "'></div>";
||||||| .r93
                catch (err) {
                    console.log(err.toString());
                    return "<div style='width:100%;height:100%;margin:2px;float:" + columnproperties.cellsalign + "'></div>";
=======
                else {
                    return defaulthtml;
>>>>>>> .r109
                }
            }

            $scope.tablesettings =
            {
                width: 350,
                height: 711,
                rowsheight: 35,
                selectionmode: 'multiplecellsextended',
                sortable: false,
                source: {
                    localdata: $scope.griddata,
                    datatype: "array"
                },
                columns: [
                  { text: 'Zone', width: 40, cellsalign: 'center', align: 'center' },
                  { text: 't', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-15', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-30', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-45', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-60', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-75', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-90', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-105', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-120', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-135', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-150', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-165', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-180', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-195', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-210', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-225', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-240', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-255', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-270', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-285', width: 45, cellsrenderer: dispatchRenderer },
                  { text: 't-300', width: 45, cellsrenderer: dispatchRenderer },
                ]
            };

            $scope.$on('data-received', function (event, arg) {
                try {
                    if (arg.datapoint == "0x0001000000000020") {
                        var columnindex = 0;
                        var firstdata = arg.points[0];
                        var timevalues = firstdata.time.split(":");
                        var time = new Date(firstdata.date).setHours(parseInt(timevalues[0]), parseInt(timevalues[1]), parseInt(timevalues[2]));

                        if (columninfo[0].created == null || columninfo[0].created < new Date(firstdata.created)) {
                            if (columninfo[0].created != null && columninfo[0].time != time) {
                                for (var i = 21; i > 0; i--) {
                                    for (var j = 0; j < 19; j++) {
                                        $scope.griddata[j][i] = $scope.griddata[j][i - 1]
                                    }
                                }
                            }

                            columninfo[columnindex].time = time;
                            columninfo[columnindex].created = new Date(firstdata.created);

                            var zone = 0;
                            var name = '';
                            var cellvalue = '';

                            for (var i = 0; i < arg.points.length; i++) {
                                var rawlocation = arg.points[i].location;
                                var splitterindex = rawlocation.indexOf(':');
                                if (splitterindex >= 0) {
                                    zone = parseInt(rawlocation.substring(0, splitterindex));
                                    name = rawlocation.substring(splitterindex + 1, rawlocation.length);
                                    cellvalue = name + ':' + arg.points[i].value;
                                }
                                else {
                                    zone = parseInt(rawlocation);
                                    cellvalue = ':' + arg.points[i].value;
                                }

                                $scope.griddata[i][columnindex + 1] = cellvalue;
                            }

                            $("#dispatchGrid").jqxGrid('updatebounddata');
                        }
                    }
                }
                catch (err) {
                    console.log(err.toString());
                }
            });
        }
        catch (err) {
            console.log(err.toString());
        }
    };
}]);

function getbackcolor(value) {
    var backcolor = '#ed0a01';
    if (value < -10) { backcolor = "#64019a" }
    else if (value < 0) { backcolor = '#12108d' }
    else if (value < 6) { backcolor = '#0201ff' }
    else if (value < 14) { backcolor = '#3533ff' }
    else if (value < 20) { backcolor = '#067dff' }
    else if (value < 26) { backcolor = '#0ea8ff' }
    else if (value < 30) { backcolor = '#11c8ff' }
    else if (value < 34) { backcolor = '#14ebff' }
    else if (value < 38) { backcolor = '#2bf5e7' }
    else if (value < 46) { backcolor = '#67f7a2' }
    else if (value < 50) { backcolor = '#87fb80' }
    else if (value < 56) { backcolor = '#a6fd60' }
    else if (value < 62) { backcolor = '#c3fe40' }
    else if (value < 68) { backcolor = '#e2ff19' }
    else if (value < 76) { backcolor = '#fbfe00' }
    else if (value < 82) { backcolor = '#f9ed01' }
    else if (value < 90) { backcolor = '#fcdb02' }
    else if (value < 100) { backcolor = '#f9c802' }
    else if (value < 115) { backcolor = '#f9b700' }
    else if (value < 125) { backcolor = '#f6a300' }
    else if (value < 150) { backcolor = '#f88f00' }
    else if (value < 200) { backcolor = '#f27f00' }
    else if (value < 250) { backcolor = '#f36c0a' }
    else if (value < 300) { backcolor = '#f05900' }
    else if (value < 400) { backcolor = '#ef4802' }
    else if (value < 500) { backcolor = '#f0350c' }
    else if (value < 600) { backcolor = '#ee2201' }
    else if (value < 800) { backcolor = '#ee1100' }

    return backcolor;
}

function getforecolor(value) {
    var forecolor = '#000000';
    if (value < 14 || value >= 600) { forecolor = '#ffffff' }

    return forecolor;
}

function finddstswitchdate(year, month) {
    // Set the starting date
    var baseDate = new Date(Date.UTC(year, month, 0, 0, 0, 0, 0));
    var changeDay = 0;
    var changeMinute = -1;
    var baseOffset = -1 * baseDate.getTimezoneOffset() / 60;
    var dstDate;

    for (day = 0; day < 50; day++) {
        var tmpDate = new Date(Date.UTC(year, month, day, 0, 0, 0, 0));
        var tmpOffset = -1 * tmpDate.getTimezoneOffset() / 60;

        if (tmpOffset != baseOffset) {
            var minutes = 0;
            changeDay = day;

            tmpDate = new Date(Date.UTC(year, month, day - 1, 0, 0, 0, 0));
            tmpOffset = -1 * tmpDate.getTimezoneOffset() / 60;

            while (changeMinute == -1) {
                tmpDate = new Date(Date.UTC(year, month, day - 1, 0, minutes, 0, 0));
                tmpOffset = -1 * tmpDate.getTimezoneOffset() / 60;

                if (tmpOffset != baseOffset) {
                    tmpOffset = new Date(Date.UTC(year, month, day - 1, 0, minutes - 1, 0, 0));
                    changeMinute = minutes;
                    break;
                }
                else {
                    minutes++;
                }
            }

            dstDate = tmpOffset.getMonth() + 1;

            if (dstDate < 10) dstDate = "0" + dstDate;
            dstDate += '/' + tmpOffset.getDate() + '/' + year + ' ';

            tmpDate = new Date(Date.UTC(year, month, day - 1, 0, minutes - 1, 0, 0));
            dstDate += tmpDate.toTimeString().split(' ')[0];
            return dstDate;
        }
    }
}

function getonpeakhours(market) {

}

function getformatteddate(date) {
    try {
        var day = date.getDate();
        var month = date.getMonth();
        var year = date.getFullYear();

        var datestr = '';
        if (month >= 10) {
            daystr = month;
        }
        else {
            daystr = '0' + month
        }

        if (day >= 10) {
            daystr = '/' + day;
        }
        else {
            daystr = '/0' + day;
        }

        daystr = '/' + year;

        return daystr;

    }
    catch (err) {
        err.toString();
    }
}

function getbackgroundcolor(value) {
    var backcolor = '#ed0a01';
    if (value < -10) { backcolor = "#64019a" }
    else if (value < 0) { backcolor = '#12108d' }
    else if (value < 6) { backcolor = '#0201ff' }
    else if (value < 14) { backcolor = '#3533ff' }
    else if (value < 20) { backcolor = '#067dff' }
    else if (value < 26) { backcolor = '#0ea8ff' }
    else if (value < 30) { backcolor = '#11c8ff' }
    else if (value < 34) { backcolor = '#14ebff' }
    else if (value < 38) { backcolor = '#2bf5e7' }
    else if (value < 46) { backcolor = '#67f7a2' }
    else if (value < 50) { backcolor = '#87fb80' }
    else if (value < 56) { backcolor = '#a6fd60' }
    else if (value < 62) { backcolor = '#c3fe40' }
    else if (value < 68) { backcolor = '#e2ff19' }
    else if (value < 76) { backcolor = '#fbfe00' }
    else if (value < 82) { backcolor = '#f9ed01' }
    else if (value < 90) { backcolor = '#fcdb02' }
    else if (value < 100) { backcolor = '#f9c802' }
    else if (value < 115) { backcolor = '#f9b700' }
    else if (value < 125) { backcolor = '#f6a300' }
    else if (value < 150) { backcolor = '#f88f00' }
    else if (value < 200) { backcolor = '#f27f00' }
    else if (value < 250) { backcolor = '#f36c0a' }
    else if (value < 300) { backcolor = '#f05900' }
    else if (value < 400) { backcolor = '#ef4802' }
    else if (value < 500) { backcolor = '#f0350c' }
    else if (value < 600) { backcolor = '#ee2201' }
    else if (value < 800) { backcolor = '#ee1100' }

    return backcolor;

}

function getforegroundcolor(value) {
    var forecolor = '#000000';
    if (value < 14 || value >= 600) { forecolor = '#ffffff' }

    return forecolor;
}

(function ($) {
    $.fn.menumaker = function (options) {
        var cssmenu = $(this), settings = $.extend({
            title: "Menu",
            format: "dropdown",
            sticky: false
        }, options);

        return this.each(function () {
            cssmenu.prepend('<div id="menu-button">' + settings.title + '</div>');
            $(this).find("#menu-button").on('click', function () {
                $(this).toggleClass('menu-opened');
                var mainmenu = $(this).next('ul');
                if (mainmenu.hasClass('open')) {
                    mainmenu.hide().removeClass('open');
                }
                else {
                    mainmenu.show().addClass('open');
                    if (settings.format === "dropdown") {
                        mainmenu.find('ul').show();
                    }
                }
            });

            cssmenu.find('li ul').parent().addClass('has-sub');
            multiTg = function () {
                cssmenu.find(".has-sub").prepend('<span class="submenu-button"></span>');
                cssmenu.find('.submenu-button').on('click', function () {
                    $(this).toggleClass('submenu-opened');
                    if ($(this).siblings('ul').hasClass('open')) {
                        $(this).siblings('ul').removeClass('open').hide();
                    }
                    else {
                        $(this).siblings('ul').addClass('open').show();
                    }
                });
            };

            if (settings.format === 'multitoggle') multiTg();
            else cssmenu.addClass('dropdown');

            if (settings.sticky === true) cssmenu.css('position', 'fixed');

            resizeFix = function () {
                if ($(window).width() > 768) {
                    cssmenu.find('ul').show();
                }

                if ($(window).width() <= 768) {
                    cssmenu.find('ul').hide().removeClass('open');
                }
            };
            resizeFix();
            return $(window).on('resize', resizeFix);

        });
    };
})(jQuery);

(function ($) {
    $(document).ready(function () {

        $("#cssmenu").menumaker({
            title: "Menu",
            format: "multitoggle"
        });

    });
})(jQuery);
