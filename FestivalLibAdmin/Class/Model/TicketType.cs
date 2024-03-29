﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class TicketType:ObservableValidationObject
    {
        #region ctors
        public TicketType()
        {

        }
        public TicketType(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
            Price = Convert.ToInt32(record["Price"]);
            AmountOfTickets = Convert.ToInt32(record["AmountOfTickets"]);
        }
        #endregion

        #region props

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage="Gelieve een naam in te vullen")]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van het type ticket", GroupName = "Type ticket", Prompt = "Bv. VIP")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }

        private double _price;
        [Required(ErrorMessage="Gelieve een prijs in te geven voor het ticket")]
        [Range(0,double.MaxValue,ErrorMessage="Gelieve een prijs in te geven die niet negatief is")]
        [DataType(DataType.Currency,ErrorMessage="Gelieve een geldige prijs in te geven")]
        [Display(Name = "Prijs", Order = 1, Description = "De prijs van het type ticket", GroupName = "Type ticket",Prompt="Bv. 100")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public double Price
        {
            get { return _price; }
            set { _price = value;
            OnPropertyChanged("Price");
            }
        }

        [Display(Name = "Beschikbare tickets", Order = 3, Description = "Het aantal beschikbare tickets", GroupName = "Type ticket")]
        [Editable(false)]
        public int AvailableTickets
        {
            get { return AmountOfTickets-TicketsSold; }
        }

        private int _ticketsSold=-1;

        public int TicketsSold
        {
            get {
                if (_ticketsSold < 0&&ID!=null) TicketsSold = GetAmountOfSoldTickets();
                return _ticketsSold; }
            set {
                
                _ticketsSold = value;
            OnPropertyChanged("TicketsSold");
            OnPropertyChanged("AvailableTickets");
            }
        }

        private int _amountOfTickets;

        
        [Required(ErrorMessage="Gelieve het aantal tickets in te geven")]
        [Range(1,int.MaxValue,ErrorMessage="Het aantal tickets moet minstens 1 zijn")]
        [Display(Name = "Aantal tickets", Order = 2, Description = "Het aantal tickets", GroupName = "Type ticket",Prompt="Bv. 2000")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int AmountOfTickets
        {
            get { return _amountOfTickets; }
            set { _amountOfTickets = value;
            OnPropertyChanged("AmountOfTickets");
            OnPropertyChanged("AvailableTickets");
            }
        }

        #endregion

        #region dal

        public static ObservableCollection<TicketType> GetTypes()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<TicketType> types = new ObservableCollection<TicketType>();
                reader = Database.GetData("SELECT * FROM TicketTypes ORDER BY Name");
                while (reader.Read())
                    types.Add(new TicketType(reader));
                reader.Close();
                reader = null;
                return types;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get tickettypes", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("INSERT INTO TicketTypes (Name, Price, AmountOfTickets) VALUES(@Name, @Price, @AmountOfTickets); SELECT SCOPE_IDENTITY() as 'ID'",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Price", Price),
                    Database.CreateParameter("@AmountOfTickets",AmountOfTickets)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted user, it is possible the isert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not add user", ex);
            }
        }

        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM TicketTypes WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn ticketType", ex);
            }
        }

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE TicketTypes SET Name=@Name,Price=@Price,AmountOfTickets=@AmountOfTickets WHERE ID=@ID",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Price", Price),
                    Database.CreateParameter("@AmountOfTickets", AmountOfTickets),
                    Database.CreateParameter("@ID",ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the tickettype, me very sorry!", ex);
            }
        }

        public int GetAmountOfSoldTickets()
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("SELECT SUM([Amount]) as TicketsSold FROM Tickets WHERE TicketType=@ID", Database.CreateParameter("@ID", ID));
                int amount = -1;
                if (reader.Read())
                    amount = Convert.IsDBNull(reader["TicketsSold"]) ? 0 : Convert.ToInt32(reader["TicketsSold"]);
                reader.Close();
                reader = null;
                return amount;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get amount of sold tickets", ex);
            }
        }

        #endregion
    }
}
