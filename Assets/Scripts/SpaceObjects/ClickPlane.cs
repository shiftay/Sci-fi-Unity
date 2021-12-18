using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPlane : SpaceObject
{

    public override void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);

        if(data.button == PointerEventData.InputButton.Right) return;

        if(Time.frameCount % 10 == 0) OuterSpaceManager.instance.ClickedBackdrop(data.pointerCurrentRaycast.worldPosition);
    }

    public override void OnEndDrag(PointerEventData data) {
        base.OnEndDrag(data);

        if(data.button == PointerEventData.InputButton.Right) return;

        OuterSpaceManager.instance.SetFollowTarget(null);
    }

    public override void OnPointerClick(PointerEventData data)  { 
        if(data.clickCount > 1) {
            // Move the ship.
            Debug.Log("Clicked???");
            OuterSpaceManager.instance.MoveShip(data.pointerCurrentRaycast.worldPosition);
        } 
    }

}
