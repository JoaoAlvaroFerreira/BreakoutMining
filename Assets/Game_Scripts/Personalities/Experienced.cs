using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Experienced : Personality
{  

    protected override void Start()
    {
    base.Start();    // call base class

    minAPM = 150;
    maxAPM = 250;
   
    min_reaction_time = 0.03f; //difference between eye and hand
    max_reaction_time = 0.04f; //difference between eye and hand
    
    min_paddle_safety_distance = .8f;
    max_paddle_safety_distance = 1.1f;
    GenerateValues();
    
    InvokeRepeating("PaddleMovement", 0, (float)60/APM);
    }

  
    public override int MoveHeuristic(){
        //VERY BASIC TEST VERSION, DO BETTER LATER
        float paddleX = paddle.transform.position.x;
        float ballX = ball.transform.position.x;

        
        if(ball.GetComponent<Rigidbody2D>().velocity.y < 0)
        ballX = calcTrajectory();

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

    float prediction =  ball.transform.position.x + ball.GetComponent<Rigidbody2D>().velocity.x*dropTime;

    if(prediction > 9){
        float aux = prediction - 9;
        prediction = prediction - aux; 
    }
    return prediction;
    }

      public override float[] GetVariables(){
        float[] a = {3, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    
    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
        float content = 1;
        if(win == 1)
        content++;
        if(time < 140)
        content++;
        if(bricks < 20)
        content++;
        if(ballHits *2 < ballBounces)
        content++;

        
        
        //I felt skilful
        float skillful = 0;
        if(ballHits*2 < (30-bricks))
        skillful++;
        if(win == 1)
        skillful++;
        if(ballHits *2.5 < ballBounces)
        skillful++;
        if(ballHits *2 < ballBounces)
        skillful++;
        if(time/paddleDistance < 0.1f)
        skillful++;

        //I was fully occupied with the game
        float occupied = 0;
        if(time/paddleDistance < 0.1f)
        occupied++;
        if(time > 15)
        occupied++;
        if(time/ballHits < 4f)
        occupied++;
        if(paddleDistance/ballHits < 50)
        skillful++;
        if(ballHits > 10)
        occupied++;

        //I thought it was hard
        float hard = 5;
        if(win == 0)
        hard--;
        if(time/ballHits > 4)
        hard--;
        if(bricks > 25)
        hard--;
        if(time<5)
        hard--;
        if(ballHits *3 > ballBounces)
        hard--;

        //overall enjoyment
        float satisfaction = (float)(content *1.5 + skillful*.75 + occupied*1.25 + hard*.5);

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }


}
