using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;

    void FixedUpdate()
    {
        this.gameObject.GetComponent<Rigidbody>().MovePosition(
            this.gameObject.GetComponent<Rigidbody>().position + this.transform.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy")
        {
            Quaternion opositeRotation = Quaternion.LookRotation(-transform.forward);
            IKillable enemy = other.GetComponent<IKillable>();
            enemy.TakeDamage(damage);
            enemy.ShowBlood(opositeRotation);

        }
        Destroy(gameObject);
    }

}
