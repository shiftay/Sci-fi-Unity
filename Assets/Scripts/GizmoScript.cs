using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHAPE { CUBE, WIRECUBE, SPHERE, WIRESPHERE }

public class GizmoScript : MonoBehaviour
{
    public Color color;
    public float Size;
    public SHAPE shape;

    private void OnDrawGizmos() {
        Gizmos.color = color;

        switch(shape) 
        {
            case SHAPE.CUBE:
                Gizmos.DrawCube(this.transform.position, Vector3.one * Size);
                break;
            case SHAPE.WIRECUBE:
                Gizmos.DrawWireCube(this.transform.position, Vector3.one * Size);
                break;
            case SHAPE.SPHERE:
                Gizmos.DrawSphere(this.transform.position, Size);
                break;
            case SHAPE.WIRESPHERE:
                Gizmos.DrawWireSphere(this.transform.position, Size);
                break;
        }

    }
}
