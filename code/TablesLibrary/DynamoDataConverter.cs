using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace TablesLibrary
{
    public class DataConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var entries = entry as DynamoDBList;
            var primitives = GetPrimitives(entries);
            return primitives;
        }

        private object GetPrimitives(DynamoDBList entries)
        {
            var reurnValue = new List<object>();
            foreach (DynamoDBEntry entryValue in entries.Entries)
            {
                var primitive = entryValue as Primitive;
                if (primitive != null)
                {
                    reurnValue.Add(new { value = primitive.Value });
                }
                else
                {
                    var nestedEntries = entryValue as DynamoDBList;
                    reurnValue.Add(new { values = GetPrimitives(nestedEntries) });
                }
            }
            return reurnValue;
        }


        public DynamoDBList CreateListEntry()
        {
            var entry = new DynamoDBList();
            return entry;
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var entry = new DynamoDBList();
            var valueList = value as IEnumerable<dynamic>;
            foreach (var rawValue in valueList)
            {
                if (rawValue.values != null)
                {
                    var subEntry = new DynamoDBList();
                    foreach (var subRawValue in rawValue.values)
                    {
                        subEntry.Add(Convert.ToString(subRawValue.value));
                    }
                    entry.Add(subEntry);
                }
                else
                {
                    entry.Add(Convert.ToString(rawValue.value));
                }
            }
            return entry;
        }
    }
}
