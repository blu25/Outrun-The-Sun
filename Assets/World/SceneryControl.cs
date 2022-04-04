using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryControl : MonoBehaviour
{
    public Transform cam;
    public Transform player;

    public float time;
    public float sunSet;

    public TransitionController tc;

    // Start is called before the first frame update
    void Start()
    {
        //DynamicGI.UpdateEnvironment();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.position;

        time += Time.deltaTime * 2f;

        sunSet = time - (player.position.z / 15f);

        //Debug.Log(sunSet);

        transform.rotation = Quaternion.Euler(90 + sunSet, 0, 0);

        if (sunSet >= 115 && !tc.IsTransitioning()) {
            tc.StartTransition("Score");
            scoreHolder.time = time / 2f;
            scoreHolder.distance = player.position.z;
        }
    }
}
