using Microsoft.AspNetCore.Mvc;
using Attendees.Models;
using System.Data.SqlClient;


[Route("api/AttendeesController")]
[ApiController]
public class AttendeeController : ControllerBase
{
    private readonly string _connectionString;

    public AttendeeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpGet(Name = "Getattendees")]
    public IEnumerable<Attendee> Get()
    {
        var Attendee = new List<Attendee>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Attendees", conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Attendee.Add(new Attendee
                    {
                        AttendeeID = (int)reader["AttendeeID"],
                        Name = (string)reader["Name"],
                        Email = (string)reader["Email"],
                        TicketType = reader["TicketType"].ToString()
                    });
                }
            }
        }
        return Attendee;
    }
    [HttpPost(Name = "AddNewAttendees")]
    public IActionResult Create(Attendee attendee)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Attendees WHERE Email = @Email", conn))
            {
                cmd.Parameters.AddWithValue("@Email", attendee.Email);

                int num = (int)cmd.ExecuteScalar();
                if (num > 0) {
                    return Conflict(new { message = "Email Already exists", email = attendee.Email });
                }
            }

            using (SqlCommand cmd = new SqlCommand("INSERT INTO Attendees (Name, Email, TicketType) VALUES (@Name, @Email, @TicketType)", conn)) 
            {
                cmd.Parameters.AddWithValue("@Name", attendee.Name);
                cmd.Parameters.AddWithValue("@Email", attendee.Email);
                cmd.Parameters.AddWithValue("@TicketType", attendee.TicketType);

                int ChangeRows = cmd.ExecuteNonQuery();

                if (ChangeRows > 0)
                    return Ok("New Attendee Added Successfully!");
                else return BadRequest("Unsuccessful... Added Attendee");
            }
        }
    }
    [HttpPut("{id}", Name = "UpdateAttendes")]
    	public IActionResult Update(int id, Attendee Attendee) 
    	{
    	using (SqlConnection connection = new SqlConnection(_connectionString))
    		{
    			connection.Open();
            using (SqlCommand cmd = new SqlCommand(@"UPDATE Attendees SET Name = '@Name', Email = '@Email', TicketType = '@TicketType' WHERE AttendeeID = @ID", connection))
            {
                cmd.Parameters.AddWithValue("ID", id);
                cmd.Parameters.AddWithValue("@Name", Attendee.Name);
                cmd.Parameters.AddWithValue("@Email", Attendee.Email);
                cmd.Parameters.AddWithValue("@TicketType", Attendee.TicketType);


                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Ok("Update All Good");
                else return BadRequest("Update Not Good");
            }
    		}
    	}
    [HttpDelete("{id}", Name = "DeleteAttendees")]
    public IActionResult Delete(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Registration WHERE AttendeeID = @ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                int nums = (int)cmd.ExecuteScalar();
                if (nums > 0)
                {
                    return Conflict("Remember to delete in Registration first before deleting here.");
                }
            }

            using (SqlCommand cmd2 = new SqlCommand("DELETE FROM Attendees WHERE AttendeeID = @ID", conn)) 
            {
                cmd2.Parameters.AddWithValue("@ID", id);
                int rowsAffected = cmd2.ExecuteNonQuery();
                if (rowsAffected > 0) return Ok("Delete Work");
                else return NotFound("Attendee does not exist");
            }
        }

    }
}