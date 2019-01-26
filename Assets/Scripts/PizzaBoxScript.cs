using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxScript : MonoBehaviour
{
    bool Fix = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PizzaBox")
        {
            if (!Fix)
            {
                //Get Collider
                var col = collision.gameObject.GetComponent<BoxCollider>();
                var offset = col.bounds.extents.y / 2; //half height of the previous box
                offset += GetComponent<BoxCollider>().bounds.extents.y / 2; //half height of this box
                Debug.Log(offset);
                transform.position = collision.transform.position + new Vector3(0, offset, 0);
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Fix = true;
                StartCoroutine(FixAfterSec(collision, offset));
            }
        }
    }

    IEnumerator FixAfterSec(Collision collision, float offset)
    {
        yield return new WaitForSeconds(1.0f);
        
    }
}
