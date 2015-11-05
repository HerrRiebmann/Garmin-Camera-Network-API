using System.Windows;
using VirbControl.Connection;

namespace VirbControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Network network;
        public MainWindow()
        {
            InitializeComponent();
            network = new Network();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            network.StartSearch();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!Data.Request.Submit(network.ServiceList[0].Addresses[0], Data.Commands.status))
                MessageBox.Show(Data.Request.LastErrorMessage);
        }
    }
}
