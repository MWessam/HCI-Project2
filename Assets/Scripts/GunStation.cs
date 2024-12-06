using UnityEngine;

public class GunStation : MonoBehaviour
{
   
    public GunController gunController;

    private bool isNear = false;

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            gunController.fireRate = Mathf.Max(0.1f, gunController.fireRate - 0.1f); // تقليل معدل الإطلاق
            Debug.Log("Gun Upgraded!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

}
