using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ChatRoomOwner: EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        [Column(Order = 0), Key]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual ChatRoom ChatRoom { get; set; }
        [Column(Order = 1), Key]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}
