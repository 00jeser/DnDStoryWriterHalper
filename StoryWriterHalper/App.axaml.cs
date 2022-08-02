using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using StoryWriterHalper.Services;

namespace StoryWriterHalper
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            ProjectService.Instance.LoadFromFile("C:\\Users\\00jes\\OneDrive\\Desktop\\Story\\1.zip");
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}