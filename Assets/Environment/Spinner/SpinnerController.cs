using UnityEngine;
using System.Collections;

public class SpinnerController : MonoBehaviour
{
    public float Speed;

    void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.up, Time.fixedDeltaTime * Speed);
    }
}
