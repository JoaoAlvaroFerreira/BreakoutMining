using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Competitive : Personality
{  

    protected override void Start()
    {
    base.Start();    // call base class

    minAPM = 370;
    maxAPM = 500;
   
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
        float[] a = {2, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    
    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
        float content = 0;
        if(win == 1)
        content++;
        if(time < 120)
        content++;
        if(bricks < 20)
        content++;
        if(bricks < 15)
        content++;
        if(ballHits*1.5 < (30-bricks))
        content++;
        
        //I felt skilful
        float skillful = 0;
        if(ballHits*2 < (30-bricks))
        skillful++;
        if(win == 1)
        skillful++;
        if(ballHits *2 < ballBounces)
        skillful++;
        if(paddleDistance/ballHits < 50)
        skillful++;
        if(time/paddleDistance < 0.1f)
        content++;

        //I was fully occupied with the game
        float occupied = 0;
        if(time/paddleDistance < 0.1f)
        occupied++;
        if(time > 15)
        occupied++;
        if(time/ballHits < 4f)
        occupied++;
        if(ballHits *2f < ballBounces)
        occupied++;
        if(ballHits *2.5 < ballBounces)
        occupied++;

        //I thought it was hard
        float hard = 5;
        if(win == 0)
        hard--;
        if(time/ballHits > 4)
        hard--;
        if(bricks > 17)
        hard--;
        if(time<5)
        hard--;
        if(ballHits *3 > ballBounces)
        hard--;

        //overall enjoyment
        float satisfaction = (float)(content *.75 + skillful*1.5 + occupied*.5 + hard*1.25);

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }


}
