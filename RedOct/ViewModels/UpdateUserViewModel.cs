using RedOct.Infrastructure.Commands;
using RedOct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RedOct.ViewModels
{
    internal class UpdateUserViewModel : ViewModelBase
    {
        public bool IsSaved { get; set; }
        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public UpdateUserViewModel(User user)
        {
            User = user;
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
        }
        public ICommand SaveCommand { get; private set; }
        private bool CanSaveCommandExecute(object p) => true;
        private void OnSaveCommandExecuted(object p)
        {
            IsSaved = true;
            //CloseWindow();
        }
        //private void CloseWindow()
        //{
        //    var window = Window.GetWindow(this);
        //    if (window != null)
        //    {
        //        window.DialogResult = true;
        //        window.Close();
        //    }
        //}
    }
}
