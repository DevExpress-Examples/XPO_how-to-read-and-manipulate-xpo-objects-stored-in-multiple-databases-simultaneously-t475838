using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication {
    class Program {

        static IDataLayer DB1dal;
        static IDataLayer DB2dal;

        static void Main(string[] args) {
            //Setup data layers
            DB1dal = SetupDB1();
            DB2dal = SetupDB2();

            //connect DB1
            using (UnitOfWork session1 = new UnitOfWork(DB1dal)) {
                Contact contact = session1.FindObject<Contact>(CriteriaOperator.Parse("LastName = ?", "Smith"));
                //connect DB2
                using (UnitOfWork session2 = new UnitOfWork(DB2dal)) {
                    Task task = new Task(session2);
                    task.Subject = "Improve";
                    task.SetContact(contact);
                    session2.CommitChanges();
                }
            }

            //check 1
            using (UnitOfWork session1 = new UnitOfWork(DB1dal)) {
                Contact contact = session1.FindObject<Contact>(CriteriaOperator.Parse("LastName = ?", "Smith"));
                //connect DB2
                using (UnitOfWork session2 = new UnitOfWork(DB2dal)) {
                    var tasks = contact.GetTasks(session2);
                    foreach (var task in tasks) {
                        Console.WriteLine(task.Subject);
                    }
                }
            }

            Console.WriteLine();

            //check 2
            //connect DB2
            using (UnitOfWork session2 = new UnitOfWork(DB2dal)) {
                var task = session2.FindObject<Task>(CriteriaOperator.Parse("Subject = ?", "Improve"));
                using (UnitOfWork session1 = new UnitOfWork(DB1dal)) {
                    Contact contact = task.GetContact(session1);
                    Console.WriteLine("{0} {1}", contact.FirstName, contact.LastName);
                }
            }

            Console.ReadLine();
        }

        static IDataLayer SetupDB1() {
            string connString = MSSqlConnectionProvider.GetConnectionString("(local)", "SampleDB1");
            XPDictionary dict = new ReflectionDictionary();
            dict.CollectClassInfos(typeof(Contact));
            IDataLayer dal = XpoDefault.GetDataLayer(connString, dict, AutoCreateOption.DatabaseAndSchema);
            using (UnitOfWork uow = new UnitOfWork(dal)) {
                uow.UpdateSchema(typeof(Contact));
                if (uow.FindObject<Contact>(null) == null) {
                    Contact contact1 = new Contact(uow) { FirstName = "John", LastName = "Doe" };
                    Contact contact2 = new Contact(uow) { FirstName = "Paul", LastName = "Smith" };
                    uow.CommitChanges();
                }
            }
            return dal;
        }

        static IDataLayer SetupDB2() {
            string connString = MSSqlConnectionProvider.GetConnectionString("(local)", "SampleDB2");
            XPDictionary dict = new ReflectionDictionary();
            dict.CollectClassInfos(typeof(Task));
            IDataLayer dal = XpoDefault.GetDataLayer(connString, dict, AutoCreateOption.DatabaseAndSchema);
            using (UnitOfWork uow = new UnitOfWork(dal)) {
                uow.UpdateSchema(typeof(Task));
            }
            return dal;
        }
    }
}
