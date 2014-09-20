jsonServieClient = (function ($) {

    var GET = 'Get',
        POST = 'Post',
        PUT = 'Put',
        DELETE = 'Delete',
        GET_ONE_WAY = 'GetOneWay',
        POST_ONE_WAY = 'PostOneWay',
        PUT_ONE_WAY = 'PutOneWay',
        DELETE_ONE_WAY = 'DeleteOneWay';

    function Client(url) {
        checkAjax();
        var serviceUrl = url;

        function checkAjax() {
            if ($ && $.ajax) {
                return;
            }
            throw "no ajax provider found";
        }

        function deleteAction(request) {
            return deleteJson(request, DELETE);
        }

        function deleteOneWay(request) {
            return deleteJson(request, DELETE_ONE_WAY);
        }

        function get(request) {
            return getJson(request, GET);
        }

        function getOneWay(request) {
            return getJson(request, GET_ONE_WAY);
        }

        function post(request) {
            return postJson(request, POST);
        }

        function postOneWay(request) {
            return postJson(request, POST_ONE_WAY);
        }

        function put(request) {
            return putJson(request, PUT);
        }

        function putOneWay(request) {
            return putJson(request, PUT_ONE_WAY);
        }

        function getJson(request, methodName) {
            return $.ajax({
                url: createBaseUrl(methodName, request.type),
                type: "GET",
                data: stringify(request.data),
                dataType: "json",
            });
        }

        function postJson(request, methodName) {
            return $.ajax({
                url: createBaseUrl(methodName, request.type),
                type: "POST",
                data: stringify(request.data),
                dataType: "json",
            });
        }

        function putJson(request, methodName) {
            return $.ajax({
                url: createBaseUrl(methodName, request.type),
                type: "PUT",
                data: stringify(request.data),
                dataType: "json",
            });
        }

        function deleteJson(request, methodName) {
            return $.ajax({
                url: createBaseUrl(methodName, request.type),
                type: "DELETE",
                data: stringify(request.data),
                dataType: "json",
            });
        }

        function createBaseUrl(methodName, type) {
            if (!type) {
                throw "request have to contain type field";
            }
            return serviceUrl + '/' + methodName + '?' + 'type' + '=' + type;
        }

        function stringify(json) {
            // http://www.west-wind.com/weblog/posts/2009/Sep/15/Making-jQuery-calls-to-WCFASMX-with-a-ServiceProxy-Client
            var reIso = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
            /// <summary>
            ///     Wcf specific stringify that encodes dates in the
            ///     a WCF compatible format ("/Date(9991231231)/")
            ///     Note: this format works ONLY with WCF.
            ///     ASMX can use ISO dates as of .NET 3.5 SP1
            /// </summary>
            /// <param name="key" type="var">property name</param>
            /// <param name="value" type="var">value of the property</param>
            return JSON.stringify(json, function (key, value) {
                if (typeof value == "string") {
                    var a = reIso.exec(value);
                    if (a) {
                        var val = '/Date(' + new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6])).getTime() + ')/';
                        this[key] = val;
                        return val;
                    }
                }
                return value;
            });
        }

        return {
            /// <summary>
            ///     Send data with Delete http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            delete: deleteAction,

            /// <summary>
            ///     Send data with Delete http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            deleteOneWay: deleteOneWay,

            /// <summary>
            ///     Send data with Get http command.
            /// </summary>
            /// <param name="data" type="var">data</param>
            get: get,

            /// <summary>
            ///     Send data with Get http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            getOneWay: getOneWay,

            /// <summary>
            ///     Send data with Post http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            post: post,

            /// <summary>
            ///     Send data with Post http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            postOneWay: postOneWay,

            /// <summary>
            ///     Send data with Put http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            put: put,

            /// <summary>
            ///     Send data with Put http command.
            /// </summary>
            /// <param name="request" type="var">Request example {type: 'deleteUser', data: {id: 1}}.
            /// Type is required, data is optional</param>
            putOneWay: putOneWay
        };
    }

    return {
        /// <summary>
        ///     Create new instance JsonServieClient.
        /// </summary>
        /// <param name="serviceUrl" type="var">Service Url.</param>
        create: function (serviceUrl) {
            return new Client(serviceUrl);
        }
    };

})(jQuery);