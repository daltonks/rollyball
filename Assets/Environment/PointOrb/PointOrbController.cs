using UnityEngine;
using System.Collections;

public class PointOrbController : MonoBehaviour
{
    void OnTriggerEnter()
    {
        MasterController.Points++;
        Destroy(gameObject);
    }
}
