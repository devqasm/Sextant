using System;
using System.Threading.Tasks;

namespace Sextant
{
    public interface INavigationService
    {
        IBaseLogger Logger { get; set; }

        IBaseNavigationPage<TNavigationViewModel> GetNavigationPage<TNavigationViewModel>(Action<IBaseViewModel> executeOnPageModel = null, TNavigationViewModel setNavigationPageModel = null) where TNavigationViewModel : class, IBaseNavigationPageModel;
        IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel;
        IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBaseNavigationPageModel;
        TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page) where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBaseNavigationPage<TPageModel> pageToInsert, IBaseNavigationPage<TBeforePageModel> beforePage)
            where TPageModel : class, IBaseNavigationPageModel
            where TBeforePageModel : class, IBaseNavigationPageModel;
        Task<bool> PopModalPageAsync(bool animated = true);
        Task<bool> PopPageAsync(bool animated = true);
        Task<bool> PopPagesToRootAsync(bool animated = true);
        Task<bool> PushModalPageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToPush, bool animated = true) where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> PushPageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToPush, bool animated = true) where TPageModel : class, IBaseNavigationPageModel;
        void RegisterPage<TPage, TPageModel>(Func<TPageModel> createPageModel = null)
            where TPage : class, IBaseNavigationPage<TPageModel>, new()
            where TPageModel : class, IBaseNavigationPageModel, new();
        void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>()
            where TPage : class, IBaseNavigationPage<TPageModel>, new()
            where TPageModel : class, IBaseNavigationPageModel, new()
            where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
            where TNavigationPageModel : class, IBaseNavigationPageModel, new();
        void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>(Func<IBaseNavigationPage<TPageModel>> viewCreationFunc)
            where TPage : class, IBaseNavigationPage<TPageModel>
            where TPageModel : class, IBaseNavigationPageModel, new()
            where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
            where TNavigationPageModel : class, IBaseNavigationPageModel, new();
        Task<bool> RemovePageAsync<TPageModel>(IBaseNavigationPage<TPageModel> pageToRemove) where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModel>(IBaseNavigationPage<TPageModel> newRootPage) where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBaseNavigationPageModel;
        void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBaseNavigationPageModel;
    }
}