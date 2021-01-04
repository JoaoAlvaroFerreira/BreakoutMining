using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastLearner : Personality
{
    private float timeCounter = 0.0f;

    
    protected override void Start()
    {
    base.Start();    // call base class

    timeCounter = 0.0f;
    minAPM = 180;
    maxAPM = 300;
   
    min_reaction_time = 0.03f; //difference between eye and hand
    max_reaction_time = 0.04f; //difference between eye and hand
    
    min_paddle_safety_distance = 1f;
    max_paddle_safety_distance = 1.3f;
    GenerateValues();
    
    InvokeRepeating("PaddleMovement", 0, (float)60/APM);
    InvokeRepeating("UpdateValues",0,1);
    }

    void Update(){
       if(timeCounter >= (float)60/APM){
        timeCounter = 0.0f;

        if (paddle == null) return;
        
        if(gameObject.activeSelf == true) StartCoroutine(takeAction());
       }
        timeCounter += Time.deltaTime;
    
    }
    void UpdateValues(){

        if(APM < 380)
        APM = APM + 5;

        if(reaction_time >= 0.01f)
        reaction_time = reaction_time - 0.001f;
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
        float[] a = {6, APM, reaction_time, paddle_safety_distance};
        return a;
    }

    
    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win){
  //I felt content
        float content = 0;
        if(win == 1)
        content++;
        if(time > 30)
        content++;
        if(time < 150)
        content++;
        if(bricks < 20)
        content++;
        if(ballHits*1.5 < (30-bricks))
        content++;
        
        //I felt skilful
        float skillful = 0;
        if(ballHits*2 < (30-bricks))
        skillful++;
        if(ballHits*1.5 < (30-bricks))
        skillful++;
        if(time/paddleDistance > 0.1f)
        skillful++;
        if(win == 1)
        skillful++;
        if(bricks<20)
        skillful++;

        //I was fully occupied with the game
        float occupied = 0;
        if(paddleDistance/ballHits < 70)
        occupied++;
        if(time > 15)
        occupied++;
        if(time/ballHits < 2.5f)
        occupied++;
        if(ballHits > 3)
        occupied++;
        if(ballBounces > 8)
        occupied++;

        //I thought it was hard
        float hard = 5;
        if(win == 1)
        hard--;
        if(time/ballHits > 4.5f)
        hard--;
        if(time < 5)
        hard--;
        if(bricks > 25)
        hard--;

        //overall enjoyment
        float satisfaction = (float)(content *1.5+ skillful*.75+occupied*1.25+hard*.5);

        float[] a = {content, skillful, occupied, hard, satisfaction};

        return a;
    }

}
