﻿var trackTruck = angular.module('trackTruck', ['ui.bootstrap', 'tableSort', 'ngRoute']);

trackTruck.config([
    '$locationProvider',
    'datepickerPopupConfig',
    'datepickerConfig',
    'timepickerConfig',
    '$routeProvider',
    function ($locationProvider, datepickerPopupConfig, datepickerConfig, timepickerConfig, $routeProvider) {
        $locationProvider.html5Mode(false);
        datepickerPopupConfig.datepickerPopup = 'MMMM dd, yyyy';
        datepickerConfig.showWeeks = false;
        datepickerPopupConfig.appendToBody = true;
        timepickerConfig.showSpinners = false;

        $routeProvider
            .when('/hello', {
                template: '<div>hello</div>'
            })
            .when('/truck/add', {
                template: '<add-truck></add-truck>'
            });
    }
]);

trackTruck.value('_', window._);
trackTruck.value('dateFormat', 'MMMM dd, yyyy');
trackTruck.value('designation', {
    driver: 'driver',
    helper: 'helper'
});
