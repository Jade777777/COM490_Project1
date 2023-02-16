using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;

    public LineRenderer lr;

    public Vector3 originScale = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
 

            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = LayerMask.GetMask("Terrain");
            if (Physics.Raycast(ray, out hitInfo, 200, layer))
            {

                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);

                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;

                teleportCircleUI.forward = hitInfo.normal;

                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance);
            }
            else
            {
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin+ARAVRInput.LHandDirection*200);
                teleportCircleUI.gameObject.SetActive(false);

            }
        }
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false;
            teleportCircleUI.gameObject.SetActive(false);
        }

    }
}
