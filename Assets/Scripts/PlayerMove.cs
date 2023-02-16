using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 5;

    public float jumpPower = 5;

    CharacterController cc;

    float yVelocity = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");


        Vector3 dir = new Vector3( h, 0, v);

        dir = Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y,0)*dir;

        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        yVelocity += Physics.gravity.y*Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);

    }
}
