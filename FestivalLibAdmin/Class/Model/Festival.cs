﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;
using Newtonsoft.Json;

namespace FestivalLibAdmin.Model
{
    public class Festival:ObservableValidationObject
    {    

        #region ctors
        static Festival()
        {
            _festival = new Festival();
        }

        public Festival()
        {
            #region temp while dev'ing
            //Stages = new ObservableCollection<Stage>();
            //Bands = new ObservableCollection<Band>();
            //Genres = new ObservableCollection<Genre>();
            //this.Tickets = new ObservableCollection<Ticket>();
            //this.TicketTypes = new ObservableCollection<TicketType>();
            //this.Optredens = new ObservableCollection<Optreden>();
            //ContactPersons = new ObservableCollection<Contactperson>();
            //ContactTypes = new ObservableCollection<ContactpersonType>();
            #endregion
        }

        public Festival(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
            StartDate = Convert.ToDateTime(record["StartDate"]);
            EndDate = Convert.ToDateTime(record["EndDate"]);
            FestivalMap = Convert.IsDBNull(record["Map"]) ? null : record["Map"].ToString();
        }

        #endregion

        #region props

        //dirty check for some things from stack overflow
        public static bool ISASP = Process.GetCurrentProcess().ProcessName == "w3wp" || Process.GetCurrentProcess().ProcessName == "iisexpress";

        private static Festival _festival;

        public static Festival SingleFestival
        {
            get
            {
                if (ISASP&&_festival==null) return Festival.GetFestival();
                return _festival;
            }
            set
            {
                _festival = value;
            }
        }//singleton, accessible from anywhere and obervable

        [JsonIgnore]
        public string ID { get; set; }
        

        private string _name;
        [Required(ErrorMessage = "Gelieve de naam in te vullen")]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van het festival", GroupName = "Festival",Prompt="Bv. Satisfaction")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }
        

        private DateTime _startDate = DateTime.Today;
        [Required(ErrorMessage="Gelieve een startdatum in te geven")]
        [DataType(DataType.Date,ErrorMessage="Gelieve een geldige datum in te geven")]
        [Display(Name = "Start datum", Order = 1, Description ="Datum waarop het festival start", GroupName = "Festival")]
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime _endDate = DateTime.Today.AddDays(1);
        [Required(ErrorMessage = "Gelieve een einddatum in te geven")]
        [DataType(DataType.Date, ErrorMessage = "Gelieve een geldige datum in te geven")]
        [Display(Name = "Eind datum", Order = 2, Description = "Datum waarop het festival eindigt", GroupName = "Festival")]
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        [JsonIgnore]
        public ObservableCollection<DateTime> Days
        {
            get
            {
                ObservableCollection<DateTime> days = new ObservableCollection<DateTime>();
                DateTime currentDay = new DateTime(StartDate.Ticks);
                while (currentDay < EndDate.AddDays(1))
                {
                    days.Add(new DateTime(currentDay.Ticks));
                    currentDay = currentDay.AddDays(1);
                }
                return days;
            }
        }

        private ObservableCollection<LineUp> _lineUps = new ObservableCollection<LineUp>();
        [JsonIgnore]
        public ObservableCollection<LineUp> LineUps
        {
            get
            {
                if(ISASP&&(_lineUps==null||_lineUps.Count()<1))ComputeLineUps();
                return _lineUps;
            }
            private set
            {
                _lineUps = value;
                OnPropertyChanged("LineUps");
            }
        }

        public Festival ComputeLineUps()
        {
            //LineUps.Clear();
            ObservableCollection<DateTime> days = Days;
            foreach (LineUp lineUp in _lineUps.ToList())//to list omdat de originele LineUps gewijzigd worden in de lus
                if (days.IndexOf(lineUp.Dag) == -1) _lineUps.Remove(lineUp);
            foreach (DateTime day in days)
                if (_lineUps.Where(lineUp => lineUp.Dag == day).Count() < 1) _lineUps.Add(new LineUp() { Dag = day });
            //LineUps.ToList().Sort();
            LineUps = new ObservableCollection<LineUp>(_lineUps.OrderBy(lineUp => lineUp.Dag));
            //LineUps.Add(new LineUp() { Dag = day });
            //if (LineUps.Where(lineUp => lineUp.Dag == day).Count() == 0) LineUps.Add(new LineUp() { Dag = day });
            return this;
        }

        //private ObservableCollection<LineUp> _lineUps= new ObservableCollection<LineUp>();

