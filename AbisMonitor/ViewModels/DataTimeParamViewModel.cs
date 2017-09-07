using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AbisMonitor.UI.Common.Exceptions;
using Common.Domain;
using Framework.CustomException;
using GalaSoft.MvvmLight.Command;

namespace AbisMonitor.UI.ViewModels
{
    public abstract class DataTimeParamViewModel : BaseParamViewModel
    {
         private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today.AddDays(1.0);
          int StartTime=0;
        public DataTimeParamViewModel()
        {
          
            StartDate = StartDate.AddSeconds(StartTime);
            EndDate = EndDate.AddSeconds(StartTime);

            if (StartDate > DateTime.Now)
            {
                StartDate = StartDate.AddDays(-1);
                EndDate = EndDate.AddDays(-1);
            }

        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }

        private void SetTimeToday()
        {
            StartDate = DateTime.Now.Date.AddSeconds(StartTime);
            EndDate = DateTime.Now.AddDays(1).Date.AddSeconds(StartTime);
        }

        private void SetTimeYesterday()
        {
            StartDate = DateTime.Now.AddDays(-1).Date.AddSeconds(StartTime);
            EndDate = DateTime.Now.Date.AddSeconds(StartTime);
        }


        private void SetTimeBeforeYesterday()
        {
            StartDate = DateTime.Now.AddDays(-2).Date.AddSeconds(StartTime);
            EndDate = DateTime.Now.AddDays(-1).Date.AddSeconds(StartTime);
        }


        private void SetThisWeek()
        {
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            StartDate =
                DateTime.Now.AddDays(-dayOfWeek + 1).Date.AddSeconds(StartTime);
            EndDate =
                DateTime.Now.AddDays(7 - dayOfWeek + 1).Date.AddSeconds(StartTime);
        }


        private void SetLastWeek()
        {
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            StartDate =
                DateTime.Now.AddDays(-dayOfWeek - 7 + 1).Date.AddSeconds(StartTime);
            EndDate =
                DateTime.Now.AddDays(-dayOfWeek + 1).Date.AddSeconds(StartTime);
        }

        private void SetPreviouDay()
        {
            StartDate = StartDate.AddDays(-1);
            EndDate = EndDate.AddDays(-1);
        }


        private void SetAfterDay()
        {
            StartDate = StartDate.AddDays(1);
            EndDate = EndDate.AddDays(1);
        }


        private int FirstDay = -25;
        private int FirstDayTime = 64800;
        private void SetThisMonth()
        {
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Math.Abs(FirstDay)).AddSeconds(FirstDayTime);
            if (FirstDay < 0)
            {
                StartDate = StartDate.AddMonths(-1);
            }

            EndDate = StartDate.AddMonths(1);
        }

        private void SetLastMonth()
        {
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Math.Abs(FirstDay)).AddSeconds(FirstDayTime).AddMonths(-1);
            if (FirstDay < 0)
            {
                StartDate = StartDate.AddMonths(-1);
            }

            EndDate = StartDate.AddMonths(1);
        }



        public ICommand TimeSelectedCommand
        {
            get { return new RelayCommand<int>(SetTimeSelected); }
        }

        private void SetTimeSelected(int code)
        {
            switch (code)
            {
                case 0:
                    SetTimeToday();
                    break;
                case 1:
                    SetTimeYesterday();
                    break;
                case 2:
                    SetTimeBeforeYesterday();
                    break;
                case 3:
                    SetThisWeek();
                    break;
                case 4:
                    SetLastWeek();
                    break;
                case 5:
                    SetThisMonth();
                    break;
                case 6:
                    SetLastMonth();
                    break;
                case 7:
                    SetPreviouDay();
                    break;
                case 8:
                    SetAfterDay();
                    break;
            }
        }



        public virtual void Validate(bool isMonthly = false)
        {
            if (StartDate.CompareTo(EndDate) >= 0)
            {
                throw new TException<ValidateExceptionArgs>("起始时间不能大于或等于结束时间。");
            }
            if (!isMonthly)
            {
                if (StartDate.CompareTo(DateTime.Now) > 0)
                {
                    throw new TException<ValidateExceptionArgs>("起始时间不能大于当前时间。");
                }
            }

            if ((EndDate - StartDate).TotalDays > 90 + 3)
            {
                throw new TException<ValidateExceptionArgs>(new ValidateExceptionArgs(),
                    string.Format("查询时间间隔不能大于{0}天。", 90));
            }
        }

    }
}
