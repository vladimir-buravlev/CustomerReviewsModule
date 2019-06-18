angular.module('customerReviewsModule')
    .factory('customerReviewsModule.webApi', ['$resource', function ($resource) {
        return $resource('api/customerReviewsModule', {}, {
            search: { method: 'POST', url: 'api/customerReviewsModule/search' },
            update: { method: 'PUT' }
        });
}]);
