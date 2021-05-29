using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float blockMovement = 3;

    [SerializeField] private bool xAxis = false;
    private Vector3 pos;
    private Vector3 posX;
    private Vector3 posY;

    void Start()
    {
        pos = transform.position;
        posX = new Vector3(pos.x + blockMovement, pos.y, pos.z);
        posY = new Vector3(pos.x, pos.y, pos.z+ blockMovement);
    }
    void Update()
    {
        if (xAxis)
        {
            xAxisMovement();
        }
        else
        {
            yAxisMovement();
        }
    }

    void xAxisMovement()
    {
        transform.position = Vector3.Lerp(pos, posX, Mathf.PingPong(Time.time * speed, 1.0f));
    }
    void yAxisMovement()
    {
        transform.position = Vector3.Lerp(pos, posY, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    public void toggleXAxis()
    {
        xAxis = true;
    }
}
