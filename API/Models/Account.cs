using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public int AccountNumber { get; set; }
        public bool IsBalanceAccount { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance
        {
            get
            {
                return AccountNumber switch
                {
                    >= 0 and < 20 or >= 30 and <= 3000 => BookingsFrom?.Select(s => s.Amount).Sum() - BookingsTo?.Select(s => s.Amount).Sum() ?? 0,
                    >= 20 and < 30 or > 3000 and <= 6000 => BookingsTo?.Select(s => s.Amount).Sum() - BookingsFrom?.Select(s => s.Amount).Sum() ?? 0
                };
            }
        }        
        public List<Booking> BookingsFrom { get; set; }
        public List<Booking> BookingsTo { get; set; }
    }
}
