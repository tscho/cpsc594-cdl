using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Importer_System.Gui
{
        class DisplayMetric
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
                set { status = value; }
            }
        }
}
