using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public bool spin;

    // Start is called before the first frame update
    void Start()
    {
        if (spin)
            GetComponent<Rigidbody>().angularVelocity = new Vector3(
                Random.Range(-3f, 3f),
                Random.Range(-3f, 3f),
                Random.Range(-3f, 3f)
            );
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (spin)
    //        transform.Rotate(30 * Time.deltaTime, 50 * Time.deltaTime, 70 * Time.deltaTime);
    //}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Kart") {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Kart") {
            Destroy(transform.parent.gameObject);
        }
    }
}
