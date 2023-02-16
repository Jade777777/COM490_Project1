using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //bullet 
    [SerializeField]
    private Transform bulletImpact;
    private ParticleSystem bulletParticle;
    private AudioSource bulletAudio;

    [SerializeField]
    private Transform crosshair;


    void Start()
    {
        bulletParticle = bulletImpact.GetComponent<ParticleSystem>();
        bulletAudio = bulletImpact.GetComponent<AudioSource>();
    }


    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);

        if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger))
        {

            bulletAudio.Stop();
            bulletAudio.Play();


            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

            RaycastHit hitInfo;

            int playerLayer = LayerMask.GetMask("Player");

            int towerLayer = LayerMask.GetMask("Tower");

            int layerMask = playerLayer | towerLayer;

            if(Physics.Raycast(ray,out hitInfo, layerMask))
            {
                //bullet effect
                bulletParticle.Stop();
                bulletParticle.Play();
                bulletImpact.position = hitInfo.point;
            }

        }
    }
}
