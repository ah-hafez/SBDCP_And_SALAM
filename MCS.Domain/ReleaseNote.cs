using MCS.Framework.Entities;
using System;

namespace MCS.Domain
{
    public class ReleaseNote : EntityBase
    {
        public string ReleaseNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DateHj { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
