using System.Windows;
using System.Windows.Media;
using NapoletanBot.Net.Environment.Code;
using static NapoletanBot.Net.Environment.Code.Settings.MsgTypes;

namespace NapoletanBot.Net.Environment.Window
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void AnimationsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AnimationAllowed)
                AnimationsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                AnimationsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            AnimationAllowed = !AnimationAllowed;
        }

        private void AudiosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AudioAllowed)
                AudiosBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                AudiosBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            AudioAllowed = !AudioAllowed;
        }

        private void ContactsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContactAllowed)
                ContactsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                ContactsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            ContactAllowed = !ContactAllowed;
        }

        private void DocumentsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentAllowed)
                DocumentsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                DocumentsBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            DocumentAllowed = !DocumentAllowed;
        }

        private void GamesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameAllowed)
                GamesBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                GamesBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            GameAllowed = !GameAllowed;
        }

        private void TextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextAllowed)
                TextBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                TextBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            TextAllowed = !TextAllowed;
        }

        private void StickersBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StickerAllowed)
                StickerBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                StickerBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            StickerAllowed = !StickerAllowed;
        }

        private void MediaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaAllowed)
                MediaBtn.Background = (Brush)new BrushConverter().ConvertFrom("#E8561F");
            else
                MediaBtn.Background = (Brush)new BrushConverter().ConvertFrom("#16E601");
            MediaAllowed = !MediaAllowed;
        }
    }
}
