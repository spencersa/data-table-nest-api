// See https://aka.ms/new-console-template for more information

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using TablesGet;

var function = new Function();
var apiGatewayProxyRequest = new APIGatewayProxyRequest();
var context = new TestLambdaContext();

apiGatewayProxyRequest.Headers = new Dictionary<string, string>
{
    ["authorization"] = ""
};

var result = await function.FunctionHandlerAsync(apiGatewayProxyRequest, context);
