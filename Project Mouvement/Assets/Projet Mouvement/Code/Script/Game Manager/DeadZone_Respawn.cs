using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone_Respawn : MonoBehaviour
{
    [Header("Parameter")]
    public GameObject player;
    public GameObject respawnPoint;
    [Space]
    public KeyCode pcInput = KeyCode.R;
    [Tooltip("Right Upper Button")]
    public KeyCode gamepadInput = KeyCode.Joystick1Button5;


    private void Update()
    {
        if (Input.GetKeyDown(pcInput) || Input.GetKeyDown(gamepadInput))
        {
            RespawnPlayer();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
        player.GetComponent<Player_BasicMouvement>().DeathReset();
    }
}
