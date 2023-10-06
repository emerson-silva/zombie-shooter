using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedGun : MonoBehaviour
{
    public float rotateSpeed;
    public GunType gunType;
    public float DestroyTimer;

    private void Start()
    {
        if (DestroyTimer > 0)
        {
            Destroy(gameObject, DestroyTimer);
        }
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            other.gameObject.GetComponent<Player>().PickUpGun(gunType);
            Destroy(gameObject);
        }
    }
}
