using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace WpfAppAsync
{
    public class CustomerService(string connectionString)
    {
        public string ConnectionString { get; } = connectionString;

        public async Task<IList<Customer>> GetCustomersAsync(string title)
        {
            await Task.Delay(5000);
            // Open Connection, Read database and get customers
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            using var command = connection.CreateCommand();
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

        public IList<Customer> GetCustomers(string title)
        {

            Thread.Sleep(5000);
            // Open Connection, Read database and get customers
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Customer where Title=@title";
            command.Parameters.AddWithValue("@title", title);
            var lstCustomer = new List<Customer>();
            var reader = command.ExecuteReader();
            while (reader.Read())
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
