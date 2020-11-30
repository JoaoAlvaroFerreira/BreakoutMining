using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newbie : Personality
{

    
    protected override void Start()
    {
    base.Start();    // call base class

    minAPM = 150;
    maxAPM = 250;
   
    min_reaction_time = 0.03f; //difference between eye and hand
    max_reaction_time = 0.04f; //difference between eye and hand
    
    min_paddle_safety_distance = 1f;
    max_paddle_safety_distance = 1.3f;
    GenerateValues();
    
    InvokeRepeating("PaddleMovement", 0, (float)60/APM);
    }

    public override int MoveHeuristic(){


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
        return 0;
    }
}
