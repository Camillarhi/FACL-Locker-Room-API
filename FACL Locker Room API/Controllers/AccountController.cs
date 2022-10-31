using FACL_Locker_Room_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FACL_Locker_Room_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        //get current app version
        [HttpGet("GetCurrentVersion")]
        public ActionResult GetCurrentVersion()
        {
            var appVersion = _configuration.GetSection("AppVersion").Value;
            return Ok(new { appVersion });
        }

        //get account
        [HttpGet("GetAccount")]
        public ActionResult<AccountDto> GetAccount(string firstname, string lastname)
        {
            var filepath = FilePath(firstname, lastname);
            if (!System.IO.File.Exists(filepath))
            {
                return BadRequest("Account doesn't exist");
            }

            //read file content from file path
            var jsonText = System.IO.File.ReadAllText(filepath);

            //deserialize json text 
            var result = JsonConvert.DeserializeObject<AccountDto>(jsonText);
            return Ok(result);
        }

        [HttpPost("CreateAccount")]
        public ActionResult CreateAccount([FromBody] AccountDto accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest("Fields cannot be empty");
            }
            var filepath = FilePath(accountDto.FirstName, accountDto.LastName);
            if (System.IO.File.Exists(filepath))
            {
                return BadRequest("Account already exist");
            }

            //serialize object to json and format
           var jsonString = JsonConvert.SerializeObject(accountDto, Formatting.Indented);
            System.IO.File.WriteAllText(filepath, jsonString);
            return Ok("Account created");
        }

        //get absolute filepath
        private string FilePath(string firstname,string lastname)
        {
            var fileName = $"{firstname}-{lastname}.json";
            var filepath = $"{_webHostEnvironment.ContentRootPath}accounts/{fileName}";
            return filepath;
        }
    }
}
