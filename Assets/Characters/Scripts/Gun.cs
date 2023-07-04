using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunInfo gunInfo;
    [SerializeField] ParticleSystem muzleFlash;
    [SerializeField] AudioSource shootSound;

    #region BulletInventory
    private int bulletInMag = 30;
    private int bulletInHand = 250;
    #endregion

    private void Start()
    {
        UpdateBulletInfo();
    }

    public bool PeekShoot() => bulletInMag > 0;

    public bool Shoot()
    {
        if(bulletInMag == 0) return false;

        bulletInMag--;

        if (muzleFlash) muzleFlash.Play();
        if (shootSound) shootSound.Play();

        UpdateBulletInfo();

        return true;
    }
    public void Reload()
    {
        if(bulletInHand == 0) return;

        int requiredBullets = 30 - bulletInMag;
        int GottenBullets = (bulletInHand >= requiredBullets) ? requiredBullets : bulletInHand;
        bulletInHand -= GottenBullets;
        bulletInMag += GottenBullets;

        UpdateBulletInfo();
    }

    private void UpdateBulletInfo()
    {
        gunInfo.BulletInMag = bulletInMag;
        gunInfo.BulletInHand = bulletInHand;
    }
}
