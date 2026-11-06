using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if(other.gameObject.CompareTag("Player"))
        {
            player.hasKey = true;
            Debug.Log("Player has key");
            Destroy(gameObject);
        }
    }
}
