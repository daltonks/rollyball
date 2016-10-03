using UnityEngine;
using System.Collections;

public class PointOrbController : MonoBehaviour
{
    private AudioSource AudioSource;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter()
    {
        AudioSource.PlayClipAtPoint(AudioSource.clip, transform.position);
        MasterController.Points++;
        Destroy(gameObject);
    }
}
