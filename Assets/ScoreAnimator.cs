using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnimator : MonoBehaviour
{
    float time = -1f;

    public AudioSource scoreHigh;

    public Text yourScore;
    public Text highScore;
    public Text yourPlace;

    bool isHigh = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }

        for (int i=1; i < 6; i++) {
            Invoke("PlayTick", i);
        }

        if (scoreHolder.distance >= scoreHolder.maxDistance ||
            scoreHolder.time >= scoreHolder.maxTime) {
            isHigh = true;
            Invoke("PlayHigh", 5);
        }

        yourPlace.text = scoreHolder.place + scoreHolder.GetOrdinalSuffix(scoreHolder.place);
        yourScore.text = (int)scoreHolder.distance + " meters\n" + (int)scoreHolder.time + " seconds";
        highScore.text = (int)scoreHolder.maxDistance + " meters\n" + (int)scoreHolder.maxTime + " seconds";
        if (isHigh)
            highScore.text += "\nNew high score!";

        if (scoreHolder.distance > scoreHolder.maxDistance)
            scoreHolder.maxDistance = scoreHolder.distance;
        if (scoreHolder.time > scoreHolder.maxTime)
            scoreHolder.maxTime = scoreHolder.time;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        foreach (Transform child in transform) {
            //Debug.Log(child.GetSiblingIndex() + "vs " + time);
            if (child.GetSiblingIndex() < time)
                child.gameObject.SetActive(true);
        }
    }

    void PlayTick() {
        GetComponent<AudioSource>().Play();
    }

    void PlayHigh() {
        scoreHigh.Play();
    }
}
