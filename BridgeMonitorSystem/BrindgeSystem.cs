using System.Collections;
using UnityEngine;
using UnityEngine.Networking; // İŞTE YENİ GÜCÜMÜZ (HTTP Kütüphanesi)

public class BridgeSensor : MonoBehaviour
{
    [Header("Ayarlar")]
    public int SensorID = 1;
    public float RefreshRate = 0.5f;

    // ARTIK SQL ŞİFRESİ YOK! Sadece Adres var.
    // DİKKAT: API adresin "localhost:5000" ise burası aynen kalmalı.
    private string apiUrl = "http://localhost:5000/api/sensor/latest/";

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        StartCoroutine(GetDataLoop());
    }

    IEnumerator GetDataLoop()
    {
        while (true)
        {
            // Adresi oluştur: http://localhost:5000/api/sensor/latest/1
            string fullUrl = apiUrl + SensorID;

            // API'ye "GET" isteği gönder (Mektup yolla)
            using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
            {
                // Cevap gelene kadar bekle
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"API Hatası: {request.error}");
                }
                else
                {
                    // Gelen JSON verisini metin olarak al
                    string jsonResult = request.downloadHandler.text;
                    // Debug.Log("Gelen Veri: " + jsonResult); // İstersen açıp bakabilirsin

                    // JSON'ı C# nesnesine çevir (Parse)
                    SensorData data = JsonUtility.FromJson<SensorData>(jsonResult);

                    if (data != null)
                    {
                        UpdateColor(data.stressValue);
                    }
                }
            }

            yield return new WaitForSeconds(RefreshRate);
        }
    }

    void UpdateColor(float value)
    {
        Color targetColor = Color.green;
        if (value > 120) targetColor = Color.red;
        else if (value >= 100) targetColor = Color.yellow;

        _renderer.material.color = targetColor;
    }
}

// JSON verisini karşılayacak "Kalıp" (API'deki Model ile aynı isimde değişkenler)
[System.Serializable]
public class SensorData
{
    public int id;
    public int sensorID;
    public float stressValue;
    public string createdAt;
}