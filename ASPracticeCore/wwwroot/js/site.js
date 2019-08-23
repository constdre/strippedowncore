
function getResource(url) {
    return new Promise((resolve, reject) => {
        var request = new XMLHttpRequest();
        request.open("get", url, true);
        request.onload = () => {
            var responseData = JSON.parse(request.response);
            console.log(responseData);
            resolve(responseData);
        };
        request.onerror = function () {
            console.log(request.statusText);
            reject(request.statusText);
        };
        request.send();
    });
}

