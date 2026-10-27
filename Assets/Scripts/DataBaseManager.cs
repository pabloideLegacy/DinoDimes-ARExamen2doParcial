using UnityEngine;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine.Networking;

public class DataBaseManager : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField inputNombre;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private string supabaseURL = "https://abyeyzrqahcpugxaaeve.supabase.co/rest/v1";
    [SerializeField] private string apiKey;
    [SerializeField] private string tableName= "PuntuacionesExamen";

    private bool solicitudEnCurso;

    public void PostScore()
    {
        if (solicitudEnCurso) return;
        string usuario = inputNombre.text;
        if(string.IsNullOrWhiteSpace(usuario))
        {
            Debug.LogError("Por favor ingresa un nombre para poder guardar el puntaje");
            return;
        }
        solicitudEnCurso=true;
        StartCoroutine(PostScoreRequest(usuario.Trim(),uiManager.PuntajeActual));
    }

    private IEnumerator PostScoreRequest(string usuario, int puntuacion)
    {
        string requestURL = supabaseURL.TrimEnd('/') + "/" + UnityWebRequest.EscapeURL(tableName);
        string json = JsonUtility.ToJson(new PuntuacionPayLoad
        {
            usuario = usuario,
            puntuacion = puntuacion
        });
        using (UnityWebRequest request = new UnityWebRequest(requestURL, UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("apiKey", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Prefer", "return=representation");

            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Guardado: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error HTTP " + request.responseCode + ": " + request.error + " " +request.downloadHandler.text);
            }
            solicitudEnCurso = false;
        }
    }

    [System.Serializable]
    private class PuntuacionPayLoad
    {
        public string usuario;
        public int puntuacion;
    }
}
