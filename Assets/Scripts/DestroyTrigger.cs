using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        MoverObjetoInstanciado objetoMovil = other.GetComponentInParent<MoverObjetoInstanciado>();
        if (objetoMovil!=null) Destroy (objetoMovil.gameObject);
    }
}
