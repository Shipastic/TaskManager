using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UsersRequestService _userRequestService;
        
        #region COMMANDS
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        #endregion

        public LoginViewModel()
        {
            _userRequestService = new UsersRequestService();
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
        }

        #region PROPERTIES
        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }

        private UserModel _currentUser;
        public UserModel CurrentUser 
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }
        #endregion

        #region METHODS

        private void GetUserFromDB(object param)
        {
            var passBox = param as PasswordBox;

            UserPassword = passBox.Password;
            AuthToken = _userRequestService.GetToken(UserLogin, UserPassword);

            if (AuthToken != null)
            {
                CurrentUser = _userRequestService.GetCurrent(AuthToken);
                if (CurrentUser != null)
                {
                    MessageBox.Show(CurrentUser.FirstName);
                }
            }

        }
        #endregion
    }
}
