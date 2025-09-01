using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Domain
{
    public class UserAccomplishmentReportInfo
    {
        public int TransactionCount { get; set; }
        public int DelayedCount { get; set; }
        public int DecisionCount { get; set; }
        public int WithAppointmentCount { get; set; }
        public int TransPartiesCount { get; set; }
    }
}