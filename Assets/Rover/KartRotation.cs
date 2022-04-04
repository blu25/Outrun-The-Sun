using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRotation : MonoBehaviour
{
    const float ROTATION_SPEED = 10f;
    const float SPIN_CONSTANT = 100f;
    const float SPIN_CAP = 750f;

    KartController kc;

    bool wasGrounded = true;

    Quaternion freezeRotation;

    Vector3 curAirTricks;

    float maxDeltaAirTricks;
    public GameObject trickObj;

    // Start is called before the first frame update
    void Start()
    {
        kc = transform.parent.GetComponent<KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (kc.isGrounded) {
            if (!wasGrounded) {
                transform.rotation = freezeRotation;
                curAirTricks = Vector3.zero;
                wasGrounded = true;
                maxDeltaAirTricks = 0f;
                //Debug.Log("RESETTING");
            }
            GetGroundRotation();
        } else {
            // TODO: rotation
            if (wasGrounded) {
                freezeRotation = transform.rotation;
                wasGrounded = false;
            }

            curAirTricks = Vector3.Lerp(curAirTricks, kc.airTricks, Time.deltaTime * 10f);

            doTrickEffect();

            transform.rotation = freezeRotation * Quaternion.Euler(curAirTricks);
        }

        SpinWheels();
    }

    void GetGroundRotation() {
        RaycastHit hit;

        Quaternion upCalc = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);

        if (Physics.Raycast(transform.position + transform.forward, Vector3.down, out hit, 100f, 1 << 6)) {
            transform.up -= (transform.up - hit.normal) * Time.deltaTime * ROTATION_SPEED;
            transform.rotation = transform.rotation * transform.parent.rotation;
        }
    }

    void SpinWheels() {
        float velo = transform.parent.InverseTransformDirection(transform.parent.GetComponent<Rigidbody>().velocity).z;
        for (int i=1; i<5; i++) {
            int multiplier = (kc.IsDrifting() && i % 2 == 0 ? -1 : 1); // Rotate the wheel in different directions in drift mode
            transform.GetChild(i).transform.Rotate(0, 0, Mathf.Clamp(-velo * SPIN_CONSTANT * multiplier, -SPIN_CAP, SPIN_CAP) * Time.deltaTime);
        }
    }

    void doTrickEffect() {
        float curDeltaAirTricks = kc.airTricks.magnitude + 1;
        if ((int)(curDeltaAirTricks / 360) > (int)(maxDeltaAirTricks / 360)) {
            if (trickObj)
                Instantiate(trickObj, transform);
        }
        maxDeltaAirTricks = Mathf.Max(curDeltaAirTricks, maxDeltaAirTricks);
    }
}
