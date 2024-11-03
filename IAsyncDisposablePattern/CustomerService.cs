using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace IAsyncDisposablePattern
{
    public class CustomerService(string connectionString)
    {

        public string ConnectionString { get; } = connectionString;

        public async Task<IList<Customer>> GetCustomersAsync(string title, SqlConnection connection = default, SqlTransaction transaction = default, CancellationToken cancellationToken = default)
        {
            // Open Connection, Read database and get customers
            using var runner = await SqlConnectionRunner.CreateRunnerAsync(connection, transaction, cancellationToken).ConfigureAwait(false);

            await using var _ = runner.ConfigureAwait(false);

            using var command = runner.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Customer where Title=@title";
            command.Parameters.AddWithValue("@title", title);

            var lstCustomer = new List<Customer>();
            var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                var newCustomer = new Customer
                {
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Title = reader.GetString("Title"),
                    Email = reader.GetString("EmailAddress")
                };

                lstCustomer.Add(newCustomer);
            }

            return lstCustomer;

        }
    }
}
