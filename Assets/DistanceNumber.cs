using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceNumber : MonoBehaviour
{
	public Transform player;
	Text txt;

	// Start is called before the first frame update
	void Start()
    {
		txt = GetComponent<Text>();
	}

    // Update is called once per frame
    void Update()
    {
		txt.text = (int)(player.position.z / 100)*100 + "m";
	}
}
