The solution contains 3 projects:

1. Siils.Api - main WebApi application containing the endpoint that the platform should post to:
http://localhost/Siils.Api/api/forex

2. Siils.Tests - integration tests.

3. SLYEx.API.Dummy - web project which emulates platform behaviour for testing purpose. Can be enabled by uncommenting:

            //Task.Factory.StartNew(() =>
and following lines in global.asax.

Only thing needed is to deploy Siils.Api web application and start sending post requests to the endpoint!
