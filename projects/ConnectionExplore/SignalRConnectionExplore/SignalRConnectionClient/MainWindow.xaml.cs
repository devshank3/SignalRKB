using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SignalRConnectionClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            UrlTextBox.Text = "https://localhost:7128/Chat";
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text;

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .WithAutomaticReconnect()
                .Build();

            RegisterConnectionEvents();

            await StartConnection();
        }

        private void RegisterConnectionEvents()
        {
            _connection.Closed += (error) => 
            {
                UpdateConnectionStatus(Brushes.Red);
                return Task.CompletedTask;
            };

            _connection.Reconnecting += (error) =>
            {
                UpdateConnectionStatus(Brushes.Yellow);
                return Task.CompletedTask;
            };

            _connection.Reconnected += (connectionId) =>
            {
                UpdateConnectionStatus(Brushes.Green);
                return Task.CompletedTask;
            };
        }
        private async Task StartConnection()
        {
            try
            {
                await _connection.StartAsync();
                connectButton.IsEnabled = false;
                disconnectButton.IsEnabled = true;
                UpdateConnectionStatus(Brushes.Green);
                MessageBox.Show("Connected to the server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private async void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection != null && (_connection.State == HubConnectionState.Connected || _connection.State == HubConnectionState.Reconnecting))
            {
                await _connection.StopAsync();
                connectButton.IsEnabled = true;
                disconnectButton.IsEnabled = false;
                UpdateConnectionStatus(Brushes.Red);
                MessageBox.Show("Disconnected from the server.");
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.StopAsync();
            }
        }

        private void UpdateConnectionStatus(Brush color)
        {
            Dispatcher.Invoke(() => connectionStatusIndicator.Fill = color);
        }
    }
}
