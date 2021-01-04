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
    public GameObject personalityCompetitive; 
    public GameObject personalityNewbie;
    public GameObject personalityExperienced;
    public GameObject personalityUnpredictable;
    public GameObject personalityEdgy;
    public GameObject personalityFastLearner;
    public GameObject personalityGiftedNewbie;

    private float brickHeight;
    private float[] roundCharacteristics = new float[5] { 4.5f, 25f, 10f, 0f, 0f };

    private int episodeNumber = -1;
    private int episodeCount = 0;

    private bool stopped = true;
    private bool haveParameters = false;
    private bool requestingDecision = false;
    private bool firstDecision = true;

    private Observations latestObservations;


    void generatePlayerList()
    {
        List<GameObject> personalities = new List<GameObject>();
        PlayerList = new List<GameObject>();

        personalities.Add(personalityNewbie);
        personalities.Add(personalityCompetitive);
        personalities.Add(personalityExperienced);
        personalities.Add(personalityGiftedNewbie);              
        personalities.Add(personalityEdgy);
        personalities.Add(personalityFastLearner);



        for (int i = 0; i < amountOfPlayersPerRound; i++)
        {
            GameObject player = Instantiate(personalities[i % personalities.Count]);
            PlayerList.Add(player);
            player.SetActive(false);
        }

        foreach (GameObject p in PlayerList)
            DontDestroyOnLoad(p);


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
    void FixedUpdate()
    {
        if (!stopped)
        {
            if (haveParameters)
            {
                time += Time.deltaTime;
                if (ball.GetComponent<BallScript>().getHitFloor())
                    ManagerLogs(0);

                if (destructionCheck())
                    ManagerLogs(1);
            }
            else
            {
                if (!requestingDecision && Academy.IsInitialized && Academy.Instance.IsCommunicatorOn)
                {
                    requestingDecision = true;
                    Debug.Log("Requesting parameters");
                    RequestDecision();
                }
            }
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
        if (episodeNumber >= 0)
            PlayerList[episodeNumber].SetActive(false);
        episodeNumber++;
        if (episodeNumber >= PlayerList.Count)
            episodeNumber = 0;
    }



    private void ManagerLogs(int win)
    {
        stopped = true;
        // Set dot as default for floats
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        //call Personality, give game data to obtain satisfaction
        //write logs
        //restart scene with new player, for now just restart

        //float satisfaction = PlayerList[round-1].GetComponent<Personality>().CalculateSatisfaction(win, time);
        float paddleDistance = paddle.GetComponent<PaddleScript>().distanceRan;
        float ballHits = paddle.GetComponent<PaddleScript>().ballHits;
        int ballBounces = ball.GetComponent<BallScript>().getBallBounces();

        float[] playerVars = PlayerList[episodeNumber].GetComponent<Personality>().GetVariables();
        float[] playerQED = PlayerList[episodeNumber].GetComponent<Personality>().GetGEQ(paddleDistance, ballHits, ballBounces, time, bricksCount(), win);
        SetReward(playerQED[4]);




        string strFilePath = @"./data.csv";
        //session id, time, type of personality, amount of bricks,win/lose


        //float[] outputarray = new float[] { round, roundCharacteristics[0], roundCharacteristics[1], roundCharacteristics[2], time, playerVars[0], bricksCount(), win, playerVars[1], playerVars[2], playerVars[3], playerQED[0], playerQED[1], playerQED[2], playerQED[3], playerQED[4] }; //valores das colunas
        float[] outputarray = new float[] { round, roundCharacteristics[0], roundCharacteristics[1], roundCharacteristics[2], roundCharacteristics[3], roundCharacteristics[4], time, paddleDistance, ballHits, ballBounces, bricksCount(), win, playerVars[0], playerVars[1], playerVars[2], playerVars[3], playerQED[0], playerQED[1], playerQED[2], playerQED[3], playerQED[4] }; //valores das colunas
        this.latestObservations = new Observations(time/10, paddleDistance, ballHits, ballBounces, bricksCount(), win, playerVars, playerQED);
        Debug.Log("PERSONALITY TYPE: "+ playerVars[0]+ " WIN: "+ win+" TIME: "+ time/10 + " Satisfaction: "+ playerQED[4]);
        time = 0;
        paddle.GetComponent<PaddleScript>().resetValues();
        StringBuilder sbOutput = new StringBuilder();
        sbOutput.AppendLine(string.Join(";", outputarray));


        // Create and write the csv file
        // File.WriteAllText(strFilePath, sbOutput.ToString());

        // To append more lines to the csv file
        File.AppendAllText(strFilePath, sbOutput.ToString());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // 
        haveParameters = false;

        if (round == 10)
        {
            round = 0;
            firstDecision = true;
            EndEpisode();
        }
        else
        {
            initGame();
            stopped = false;
        }
    }

    private void EndRound()
    {
        round = 0;
        //get new players, train based on ML, etc.
    }

    void initGame()
    {
        time = 0;
        paddle.transform.position = new Vector3(0, -4, 0);
        ball.transform.position = new Vector3(0, -3, 0);
        ball.GetComponent<BallScript>().Reset();
    }

    private class Observations
    {
        public float time { get; set; }
        public float paddleDistance { get; set; }
        public float ballHits { get; set; }
        public float ballBounces { get; set; }
        public int bricksCount { get; set; }
        public int win { get; set; }
        public float[] playerVars { get; set; }
        public float[] playerQED { get; set; }

        public Observations(float time, float paddleDistance, float ballHits, int ballBounces, int bricksCount, int win, float[] playerVars, float[] playerQED)
        {
            this.time = time;
            this.paddleDistance = paddleDistance;
            this.ballHits = ballHits;
            this.ballBounces = ballBounces;
            this.bricksCount = bricksCount;
            this.win = win;
            this.playerVars = playerVars;
            this.playerQED = playerQED;
        }
    }





    public override void Initialize()
    {
        string strFilePath = @"./data.csv";
        File.WriteAllText(strFilePath, "session id;brick height;paddle speed;ball speed;paddle length;ball size;time;paddle distance;ballHits;ballBounces;amount of bricks;win/lose;type of personality;playerAPM;playerReactionTime;playerPaddleSafety;GEQ - content;GEQ - skillful;GEQ - occupied;GEQ - difficulty;satisfaction"); //COMMENT THIS IF YOU JUST WANT TO APPEND - last 5 are player attributes
        File.AppendAllText(strFilePath, Environment.NewLine);

        round = 0;
        generatePlayerList();
        this.latestObservations = new Observations(1, 1, 1, 1, 1, 1, new float[4], new float[4]);
        paddle = GameObject.Find("Paddle");
        ball = GameObject.Find("Ball");
        stopped = true;
        requestingDecision = false;
        haveParameters = false;
        RequestDecision();

        Time.timeScale = 10.0f;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.latestObservations.time);
        sensor.AddObservation(this.latestObservations.paddleDistance);
        sensor.AddObservation(this.latestObservations.ballHits);
        sensor.AddObservation(this.latestObservations.ballBounces);
        sensor.AddObservation(this.latestObservations.bricksCount);
        sensor.AddObservation(this.latestObservations.win);
        sensor.AddObservation(this.latestObservations.playerVars[0]);
        sensor.AddObservation(this.latestObservations.playerVars[1]);
        sensor.AddObservation(this.latestObservations.playerVars[2]);
        sensor.AddObservation(this.latestObservations.playerVars[3]);
        sensor.AddObservation(this.latestObservations.playerQED[0]);
        sensor.AddObservation(this.latestObservations.playerQED[1]);
        sensor.AddObservation(this.latestObservations.playerQED[2]);
        sensor.AddObservation(this.latestObservations.playerQED[3]);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        roundCharacteristics[0] = (vectorAction[0] + 1) * 2 + 1;
        roundCharacteristics[1] = (vectorAction[1] + 1) * 10 + 15;
        roundCharacteristics[2] = (vectorAction[2] + 1) * 5 + 2;
        roundCharacteristics[3] = (vectorAction[3] + 1) * 10 + 10;
        roundCharacteristics[4] = (vectorAction[4] + 2) * 2;

        brickHeight = roundCharacteristics[0];
        resetBricks(); // Delete old bricks and create new ones
        paddle.GetComponent<PaddleScript>().PaddleSpeed = roundCharacteristics[1];
        ball.GetComponent<BallScript>().SetSpeed(roundCharacteristics[2]);
        paddle.transform.localScale = new Vector3(roundCharacteristics[3], 1f, 4);
        ball.transform.localScale = new Vector3(roundCharacteristics[4], roundCharacteristics[4], roundCharacteristics[4]);
        PlayerList[episodeNumber].SetActive(true);
        PlayerList[episodeNumber].GetComponent<Personality>().Play();
        round++;
        Debug.Log("Starting the game");
        haveParameters = true;
        requestingDecision = false;

    }

    public override void Heuristic(float[] actionsOut)
    {
    }
    public override void OnEpisodeBegin()
    {
        if (firstDecision)
        {
            firstDecision = false;
            return;
        }
        Debug.Log("episode begin number: " + episodeCount);
        this.latestObservations = new Observations(1, 1, 1, 1, 1, 1, new float[4], new float[4]);
        episodeCount++;
        initGame(); // Find the ball and Reset the position of the ball and paddle and time
        SummonPlayer(); // Change the player
        stopped = false;
    }
}
