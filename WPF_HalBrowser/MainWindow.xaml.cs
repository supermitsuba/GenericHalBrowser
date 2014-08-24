using GenericHalHelper;
using GenericHalHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace WPF_HalBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BaseAddress.Text = "http://fbombcode.com";
            RelativeAddress.Text = "api/articles";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var baseAddress = BaseAddress.Text;
                var relativeAddress = RelativeAddress.Text;
                using (HttpClient client = new HttpClient())
                {
                    var HalMediaType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.hal+json");
                    client.DefaultRequestHeaders.Accept.Add(HalMediaType);
                    client.BaseAddress = new Uri(baseAddress);
                    HalClient hal = new HalClient(client);
                    var obj = hal.Get(relativeAddress);

                    ClearControls();
                    CreateProperties(obj, Properties);
                    CreateLinks(obj, Links);
                    CreateEmbedded(obj, Embedded);
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "You should feel bad about this error", MessageBoxButton.OK);
            }
        }
        
        private void ClearControls()
        {
            Links.Children.Clear();
            Properties.Children.Clear();
            Embedded.Children.Clear();
        }

        private void CreateLinks(HalObject obj, StackPanel panel)
        {
            if (obj.Links != null && obj.Links.Count > 0)
            {
                var title = new TextBlock()
                {
                    Text = "Links",
                    FontSize = panel.Name == "Embedded" ? 14 : 16,
                    TextDecorations = System.Windows.TextDecorations.Underline
                };
                panel.Children.Add(title);
                foreach (var p in obj.Links.Keys)
                {
                    var label = new TextBlock()
                    {
                        Text = p
                    };
                    panel.Children.Add(label);
                    foreach(var l in obj.Links[p])
                    {
                        var label2 = new Button()
                        {
                            Content = l.Href,
                            Style = this.FindResource("LinkButton") as Style
                        };
                        label2.Click += ClickLinkEvent;
                        panel.Children.Add(label2);
                    }
                }
            }
        }

        private void ClickLinkEvent(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            RelativeAddress.Text = btn.Content.ToString();
        }
        
        private void CreateProperties(HalObject obj, StackPanel panel)
        {
            if (obj.DataProperties != null && obj.DataProperties.Count > 0)
            {
                var title = new TextBlock()
                {
                    Text = "Properties",
                    FontSize = panel.Name == "Embedded" ? 14 : 16,
                    TextDecorations = System.Windows.TextDecorations.Underline
                };
                panel.Children.Add(title);
                foreach (var p in obj.DataProperties.Keys)
                {
                    var label = new TextBlock()
                    {
                        Text = p + " : " + obj.DataProperties[p]
                    };
                    panel.Children.Add(label);
                }
            }
        }
        
        private void CreateEmbedded(HalObject obj, StackPanel panel)
        {


           if (obj.EmbeddedResources != null && obj.EmbeddedResources.Count > 0)
           {
               var title = new TextBlock()
                {
                    Text = "Embedded",
                    FontSize = 16,
                    TextDecorations = System.Windows.TextDecorations.Underline
                };
                panel.Children.Add(title);
                foreach (var p in obj.EmbeddedResources.Keys)
                {
                    int i = 0;
                    foreach(var e in obj.EmbeddedResources[p])
                    {
                        var countTitle = new TextBlock()
                        {
                            Text = i++.ToString()+") " + p
                        };
                        panel.Children.Add(countTitle);
                        CreateProperties(e, panel);
                        CreateLinks(e, panel);

                        var blank = new TextBlock()
                        {
                            Text = "--------------------------------------"
                        };
                        panel.Children.Add(blank);
                    }
                }
           }
        }
    }
}
