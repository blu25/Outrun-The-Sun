using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject AIKart;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=-30; i<=30; i+= 20) {
            for (int j=0; j<=60; j+= 20) {
                Instantiate(AIKart, transform.position + (Vector3.right * i) + (Vector3.forward * j), Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CalculatePlayerPlace() {
        float valToBeat = player.position.z;
        int place = 1;
        foreach (Transform child in transform) {
            if (child.position.z > valToBeat)
                place++;
        }

        scoreHolder.place = place;
        return place;
    }
}
