using DevExpress.Mvvm.POCO;
using Microsoft.EntityFrameworkCore;
using RedOct.Data;
using RedOct.Infrastructure.Commands;
using RedOct.Models;
using RedOct.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedOct.ViewModels
{
    internal class UserVM : ViewModelBase
    {
        public UserVM() 
        {
            NewUser = new User();
            AddUserCommand = new LambdaCommand(OnAddUserCommandExecuted, CanAddUserCommandExecute);
            RemoveUserCommand = new LambdaCommand(OnRemoveUserCommandExecuted, CanRemoveUserCommandExecute);
            SaveUserCommand = new LambdaCommand(OnSaveUserCommandExecuted, CanSaveUserCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            //UpdateUserCommand = new LambdaCommand(OnUpdateUserCommandExecuted, CanUpdateUserCommandExecute);
            LoadUser();
        }


        private User _newUser;
        public User NewUser
        {
            get { return _newUser; } 
            set 
            { 
                _newUser = value; 
                OnPropertyChanged(nameof(NewUser));
            }
        }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private User _SelectedUser;
        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                if (SelectedUser != null)
                {
                    NewUser = SelectedUser;
                }
            }
        }
        private void LoadUser()
        {
            using (var context = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(context.Users.ToList());
            }
        }

        public ICommand AddUserCommand { get; private set; }
        private bool CanAddUserCommandExecute(object p) => true;
        private void OnAddUserCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                context.Users.Add(NewUser);
                context.SaveChanges();
            }
            Users.Add(NewUser);
            NewUser = new User();
            OnPropertyChanged(nameof(Users));
        }
        public ICommand SaveUserCommand { get; private set; }
        private bool CanSaveUserCommandExecute(object p) => SelectedUser != null;
        private void OnSaveUserCommandExecuted(object p)
        {
            if (SelectedUser.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var UserToUpdate = context.Users.Find(SelectedUser.Id);
                    if (UserToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        UserToUpdate.Id = NewUser.Id;
                        UserToUpdate.Salary = NewUser.Salary;
                        UserToUpdate.Position = NewUser.Position;
                        UserToUpdate.Login = NewUser.Login;
                        UserToUpdate.Password = NewUser.Password;
                        context.SaveChanges();
                    }
                }
            }
        }
        public ICommand RemoveUserCommand { get; private set; } 
        private bool CanRemoveUserCommandExecute(object p) => p is User user && Users.Contains(user);
        private void OnRemoveUserCommandExecuted(object p)
        {
            if(SelectedUser != null)
            {
                using(var context = new ApplicationContext())
                {
                    context.Users.Remove(SelectedUser);
                    context.SaveChanges();
                }
                Users.Remove(SelectedUser);
            }
        }
        public ICommand RefreshCommand { get; private set; }
        private bool CanRefreshCommandExecute(object p) => true;
        private void OnRefreshCommandExecuted(object p)
        {
            NewUser = new User();
        }

        //public ICommand UpdateUserCommand { get; private set; }
        //private bool CanUpdateUserCommandExecute(object p) => SelectedUser != null;
        //private void OnUpdateUserCommandExecuted(object p)
        //{
        //    UpdateUserViewModel editUserViewModel = new UpdateUserViewModel(SelectedUser);
        //    var editUserV = new EditUserV();
        //    editUserV.DataContext = SelectedUser;
        //    editUserV.ShowDialog();

        //    if (editUserViewModel.IsSaved)
        //    {
        //        using (var context = new ApplicationContext())
        //        {
        //            context.Entry(SelectedUser).CurrentValues.SetValues(editUserViewModel.User);
        //            context.SaveChanges();
        //        }
        //    }
        //}
    }
}
