using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Authentication;

public class Function
{
    public ClaimsPrincipal? FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var token = request.Headers["Authorization"];
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = "https://dev-77r3tluzofdan1kf.us.auth0.com/",
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
            Console.WriteLine("Success");
            return claimsPrincipal;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}