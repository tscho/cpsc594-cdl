using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Importer_System
{
    class StatusNode : INotifyPropertyChanged
    {
        private String project;
        private String status;

        public event PropertyChangedEventHandler PropertyChanged;

        public StatusNode(String m, String s)
        {
            this.project = m;
            this.status = s;
        }
        public string Project
        {
            get { return project; }
            set
            {
                project = value;
                this.NotifyPropertyChanged("Project");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
