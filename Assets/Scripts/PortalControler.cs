using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControler : MonoBehaviour
{
    
    public GameObject Zombie;
    private bool canCreate = true;
    void Start()
    {

    }


    void Update()
    {
        int num = Random.Range(4, 8);
        if (canCreate)
        {
            Vector2 position = new Vector2(transform.position.x, Zombie.transform.position.y);
            Instantiate(Zombie, position, Zombie.transform.rotation);
            canCreate = false;
            Invoke("crearEnemigo", num);
        }
    }
    private void crearEnemigo()
    {
        canCreate = true;
    }

}
