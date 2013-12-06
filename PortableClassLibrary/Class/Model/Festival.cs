﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableClassLibrary;

namespace PortableClassLibrary.Model
{
    public class Festival : ObservableObject
    {
        static Festival()
        {
            ////testingdata
            //StartDate = DateTime.Today.AddDays(-1);
            //EndDate = DateTime.Today.AddDays(2);
            //LineUps = new List<LineUp>();
            //for (int i = 0; i < Days.Count; i++)
            //{
            //    LineUps.Add(
            //        new LineUp()
            //        {
            //            Dag = Days[i],
            //            Stages=new List<Stage>{
            //                new Stage(){
            //                    ID=""+i,
            //                    Name="test",
            //                    StageNumber=i
            //                },
            //                new Stage(){
            //                    ID=""+i,
            //                    Name="test",
            //                    StageNumber=i
            //                }
            //            }
            //            });
            //        }
            
            _festival = new Festival();
            //_festival.StartDate = DateTime.Today.AddDays(-1);
            //_festival.EndDate = DateTime.Today.AddDays(2);
        }

        private static Festival _festival;

        public static Festival SingleFestival
        {
            get
            {
                return _festival;
            }
        }

        private DateTime _startDate=DateTime.Today;

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value;
            OnPropertyChanged("StartDate");
            }
        }

        private DateTime _endDate=DateTime.Today.AddDays(1);

        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value;
            OnPropertyChanged("EndDate");
            }
        }

        public virtual ObservableCollection<DateTime> Days
        {
            get
            {
                ObservableCollection<DateTime> days = new ObservableCollection<DateTime>();
                DateTime currentDay = new DateTime(StartDate.Ticks);
                while (currentDay < EndDate.AddDays(1))
                {
                    days.Add(new DateTime(currentDay.Ticks));
                    currentDay=currentDay.AddDays(1);
                }
                return days;
            }
        }

        private ObservableCollection<LineUp> _lineUps= new ObservableCollection<LineUp>();

        public virtual ObservableCollection<LineUp> LineUps
        {
            get
            {
                return _lineUps;
            }
            set
            {
                _lineUps = value;
                OnPropertyChanged("LineUps");
            }
        }

        public void ComputeLineUps()
        {
            //LineUps.Clear();
            ObservableCollection<DateTime> days= Days;
            foreach (LineUp lineUp in LineUps.ToList())//to list omdat de originele LineUps gewijzigd worden in de lus
                if (days.IndexOf(lineUp.Dag) == -1) LineUps.Remove(lineUp);
            foreach (DateTime day in days)
                if (LineUps.Where(lineUp => lineUp.Dag == day).Count() < 1) LineUps.Add(new LineUp() { Dag = day });
            //LineUps.ToList().Sort();
            LineUps = new ObservableCollection<LineUp>(LineUps.OrderBy(lineUp => lineUp.Dag));
                //LineUps.Add(new LineUp() { Dag = day });
                //if (LineUps.Where(lineUp => lineUp.Dag == day).Count() == 0) LineUps.Add(new LineUp() { Dag = day });
        }

        //private ObservableCollection<LineUp> _lineUps= new ObservableCollection<LineUp>();

        public void LineUpsPropertyChanged()
        {
            OnPropertyChanged("LineUps");
        }

        private string _festivalMap;

        public virtual string FestivalMap
        {
            get
            {
                return _festivalMap;
            }
            set
            {
                _festivalMap = value;
                OnPropertyChanged("FestivalMap");
            }
        }


        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set
            {
                _bands = value;
                OnPropertyChanged("Bands");
            }
        }

        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set
            {
                _stages = value;
                OnPropertyChanged("Stages");
            }
        }

        private ObservableCollection<ContactpersonType> _contactTypes;

        public ObservableCollection<ContactpersonType> ContactTypes
        {
            get { return _contactTypes; }
            set
            {
                _contactTypes = value;
                OnPropertyChanged("ContactTypes");
            }
        }

        private ObservableCollection<Contactperson> _contactPersons;

        public ObservableCollection<Contactperson> ContactPersons
        {
            get { return _contactPersons; }
            set
            {
                _contactPersons = value;
                OnPropertyChanged("ContactPersons");
            }
        }

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        private ObservableCollection<Ticket> _tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                _tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        private ObservableCollection<TicketType> _ticketTypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _ticketTypes; }
            set
            {
                _ticketTypes = value;
                OnPropertyChanged("TicketTypes");
            }
        }

        private ObservableCollection<Optreden> _optredens;

        public ObservableCollection<Optreden> Optredens
        {
            get { return _optredens; }
            set
            {
                _optredens = value;
                OnPropertyChanged("Optredens");
            }
        }
        
    }
}