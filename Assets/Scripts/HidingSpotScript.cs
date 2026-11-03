using UnityEngine;

public class HidingSpotScript : MonoBehaviour
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
        //Debug.Log("Entered hiding spot: " + other.name);
        PlayerScript player = other.GetComponent<PlayerScript>();
        if(player != null && other == player.hidingCollider)
        {
            //Debug.Log("player is hidden");
            player.isHidden = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exited hiding spot: " + other.name);
        PlayerScript player = other.GetComponent<PlayerScript>();
        if(player != null && other == player.hidingCollider)
        {
            //Debug.Log("player is not hidden");
            player.isHidden = false;
        }
    }

}
