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


    //��J�ʤ�������|�۹��m�A�p�G�n�q�����ӭn�ǤJpath�A���o�̥��o�ˡC
    Vector3 LineProgressRatio(float zeroToOne)
    {
        zeroToOne = Mathf.Clamp01(zeroToOne);
        float Length = 0;
        //�p��u�q�`��

        for (int i = 0; i < pathPoint.Length; i++)
        {
            Length += (pathPoint[i].position - pathPoint[(i+1)%pathPoint.Length].position).magnitude;
        }
        //�i�H��lerp���ؼЦ�m�A���O�o�̥i�H���� * �]�����O0�C
        float target = Length * zeroToOne;

        //�p�G�ؼЪ��פj����I�Z��-->�ؼЪ��״���I�Z��
        //�p�G�ؼЪ��פp�󵥩���I�Z�� -->�N��b��e�I�P�U���I���� --> �����e�Iindex�A����j��C
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
        //�ΤW����쪺�I��m�A�M���X�ؼЪ��צb���I���פ�����ҡA��lerp��X��m�C
        Vector3 startPos = pathPoint[index].position;
        Vector3 endPos = pathPoint[(index + 1) % pathPoint.Length].position;
        float segmentProgress = target / Vector3.Distance(startPos, endPos);
        return Vector3.Lerp(pathPoint[index].position, pathPoint[(index + 1) % pathPoint.Length].position, segmentProgress);

    }
}
