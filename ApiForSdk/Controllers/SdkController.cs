using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class SdkController : ControllerBase
{
    MySqlConnection _conn;
    public SdkController()
    {
        _conn = new MySqlConnection();
        _conn.ConnectionString = "Server=sql.freedb.tech;Port=3306;Database=freedb_apiforsdk;uid=freedb_Schmitz;pwd=4VuHd5Kr!vm748%";
    }

    private class MyselfData
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
    }

    [HttpGet]
    public IActionResult GetMyself()
    {
        try
        {
            _conn.Open();
        }
        catch
        {
            return NotFound("connection can not be established");
        }

        MyselfData self = new MyselfData();

        MySqlCommand command = _conn.CreateCommand();
        command.CommandText = "SELECT * FROM myself";

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            self.ID = reader.GetInt32(0);
            self.FirstName = !reader.IsDBNull("FirstName") ? reader.GetString("FirstName") : null;
            self.LastName = !reader.IsDBNull("LastName") ? reader.GetString("LastName") : null;
            self.DayOfBirth = !reader.IsDBNull("DayOfBirth") ? reader.GetDateTime("DayOfBirth") : null;
            self.Email = !reader.IsDBNull("Email") ? reader.GetString("Email") : null;
            self.Message = !reader.IsDBNull("Message") ? reader.GetString("Message") : null;
        }

        reader.Close();
        _conn.Close();

        return Ok(self);
    }
}