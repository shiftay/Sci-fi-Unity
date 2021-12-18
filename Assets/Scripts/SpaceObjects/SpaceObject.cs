using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SPACEOBJECT { PLANET, PLAYER, CLICKPLANE }

public class SpaceObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public SPACEOBJECT type;
    public string objName;
    public virtual void OnPointerDown( PointerEventData data) {}
    public virtual void OnPointerUp( PointerEventData data) {}
    public virtual void OnPointerClick(PointerEventData data) {}
    public virtual void OnDrag(PointerEventData data) {}
    public virtual void OnEndDrag(PointerEventData data) {}
    public virtual void OnBeginDrag(PointerEventData data) {}

}
