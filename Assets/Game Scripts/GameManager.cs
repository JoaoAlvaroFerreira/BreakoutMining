using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject paddle;
    GameObject ball;
    public GameObject brick;
    private List<GameObject> bricks;

    public GameObject personality; //TEMP, will be personality list later

    private float brickHeight;

    void Start()
    {
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
    
       if(ball.GetComponent<BallScript>().getHitFloor())
        ManagerLogs();

       if(destructionCheck())
        Debug.Log("MakeLog Win");
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

    private void SummonPlayer(){
        Instantiate(personality);
        personality.GetComponent<Personality>().ball = this.ball;
        personality.GetComponent<Personality>().paddle = this.paddle;
    }

    private void ManagerTuning(){
        //TEMP
        brickHeight = 4.5f;
        paddle.GetComponent<PaddleScript>().PaddleSpeed = 15f;
        ball.GetComponent<BallScript>().SetSpeed(5);
    }

    private void ManagerLogs(){
        //call Personality
        //write logs
        //restart scene with new player, for now just restart
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
