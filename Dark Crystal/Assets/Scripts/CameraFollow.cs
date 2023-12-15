using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform _playerTransform;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            Debug.Log("No Player Found");
        }
    }
    private void LateUpdate()
    {
        if (_playerTransform == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                Debug.Log("No Player Found");
            }
        }

        if (_playerTransform != null)
        {
            transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);
        }
    }
}