        public void LineUpsPropertyChanged()
        {
            OnPropertyChanged("LineUps");
        }

        private string _festivalMap;
        [RegularExpression(@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?", ErrorMessage = "Gelieve een geldige url te geven")]
        [DataType(DataType.ImageUrl)]
        [Url(ErrorMessage="Gelieve een geldige url in te geven")]
        [FileExtensions(ErrorMessage = "Gelieve een geldige image in te geven (.jpg, .jpeg, .gif or .png)", Extensions = "jpg,jpeg,gif,png")]
        [Display(Name = "Festivalmap", Order = 3, Description = "Een map van het festival", GroupName = "Festival")]
        public string FestivalMap
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
        [JsonIgnore]
        public ObservableCollection<Band> Bands
        {
            get {
                if (_bands == null) Bands = Band.GetBands();
                return _bands; 
                }

            set
            {
                _bands = value;
                OnPropertyChanged("Bands");
            }
        }

        private ObservableCollection<Stage> _stages;
        [JsonIgnore]
        public ObservableCollection<Stage> Stages
        {
            get {
                if (_stages == null) Stages = Stage.GetStages();
                return _stages; }
            set
            {
                _stages = value;
                OnPropertyChanged("Stages");
            }
        }

        private ObservableCollection<ContactpersonType> _contactTypes;
        [JsonIgnore]
        public ObservableCollection<ContactpersonType> ContactTypes
        {
            get {
                if (_contactTypes == null) ContactTypes = ContactpersonType.GetContactTypes();
                return _contactTypes; }
            set
            {
                _contactTypes = value;
                OnPropertyChanged("ContactTypes");
            }
        }

        private ObservableCollection<Contactperson> _contactPersons;
        [JsonIgnore]
        public ObservableCollection<Contactperson> ContactPersons
        {
            get { 
                if(_contactPersons==null)ContactPersons = Contactperson.GetContacts();
                return _contactPersons; }
            set
            {
                _contactPersons = value;
                OnPropertyChanged("ContactPersons");
            }
        }

        private ObservableCollection<Genre> _genres;
        [JsonIgnore]
        public ObservableCollection<Genre> Genres
        {
            get {
                if (_genres == null) Genres = Genre.GetGenres();
                return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        private ObservableCollection<Ticket> _tickets;
        [JsonIgnore]
        public ObservableCollection<Ticket> Tickets
        {
            get {
                if (_tickets == null) Tickets = Ticket.GetTickets();
                return _tickets; }
            set
            {
                _tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        private ObservableCollection<TicketType> _ticketTypes;
        [JsonIgnore]
        public ObservableCollection<TicketType> TicketTypes
        {
            get {
                if (_ticketTypes == null) TicketTypes = TicketType.GetTypes();
                return _ticketTypes; }
            set
            {
                _ticketTypes = value;
                OnPropertyChanged("TicketTypes");
            }
        }

        private ObservableCollection<Optreden> _optredens;
        [JsonIgnore]
        public ObservableCollection<Optreden> Optredens
        {
            get {
                if (_optredens == null) Optredens = Optreden.GetOptredens();
                return _optredens; }
            set
            {
                _optredens = value;
                OnPropertyChanged("Optredens");
            }
        }

        #endregion

        #region dal

        public static Festival GetFestival()
        {
            DbDataReader reader = null;
            try
            {
                //if(Database.ConnectionString.ProviderName==dat van mysql)gebruik limit
                reader = Database.GetData("SELECT TOP 1 * FROM Festival");
                Festival festival=null;
                if (reader.Read())
                    festival = new Festival(reader);
                reader.Close();
                reader = null;
                return festival;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get festival", ex);
            }
        }

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE Festival SET Name=@Name,StartDate=@StartDate,EndDate=@EndDate, Map=@Map WHERE ID=@ID",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@StartDate", StartDate),
                    Database.CreateParameter("@EndDate", EndDate),
                    Database.CreateParameter("@Map",FestivalMap),
                    Database.CreateParameter("@ID", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the festival, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("INSERT INTO Festival (Name, StartDate, EndDate, Map) VALUES(@Name, @StartDate, @EndDate, @Map);SELECT SCOPE_IDENTITY() as 'ID'",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@StartDate", StartDate),
                    Database.CreateParameter("@EndDate", EndDate),
                    Database.CreateParameter("@Map", FestivalMap)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted festival, it is possible the insert failed");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not add festival", ex);
            }
        }

        #endregion
    }
}
