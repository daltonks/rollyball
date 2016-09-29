using UnityEngine;
using System.Collections;

public class SpeedPadController : MonoBehaviour
{
    public float Speed;

    void OnTriggerEnter(Collider collider)
    {
        OnTriggerStay(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        collider.GetComponent<Rigidbody>().AddForce(transform.rotation * Vector3.forward * Speed);
    }
}
