using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{ public static BulletPool instance;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();
    void Awake()
    {
        instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        foreach ( GameObject bullet in pool)
        {
            if(!bullet.activeInHierarchy)return bullet;
        }
        GameObject newObj = Instantiate(bulletPrefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }


}

