using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.DynamoDB;

namespace Infra
{
    public class DataTableNestApiStack : Stack
    {
        internal DataTableNestApiStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var api = new RestApi(this, "data-table-nest-api");

            var tablesGetLambda = new Function(this, "TablesGet", new FunctionProps
            {
                Code = Code.FromAsset("../code/TablesGet/src/bin/Release/net6.0/src.zip"),
                Handler = "TablesGet::TablesGet.Function::FunctionHandlerAsync",
                Runtime = Runtime.DOTNET_6,
                Timeout = Duration.Seconds(30)
            });

            var tables = api.Root.AddResource("tables");
            tables.AddMethod("GET", new LambdaIntegration(tablesGetLambda));

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
