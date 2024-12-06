using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GunController gun = other.GetComponentInChildren<GunController>();
            if (gun != null)
            {
                gun.Reload(ammoAmount);
                Destroy(gameObject); // مسح صندوق الذخيرة
            }
        }
    }
}
