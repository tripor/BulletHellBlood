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

    public float firingInterval = 1f;
    public GameObject friendlyBullet;
    public float radiusCheck = 2f;
    public LayerMask mask;

    private bool drawingLineCheck = false;
    private float skipPositions;
    private float firingTimer = 0;
    public Vector3 shipLastPosition;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        firingTimer = 0;
        shipLastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameRunning)
        {
            shipLastPosition = transform.position;
            firingTimer += Time.deltaTime;
            if (firingTimer > firingInterval)
            {
                firingTimer = 0;
                GameObject obj = Instantiate(friendlyBullet, transform.position, transform.rotation);
                GameObject closest = null;
                float distance = float.MaxValue;
                for (int i = 0; i < GameManager.Instance.enemyShips.Count; i++)
                {
                    float calculated = Vector3.Distance(GameManager.Instance.enemyShips[i].transform.position, transform.position);
                    if (distance > calculated)
                    {
                        distance = calculated;
                        closest = GameManager.Instance.enemyShips[i];
                    }
                }
                obj.GetComponent<FriendlyBullet>().setTarget(closest);
            }
        }
        if (GameManager.Instance.gameRunning && lineObj != null)
        {
            moving = true;
            if (drawingLineCheck)
            {
                time += Time.deltaTime;
                int index = Mathf.RoundToInt(time / skipPositions);
                if (index < lineRender.positionCount)
                    transform.position = lineRender.GetPosition(index);
            }
            else
            {
                this.lineRender.SetPosition(0, transform.position);
                time += Time.deltaTime / flightDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, time);
                if (time > 1)
                {
                    time = 1;

                }
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
    public void deleteLastLine()
    {
        if (lineObj != null)
        {
            Destroy(lineObj);
            lineObj = null;
            lineObj = null;
        }
    }

    public void drawingLine(GameObject line)
    {
        drawingLineCheck = true;
        this.lineObj = line;
        this.lineRender = line.GetComponent<LineRenderer>();
        startPosition = lineRender.GetPosition(0);
        if (lineRender.positionCount > 0)
        {
            skipPositions = 10f / lineRender.positionCount;
            endPosition = lineRender.GetPosition(1);
        }
    }

    public void setPosition(Vector3 position, GameObject line)
    {
        this.lineObj = line;
        this.lineRender = line.GetComponent<LineRenderer>();
        startPosition = transform.position;
        endPosition = position;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameRunning)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusCheck, mask);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "Enemy")
                {
                    GameManager.Instance.EndGame();
                }
                else
                {
                    GameManager.Instance.RemoveLife();
                    Destroy(hitColliders[i].gameObject);
                }
            }
        }

    }
}
