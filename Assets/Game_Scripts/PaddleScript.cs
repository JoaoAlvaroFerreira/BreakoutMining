using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float PaddleSpeed;
    private Rigidbody2D rb;
    private Vector2 spriteSize;

    public float distanceRan;
    public int ballHits;

    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        distanceRan =0;
        ballHits = 0;
        rb = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().size;
        
        Debug.Log("Paddle Size: "+ spriteSize.x);
     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            ballHits++;
            float halfLength = (spriteSize.x * transform.localScale.x) / 2;
            float x = collision.gameObject.transform.position.x;
            float myX = transform.position.x;
            float moreVelocity = 0;
            moreVelocity = Mathf.Abs(myX - x);
            float percentageSide = 100 - (moreVelocity * 100 / halfLength);
            float angle = (percentageSide * 80 / 100) * Mathf.Deg2Rad;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            
            if (myX > x)
                rb.velocity = new Vector2(-rb.velocity.magnitude * Mathf.Cos(angle), rb.velocity.magnitude * Mathf.Sin(angle));
            else rb.velocity = new Vector2(rb.velocity.magnitude * Mathf.Cos(angle), rb.velocity.magnitude * Mathf.Sin(angle));
        }
    }

    // Update is called once per frame
    void Update()
    {
         distanceRan += Vector3.Distance(transform.position, lastPosition);
  lastPosition = transform.position;
       
        if(transform.position.x > 8){
            transform.position = new Vector3(8,transform.position.y,transform.position.z);
        }
          if(transform.position.x < -8){
            transform.position = new Vector3(-8,transform.position.y,transform.position.z);
        }
    }

    public void MoveRight()
    {
        rb.velocity = new Vector3(PaddleSpeed, 0, 0);
    }
    public void MoveLeft()
    {
        rb.velocity = new Vector3(-PaddleSpeed, 0, 0);
    }
    public void Stay()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }
}
