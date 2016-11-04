using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string result;
        private string translation = "en-ru";

        public MainPage()
        {
            this.InitializeComponent();

        }

        public async Task getTranslation(string text)
        {
            String uri = "https://translate.yandex.net/api/v1.5/tr/translate?" +
                "lang=" + translation + "&key=trnsl.1.1.20161024T144122Z.7866028e157a6e7f.369cc952fed6bc6f5303f015c2b0a25415190ebb";
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
               { "text", text }
            };
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            string responseXML = await response.Content.ReadAsStringAsync();
            XDocument document = XDocument.Parse(responseXML);
            this.result = document.Root.Element("text").Value;
        }

        private void switchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (translation == "en-ru")
            {
                translation = "ru-en";
                leftLabel.Text = "Русский";
                rightLabel.Text = "English";
            }
            else
            {
                translation = "en-ru";
                leftLabel.Text = "English";
                rightLabel.Text = "Русский";
            }
        }

        private async void translateBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = leftBox.Text;
            await getTranslation(input);
            try
            {
                rightBox.Text = result;
            }
            catch (NullReferenceException)
            {
                rightBox.Text = "Error";
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
