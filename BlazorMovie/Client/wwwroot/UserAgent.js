async function getUserAgent() {
    //return String(navigator.userAgent + data.ip);
    return fetch('https://jsonip.com', { mode: 'cors' })
        .then((resp) => resp.json())
        .then((ip) => {
            console.log(ip)
            return String(navigator.userAgent + ' And IP: ' + ip.ip)
        });
}