using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class SimpleLogic : MonoBehaviour
{
    public GameObject ObjToInstance;
    public GameObject IndicatorObj;
    ARRaycastManager raycastMgr;
    Pose placementPose;
    bool placementPoseIsValid = false;
    // Start is called before the first frame update
    void Start()
    {
        raycastMgr = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlacementPosition();
        Indicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ObjInstance();
        }
    }

    private void PlacementPosition()
    {
        var _screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var _hits = new List<ARRaycastHit>();
        raycastMgr.Raycast(_screenCenter, _hits, TrackableType.Planes);
        placementPoseIsValid = _hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = _hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var smartCamera = new Vector3(cameraForward.x, 0, cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(smartCamera);
        }
    }

    private void Indicator()
    {
        if (placementPoseIsValid)
        {
            IndicatorObj.SetActive(true);
            IndicatorObj.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
           
        else
            IndicatorObj.SetActive(false);
    }

    private void ObjInstance()
    {
        Instantiate(ObjToInstance, placementPose.position, placementPose.rotation);
    }
}
