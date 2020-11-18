using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Personality: MonoBehaviour{

    public GameObject paddle;
    public GameObject ball;
    private float APM = 300f; //actions per minute
    private float reaction_time = 0.05f; //difference between eye and hand

    private float paddle_safety_distance = 1f; //how close to the edge of the paddle 
    //this player feels comfortable playing with, less means more safe

    // Start is called before the first frame update
    void Start()
    {
        if(paddle == null)
        paddle = GameObject.Find("Paddle");

        if(ball == null)
        ball = GameObject.Find("Ball");
        
        InvokeRepeating("PaddleMovement", 0, (float)60f/APM);
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }

    void PaddleMovement(){
       
        StartCoroutine(takeAction());
        
    }

    IEnumerator takeAction(){
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

    //TWO FUNCTIONS BELOW NEED TO BE DEVELOPED AND WILL BE ABSTRACTED INTO MULTIPLE OTHER PERSONALITIES
    void GenerateValues(){
        //Generate different paddle_safety, APM and reaction time values within specific ranges dependent on specific personality type
    }

    int MoveHeuristic(){
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

    float CalculateSatisfaction(){
        return 1;
    }

}