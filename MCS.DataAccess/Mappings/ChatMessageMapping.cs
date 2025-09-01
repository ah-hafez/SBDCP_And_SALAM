using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    class ChatMessageMapping : EntityTypeConfiguration<ChatMessage>
    {
        public ChatMessageMapping()
        {
            HasIndex(p => p.When);
        }
    }
}
