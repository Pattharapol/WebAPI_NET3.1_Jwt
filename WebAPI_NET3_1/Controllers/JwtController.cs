using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_NET3_1.Helpers;

namespace WebAPI_NET3_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {

        [HttpGet]
        public IActionResult Jwt()
        {
            return Ok(new ObjectResult(JwtToken.GenerateJwtToken()));
        }


    }
}
