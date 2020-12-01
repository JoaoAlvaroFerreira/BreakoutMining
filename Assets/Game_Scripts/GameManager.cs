using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class GameManager : Agent
{
    // Start is called before the first frame update
    static int round = 0;
    float time;
    GameObject paddle;
    GameObject ball;
    public GameObject brick;
    private List<GameObject> bricks = new List<GameObject>();

    public int amountOfPlayersPerRound;
    public static List<GameObject> PlayerList;
    public GameObject personalityCompetitive; //TEMP, will be personality list later
    public GameObject personalityNewbie;

    private float brickHeight;
    private float[] roundCharacteristics = new float[3] { 4.5f, 25f, 10f };

    private bool stopped = true;

    private void ManagerTuning()
    {
        //TEMP
        brickHeight = roundCharacteristics[0];
        paddle.GetComponent<PaddleScript>().PaddleSpeed = roundCharacteristics[1];
        ball.GetComponent<BallScript>().SetSpeed(roundCharacteristics[2]);
    }

    void generatePlayerList()
    {
        List<GameObject> personalities = new List<GameObject>();
        PlayerList = new List<GameObject>();
        personalities.Add(personalityNewbie); 
        personalities.Add(personalityCompetitive);

        for (int i = 0; i < amountOfPlayersPerRound; i++)
        {
            GameObject player = Instantiate(personalities[i % personalities.Count]);
            PlayerList.Add(player);
            player.SetActive(false);
        }

        foreach (GameObject p in PlayerList)
            DontDestroyOnLoad(p);

        //register to csv ()
        // test
        // ML AGENTS HEREEEEEEEEEEEEE

    }

    void initGame()
    {
        time = 0;
        paddle = GameObject.Find("Paddle");
        ball = GameObject.Find("Ball");

        
    }
    void resetBricks()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            Destroy(bricks[i]);
        }

        bricks = new List<GameObject>();



        for (int i = 0; i < 9; i++)
        {
            GameObject tijolo = Instantiate(brick, new Vector3(i * 2 - 8f, brickHeight, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject tijolo = Instantiate(brick, new Vector3(i * 2 - 8f, brickHeight - 0.5f, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject tijolo = Instantiate(brick, new Vector3(i * 2 - 8f, brickHeight - 1f, 0), Quaternion.identity);
            bricks.Add(tijolo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopped)
        {
            time += Time.deltaTime;
            if (ball.GetComponent<BallScript>().getHitFloor())
                ManagerLogs(0);

            if (destructionCheck())
                ManagerLogs(1);
        }
    }

    private bool destructionCheck()
    {
        bool a = true;
        foreach (GameObject brick in bricks)
        {
            if (brick != null)
                a = false;
        }
        return a;
    }

    private int bricksCount()
    {
        int a = 0;
        foreach (GameObject brick in bricks)
        {
            if (brick != null)
                a++;
        }
        return a;
    }

    private void SummonPlayer()
    {
        PlayerList[round - 1].SetActive(true);
        PlayerList[round - 1].GetComponent<Personality>().Play();
    }



    private void ManagerLogs(int win)
    {
        stopped = true;
        paddle.SetActive(false);
        ball.SetActive(false);
        //call Personality, give game data to obtain satisfaction
        //write logs
        //restart scene with new player, for now just restart

        //float satisfaction = PlayerList[round-1].GetComponent<Personality>().CalculateSatisfaction(win, time);
        float paddleDistance = paddle.GetComponent<PaddleScript>().distanceRan;
        float ballHits = paddle.GetComponent<PaddleScript>().ballHits;

        float[] playerVars = PlayerList[round - 1].GetComponent<Personality>().GetVariables();
        float[] playerQED = PlayerList[round - 1].GetComponent<Personality>().GetGEQ(paddleDistance, ballHits, time, bricksCount(), win);

        PlayerList[round - 1].SetActive(false);



        string strFilePath = @"./data.csv";

        if (round == 1)
        {
            File.WriteAllText(strFilePath, "session id, brick height, paddle speed, ball speed, time, type of personality, amount of bricks,win/lose, playerAPM, playerReactionTime, playerPaddleSafety, GEQ - content, GEQ- skillful, GEQ - occupied, GEQ - difficulty, satisfaction"); //COMMENT THIS IF YOU JUST WANT TO APPEND - last 5 are player attributes
            File.AppendAllText(strFilePath, Environment.NewLine);
        }
        //session id, time, type of personality, amount of bricks,win/lose


        float[] outputarray = new float[] { round, roundCharacteristics[0], roundCharacteristics[1], roundCharacteristics[2], time, playerVars[0], bricksCount(), win, playerVars[1], playerVars[2], playerVars[3], playerQED[0], playerQED[1], playerQED[2], playerQED[3], playerQED[4] }; //valores das colunas

        StringBuilder sbOutput = new StringBuilder();
        sbOutput.AppendLine(string.Join(",", outputarray));

        // Create and write the csv file
        // File.WriteAllText(strFilePath, sbOutput.ToString());

        // To append more lines to the csv file
        File.AppendAllText(strFilePath, sbOutput.ToString());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // 

        Debug.Log("Episode end");
        EndEpisode();

        if (round == amountOfPlayersPerRound)
            EndRound();
    }

    private void EndRound()
    {
        round = 0;
        //get new players, train based on ML, etc.
    }




















    public override void Initialize()
    {
        stopped = true;
        if (round == 0)
            generatePlayerList();
        Debug.Log("Initialize");
        initGame();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log(vectorAction[1]);
    }

    public override void Heuristic(float[] actionsOut)
    {
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode begin");
        stopped = true;
        round++;
        paddle.transform.position = new Vector3(0, -4, 0);
        ball.transform.position = new Vector3(0, -3, 0);
        ball.GetComponent<BallScript>().Reset();
        paddle.SetActive(true);
        ball.SetActive(true);
        initGame();
        SummonPlayer();
        ManagerTuning();
        resetBricks();
        RequestDecision();
        stopped = false;
    }
}
