using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.ViewModels
{
    public abstract class BaseParamViewModel : GalaSoft.MvvmLight.ObservableObject
    {
        #region HasUpdate 是否有更新，默认为false

        /// <summary>
        /// 在配置参数 ，列表项有变动时，置为true
        /// </summary>
        private bool _hasUpdate = false;

        public bool HasUpdate
        {
            get { return _hasUpdate; }
            set
            {
                _hasUpdate = value;
                this.RaisePropertyChanged(() => HasUpdate);
            }
        }

        #endregion

        public abstract string CreateWhereSqlStr();
    }
}
