using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateBlinkingBall : MonoBehaviour
{
   public GameObject prefab;
   public int timer = 0;

   void Start()
   {
       if("PinkBall_RedBall" == gameObject.tag || "PinkBall_BlueBall" == gameObject.tag){
          timer = 10;
          InvokeRepeating ("runTimer", 0.0f, 1.0f);
       } 
       if(gameObject.tag=="BlueBall"){
        prefab = Resources.Load<GameObject>("Prefabs/PinkBallBlueBall");
       } else {
         prefab = Resources.Load<GameObject>("Prefabs/PinkBallRedBall");
       }
   }

   void Update()
   {
        // if((gameObject.tag=="PinkBall_BlueBall" || gameObject.tag=="PinkBall_RedBall")){

        //   var go = Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);

        //   go.transform.parent = gameObject.transform.parent;
        //   go.transform.localScale = gameObject.transform.localScale;
        //   Destroy(gameObject);
        // }  
   }

    void runTimer()
    {
      if (timer <= 0)
      {
        gameObject.GetComponent<BlinkBall>().destroyBalls();
      }

      // Run the timer every second
      timer -= 1;      
    }

    public void updateTimer(int newTime)
    {
      timer = newTime;
    }
  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if("PowerUp_Star" == collision.gameObject.tag){
          GamesManager.powerAttainStartTime = DateTime.Now; 

          // Destroy the star and the canvas holding it
          // Save ID of deleted game object
          GameObject parent = collision.gameObject.transform.parent.gameObject;
          List<int> deletedIdList = new List<int>(); 
          int deletedID = parent.GetInstanceID();
          deletedIdList.Add(deletedID);

          Destroy(parent);

          if("RedBall" == gameObject.tag || "BlueBall" == gameObject.tag){
            var go = Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);

            go.transform.parent = gameObject.transform.parent;
            go.transform.localScale = gameObject.transform.localScale;
            deletedIdList.Add(gameObject.GetInstanceID());
            Destroy(gameObject);
          }
          else if("PinkBall_RedBall" == gameObject.tag || "PinkBall_BlueBall" == gameObject.tag){
            timer += 10;
          } 

          //Update game state
          GameStateTracking.UpdateGameStack(deletedIdList, "Create blink ball script: " + collision.gameObject.name);
        }
    }

}
