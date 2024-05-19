using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using RedOct.Data;
using RedOct.Infrastructure.Commands;
using RedOct.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Reflection.Metadata;

namespace RedOct.ViewModels
{
    internal class AnalystviewModel : ViewModelBase
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public List<Axis> XAxes { get; set; }
        public List<Axis> YAxes { get; set; }
        public AnalystviewModel()
        {
            NewUser = new User();
            SaveUserCommand = new LambdaCommand(OnSaveUserCommandExecuted, CanSaveUserCommandExecute);
            SavePurchaseMaterialCommand = new LambdaCommand(OnSavePurchaseMaterialCommandExecuted, CanSavePurchaseMaterialCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            SearchUserCommand = new LambdaCommand(OnSearchUserCommandExecuted, CanSearchUserCommandExecute);
            ResetData = new LambdaCommand(OnResetDataExecuted, CanResetDataExecute);
            SearchRangeDateCommand = new LambdaCommand(OnSearchRangeDateCommandExecuted, CanSearchRangeDateCommandExecute);
            SearchRangeDatePurchaseMaterialsCommand = new LambdaCommand(OnSearchRangeDatePurchaseMaterialsCommandExecuted, CanSearchRangeDatePurchaseMaterialsCommandExecute);
            PrintCommand = new LambdaCommand(OnPrintCommandExecuted, CanPrintCommandExecute);
            LoadDataCombo();
            LoadData();

        }
        #region Search Data
        private string _serachName;
        public string SearchName
        {
            get { return _serachName; }
            set
            {
                _serachName = value;
                OnPropertyChanged(nameof(SearchName));
            }
        }
        private DateTime _dateNow = DateTime.Now;
        public DateTime SearchDateNow
        {
            get { return _dateNow; }
            set
            {
                _dateNow = value;
                OnPropertyChanged(nameof(SearchDateNow));
            }
        }
        private DateTime _dateFrome = DateTime.Now;
        public DateTime SearchDateFrome
        {
            get { return _dateFrome; }
            set
            {
                _dateFrome = value;
                OnPropertyChanged(nameof(SearchDateFrome));
            }
        }
        public static List<User> SearchByTitlUser(string SearchName)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(SearchName))
                {
                    query = query.Where(e => e.Id.Contains(SearchName));
                }
                return query.ToList();
            }
        }
        public static List<Order> SearchByDateRangeOrder(DateTime SearchDateFrome, DateTime SearchDateNow)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Orders.AsQueryable();
                query = query.Where(e => e.Date.Date >= SearchDateFrome.Date
                                         && e.Date.Date <= SearchDateNow.Date);
                return query.ToList();
            }
        }
        public static List<PurchaseMaterial> SearchRangeDatePurchaseMaterials(DateTime SearchDateFrome, DateTime SearchDateNow)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.PurchaseMaterials.AsQueryable();
                query = query.Where(e => e.DateCreate.Date >= SearchDateFrome.Date
                                         && e.DateCreate.Date <= SearchDateNow.Date);
                return query.ToList();
            }
        }
        public ICommand SearchUserCommand { get; set; }
        public bool CanSearchUserCommandExecute(object p) => true;
        public void OnSearchUserCommandExecuted(object p)
        {
            Users.Clear();
            Users = new ObservableCollection<User>(SearchByTitlUser(SearchName));
        }
        public ICommand SearchRangeDateCommand { get; set; }
        public bool CanSearchRangeDateCommandExecute(object p) => true;
        public void OnSearchRangeDateCommandExecuted(object p)
        {
            Orders.Clear();
            Orders = new ObservableCollection<Order>(SearchByDateRangeOrder(SearchDateFrome, SearchDateNow));
        }
        public ICommand SearchRangeDatePurchaseMaterialsCommand { get; set; }
        public bool CanSearchRangeDatePurchaseMaterialsCommandExecute(object p) => true;
        public void OnSearchRangeDatePurchaseMaterialsCommandExecuted(object p)
        {
            PurchaseMaterials.Clear();
            PurchaseMaterials = new ObservableCollection<PurchaseMaterial>(SearchRangeDatePurchaseMaterials(SearchDateFrome, SearchDateNow));
        }
        #endregion
        #region User Properties
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
                        ClearData();
                        LoadData();
                    }
                }
            }
        }
        #endregion
        #region Order Prop
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }
        private Order _newOrder;
        public Order NewOrder
        {
            get { return _newOrder; }
            set
            {
                _newOrder = value;
                OnPropertyChanged(nameof(NewOrder));
            }
        }
        private IList _selectedItems;
        public IList SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }
        #endregion
        #region PurchaseMaterial Prop
        private PurchaseMaterial _NewPurchaseMaterial;
        public PurchaseMaterial NewPurchaseMaterial
        {
            get { return _NewPurchaseMaterial; }
            set
            {
                _NewPurchaseMaterial = value;
                OnPropertyChanged(nameof(NewPurchaseMaterial));
            }
        }
        private ObservableCollection<PurchaseMaterial> _PurchaseMaterials;
        public ObservableCollection<PurchaseMaterial> PurchaseMaterials
        {
            get { return _PurchaseMaterials; }
            set
            {
                _PurchaseMaterials = value;
                OnPropertyChanged(nameof(PurchaseMaterials));
            }
        }
        private PurchaseMaterial _SelectedPurchaseMaterial;
        public PurchaseMaterial SelectedPurchaseMaterial
        {
            get { return _SelectedPurchaseMaterial; }
            set
            {
                _SelectedPurchaseMaterial = value;
                OnPropertyChanged(nameof(SelectedPurchaseMaterial));
                if (SelectedPurchaseMaterial != null)
                {
                    NewPurchaseMaterial = SelectedPurchaseMaterial;
                }
            }
        }
        public ICommand SavePurchaseMaterialCommand { get; private set; }
        private bool CanSavePurchaseMaterialCommandExecute(object p) => SelectedPurchaseMaterial != null;
        private void OnSavePurchaseMaterialCommandExecuted(object p)
        {
            if (SelectedPurchaseMaterial.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var MaterialInfo = context.Materials.Find(NewPurchaseMaterial.MaterialId);
                    var PurchaseMaterialsToUpdate = context.PurchaseMaterials.Find(SelectedPurchaseMaterial.Id);
                    if (PurchaseMaterialsToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        PurchaseMaterialsToUpdate.MaterialId = NewPurchaseMaterial.MaterialId;
                        PurchaseMaterialsToUpdate.DateCreate = NewPurchaseMaterial.DateCreate;
                        PurchaseMaterialsToUpdate.Amount = NewPurchaseMaterial.Amount;
                        PurchaseMaterialsToUpdate.Unit = NewPurchaseMaterial.Unit;
                        PurchaseMaterialsToUpdate.Status = NewPurchaseMaterial.Status;
                        PurchaseMaterialsToUpdate.Price = MaterialInfo.UnitPrice * NewPurchaseMaterial.Amount;
                        PurchaseMaterialsToUpdate.ProviderId = NewPurchaseMaterial.ProviderId;
                        ClearData();
                        LoadData();
                        context.SaveChanges();
                        //NewPurchaseMaterial = new PurchaseMaterial();
                    }
                }
            }
        }
        #endregion
        #region Data
        public ObservableCollection<Provider> PrimaryKeysProvider { get; set; }
        public ObservableCollection<Models.Material> PrimaryKeysMaterial { get; set; }
        private void LoadDataCombo()
        {
            using (var context = new ApplicationContext())
            {
                PrimaryKeysProvider = new ObservableCollection<Provider>(context.Providers.Select(e => new Provider { Id = e.Id }).ToList());
                PrimaryKeysMaterial = new ObservableCollection<Models.Material>(context.Materials.Select(e => new Models.Material { Id = e.Id }).ToList());
            }
        }
        public ICommand RefreshCommand { get; private set; }
        private bool CanRefreshCommandExecute(object p) => true;
        private void OnRefreshCommandExecuted(object p)
        {
            NewUser = new User();
            NewPurchaseMaterial = new PurchaseMaterial();
        }
        public ICommand ResetData { get; private set; }
        private bool CanResetDataExecute(object p) => true;
        private void OnResetDataExecuted(object p)
        {
            SearchName = "";
            SearchDateFrome = DateTime.Now;
            SearchDateNow = DateTime.Now;
            ClearData();
            LoadData();
        }
        private void ClearData()
        {
            Users.Clear();
            PurchaseMaterials.Clear();
        }
        private void LoadData()
        {
            using (var context = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(context.Users.ToList());
                PurchaseMaterials = new ObservableCollection<PurchaseMaterial>(context.PurchaseMaterials.ToList());
                Orders = new ObservableCollection<Order>(context.Orders.ToList());
                Series = new ObservableCollection<ISeries>
                    {
                        new ColumnSeries<double>
                        {
                            Values = new ObservableCollection<double> { (double)context.Users.Sum(e => e.Salary),
                                (double)context.Orders.Sum(e => e.Sum),
                                (double)context.PurchaseMaterials.Sum(e => e.Price)},
                        }
                    };
                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        Labels = new string[] { "Зарплаты", "Заказы", "Заявки"}
                    }
                };
                YAxes = new List<Axis>
                {
                    new Axis
                        {
                            Labeler = Labelers.Currency
                        }
                };
            }
        }
        #endregion
        #region Print
        public ICommand PrintCommand { get; private set; }
        private bool CanPrintCommandExecute(object p) => true;
        private void OnPrintCommandExecuted(object p)
        {
            //var selectedItems = p as IList;
            //var printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    FlowDocument doc = new FlowDocument();
            //    foreach (var item in selectedItems)
            //    {
            //        doc.Blocks.Add(new Paragraph(new Run(item.ToString())));
            //    }
            //    IDocumentPaginatorSource idpSource = doc;
            //    printDialog.PrintDocument(idpSource.DocumentPaginator, "Печать выбранных элементов");
            //}
            PrintSelectedItems(p);
        }
        private void PrintSelectedItems(object parameter)
        {
            var selectedItems = parameter as IList;

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();

                foreach (var item in selectedItems)
                {

                    doc.Blocks.Add(new Paragraph(new Run(item.ToString())));
                }
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать данных");
            }
        }
        #endregion
    }
}
