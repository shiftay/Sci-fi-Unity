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
    CinemachineFreeLook freeLookComponent;
    const float ZOOMMAX = 200.0f, ZOOMMIN = 50.0f, ZOOMSTEP = 10.0f;
    const float SWIVELMAX = 250.0f, SWIVELMIN = 5.0f, SWIVELSTEP = 5.0f;
    const float SWIVELDEFAULT = 200.0f, ZOOMDEFAULT = 100.0f;
    const float YPOINT = 67.78934f;
    RaycastHit hit;
    Ray ray;
    private float _doubleClickStart;
    private bool _moveShip;
    private SpaceObject objectSelected = null;


    private void Awake()
    {
        instance = this;
        freeLookComponent = freeLookCamera.GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        // Move ship
        if(_moveShip) 
        {
            playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, empyObject.transform.position, 20.0f * Time.deltaTime);
            playerObject.transform.rotation = Quaternion.LookRotation(empyObject.transform.position - playerObject.transform.position, Vector3.up);
            if(Vector3.Distance(playerObject.transform.position, empyObject.transform.position) < 10.0f) 
            {
                _moveShip = false;
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


    void UpdateCameraObject(Transform T_FollowLookObject) {
        freeLookComponent.m_Follow = freeLookComponent.m_LookAt = T_FollowLookObject;
    }


    void UpdateZoom(int direction) {
        float currentZoom = freeLookComponent.m_Lens.OrthographicSize;
        
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
}

