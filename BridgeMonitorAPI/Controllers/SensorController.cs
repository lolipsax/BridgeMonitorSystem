using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using BridgeMonitorAPI.Models; // Artık hata vermeyecek çünkü yukarıda düzelttik

namespace BridgeMonitorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        // TCP/IP açık olduğu için 127.0.0.1
        private string connectionString = "Server=127.0.0.1;Database=BridgeMonitor;Trusted_Connection=True;TrustServerCertificate=True;";

        [HttpGet("latest/{sensorId}")]
        public IActionResult GetLatestData(int sensorId)
        {
            SensorData data = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TOP 1 * FROM SensorData WHERE SensorID = @id ORDER BY ID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", sensorId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                data = new SensorData
                                {
                                    ID = (int)reader["ID"],
                                    SensorID = (int)reader["SensorID"],
                                    StressValue = (float)(double)reader["StressValue"],
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Sunucu Hatası: {ex.Message}");
                }
            }

            if (data == null) return NotFound("Veri bulunamadı");
            return Ok(data);
        }
    }
}