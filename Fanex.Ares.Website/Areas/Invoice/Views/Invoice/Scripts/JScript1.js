// *******************************************************************
// Define services for the MyDemo app (requires `messaging` module in MessagingServices.js)
//
// NOTE: The `messaging` module is defined in MessagingServices.js; loaded as a managed resource
// on the left: http://www.gridlinked.info/angularJS/lib/MessagingServices.js
//
// *******************************************************************
// Build two (2) dispatcher/messaging routers: Channel1 and Channel2
angular.module('MessagingDemo', ['messaging'])
    .factory('channel1', ['EventDispatcher',
        function (EventDispatcher) {
            // Build specific EventDispatchers for topic: Channel1
            return EventDispatcher.getInstance("Channel1");
        }
    ])
    .factory('channel2', ['EventDispatcher',
        function (EventDispatcher) {
            // Build specific EventDispatchers for topic: Channel2
            return EventDispatcher.getInstance("Channel2");
        }
    ])
    .factory('pushService', ['EventDispatcher',
        function (EventDispatcher) {
            // Background timer
            var dispatcher = EventDispatcher.getInstance("myBackgroundService"),
                count = 1,
                timerID;
            // Timer process to announce service changes; defaults to every 500ms

            function runBackgroundProcess(interval) {
                if (!interval) interval = .5;
                timerID = setInterval(function () {
                    dispatcher.dispatch("Service update #" + count++);
                }, interval * 1000);
            }
            // Build specific EventDispatcher used exclusively with this background service.
            return {
                start: function (interval) {
                    if (!timerID) runBackgroundProcess(interval);
                    return dispatcher;
                },
                stop: function (interval) {
                    if (timerID) clearInterval(timerID);
                    timerID = 0;
                }
            };
        }
    ]);
// *******************************************************************
// Define Controllers: to prepare view models and inject behaviors
// *******************************************************************
/**
* Announcer - the MAIN controller
*/

function Announcer($scope, channel1, channel2) {
    // When the pub/sub announces changes, update our model data with the `msg`
    channel1.addListener(function (msg) {
        $scope.c0_1 = msg;
    });
    channel2.addListener(function (msg) {
        $scope.c0_2 = msg;
    });
    // When the user clicks a button, use the pub/sub to announce the input `msg` value
    $scope.updateChannel1 = function (msg) {
        channel1.dispatch(msg);
    };
    $scope.updateChannel2 = function (msg) {
        channel2.dispatch(msg);
    };
}
/**
* Controller 1 -
*/

function ControllerOne($scope, channel1, channel2) {
    channel1.addListener(function (msg) {
        $scope.c1_1 = '(1): ' + msg;
    });
    channel2.addListener(function (msg) {
        $scope.c1_2 = '(2): ' + msg;
    });
}
/**
* Controller 2 -
*/

function ControllerTwo($scope, channel1, channel2) {
    var onChannel1Change = function (msg) {
        $scope.ctrl2_channel1 = '(1): ' + msg;
    },
        onChannel2Change = function (msg) {
            $scope.ctrl2_channel2 = '(2): ' + msg;
        },
        buildToggle = function (channelID) {
            // Build button label and use messageQueue
            var action = "action" + channelID,
                handler = channelID == 1 ? onChannel1Change : onChannel2Change,
                dispatcher = channelID == 1 ? channel1 : channel2;
            return function () {
                if ($scope[action] == "Ignore Channel " + channelID) {
                    dispatcher.removeListener(handler);
                    $scope[action] = "Listen Channel " + channelID;
                } else {
                    dispatcher.addListener(handler);
                    $scope[action] = "Ignore Channel " + channelID;
                }
            };
        };
    $scope.action1 = ""; // button label
    $scope.toggleSubscribe1 = buildToggle(1); // click handler
    $scope.toggleSubscribe1(); // establish lable and queue connection
    $scope.action2 = "";
    $scope.toggleSubscribe2 = buildToggle(2)
    $scope.toggleSubscribe2();
}
/**
* Subscriber -
*/

function ServiceSubscriber($scope, pushService) {
    var dispatcher = null,
        onServiceMsgs = function (msg) {
            $scope.$apply(function () {
                $scope.message = msg;
            });
        };
    $scope.message = "";
    $scope.title = "Start Background Service";
    $scope.startStopService = function () {
        if (dispatcher) {
            $scope.title = "Start Background Service";
            pushService.stop();
            dispatcher.removeListener(onServiceMsgs);
            dispatcher = null;
        } else {
            $scope.title = "Stop Service";
            dispatcher = pushService.start();
            dispatcher.addListener(onServiceMsgs);
        }
    };
}