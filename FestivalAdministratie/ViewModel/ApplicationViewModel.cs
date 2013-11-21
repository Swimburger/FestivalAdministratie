﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class ApplicationViewModel:ObservableObject
    {
        static ApplicationViewModel()
        {
            ViewModel = new ApplicationViewModel();
        }
        private static ApplicationViewModel _viewModel;
        public static ApplicationViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            private set { _viewModel = value; }
        }

        public ApplicationViewModel()
        {
            Pages = new List<IPage>();
            Pages.Add(new LineUpViewModel());
            Pages.Add(new MapViewModel());
            Pages.Add(new TicketsViewModel());
            Pages.Add(new ContactenViewModel());
            currentPage = Pages[0];
        }

        private List<IPage> _pages;

        public List<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<IPage>();
                return _pages;
            }
            set { _pages = value;
            OnPropertyChanged("Pages");
            }
        }

        private IPage currentPage;

        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value;
            OnPropertyChanged("CurrentPage");
            }
        }

        //private ObservableCollection<T> _list;

        //public ObservableCollection<T> List
        //{
        //    get { return _list; }
        //    set { _list = value; }
        //}
        
    }
}