using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unpredictable : Personality
{


    protected override void Start()
    {
        base.Start();    // call base class

        int minAPM = 200 + RandomNumber(0, 200);
        int maxAPM = RandomNumber(minAPM, 500);

        min_reaction_time = (0.01f * 1000 - RandomNumber(0, 9)) / 1000; //difference between eye and hand
        max_reaction_time = (min_reaction_time * 1000 + RandomNumber(0, 35)) / 1000; //difference between eye and hand

        min_paddle_safety_distance = 1f;
        max_paddle_safety_distance = 1.3f;
        GenerateValues();

        InvokeRepeating("PaddleMovement", 0, (float)60 / APM);
    }


    // Instantiate random number generator.  
    private readonly System.Random _random = new System.Random();

    // Generates a random number within a range.      
    public int RandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }

    public override int MoveHeuristic()
    {
        //VERY BASIC TEST VERSION, DO BETTER LATER
        float paddleX = paddle.transform.position.x;
        float ballX = ball.transform.position.x;


        if (ball.GetComponent<Rigidbody2D>().velocity.y < 0)
            ballX = calcTrajectory();

        float distanceX = paddleX - ballX;

        if (Math.Abs(distanceX) <= paddle_safety_distance)
            return 0;

        if (distanceX > paddle_safety_distance)
            return 1;

        if (distanceX < paddle_safety_distance)
            return 2;

        return 0;

    }

    float calcVerticalTime()
    {

        return (paddle.transform.position.y - ball.transform.position.y) / ball.GetComponent<Rigidbody2D>().velocity.y;
    }

    float calcTrajectory()
    {
        float dropTime = calcVerticalTime();

        float prediction = ball.transform.position.x + ball.GetComponent<Rigidbody2D>().velocity.x * dropTime;

        if (prediction > 9)
        {
            float aux = prediction - 9;
            prediction = prediction - aux;
        }
        return prediction;
    }


    public override float[] GetVariables()
    {
        float[] a = { 4, APM, reaction_time, paddle_safety_distance };
        return a;
    }


    public override float[] GetGEQ(float paddleDistance, float ballHits, int ballBounces, float time, int bricks, int win)
    {
        //I felt content
        float content = 0;
        if (win == 1)
            content++;
        if (time > 40 && time < 180)
            content++;
        if (bricks < 20)
            content++;
        if (bricks < 10)
            content++;
        if (ballHits * 2 < (30 - bricks))
            content++;

        //I felt skilful
        float skillful = 0;
        if (ballHits * 2 < (30 - bricks))
            skillful++;
        if (win == 1)
            skillful++;
        if (time / paddleDistance > 10)
            skillful++;
        if (time / paddleDistance > 16)
            skillful++;
        if (bricks < 15)
            skillful++;

        //I was fully occupied with the game
        float occupied = 0;
        if (time / paddleDistance < 14)
            occupied++;
        if (time / paddleDistance < 8)
            occupied++;
        if (time / ballHits < 3)
            occupied++;
        if (ballHits > 3)
            occupied++;
        if (ballHits > 8)
            occupied++;

        //I thought it was hard
        float hard = 5;
        if (win == 0)
            hard--;
        if (time / ballHits > 4)
            hard--;
        if (bricks > 20)
            hard--;
        if (bricks > 25)
            hard--;

        //overall enjoyment
        float satisfaction = (float)(content + skillful + occupied + hard);

        float[] a = { content, skillful, occupied, hard, satisfaction };

        return a;
    }

}
