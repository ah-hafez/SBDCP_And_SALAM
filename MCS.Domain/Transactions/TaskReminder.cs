using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TaskReminder : EntityBase
    {
        public virtual Task Task { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
    }
}
