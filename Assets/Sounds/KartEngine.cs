using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartEngine : MonoBehaviour
{
	AudioSource aud;
    Transform player;
    KartController kc;
    public AudioSource drift_aud;
    public AudioSource off_aud;
    public AudioSource music_aud;

    bool isDrifting = false;
    bool wasAlive = true;

    // Start is called before the first frame update
    void Start()
    {
		aud = GetComponent<AudioSource>();
        player = transform.parent.transform;
        kc = player.GetComponent<KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        aud.volume = Mathf.Clamp(speed / 20f, 0, 0.35f);
        aud.pitch = Mathf.Clamp((speed / 40f) + 0.95f, 1f, 2f);

        if (!drift_aud.isPlaying && kc.DriftTime() > 0) {
            drift_aud.Play();
        } else if (drift_aud.isPlaying && kc.DriftTime() <= 0) {
            drift_aud.Stop();
        }

        if (!kc.IsAlive() && wasAlive) {
            off_aud.Play();
            music_aud.Stop();
            wasAlive = false;
        }
    }
}
