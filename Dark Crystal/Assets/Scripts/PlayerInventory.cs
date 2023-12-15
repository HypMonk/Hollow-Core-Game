using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    int _shards = 0;

    public int Shards { get { return _shards; } set { _shards = value; } }
}
