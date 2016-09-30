using UnityEngine;
using System.Collections;

public class JumpPadController : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        
    }

    void OnPlayerJump(PlayerController player, bool wallJump)
    {
        player.RigidBody.AddForce(transform.rotation * Vector3.up * Speed, ForceMode.Impulse);
    }

    void OnTriggerEnter()
    {
        PlayerController.JumpEvent += OnPlayerJump;
    }

    void OnTriggerExit()
    {
        PlayerController.JumpEvent -= OnPlayerJump;
    }
}
