/**
 * xhr.js
 * (c) 2013 Adam Boxall, http://adamboxall.com
 * https://github.com/AdamBoxall/xhr.js
 * license MIT
 */
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {
        define([], factory);
    } else {
        root.xhr = factory();
    }

})(this, function() {

    function createXhr(callback) {

        if (callback && typeof callback !== 'function') {
            throw new Error('Callback is not callable');
        }

        var xhr;
        if (window.XMLHttpRequest) {
            xhr = new XMLHttpRequest();
        } else {
            xhr = new ActiveXObject("Microsoft.XMLHTTP");
        }

        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

        if (callback) {
            xhr.onreadystatechange = function() {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        var contentType = xhr.getResponseHeader('Content-Type');
                        if (contentType === 'application/json') {
                            try {
                                callback(null, JSON.parse(xhr.responseText), xhr);
                            } catch (e) {
                                callback(e.message, null, xhr);
                            }
                        } else {
                            callback(null, xhr.responseText, xhr);
                        }
                    } else {
                        callback(xhr.statusText, null, xhr);
                    }
                }
            }
        }

        return xhr;
    }

    return {

        get: function(uri, callback) {

            this.request({uri: uri, callback: callback});
        },

        post: function(uri, data, callback) {

            if (typeof data === 'function') {
                callback = data;
                data = null;
            }

            this.request({
                uri: uri,
                method: 'POST',
                data: data,
                callback: callback,
            });
        },

        request: function(options) {

            if (!options.uri) {
                throw new Error('No URI has been provided');
            }

            var data = options.data || null;
            var method = options.method || 'GET';
            var callback = options.callback || null;

            if (['GET', 'POST', 'PUT', 'DELETE'].indexOf(method) === -1) {
                throw new Error('Invalid request method: ' + method);
            }

            var xhr = createXhr(callback);
            xhr.open(method, options.uri, true);

            if (data) {
                xhr.setRequestHeader('Content-Type', (typeof data === 'string')
                    ? 'application/json'
                    : 'application/x-www-form-urlencoded'
                );
            }

            xhr.send(data);
        }

    };

});