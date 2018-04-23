using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConsoleApplication {

    //class persisted in DB2
    public class Task : XPLiteObject {
        int fTaskID;
        [Key(true)]
        public int TaskID {
            get { return fTaskID; }
            set { SetPropertyValue("TaskID", ref fTaskID, value); }
        }
        string fSubject;
        public string Subject {
            get { return fSubject; }
            set { SetPropertyValue("Subject", ref fSubject, value); }
        }
        bool fDone;
        public bool Done {
            get { return fDone; }
            set { SetPropertyValue("Done", ref fDone, value); }
        }

        [Browsable(false)]
        public int? ContactID;

        public Contact GetContact(Session foreignSession) {
            return ContactID.HasValue ? foreignSession.GetObjectByKey<Contact>(ContactID) : null;
        }
        public void SetContact(Contact contact) {
            ContactID = (contact == null) ? (int?)null : (int?)contact.ContactID;
        }

        public Task(Session session) : base(session) { }
    }

}
