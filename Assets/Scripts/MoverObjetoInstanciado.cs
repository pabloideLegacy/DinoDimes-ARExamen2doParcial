using UnityEngine;

public class MoverObjetoInstanciado : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;

    public void Configurar(float speed, Vector3 direction)
    {
        moveSpeed = speed;
        moveDirection = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        if(moveDirection.sqrMagnitude < 0.001f) moveDirection = Vector3.forward;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) rigidbody=gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
