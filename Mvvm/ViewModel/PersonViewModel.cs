using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;


namespace Mvvm
{
    [Serializable]
    public class Person : ViewModelBase, IDataErrorInfo
    {
        #region Fields
        private string lastname;
        private DateTime dateofbirth;
        private uint height;
        #endregion

        #region Constructors
        public Person(string LastName, DateTime DateOfBirth, uint Height)
        {
            this.dateofbirth = DateOfBirth;
            this.lastname = LastName;
            this.height = Height;
        }
        public Person(Person p)
        {
            this.dateofbirth = p.DateOfBirth;
            this.lastname = p.LastName;
            this.height = p.Height;
        }
        #endregion

        #region Propeties
        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
                NotifityPropetyChanged("LastName");
            }
        }
        public DateTime DateOfBirth
        {
            get { return dateofbirth; }
            set
            {
                dateofbirth = value;
                NotifityPropetyChanged("DateOfBirth");
                NotifityPropetyChanged("ShortDate");
            }
        }
        public uint Height
        {
            get { return height; }
            set
            {
                height = value;
                NotifityPropetyChanged("Height");
            }
        }
        public string ShortDate
        {
            get { return dateofbirth.ToShortDateString(); }
        }
        #endregion
        public override string ToString()
        {
            return lastname + "\n   birthday:" + dateofbirth.ToShortDateString() + "   height:" + height.ToString();
        }
        public string Error
        {
            get
            {
                return "Error at filling field\n";
            }
        }

        public string this[string propety]
        {
            get
            {
                string ErrorMessage = null;
                switch (propety)
                {
                    case "LastName":
                        if (LastName.Length > 25 || LastName == "")
                            ErrorMessage = "LastName is too long\n";
                        break;
                    case "Height":
                        if (Height > 300 || Height<=0)
                            ErrorMessage = "Error at filling field HEIGHT\n";
                        break;
                    case "DateOfBirth":
                        if (DateTime.Today <= dateofbirth)
                            ErrorMessage = "Error at filling field Date of Bith\n";
                        break;
                    default:
                        break;
                }
                return ErrorMessage;
            }
        }
       
    }
}
