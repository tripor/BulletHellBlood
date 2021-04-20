using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShipInteractor : MonoBehaviour
{

    public Camera playerNonXrCamera;
    public GameObject line;
    public GameObject rightHand;

    public bool drawLine = true;
    public bool moveToPosition = false;
    public bool pickAndPlace = false;

    private bool usingXr;
    private bool creatingPath;

    private GameObject ship;
    private LineRenderer lineRender = null;
    private GameObject lineObj = null;
    private bool drawingLineStarted = false;
    public float maxDistanceDraw = 0.5f;
    public GameObject movingShipInfo;
    public GameObject middleCircle;

    // Start is called before the first frame update
    void Start()
    {
        creatingPath = false;

        if (XRSettings.isDeviceActive)
        {
            usingXr = true;
        }
        else
        {
            usingXr = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.gameRunning)
        {
            if (!usingXr)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!creatingPath)
                    {
                        RaycastHit hit;
                        Ray ray = playerNonXrCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "PlayerShip")
                        {
                            interactWithShipStarted(hit.transform.gameObject);
                        }
                    }
                    else
                    {
                        interactWithShipExited();
                    }
                }
            }
            if (creatingPath)
            {
                Vector3 shipLastPos = ship.GetComponent<ShipMovement>().shipLastPosition;
                if (usingXr)
                {
                    Vector3 position = rightHand.transform.position + transform.position;
                    float distance = Vector3.Distance(position, shipLastPos);
                    if (drawLine)
                        lineRender.positionCount++;
                    if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                    {
                        float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                        lineRender.SetPosition(lineRender.positionCount - 1, new Vector3(
                            ((position.x - shipLastPos.x) * percentage) + shipLastPos.x,
                            ((position.y - shipLastPos.y) * percentage) + shipLastPos.y,
                            ((position.z - shipLastPos.z) * percentage) + shipLastPos.z
                        ));
                    }
                    else
                        lineRender.SetPosition(lineRender.positionCount - 1, position);
                }
                else
                {
                    Vector3 position = transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up;
                    float distance = Vector3.Distance(position, shipLastPos);
                    if (drawLine && (Input.GetMouseButtonDown(1) || drawingLineStarted))
                    {
                        drawingLineStarted = true;
                        lineRender.positionCount++;
                    }
                    if (drawLine && !drawingLineStarted)
                    {
                        if (distance > maxDistanceDraw)
                        {
                            float percentage = maxDistanceDraw / distance;
                            lineRender.SetPosition(lineRender.positionCount - 1, new Vector3(
                                ((position.x - shipLastPos.x) * percentage) + shipLastPos.x,
                                ((position.y - shipLastPos.y) * percentage) + shipLastPos.y,
                                ((position.z - shipLastPos.z) * percentage) + shipLastPos.z
                            ));
                        }
                        else
                            lineRender.SetPosition(lineRender.positionCount - 1, position);
                    }
                    else if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                    {
                        float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                        lineRender.SetPosition(lineRender.positionCount - 1, new Vector3(
                            ((position.x - shipLastPos.x) * percentage) + shipLastPos.x,
                            ((position.y - shipLastPos.y) * percentage) + shipLastPos.y,
                            ((position.z - shipLastPos.z) * percentage) + shipLastPos.z
                        ));
                    }
                    else
                        lineRender.SetPosition(lineRender.positionCount - 1, position);
                }
            }
        }
        else
        {
            creatingPath = false;
            if (lineObj != null)
            {
                Destroy(lineObj);
                lineObj = null;
                lineRender = null;
            }
        }
    }

    public void interactWithShipStarted(GameObject ship)
    {
        if (!creatingPath && !GameManager.Instance.gameRunning)
        {
            if (!usingXr)
            {
                movingShipInfo.SetActive(true);
                middleCircle.SetActive(false);
            }
            creatingPath = true;
            this.ship = ship;
            this.ship.GetComponent<ShipMovement>().deleteLastLine();
            this.lineObj = Instantiate(line);
            this.lineRender = lineObj.GetComponent<LineRenderer>();
            this.lineRender.SetPosition(0, ship.transform.position);
            if (drawLine)
            {
                drawingLineStarted = false;
            }
        }
    }

    public void interactWithShipExited()
    {
        if (creatingPath && !GameManager.Instance.gameRunning)
        {
            Vector3 shipLastPos = ship.GetComponent<ShipMovement>().shipLastPosition;
            if (usingXr)
            {
                // ship.GetComponent<ShipMovement>().setPosition(rightHand.transform.position + transform.position, lineObj);

                Vector3 position = rightHand.transform.position + transform.position;
                float distance = Vector3.Distance(position, shipLastPos);
                if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                {
                    float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                    Vector3 newPosition = new Vector3(
                            ((position.x - shipLastPos.x) * percentage) + shipLastPos.x,
                            ((position.y - shipLastPos.y) * percentage) + shipLastPos.y,
                            ((position.z - shipLastPos.z) * percentage) + shipLastPos.z
                        );
                    if (drawLine)
                    {
                        ship.GetComponent<ShipMovement>().drawingLine(lineObj);
                    }
                    else if (pickAndPlace)
                    {
                        ship.transform.position = newPosition;
                        Destroy(lineObj);
                    }
                    else if (moveToPosition)
                    {
                        ship.GetComponent<ShipMovement>().setPosition(newPosition, lineObj);
                    }

                }
                else
                {
                    if (drawLine)
                    {
                        ship.GetComponent<ShipMovement>().drawingLine(lineObj);
                    }
                    else if (pickAndPlace)
                    {
                        ship.transform.position = position;
                        Destroy(lineObj);
                    }
                    else if (moveToPosition)
                    {
                        ship.GetComponent<ShipMovement>().setPosition(position, lineObj);
                    }
                }

                lineObj = null;
                lineRender = null;
            }
            else
            {
                // ship.GetComponent<ShipMovement>().setPosition(transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up, lineObj);

                movingShipInfo.SetActive(false);
                middleCircle.SetActive(true);
                Vector3 position = transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up;
                float distance = Vector3.Distance(position, shipLastPos);
                if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                {
                    float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                    Vector3 newPosition = new Vector3(
                            ((position.x - shipLastPos.x) * percentage) + shipLastPos.x,
                            ((position.y - shipLastPos.y) * percentage) + shipLastPos.y,
                            ((position.z - shipLastPos.z) * percentage) + shipLastPos.z
                        );
                    if (drawLine)
                    {
                        ship.GetComponent<ShipMovement>().drawingLine(lineObj);
                    }
                    else if (pickAndPlace)
                    {
                        ship.transform.position = newPosition;
                        Destroy(lineObj);
                    }
                    else if (moveToPosition)
                    {
                        ship.GetComponent<ShipMovement>().setPosition(newPosition, lineObj);
                    }
                }
                else
                {
                    if (drawLine)
                    {
                        ship.GetComponent<ShipMovement>().drawingLine(lineObj);
                    }
                    else if (pickAndPlace)
                    {
                        ship.transform.position = position;
                        Destroy(lineObj);
                    }
                    else if (moveToPosition)
                    {
                        ship.GetComponent<ShipMovement>().setPosition(position, lineObj);
                    }
                }

                lineObj = null;
                lineRender = null;
            }
            creatingPath = false;
        }
    }
}
