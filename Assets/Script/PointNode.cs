using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNode : MonoBehaviour
{

    public float X;
    public float Y;
    // Start is called before the first frame update
    void Awake()
    {

        X = this.transform.localPosition.x;
        Y = this.transform.localPosition.y;


    }

    public void Update()
    {
        X = this.transform.localPosition.x;
        Y = this.transform.localPosition.y;
    }

}
