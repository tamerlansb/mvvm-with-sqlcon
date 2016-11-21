using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubledLinkedList;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows;

namespace Mvvm
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private DoubledLinkedList<Person> people;
        private Person newPerson, currPerson;
        #endregion

        #region Constructor
        public MainViewModel()
        {
            people = new DoubledLinkedList<Person>();
            DownloadFromDateBaseF();
            //newPerson = new Person("Sidorov", new DateTime(1996, 01, 03), 179); this.AddPerson();
            //newPerson = new Person("Ivanov", new DateTime(1995, 03, 03), 170); this.AddPerson();
            //newPerson = new Person("Petrov", new DateTime(1996, 02, 03), 172); this.AddPerson();
            //newPerson = new Person("Sadovnichyi", new DateTime(1996, 12, 12), 178); this.AddPerson();
            newPerson = new Person("aaa", new DateTime(2001,01,01), 0);
        }
        #endregion

        #region Properties
        public DoubledLinkedList<Person> People
        {
            get { return people; }
            private set { people = value;  }
        }
        public Person NewPerson
        {
            get { return newPerson; }
            set {
                newPerson = value;
                NotifityPropetyChanged("NewPerson");
            }
        }
        public Person CurrentPerson
        {
            get { return currPerson; }
            set
            {
                currPerson = value;
                NotifityPropetyChanged("CurrentPerson");
            }
        }
        public uint IndexForAdd
        {
            get; set;
        }
        #endregion

        #region Commands
     
        #region SaveToDateBase
        private DelegateCommand saveToDateBase;

        public ICommand SaveToDateBase
        {
            get
            {
                if (saveToDateBase == null)
                    saveToDateBase = new DelegateCommand(SaveToDateBaseF);
                return saveToDateBase;
            }
        }

        private void SaveToDateBaseF()
        {
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " +
                                       "Trusted_Connection=yes;" +
                                       "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO FirstTable (PersonID,LastName, DateOfBirth,Height) VALUES(@PersonID,@LastName, @DateOfBirth,@Height)", conn);
                foreach (Person p in people)
                {
                    cmd.Parameters.Add(new SqlParameter("PersonID", p.GetHashCode()));
                    cmd.Parameters.Add(new SqlParameter("LastName", p.LastName));
                    cmd.Parameters.Add(new SqlParameter("DateOfBirth", p.DateOfBirth));
                    cmd.Parameters.Add(new SqlParameter("Height", (int)p.Height));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region DownloadFromDateBase
        private DelegateCommand downloadFromDateBase;
        public ICommand DownloadFromDateBase
        {
            get
            {
                if (downloadFromDateBase == null)
                {
                    downloadFromDateBase = new DelegateCommand(DownloadFromDateBaseF);
                }
                return downloadFromDateBase;
            }
        }
        private void DownloadFromDateBaseF()
        {
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                string ID = null;
                conn.Open();
                var cmdFindFirst = new SqlCommand("SELECT * FROM FirstTable WHERE PrevID IS NULL", conn);
                var reader = cmdFindFirst.ExecuteReader(); reader.Read();
                ID = reader.HasRows ? reader["PersonID"].ToString() : null;
                if (ID!=null)
                {
                    Person temp = new Person(reader["LastName"].ToString(), DateTime.Parse(reader["DateOfBirth"].ToString()),uint.Parse( reader["Height"].ToString()) );
                    people.Add(temp);
                }
                do
                {
                    conn.Close();
                    conn.Open();
                    var cmd = new SqlCommand("SELECT * FROM FirstTable WHERE PrevID=" + ID, conn);
                    reader = cmd.ExecuteReader(); reader.Read();
                    ID = reader.HasRows ? reader["PersonID"].ToString() : null;
                    if (ID != null)
                    {
                        Person temp = new Person(reader["LastName"].ToString(), DateTime.Parse(reader["DateOfBirth"].ToString()), uint.Parse(reader["Height"].ToString()));
                        people.Add(temp);
                    }

                } while (ID != null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region ClearList
        private DelegateCommand clearListCom;
        public ICommand ClearListCom
        {
            get
            {
                if (clearListCom == null)
                {
                    clearListCom = new DelegateCommand(ClearListExecute, ClearListCanExecute);
                }
                return clearListCom;
            }
        }
        public void ClearListExecute()
        {
            people.ClearList();
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmd = new SqlCommand("TRUNCATE TABLE FirstTable", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool ClearListCanExecute()
        {
            return people.Count > 0;
        }
        #endregion

        #region Sort By LastName
        private DelegateCommand sortByLastName;
        public ICommand SortByLastName
        {
            get
            {
                if (sortByLastName == null)
                {
                    sortByLastName = new DelegateCommand(SortExecute, SortCanExecute);
                }
                return sortByLastName;
            }
        } 
        public void SortExecute()
        {
            people.SortByPred((Person x, Person y) => { return x.LastName.CompareTo(y.LastName); });

            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmd = new SqlCommand("TRUNCATE TABLE FirstTable", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            foreach (Person p in people)
                AddToServer(p);
        }
        public bool SortCanExecute()
        {
            return people.Count > 0;
        }
        #endregion

        #region Delete
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new DelegateCommand(DeletePerson,CanExecuteDelete);
                }
                return deleteCommand;
            }
        }
        private void DeletePerson()
        {
            if (CurrentPerson != null)
            {
                people.RemoveByPredicate((Person x) => {return x == CurrentPerson; });
                var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand("TRUNCATE TABLE FirstTable", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                foreach (Person p in people)
                    AddToServer(p);
            }
            CurrentPerson = null;
        }
        private bool CanExecuteDelete()
        {
            return people.Count > 0 && CurrentPerson != null;
        }
        #endregion

        #region Add By Index
        private DelegateCommand addCommandByIndex;
        public ICommand AddCommandByIndex
        {
            get
            {
                if (addCommandByIndex == null)
                {
                    addCommandByIndex = new DelegateCommand(AddPersonByIndex, CanExecuteAddByIndex);
                }
                return addCommandByIndex;
            }
        }
        private void AddPersonByIndex()
        {
            string ID, prID = "";
            people.AddByIndex(newPerson,IndexForAdd);
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmdFindFirst = new SqlCommand("SELECT * FROM FirstTable WHERE PrevID IS NULL", conn);
                var reader = cmdFindFirst.ExecuteReader(); reader.Read();
                ID = reader.HasRows ? reader["PersonID"].ToString() : null;
                for (int i = 1; i < IndexForAdd; ++i)
                {
                    conn.Close();
                    conn.Open();
                    var cmd = new SqlCommand("SELECT * FROM FirstTable WHERE PrevID=" + ID, conn);
                    reader = cmd.ExecuteReader(); reader.Read();
                    ID = reader.HasRows ? reader["PersonID"].ToString() : null;
                    prID = reader["PrevID"].ToString();
                }
                conn.Close();
                conn.Open();
                if (IndexForAdd == 1)
                {
                    var cmd = new SqlCommand("INSERT INTO FirstTable (LastName,DateOfBirth,Height,NextID,PrevID,IsHead) VALUES(@lastName, @dateOfBirth,@height,@next ,@prev,@ishead)", conn);
                    cmd.Parameters.Add(new SqlParameter("lastName", newPerson.LastName));
                    cmd.Parameters.Add(new SqlParameter("dateOfBirth", newPerson.DateOfBirth));
                    cmd.Parameters.Add(new SqlParameter("height", (int)newPerson.Height));
                    cmd.Parameters.Add(new SqlParameter("next", ID));
                    cmd.Parameters.Add(new SqlParameter("prev", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("ishead", 1));
                    cmd.ExecuteNonQuery();
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("SELECT PersonID FROM FirstTable WHERE IsHead=1 AND NOT PersonID="+ID, conn);
                    reader = cmd.ExecuteReader(); reader.Read();
                    var NewHeadID = reader["PersonID"];
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("UPDATE FirstTable SET PrevID=" + NewHeadID.ToString() +", IsHead=0" + " " + " WHERE PersonID=" + ID, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var cmd = new SqlCommand("INSERT INTO FirstTable (LastName,DateOfBirth,Height,NextID,PrevID,IsHead) VALUES(@lastName, @dateOfBirth,@height,@next ,@prev,@ishead)", conn);
                    cmd.Parameters.Add(new SqlParameter("lastName", newPerson.LastName));
                    cmd.Parameters.Add(new SqlParameter("dateOfBirth", newPerson.DateOfBirth));
                    cmd.Parameters.Add(new SqlParameter("height", (int)newPerson.Height));
                    cmd.Parameters.Add(new SqlParameter("next", ID));
                    cmd.Parameters.Add(new SqlParameter("prev", prID));
                    cmd.Parameters.Add(new SqlParameter("ishead",  false));
                    cmd.ExecuteNonQuery();
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("SELECT PersonID FROM FirstTable WHERE NextID="+ID + " AND PrevID="+prID, conn);
                    reader = cmd.ExecuteReader(); reader.Read();
                    var NewHeadID = reader["PersonID"];
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("UPDATE FirstTable SET PrevID=" + NewHeadID.ToString() + " " + " WHERE PersonID=" + ID, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("UPDATE FirstTable SET NextID=" + NewHeadID.ToString() + " " + " WHERE PersonID=" + prID, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            NewPerson = new Person("", new DateTime(2001,01,01), 0);
        }
        private bool CanExecuteAddByIndex()
        {
            return CanExecuteAdd() && IndexForAdd > 0 && IndexForAdd <= people.Count;
        }
        #endregion

        #region Add
        private DelegateCommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new DelegateCommand(AddPerson,CanExecuteAdd);
                }
                return addCommand;
            }
        }
        private void AddPerson()
        {
            people.Add(newPerson);
            AddToServer(newPerson);
            NewPerson = new Person("", new DateTime(2001, 01, 01), 0);    
        }
        private bool CanExecuteAdd()
        {
            bool flag;
            flag = newPerson.DateOfBirth <= DateTime.Today && newPerson.LastName != "" && newPerson.LastName.Length <= 25 && NewPerson.Height > 0 && newPerson.Height < 300;
            return flag;
        }
        #endregion

        #endregion

        private void AddToServer(Person p)
        {
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmdFindLast = new SqlCommand("SELECT PersonId FROM FirstTable WHERE NextID IS NULL", conn);
                var reader = cmdFindLast.ExecuteReader(); reader.Read();
                var ID = reader.HasRows ? reader["PersonID"].ToString() : null;

                conn.Close(); conn.Open();
                var cmd = new SqlCommand("INSERT INTO FirstTable (LastName,DateOfBirth,Height,NextID,PrevID,IsHead) VALUES(@lastName, @dateOfBirth,@height,@next ,@prev,@ishead)", conn);
                cmd.Parameters.Add(new SqlParameter("lastName", p.LastName));
                cmd.Parameters.Add(new SqlParameter("dateOfBirth", p.DateOfBirth));
                cmd.Parameters.Add(new SqlParameter("height", (int)p.Height));
                cmd.Parameters.Add(new SqlParameter("next", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("prev", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("ishead", ID == null ? 1 : 0));
                cmd.ExecuteNonQuery();

                if (ID != null)
                {
                    conn.Close(); conn.Open();
                    cmdFindLast = new SqlCommand("SELECT PersonId FROM FirstTable WHERE NextID IS NULL AND PrevID IS NULL AND IsHead='False' ", conn);
                    reader = cmdFindLast.ExecuteReader(); reader.Read();
                    var LastID = reader["PersonID"];
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("UPDATE FirstTable SET PrevID=" + ID.ToString() + " where PersonID=" + LastID.ToString(), conn);
                    cmd.ExecuteNonQuery();
                    conn.Close(); conn.Open();
                    cmd = new SqlCommand("UPDATE FirstTable SET NextID=" + LastID.ToString() + " where PersonID=" + ID.ToString(), conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
