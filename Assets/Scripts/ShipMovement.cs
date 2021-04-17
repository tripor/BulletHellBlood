using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    private LineRenderer lineRender = null;
    private GameObject lineObj = null;
    private float time = 0;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool moving = false;
    public float flightDuration = 10;
    public float maxDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameRunning && lineObj != null)
        {
            moving = true;
            this.lineRender.SetPosition(0, transform.position);
            time += Time.deltaTime / flightDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            if (time > 1)
            {
                time = 1;
            }
        }
        else
        {
            if (moving)
            {
                time = 0;
                Destroy(this.lineObj);
                lineObj = null;
                lineRender = null;
                moving = false;
            }
        }
    }

    public void setPosition(Vector3 position, GameObject line)
    {
        this.lineObj = line;
        this.lineRender = line.GetComponent<LineRenderer>();
        startPosition = transform.position;
        endPosition = position;
    }
}
