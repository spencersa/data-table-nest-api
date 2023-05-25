using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

namespace TablesGet.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestToUpperFunctionAsync()
    {
        var function = new Function();
        var apiGatewayProxyRequest = new APIGatewayProxyRequest();
        var context = new TestLambdaContext();
        var result = await function.FunctionHandlerAsync(apiGatewayProxyRequest, context);
    }
}
