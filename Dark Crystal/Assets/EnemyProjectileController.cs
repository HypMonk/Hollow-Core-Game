using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [HideInInspector]
    public Transform sourceTransform;

    [HideInInspector]
    public float speed, damage, knockback;

    [HideInInspector]
    public Vector2 directionToTarget;

    float maxDistance = 100;

    // Start is called before the first frame update
    void Start()
    {
        if (sourceTransform == null)
        {
            Debug.Log("Projectile has no source.");
            Destroy(this.gameObject);
            return;
        }

        GetComponent<Rigidbody2D>().velocity = directionToTarget * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceTransform == null) return;

        if (Vector2.Distance((Vector2)sourceTransform.position, transform.position) > maxDistance)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Torch")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "DarkCrystal")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Damage((int)damage, knockback, transform.position);
            Destroy(this.gameObject);
        }
    }
}
