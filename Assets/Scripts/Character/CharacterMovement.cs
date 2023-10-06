using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void LookAt (Vector3 direction)
    {
        rb.MoveRotation(Quaternion.LookRotation(direction));
    }

    public void MoveTo(Vector3 direction, float speed)
    {
        rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
    }

    public void DisableCollisions()
    {
        rb.useGravity = false;
        GetComponent<Collider>().enabled = false;
    }
}
