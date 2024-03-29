﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalAdministratie.Model
{
    public class Genre:ObservableValidationObject
    {
        static Genre()
        {
            Genres = new ObservableCollection<Genre>();
        }

        private static ObservableCollection<Genre> _genres;

        public static ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }
        

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
    }
}
