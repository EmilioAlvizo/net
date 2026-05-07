// INavigationService.cs

using System.Threading.Tasks;

namespace AppAvalonia3.Services;

public interface INavigationService
{
    // Permite navegar pasando el tipo de ViewModel
    Task NavigateToAsync<T>() where T : class;
    
    // Necesitamos que el servicio conozca al MainViewModel para cambiar su CurrentView
    void SetMainViewModel(object mainVM);
}