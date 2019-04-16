using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputMagager : MonoBehaviour
{

    public static Dictionary<int, M_Finger> KP_FINGER_ID_m_Finger = new Dictionary<int, M_Finger>();

    public static  List<int[]> the_Tri = new List<int[]>();

    public static InputMagager instance;

    #region debug
    public Text Debugtext;
    string DebugString;
    #endregion
    public const float Re_angleA = 60;
    public const float Re_angleB = 60;
    public const float Re_angleC = 60;

    public const float angleThreshold = 10f;

    public const float MaxPerimeter = 2000;

    public const int pointDiv = 3;

    public GameObject G_finger;

    public GameObject TheNodeObj;
    //public List<M_Finger> m_Fingers = new List<M_Finger>();

    public List<int[]> triID = new List<int[]>();

    public List<int[]> perviousTriID = new List<int[]>();
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        EventCenter.AddListener<int>(EventDefine.AddFinger, spawnFinger);
        EventCenter.AddListener<int>(EventDefine.RemoveFinger, removeFineger);
        EventCenter.AddListener(EventDefine.CheckTri, CheckTir);

    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                EventCenter.Broadcast<int>(EventDefine.AddFinger, Input.GetTouch(i).fingerId);
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                EventCenter.Broadcast<int>(EventDefine.RemoveFinger, Input.GetTouch(i).fingerId);
            }
        }


        for (var i = 0; i < Input.touchCount; ++i)
        {
            string x = Input.GetTouch(i).position.x.ToString();
            string y = Input.GetTouch(i).position.y.ToString();

            DebugString +="TouchId:"+ Input.GetTouch(i).fingerId+ "X: " + x + "_ _" + "Y: " + y+"\n";

        
        }
        Debugtext.text = DebugString;
        DebugString = "";

    }

    public void DeleteNode() {

    }

    public void SpawnNode() {
 

    }


    //集合TriID（当前帧）减去pervious(前一帧)检查是否需要生成新的三角形
    void CheckTir() {

        triID = getTriID();
        //展开current List<int[]> 为list<int>
        List<int> current = new List<int>();

        foreach (var a in triID)
        {
            foreach (var b in a)
            {
                current.Add(b);
            }
        }

        //pervious List<int[]> 为list<int>
        List<int> pervious = new List<int>();

        foreach (var item in perviousTriID)
        {
            foreach (var c in item)
            {
                pervious.Add(c);
            }
        }
        //集合做插值
        List<int> temp = new List<int>();
       temp =      current.Except(pervious).ToList();

        //再将list<int> 装换成int[3]
        List<int[]> newList = new List<int[]>();
        List<int> intlist = new List<int>();
        for (int i = 0; i < temp.Count; i++)
        {
            intlist.Add(temp[i]);
            if ((i + 1) % 3 == 0) {
                newList.Add(intlist.ToArray());
                intlist.Clear();
            }
        }

        //foreach (var a in newList)
        //{
        //    foreach (var b in a)
        //    {
        //        Debug.Log(b);
        //    }
        
        //}
   
        StartCoroutine(createNode(newList));

        //更新pervious
        perviousTriID = triID;
    }

    IEnumerator createNode(List<int[]> ids) {
        if (ids.Count >= 1)
        {
            for (int i = 0; i < ids.Count; i++)
            {


                    List<M_Finger> m_Fingers = new List<M_Finger>();


                    yield return new WaitForSeconds(0f);

                    for (int j = 0; j < ids[i].Length; j++)
                    {
                        try
                        {
                            M_Finger FINGER = KP_FINGER_ID_m_Finger[ids[i][j]];

                            m_Fingers.Add(FINGER);//获取三个角顶点，为了加入给中心Node
                        }
                        catch (System.Exception e)
                        {
                        break;
                        throw e;
                     
                        }

                    }



                GameObject g = Instantiate(TheNodeObj);//新建中心点

                TheNode theNode = g.GetComponent<TheNode>();

                g.transform.SetParent(this.transform);

                theNode.Ini(m_Fingers, ids[i]);//初始化N

            }
        }
  
    }




    private void spawnFinger(int _id) {
       GameObject g=  Instantiate(G_finger);
        g.transform.SetParent(this.transform);
       M_Finger m_finger = g.GetComponent<M_Finger>();
        m_finger.ini(_id);
        KP_FINGER_ID_m_Finger.Add(_id, m_finger);
        EventCenter.Broadcast(EventDefine.CheckTri);
    }


    

    private void removeFineger(int _id) {
      KP_FINGER_ID_m_Finger[_id].DestoryMe();

        //foreach (int[] key in KP_TriIDS_m_Tri.Keys)
        //{
        //    if (key.Contains(_id))
        //    {

        //        KP_TriIDS_m_Tri[key].theNode.DestoryMe();
        //        KP_TriIDS_m_Tri.Remove(key);
        //    }
        //}
      

      KP_FINGER_ID_m_Finger.Remove(_id);
        EventCenter.Broadcast(EventDefine.CheckTri);
    }


    List<int[]> getTriID()
    {
        List<int[]> Tri = new List<int[]>();
        IEnumerable<int[]> unm = test(Input.touchCount);
        foreach (var item in unm)
        {
            int A = item[0]; 
            int B = item[1];
            int C = item[2];

          //  Debug.Log("A:" + A.ToString() + "B"+B.ToString() + "C"+C.ToString());
            Vector2 positionA = Input.GetTouch(A).position;
            Vector2 positionB = Input.GetTouch(B).position;
            Vector2 positionC = Input.GetTouch(C).position;


            float angleA = getAngle(positionA, positionB, positionC);
            float angleB = getAngle(positionB, positionA, positionC);
            float angleC = getAngle(positionC, positionB, positionA);

            float TRIPerimeter = (positionC - positionA).magnitude + (positionB - positionA).magnitude + (positionC - positionB).magnitude;

            //Debug.Log(TRIPerimeter);
            if (checkAngle(angleA, angleB, angleC) && TRIPerimeter < MaxPerimeter)
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

    bool checkAngle(float a, float b, float c)
    {
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
        else
        {
            return false;
        }
    }


    float pi180 = 180 / Mathf.PI;

    float getAngle(Vector2 a, Vector2 b, Vector2 c)
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
    float getLength(float point1_x, float point1_y, float point2_x, float point2_y)
    {
        var diff_x = Mathf.Abs(point2_x - point1_x);
        var diff_y = Mathf.Abs(point2_y - point1_y);

        var length_pow = Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2);//两个点在 横纵坐标的差值与两点间的直线 构成直角三角形。length_pow等于该距离的平方

        return Mathf.Sqrt(length_pow);
    }



    //5选3
    IEnumerable<int[]> test(int PointCount)
    {

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

[System.Serializable]
public class m_Tri
{
    public List<M_Finger> m_Fingers = new List<M_Finger>();

    public TheNode theNode;

    public void ini(List<M_Finger> _m_Fingers, TheNode _theNode) {
        m_Fingers = _m_Fingers;
        theNode = _theNode;
    }

}
