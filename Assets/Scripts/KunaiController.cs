using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    private Rigidbody2D rbb;
    public float speed = 10f;
    private PlayerController pl;

    void Start()
    {
        rbb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
        pl = FindObjectOfType<PlayerController>();


    }

    void Update()
    {
        rbb.velocity = new Vector2(speed, rbb.velocity.y);



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            pl.aumentarPuntaje();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

}
