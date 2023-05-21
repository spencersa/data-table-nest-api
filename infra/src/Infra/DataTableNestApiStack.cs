using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Apigatewayv2.Alpha;
using Amazon.CDK.AWS.Apigatewayv2.Integrations.Alpha;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.Alpha.HttpMethod;
using Amazon.CDK.AWS.Apigatewayv2.Authorizers.Alpha;

namespace Infra
{
    public class DataTableNestApiStack : Stack
    {
        internal DataTableNestApiStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var issuer = "https://dev-77r3tluzofdan1kf.us.auth0.com/";
            var authorizer = new HttpJwtAuthorizer("JwtAuthorizer", issuer, new HttpJwtAuthorizerProps
            {
                JwtAudience = new[] { "https://data-table-nest-api" }
            });

            var api = new HttpApi(this, "data-table-nest-api");

            var tablesGetLambda = new Function(this, "TablesGet", new FunctionProps
            {
                Code = Code.FromAsset("../code/TablesGet/src/bin/Release/net6.0/src.zip"),
                Handler = "TablesGet::TablesGet.Function::FunctionHandlerAsync",
                Runtime = Runtime.DOTNET_6,
                Timeout = Duration.Seconds(30),        
            });

            var tablesGetLambdaIntegration = new HttpLambdaIntegration("TablesGetLambdaIntegration", tablesGetLambda);

            api.AddRoutes(new AddRoutesOptions
            {
                Path = "/tables",
                Methods = new[] { HttpMethod.GET },
                Integration = tablesGetLambdaIntegration,
                Authorizer = authorizer
            });

            var table = new Table(this, "tablesTable", new TableProps
            {
                TableName = "tables",
                PartitionKey = new Attribute
                {
                    Name = "userid",
                    Type = AttributeType.STRING
                },
                SortKey = new Attribute
                {
                    Name = "unixtimestamp",
                    Type = AttributeType.NUMBER
                },
                //TODO: REMOVE THIS AFTER TESTING
                DeletionProtection = false,
                //TODO: REMOVE THIS AFTER TESTING
                RemovalPolicy = RemovalPolicy.DESTROY
            });

            var readScaling = table.AutoScaleReadCapacity(new EnableScalingProps { MinCapacity = 1, MaxCapacity = 50 });
            readScaling.ScaleOnUtilization(new UtilizationScalingProps { TargetUtilizationPercent = 75 });

            var writeAutoScaling = table.AutoScaleWriteCapacity(new EnableScalingProps { MinCapacity = 1, MaxCapacity = 50 });
            writeAutoScaling.ScaleOnUtilization(new UtilizationScalingProps { TargetUtilizationPercent = 75 });

            table.GrantReadData(tablesGetLambda);
        }
    }
}
