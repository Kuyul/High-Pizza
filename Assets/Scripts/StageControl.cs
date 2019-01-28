using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    //Declare public variables
    public GameObject pizzaFake; //Pizza Prefab
    public PlayerControl Player;
    public float PizzaFallHeight = 3.0f; //Height above the player hand where the pizza will start dropping from
    public float PizzaFallFrequency = 1.0f;
    public int MaximumUnbalance = 6;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPizza", 0, PizzaFallFrequency);
    }

    //Spawns a pizza at a random location (left/right)
    void SpawnPizza()
    {
        int right;
        bool isRightHand;
        Transform hand;
        //Check whether the weight unbalance is above the threshold.
        var unbalance = GameControl.instance.GetPlayerUnbalance();
        if (Mathf.Abs(unbalance) >= MaximumUnbalance)
        {
            //Unbalanced - heavy on the right
            if (unbalance < 0)
            {
                right = 0;
                isRightHand = false;
                hand = Player.leftPizzaPlate;
            }
            else
            {
                right = 1;
                isRightHand = true;
                hand = Player.rightPizzaPlate;
            }
        }
        else
        {
            right = Random.Range(0, 2);
            if (right == 0)
            {
                hand = Player.leftPizzaPlate;
                isRightHand = false;
            }
            else
            {
                hand = Player.rightPizzaPlate;
                isRightHand = true;
            }
        }

        //Set pizza falling position
        var pizzaFallPos = hand.position + new Vector3(0, PizzaFallHeight, 0);
        var pizzaObj = Instantiate(pizzaFake, pizzaFallPos, Quaternion.identity);
        pizzaObj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Player.WalkSpeed);
        pizzaObj.GetComponent<PizzaBoxScript>().Right = isRightHand;
        //pizzaObj.transform.SetParent(hand);
    }
}
