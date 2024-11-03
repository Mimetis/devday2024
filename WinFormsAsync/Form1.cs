namespace WinFormsAsync
{
    public partial class Form1 : Form
    {
        public const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AdventureWorks; Integrated Security=true;MultipleActiveResultSets=False;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var button = (Button)sender;

                var sc = SynchronizationContext.Current;

                ThreadPool.QueueUserWorkItem((state) =>
                {
                    var customerService = new CustomerService(ConnectionString);
                    var customers = customerService.GetCustomers("Mr.");

                    //  using Invoke
                    lstCustomers.Invoke(() =>
                    {
                        lstCustomers.DataSource = customers;
                        lstCustomers.DisplayMember = "FirstName";
                    });

                    //// using Synchronization Context
                    //sc.Post(delegate
                    //{
                    //    lstCustomers.DataSource = customers;
                    //    lstCustomers.DisplayMember = "FirstName";
                    //}, null);

                });

                //Debug.WriteLine(Environment.CurrentManagedThreadId); // main thread

                //var customerService = new CustomerService(ConnectionString);
                //var customers = await customerService.GetCustomersAsync("Mr.").ConfigureAwait(false);
                //Debug.WriteLine(Environment.CurrentManagedThreadId); // main thread

                //lstCustomers.DataSource = customers;
                //lstCustomers.DisplayMember = "FirstName";

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
