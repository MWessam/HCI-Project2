using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class GunController : MonoBehaviour
{

    public Bullet bulletPrefab;  // Prefab الرصاصة
    public Transform firePoint;     // نقطة الإطلاق
    public int maxAmmo = 10;        // أقصى عدد ذخيرة
    public int currentAmmo;         // الذخيرة الحالية
    public int currentDamage;
    public float fireRate = 0.5f;   // معدل الإطلاق
    public float bulletSpeed = 20f; // سرعة الرصاصة
    public AudioClip fireSound;     // صوت الإطلاق
    private AudioSource _audioSource;
    private float nextFireTime = 0f;
    [SerializeField] private Animator _animator;

    /// <summary>
    /// Object pool to optimize creation logic.
    /// </summary>
    private IObjectPool<Bullet> _bulletPool;

    private static readonly int IsShooting = Animator.StringToHash("isShooting");
    [SerializeField] private ParticleSystem _particles;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(
            () => Instantiate(bulletPrefab, firePoint.position, Quaternion.identity),
            x => x.gameObject.SetActive(true),
            x => x.gameObject.SetActive(false),
            x => Destroy(x),
            defaultCapacity:30);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo; // بدء الذخيرة
        _audioSource = GetComponent<AudioSource>();
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

        var bullet = _bulletPool.Get();
        var rb = bullet.GetComponent<Rigidbody2D>();
        bullet.damage = currentDamage;
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        rb.linearVelocity = firePoint.right * bulletSpeed;

        _audioSource.clip = fireSound;
        _audioSource.Play();
        _particles.Play();
        _animator.SetTrigger(IsShooting);
        StartCoroutine(BulletTimer(bullet));
    }

    private IEnumerator BulletTimer(Bullet bullet)
    {
        yield return new WaitForSeconds(3.0f);
        _bulletPool.Release(bullet);
    }

    public void Reload(int ammo)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo);
    }

}
