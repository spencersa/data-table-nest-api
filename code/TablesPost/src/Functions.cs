using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Amazon.DynamoDBv2;
using TablesLibrary;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using TablesLibrary.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TablesPost;

public class Function
{

    public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var decodedToken = AuthorizationHelpers.DecodeJwtToken(request.Headers["Authorization"]);

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDBContext = new DynamoDBContext(dynamoDbClient);

        var dataTable = new DataTable
        {
            id = Guid.NewGuid().ToString(),
            userid = decodedToken.Subject
        };

        await dynamoDBContext.SaveAsync(dataTable);

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.Created,
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }
}
