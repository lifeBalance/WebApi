using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    // GET: api/Users
    [HttpGet(Name = "GetUsers")]
    public IEnumerable<string> Get()
    {
        return ["Bobbys", "Frank"];
    }

    // GET: api/Users/5
    [HttpGet("{id}", Name = "GetUser")]
    public string Get(int id)
    {
        return $"User {id}";
    }

    // POST: api/Users
    [HttpPost(Name = "PostUser")]
    public string Post()
    {
        // Convert JSON body to string
        // string value = jsonBody.ToString();

        return $"User POST: ";
    }
}
// {
//   "jsonBody": "balls"
// }