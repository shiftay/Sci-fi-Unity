using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SPACEOBJECT { PLANET, PLAYER, CLICKPLANE }

public class SpaceObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public SPACEOBJECT type;
    public void OnPointerDown( PointerEventData eventData )
    {
    }
 
    public void OnPointerUp( PointerEventData eventData )
    {
    }
 
    public virtual void OnPointerClick(PointerEventData data)  {
        
        // OuterSpaceManager.instance.
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
