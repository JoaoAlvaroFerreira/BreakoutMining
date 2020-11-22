using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    static int round = 0;
    float time;
    GameObject paddle;
    GameObject ball;
    public GameObject brick;
    private List<GameObject> bricks;

    public GameObject personality; //TEMP, will be personality list later
    public GameObject personalityNewbie;

    private float brickHeight;

    void Start()
    {
        round++;
        time = 0;
        paddle = GameObject.Find("Paddle");
        ball = GameObject.Find("Ball"); 
        SummonPlayer();
        ManagerTuning();
        
        bricks = new List<GameObject>();

        
        
        for(int i = 0; i < 9; i++){
            GameObject tijolo = Instantiate (brick, new Vector3(i*2-8f, brickHeight, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
        for(int i = 0; i < 9; i++){
            GameObject tijolo = Instantiate (brick, new Vector3(i*2-8f, brickHeight-0.5f, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
        for(int i = 0; i < 9; i++){
            GameObject tijolo = Instantiate (brick, new Vector3(i*2-8f, brickHeight-1f, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    
        time += Time.deltaTime;
       if(ball.GetComponent<BallScript>().getHitFloor())
        ManagerLogs(0);

       if(destructionCheck())
        ManagerLogs(1);
    }

    private bool destructionCheck(){
        bool a = true;
        foreach(GameObject brick in bricks)
        {
            if(brick != null)
            a = false;
        }
        return a;
    }

    private int bricksCount(){
        int a = 0;
        foreach(GameObject brick in bricks)
        {
            if(brick != null)
            a++;
        }
        return a;
    }

    private void SummonPlayer(){
        Instantiate(personalityNewbie);
    }

    private void ManagerTuning(){
        //TEMP
        brickHeight = 4.5f;
        paddle.GetComponent<PaddleScript>().PaddleSpeed = 15f;
        ball.GetComponent<BallScript>().SetSpeed(10);
    }

    private void ManagerLogs(int win){
        //call Personality, give game data to obtain satisfaction
        //write logs
        //restart scene with new player, for now just restart

        string strFilePath = @"./data.csv";

        if(round == 1){
        File.WriteAllText(strFilePath,"session id, time, type of personality, amount of bricks,win/lose"); //COMMENT THIS IF YOU JUST WANT TO APPEND
         File.AppendAllText(strFilePath,Environment.NewLine);}
        //session id, time, type of personality, amount of bricks,win/lose
       
        
        int[] outputarray = new int[]{round,(int)time, 0, bricksCount() ,win};

        StringBuilder sbOutput = new StringBuilder();
        sbOutput.AppendLine(string.Join(",", outputarray));
 
        // Create and write the csv file
       // File.WriteAllText(strFilePath, sbOutput.ToString());
 
        // To append more lines to the csv file
        File.AppendAllText(strFilePath, sbOutput.ToString());
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
