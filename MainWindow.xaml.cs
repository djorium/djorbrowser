using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BrowserApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddNewTab("https://github.com/djorium"); 
        }
        
        private void AddNewTab(string initialUrl)
        {
            var browser = CreateBrowser(initialUrl);

            var tabItem = new TabItem
            {
                Header = $"Tab {TabControl.Items.Count + 1}",
                Content = browser
            };

            TabControl.Items.Add(tabItem);
            TabControl.SelectedItem = tabItem; 
        }
        
        private WebView2 CreateBrowser(string initialUrl)
        {
            var browser = new WebView2();
            browser.Loaded += async (s, e) => await browser.EnsureCoreWebView2Async();
            browser.Source = new Uri(initialUrl);
            return browser;
        }
        
        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab("https://github.com/djorium"); 
        }
        
        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTab)
            {
                TabControl.Items.Remove(selectedTab);
            }
        }
        
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is WebView2 browser)
            {
                string input = SearchBar.Text.Trim();
                string url;
                
                if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
                {
                    url = input;
                }
                else
                {
                    url = $"https://www.google.com/search?q={Uri.EscapeDataString(input)}";
                }

                browser.Source = new Uri(url);
            }
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is WebView2 browser)
            {
                SearchBar.Text = browser.Source?.ToString() ?? string.Empty;
            }
        }
    }
}

