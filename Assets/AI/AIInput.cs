using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    KartController kc;

    WorldGenerator wg;

    Transform player;

    public float turnSpeed;

    float perlinCount;
    float perlinNoise;

    Vector3 goalPos;

    public bool doDrift;

    // Start is called before the first frame update
    void Start()
    {
        kc = GetComponent<KartController>();
        perlinCount = Random.Range(0f, 10000f);

        wg = GameObject.Find("World Generator").GetComponent<WorldGenerator>();
        GetNewGoalPos();

        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float xStickAmt = 0f;

        doDrift = false;

        if (kc.isGrounded) {
            xStickAmt = getXAmount();


            Transform nearestObstacle = GetNearestObstacle();
            if (!nearestObstacle) {
                // Do nothing
            }
            else if (nearestObstacle.tag == "Boost") {
                goalPos = nearestObstacle.position + (Vector3.left * Random.Range(-2f, 2f));
                VerifyGoalPos();
            } else if (nearestObstacle.tag == "Stop" && Vector3.Distance(transform.position, nearestObstacle.position) < 10f) {
                //goalPos = nearestObstacle.position;
                //driftTime = 1f;
                if (nearestObstacle.position.x < transform.position.x)
                    //goalPos += Vector3.right * 5f;
                    xStickAmt = 1;
                else
                    //goalPos += Vector3.left * 5f;
                    xStickAmt = -1;
                //VerifyGoalPos();
            }

            if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
                GetNewGoalPos();
        }


        kc.SetJoystick(xStickAmt, 1, doDrift);

        if (transform.position.z >= goalPos.z)
            GetNewGoalPos();

        RubberBand(); // Yes...this game has rubber banding
    }

    float getXAmount() {
        Vector3 relativeVector = transform.InverseTransformPoint(goalPos);
        float newSteer = relativeVector.x / relativeVector.magnitude;
        newSteer *= 2;
        //if (Mathf.Abs(newSteer) > 1) {
        //    //Debug.Log("DRIFT!");
        //    doDrift = true;
        //} else if (Mathf.Abs(newSteer) < 0.2) {
        //    doDrift = false;
        //}
        newSteer = Mathf.Clamp(newSteer * 2, -1, 1);
        //Debug.Log(newSteer);
        return newSteer;
    }

    Transform GetNearestObstacle() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f)) {
            return hit.transform;
        }
        return null;
    }

    float GetObstacleDist(float lOffset) {
        RaycastHit hit;
        Quaternion offsetRot = Quaternion.Euler(0, lOffset, 0);
        Debug.DrawRay(transform.position, offsetRot * transform.forward);
        if (Physics.Raycast(transform.position, offsetRot * transform.forward, out hit, 10f, 1 << 6)) {
            //Debug.Log("Going to hit something! " + hit.transform.name);
            return Vector3.Distance(transform.position, hit.point);
        }
        return -1f;
    }

    void GetNewGoalPos() {
        goalPos = transform.position + (Vector3.forward * Random.Range(10f, 40f)) + (Vector3.left * Random.Range(-80f, 80f));
        VerifyGoalPos();
    }

    void VerifyGoalPos() {
        if (goalPos.x < 30)
            goalPos.x = 30;
        if (goalPos.x > (wg.mapWidth) - 30)
            goalPos.x = (wg.mapWidth) - 30;
    }

    void RubberBand() {
        kc.SetRubberBand(1 + ((player.position.z - transform.position.z) / 200f));

        if (player.position.z - transform.position.z > 200) {
            // This usually means the kart got stuck, so place it back in the map
            //Debug.Log("Teleporting!");
            transform.position = player.position + (Vector3.up * 20f) - (Vector3.forward * 20);
        }
    }
}
