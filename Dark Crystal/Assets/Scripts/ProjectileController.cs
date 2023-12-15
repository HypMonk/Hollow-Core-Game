using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [HideInInspector]
    public Transform sourceTransform;

    [HideInInspector]
    public float speed, damage, power, knockback;

    float maxDistance = 100;

    private void Start()
    {
        if (sourceTransform == null)
        {
            Debug.Log("Projectile has no source.");
            Destroy(this.gameObject);
            return;
        }

        GetComponent<Rigidbody2D>().velocity = transform.up * speed;

    }

    // Update is called once per frame
    void Update()
    {

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
            collision.gameObject.GetComponent<TorchController>().ToggleLight(power);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "DarkCrystal")
        {
            collision.gameObject.GetComponent<DarkCrystal>().Damage((int)damage, transform);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BaseEnemyController>().Damage((int)damage, knockback, transform.position);
        }
    }
}
