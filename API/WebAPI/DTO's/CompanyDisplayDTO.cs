using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DTO_s
{
    public class CompanyDisplayDTO
    {
        public string CompanyName { get; set; }
        public List<AccountDisplayDTO> accounts { get; set; }
    }
}
