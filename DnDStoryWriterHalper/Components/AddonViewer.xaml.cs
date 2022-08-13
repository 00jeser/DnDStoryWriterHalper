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
using CefSharp;
using DnDStoryWriterHalper.Models;

namespace DnDStoryWriterHalper.Components
{
    /// <summary>
    /// Логика взаимодействия для AddonViewer.xaml
    /// </summary>
    public partial class AddonViewer : UserControl
    {
        public static DependencyProperty PageModelProperty = DependencyProperty.Register(
            nameof(PageModelfp), 
            typeof(object), 
            typeof(AddonViewer), 
            new FrameworkPropertyMetadata(
                new AddonPage(), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PageModelPropertyChanged));

        public bool IsLoading = false;

        private static void PageModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = (e.NewValue as AddonPage);
            if (page == null || page.PageName == null || page.PluginName == null) 
                return;
            var av = (d as AddonViewer);
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Addons", page.PluginName,
                page.PageName, "index.html");
            av.Browser.Load(path);
            av.PageModel = page;
            av.Js = new JsMethodsHandler(av);
            av.IsLoading = true;
            av.SetContent(page.Content);
        }

        public AddonPage PageModelfp
        {
            get => GetValue(PageModelProperty) as AddonPage;
            set => SetValue(PageModelProperty, value);
        }

        public AddonPage PageModel;

        public record JsMethodsHandler(AddonViewer This)
        {
            public void UpdateContent(string Content)
            {
                var pm = This.PageModel;
                pm.Content = Content;
            }
        }

        public JsMethodsHandler Js;
        public AddonViewer()
        {
            InitializeComponent();
            Js = new JsMethodsHandler(this);
            Browser.AddressChanged += Browser_OnAddressChanged;
            Browser.LoadingStateChanged += BrowserOnLoadingStateChanged;
        }

        private async void SetContent(string content)
        {
            //while (IsLoading)
            //{
            //    await Task.Delay(100);
            //}
            await Browser.WaitForInitialLoadAsync();
            if (Browser.CanExecuteJavascriptInMainFrame)
            {
                JavascriptResponse response = await Browser.EvaluateScriptAsync("window.setContent(\"" + content + "\")");

#if DEBUG
                if (response.Result != null)
                {
                    MessageBox.Show(response.Result.ToString(), "JavaScript Result");
                }
#endif
            }
        }

        private bool registered = false;
        private async void BrowserOnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if (!registered && e.IsLoading == false)
            {
#if DEBUG
            //Browser.ShowDevTools();
#endif
                IsLoading = false;
                Browser.JavascriptObjectRepository.Register("dnd", Js, options: BindingOptions.DefaultBinder);
                var r = await Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync(\"dnd\")");
                registered = true;
            }
        }

        private void Browser_OnAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
