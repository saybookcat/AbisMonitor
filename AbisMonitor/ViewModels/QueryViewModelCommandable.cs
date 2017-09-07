using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AbisMonitor.UI.Common.Controls.PagerControl;
using Common;
using Common.Domain;
using Common.Service;
using System.Threading;

namespace AbisMonitor.UI.ViewModels
{
    public abstract class QueryViewModelCommandable<TDbModel, TParamViewModel> : BaseViewModel
        where TDbModel : DbModel, new()
        where TParamViewModel : BaseParamViewModel
    {
        protected QueryDbService<TDbModel> DbService;
        protected ObservableCollection<TDbModel> ItemSource;

        protected QueryViewModelCommandable(TParamViewModel paramViewModel,QueryDbService<TDbModel> dbService)
        {
            ParamViewModel = paramViewModel;
            DbService = dbService;
            PagerControlViewModel = new PagerControlViewModel();
            PagerControlViewModel.LoadPager(Pub.PageLineCount);
        }


        private PagerControlViewModel _pagerControlViewModel;

        public PagerControlViewModel PagerControlViewModel
        {
            get { return _pagerControlViewModel; }
            set
            {
                _pagerControlViewModel = value;
                this.RaisePropertyChanged(() => PagerControlViewModel);
            }
        }

        public TParamViewModel ParamViewModel { get; set; }

        protected virtual void UpdateAction()
        {
            if (ItemSource == null || !ItemSource.Any()) return;
            if (ParamViewModel == null) return;
            this.ParamViewModel.HasUpdate = true;
        }

        private TDbModel _selectedModel;


        public TDbModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                this.RaisePropertyChanged(() => SelectedModel);
            }
        }
    }
}
