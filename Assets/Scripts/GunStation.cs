using System;
using System.Collections.Generic;
using UnityEngine;

public class GunStation : MonoBehaviour
{
    public GunController gunController;
    [SerializeField] List<WeaponUpgradeData> _weaponUpgrades;
    private int _currentLevel = 0;

    private bool isNear = false;

    private void Start()
    {
        gunController.fireRate = _weaponUpgrades[0].FireRate; // تقليل معدل الإطلاق
        gunController.currentDamage = (int)_weaponUpgrades[0].Damage; // تقليل معدل الإطلاق
        Debug.Log("Gun Upgraded!");
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E) && _currentLevel < _weaponUpgrades.Count - 1)
        {
            var nextUpgrade = _weaponUpgrades[_currentLevel + 1];
            if (ScoreManager.instance.GetScore() < nextUpgrade.RequiredScore) return;
            ScoreManager.instance.AddScore(-nextUpgrade.RequiredScore);
            gunController.fireRate = nextUpgrade.FireRate; // تقليل معدل الإطلاق
            gunController.currentDamage = (int)nextUpgrade.Damage; // تقليل معدل الإطلاق
            Debug.Log("Gun Upgraded!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

}

[Serializable]
public class WeaponUpgradeData
{
    public float Damage;
    public float FireRate;
    public int RequiredScore;
}
