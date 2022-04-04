using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinePlacement : MonoBehaviour
{
    Transform cam;
    public KartController kc;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = cam.position + (cam.rotation * cam.forward * 25f);
        transform.position = new Vector3(
            transform.position.x,
            cam.position.y - cam.GetComponent<FollowPlayer>().offsetY,
            transform.position.z
        );
        transform.LookAt(cam);

        GetComponent<ParticleSystem>().emissionRate = kc.GetBoostTime() > 0f ? 100 : 0;
    }
}
