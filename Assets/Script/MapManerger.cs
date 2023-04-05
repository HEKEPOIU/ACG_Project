//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MapManerger : MonoBehaviour
{
    [SerializeField] Transform[] pathPoint;
    [SerializeField] GameObject[] floorObjectR;
    [SerializeField] GameObject[] floorObjectB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(floorObjectB[Random.Range(0, 5)], LineProgressRatio(Random.Range(0, 1f)), Quaternion.identity);        
    }


    //丟入百分比找到路徑相對位置，如果要通用應該要傳入path，但這裡先這樣。
    Vector3 LineProgressRatio(float zeroToOne)
    {
        zeroToOne = Mathf.Clamp01(zeroToOne);
        float Length = 0;
        //計算線段總長

        for (int i = 0; i < pathPoint.Length; i++)
        {
            Length += (pathPoint[i].position - pathPoint[(i+1)%pathPoint.Length].position).magnitude;
        }
        //可以用lerp找到目標位置，但是這裡可以直接 * 因為底是0。
        float target = Length * zeroToOne;

        //如果目標長度大於兩點距離-->目標長度減掉兩點距離
        //如果目標長度小於等於兩點距離 -->代表在當前點與下個點中間 --> 獲取當前點index，停止迴圈。
        int index = 0;
        for (int i = 0; i < pathPoint.Length; i++)
        {
            float L= (pathPoint[i].position - pathPoint[(i + 1) % pathPoint.Length].position).magnitude;
            if (target > L) target -= L;
            else if (target <= L)
            {
                index = i;
                break;
            }
        }
        //用上面找到的點位置，然後算出目標長度在兩點長度中的比例，用lerp算出位置。
        Vector3 startPos = pathPoint[index].position;
        Vector3 endPos = pathPoint[(index + 1) % pathPoint.Length].position;
        float segmentProgress = target / Vector3.Distance(startPos, endPos);
        return Vector3.Lerp(pathPoint[index].position, pathPoint[(index + 1) % pathPoint.Length].position, segmentProgress);

    }
}
