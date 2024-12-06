using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject bulletPrefab;  // Prefab الرصاصة
    public Transform firePoint;     // نقطة الإطلاق
    public int maxAmmo = 10;        // أقصى عدد ذخيرة
    public int currentAmmo;         // الذخيرة الحالية
    public float fireRate = 0.5f;   // معدل الإطلاق
    public float bulletSpeed = 20f; // سرعة الرصاصة
    public AudioClip fireSound;     // صوت الإطلاق
    private float nextFireTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo; // بدء الذخيرة
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;

        audioSource.PlayOneShot(fireSound);

        Destroy(bullet, 2f); // مسح الرصاصة بعد ثانيتين
    }
    public void Reload(int ammo)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo);
    }

}
