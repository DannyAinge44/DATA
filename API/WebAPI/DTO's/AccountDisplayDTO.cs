using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DTO_s
{
    public class AccountDisplayDTO
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public bool IsBalanceAccount { get; set; }
        public bool IsActive { get; set; }
    }
}
