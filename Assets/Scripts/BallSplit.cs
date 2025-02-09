using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BallSplit : MonoBehaviour
{
    
    string levelName,splitterMomentColor;
    

    void Start()
    {
        
        levelName = SceneManager.GetActiveScene().name;
        

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        // Nothing should happen if colliding with a non-player object (anything other than red/blue balls)
        if(!(collision.gameObject.tag != null && collision.gameObject.tag.Contains("Ball")))
            return;

        if(doesBallHaveHalo(collision.gameObject))
        {
            removeHalo(collision.gameObject);
            // if("BlueBall".Equals(collision.gameObject.tag) && "RedSplitterTriangle".Equals(gameObject.tag))
            // {
            //     return;
            // }
            // else if("RedBall".Equals(collision.gameObject.tag) && "BlueSplitterTriangle".Equals(gameObject.tag))
            // {
            //     return;
            // }
            // else //Same color splitter
            // {
            //     List<int> delIdList = new List<int>(); 

            //     //Kill the splitter
            //     int delId = gameObject.GetInstanceID();
            //     delIdList.Add(delId);
            //     Destroy(gameObject);
                
            //     //Update game state
            //     GameStateTracking.UpdateGameStack(delIdList, "Splitter script: " + gameObject.name);
            // }
            // return;
        }

        //Do not change these colors ever
        Color redColor = new Vector4(0.7830189f, 0.1578784f, 0.1071111f,1.0f);
        Color blueColor = new Vector4(0.09019608f, 0.6f, 0.9058824f,1.0f);
        List<int> deletedIdList = new List<int>(); 

        //Used for analytics
        string ballColorBeforeCollision = collision.gameObject.tag;

        //First destroy the spillter and then instantiate the balls
        String tagName = gameObject.tag;
        
        //Get instance ID of the deleted objects for updating state
        int deletedID = gameObject.GetInstanceID();
        deletedIdList.Add(deletedID);
        Destroy(gameObject);

        Debug.Log(tagName + " " + GameObject.FindGameObjectsWithTag(tagName).Length);

        //Get the color of the splitter and assign it to the ball
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(gameObject.tag == "BlinkingSplitter"){
            collision.gameObject.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<ColorChanger>().currentColor;
        } else 
            collision.gameObject.GetComponent<SpriteRenderer>().color = renderer.color;
        
        //Clone the ball
        var go = Instantiate(collision.gameObject, collision.transform.position, collision.transform.rotation);
        go.transform.parent = collision.transform.parent;
        go.transform.localScale = collision.transform.localScale;

        if(doesBallHaveHalo(go))
        {
            removeHalo(go);
        }

        updateBallQueueInKillerScript(go);

        //Add tags for the new balls
        if(collision.gameObject.tag=="PinkBall_RedBall")
        {
            if(blueColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                collision.gameObject.tag = "PinkBall_BlueBall"; //original ball
                go.gameObject.tag = "PinkBall_BlueBall"; //cloned ball
                
                //copy time remaining from original ball
                int org_time = collision.gameObject.GetComponent<CreateBlinkingBall>().timer;
                go.gameObject.GetComponent<CreateBlinkingBall>().updateTimer(org_time - 1);
            }
        }
        else if(collision.gameObject.tag=="PinkBall_BlueBall")
        {
            if(redColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                collision.gameObject.tag = "PinkBall_RedBall";
                go.gameObject.tag = "PinkBall_RedBall";

                //copy time remaining from original ball
                int org_time = collision.gameObject.GetComponent<CreateBlinkingBall>().timer;
                go.gameObject.GetComponent<CreateBlinkingBall>().updateTimer(org_time - 1);
            }
        }
        else
        {
            if(blueColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                go.gameObject.tag = "BlueBall";
                collision.gameObject.tag="BlueBall";
            }
            else if(redColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                go.gameObject.tag = "RedBall";
                collision.gameObject.tag="RedBall";
            }
        }

        //Get splitter colour based on
        splitterMomentColor = collision.gameObject.tag.Contains("RedBall") ? "RedSplitterTriangle" : "BlueSplitterTriangle";

        Debug.Log("Restttarrrt " + RestartButton.isRestartClicked);
       
        

        //Save collision in analytics
        AnalyticsManager._instance.analytics_split_record(levelName, DateTime.Now, splitterMomentColor, ballColorBeforeCollision, gameObject.name,RestartButton.isRestartClicked);

        //Update game state
        GameStateTracking.UpdateGameStack(deletedIdList, "Splitter script: " + gameObject.name);
    
    }

    bool doesBallHaveHalo(GameObject ball)
    {
        foreach (Transform child in ball.transform)
        {
            if(child.name == "Halo")
                return true;
        }
        return false;
    }

    void removeHalo(GameObject go)
    {
        foreach (Transform child in go.transform)
        {
            // Destroy the child game object
            Destroy(child.gameObject);
        }
    }

    void updateBallQueueInKillerScript(GameObject go)
    {
        GameObject wallsParent = GameObject.Find("Parent Walls");

        if (wallsParent != null)
        {
            // Check if the "SelectKiller" script is attached to the parent "Walls" GameObject
            SelectKiller selectKillerScript = wallsParent.GetComponent<SelectKiller>();

            if (selectKillerScript != null)
            {
                // "SelectKiller" script is attached
                GameObject.Find("Parent Walls").GetComponent<SelectKiller>().addBallToQueue(go);
                Debug.Log("The parent Walls GameObject contains a SelectKiller script.");
            }
            else
            {
                // "SelectKiller" script is not attached
                Debug.Log("The parent Walls GameObject does not contain a SelectKiller script.");
            }
        }
        else
        {
            // Parent "Walls" GameObject not found
            Debug.LogError("The parent Walls GameObject could not be found.");
        }
    }
}
