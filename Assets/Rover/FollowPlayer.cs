using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float offsetZ;
    public float offsetY;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(player);
        transform.position = Vector3.Lerp(
            transform.position,
            player.position + (-player.forward * offsetZ) + (Vector3.up * offsetY),
            Time.deltaTime * moveSpeed);
    }
}
