﻿trackTruck.controller('signInController', [
    '$scope',
    '$location',
    '$window',
    'authService',
    'url',
    'globalMessage',
    signInController
]);

function signInController($scope, $location, $window, authService, url, globalMessage) {
    $scope.model = {};

    $scope.signIn = function () {
        $scope.response = null;
        globalMessage.info('checking...');
        authService.signIn($scope.model).then(function (response) {
            $scope.message = null;
            $scope.response = response;

            if (!response.IsError) {
                globalMessage.success(response.Message);
                $window.location.href = url.resolve();
            } else {
                globalMessage.error(response.Message);
            }
        });
    };
}
