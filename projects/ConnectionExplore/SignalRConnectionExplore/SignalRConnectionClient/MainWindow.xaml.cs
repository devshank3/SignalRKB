using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRConnectionClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text;

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
                }).ConfigureLogging(
                logging =>
                {
                    logging.AddDebug();
                })
                .Build();

            try
            {
                await connection.StartAsync();
                MessageBox.Show("Connected to the server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }
    }
}
