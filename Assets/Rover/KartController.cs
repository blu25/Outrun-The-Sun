using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    Rigidbody rb;

    public float topSpeed;
    public float acceleration;

    public float boostSpeed;
    public float boostAccel;
    float boostTime = 0f;

    float rubberBandVal = 1f;

    public float rotationSpeed;
    public float rotationAccel;
    float curRotationSpeed = 0f;

    public float alignSpeed; // How fast the kart aligns with the ground

    public float airTricksSpeed; // How fast you rotate in the air

    Vector2 joystick;

    public Vector3 airTricks;
    float airTime;

    public bool isGrounded = true;
    public bool wasGrounded = true;

    float locRotX = 0, locRotZ = 0;

    public float topDriftSpeed;
    public float driftSlide;
    bool isDrifting = false;
    int driftDirection = 0;
    float driftTime = 0f;
    bool driftCooldown = false; // Used to prevent turning for a bit after drifting

    SceneryControl sc;

    public GameObject hitSound;
    public GameObject groundSound;
    public GameObject bigBoost;
    public GameObject smallBoost;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = FindObjectOfType<SceneryControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive())
            return;

        if (isGrounded) {
            if (!wasGrounded) {
                // Figure out if the player gets a boost after landing
                CalculateTrickBoost();
                airTricks = Vector3.zero;
                wasGrounded = true;
            }

            if (boostTime <= 0f) {
                // Add basic speed, slowing down if drifting
                if (rb.velocity.magnitude < (driftDirection == 0 ? topSpeed * rubberBandVal : topDriftSpeed)) {
                    //rb.AddRelativeForce(Vector3.forward * acceleration * joystick.y, ForceMode.Acceleration);
                    rb.velocity += transform.forward * acceleration * joystick.y * Time.deltaTime;
                }
            } else {
                // Go a lot faster if we're boosting
                boostTime -= Time.deltaTime;
                if (rb.velocity.magnitude < boostSpeed * rubberBandVal) {
                    //rb.AddRelativeForce(Vector3.forward * boostAccel, ForceMode.Acceleration);
                    rb.velocity += transform.forward * boostAccel * Time.deltaTime;
                }
                if (rb.velocity.magnitude < boostSpeed * rubberBandVal * 0.25f) {
                    rb.velocity += transform.forward * boostSpeed * rubberBandVal * 0.25f;
                }
            }

            HandleDrift();

            // Add turning rotation
            //if (driftCooldown <= 0f && rb.angularVelocity.magnitude < rotationSpeed)
            //    rb.angularVelocity = Vector3.up * rotationAccel * joystick.x;
            if (!driftCooldown) {
                curRotationSpeed = Mathf.Lerp(curRotationSpeed, rotationSpeed * joystick.x * (boostTime > 0f ? 0.5f : 1f), Time.deltaTime * 10f);
                transform.Rotate(Vector3.up * Time.deltaTime * curRotationSpeed);
            }
            if (driftCooldown && Mathf.Abs(joystick.x) < 0.1f) {
                driftCooldown = false;
            }
        } else {
            if (wasGrounded) {
                wasGrounded = false;
                boostTime = 0f;
                //rb.angularVelocity = Vector3.zero;
            }

            if (rb.velocity.magnitude < topSpeed) {
                rb.AddRelativeForce(Vector3.forward * acceleration, ForceMode.Acceleration);
            }

            airTricks = new Vector3(
                Mathf.Clamp(airTricks.x - (joystick.y * airTricksSpeed * Time.deltaTime), -360, 360),
                airTricks.y + (joystick.x * airTricksSpeed * Time.deltaTime),
                0);
            airTime += Time.deltaTime;
        }

        CheckGrounded();
    }

    void CalculateTrickBoost() {
        if ((Mathf.Abs(airTricks.x) + 90) % 360 <= 180f &&
            (Mathf.Abs(airTricks.y) + 90) % 360 <= 180f) {
            // Landed well, get a boost
            if (airTime > 0.5f) {
                boostTime = (Mathf.Abs(airTricks.x) + Mathf.Abs(airTricks.y)) * airTime / 2000f;
                if (boostTime > 1f)
                    Instantiate(bigBoost, transform);
                else if (boostTime > 0.1f)
                    Instantiate(smallBoost, transform);
            }
            Instantiate(groundSound, transform);
        } else {
            // Landed poorly, stop in your tracks
            boostTime = 0f;
            rb.velocity = Vector3.zero;
            Instantiate(hitSound, transform);
            //rb.angularVelocity = Vector3.zero;

        }
        airTime = 0f;
    }

    void HandleDrift() {
        if (!isDrifting) {
            if (driftTime > 0f) {
                // Give the player a boost from the drift
                if (driftTime >= 0.5f) {
                    boostTime += 0.1f;
                    Instantiate(smallBoost, transform);
                }
                if (driftTime >= 1.5f)
                    boostTime += 0.3f;

                driftDirection = 0;
                driftTime = 0f;
            }

            return;
        }

        driftCooldown = true;

        if (driftDirection == 0) {
            if (joystick.x >= 0.1)
                driftDirection = 1;
            if (joystick.x <= -0.1)
                driftDirection = -1;
            return;
        }

        driftTime += Time.deltaTime;

        //transform.Translate(Vector3.right * driftDirection * driftSlide * Time.deltaTime);
        float driftControlInputMultiplier = (joystick.x + driftDirection) / 2f;
        //rb.AddRelativeForce(Vector3.right * driftSlide * driftControlInputMultiplier, ForceMode.Force);
        rb.velocity += Vector3.right * driftSlide * driftControlInputMultiplier * Time.deltaTime;
        //rb.AddRelativeForce((-Vector3.forward * driftSlowdown) + (Vector3.right * driftDirection * driftSlowdown), ForceMode.Acceleration);
    }

    public bool IsDrifting() {
        return isDrifting;
    }

    public float DriftTime() {
        return driftTime;
    }

    public int DriftDirection() {
        return driftDirection;
    }

    void CheckGrounded() {
        RaycastHit hit;
        if (isGrounded) {
            if (!Physics.Raycast(transform.position + transform.forward, Vector3.down, out hit, 1.5f, 1 << 6)) {
                isGrounded = false;
            }
        }

        if (rb.velocity.magnitude < 0.25f)
            isGrounded = true;
    }

    public float GetBoostTime() {
        return boostTime;
    }

    public void SetJoystick(float x, float y, bool drift) {
        joystick = new Vector2(x, y);
        isDrifting = drift;
    }

    private void OnCollisionEnter(Collision collision) {
        CheckTag(collision.gameObject.tag);

        if (!isGrounded && collision.gameObject.layer == 6) {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        CheckTag(other.gameObject.tag);
    }

    void CheckTag(string theTag) {
        if (theTag == "Boost") {
            boostTime = 1f;
            Instantiate(bigBoost, transform);
        } else if (theTag == "Stop") {
            rb.velocity = Vector3.zero;
            boostTime = 0f;
            Instantiate(hitSound, transform);
            //rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetRubberBand(float amount) {
        rubberBandVal = amount;
    }

    public bool IsAlive() {
        return sc.sunSet <= 110;
    }
}
