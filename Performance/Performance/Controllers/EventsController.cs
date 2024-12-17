using Microsoft.AspNetCore.Mvc;
using Events.Models;
using System.Data.SqlClient;


[Route("api/EventsController")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly string _connectionString;

    public EventsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpGet(Name = "Getevents")]
    public IEnumerable<Event> Get()
    {
        var Events = new List<Event>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Events", conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Events.Add(new Event
                    {
                        EventID = (int)reader["EventID"],
                        Title = (string)reader["Title"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Date = (DateTime)reader["Date"]
                    });
                }
            }
        }
        return Events;
    }

    [HttpPost(Name = "AddNewEvents")]
    public IActionResult Create(Event Event)
    {
        var ValidStatus = new List<string> { "Cancelled", "Completed", "Scheduled" };
        if (!ValidStatus.Contains(Event.Status))
        {
            return BadRequest($"Invalid Status value. Allowed value are: {string.Join(", ", ValidStatus)}");
        }
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Events (Title, Location, Status, Date) VALUES ( @Title, @Location, @Status, @Date)", conn))
            {
                cmd.Parameters.AddWithValue("@Title", Event.Title);
                cmd.Parameters.AddWithValue("@Location", Event.Location);
                cmd.Parameters.AddWithValue("@Status", Event.Status);
                cmd.Parameters.AddWithValue("@Date", Event.Date);

                int AttendeeAddSuccess = cmd.ExecuteNonQuery();

                if (AttendeeAddSuccess > 0)
                    return Ok("New Event Added Successfully!");
                else return BadRequest("Unsuccessful... Event not added");
            }
        }
    }

    [HttpPut("{id}", Name = "UpdateEvents")]
    public IActionResult Update(int id, Event Event)
    {
        var ValidStatus = new List<string> { "Cancelled", "Completed", "Scheduled" };
        if (!ValidStatus.Contains(Event.Status))
        {
            return BadRequest($"Invalid Status value. Allowed value are: {string.Join(", ", ValidStatus)}");
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {


            connection.Open();
            using (SqlCommand cmd = new SqlCommand(@"UPDATE Events SET Title = @Title, Location = @Location, Date = @Date, Status = @Status WHERE EventID = @ID", connection))
            {
                cmd.Parameters.AddWithValue("ID", id);
                cmd.Parameters.AddWithValue("@Title", Event.Title);
                cmd.Parameters.AddWithValue("@Location", Event.Location);
                cmd.Parameters.AddWithValue("@Date", Event.Date);
                cmd.Parameters.AddWithValue("@Status", Event.Status);


                int rowAffected = cmd.ExecuteNonQuery();

                if (rowAffected > 0)
                    return Ok("Update All Good");
                else return BadRequest("Update Not Good");
            }
        }
    }

    [HttpDelete("{id}", Name = "DeleteEvents")]
    public IActionResult Delete(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Events WHERE EventID = @ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0) return Ok("Delete Work");
                else return NotFound("Attendee does not exist");
            }
        }

    }
}