using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 8f;
    public float bulletSpeed = 80f;
    public WeaponProceduralRecoil weaponRecoil;

    float nextFireTime;

    void Update()
    {
        if (!PracticeSessionManager.Instance.sessionActive) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        PracticeSessionManager.Instance.AddShotFired();

        if (weaponRecoil != null)
            weaponRecoil.ApplyRecoil();

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 500f;

        Vector3 dir = (targetPoint - firePoint.position).normalized;

        GameObject bulletObj = BulletPool.instance.GetBullet();
        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = Quaternion.LookRotation(dir);

    
        bulletObj.SetActive(true);

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
            bullet.SetDirection(dir, bulletSpeed);
    }
}

