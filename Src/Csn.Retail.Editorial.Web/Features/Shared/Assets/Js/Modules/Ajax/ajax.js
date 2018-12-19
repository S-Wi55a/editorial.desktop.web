function get(url, success = (resp) => { }, error = ()=>{}) {

        if (url === null || !url.length) {throw new Error('Invalid URL')}

        var request = new XMLHttpRequest();
        request.open('GET', url, true);

        request.onload = function() {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                var resp = request.responseText;
                success(resp)

            } else {
                // We reached our target server, but it returned an error
                error()
            }
        };

        request.onerror = function() {
            // There was a connection error of some sort
            error()
        };

        request.send();



        //$.get("example.php", function (resp) {
        //        success(resp)
        //    })
        //    .fail(function () {
        //        error()
        //    });
}

function promisedGet(url) {
    return new Promise((resolve, reject) => {
        let request = new XMLHttpRequest();
        request.open("GET", url);
        
        request.onload = () => {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                resolve(request.responseText);
            } else {
                // Error
                reject(request.statusText);
            }
        };

        request.onerror = () => reject(request.statusText);
        request.send();
    });
}


export { get, promisedGet }




