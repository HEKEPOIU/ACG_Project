using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttractAble
{
    bool Electrode { get; set;}

    void Attract(Transform target,float speed,bool inver =false, ForceMode2D forceMode2D = ForceMode2D.Force);
}
