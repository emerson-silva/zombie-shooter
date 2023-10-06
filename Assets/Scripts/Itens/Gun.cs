using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun :MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawn;
    private float timeLastShoot;
    public float timeBetweenShoots;
    public GunType type;
    public ShootType ShootType;

    public void Shoot()
    {
        if (Time.timeSinceLevelLoad > timeLastShoot + timeBetweenShoots)
        {
            timeLastShoot = Time.timeSinceLevelLoad;
            GameObject.Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}

public enum ShootType
{
    TRIGGER,
    CONTINUOUS
}