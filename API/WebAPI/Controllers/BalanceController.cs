using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DTO_s;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        AccountingContext context;
        public BalanceController(AccountingContext context)
        {
            this.context = context;
        }
        [HttpGet("{companyName}")]
        public async Task<ActionResult> GetBalanceOfCompany(string companyName)
        {
            var t = context.Companies
                .Where(s => s.Name.ToUpper() == companyName.ToUpper())
                .Include(s => s.Accounts)
                .ThenInclude(s => s.BookingsFrom)
                .Include(s => s.Accounts)
                .ThenInclude(s => s.BookingsTo)
                .ToList()
                .Select(s =>
                new CompanyDisplayDTO
                {
                    CompanyName = s.Name,
                    accounts = s.Accounts.ToList()
                    .Select(s => s.AccountNumber != 28 ?
                    new AccountDisplayDTO
                    {
                        AccountNumber = s.AccountNumber,
                        Balance = s.Balance,
                        Name = s.Name
                    }
                    :
                    new AccountDisplayDTO
                    {
                        Name = s.Name,
                        AccountNumber = s.AccountNumber,
                        Balance = context.Accounts.Include(s => s.BookingsFrom).Include(s => s.BookingsTo)
                        .ToList().Where(s => s.IsBalanceAccount && s.IsActive)
                        .Select(s => s.Balance).Sum() - context.Accounts.Include(s => s.BookingsFrom).Include(s => s.BookingsTo).ToList()
                        .Where(s => s.IsBalanceAccount && !s.IsActive)
                        .Select(s => s.Balance).Sum()
                    })
                    .ToList()
                }
            );
            return Ok(t);
        }
    }
}
