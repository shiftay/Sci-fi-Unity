using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown( PointerEventData eventData )
    {
    }
 
    public void OnPointerUp( PointerEventData eventData )
    {
    }
 
    public void OnPointerClick(PointerEventData data)  {
        Debug.Log(this.name + " was clicked.");
    }


    
        //     if ((Time.time - _doubleClickStart) < 0.3f)
        //     {
        //         _doubleClickStart = -1;
        //         // Double Clicked;

        //         // Move ship
        //         UpdateCameraObject(playerObject.transform);
        //         _moveShip = true;

        //     }
        //     else
        //     {
        //         _doubleClickStart = Time.time;
        //     }

}
