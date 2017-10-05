using System.Windows;

namespace Triangles
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow(ViewModel vm)
    {
      DataContext = vm;

      InitializeComponent();
    }

    /// <summary>
    /// Close button handler - closes the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
