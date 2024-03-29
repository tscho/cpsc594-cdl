﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MetricAnalyzer.ImporterSystem.Gui
{
        class DisplayMetric : INotifyPropertyChanged 
        {

            public DisplayMetric(Int16 metricID, string metricName, string metricStatus)
            {
                this.Name = metricName;
                this.Id = metricID;
                this.Status = metricStatus;
            }

            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private Int16 id;

            public Int16 Id
            {
                get { return id; }
                set { id = value; }
            }
            private string status;

            public string Status
            {
                get { return status; }
                set { if(status != value)
                        {
                            status = value;
                            if(PropertyChanged != null)
                                PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                        } 
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
}
