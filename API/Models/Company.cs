using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
