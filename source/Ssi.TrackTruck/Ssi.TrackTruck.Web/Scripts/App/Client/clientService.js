﻿trackTruck.factory('clientService', [
    'repository',
    clientService
]);

function clientService(repository) {
    return {
        getAll: function() {
            return repository.get('Client', 'All');
        }
    };
}