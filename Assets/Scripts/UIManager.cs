using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private int puntajeActual;
    public int PuntajeActual => puntajeActual;

    [Header("PanelesUI")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelVistaAR;
    [SerializeField] private GameObject panelGameOver;

    [Header("TextosUI")]
    [SerializeField] private TextMeshProUGUI textoPuntaje;
    [SerializeField] private TextMeshProUGUI textoVidas;
    [SerializeField] private TextMeshProUGUI textoPuntajeFinal;
    [SerializeField] private TMP_InputField inputNombre;

    public ARManager arManager; // Referencia al ARManager

    
    void Start()
    {
        panelInicio.SetActive(true);
        panelVistaAR.SetActive(false);
        panelGameOver.SetActive(false);
        SumarPuntos(0);
        MostrarHUDJuego(false);
    }

    public void SumarPuntos(int puntos)
    {
        puntajeActual+= puntos;
        textoPuntaje.text = "Puntuacion: " + puntajeActual.ToString();
    }

    public void ActualizarVidas(int vidas)
    {
        textoVidas.text = "Vidas: " + vidas.ToString();
    }

    public void MostrarHUDJuego(bool mostrar)
    {
        textoPuntaje.enabled = mostrar;
        textoVidas.enabled = mostrar;
    }

    public void ReiniciarJuego()
    {
        gameManager.ReiniciarJuego();
    }
    
    public void ReiniciarInterfaz(int vidas)
    {
        puntajeActual = 0;
        textoPuntaje.text = "Puntuacion: 0";
        ActualizarVidas(vidas);
        MostrarHUDJuego(false);
        inputNombre.text = "";   
        panelGameOver.SetActive(false);
        panelVistaAR.SetActive(true);
    }

    public void IniciarAR()
    {
        panelInicio.SetActive(false);
        panelVistaAR.SetActive(true);
        arManager.DetectarPlanos(); // Llamada al método DetectarPlanos del ARManager
    }
    
    public void GameOver()
    {
        panelVistaAR.SetActive(false);
        panelGameOver.SetActive(true);
        textoPuntajeFinal.text = "Puntaje Final: " + puntajeActual.ToString();
    }

}
