using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace Example_Cosmos_DB_Table_API
{
    class Program
    {
        public static string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=[AccountName];AccountKey=[AccountKey]";

        static void Main(string[] args)
        {
            Run().Wait();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static async Task Run()
        {
            // Create a new table
            CloudTable table = await CreateTableAsync("Customers");

            try
            {
                // Create a new record in the table
                CustomerEntity customer = new CustomerEntity("Angelos", "Petropoulos");
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(customer);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                customer = result.Result as CustomerEntity;

                // Delete the newly created record
                TableOperation deleteOperation = TableOperation.Delete(customer);
                await table.ExecuteAsync(deleteOperation);

            }
            finally
            {
                // Delete the table itself
                await table.DeleteIfExistsAsync();
            }
        }

        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
