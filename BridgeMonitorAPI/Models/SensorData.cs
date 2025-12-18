using System; // Tarih saat işlemleri için gerekli

namespace BridgeMonitorAPI.Models
{
    public class SensorData
    {
        public int ID { get; set; }
        public int SensorID { get; set; }
        public float StressValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}