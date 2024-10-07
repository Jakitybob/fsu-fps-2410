using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleR : MonoBehaviour
{
    [SerializeField] Camera playerrCam;
    [SerializeField] LayerMask grappable;
    [SerializeField] Transform grapTip;
    [SerializeField] float maxGrapDist = 50f;
    [SerializeField] float pullSpeed;
    [SerializeField] float Sstrength;
    [SerializeField] float DampStrength;
    [SerializeField] float MassScale;
   
   

     LineRenderer grapLine;
     Vector3 grappePoint;
     SpringJoint joint;
     Rigidbody playerRb;
     bool isGrappling;





    // Start is called before the first frame update
   void Awake() 
   {
        grapLine=gameObject.AddComponent<LineRenderer>();
      
        
   }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Q))
            {
               StartGrapple();
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                StopGrap();

            }

           if (isGrappling)
           {
              grapLine.SetPosition(0, grapTip.position);
              grapLine.SetPosition(1, grappePoint);
            
           } 
    }
   
     void StartGrapple() 
     {


        Ray ray= playerrCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray,out hit,maxGrapDist,grappable))
        {
            grappePoint = hit.point;
            isGrappling = true; 

            
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grappePoint;

            float distfrompoint = Vector3.Distance(transform.position, grappePoint);
            joint.maxDistance = distfrompoint * 50f;
            joint.minDistance = distfrompoint * 0f;
            
            joint.spring = Sstrength;
            joint.damper = DampStrength;
            joint.massScale = MassScale;
            
            grapLine.enabled = true;

        }
        
     }
    //removes joint and not grrappling 
     public void StopGrap() 
     {
        isGrappling= false;
        grapLine.enabled = false;
        Destroy(joint);
        
        
        
     }
  
    
    //are you gappling??
    public bool IsGrapple()
    {
        return isGrappling;
    }
    //where grapplepoint 
    public Vector3 GetGrapPoint()
    {
        return grappePoint.normalized;
    }
}
