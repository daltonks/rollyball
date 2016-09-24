using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject PlayerPrefab, CameraPrefab;
    public static Transform Player { get; private set; }
    public static Transform Camera { get; private set; }

    void Start ()
    {
        Camera = ((GameObject) Instantiate(CameraPrefab, SpawnPoint.transform)).transform;
        Player = ((GameObject) Instantiate(PlayerPrefab, SpawnPoint.transform)).transform;
    }
	
	void Update ()
    {
	    
	}
}
