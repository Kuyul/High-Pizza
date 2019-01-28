using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxScript : MonoBehaviour
{
    //Declare public variables
    public bool Right;

    //Declare private variables
    private float pizzaOffset = 0.5f;
    private bool fix = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PizzaReal")
        {
            Destroy(gameObject);
            GameObject temp = Instantiate(GameControl.instance.pizza, other.transform.position, Quaternion.identity);
            temp.transform.SetParent(other.gameObject.transform);
            temp.GetComponent<Rigidbody>().isKinematic = true;
            temp.transform.localPosition = new Vector3(0, pizzaOffset, 0);
            temp.transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (!fix) { 
                if (Right)
                {
                    GameControl.instance.AddRight();
                }
                else
                {
                    GameControl.instance.AddLeft();
                }
            }
            fix = true;
        }

        if (other.tag == "PizzaPlate")
        {
            Destroy(gameObject);
            GameObject temp = Instantiate(GameControl.instance.pizza, other.transform.position, Quaternion.identity);
            temp.transform.SetParent(other.gameObject.transform);
            temp.GetComponent<Rigidbody>().isKinematic = true;
            temp.transform.localPosition = new Vector3(0, 0.05f, 0);
            temp.transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (!fix)
            {
                if (Right)
                {
                    GameControl.instance.AddRight();
                }
                else
                {
                    GameControl.instance.AddLeft();
                }
            }
            fix = true;
        }
    }
}

