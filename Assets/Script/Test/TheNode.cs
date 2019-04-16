using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNode : MonoBehaviour
{
    public int[] num;
    public List<M_Finger> m_Fingers = new List<M_Finger>();

    private bool finishSetup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (finishSetup) {
            check();
            this.transform.localPosition = getNeiQieCenter(m_Fingers);
         
        }
   
    }

    void check() {
        try
        {
            foreach (var item in m_Fingers)
            {
                if (item == null)
                {
                    DestoryMe();
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
            DestoryMe();
            throw e;
        }

 
    }

    public void Ini(List<M_Finger> _m_Fingers,int[] _nums) {
        m_Fingers = _m_Fingers;
        finishSetup = true;
        num = _nums;
    }


    Vector3 getNeiQieCenter(List<M_Finger> node) {
        float x1, x2, y1, y2, z1, z2, a, b, c;
        x1 = node[0].X;
        x2 = node[0].Y;

        y1 = node[1].X;
        y2 = node[1].Y;

        z1 = node[2].X;
        z2 = node[2].Y;

        Vector2 A = new Vector2 (x1, x2);
        Vector2 B = new Vector2(y1, y2);
        Vector2 C = new Vector2(z1, z2);

        a = (C-B).magnitude;
        b = (C - A).magnitude;
        c = (B - A).magnitude;

        float x = (a * x1 + b * y1 + c * z1) / (a + b + c);
        float y = (a * x2 + b * y2 + c * z2) / (a + b + c);
        Vector2 CenterPos = new Vector2(x, y);

        return CenterPos;
    }

    //Vector3 getCenterPos(List<M_Finger> node)
    //{
    //    float x1, x2, x3, y1, y2, y3, A1, A2, B1, B2, C1, C2, X, Y;


    //    x1 = node[0].X;
    //    y1 = node[0].Y;

    //    x2 = node[1].X;
    //    y2 = node[1].Y;

    //    x3 = node[2].X;
    //    y3 = node[2].Y;

    //    A1 = 2 * (x2 - x1);
    //    B1 = 2 * (y2 - y1);
    //    C1 = x2 * x2 + y2 * y2 - x1 * x1 - y1 * y1;
    //    A2 = 2 * (x3 - x2);
    //    B2 = 2 * (y3 - y2);
    //    C2 = x3 * x3 + y3 * y3 - x2 * x2 - y2 * y2;
    //    X = ((B2 * C1) - (B1 * C2)) / ((B2 * A1) - (B1 * A2));
    //    Y = (-(A2 * C1) + (A1 * C2)) / ((B2 * B1) - (B1 * A2));

    //    return new Vector3(X, Y, 0);
    //}

    public void DestoryMe() {
        Destroy(this.gameObject);
    }
}
