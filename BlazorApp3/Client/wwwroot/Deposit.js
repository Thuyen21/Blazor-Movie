function Deposit(dotNetHelper, clientToken, cash) {
    
    var form = document.getElementById('payment-form');
    braintree.dropin.create({
        authorization: clientToken,
        container: '#dropin-container'
    }).then((dropinInstance) => {
        form.addEventListener('submit',
            (event) => {
                event.preventDefault();
                dropinInstance.requestPaymentMethod().then((payload) => {
                    //document.getElementById('nonce').value = payload.nonce;
                    //document.getElementById('cash').value = cash;
                    //form.action = 'Customer/DoCard';
                    //form.submit();

                    
                    return dotNetHelper.invokeMethodAsync('Test', String(payload.nonce), String(cash), 'DoCard');

                }).catch((error) => { throw error; });
            });
    }).catch((error) => {
    });
    braintree.client.create({
        authorization: clientToken
    }).then(function (clientInstance) {
        // Create a PayPal Checkout component.
        return braintree.paypalCheckout.create({
            client: clientInstance
        });
    }).then(function (paypalCheckoutInstance) {
        return paypalCheckoutInstance.loadPayPalSDK({
            currency: 'USD',
            intent: 'capture'
        });
    }).then(function (paypalCheckoutInstance) {
        return paypal.Buttons({
            fundingSource: paypal.FUNDING.PAYPAL,
            createOrder: function () {
                return paypalCheckoutInstance.createPayment({
                    flow: 'checkout', // Required
                    amount: cash, // Required
                    currency: 'USD', // Required, must match the currency passed in with loadPayPalSDK
                    intent: 'capture' // Must match the intent passed in with loadPayPalSDK
                });
            },
            onApprove: function (data, actions) {
                return paypalCheckoutInstance.tokenizePayment(data).then(function (payload) {
                    // Submit `payload.nonce` to your server
                    //document.getElementById('nonce').value = payload.nonce;
                    //document.getElementById('cash').value = cash;
                    //form.action = 'Customer/DoPayPal';
                    //form.submit();

                    return dotNetHelper.invokeMethodAsync('Test', String(payload.nonce), String(cash), 'DoCard');
                });
            },
            onCancel: function (data) {
                console.log('PayPal payment cancelled', JSON.stringify(data, 0, 2));
            },
            onError: function (err) {
                console.error('PayPal error', err);
            }
        }).render('#paypal-button');
    }).then(function () {
        // The PayPal button will be rendered in an html element with the ID
        // `paypal-button`. This function will be called when the PayPal button
        // is set up and ready to be used
    });
}
