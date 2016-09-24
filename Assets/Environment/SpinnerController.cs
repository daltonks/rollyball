using UnityEngine;
using System.Collections;

public class SpinnerController : MonoBehaviour
{
    public float Speed;

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, Time.fixedDeltaTime * Speed, 0) * transform.rotation;
    }
}
