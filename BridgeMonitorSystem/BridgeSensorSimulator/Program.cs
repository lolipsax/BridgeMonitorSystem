using System;
using System.Data.SqlClient;
using System.Threading;

namespace BridgeSensorSimulator
{
    // VERİTABANI YÖNETİCİSİ (Repository)
    public static class DatabaseRepository
    {
        // Azure Data Studio'da gördüğüm kadarıyla sunucun "localhost"
        // Eğer bağlantı hatası alırsan burayı kontrol ederiz.
        private static string _connectionString = "Server=localhost;Database=BridgeMonitor;Trusted_Connection=True;TrustServerCertificate=True;";

        public static void InsertSensorData(int sensorId, float value)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO SensorData (SensorID, StressValue) VALUES (@id, @val)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", sensorId);
                        cmd.Parameters.AddWithValue("@val", value);
                        cmd.ExecuteNonQuery();
                    }

                    // Başarılı işlem logu
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[SQL] Kayıt Başarılı -> Sensör: {sensorId} | Değer: {value:F2}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[HATA] SQL Bağlantı Sorunu: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }

    // SANAL SENSÖR MANTIĞI
    public class VirtualSensor
    {
        private Random _random = new Random();
        private int _sensorId;

        public VirtualSensor(int id)
        {
            _sensorId = id;
        }

        public float ReadCurrentStress()
        {
            int chance = _random.Next(0, 100);

            // %10 İhtimalle TEHLİKE (Kırmızı Alarm Denemesi İçin)
            if (chance > 90) return _random.Next(125, 160);
            // %20 İhtimalle UYARI
            else if (chance > 70) return _random.Next(100, 124);
            // %70 İhtimalle NORMAL
            else return _random.Next(50, 99);
        }

        public int GetId() => _sensorId;
    }

    // ANA PROGRAM
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bridge Monitor - Simulator";
            Console.WriteLine("=== SİMÜLASYON BAŞLADI ===");

            VirtualSensor sensor1 = new VirtualSensor(1); // Sol Ayak
            VirtualSensor sensor2 = new VirtualSensor(2); // Sağ Ayak

            while (true)
            {
                // Veri üret ve kaydet
                DatabaseRepository.InsertSensorData(sensor1.GetId(), sensor1.ReadCurrentStress());
                Thread.Sleep(100); // Kısa bir ara
                DatabaseRepository.InsertSensorData(sensor2.GetId(), sensor2.ReadCurrentStress());

                // 1 Saniye bekle (Veritabanını boğmamak için)
                Console.WriteLine("...");
                Thread.Sleep(1000);
            }
        }
    }
}