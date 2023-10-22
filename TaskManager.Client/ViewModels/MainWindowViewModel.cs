using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region COMMANDS

        public DelegateCommand OpenMyInfoPageCommand;
        public DelegateCommand OpenMyDesksPageCommand;
        public DelegateCommand OpenMyTasksPageCommand;
        public DelegateCommand OpenMyProjectsPageCommand;
        public DelegateCommand LogoutCommand;

        public DelegateCommand OpenUsersManagementCommand;
        #endregion

        public MainWindowViewModel(AuthToken token, UserModel currentUser)
        {
            Token = token;
            CurrentUser = currentUser;

            OpenMyInfoPageCommand     = new DelegateCommand(OpenMyInfoPage);
            NavButtons.Add(_userInfoBtnName, OpenMyInfoPageCommand);

            OpenMyDesksPageCommand    = new DelegateCommand(OpenMyDesksPage);
            NavButtons.Add(_userDesksBtnName, OpenMyDesksPageCommand);

            OpenMyTasksPageCommand    = new DelegateCommand(OpenMyTasksPage);
            NavButtons.Add(_userTasksBtnName, OpenMyTasksPageCommand);

            OpenMyProjectsPageCommand = new DelegateCommand(OpenMyProjectsPage);
            NavButtons.Add(_userProjectsBtnName, OpenMyProjectsPageCommand);

            if (CurrentUser.Status == UserStatus.Admin)
            {
                OpenUsersManagementCommand = new DelegateCommand(OpenUsersManagement);
                NavButtons.Add(_managaUsersBtnName, OpenUsersManagementCommand);
            }
            
            LogoutCommand             = new DelegateCommand(Logout);
            NavButtons.Add(_logoutBtnName, LogoutCommand);



        }
        #region PROPERTIES
        private readonly string _logoutBtnName       = "Logout";
        private readonly string _userInfoBtnName     = "My Info";
        private readonly string _userTasksBtnName    = "My tasks";
        private readonly string _userDesksBtnName    = "My desks";
        private readonly string _userProjectsBtnName = "My projects";

        private readonly string _managaUsersBtnName  = "Users";

        private AuthToken _token;

        public AuthToken Token
        {
            get => _token;
            private set
            {
                _token = value;
                RaisePropertyChanged(nameof(Token));           
            }
        }

        private UserModel _currentUser;

        public UserModel CurrentUser
        {
            get =>_currentUser; 
            private set 
            { 
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));            
            }
        }

        private Dictionary<string, DelegateCommand> _navButtons = new Dictionary<string, DelegateCommand>();

        public Dictionary<string, DelegateCommand> NavButtons
        {
            get => _navButtons;
            set 
            {
                _navButtons = value;
                RaisePropertyChanged(nameof(NavButtons));
            }

        }

        #endregion

        #region METHODS

        private void OpenMyInfoPage()
        {
            ShowMessage(_userInfoBtnName);
        }

        private void OpenMyDesksPage()
        {
            ShowMessage(_userDesksBtnName);
        }

        private void OpenMyTasksPage()
        {
            ShowMessage(_userTasksBtnName);
        }

        private void OpenMyProjectsPage()
        {
            ShowMessage(_userProjectsBtnName);
        }
        private void Logout()
        {
            ShowMessage(_logoutBtnName);
        }

        #endregion

        private void ShowMessage(string mes)
        {
            MessageBox.Show(mes);
        }

        private void OpenUsersManagement()
        {
            ShowMessage(_managaUsersBtnName);
        }
    }
}
