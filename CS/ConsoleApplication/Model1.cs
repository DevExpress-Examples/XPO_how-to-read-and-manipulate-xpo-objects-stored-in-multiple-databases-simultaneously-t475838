using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication {

    public class Contact : XPLiteObject {
        int fContactID;
        [Key(true)]
        public int ContactID {
            get { return fContactID; }
            set { SetPropertyValue("ContactID", ref fContactID, value); }
        }
        string fFirstName;
        public string FirstName {
            get { return fFirstName; }
            set { SetPropertyValue("FirstName", ref fFirstName, value); }
        }
        string fLastName;
        public string LastName {
            get { return fLastName; }
            set { SetPropertyValue("LastName", ref fLastName, value); }
        }

        public XPCollection<Task> GetTasks(Session foreignSession) {
            return new XPCollection<Task>(foreignSession, new BinaryOperator("ContactID", this.ContactID));
        }

        public Contact(Session session) : base(session) { }
    }

}
