using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Models;
using Grocery.Core.Exceptions;
using Grocery.App.Views;

namespace Grocery.App.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;

        [ObservableProperty]
        private string email = "user3@mail.com";

        [ObservableProperty]
        private string password = "Useruser4!";

        [ObservableProperty]
        private string name = "Bob Bladerdeeg";

        [ObservableProperty]
        private string errorMessage = "";

        public RegisterViewModel(IAuthService authService, GlobalViewModel global)
        {
            _authService = authService;
            _global = global;
        }

        [RelayCommand]
        private void Register()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
                ErrorMessage += "Email moet een waarde hebben. ";

            if (string.IsNullOrWhiteSpace(Name))
                ErrorMessage += "Naam moet een waarde hebben. ";

            if (string.IsNullOrWhiteSpace(Password))
                ErrorMessage += "Wachtwoord moet een waarde hebben. ";
            
            if (ErrorMessage != "")
                return;

            try
            {
                Client client = _authService.Register(Email, Password, Name);

                _global.Client = client;
                Application.Current.MainPage = new AppShell();
            }
            catch (UsedEmailException _)
            {
                ErrorMessage = "Emailadres is ongeldig of al in gebruik.";
            }
            catch (InvalidEmailException _)
            {
                ErrorMessage = "Emailadres is ongeldig of al in gebruik.";
            }
            catch (InvalidPasswordException _)
            {
                ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten, waaronder een hoofdletter, kleine letter, cijfer en speciaal teken.";
            }
            catch (Exception _)
            {
                ErrorMessage = "Er is iets mis gegaan met het registreren.";
            }
        }
    }
}
