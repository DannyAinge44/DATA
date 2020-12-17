using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebAPI.DTO_s;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public AccountingContext AccountingContext { get; }
        public CompanyController(AccountingContext accountingContext)
        {
            AccountingContext = accountingContext;
        }
        [HttpPost]
        public async Task<ActionResult> CreateCompany(CompanyCreateDTO companyDTO)
        {
            Company company = new Company
            {
                Name = companyDTO.Name,
                Accounts = new List<Account>()
            };
            foreach (var item in AccountsDefaultGet.GetAccounts())
            {
                company.Accounts.Add(new Account { Name = item.Name, AccountNumber = item.AccountNumber, IsBalanceAccount = item.IsBalanceAccount, IsActive = item.IsActive });
            }
            if(companyDTO.AdditionalAccounts?.Count > 0)
            {
                foreach (var item in companyDTO.AdditionalAccounts)
                {
                    company.Accounts.Add(new Account { Name = item.Name, AccountNumber = item.AccountNumber });
                }
            }
            AccountingContext.Companies.Add(company);
            await AccountingContext.SaveChangesAsync();
            return Ok(new { Name = company.Name, Accounts = company.Accounts.Select(s => new { s.IsBalanceAccount, s.IsActive, s.Name }) });
        }
        [HttpGet]
        public async Task<ActionResult<CompanyDisplayDTO>> GetAllCompnanies()
        {
            var t = AccountingContext.Companies
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
                    .Select(s => s.AccountNumber != 28 ? new AccountDisplayDTO { AccountNumber = s.AccountNumber, Balance = s.Balance, Name = s.Name } : new AccountDisplayDTO { Name = s.Name, AccountNumber = s.AccountNumber, Balance = AccountingContext.Accounts.ToList().Where(s => s.IsBalanceAccount && s.IsActive).Select(s => s.Balance).Sum() - AccountingContext.Accounts.ToList().Where(s => s.IsBalanceAccount && !s.IsActive).Select(s => s.Balance).Sum() })
                    .ToList()
                }
            );
            return Ok(t);
        }
        [HttpGet("{companyName}")]
        public async Task<ActionResult> GetCompanyByName(string companyName)
        {
            var t = AccountingContext.Companies
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
                        Balance = AccountingContext.Accounts.Include(s => s.BookingsFrom).Include(s => s.BookingsTo)
                        .ToList().Where(s => s.IsBalanceAccount && s.IsActive)
                        .Select(s => s.Balance).Sum() - AccountingContext.Accounts.Include(s => s.BookingsFrom).Include(s => s.BookingsTo).ToList()
                        .Where(s => s.IsBalanceAccount && !s.IsActive)
                        .Select(s => s.Balance).Sum() })
                    .ToList()
                }
            );
            return Ok(t);
        }
    }
}
