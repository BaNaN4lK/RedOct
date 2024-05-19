using RedOct.Data;
using RedOct.Infrastructure.Commands;
using RedOct.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Material = RedOct.Models.Material;

namespace RedOct.ViewModels
{
    class ConfectionerViewModel : ViewModelBase
    {
        public ConfectionerViewModel()
        {
            NewMaterial = new Material();
            NewProvider = new Provider();
            NewPurchaseMaterial = new PurchaseMaterial();
            NewProductionPlane = new ProductionPlane();
            //Комманды на поставщика
            AddProviderCommand = new LambdaCommand(OnAddProviderCommandExecuted, CanAddProviderCommandExecute);
            RemoveProviderCommand = new LambdaCommand(OnRemoveProviderCommandExecuted, CanRemoveProviderCommandExecute);
            SaveProviderCommand = new LambdaCommand(OnSaveProviderCommandExecuted, CanSaveProviderCommandExecute);
            //Комманды на Заявка на заказ
            AddPurchaseMaterialCommand = new LambdaCommand(OnAddPurchaseMaterialCommandExecuted, CanAddPurchaseMaterialCommandExecute);
            RemovePurchaseMaterialCommand = new LambdaCommand(OnRemovePurchaseMaterialCommandExecuted, CanRemovePurchaseMaterialCommandExecute);
            SavePurchaseMaterialCommand = new LambdaCommand(OnSavePurchaseMaterialCommandExecuted, CanSavePurchaseMaterialCommandExecute);
            //Комманды на материалы 
            AddMaterialCommand = new LambdaCommand(OnAddMaterialCommandExecuted, CanAddMaterialCommandExecute);
            RemoveMaterialCommand = new LambdaCommand(OnRemoveMaterialCommandExecuted, CanRemoveMaterialCommandExecute);
            SaveMaterialCommand = new LambdaCommand(OnSaveMaterialCommandExecuted, CanSaveMaterialCommandExecute);
            //Комманды на план
            AddProductionPlaneCommand = new LambdaCommand(OnAddProductionPlaneCommandExecuted, CanAddProductionPlaneCommandExecute);
            RemoveProductionPlaneCommand = new LambdaCommand(OnRemoveProductionPlaneCommandExecuted, CanRemoveProductionPlaneCommandExecute);
            SaveProductionPlaneCommand = new LambdaCommand(OnSaveProductionPlaneCommandExecuted, CanSaveProductionPlaneCommandExecute);

            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            SearchProductCommand = new LambdaCommand(OnSearchProductCommandExecuted, CanSearchProductCommandExecute);
            ResetData = new LambdaCommand(OnResetDataExecuted, CanResetDataExecute);
            LoadData();
            LoadDataCombo();
        }
        #region Purchase Material Properties
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
        public ICommand AddPurchaseMaterialCommand { get; private set; }
        private bool CanAddPurchaseMaterialCommandExecute(object p) => true;
        private void OnAddPurchaseMaterialCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                var MaterialInfo = context.Materials.Find(NewPurchaseMaterial.MaterialId);
                NewPurchaseMaterial.Price = MaterialInfo.UnitPrice * NewPurchaseMaterial.Amount;
                context.PurchaseMaterials.Add(NewPurchaseMaterial);
                context.SaveChanges();
                ClearData();
                LoadData();
            }
            //NewPurchaseMaterial = new PurchaseMaterial();
            OnPropertyChanged(nameof(PurchaseMaterials));
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
        public ICommand RemovePurchaseMaterialCommand { get; private set; }
        private bool CanRemovePurchaseMaterialCommandExecute(object p) => p is PurchaseMaterial purchaseMaterial && PurchaseMaterials.Contains(purchaseMaterial);
        private void OnRemovePurchaseMaterialCommandExecuted(object p)
        {
            if (SelectedPurchaseMaterial != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.PurchaseMaterials.Remove(SelectedPurchaseMaterial);
                    ClearData();
                    LoadData();
                    context.SaveChanges();
                }
                PurchaseMaterials.Remove(SelectedPurchaseMaterial);
            }
        }
        #endregion
        #region Provider Properties
        private Provider _NewProvider;
        public Provider NewProvider
        {
            get { return _NewProvider; }
            set
            {
                _NewProvider = value;
                OnPropertyChanged(nameof(NewProvider));
            }
        }
        private ObservableCollection<Provider> _providers;
        public ObservableCollection<Provider> Providers
        {
            get { return _providers; }
            set
            {
                _providers = value;
                OnPropertyChanged(nameof(Providers));
            }
        }
        private Provider _SelectedProviders;
        public Provider SelectedProviders
        {
            get { return _SelectedProviders; }
            set
            {
                _SelectedProviders = value;
                OnPropertyChanged(nameof(SelectedProviders));
                if (SelectedProviders != null)
                {
                    NewProvider = SelectedProviders;
                }
            }
        }
        public ICommand AddProviderCommand { get; private set; }
        private bool CanAddProviderCommandExecute(object p) => true;
        private void OnAddProviderCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                context.Providers.Add(NewProvider);
                ClearData();
                LoadData();
                LoadDataCombo();
                context.SaveChanges();
            }
            Providers.Add(NewProvider);
            //NewProvider = new Provider();
            OnPropertyChanged(nameof(Providers));
        }
        public ICommand SaveProviderCommand { get; private set; }
        private bool CanSaveProviderCommandExecute(object p) => SelectedProviders != null;
        private void OnSaveProviderCommandExecuted(object p)
        {
            if (SelectedProviders.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var ProviderToUpdate = context.Providers.Find(SelectedProviders.Id);
                    if (ProviderToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        ProviderToUpdate.Id = NewProvider.Id;
                        ProviderToUpdate.Phone = NewProvider.Phone;
                        ProviderToUpdate.Email = NewProvider.Email;
                        ProviderToUpdate.City = NewProvider.City;
                        ClearData();
                        LoadData();
                        LoadDataCombo();
                        context.SaveChanges();
                        //NewProvider = new Provider();
                    }
                }
            }
        }
        public ICommand RemoveProviderCommand { get; private set; }
        private bool CanRemoveProviderCommandExecute(object p) => p is Provider provider && Providers.Contains(provider);
        private void OnRemoveProviderCommandExecuted(object p)
        {
            if (SelectedProviders != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.Providers.Remove(SelectedProviders);
                    ClearData();
                    LoadData();
                    LoadDataCombo();
                    context.SaveChanges();
                }
                Providers.Remove(SelectedProviders);
            }
        }
        #endregion
        #region Material Properties
        private Material _Newmaterial;
        public Material NewMaterial
        {
            get { return _Newmaterial; }
            set 
            {
                _Newmaterial = value;
                OnPropertyChanged(nameof(NewMaterial));
            }
        }
        private ObservableCollection<Material> _materials;
        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            set
            {
                _materials = value;
                OnPropertyChanged(nameof(Materials));
            }
        }
        private Material _SelectedMaterial;
        public Material SelectedMaterial
        {
            get { return _SelectedMaterial; }
            set
            {
                _SelectedMaterial = value;
                OnPropertyChanged(nameof(SelectedMaterial));
                if (SelectedMaterial != null)
                {
                    NewMaterial = SelectedMaterial;
                }
            }
        }
        public ICommand AddMaterialCommand { get; private set; }
        private bool CanAddMaterialCommandExecute(object p) => true;
        private void OnAddMaterialCommandExecuted(object p)
        {
            using (var context = new ApplicationContext())
            {
                context.Materials.Add(NewMaterial);
                ClearData();
                LoadData();
                LoadDataCombo();
                context.SaveChanges();
            }
            Materials.Add(NewMaterial);
            //NewMaterial = new Material();
            OnPropertyChanged(nameof(Materials));
        }
        public ICommand SaveMaterialCommand { get; private set; }
        private bool CanSaveMaterialCommandExecute(object p) => SelectedMaterial != null;
        private void OnSaveMaterialCommandExecuted(object p)
        {
            if (SelectedMaterial.Id != null)
            {
                using (var context = new ApplicationContext())
                {
                    var MaterialToUpdate = context.Materials.Find(SelectedMaterial.Id);
                    if (MaterialToUpdate != null)
                    {
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        MaterialToUpdate.Id = NewMaterial.Id;
                        MaterialToUpdate.Date= NewMaterial.Date;
                        MaterialToUpdate.Amount = NewMaterial.Amount;
                        MaterialToUpdate.Unit = NewMaterial.Unit;
                        MaterialToUpdate.UnitPrice = NewMaterial.UnitPrice;
                        ClearData();
                        LoadData();
                        LoadDataCombo();
                        context.SaveChanges();
                        //NewMaterial = new Material();
                    }
                }
            }
        }
        public ICommand RemoveMaterialCommand { get; private set; }
        private bool CanRemoveMaterialCommandExecute(object p) => p is Material material && Materials.Contains(material);
        private void OnRemoveMaterialCommandExecuted(object p)
        {
            if (SelectedMaterial != null)
            {
                using (var context = new ApplicationContext())
                {
                    context.Materials.Remove(SelectedMaterial);
                    ClearData();
                    LoadData();
                    LoadDataCombo();
                    context.SaveChanges();
                }
                Materials.Remove(SelectedMaterial);
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
                    var ProductCountToUpdate = context.Products.Find(SelectedProductionPlane.ProductId);
                    var ProductionPlaneToUpdate = context.ProductionPlanes.Find(SelectedProductionPlane.Id);
                    if (ProductionPlaneToUpdate != null && NewProductionPlane.Status == "Выполнен")
                    {
                        ProductCountToUpdate.Amout += SelectedProductionPlane.Amount;
                        //context.Entry(SelectedUser).State = EntityState.Modified;
                        ProductionPlaneToUpdate.UserId = NewProductionPlane.UserId;
                        ProductionPlaneToUpdate.ProductId = NewProductionPlane.ProductId;
                        ProductionPlaneToUpdate.Amount = NewProductionPlane.Amount;
                        ProductionPlaneToUpdate.Date = NewProductionPlane.Date;
                        ProductionPlaneToUpdate.Status = NewProductionPlane.Status;
                        ClearData();
                        LoadData();
                        context.SaveChanges();
                        //NewProductionPlane = new ProductionPlane();
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
        public static List<Material> SearchByTitlProduct(string SearchName)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Materials.AsQueryable();
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
            Materials.Clear();
            Materials = new ObservableCollection<Material>(SearchByTitlProduct(SearchName));
        }

        #endregion
        #region Load Data
        //Загрузка данных
        public ICommand RefreshCommand { get; private set; }
        private bool CanRefreshCommandExecute(object p) => true;
        private void OnRefreshCommandExecuted(object p)
        {
            NewMaterial = new Material();
            NewProvider = new Provider();
            NewPurchaseMaterial = new PurchaseMaterial();
            NewProductionPlane = new ProductionPlane();
        }
        private void LoadData()
        {
            using (var context = new ApplicationContext())
            {
                ProductionPlans = new ObservableCollection<ProductionPlane>(context.ProductionPlanes.ToList());
                PurchaseMaterials = new ObservableCollection<PurchaseMaterial>(context.PurchaseMaterials.ToList());
                Materials = new ObservableCollection<Material>(context.Materials.ToList());
                Providers = new ObservableCollection<Provider>(context.Providers.ToList());
            }
        }
        public ObservableCollection<Provider> PrimaryKeysProvider { get; set; }
        public ObservableCollection<Material> PrimaryKeysMaterial { get; set; }
        public ObservableCollection<Models.User> PrimaryKeysUser { get; set; }
        public ObservableCollection<Product> PrimaryKeysProduct { get; set; }
        private void LoadDataCombo()
        {
            using (var context = new ApplicationContext())
            {
                PrimaryKeysProduct = new ObservableCollection<Product>(context.Products.Select(e => new Product { Id = e.Id, Expiration = e.Expiration }).ToList());
                PrimaryKeysProvider = new ObservableCollection<Provider>(context.Providers.Select(e => new Provider { Id = e.Id}).ToList());
                PrimaryKeysMaterial = new ObservableCollection<Material>(context.Materials.Select(e => new Material { Id = e.Id}).ToList());
                PrimaryKeysUser = new ObservableCollection<Models.User>(context.Users.Where(e => e.Position == "Кондитер").
                    Select(e => new Models.User { Id = e.Id }).ToList());
            }
        }
        public ICommand ResetData { get; private set; }
        private bool CanResetDataExecute(object p) => true;
        private void OnResetDataExecuted(object p)
        {
            SearchName = "";
            ClearData();
            LoadData();
        }
        private void ClearData()
        {
            ProductionPlans.Clear();
            PurchaseMaterials.Clear();
            Materials.Clear();
            Providers.Clear();
        }
        #endregion
    }
}
