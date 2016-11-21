using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubledLinkedList;
using Mvvm;
using System.Data.SqlClient;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Started");
            //using (PersonContext db = new PersonContext())
            //{
            //    Person p1 = new Person("Sidorov", new DateTime(1996, 01, 03), 179);
            //    Person p2 = new Person("Ivanov", new DateTime(1995, 03, 03), 170);
            //    Console.WriteLine("Elements created");
            //    db.Persons.Add(p1);
            //    db.Persons.Add(p2);
            //    db.SaveChanges();

            //    Console.WriteLine("Объекты успешно сохранены");

            //    // получаем объекты из бд и выводим на консоль
            //    var persons = db.Persons;
            //    Console.WriteLine("Список объектов:");
            //    foreach (Person p in persons)
            //    {
            //        Console.WriteLine("{0} | {1} | {2}", p.LastName, p.DateOfBirth, p.Height);
            //    }
            //}
            //Console.ReadLine();
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + 
                                       "Trusted_Connection=yes;" +
                                       "database=TrainingDatabase; ");
            conn.Open();
            try
            {
                var cmd = new SqlCommand("SELECT * FROM FirstTable", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var obj = reader["LastName"];
                    Console.WriteLine("{0} {1} {2}", reader["LastName"],reader["DateOfBirth"], reader["Height"]);
                }
            }
            finally
            {
                conn.Close();
            }
            Console.ReadLine();
        }
    }
}
