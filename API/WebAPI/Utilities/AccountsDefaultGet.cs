using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DTO_s;

namespace WebAPI.Utilities
{
    public class AccountsDefaultGet
    {
        public static List<AccountDisplayDTO> GetAccounts()
        {
            return new List<AccountDisplayDTO>
            {
                new AccountDisplayDTO
                {
                    Name = "Current Assets",
                    AccountNumber = 10,
                    IsActive = true,
                    IsBalanceAccount = true
                },
                new AccountDisplayDTO
                {
                    Name = "Capital Assets",
                    AccountNumber = 14,
                    IsActive = true,
                    IsBalanceAccount = true
                },
                new AccountDisplayDTO
                {
                    Name = "Current Liabilities",
                    AccountNumber = 20,
                    IsActive = true,
                    IsBalanceAccount = true
                },
                new AccountDisplayDTO
                {
                    Name = "Long-term Liabilities",
                    AccountNumber = 24,
                    IsActive = true,
                    IsBalanceAccount = true
                },
                new AccountDisplayDTO
                {
                    Name = "Equity",
                    AccountNumber = 28,
                    IsBalanceAccount = false
                },
                new AccountDisplayDTO
                {
                    Name = "Equity of Sole Proprietoryship",
                    AccountNumber = 290, 
                    IsBalanceAccount = false
                },
                new AccountDisplayDTO
                {
                    Name = "Sales of Products",
                    AccountNumber = 3000,
                    IsBalanceAccount = false
                },
                new AccountDisplayDTO
                {
                    Name = "Cost of raw materials",
                    AccountNumber = 4000,
                    IsBalanceAccount = false
                },
                new AccountDisplayDTO
                {
                    Name = "Wages and salaries",
                    AccountNumber = 5000,
                    IsBalanceAccount = false
                },
                new AccountDisplayDTO
                {
                    Name = "Rent",
                    AccountNumber = 6000,
                    IsBalanceAccount = false
                }
            };
        }
    }
}
