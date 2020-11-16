using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float PaddleSpeed;
    private Rigidbody2D rb;
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveRight(){
        rb.velocity = new Vector3(PaddleSpeed, 0, 0);
    }
    public void MoveLeft(){
        rb.velocity = new Vector3(-PaddleSpeed, 0, 0);
    }
    public void Stay(){
        rb.velocity = new Vector3(0, 0, 0);
    }
}
