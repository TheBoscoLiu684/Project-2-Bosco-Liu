using UnityEngine;
using UnityEngine.SceneManagement;
public class EscapeScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (collision.gameObject.CompareTag("Player") && player.hasKey)
        {
            SceneManager.LoadScene("Retry");
        }
    }
}
