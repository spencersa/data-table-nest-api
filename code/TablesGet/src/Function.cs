using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Amazon.DynamoDBv2;
using Newtonsoft.Json;
using TablesLibrary;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using TablesLibrary.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TablesGet;

public class Function
{

    public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var decodedToken = AuthorizationHelpers.DecodeJwtToken(request.Headers["authorization"]);

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDBContext = new DynamoDBContext(dynamoDbClient);

        var search = dynamoDBContext.FromQueryAsync<DataTable>(new QueryOperationConfig()
        {
            IndexName = "userid",
            Filter = new QueryFilter("userid", QueryOperator.Equal, decodedToken.Subject)
        });

        var searchResponse = await search.GetRemainingAsync();

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonConvert.SerializeObject(searchResponse),
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }
}
