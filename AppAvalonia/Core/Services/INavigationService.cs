// INavigationService.cs

using System.Threading.Tasks;

namespace AppAvalonia.Services;

public interface INavigationService
{
    Task NavigateToAsync<TViewModel>(object? parameter = null);
    Task GoBackAsync();
}