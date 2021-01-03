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

    public virtual float[] GetVariables(){
        float[] a = {0, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    public virtual float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
        //I felt content
        float content = 0;
        //I felt skilful
        float skillful = 0;
        //I was fully occupied with the game
        float occupied = 0;
        //I thought it was hard
        float hard = 0;
        //overall enjoyment
        float satisfaction = 0;

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }

    public void Play(){

        if(paddle == null)
        paddle = GameObject.Find("Paddle");

        if(ball == null)
        ball = GameObject.Find("Ball");


        
    }


    void PaddleMovement(){
        if (paddle == null) return;
        if(gameObject.activeSelf == true) StartCoroutine(takeAction());
        
    }

    protected IEnumerator takeAction(){
        
        int choice = MoveHeuristic();
        
        yield return new WaitForSeconds(reaction_time);

      

        switch(choice){
            case 0:
            paddle.GetComponent<PaddleScript>().Stay();
           
            break;
            case 1:
            paddle.GetComponent<PaddleScript>().MoveLeft();
    
            break;
            case 2:
            paddle.GetComponent<PaddleScript>().MoveRight();
        
            break;
        }
    }

    //TWO FUNCTIONS WILL BE ABSTRACTED INTO MULTIPLE OTHER PERSONALITIES
    public virtual void GenerateValues(){
        //Generate different paddle_safety, APM and reaction time values 
        //within specific ranges dependent on specific personality type

       
        APM = UnityEngine.Random.Range(minAPM, maxAPM);
        reaction_time = UnityEngine.Random.Range(min_reaction_time, max_reaction_time);
        paddle_safety_distance = UnityEngine.Random.Range(min_paddle_safety_distance, max_paddle_safety_distance);
        

    }

    public virtual int MoveHeuristic(){
       
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
   
}