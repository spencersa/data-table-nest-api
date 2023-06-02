using Amazon.DynamoDBv2.DataModel;

namespace TablesLibrary.Models
{
    [DynamoDBTable("data-tables")]
    public class DataTable
    {
        [DynamoDBHashKey]
        public string id { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string userid { get; set; }
        [DynamoDBProperty(typeof(DataConverter))]
        public List<object> values { get; set; }
    }
}