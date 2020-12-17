using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        public AccountingContext Context { get; }
        public BookingController(AccountingContext context)
        {
            Context = context;
        }
        [HttpGet]
        public async Task<ActionResult<BookingDisplayDTO>> GetAllBookings()
        {
            return Ok(Context.Bookings.Include(s => s.AccountFrom).ThenInclude(s => s.Company).Include(s => s.AccountTo)
                .Select(s => new BookingDisplayDTO 
                { 
                    CompanyName = s.AccountFrom.Company.Name,
                    AccountFromName = s.AccountFrom.Name,
                    AccountToName = s.AccountTo.Name,
                    AmountCHF = s.Amount,
                    Message = s.Message,
                    AccountFromNumber = s.AccountFrom.AccountNumber,
                    AccountToNumber = s.AccountTo.AccountNumber
                }));
        }
        [HttpPost]
        public async Task<ActionResult> CreateBooking(BookingCreateDTO bookingDTO)
        {
            Company company = Context.Companies
                .Include(s => s.Accounts)
                .ThenInclude(s => s.BookingsFrom)
                .Include(s => s.Accounts)
                .ThenInclude(s => s.BookingsTo)
                .Where(s => s.Name.ToLower() == bookingDTO.CompanyName.ToLower())
                .FirstOrDefault();
            if(company == null)
            {
                return NotFound("We havent found the specified Company");
            }
            foreach (var item in company.Accounts)
            {
                if(item.BookingsTo == null && item.BookingsFrom == null)
                {
                    item.BookingsTo = new List<Booking>();
                    item.BookingsFrom = new List<Booking>();
                }
            }
            Account accountFrom = company.Accounts.Where(s => s.AccountNumber == bookingDTO.AccountFromNumber).FirstOrDefault();
            Account accountTo = company.Accounts.Where(s => s.AccountNumber == bookingDTO.AccountToNumber).FirstOrDefault();    
            
            if(accountFrom == null || accountTo == null)
            {
                return NotFound("We havent found the specified Accounts");
            }

            int idFrom = accountFrom.AccountNumber;
            int idTo = accountTo.AccountNumber;

            Booking booking = new Booking { AccountFrom = accountFrom, AccountTo = accountTo, Amount = bookingDTO.AmountCHF, Message = bookingDTO.Message };
            Context.Bookings.Add(booking);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
