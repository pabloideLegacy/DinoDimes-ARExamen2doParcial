using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int vidasIniciales = 3;

    [Header("Externos")]
    [SerializeField] private ARManager arManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private float maxMovimiento;
    [SerializeField] private float minMovimiento;
    private Transform jugador;
    private GameObject contenedorJugador;
    private Vector3 posicionInicialJugador;
    private Vector3 direccionLateralJugador;
    private int vidasActuales;
    private bool juegoTerminado;
    public int puntos = 10; // Puntos a sumar al puntaje al recoger una moneda

    void Start()
    {
        vidasActuales = vidasIniciales;
        UIManager.ActualizarVidas(vidasActuales);
    }

    void Update()
    {
        if(juegoTerminado || jugador==null || Input.touchCount==0) return;
        Touch touch = Input.GetTouch(0);
        if(EventSystem.current!=null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        float posicionHorizontal = Mathf.InverseLerp(0f,Screen.width, touch.position.x);
        float desplazamientoX = Mathf.Lerp(minMovimiento,maxMovimiento,posicionHorizontal);
        jugador.position = posicionInicialJugador + direccionLateralJugador * desplazamientoX;
    }

    public void EstablecerJugador (GameObject objetoColocado)
    {
        DinoCollisions colisiones = objetoColocado.GetComponentInChildren<DinoCollisions>(true);
        contenedorJugador = objetoColocado;
        jugador = colisiones.transform;
        posicionInicialJugador = jugador.position;
        direccionLateralJugador = Vector3.ProjectOnPlane(colisiones.transform.right, Vector3.up).normalized;
        if (direccionLateralJugador.sqrMagnitude < 0.001f) direccionLateralJugador = Vector3.right;
        colisiones.gameManager = this;
        UIManager.MostrarHUDJuego (true);
    }

    public void ReiniciarJuego()
    {
        if (!arManager.VolverAColocacion()) return;
        contenedorJugador = null;
        jugador = null;
        juegoTerminado = false;
        vidasActuales = vidasIniciales;
        UIManager.ReiniciarInterfaz(vidasActuales);
    }

    public void SumarPuntos(int puntos)
    {
        if (juegoTerminado) return;
        UIManager.SumarPuntos(puntos);
    }

    public void RestarVida()
    {
        if (juegoTerminado) return;

        vidasActuales--;
        UIManager.ActualizarVidas(vidasActuales);
        if (vidasActuales <= 0) GameOver();
    }

    void GameOver()
    {
        juegoTerminado = true;
        if(contenedorJugador != null) contenedorJugador.SetActive(false);
        UIManager.GameOver();
    }
    
}
