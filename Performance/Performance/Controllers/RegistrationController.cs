using Attendees.Models;
using Microsoft.AspNetCore.Mvc;
using Registration.Models;
using System.Data.SqlClient;


[Route("api/RegistrationController")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly string _connectionString;

    public RegistrationController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpGet(Name ="api/Getregistration")]
    public IEnumerable<Registrations> Get()
    {
        var Registrations = new List<Registrations>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Registration", conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Registrations.Add(new Registrations
                    {
                        RegistrationID = (int)reader["RegistrationID"],
                        EventId = (int)reader["EventId"],
                        AttendeeId = (int)reader["AttendeeId"],
                        RegistrationDate = (DateTime)reader["RegistrationDate"]
                        
                    });
                }
            }
        }
        return Registrations;
    }

    [HttpPost(Name = "api/AddNewRigistration")]
    public IActionResult Create(Registrations Registrations)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using (SqlCommand varifiycmd = new SqlCommand("SELECT COUNT(*) FROM Events WHERE EventID = @EventID", conn))
            {
                varifiycmd.Parameters.AddWithValue("@EventID", Registrations.EventId);

                int EventIdExist = (int)varifiycmd.ExecuteScalar();

                if (EventIdExist == 0)
                {
                    return BadRequest("EventId does not exist try to use the get command to find event Id's");
                }
            }
            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Registration (EventId, AttendeeId, RegistrationDate) VALUES (@EventID, @AttendeeId, @RegistrationDate)", conn))
            {
                cmd.Parameters.AddWithValue("@EventId", Registrations.EventId);
                cmd.Parameters.AddWithValue("@AttendeeId", Registrations.AttendeeId);
                cmd.Parameters.AddWithValue("@RegistrationDate", Registrations.RegistrationDate);

                int AttendeeAddSuccess = cmd.ExecuteNonQuery();

                if (AttendeeAddSuccess > 0)
                    return Ok("New Attendee Added Successfully!");
                else return BadRequest("Unsuccessful... Added Attendee");
            }
        }
    }

    [HttpPut("{id}", Name = "api/UpdateRegistration")]
    public IActionResult Update(int id, Registrations Registrations)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(@"UPDATE Registration SET EventId = @EventId, AttendeeId = @AttendeeId WHERE RegistrationID = @Id", connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("@EventId", Registrations.EventId);
                cmd.Parameters.AddWithValue("@AttendeeId", Registrations.AttendeeId);
                cmd.Parameters.AddWithValue("@RegistrationDate", Registrations.RegistrationDate);

                int rowAffected = cmd.ExecuteNonQuery();

                if (rowAffected > 0)
                    return Ok("Update All Good");
                else return BadRequest("Update Not Good");
            }
        }
    }

    [HttpDelete("{id}", Name = "DeleteRegistration")]
    public IActionResult Delete(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Registration WHERE RegistrationID = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0) return Ok("Delete Work");
                else return NotFound("Registration does not exist");
            }
        }

    }
}