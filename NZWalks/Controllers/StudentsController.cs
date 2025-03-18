using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet(Name = "GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "John", "Vlad", "Zakhar" };
            return Ok(studentNames);
        }
    }
}