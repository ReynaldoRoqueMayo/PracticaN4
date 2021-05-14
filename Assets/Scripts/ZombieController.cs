using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{

    private Rigidbody2D rb;

    public float speed = 7f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

   
    void Update()
    {
       


        rb.velocity = new Vector2(-speed, rb.velocity.y);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "enemyCleaner")
        {
            Destroy(this.gameObject);
        }
    }

}
