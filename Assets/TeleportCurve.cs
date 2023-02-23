using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI;


    LineRenderer lr;

    Vector3 originalScale =Vector3.one *0.2f;

    public int lineSmooth = 40;

    public float curveLength = 50;

    public float gravity = -60;

    public float simulateTime = 0.02f;

    List<Vector3> lines = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);

        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.0f;
        lr.endWidth = 0.02f;

    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false;
            if (teleportCircleUI.gameObject.activeSelf)
            {
  
                    GetComponent<CharacterController>().enabled = false;
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                
            }
            teleportCircleUI.gameObject.SetActive(false);
        }
        else if(ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }

    }

    private void MakeLines()
    {
        lines.RemoveRange(0, lines.Count);
        Vector3 dir = ARAVRInput.LHandDirection * curveLength;
        Vector3 pos = ARAVRInput.LHandPosition;
        lines.Add(pos);
        
    }
}
