using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DTO_s;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("/info")]
    [ApiController]
    public class InfoAboutAllDefaultAccountsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<AccountDisplayDTO>> GetAllDefaultAccounts()
        {
            return Ok(AccountsDefaultGet.GetAccounts());
        }
    }
}
