using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOITower : MonoBehaviour, ITower
{
    float _range = 10f;
    float _damage ;
    int _cost;

    public float range
    {
        get { return _range; }
        set { _range = value; }
    }
    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    public int cost
    {
        get { return _cost; }
        set { _cost = value; }
    }


}
