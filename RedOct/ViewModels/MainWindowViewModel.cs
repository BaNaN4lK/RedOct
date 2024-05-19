using DevExpress.Mvvm.UI;
using Microsoft.VisualBasic.ApplicationServices;
using RedOct.Data;
using RedOct.Infrastructure;
using RedOct.Infrastructure.Commands;
using RedOct.Models;
using RedOct.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RedOct.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Models.User> Users { get; }
        public MainWindowViewModel()
        {
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        private string name;
        private string password;
        private string _currentUser;

        public string CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
            public ICommand LoginCommand { get; }

            private bool CanLoginCommandExecute(object p) => true;
            private void OnLoginCommandExecuted(object p)
            {
                //UserView win = new UserView();
                using (var context = new ApplicationContext())
                {
                var user = context.Users.FirstOrDefault(u => name == u.Login && password == u.Password);
                if (user != null)
                {
                    switch (user.Position)
                    {
                        case "Кадровик":
                            Manager.MainFrame.Navigate(new UserView());
                            //CurrentUser = user.Login;
                            break;
                        case "Менеджер по продажам":
                            Manager.MainFrame.Navigate(new OrderView());
                            //_currentUser = user.Id;
                            break;
                        case "Аналитик":
                            Manager.MainFrame.Navigate(new AnalystView());
                            //_currentUser = user.Id;
                            break;
                        case "Кондитер":
                            Manager.MainFrame.Navigate(new ConfectionerView());
                            //_currentUser = user.Id;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль");
                }
                //if (context.Users.Any(u => Name == u.Login && Password == u.Password && u.Position == "Кадровик"))
                //{
                //    Manager.MainFrame.Navigate(new UserView());
                //    //win.Show();
                //}
                //if (context.Users.Any(u => Name == u.Login && Password == u.Password && u.Position == "Менеджер по продажам"))
                //{
                //    Manager.MainFrame.Navigate(new OrderView());
                //}
                //if (context.Users.Any(u => Name == u.Login && Password == u.Password && u.Position == "Аналитик"))
                //{
                //    Manager.MainFrame.Navigate(new UserView());
                //}
                //if (context.Users.Any(u => Name == u.Login && Password == u.Password && u.Position == "Кондитер"))
                //{
                //    Manager.MainFrame.Navigate(new UserView());
                //}
                //else
                //{
                //    MessageBox.Show("Мды");
                //}

            }
        }
    }
}
