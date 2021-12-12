using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OuterSpaceManager : MonoBehaviour
{
    public static OuterSpaceManager instance;
    [SerializeField] GameObject freeLookCamera;
    [SerializeField] GameObject empyObject;
    [SerializeField] GameObject playerObject;
    [SerializeField] Animator _descriptorAnimator;
    CinemachineFreeLook freeLookComponent;
    const float PLANETZOOM = 350.0f;
    const float ZOOMMAX = 200.0f, ZOOMMIN = 50.0f, ZOOMSTEP = 10.0f;
    const float SWIVELMAX = 250.0f, SWIVELMIN = 5.0f, SWIVELSTEP = 5.0f;
    const float SWIVELDEFAULT = 200.0f, ZOOMDEFAULT = 100.0f;
    const float YPOINT = 67.78934f;
    private float _doubleClickStart;
    private bool _moveShip, _moveCamera, _ignoreCamera;
    private SpaceObject objectSelected = null;
    [SerializeField] Vector3 currentTrackingpoint;

    private float currentZoom;
    
    public float CameraSpeed, ShipSpeed; // Set within Inspector

    private void Awake()
    {
        instance = this;
        freeLookComponent = freeLookCamera.GetComponent<CinemachineFreeLook>();

        currentZoom = freeLookComponent.m_Lens.OrthographicSize;
        // freeLookComponent.m_Lens.NearClipPlane = -90.0f;
    }

    private void Update()
    {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations
        // Move ship
        if(_moveShip) 
        {
            playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, empyObject.transform.position, ShipSpeed * Time.deltaTime);
            playerObject.transform.rotation = Quaternion.LookRotation(empyObject.transform.position - playerObject.transform.position, Vector3.up);
            if(Vector3.Distance(playerObject.transform.position, empyObject.transform.position) < 10.0f) 
            {
                _moveShip = false;
            }
        }


        if(_moveCamera) {

            empyObject.transform.position = Vector3.MoveTowards(empyObject.transform.position, currentTrackingpoint, CameraSpeed * Time.deltaTime);
            if(Vector3.Distance(currentTrackingpoint, empyObject.transform.position) < 10.0f) 
            {
                _moveCamera = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Unlock the camera to swivel
            freeLookComponent.m_XAxis.m_MaxSpeed = 500;
        }
        if (Input.GetMouseButtonUp(1))
        {
            // Upon release stop moving the camera
            freeLookComponent.m_XAxis.m_MaxSpeed = 0;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            //invert the scroll
            UpdateZoom((int)Mathf.Sign(Input.mouseScrollDelta.y) * -1);
        }
    }


    void UpdateCameraObject(Transform T_FollowLookObject, float zoom = 0.0f) {
        freeLookComponent.m_Follow = freeLookComponent.m_LookAt = T_FollowLookObject;

        if(zoom > 0) freeLookComponent.m_Lens.OrthographicSize = zoom;
    }

    void UpdateZoom(int direction) {
        currentZoom = freeLookComponent.m_Lens.OrthographicSize;
        
        if(currentZoom + (ZOOMSTEP * direction) > ZOOMMAX || currentZoom + (ZOOMSTEP * direction) < ZOOMMIN) return;

        freeLookComponent.m_Lens.OrthographicSize = currentZoom + (ZOOMSTEP * direction);
    }

    void UpdateSwivel(int direction) 
    {
        Debug.Log("Direction " + direction);

        float currentZoom = freeLookComponent.m_Orbits[0].m_Height;
        
        if(currentZoom + (ZOOMSTEP * direction) > ZOOMMAX || currentZoom + (ZOOMSTEP * direction) < ZOOMMIN) return;


        for(int i = 0; i < freeLookComponent.m_Orbits.Length; i++) 
        {
            freeLookComponent.m_Orbits[i].m_Height = freeLookComponent.m_Orbits[i].m_Radius = currentZoom + (ZOOMSTEP * direction);
        }
    }
    
    // Remove Y Axis from the equation
    Vector3 ConformHitPoint(Vector3 hit) 
    {
        return new Vector3(hit.x, YPOINT, hit.z);
    }


    public void ResetCamera() 
    {
        UpdateCameraObject(playerObject.transform);

        objectSelected = null;

        for(int i = 0; i < freeLookComponent.m_Orbits.Length; i++) 
        {
            freeLookComponent.m_Orbits[i].m_Height = freeLookComponent.m_Orbits[i].m_Radius = SWIVELDEFAULT;
        }

        freeLookComponent.m_Lens.OrthographicSize = ZOOMDEFAULT;
    }

    public void TestCameraStuff(Transform t) {
        freeLookComponent.m_Follow = null;
        freeLookComponent.m_LookAt = t;

    }

    public void ClickedObject(Transform obj, SpaceObject spaceObj) {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations

        switch(spaceObj.type) {
            case SPACEOBJECT.PLANET:
                currentZoom = freeLookComponent.m_Lens.OrthographicSize;
                StartCoroutine(CameraTransistion(obj, spaceObj, PLANETZOOM));
                break;
        }
    }


    IEnumerator CameraTransistion(Transform t, SpaceObject spaceObj, float Zoom) {
        while(Vector3.Distance(t.transform.position, empyObject.transform.position) > 100 || freeLookComponent.m_Lens.OrthographicSize < Zoom) {
            if(Vector3.Distance(t.transform.position, empyObject.transform.position) > 100) {
                empyObject.transform.position = Vector3.MoveTowards(empyObject.transform.position, t.transform.position, CameraSpeed * Time.deltaTime);
            }

            if(Vector3.Distance(t.transform.position, empyObject.transform.position) <= 100) {
                freeLookComponent.m_Lens.OrthographicSize += ZOOMSTEP;
            }

            yield return new WaitForFixedUpdate();
        }

        freeLookComponent.m_Lens.OrthographicSize = Zoom;

        TestCameraStuff(t);
        

        // Set up the UI For the planet.
        _descriptorAnimator.SetTrigger("Open");
    }


    public void ClickedBackdrop(Vector3 point) {
        if(_ignoreCamera) return; // Currently in a UI State so ignore camera operations

        currentTrackingpoint = ConformHitPoint(point);
        _moveCamera = true;
        UpdateCameraObject(empyObject.transform);
    }
}

