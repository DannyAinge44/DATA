using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DTO_s
{
    public class BookingDisplayDTO
    {
        public string CompanyName { get; set; }
        public string Message { get; set; }
        public string AccountFromName { get; set; }
        public int AccountFromNumber { get; set; }
        public string AccountToName { get; set; }
        public int AccountToNumber { get; set; }
        public decimal AmountCHF { get; set; }
    }
}
