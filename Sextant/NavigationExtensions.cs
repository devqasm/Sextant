using System;
using System.Threading.Tasks;

namespace Sextant
{
	public static class NavigationExtensions
	{
		public static Task<bool> PushAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushPageAsync(pageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushModalPageAsync(pageToPush, animated);
			}

			return Task.FromResult(false);
		}


		public static Task<bool> PopAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopPageAsync(animated);

			return Task.FromResult(false);
		}


		public static Task<bool> PopModalAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopModalPageAsync(animated);

			return Task.FromResult(false);
		}

		public static Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBaseNavigationPage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.RemovePageAsync(pageToRemove);

			return Task.FromResult(false);
		}

		public static Task<bool> PopToRootAsync<TCurrentPageModel>(this TCurrentPageModel currentPageModel, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.PopPagesToRootAsync(animated);

			return Task.FromResult(false);
		}


		public static Task<bool> SetNewRootAndResetAsync<TNewRootPageModel>(this IBaseNavigationPageModel currentPageModel) where TNewRootPageModel : class, IBaseNavigationPageModel
		{
			return SextantCore.Instance.SetNewRootAndResetAsync<TNewRootPageModel>();
		}

		public static Task<bool> PushPageAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currentViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentViewModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TViewModel>();
				var navigationPageToPush = SextantCore.Instance.GetNavigationPage<TPageNavigationModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushPageAsync(navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currentViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentViewModel);

			if (currentPage != null)
			{
                var pageToPush = SextantCore.Instance.GetPage<TViewModel>();
                var navigationPageToPush = SextantCore.Instance.GetNavigationPage<TPageNavigationModel>();

                if (executeOnPageModel != null)
                    pageToPush.ExecuteOnPageModel(executeOnPageModel);

                return SextantCore.Instance.PushModalPageAsync(navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}
	}
}