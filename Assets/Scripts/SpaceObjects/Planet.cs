using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Planet : SpaceObject
{

    override public void OnPointerClick(PointerEventData data)  { 
        OuterSpaceManager.instance.ClickedObject(transform, this);
        Debug.Log("PLANET");
    }
}
