using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Personality: MonoBehaviour{

    public GameObject paddle;
    public GameObject ball;

    //player stats
    public float APM = 500f;
    public float minAPM = 200f;
    public float maxAPM = 300f;
    public float reaction_time = 0f; //difference between eye and hand
    public float min_reaction_time = 0.02f; //difference between eye and hand
    public float max_reaction_time = 0.03f; //difference between eye and hand
    public float paddle_safety_distance = 1f; //how close to the edge of the paddle 
    //this player feels comfortable playing with, less means more safe
    public float min_paddle_safety_distance = .8f;
    public float max_paddle_safety_distance = 1.1f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
      
      
        
        
        
    }

    public float getAPM(){
        return APM;
    }
    public float getReactionTime(){
        return reaction_time;
    }

    public void Play(){

        if(paddle == null)
        paddle = GameObject.Find("Paddle");

        if(ball == null)
        ball = GameObject.Find("Ball");


        
    }


    void PaddleMovement(){
       if(gameObject.activeSelf == true) 
        StartCoroutine(takeAction());
        
    }

    IEnumerator takeAction(){
        Debug.Log("hallo?");
        int choice = MoveHeuristic();
        
        yield return new WaitForSeconds(reaction_time);

        switch(choice){
            case 0:
            paddle.GetComponent<PaddleScript>().Stay();
            Debug.Log("stay");
            break;
            case 1:
            paddle.GetComponent<PaddleScript>().MoveLeft();
            Debug.Log("left");
            break;
            case 2:
            paddle.GetComponent<PaddleScript>().MoveRight();
            Debug.Log("right");
            break;
        }
    }

    //TWO FUNCTIONS WILL BE ABSTRACTED INTO MULTIPLE OTHER PERSONALITIES
    public void GenerateValues(){
        //Generate different paddle_safety, APM and reaction time values 
        //within specific ranges dependent on specific personality type

        APM = 800f;
        reaction_time = 0f;
        paddle_safety_distance = 1f;
        
        /*
        APM = UnityEngine.Random.Range(minAPM, maxAPM);
        reaction_time = UnityEngine.Random.Range(min_reaction_time, max_reaction_time);
        paddle_safety_distance = UnityEngine.Random.Range(min_paddle_safety_distance, max_paddle_safety_distance); */
        

    }

    public virtual int MoveHeuristic(){
        //VERY BASIC TEST VERSION, DO BETTER LATER
        float paddleX = paddle.transform.position.x;
        float ballX = ball.transform.position.x;
        float distanceX = paddleX - ballX;

        if(Math.Abs(distanceX) <= paddle_safety_distance)
            return 0;
        
        if(distanceX > paddle_safety_distance)
            return 1;

        if(distanceX < paddle_safety_distance)
            return 2;
        
        return 0;
        
    }
    //make virtual eventually
    public float CalculateSatisfaction(){
        StopCoroutine(takeAction());
        return 1;
    }

}