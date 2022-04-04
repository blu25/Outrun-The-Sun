using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunBarController : MonoBehaviour
{
	public SceneryControl sc;
    float blinkVal = 1;
    public GameObject sunImg;

    AudioSource ac;
    bool ac_played = false;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float ssval = 5 - (sc.sunSet / 20);
        for (int i = 0; i < 5; i++) {
            transform.GetChild(i).gameObject.SetActive(
                i < ssval
            );
        }
        if (ssval <= 0f) {
            blinkVal -= Time.deltaTime * 2f;

            if (blinkVal < -1)
                blinkVal = 1f;

            if (!ac_played) {
                ac.Play();
                ac_played = true;
            }
        }

        sunImg.SetActive(blinkVal >= 0);
    }
}
