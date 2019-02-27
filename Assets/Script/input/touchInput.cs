using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class touchInput : MonoBehaviour
{
    public static touchInput instance;

    public const int pointDiv = 3;

    public const float Re_angleA = 60f;
    public const float Re_angleB = 60f;
    public const float Re_angleC = 60f;

    public const float angleThreshold = 10f;

    public const float MaxPerimeter = 1000;

    public int temp;

    public List<int[]> triID = new List<int[]>();
    public List<int[]> perviousTriID = new List<int[]>();


    public GameObject PrefabNode;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }
        EventCenter.AddListener(EventDefine.AddFinger, AddFinger);
        EventCenter.AddListener(EventDefine.RemoveFinger, removeFineger);
    }

    // Update is called once per frame
    void Update()
    {
     
                if (Input.touchCount > temp)
                {
                    temp++;
                    //add
                    EventCenter.Broadcast(EventDefine.AddFinger);
                }
                else if (Input.touchCount < temp)
                {

                    temp--;

                EventCenter.Broadcast(EventDefine.RemoveFinger);
            //remove
                  }

        //if (Input.touchCount != 0) {
        //    Debug.Log(Input.touches[0].fingerId);
        //}
    }

    void AddFinger() {
        triID = getTriID();
        int val = 0;
        int pval = 0;
        foreach (var item in triID)
        {
            foreach (var a in item)
            {
                val += a;
            }
            
        }

        foreach (var item in perviousTriID)
        {
            foreach (var a in item)
            {
                pval += a;
            }

        }

        Debug.Log("Add finger");
        foreach (var item in triID)
        {
            if (val!=pval) {
                GameObject g = Instantiate(PrefabNode);
                g.transform.SetParent(this.transform);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = Vector3.one;
                g.transform.localRotation = Quaternion.Euler(Vector3.zero);

                TouchManager touchManager = g.GetComponent<TouchManager>();
                touchManager.initilization(item,this.GetComponent<RectTransform>());
                StartCoroutine(SyncID());
            }
        }
     
    }

    public IEnumerator SyncID() {
        perviousTriID = triID;
        yield return new WaitForSeconds(0);
    }

    void removeFineger() {
        Debug.Log("Remove finger");
       triID = getTriID();


       StartCoroutine( RemoveTouchManager());     
    }


    IEnumerator RemoveTouchManager() {


        TouchManager[] touchManagers = FindObjectsOfType<TouchManager>();
        foreach (var item in touchManagers)
        {
         yield return  StartCoroutine  ( item.DestoryObject(getRemoveInt()));
        }

    }

    int[] getRemoveInt() {
        List<int> perviousTemp = new List<int>();
        List<int> triIDTemp = new List<int>();

        List<int> removeInt = new List<int>();
        foreach (var perviousID in perviousTriID)
        {
            for (int i = 0; i < perviousID.Length; i++)
            {
                Debug.Log("PERVIOUS____"+perviousID[i]);
                perviousTemp.Add(perviousID[i]);
            }
        }

        foreach (var triid in triID)
        {
            for (int i = 0; i < triid.Length; i++)
            {

                Debug.Log("Current____" + triid[i]);
                triIDTemp.Add(triid[i]);
            }
        }

        foreach (var item in perviousTemp)
        {
            if (!triIDTemp.Contains(item)){

                removeInt.Add(item);
            }

        }
        return removeInt.ToArray();
    }


    List<int[]> getTriID() {
        List<int[]> Tri = new List<int[]>();
        IEnumerable<int[]> unm = test(Input.touchCount);
        foreach (var item in unm)
        {
            int A = item[0]; ;
            int B = item[1];
            int C = item[2];


            Vector2 positionA = Input.touches[A].position;
            Vector2 positionB = Input.touches[B].position;
            Vector2 positionC = Input.touches[C].position;


            float angleA= getAngle(positionA, positionB, positionC);
            float angleB = getAngle(positionB, positionA, positionC);
            float angleC = getAngle(positionC, positionB, positionA);

            float TRIPerimeter = (positionC - positionA).magnitude+ (positionB - positionA).magnitude+ (positionC - positionB).magnitude;

            //Debug.Log(TRIPerimeter);
            if (checkAngle(angleA, angleB, angleC)&& TRIPerimeter<MaxPerimeter)
            {
                int[] array = new int[3];

                for (int i = 0; i < item.Length; i++)
                {
                    array[i] = Input.touches[item[i]].fingerId;
                }
               Tri.Add(array);
            }
        }
        return Tri;
    }

    bool checkAngle(float a,float b, float c) {
        float tempa = Re_angleA + angleThreshold;
        float temp_a = Re_angleA - angleThreshold;

        float tempb = Re_angleB + angleThreshold;
        float temp_b = Re_angleB - angleThreshold;

        float tempc = Re_angleC + angleThreshold;
        float temp_c = Re_angleC - angleThreshold;

        if (temp_a < a && a < tempa && temp_b < b && b < tempb && temp_c < c && c < tempc)
        {
            return true;
        }
        else {
            return false;
        }
    }


    float pi180 = 180 / Mathf.PI;

    float getAngle(Vector2 a,Vector2 b ,Vector2 c)
    {
        var _cos1 = getCos(a.x, a.y, b.x, b.y, c.x, c.y);//第一个点为顶点的角的角度的余弦值

        return Mathf.Acos(_cos1) * pi180;
    }

    //获得三个点构成的三角形的 第一个点所在的角度的余弦值
    float getCos(float point1_x, float point1_y, float point2_x, float point2_y, float point3_x, float point3_y)
    {
        var length1_2 = getLength(point1_x, point1_y, point2_x, point2_y);//获取第一个点与第2个点的距离
        var length1_3 = getLength(point1_x, point1_y, point3_x, point3_y);
        var length2_3 = getLength(point2_x, point2_y, point3_x, point3_y);

        float res = (Mathf.Pow(length1_2, 2) + Mathf.Pow(length1_3, 2) - Mathf.Pow(length2_3, 2)) / (length1_2 * length1_3 * 2);//cosA=(pow(b,2)+pow(c,2)-pow(a,2))/2*b*c

        return res;
    }

    //获取坐标轴内两个点间的距离
    float getLength(float point1_x,float point1_y,float point2_x,float point2_y)
    {
        var diff_x = Mathf.Abs(point2_x - point1_x);
        var diff_y = Mathf.Abs(point2_y - point1_y);

        var length_pow = Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2);//两个点在 横纵坐标的差值与两点间的直线 构成直角三角形。length_pow等于该距离的平方

        return Mathf.Sqrt(length_pow);
    }

    IEnumerable<int[]> test(int PointCount) {
        int n = pointDiv;
        List<int> temp = new List<int>();

        for (int i = 0; i < PointCount; i++)
        {
            temp.Add(i);
        }    
        int[] are = temp.ToArray();
        
        var result = are.Select(x => new int[] { x });
        for (int i = 0; i < n - 1; i++)
        {
            result = result.SelectMany(x => are.Where(y => y.CompareTo(x.First()) < 0).Select(y => new int[] { y }.Concat(x).ToArray()));
        }

        return result;
    }
}
