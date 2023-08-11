using System.Windows.Controls;

namespace SDRDBE.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
