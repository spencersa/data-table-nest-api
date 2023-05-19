using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;

namespace Infra
{
    public class DataTableNestApiStack : Stack
    {
        internal DataTableNestApiStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var api = new RestApi(this, "data-table-nest-api");
            
            var tablesGetLambda = new Function(this, "TablesGet", new FunctionProps {
                Code = Code.FromAsset("../code/TablesGet/src/bin/Release/net6.0/src.zip"),
                Handler = "TablesGet::TablesGet.Function::FunctionHandler",
                Runtime = Runtime.DOTNET_6
            });

            var tables = api.Root.AddResource("tables");
            tables.AddMethod("GET", new LambdaIntegration(tablesGetLambda));

        }
    }
}
