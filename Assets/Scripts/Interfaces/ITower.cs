using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    float range { get; set; }
    float damage { get; set; }
    int cost { get; set; }
}
