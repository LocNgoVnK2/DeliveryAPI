using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("DeliveryDetails")]
    public class DeliveryDetail
    {
        [Key]
        public int DeliveryId { get; set; }
        public int? AccountID { get; set; }
        public bool? DeliveryStatus { get; set; }
        public DateTime? TimeReceived { get; set; }
        public byte[]? PickUpPhoto { get; set; }
        public DateTime? TimeComplete { get; set; }
    }
}
