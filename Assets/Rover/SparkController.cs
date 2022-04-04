using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour
{
    KartController kc;



    // Start is called before the first frame update
    void Start()
    {
        kc = transform.parent.parent.GetComponent<KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (kc.DriftTime() < 0.5f)
            SetSparkAmount(0, 0);
        else if (kc.DriftTime() < 1.5f)
            SetSparkAmount(15, 0);
        else
            SetSparkAmount(0, 35);
    }

    void SetSparkAmount(int blue, int orange) {
        transform.GetChild(0).GetComponent<ParticleSystem>().emissionRate = blue;
        transform.GetChild(1).GetComponent<ParticleSystem>().emissionRate = blue;
        transform.GetChild(2).GetComponent<ParticleSystem>().emissionRate = orange;
        transform.GetChild(3).GetComponent<ParticleSystem>().emissionRate = orange;
    }
}
