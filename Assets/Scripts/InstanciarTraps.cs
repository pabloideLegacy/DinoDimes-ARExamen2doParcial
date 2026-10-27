using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InstanciarTraps : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab; // Prefab del trap a instanciar
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float tiempoRespawnMinimo = 3f;
    [SerializeField] private float tiempoRespawnMaximo = 6f;
    [SerializeField] private float moveSpeed = 1f;

    private readonly List<GameObject> objetosInstanciados = new List<GameObject>();
    private Coroutine rutinaRespawn;


    private void OnEnable()
    {
        rutinaRespawn = StartCoroutine(RutinaRespawn());
    }

    private void OnDisable()
    {
        if (rutinaRespawn != null)
        {
            StopCoroutine(rutinaRespawn);
            rutinaRespawn = null;
        }
        LimpiarObjetosInstanciados();
    }

    private IEnumerator RutinaRespawn()
    {
        while(true) { yield return new WaitForSeconds(Random.Range(tiempoRespawnMinimo,tiempoRespawnMaximo));
        InstanciarObjeto();}
    }

    private void InstanciarObjeto()
    {
        GameObject prefab = Random.value < 0.5f ? trapPrefab : coinPrefab;
        GameObject instancia = Instantiate(prefab,transform.position,transform.rotation);
        objetosInstanciados.RemoveAll(objeto => objeto == null);
        objetosInstanciados.Add(instancia);

        MoverObjetoInstanciado movimiento = instancia.GetComponent<MoverObjetoInstanciado>();
        if (movimiento == null)
        {
            movimiento = instancia.AddComponent<MoverObjetoInstanciado>();
        } 

        movimiento.Configurar(moveSpeed, transform.forward);

    }
    
    private void LimpiarObjetosInstanciados ()
    {
        foreach (GameObject objeto in objetosInstanciados)
        if(objeto!=null) Destroy(objeto);
        objetosInstanciados.Clear();
    }
    

}
