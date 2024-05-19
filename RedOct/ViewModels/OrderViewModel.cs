using DevExpress.Mvvm;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;
using RedOct.Data;
using RedOct.Infrastructure.Commands;
using RedOct.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using ApplicationContext = RedOct.Data.ApplicationContext;
using MessageBox = System.Windows.MessageBox;

namespace RedOct.ViewModels
{
    internal class OrderViewModel : ViewModelBase
    {
        public OrderViewModel() 
        {
            NewOrder = new Order();
            NewProductionPlane = new ProductionPlane();
            NewProduct = new Product();
            //Комманды на заказы
            AddOrderCommand = new LambdaCommand(OnAddOrderCommandExecuted, CanAddOrderCommandExecute);
            RemoveOrderCommand = new LambdaCommand(OnRemoveOrderCommandExecuted, CanRemoveOrderCommandExecute);
            SaveOrderCommand = new LambdaCommand(OnSaveOrderCommandExecuted, CanSaveOrderCommandExecute);
            //Комманды на план
            AddProductionPlaneCommand = new LambdaCommand(OnAddProductionPlaneCommandExecuted, CanAddProductionPlaneCommandExecute);
            RemoveProductionPlaneCommand = new LambdaCommand(OnRemoveProductionPlaneCommandExecuted, CanRemoveProductionPlaneCommandExecute);
            SaveProductionPlaneCommand = new LambdaCommand(OnSaveProductionPlaneCommandExecuted, CanSaveProductionPlaneCommandExecute);
            //Комманды на продукты
            AddProductCommand = new LambdaCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            RemoveProductCommand = new LambdaCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            SaveProductCommand = new LambdaCommand(OnSaveProductCommandExecuted, CanSaveProductCommandExecute);

            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            IncreaseCommand = new LambdaCommand(OnIncreaseCommandExecuted, CanIncreaseCommandExecute);
            DecreaseCommand = new LambdaCommand(OnDecreaseCommandExecuted, CanDecreaseCommandExecute);
            SearchProductCommand = new LambdaCommand(OnSearchProductCommandExecuted, CanSearchProductCommandExecute);
            ResetData = new LambdaCommand(OnResetDataExecuted, CanResetDataExecute);
            LoadData();
            LoadDataCombo();
        }
        #region Search
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
        public static List<Product> SearchByTitlProduct(string SearchName)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(SearchName))
                {
                    query = query.Where(e => e.Id.Contains(SearchName));
                }
                return query.ToList();
            }
        }
        public ICommand SearchProductCommand { get; set; }
        public bool CanSearchProductCommandExecute(object p) => true;
        public void OnSearchProductCommandExecuted(object p)
        {
            Products.Clear();
            Products = new ObservableCollection<Product>(SearchByTitlProduct(SearchName));
        }

        #endregion
        #region Order Properties
        //На закз
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
        private Order _SelectedOrder;
        public Order SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                _SelectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                if (SelectedOrder != null)
                {
                    NewOrder = SelectedOrder;
                }
            }
        }
        public ICommand AddOrderCommand { get; private set; }
        private bool CanAddOrderCommandExecute(object p) => true;
        private void OnAddOrderCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                var ProductCountToUpdate = context.Products.Find(NewOrder.ProductId);
                if(NewOrder.Amount <= ProductCountToUpdate.Amout)
                {
                    ProductCountToUpdate.Amout -= NewOrder.Amount;
                    var OrderSum = context.Products.Find(NewOrder.ProductId);
                    if (OrderSum != null)
                    {
                        NewOrder.Sum = OrderSum.Price * NewOrder.Amount - (OrderSum.Price * NewOrder.Amount * (Value / 100));
                        context.Orders.Add(NewOrder);
                        context.SaveChanges();
                        //Orders.Add(NewOrder);
                        NewOrder = new Order();
                        ClearData();
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show($"Недостаточное количество продукции. \n Сейчас на складе {ProductCountToUpdate.Amout}");
                }
            }
            OnPropertyChanged(nameof(Orders));
        }
        public ICommand SaveOrderCommand { get; private set; }
        private bool CanSaveOrderCommandExecute(object p) => SelectedOrder != null;
        private void OnSaveOrderCommandExecuted(object p)
        {
            if (SelectedOrder.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var OrderNameToUpdate = context.Orders.Find(SelectedOrder.Id);
                    var OrderSum = context.Products.Find(NewOrder.ProductId);
                    if (OrderNameToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        OrderNameToUpdate.ProductId = NewOrder.ProductId;
                        OrderNameToUpdate.Amount = NewOrder.Amount;
                        OrderNameToUpdate.Sum = OrderSum.Price * NewOrder.Amount - (OrderSum.Price * NewOrder.Amount * (Value / 100));
                        OrderNameToUpdate.Date = NewOrder.Date;
                        OrderNameToUpdate.Status = NewOrder.Status;
                        OrderNameToUpdate.CustomerName = NewOrder.CustomerName;
                        OrderNameToUpdate.PhoneCustomer = NewOrder.PhoneCustomer;
                        ClearData();
                        LoadData();
                        context.SaveChanges();
                        NewOrder = new Order();
                    }
                }
            }
        }
        public ICommand RemoveOrderCommand { get; private set; }
        private bool CanRemoveOrderCommandExecute(object p) => p is Order order && Orders.Contains(order);
        private void OnRemoveOrderCommandExecuted(object p)
        {
            if (SelectedOrder != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.Orders.Remove(SelectedOrder);
                    ClearData();
                    LoadData();
                    context.SaveChanges();
                }
                Orders.Remove(SelectedOrder);
            }
        }
        #endregion
        #region Production PlaneProperties
        //На план
        private ProductionPlane _newProductionPlane;
        public ProductionPlane NewProductionPlane
        {
            get { return _newProductionPlane; }
            set
            {
                _newProductionPlane = value;
                OnPropertyChanged(nameof(NewProductionPlane));
            }
        }
        private ObservableCollection<ProductionPlane> _productionPlans;
        public ObservableCollection<ProductionPlane> ProductionPlans
        {
            get { return _productionPlans; }
            set
            {
                _productionPlans = value;
                OnPropertyChanged(nameof(ProductionPlans));
            }
        }
        private ProductionPlane _SelectedProductionPlane;
        public ProductionPlane SelectedProductionPlane
        {
            get { return _SelectedProductionPlane; }
            set
            {
                _SelectedProductionPlane = value;
                OnPropertyChanged(nameof(SelectedProductionPlane));
                if (SelectedProductionPlane != null)
                {
                    NewProductionPlane = SelectedProductionPlane;
                }
            }
        }
        public ICommand AddProductionPlaneCommand { get; private set; }
        private bool CanAddProductionPlaneCommandExecute(object p) => true;
        private void OnAddProductionPlaneCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                context.ProductionPlanes.Add(NewProductionPlane);
                ClearData();
                LoadData();
                context.SaveChanges();
            }
            ProductionPlans.Add(NewProductionPlane);
            //NewProductionPlane = new ProductionPlane();
            OnPropertyChanged(nameof(ProductionPlans));
        }
        public ICommand SaveProductionPlaneCommand { get; private set; }
        private bool CanSaveProductionPlaneCommandExecute(object p) => SelectedProductionPlane != null;
        private void OnSaveProductionPlaneCommandExecuted(object p)
        {
            if (SelectedProductionPlane.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var ProductionPlaneToUpdate = context.ProductionPlanes.Find(SelectedProductionPlane.Id);
                    if (ProductionPlaneToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        ProductionPlaneToUpdate.UserId = NewProductionPlane.UserId;
                        ProductionPlaneToUpdate.ProductId = NewProductionPlane.ProductId;
                        ProductionPlaneToUpdate.Amount = NewProductionPlane.Amount;
                        ProductionPlaneToUpdate.Date = NewProductionPlane.Date;
                        ProductionPlaneToUpdate.Status = NewProductionPlane.Status;
                        ClearData();
                        LoadData();
                        context.SaveChanges();
                        NewProductionPlane = new ProductionPlane();
                    }
                }
            }
        }
        public ICommand RemoveProductionPlaneCommand { get; private set; }
        private bool CanRemoveProductionPlaneCommandExecute(object p) => p is ProductionPlane productplane && ProductionPlans.Contains(productplane);
        private void OnRemoveProductionPlaneCommandExecuted(object p)
        {
            if (SelectedProductionPlane != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.ProductionPlanes.Remove(SelectedProductionPlane);
                    ClearData();
                    LoadData();
                    context.SaveChanges();
                }
                ProductionPlans.Remove(SelectedProductionPlane);
            }
        }
        #endregion
        #region Product Properties
        //На продукты
        private Product _newProduct;
        public Product NewProduct
        {
            get { return _newProduct; }
            set
            {
                _newProduct = value;
                OnPropertyChanged(nameof(NewProduct));
            }
        }
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        private Product _SelectedProduct;
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                if (SelectedProduct != null)
                {
                    NewProduct = SelectedProduct;
                }
            }
        }
        public ICommand AddProductCommand { get; private set; }
        private bool CanAddProductCommandExecute(object p) => true;
        private void OnAddProductCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                context.Products.Add(NewProduct);
                ClearData();
                LoadData();
                context.SaveChanges();
            }
            Products.Add(NewProduct);
            //NewProduct = new Product();
            OnPropertyChanged(nameof(Products));
        }
        public ICommand SaveProductCommand { get; private set; }
        private bool CanSaveProductCommandExecute(object p) => SelectedProduct != null;
        private void OnSaveProductCommandExecuted(object p)
        {
            if (SelectedProduct.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var ProductNameToUpdate = context.Products.Find(SelectedProduct.Id);
                    if (ProductNameToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        ProductNameToUpdate.Id = NewProduct.Id;
                        ProductNameToUpdate.Expiration = NewProduct.Expiration;
                        ProductNameToUpdate.Price = NewProduct.Price;
                        ProductNameToUpdate.Amout = NewProduct.Amout;
                        ClearData();
                        LoadData();
                        context.SaveChanges();
                        NewProduct = new Product();
                    }
                }
            }
        }
        public ICommand RemoveProductCommand { get; private set; }
        private bool CanRemoveProductCommandExecute(object p) => p is Product product && Products.Contains(product);
        private void OnRemoveProductCommandExecuted(object p)
        {
            if (SelectedProduct != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.Products.Remove(SelectedProduct);
                    ClearData();
                    LoadData();
                    context.SaveChanges();
                }
                Products.Remove(SelectedProduct);
            }
        }
        #endregion
        #region Load Data
        //Загрузка данных
        public ICommand RefreshCommand { get; private set; }
        private bool CanRefreshCommandExecute(object p) => true;
        private void OnRefreshCommandExecuted(object p)
        {
            NewProduct = new Product();
            NewOrder = new Order();
            NewProductionPlane = new ProductionPlane();
        }
        private void LoadData()
        {
            using (var context = new ApplicationContext())
            {
                Orders = new ObservableCollection<Order>(context.Orders.ToList());
                ProductionPlans = new ObservableCollection<ProductionPlane>(context.ProductionPlanes.ToList());
                Products = new ObservableCollection<Product>(context.Products.ToList());
            }
        }
        public ObservableCollection<Product> PrimaryKeysProduct { get; set; }
        public ObservableCollection<Models.User> PrimaryKeysUser { get; set; }
        private void LoadDataCombo()
        {
            using (var context = new ApplicationContext())
            {
                PrimaryKeysProduct = new ObservableCollection<Product>(context.Products.Select(e => new Product { Id = e.Id, Expiration = e.Expiration }).ToList());
                PrimaryKeysUser = new ObservableCollection<Models.User>(context.Users.Where(e => e.Position == "Кондитер").
                    Select(e => new Models.User { Id = e.Id }).ToList());
            }
        }
        private void ClearData()
        {
            Orders.Clear();
            ProductionPlans.Clear();
            Products.Clear();
            Value = 0;
        }
        public ICommand ResetData { get; private set; }
        private bool CanResetDataExecute(object p) => true;
        private void OnResetDataExecuted(object p)
        {
            SearchName = "";
            ClearData();
            LoadData();
        }
        #endregion
        private decimal _Value;
        public decimal Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        public ICommand IncreaseCommand { get; }
        private bool CanIncreaseCommandExecute(object p) => Value <= 99;
        private void OnIncreaseCommandExecuted(object p)
        {
            Value++;
        }
        public ICommand DecreaseCommand { get; }
        private bool CanDecreaseCommandExecute(object p) => Value >= 0;
        private void OnDecreaseCommandExecuted(object p)
        {
            Value--;
        }
    }
}
