using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbd;
    private SpriteRenderer spr;
    private Animator amtr;
    private BoxCollider2D bx2d;

    private static int vidas = 3;
    private int playerLayer = 3, enemyLayer = 6;
    private bool dobleSalto;
    private bool tocandoSuelo = false;


    public GameObject escalera;
    private TilemapCollider2D esrb;
    private bool puedeEscalar = false;
    private bool bajando = false;

    //ataque
    public GameObject kunaiDerecho;
    public GameObject kunaiIzquierdo;


    //VOLAR
    private int estadoVolar = 0;
    private bool puedeVolar = false;

    //PUNTAJE
    public Text scoreText;
    public Text vidasText;
    private int puntaje = 0;
    //intervalo
    private float maxTime = 0f;
    private float TimeNow = 0f;

    private float switchColorDelay = .1f;
    private float switchColorTime = 0f;

    private Color originalColor;
    private bool FueAtacado = false;
    //caida
    private bool PuedeMorir = false;
    private bool muerto = false;
    private static int ANIMACION_PARADO = 0, ANIMACION_CORRER = 1, ANIMACION_SALTAR = 2, ANIMACION_LANZAR=3, ANIMACION_DESLIZAR=4, ANIMACION_ESCALAR = 5, ANIMACION_FLY = 6, ANIMACION_MORIR = 7;
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        amtr = GetComponent<Animator>();
      
        bx2d = GetComponent<BoxCollider2D>();

        //escalera
        esrb = escalera.GetComponent<TilemapCollider2D>();
        //revivir
        originalColor = spr.color;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (FueAtacado && maxTime < 6)
        {
            maxTime += Time.deltaTime;
            TimeNow += Time.deltaTime;
            Debug.Log("Tiempo: " + maxTime);
            if (TimeNow >=1)
            {
                cambioColor();
            }

        }
        else if (FueAtacado && maxTime >= 6)
        {
            maxTime = 0;
            spr.color = originalColor;
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            FueAtacado = false;
        }







        amtr.SetInteger("Estado", ANIMACION_PARADO);
        rbd.velocity = new Vector2(0, rbd.velocity.y);


        scoreText.text = "Puntaje: " + puntaje;

        vidasText.text = "Vidas: " + vidas;


        if (transform.position.y>= 25.748f)
        {
            PuedeMorir = true;
           
        }
        if (muerto)
        {
            amtr.SetInteger("Estado",ANIMACION_MORIR);
          
        }

        if (Input.GetKey(KeyCode.RightArrow) && estadoVolar == 0)
        {
            spr.flipX = false;
            if (Input.GetKey(KeyCode.C) && tocandoSuelo)
            {
                setDeslizar(1);
            }
            else
            {

                SetCorrer(1);
            }


        }
        if (Input.GetKey(KeyCode.LeftArrow) && estadoVolar == 0)
        {
            spr.flipX = true;
            if (Input.GetKey(KeyCode.C) && tocandoSuelo)
            {
                setDeslizar(-1);
            }
            else
            {

                SetCorrer(-1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && tocandoSuelo)
        {
            amtr.SetInteger("Estado", ANIMACION_SALTAR);
            rbd.velocity = Vector2.up * 24f;
            tocandoSuelo = false;
            dobleSalto = true;
          


        }
        else if (Input.GetKeyDown(KeyCode.Space) && dobleSalto)
        {
            amtr.SetInteger("Estado", ANIMACION_SALTAR);
            rbd.velocity = Vector2.up * 22f;
            dobleSalto = false;
        }
        //escalera
        if (!bajando && puedeEscalar)
        {
            
            rbd.gravityScale = 0;
            rbd.velocity = new Vector2(rbd.velocity.x, 0);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                amtr.SetInteger("Estado", ANIMACION_ESCALAR);
                rbd.velocity = new Vector2(rbd.velocity.x, 4);
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && puedeEscalar)
        {
            bajando = true;
            esrb.isTrigger = true;
            rbd.gravityScale = 5;
            //rbd.velocity = new Vector2(rbd.velocity.x, -4);


        }


        //ataque
        if (Input.GetKeyDown(KeyCode.X) && !spr.flipX)
        {
            amtr.SetInteger("Estado",ANIMACION_LANZAR);
            Vector2 position = new Vector2(transform.position.x + 2f, transform.position.y - .6f);
            Instantiate(kunaiDerecho, position, kunaiDerecho.transform.rotation );
        }
        else if (Input.GetKeyDown(KeyCode.X) && spr.flipX)
        {
            amtr.SetInteger("Estado", ANIMACION_LANZAR);
            Vector2 position = new Vector2(transform.position.x - 2.5f, transform.position.y - .6f);
            Instantiate(kunaiIzquierdo, position, kunaiIzquierdo.transform.rotation);

        }




        //VOLAR

        if (Input.GetKeyDown(KeyCode.V))
        {
            estadoVolar++;
            if (estadoVolar == 1)
            {
                PuedeMorir = false;
                puedeVolar = true;
                //rbd.velocity = Vector2.up * 8f;
                Vector3 arriba = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                transform.position = arriba;
            }
            if (estadoVolar == 2 || estadoVolar == 0)
            {
                puedeVolar = false;
                estadoVolar = 0;
                rbd.gravityScale = 5;
            }
        }
        if (puedeVolar)
        {
            rbd.gravityScale = 0;
            rbd.velocity = new Vector2(0, 0);
            amtr.SetInteger("Estado", ANIMACION_FLY);

            if (Input.GetKey(KeyCode.RightArrow))
            {

                spr.flipX = false;
                rbd.velocity = new Vector2(8, rbd.velocity.y);
                if (Input.GetKey(KeyCode.UpArrow))
                {

                    rbd.velocity = new Vector2(rbd.velocity.x, 5);


                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rbd.velocity = new Vector2(rbd.velocity.x, -5);
                }

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spr.flipX = true;
                rbd.velocity = new Vector2(-8, rbd.velocity.y);
                if (Input.GetKey(KeyCode.UpArrow))
                {

                    rbd.velocity = new Vector2(rbd.velocity.x, 5);


                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rbd.velocity = new Vector2(rbd.velocity.x, -5);
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {

                rbd.velocity = new Vector2(rbd.velocity.x, 5);


            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rbd.velocity = new Vector2(rbd.velocity.x, -5);
            }
            //rbd.gravityScale = 0;
            // rbd.velocity = new Vector2(rbd.velocity.x, 0);
        }



    }




    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "TerrenoBajo" && PuedeMorir)
        {
            muerto = true;
            Debug.Log("Muerto");
        }
        if (collision.gameObject.tag == "Terreno")
        {
            Debug.Log("CHOQUE TERRENO");
            bajando = false;
            tocandoSuelo = true;
          
        }
    
        if (collision.gameObject.tag == "Enemigo")

        {
           

            vidas--;
            FueAtacado = true;
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            if (vidas==0)
            {
                muerto = true;
            }
            //Destroy(this.gameObject);
            //if (vidas != 0)
            //{

            //    Invoke("crearPersonaje", 1f);
            //}

        }



    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Escalera")
        {
            puedeEscalar = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Escalera")
        {
            esrb.isTrigger = false;
            puedeEscalar = false;
            rbd.gravityScale = 5;
        }
    }

    public void SetCorrer(int sentido)
    {
        rbd.velocity = new Vector2(8 * sentido, rbd.velocity.y);
        amtr.SetInteger("Estado", ANIMACION_CORRER);
    }
    public void setSaltar()
    {
        amtr.SetInteger("Estado", ANIMACION_SALTAR);
        //rbd.velocity = Vector2.up * 10;
        rbd.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    public void setDeslizar(int sentido)
    {
        amtr.SetInteger("Estado", ANIMACION_DESLIZAR);
        rbd.velocity = new Vector2(5 * sentido, rbd.velocity.y);
    }
    public void aumentarPuntaje()
    {

        puntaje += 10;
    }
    private void SwitchColor()
    {
        if (spr.color == originalColor)
            spr.color = Color.yellow;
        else
            spr.color = originalColor;
        switchColorTime = 0;

    }
    private void cambioColor()
    {
        if (spr.color == originalColor)
        {
            spr.color = Color.green;
        }
        else if (spr.color == Color.yellow )
        {

            spr.color = originalColor;
        }
        TimeNow = 0;
    }
}
