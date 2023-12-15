using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointIndicatorRotation : MonoBehaviour
{
    [SerializeField]
    Transform _playerLocation;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = ((_playerLocation.position + new Vector3(0,.15f,0)) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
