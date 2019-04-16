using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNode : MonoBehaviour
{
    public int FINGER_ID;
    public float X;
    public float Y;

    public float xoffset; 
    public float yoffset;

    public bool isSetFingerId;
    // Start is called before the first frame update
    void Awake()
    {

        X = this.transform.localPosition.x;
        Y = this.transform.localPosition.y;


    }

    public void Update()
    {
        if (isSetFingerId) {
            foreach (var touch in Input.touches)
            {
                if (touch.fingerId == FINGER_ID) {
                    this.transform.localPosition = new Vector3( touch.position.x+xoffset,touch.position.y+yoffset);

                    X = this.transform.localPosition.x;
                    Y = this.transform.localPosition.y;

                  //  Debug.Log("touchPositionX" + touch.position.x + "________" + "TransformPosition" + this.transform.localPosition.x);

                }
            }
        }
    }
}
