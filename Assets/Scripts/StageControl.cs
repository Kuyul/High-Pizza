using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    //Declare public variables
    public GameObject Pizza; //Pizza Prefab
    public PlayerControl Player;
    public float PizzaFallHeight = 2.0f; //Height above the player hand where the pizza will start dropping from

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPizza", 0, 1.0f);
    }

    //Spawns a pizza at a random location (left/right)
    void SpawnPizza()
    {
        var random = Random.Range(0, 2);
        Transform hand;
        if(random == 0)
        {
            hand = Player.leftPizzaPlate;
        }
        else
        {
            hand = Player.rightPizzaPlate;
        }

        //Set pizza falling position
        var pizzaFallPos = hand.position + new Vector3(0, PizzaFallHeight, 0);
        var pizzaObj = Instantiate(Pizza, pizzaFallPos, Quaternion.identity);
        pizzaObj.transform.SetParent(hand);
    }
}
