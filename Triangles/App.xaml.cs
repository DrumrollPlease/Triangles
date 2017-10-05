using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Triangles
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
      System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      var mainWnd = new MainWindow(new ViewModel());

      mainWnd.Show();
    }

  }
}