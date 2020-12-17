using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public Guid AccountFromId { get; set; }
        public Account AccountFrom { get; set; }
        public Guid AccountToId { get; set; }
        public Account AccountTo { get; set; }
    }
}
