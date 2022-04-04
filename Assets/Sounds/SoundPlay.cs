using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
	public AudioClip[] clips;
	AudioSource aud;

	// Start is called before the first frame update
	void Start()
	{
		aud = GetComponent<AudioSource>();

		aud.clip = clips[Random.Range(0, clips.Length)];
		aud.pitch = Random.Range(0.9f, 1.1f);
		aud.Play();
	}

	// Update is called once per frame
	void Update()
	{
        if (!aud.isPlaying)
            Destroy(gameObject);
	}
}