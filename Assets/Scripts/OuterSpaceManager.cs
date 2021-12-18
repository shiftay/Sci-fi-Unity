using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class OuterSpaceManager : MonoBehaviour
{

    private const int TOPPRIORITY = 100, LOWPRIORITY = 1;

    [SerializeField] CinemachineFreeLook freeLook;
    [SerializeField] CinemachineFreeLook eventLook;
    public static OuterSpaceManager instance;
    [SerializeField] GameObject freeLookCamera;
    [SerializeField] GameObject empyObject;
    [SerializeField] GameObject playerObject;
    [SerializeField] UIManager UIManager;
    [SerializeField] GalaxyManager GalManager;

    const float PLANETZOOM = 350.0f;
    const float ZOOMMAX = 200.0f, ZOOMMIN = 50f, ZOOMSTEP = 10.0f;
    const float SWIVELMAX = 45.0f, SWIVELMIN = 10.0f, SWIVELSTEP = 1.0f;
    const float SWIVELDEFAULT = 200.0f, ZOOMDEFAULT = 100.0f;
    const float YPOINT = 0.0f;
    private float _doubleClickStart;
    private bool _moveShip, _moveCamera, _ignoreCamera;
    private SpaceObject objectSelected = null;
    [SerializeField] Vector3 currentTrackingpoint;

    private float currentZoom;
    
    public float CameraSpeed, ShipSpeed; // Set within Inspector

    private void Awake()
    {
        instance = this;
        freeLook = freeLookCamera.GetComponent<CinemachineFreeLook>();

        currentZoom = freeLook.m_Lens.OrthographicSize;
        freeLook.Priority = TOPPRIORITY;
        eventLook.Priority = LOWPRIORITY;
    }

    private void Update()
    {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations
        // Move ship
        if(_moveShip) {
            playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, empyObject.transform.position, ShipSpeed * Time.deltaTime);
            playerObject.transform.rotation = Quaternion.LookRotation(empyObject.transform.position - playerObject.transform.position, Vector3.up);

            if(Vector3.Distance(playerObject.transform.position, empyObject.transform.position) < 10.0f) _moveShip = false;
        } else {
 
            // If the ship is moving, do not allow any of the following camera manipulation.
            if (Input.GetMouseButtonDown(1)) {
                // Unlock the camera to rotate on the x axis
                SetFollowTarget(freeLook.LookAt);
                freeLook.m_XAxis.m_MaxSpeed = 500;
            }
            /*
                Currently on hold, because interaction with Orthographic size doesn't fully work as intended.
                > Causes weird rotation issues.

            if(Input.GetMouseButton(1)) {
                // Update Swivel every few frames, otherwise it's too quick / reactive.
                if(Time.frameCount % 10 == 0 && freeLook.m_YAxis.m_InputAxisValue != 0) UpdateSwivel((int)Mathf.Sign(freeLook.m_YAxis.m_InputAxisValue));
            }

            */

            if (Input.GetMouseButtonUp(1)) {
                // Upon release stop rotating the camera
                SetFollowTarget(null);
                freeLook.m_XAxis.m_MaxSpeed = 0;
            }

            if (Input.mouseScrollDelta.y != 0) {
                //invert the scroll
                UpdateZoom((int)Mathf.Sign(Input.mouseScrollDelta.y) * -1);
            }
        }
    }

    internal void MoveShip(Vector3 point) {
        if(_moveShip) return;

        _moveShip = true;
        SetFollowTarget(playerObject.transform);
        empyObject.transform.position = ConformHitPoint(point);
    }


    void UpdateCameraObject(Transform T_FollowLookObject, float zoom = 0.0f) {
        SetUpCameraTargets(T_FollowLookObject);

        if(zoom > 0) freeLook.m_Lens.OrthographicSize = zoom;
    }

    void UpdateZoom(int direction) {
        currentZoom = freeLook.m_Lens.OrthographicSize;
        
        if(currentZoom + (ZOOMSTEP * direction) > ZOOMMAX || currentZoom + (ZOOMSTEP * direction) < ZOOMMIN) return;

        freeLook.m_Lens.OrthographicSize = currentZoom + (ZOOMSTEP * direction);
    }

    void UpdateSwivel(int direction) {
        float currentSwivel = freeLook.m_Orbits[1].m_Height;
        
        if(currentSwivel + (SWIVELSTEP * direction) > SWIVELMAX || currentSwivel + (SWIVELSTEP * direction) < SWIVELMIN) return;

        for(int i = 0; i < freeLook.m_Orbits.Length; i++) 
        {
            freeLook.m_Orbits[i].m_Height = freeLook.m_Orbits[i].m_Radius = currentSwivel + (SWIVELSTEP * direction);
        }
    }
    


    internal void SetFollowTarget(Transform T) {
        eventLook.m_Follow = freeLook.m_Follow = T;
    }

    // Remove Y Axis from the equation
    Vector3 ConformHitPoint(Vector3 hit) 
    {
        return new Vector3(hit.x, YPOINT, hit.z);
    }

    void SetUpCameraTargets(Transform lookObj_T) {
        //eventLook.m_Follow = freeLook.m_Follow =
        eventLook.m_LookAt = freeLook.m_LookAt = lookObj_T;
    }


    public void ResetCamera() 
    {
        UpdateCameraObject(playerObject.transform);

        objectSelected = null;
        SetFollowTarget(playerObject.transform);
        empyObject.transform.position = playerObject.transform.position;

        freeLook.m_Lens.OrthographicSize = ZOOMDEFAULT;
        SetFollowTarget(null);
    }

    public void EventCamera(Transform t) {
        eventLook.m_Follow = null;
        eventLook.m_LookAt = t;
    }

    public void ClickedObject(Transform obj, SpaceObject spaceObj) {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations

        switch(spaceObj.type) {
            case SPACEOBJECT.PLANET:
                currentZoom = freeLook.m_Lens.OrthographicSize;
                StartCoroutine(CameraTransistion(obj, spaceObj, PLANETZOOM));
                break;
        }
    }


    IEnumerator CameraTransistion(Transform t, SpaceObject spaceObj, float Zoom) {
        _ignoreCamera = true;
        EventCamera(t);
        eventLook.Priority = TOPPRIORITY;
        freeLook.Priority = LOWPRIORITY;

        yield return new WaitForSeconds(1.5f);

        // Set up the UI For the planet.
        UIManager.SetupPlanetDescription(GalManager.GetPlanet(spaceObj.objName));
        // _descriptorAnimator.SetTrigger("Open");
    }


    public void ClickedBackdrop(Vector3 point) {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations





        currentTrackingpoint = ConformHitPoint(Vector3.MoveTowards(empyObject.transform.position, point, (Vector3.Distance(empyObject.transform.position, point) / 4.0f)));

        // _moveCamera = true;

        UpdateCameraObject(empyObject.transform);
        SetFollowTarget(freeLook.m_LookAt);
        empyObject.transform.position = point;
    }
}

