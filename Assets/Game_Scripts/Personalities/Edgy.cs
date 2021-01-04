using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Edgy : Personality
{  

    protected override void Start()
    {
    base.Start();    // call base class

    minAPM = 150;
    maxAPM = 250;
   
    min_reaction_time = 0.005f; //difference between eye and hand
    max_reaction_time = 0.01f; //difference between eye and hand
    
    min_paddle_safety_distance = 1f;
    max_paddle_safety_distance = 1.3f;
    GenerateValues();
    
    InvokeRepeating("PaddleMovement", 0, (float)60/APM);
    }

  
    public override int MoveHeuristic(){
        //VERY BASIC TEST VERSION, DO BETTER LATER
        float paddleX = paddle.transform.position.x;
        float ballX = ball.transform.position.x;

        float pointA = paddleX - paddle_safety_distance/2;
        float pointB = paddleX + paddle_safety_distance/2;
        
        if(ball.GetComponent<Rigidbody2D>().velocity.y < 0)
        ballX = calcTrajectory();

        float distanceX = paddleX - ballX;
        float distanceA = pointA - ballX;
        float distanceB = pointB - ballX;

        if(Math.Abs(distanceA) <= paddle_safety_distance/2 || Math.Abs(distanceB) <= paddle_safety_distance/2)
            return 0;
        
        else if(Math.Abs(distanceA) <= Math.Abs(distanceB))
            return 1;

        else return 2;
        
    
        
    }

    float calcVerticalTime(){
        
        return (paddle.transform.position.y - ball.transform.position.y)/ball.GetComponent<Rigidbody2D>().velocity.y;
    }
    
    float calcTrajectory(){
    float dropTime = calcVerticalTime();

    float prediction =  ball.transform.position.x + ball.GetComponent<Rigidbody2D>().velocity.x*dropTime;

    if(prediction > 9){
        float aux = prediction - 9;
        prediction = prediction - aux; 
    }
    return prediction;
    }

      public override float[] GetVariables(){
        float[] a = {4, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    
    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
        float content = 1;
        if(win == 1)
        content++;
        if(bricks < 20)
        content++;
        if(ballHits *2 < ballBounces)
        content++;
        if(ballHits*1.5 < (30-bricks))
        content++;
        
        //I felt skilful
        float skillful = 0;
        if(ballHits*2 < (30-bricks))
        skillful++;
        if(win == 1)
        skillful++;
        if(time/paddleDistance > 0.1f)
        skillful++;
        if(ballHits *2.5 < ballBounces)
        skillful++;
        if(bricks<15)
        skillful++;

        //I was fully occupied with the game
        float occupied = 0;
        if(paddleDistance/ballHits < 50)
        occupied++;
        if(ballHits *2 < ballBounces)
        occupied++;
        if(time/ballHits < 4f)
        occupied++;
        if(time > 15)
        occupied++;
        if(ballBounces > 20)
        occupied++;



        //I thought it was hard
        float hard = 5;
        if(win == 1)
        hard--;
        if(time/ballHits > 4)
        hard--;
        if(time < 5)
        hard--;
        if(time < 10)
        hard--;
        if(bricks > 25)
        hard--;

        //overall enjoyment
        float satisfaction = (float)(content + skillful*1.5 + occupied*.5 + hard);

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }


}
