using NapoletanBot.Net.Environment.Code;
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

namespace NapoletanBot.Net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DebugConsole.AppendText("StartButton\n");
            var botInstance = new TelegramBot(TokenTextBox.Text, this);
        }
        public static Environment.Window.Settings settings;
        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settings = new Environment.Window.Settings();
            settings.Show();
        }
    }
}
