using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrabObject : MonoBehaviour
{
   
    bool isGrabbing = false;

    GameObject grabbedObject;
   
    public LayerMask grabbedLayer;
  
    public float grabRange = 0.2f;
  
    Vector3 prevPos;
   
    float throwPower = 10;
    
    Quaternion prevRot;
    
    public float rotPower = 5;
  
    public bool isRemoteGrab = true;
  
    public float remoteGrabDistance = 20;

    void Update()
    {
       
        if (isGrabbing == false)
        {
       
            TryGrab();
        }
        else
        {

            TryUngrab();
        }
    }

    private void TryGrab()
    {
       
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            
            if (isRemoteGrab)
            {
                
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;

               
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance,grabbedLayer))
                {
                   
                    isGrabbing = true;
                    
                    grabbedObject = hitInfo.transform.gameObject;
                    
                    StartCoroutine(GrabbingAnimation());
                }
                return;
            }

  
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);
            
            int closest = 0;

            
            for (int i = 1; i < hitObjects.Length; i++)
            {
            
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos, ARAVRInput.RHandPosition);

              
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);

               
                if (nextDistance < closestDistance)
                {
                   
                    closest = i;
                }
            }
           
            if (hitObjects.Length > 0)
            {
             
                isGrabbing = true;
              
                grabbedObject = hitObjects[closest].gameObject;
           
                grabbedObject.transform.parent = ARAVRInput.RHand;
             
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                              
                prevPos = ARAVRInput.RHandPosition;
                
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    private void TryUngrab()
    {
        
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
       
        prevPos = ARAVRInput.RHandPosition;
      
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
       
        prevRot = ARAVRInput.RHand.rotation;


      
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
           
            isGrabbing = false;
           
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            
            grabbedObject.transform.parent = null;
       
            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;
            
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

            grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimation()
    {
      
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

        prevPos = ARAVRInput.RHandPosition;
   
        prevRot = ARAVRInput.RHand.rotation;
        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;

        float elapsedRate = currentTime / finishTime;

        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;

            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);

            yield return null;
        }

      
        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = ARAVRInput.RHand;
    }
}
