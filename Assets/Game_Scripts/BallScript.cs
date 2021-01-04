using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody2D rb;
    private bool hitFloor = false;
    private int bounces = 0;
    private int vertCheck = 0;
    // Start is called before the first frame update
    void Start()
    {
        bounces = 0;
        vertCheck = 0;
    }

    public void SetSpeed(float x){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Random.Range(-x, x), x);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        bounces++;


        if(rb.velocity.x < 1 && rb.velocity.x > -1)
        vertCheck++;
        else vertCheck = 0;

        if(rb.velocity.y < 1 && rb.velocity.y > -1)
        rb.velocity = new Vector2(rb.velocity.x, 5);

        if(vertCheck == 6)
        rb.velocity = new Vector2(5, rb.velocity.x);
    

        if(col.gameObject.name == "Floor")
        hitFloor = true;
    }

    public bool getHitFloor(){
        return hitFloor;
    }
    public void Reset()
    {
        hitFloor = false;
    }

    public int getBallBounces(){
        int a = bounces;
        bounces = 0;
        return a;
    }
}
