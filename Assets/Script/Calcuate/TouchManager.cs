using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    //  public List<List<PointNode>> ItemObjects = new List<List<PointNode>>();
    public bool isTrackRoation;

    public RectTransform canvasRect;
    public List<PointNode> node = new List<PointNode>();

    public Transform MidPoint;

    public int[] fingerIds = new int[3];
    // Start is called before the first frame update

    public void initilization(int[] _fingerID, RectTransform _canvasRect) {
        fingerIds = _fingerID;
        canvasRect = _canvasRect;

        float xoffset = -canvasRect.rect.width / 2;
        float yoffset = -canvasRect.rect.height / 2;
        foreach (var item in node)
        {
            item.xoffset = xoffset;
            item.yoffset = yoffset;
            item.FINGER_ID = fingerIds[node.IndexOf(item)];
            item.isSetFingerId = true;
        }
    }

    void Start()
    {
        
    }

    public void DestoryObject(int[] IDS) {

        if (IDS == null)
        {

        }
        else {
            foreach (var itemNode in node)
            {

                for (int i = 0; i < IDS.Length; i++)
                {

                    if (IDS[i] == itemNode.FINGER_ID) {
                     touchInput.instance.SyncID();
                        touchInput.instance.touchManagers.Remove(this);

                        Destroy(this.gameObject,0.1F);
                    }
                } 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MidPoint.localPosition = getCenterPos(node);
        UpdateMidRotation();
    }

    void UpdateMidRotation() {
        if (isTrackRoation) {
            MidPoint.LookAt(node[0].transform);
        }
    }

    Vector3 getCenterPos(List<PointNode> node) {
        float x1,x2,x3,y1,y2,y3,A1, A2, B1, B2, C1, C2, X, Y;


        x1 = node[0].X;
        y1 = node[0].Y;

        x2 = node[1].X;
        y2 = node[1].Y;

        x3 = node[2].X;
        y3 = node[2].Y;

        A1 = 2 * (x2 - x1);
        B1 = 2*(y2 - y1);
        C1 = x2 * x2 + y2 * y2 - x1 * x1 - y1 * y1;
        A2 = 2 * (x3 - x2);
        B2 = 2 * (y3 - y2);
        C2 = x3 * x3 + y3 * y3 - x2 * x2 - y2 * y2;
        X = ((B2 * C1) - (B1 * C2)) / ((B2 * A1) - (B1 * A2));
        Y = (-(A2 * C1) + (A1 * C2)) / ((B2 * B1) - (B1 * A2));

        return new Vector3(X, Y, 0);


    }
}
