using UnityEngine;

public class DinoCollisions : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            // Lógica para cuando el dinosaurio colisiona con un obstáculo
            Debug.Log("Colisión con obstáculo");
            GameObject.Destroy(other.gameObject); // Destruye el obstáculo al colisionar
            gameManager.RestarVida();

        }
        else if (other.CompareTag("Coin"))
        {
            // Lógica para cuando el dinosaurio colisiona con una moneda
            Debug.Log("Colisión con moneda");
            GameObject.Destroy(other.gameObject); // Destruye la moneda al colisionar
            gameManager.SumarPuntos(gameManager.puntos); // Sumar los puntos definidos en GameManager
        }
    }
}
