using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamForward : MonoBehaviour
{
    public Transform Player;

    private Vector3 CamOffset;

    // Start is called before the first frame update
    void Start()
    {
        CamOffset = Player.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = Player.position - CamOffset;
    }
}
