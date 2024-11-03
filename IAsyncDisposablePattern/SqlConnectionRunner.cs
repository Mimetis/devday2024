using Microsoft.Data.SqlClient;
using System.Data;

namespace IAsyncDisposablePattern
{
    /// <summary>
    /// Disposable runner to encapsulate a connection and a transaction.
    /// </summary>
    public sealed class SqlConnectionRunner : IDisposable, IAsyncDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionRunner"/> class.
        /// </summary>
        public SqlConnectionRunner(SqlConnection connection, SqlTransaction transaction = default, bool alreadyOpened = false, bool alreadyInTransaction = false, CancellationToken cancellationToken = default)
        {
            this.Connection = connection;
            this.Transaction = transaction;
            this.AlreadyOpened = alreadyOpened;
            this.AlreadyInTransaction = alreadyInTransaction;
            this.CancellationToken = cancellationToken;
        }


        /// <summary>
        /// Returns a new instance of SqlConnectionRunner.
        /// </summary>
        public static async Task<SqlConnectionRunner> CreateRunnerAsync(SqlConnection connection = default, SqlTransaction transaction = default, CancellationToken cancellationToken = default)
        {
            var alreadyOpened = connection != null && connection.State == ConnectionState.Open;
            var alreadyInTransaction = transaction != null && transaction.Connection == connection;

            if (connection == default)
                connection = new SqlConnection(Program.ConnectionString);

            // Open connection
            if (!alreadyOpened)
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            // Create a transaction
            if (!alreadyInTransaction)
                transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false) as SqlTransaction;

            return new SqlConnectionRunner(connection, transaction, alreadyOpened, alreadyInTransaction);
        }


        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public SqlConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        public SqlTransaction Transaction { get; set; }

        /// <summary>
        /// Gets a value indicating whether the connection is already opened.
        /// </summary>
        public bool AlreadyOpened { get; }

        /// <summary>
        /// Gets a value indicating whether the transaction is already opened.
        /// </summary>
        public bool AlreadyInTransaction { get; }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Commit the transaction and call an interceptor.
        /// </summary>
        public async Task CommitAsync(bool autoClose = true)
        {

            if (!this.AlreadyInTransaction && this.Transaction != null)
                this.Transaction.Commit();

            if (autoClose)
                await this.CloseAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Commit the transaction and call an interceptor.
        /// </summary>
        public async Task CloseAsync()
        {
            if (!this.AlreadyOpened && this.Connection != null)
                await this.Connection.CloseAsync();
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        public Task RollbackAsync(string reason) => Task.Run(() =>
        {
            if (this.Transaction == null || this.AlreadyInTransaction)
                return;

            try
            {
                this.Transaction.Rollback();
                return;
            }
            catch (Exception)
            {
            }
        });

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the current transaction and connection.
        /// </summary>
        public void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {

                    if (!this.AlreadyInTransaction && this.Transaction != null)
                    {
                        this.Transaction.Dispose();
                        this.Transaction = null;
                    }

                    if (!this.AlreadyOpened && this.Connection != null)
                    {
                        if (this.Connection.State == ConnectionState.Open)
                            this.Connection.Close();

                        this.Connection.Dispose();
                        this.Connection = null;
                    }

                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Async dispose, when using "await using var runner = await this.GetConnectionAsync()".
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (!this.AlreadyInTransaction && this.Transaction != null)
            {
                await this.Transaction.DisposeAsync().ConfigureAwait(false);
                this.Transaction = null;
            }

            if (!this.AlreadyOpened && this.Connection != null)
            {
                await this.Connection.CloseAsync();
                await this.Connection.DisposeAsync().ConfigureAwait(false);
                this.Connection = null;
            }
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
