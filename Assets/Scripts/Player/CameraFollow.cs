using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, transform.position.z);
    }
}
