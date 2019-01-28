using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxScript : MonoBehaviour
{
    private float pizzaOffset = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PizzaReal")
        {
            Destroy(gameObject);
            GameObject temp = Instantiate(GameControl.instance.pizza, other.transform.position, Quaternion.identity);
            temp.transform.SetParent(other.gameObject.transform);
            temp.GetComponent<Rigidbody>().isKinematic = true;
            temp.transform.localPosition = new Vector3(0, pizzaOffset , 0);
            temp.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

