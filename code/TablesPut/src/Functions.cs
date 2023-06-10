using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Amazon.DynamoDBv2;
using TablesLibrary;
using Amazon.DynamoDBv2.DataModel;
using TablesLibrary.Models;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TablesPut;

public class Function
{

    public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var decodedToken = AuthorizationHelpers.DecodeJwtToken(request.Headers["authorization"]);

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDBContext = new DynamoDBContext(dynamoDbClient);

        var dataTable = JsonConvert.DeserializeObject<DataTable>(request.Body);

        if (decodedToken.Subject != dataTable.userid)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        await dynamoDBContext.SaveAsync(dataTable);

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }
}
