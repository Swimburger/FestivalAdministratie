﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FestivalLibAdmin
{
    //public class ObservableObject :PortableClassLibrary.ObservableObject
    //{
    //}

    public class ObservableValidationObject : PortableClassLibrary.ObservableObject, IDataErrorInfo
    {
        [ScaffoldColumn(false)]
        [JsonIgnore]
        public string Error
        {
            get { return "Er is een fout gebeurt."; }
        }
        [JsonIgnore]
        [ScaffoldColumn(false)]
        public virtual string this[string propertyName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(propertyName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this) { MemberName = propertyName });
                }
                catch (Exception ex)//nulpointer + validationexception
                {
                    return ex.Message;
                }
                return string.Empty;
            }
        }

        public virtual bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }
    }

    public class ValidationObject : IDataErrorInfo
    {
        [JsonIgnore]
        [ScaffoldColumn(false)]
        public string Error
        {
            get { return "Er is een fout gebeurt."; }
        }
        [JsonIgnore]
        [ScaffoldColumn(false)]
        public string this[string propertyName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(propertyName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this) { MemberName = propertyName });
                }
                catch (Exception ex)//moet nog validation exception worden
                {
                    return ex.Message;
                }
                return string.Empty;
            }
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }
    }

}
