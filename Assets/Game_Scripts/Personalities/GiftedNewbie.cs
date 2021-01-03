using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedNewbie : Personality
{

    
    protected override void Start()
    {
    base.Start();    // call base class

    minAPM = 370;
    maxAPM = 500;
   
    min_reaction_time = 0.005f; //difference between eye and hand
    max_reaction_time = 0.01f; //difference between eye and hand
    
    min_paddle_safety_distance = 0.9f;
    max_paddle_safety_distance = 1.2f;
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

      public override float[] GetVariables(){
        float[] a = {4, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    
    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
        //I felt content
        float content = 0;
        if(win == 1)
        content++;
        if(time < 120)
        content++;
        if(bricks < 20)
        content++;
        if(bricks < 10)
        content++;
        if(ballHits*2 < (30-bricks))
        content++;
        
        //I felt skilful
        float skillful = 0;
        if(ballHits*2 < (30-bricks))
        skillful++;
        if(win == 1)
        skillful++;
        if(time/paddleDistance > 10)
        skillful++;
        if(time < 40)
        skillful++;
        if(bricks<15)
        skillful++;

        //I was fully occupied with the game
        float occupied = 0;
        if(time/paddleDistance < 14)
        occupied++;
        if(time < 50)
        occupied++;
        if(time/ballHits < 3)
        occupied++;
        if(ballHits > 3)
        occupied++;
        if(ballHits > 8)
        occupied++;

        //I thought it was hard
        float hard = 5;
        if(win == 0)
        hard--;
        if(time/ballHits > 4)
        hard--;
        if(bricks > 20)
        hard--;
        if(bricks > 25)
        hard--;

        //overall enjoyment
        float satisfaction = (float)(content *1.5+ skillful*.75+occupied*1.25+hard*.5);

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }

}
