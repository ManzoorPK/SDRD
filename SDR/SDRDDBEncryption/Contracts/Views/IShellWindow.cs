using System.Windows.Controls;

namespace SDRDDBEncryption.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
