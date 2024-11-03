using System.Windows;
using System.Windows.Controls;

namespace WpfAppAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AdventureWorks; Integrated Security=true;MultipleActiveResultSets=False;";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var button = (Button)sender;

                var sc = SynchronizationContext.Current;


                ThreadPool.QueueUserWorkItem((state) =>
                {
                    var customerService = new CustomerService(ConnectionString);
                    var customers = customerService.GetCustomers("Mr.");

                    // using Synchronization Context
                    sc.Post(delegate
                    {
                        lstCustomers.ItemsSource = customers;
                        lstCustomers.DisplayMemberPath = "FirstName";
                    }, null);

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