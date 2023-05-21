using Amazon.DynamoDBv2.DataModel;

namespace TablesLibrary.Models
{
    [DynamoDBTable("tables")]
    public class Table
    {
        [DynamoDBHashKey]
        public string userid { get; set; }
        [DynamoDBRangeKey]
        public int unixtimestamp { get; set; }
    }
}
