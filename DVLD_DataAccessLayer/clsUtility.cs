using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataBaseTier
{
    internal class clsUtility
    {
        static public void WirteExceptionInEventLog(string Message, EventLogEntryType eventLogtype)
        {
            string SourceName = "DVLDapp";

            if (!EventLog.Exists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }

            EventLog.WriteEntry(SourceName, "This is an Error event In System.", eventLogtype); 

        }
    }
}
