using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceNumber : MonoBehaviour
{
    public AISpawner ai;
    Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int place = ai.CalculatePlayerPlace();
        txt.text = place.ToString() + scoreHolder.GetOrdinalSuffix(place);
    }

	
}
