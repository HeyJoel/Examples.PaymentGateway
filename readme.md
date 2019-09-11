# Examples.PaymentGatway

## Summary

Build a payment gateway, an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.

1. A merchant should be able to process a payment through the payment gateway and receive either a successful or unsuccessful response 
2. A merchant should be able to retrieve the details of a previously made payment

## Technical Requirements

- .NET Core 2.2
- Tested on Visual Studio 16.2.3

## Endpoints

### Payments

#### Add Payment

POST: /api/payments 

Content:

```json
{
	"currency": "GBP",
	"amount": 34.99,
	"creditCard": {
		"cardNumber": "4111111111111111",
		"nameOnCard": "Joel Mitchell",
		"ccv": "123",
		"expiryMonth": 2,
		"expiryYear": 2021
	}
}
```
Response:

```json
{
    "paymentId": 3,
    "paymentResult": "Paid"
}
```

#### Get Payment

GET: /api/payments/{id}

Reponse:

```json
{
    "paymentId": 3,
    "merchantId": 1,
    "bankPaymentId": "3",
    "currency": "GBP",
    "amount": 34.99,
    "maskedCreditCardNumber": "XXXXXXXXXXXX1111",
    "status": "Paid",
    "requestedDate": "2019-09-10T21:41:45.1910236Z",
    "responseReceivedDate": "2019-09-10T21:41:45.1999167Z"
}
```