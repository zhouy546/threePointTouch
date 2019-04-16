using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Finger : MonoBehaviour
{

    public PointNode pointNode;


    public int id;
    public float X;
    public float Y;

    public float xoffset;
    public float yoffset;

    public bool isSetFingerId;


    void Awake()
    {
        X = this.transform.localPosition.x;
        Y = this.transform.localPosition.y;

        //xoffset = -Screen.resolutions[0].width / 2;
        //yoffset = -Screen.resolutions[0].height / 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ini(int _id) {
        id = _id;

             isSetFingerId = true;

    }

    public void DestoryMe() {
            Destroy(this.gameObject, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (isSetFingerId)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.fingerId ==id)
                {
                    this.transform.localPosition = new Vector3(touch.position.x + xoffset, touch.position.y + yoffset);

                    X = this.transform.localPosition.x;
                    Y = this.transform.localPosition.y;

                    //  Debug.Log("touchPositionX" + touch.position.x + "________" + "TransformPosition" + this.transform.localPosition.x);

                }
            }
        }
    }
}
