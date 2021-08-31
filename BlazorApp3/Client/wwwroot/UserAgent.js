async function getUserAgent() {
    let devices = await navigator.hid.getDevices();
    let s = null;
    devices.forEach(device => {
        s = s + device.productName;
    });
    navigator.storage.estimate().then(function (estimate) {
        s = estimate.quota;
    });
    //return String(navigator.userAgent + navigator.deviceMemory + navigator.maxTouchPoints + navigator.vendor + navigator.hardwareConcurrency + s);
    return String(navigator.appCodeName + navigator.appName + navigator.appVersion + navigator.cookieEnabled + navigator.language + navigator.onLine + navigator.platform + navigator.userAgent + navigator.deviceMemory + navigator.maxTouchPoints + navigator.vendor + navigator.hardwareConcurrency + s);
}