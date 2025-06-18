# Simple saga example

<br/>

We have a Hotel Booking Service and a Ticket Booking Service. For the sake of the example we assume those are calling external services.

<br/>

We assume external services are idempotent and can deal with indentical requests.

<br/>

The goal is to book both ticket and hotel in one saga and compensate for a potential failure while taking in account that on our side failure can happen at any time and we need to compensate that.

<br/>

For the sake of the example both services will randomly fail and saga will run until they both successfully execute.
