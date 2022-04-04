using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    KartController kc;

    // Start is called before the first frame update
    void Start()
    {
        kc = GetComponent<KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        kc.SetJoystick(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }
}
