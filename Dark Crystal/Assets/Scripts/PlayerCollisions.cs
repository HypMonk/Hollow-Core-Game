using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField]
    PlayerStats _stats;
    [SerializeField]
    PlayerInventory _inventory;
    [SerializeField]
    PlayerController _controller;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ChargePad")
        {
            _stats.LightLevel += _stats.LightLevelRechargeRate;
            if (_stats.LightLevel > _stats.MaxLightLevel)
            {
                _stats.LightLevel = _stats.MaxLightLevel;
            }
        }

        if(collision.tag == "Pitfall")
        {
            _controller.isAtRiskofFalling = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PickUp")
        {
            _inventory.Shards++;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Pitfall")
        {
            _controller.isAtRiskofFalling = false;
        }
    }
}
