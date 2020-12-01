using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bouncer : Personality
{
    //TWO FUNCTIONS BELOW NEED TO BE DEVELOPED AND WILL BE ABSTRACTED INTO MULTIPLE OTHER PERSONALITIES
    void GenerateValues(){
        //Generate different paddle_safety, APM and reaction time values within specific ranges dependent on specific personality type
    }

    public override int MoveHeuristic(){
       
        //VERY BASIC TEST VERSION, DO BETTER LATER
        float paddleX = paddle.transform.position.x;
        float ballX = calcTrajectory();
        float distanceX = paddleX - ballX;

        if(Math.Abs(distanceX) <= paddle_safety_distance)
            return 0;
        
        if(distanceX > paddle_safety_distance)
            return 1;

        if(distanceX < paddle_safety_distance)
            return 2;
        
        return 0;
        
    }

    float calcVerticalTime(){
        
        return (paddle.transform.position.y - ball.transform.position.y)/ball.GetComponent<Rigidbody2D>().velocity.y;
    }
    float calcTrajectory(){
    float dropTime = calcVerticalTime();

    Debug.Log("Time: "+ dropTime);
     
    return ball.transform.position.x + ball.GetComponent<Rigidbody2D>().velocity.x*dropTime;;
    }

    float CalculateSatisfaction(){
        return 0;
    }
}
