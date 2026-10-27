using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;


public class ARManager : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;

    private GameObject contenedorModeloAnterior;

    [Header("AR Components")]
    [SerializeField] private ARPlaneManager arPlaneManager; 
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private Camera arCamera;
    [SerializeField] private UnityEngine.UI.Button btnColocarModelo;

    [Header("AR Prefabs")]
    [SerializeField] private GameObject modeloPrefab;
    [SerializeField] private GameObject indicadorPrefab;

    private bool planosDetectados = false;
    private bool modeloColocado = false;
    

    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();


    void Awake()
    {
        arPlaneManager.enabled = false;
    }
    void Start()
    {
        btnColocarModelo.interactable = false;
        indicadorPrefab.SetActive(false);
        indicadorPrefab = GameObject.Instantiate(indicadorPrefab);
    }

    void Update()
    {
        if (!planosDetectados) return;
        if (modeloColocado) return;

        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = arRaycastHits[0].pose;
            indicadorPrefab.SetActive(true);
            indicadorPrefab.transform.position = hitPose.position;
            btnColocarModelo.interactable = ObtenerRotacionesDesdeVista(out Quaternion rotacionVista);
            if (btnColocarModelo.interactable) {indicadorPrefab.transform.rotation=rotacionVista;}
        }
        else
        {
            indicadorPrefab.SetActive(false);
        }
    }

    public void DetectarPlanos()
    {
        if (modeloColocado) return;
        arPlaneManager.enabled = true;
        MostrarPlanosDetectados(true);
        indicadorPrefab.SetActive(true);
        arPlaneManager.planesChanged += OnPlanesChanged;
        planosDetectados = true;
    }    

    private void DetenerDeteccionPlanos()
    {
        planosDetectados = false;
        arPlaneManager.planesChanged -= OnPlanesChanged;
        MostrarPlanosDetectados(false);
        arPlaneManager.enabled = false;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane plane in args.added)
        {
            MostrarVisualesPlano(plane, true);
        }
        if (args.added.Count > 0)
        {
            // Se han detectado planos
            Debug.Log("Planos detectados: " + args.added.Count);
        }
    }

    public void ColocarModelo()
    {
        if (modeloColocado) return;

        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = arRaycastHits[0].pose;
            if (!ObtenerRotacionesDesdeVista(out Quaternion rotacionVista)) return;
            GameObject modeloInstanciado = Instantiate(modeloPrefab,hitPose.position , rotacionVista);
            gameManager.EstablecerJugador(modeloInstanciado);
            contenedorModeloAnterior = modeloInstanciado;
            modeloColocado = true;
            DetenerDeteccionPlanos();
            indicadorPrefab.SetActive(false);
            btnColocarModelo.interactable = false;
            btnColocarModelo.gameObject.SetActive(false);

        }
    }

    public bool VolverAColocacion()
    {
        if(contenedorModeloAnterior != null) Destroy(contenedorModeloAnterior);
        contenedorModeloAnterior = null;
        modeloColocado = false;
        MostrarPlanosDetectados(true);
        arPlaneManager.enabled= true;
        arPlaneManager.planesChanged -= OnPlanesChanged;
        arPlaneManager.planesChanged += OnPlanesChanged;
        planosDetectados=true;
        indicadorPrefab.SetActive(true);
        btnColocarModelo.gameObject.SetActive(true);
        btnColocarModelo.interactable=false;
        return true;
    }

    private bool ObtenerRotacionesDesdeVista(out Quaternion rotacion)
    {
        Camera camara = arCamera != null ? arCamera : Camera.main;
        Vector3 direccion = Vector3.ProjectOnPlane(camara.transform.forward, Vector3.up);
        if (direccion.sqrMagnitude < 0.001f)
        {
            direccion = Vector3.ProjectOnPlane(camara.transform.up,Vector3.up);
        }
        
        if(direccion.sqrMagnitude < 0.001f) {rotacion = Quaternion.identity; return false;}
        rotacion = Quaternion.LookRotation(direccion.normalized, Vector3.up); return true;

    }

    private void MostrarPlanosDetectados(bool mostrar)
    {
        foreach (ARPlane plane in arPlaneManager.trackables)
        MostrarVisualesPlano(plane, mostrar);
    }

private void MostrarVisualesPlano(ARPlane plane, bool mostrar)
    {
        foreach (ARPlaneMeshVisualizer v in plane.GetComponentsInChildren<ARPlaneMeshVisualizer>(true))
            v.enabled = mostrar;
        foreach (Renderer r in plane.GetComponentsInChildren<Renderer>(true))
            r.enabled = mostrar;
    }


}
