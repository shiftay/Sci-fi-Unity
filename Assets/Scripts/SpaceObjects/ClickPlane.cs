using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPlane : SpaceObject
{


    override public void OnPointerClick(PointerEventData data)  { 
        if(data.button == PointerEventData.InputButton.Right) return;
        OuterSpaceManager.instance.ClickedBackdrop(data.pointerCurrentRaycast.worldPosition);
        

        // Debug.Log("THIS IS THE CLICKPLANE" +  );
    }

}
