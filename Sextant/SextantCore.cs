using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
	public class SextantCore : INavigationService
	{
		static INavigationService _instance;
		public static INavigationService Instance => _instance ?? (_instance = new SextantCore());

		IBaseLogger _logger;
		public IBaseLogger Logger
		{
			get
			{
				if (_logger == null)
					_logger = new BaseLogger();

				return _logger;
			}

			set
			{
				_logger = value;
			}
		}

		readonly IList<NavigationElement> _navigationDeck = new List<NavigationElement>();

		INavigation FormsNavigation
		{
			get
			{
				var tabController = Application.Current.MainPage as TabbedPage;
				var masterController = Application.Current.MainPage as MasterDetailPage;

				// First check to see if we're on a tabbed page, then master detail, finally go to overall fallback
				return tabController?.CurrentPage?.Navigation ??
									 (masterController?.Detail as TabbedPage)?.CurrentPage?.Navigation ?? // special consideration for a tabbed page inside master/detail
									 masterController?.Detail?.Navigation ??
									 Application.Current.MainPage.Navigation;
			}
		}

		public void RegisterPage<TPage, TPageModel>(Func<TPageModel> createPageModel = null)
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
		{
			_navigationDeck.Add(new NavigationElement(typeof(TPage), typeof(TPageModel), createPageModel));

			Locator.CurrentMutable.Register(() => new TPage(), typeof(TPage));
			Locator.CurrentMutable.Register(() => new TPageModel(), typeof(TPageModel));
		}

		public void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>()
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
			where TNavigationPageModel : class, IBaseNavigationPageModel, new()
		{
			_navigationDeck.Add(new NavigationElement(typeof(TPage), typeof(TPageModel), typeof(TNavigationPage), typeof(TNavigationPageModel)));


			var navCreation = new Func<IBaseNavigationPage<IBaseNavigationPageModel>, IBaseNavigationPage<TNavigationPageModel>>(
				(page) => Activator.CreateInstance(typeof(TNavigationPage), page) as IBaseNavigationPage<TNavigationPageModel>);

			var createView = new Func<IBaseNavigationPageModel, IBaseNavigationPage<TPageModel>>((vm) =>
			{
				var v = Activator.CreateInstance(typeof(TPage)) as IBaseNavigationPage<TPageModel>;
				v.SetPageModel(vm);
				return v;
			});

			Locator.CurrentMutable.Register(() => new TPageModel(), typeof(TPageModel));
			Locator.CurrentMutable.Register(() => createView(Locator.Current.GetService<TPageModel>()), typeof(TPage));

			Locator.CurrentMutable.Register(() => navCreation(Locator.Current.GetService<TPage>()), typeof(TNavigationPage));
			Locator.CurrentMutable.Register(() => new TNavigationPageModel(), typeof(TNavigationPageModel));
		}

		public void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>(Func<IBaseNavigationPage<TPageModel>> viewCreationFunc)
			where TPage : class, IBaseNavigationPage<TPageModel>
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
			where TNavigationPageModel : class, IBaseNavigationPageModel, new()
		{
			_navigationDeck.Add(new NavigationElement(typeof(TPage), typeof(TPageModel), typeof(TNavigationPage), typeof(TNavigationPageModel), viewCreationFunc));


			var navCreation = new Func<IBaseNavigationPage<IBaseNavigationPageModel>, IBaseNavigationPage<TNavigationPageModel>>(
				(page) => Activator.CreateInstance(typeof(TNavigationPage), page) as IBaseNavigationPage<TNavigationPageModel>);

			var createView = new Func<IBaseNavigationPageModel, IBaseNavigationPage<TPageModel>>((vm) =>
			{
				var v = viewCreationFunc() as IBaseNavigationPage<TPageModel>;
				v.SetPageModel(vm);
				return v;
			});

			Locator.CurrentMutable.Register(() => new TPageModel(), typeof(TPageModel));
			Locator.CurrentMutable.Register(() => createView(Locator.Current.GetService<TPageModel>()), typeof(TPage));

			Locator.CurrentMutable.Register(() => navCreation(Locator.Current.GetService<IBaseNavigationPage<TPageModel>>()), typeof(TNavigationPage));
			Locator.CurrentMutable.Register(() => new TNavigationPageModel(), typeof(TNavigationPageModel));
		}

		public IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.ViewModelType == typeof(TPageModel));
			IBaseNavigationPage<TPageModel> page;
			IBaseNavigationPageModel pageModel;

			page = GetView<TPageModel>(navigationElement.ViewType);

			if (setPageModel != null)
			{
				SetPageModel(page, setPageModel);
			}
			else
			{
				pageModel = GetViewModel<TPageModel>(navigationElement.ViewModelType);
				SetPageModel(page, pageModel);
			}

			return page;
		}

		public IBaseNavigationPage<TNavigationViewModel> GetNavigationPage<TNavigationViewModel>(
			Action<IBaseViewModel> executeOnPageModel = null,
			TNavigationViewModel setNavigationPageModel = null)
			where TNavigationViewModel : class, IBaseNavigationPageModel
		{
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.NavigationViewModelType == typeof(TNavigationViewModel));
			IBaseNavigationPage<TNavigationViewModel> navigationPage;

			navigationPage = GetView<TNavigationViewModel>(navigationElement.NavigationViewType);

			if (setNavigationPageModel != null)
			{
				SetPageModel(navigationPage, setNavigationPageModel);
			}
			else
			{
				var navigationPageModel = GetViewModel<TNavigationViewModel>(navigationElement.NavigationViewModelType);
				SetPageModel(navigationPage, navigationPageModel);
			}

			return navigationPage;
		}

		public IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var element = _navigationDeck.FirstOrDefault(pt => pt.ViewModelType == pageModel.GetType());
			var page = GetView<TPageModel>(element.ViewType);
			return page;
		}

		public async Task<bool> PushPageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToPush, bool animated = true)
			where TPageModel : class, IBaseNavigationPageModel
		{
			await FormsNavigation.PushAsync((Page)pageToPush, animated);

			return true;
		}

		public async Task<bool> PushModalPageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToPush, bool animated = true)
			where TPageModel : class, IBaseNavigationPageModel
		{
			await FormsNavigation.PushModalAsync((Page)pageToPush, animated);
			return true;
		}

		public Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBaseNavigationPage<TPageModel> pageToInsert, IBaseNavigationPage<TBeforePageModel> beforePage)
			where TPageModel : class, IBaseNavigationPageModel where TBeforePageModel : class, IBaseNavigationPageModel
		{
			FormsNavigation.InsertPageBefore((Page)pageToInsert, (Page)beforePage);
			return Task.FromResult(true);
		}

		public async Task<bool> PopPageAsync(bool animated = true)
		{
			await FormsNavigation.PopAsync(animated);
			return true;
		}

		public async Task<bool> PopModalPageAsync(bool animated = true)
		{
			await FormsNavigation.PopModalAsync(animated);
			return true;
		}

		public Task<bool> RemovePageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToRemove)
			where TPageModel : class, IBaseNavigationPageModel
		{
			FormsNavigation.RemovePage((Page)pageToRemove);
			return Task.FromResult(true);
		}

		public async Task<bool> PopPagesToRootAsync(bool animated = true)
		{
			await FormsNavigation.PopToRootAsync(animated);
			return true;
		}

		public Task<bool> SetNewRootAndResetAsync<TPageModel>(IBaseNavigationPage<TPageModel> newRootPage) where TPageModel : class, IBaseNavigationPageModel
		{
			Application.Current.MainPage = (Page)newRootPage;

			return Task.FromResult(true);
		}

		public Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBaseNavigationPageModel
		{
			//TODO: Find a better way to distinguish between navigation page VMs and page VMs instead of the below try/catch block

			Page page = null;
			try
			{
				page = (Page)GetPage<TPageModelOfNewRoot>();
			}
			catch (NullReferenceException)
			{
				page = (Page)GetNavigationPage<TPageModelOfNewRoot>();
			}
			catch (Exception)
			{
				throw;
			}

			Application.Current.MainPage = page;

			return Task.FromResult(true);
		}

		public TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var xfPage = page as Page;
			return xfPage?.BindingContext as TPageModel;
		}

		public void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var xfPage = (Page)page;
			xfPage.BindingContext = newPageModel;
		}

		private static IBaseNavigationPageModel GetViewModel<TPageModel>(Type viewModelType) where TPageModel : class, IBaseNavigationPageModel
		{
			var pageModel = Locator.Current.GetService(viewModelType) as TPageModel;
			if (pageModel == null)
			{
				throw new NoPageForPageModelRegisteredException("ViewModel not registered in IOC: " + viewModelType.Name);
			}

			return pageModel;
		}

		private static IBaseNavigationPage<TPageModel> GetView<TPageModel>(Type viewType) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = Locator.Current.GetService(viewType) as IBaseNavigationPage<TPageModel>;
			if (page == null)
			{
				throw new NoPageForPageModelRegisteredException($"View for ViewModel '{typeof(TPageModel)}' not registered in IOC");
			}

			return page;
		}
	}
}