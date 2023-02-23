using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;

    private LineRenderer lr;

    public Vector3 originScale = Vector3.one *0.2f;

  
    public bool isWarp = false;

    public float warpTime =0.1f;

    [SerializeField]
    public PostProcessVolume post;
    
    // Start is called before the first frame update
    void Start()
    {
        
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
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
                if (isWarp == false)
                {
                    GetComponent<CharacterController>().enabled = false;
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else
                {
                    StartCoroutine(Warp());
                }
            }
            teleportCircleUI.gameObject.SetActive(false);
        }


        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
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
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);

            }
        }

    }
    IEnumerator Warp()
    {
        MotionBlur blur;
        Vector3 pos = transform.position;
        Vector3 targetPos = teleportCircleUI.position+Vector3.up;

        float currentTime = 0;

        post.profile.TryGetSettings(out blur);

        blur.active = true;

        GetComponent<CharacterController>().enabled = false;

        while (currentTime < warpTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);
            yield return null;
        }
        transform.position = teleportCircleUI.position + Vector3.up;
        GetComponent<CharacterController>().enabled = true;
        blur.active = false;


    }


    
}
