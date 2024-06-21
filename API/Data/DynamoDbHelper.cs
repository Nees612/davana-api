using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using API.Data.DynamoDbTablesCreateRequests;

namespace API.Data
{
    public class DynamoDbHelper : IDisposable
    {
        private static readonly AmazonDynamoDBClient _client = new();

        public static void CreateSchema()
        {
            CreateTable(CoachTableDefinition.Request, CoachTableDefinition.TableName);
            CreateTable(UserTableRequest.Request, UserTableRequest.TableName);
            CreateTable(AppointmentTableRequest.Request, AppointmentTableRequest.TableName);
        }
        private static async void CreateTable(CreateTableRequest createTableRequest, string tableName)
        {
            _ = createTableRequest ?? throw new ArgumentException("Parameter cannot be null", nameof(createTableRequest));
            _ = tableName ?? throw new ArgumentException("Parameter cannot be null", nameof(tableName));

            Console.WriteLine("\n*** Creating table ***");

            var response = await _client.CreateTableAsync(createTableRequest);

            var tableDescription = response.TableDescription;
            Console.WriteLine("{1}: {0} \t ReadsPerSec: {2} \t WritesPerSec: {3}",
                      tableDescription.TableStatus,
                      tableDescription.TableName,
                      tableDescription.ProvisionedThroughput.ReadCapacityUnits,
                      tableDescription.ProvisionedThroughput.WriteCapacityUnits);

            string status = tableDescription.TableStatus;
            Console.WriteLine(tableName + " - " + status);

            WaitUntilTableReady(tableName);
        }

        public static async void ListTables()
        {
            Console.WriteLine("\n*** listing tables ***");
            string? lastTableNameEvaluated = null;
            do
            {
                var request = new ListTablesRequest
                {
                    Limit = 2,
                    ExclusiveStartTableName = lastTableNameEvaluated
                };

                var response = await _client.ListTablesAsync(request);
                foreach (string name in response.TableNames)
                    Console.WriteLine(name);

                lastTableNameEvaluated = response.LastEvaluatedTableName;
            } while (lastTableNameEvaluated != null);
        }

        private static async void GetTableInformation(string tableName)
        {
            _ = tableName ?? throw new ArgumentException("Parameter cannot be null", nameof(tableName));

            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = tableName
            };

            var response = await _client.DescribeTableAsync(request);

            TableDescription description = response.Table;
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                      description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                      description.ProvisionedThroughput.WriteCapacityUnits);
        }

        private static async void UpdateTable(string tableName)
        {
            _ = tableName ?? throw new ArgumentException("Parameter cannot be null", nameof(tableName));

            Console.WriteLine("\n*** Updating table ***");
            var request = new UpdateTableRequest()
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput()
                {
                    ReadCapacityUnits = 6,
                    WriteCapacityUnits = 7
                }
            };

            var response = await _client.UpdateTableAsync(request);

            WaitUntilTableReady(tableName);
        }

        private static async void DeleteTable(string tableName)
        {
            _ = tableName ?? throw new ArgumentException("Parameter cannot be null", nameof(tableName));

            Console.WriteLine("\n*** Deleting table ***");
            var request = new DeleteTableRequest
            {
                TableName = tableName
            };

            var response = await _client.DeleteTableAsync(request);

            Console.WriteLine("Table is being deleted...");
        }

        private static async void WaitUntilTableReady(string tableName)
        {
            string? status = null;
            // Let us wait until table is created. Call DescribeTable.
            do
            {
                System.Threading.Thread.Sleep(5000); // Wait 5 seconds.
                try
                {
                    var res = await _client.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    Console.WriteLine("Table name: {0}, status: {1}",
                              res.Table.TableName,
                              res.Table.TableStatus);
                    status = res.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    // DescribeTable is eventually consistent. So you might
                    // get resource not found. So we handle the potential exception.
                }
            } while (status != "ACTIVE");
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}