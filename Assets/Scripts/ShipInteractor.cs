using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShipInteractor : MonoBehaviour
{

    public Camera playerNonXrCamera;
    public GameObject line;
    public GameObject rightHand;

    private bool usingXr;
    private bool creatingPath;

    private GameObject ship;
    private LineRenderer lineRender = null;
    private GameObject lineObj = null;

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
                if (usingXr)
                {
                    Vector3 position = rightHand.transform.position + transform.position;
                    float distance = Vector3.Distance(position, ship.transform.position);
                    if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                    {
                        float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                        lineRender.SetPosition(1, new Vector3(
                            ((position.x - ship.transform.position.x) * percentage) + ship.transform.position.x,
                            ((position.y - ship.transform.position.y) * percentage) + ship.transform.position.y,
                            ((position.z - ship.transform.position.z) * percentage) + ship.transform.position.z
                        ));
                    }
                    else
                        lineRender.SetPosition(1, position);
                }
                else
                {
                    Vector3 position = transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up;
                    float distance = Vector3.Distance(position, ship.transform.position);
                    if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                    {
                        float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                        lineRender.SetPosition(1, new Vector3(
                            ((position.x - ship.transform.position.x) * percentage) + ship.transform.position.x,
                            ((position.y - ship.transform.position.y) * percentage) + ship.transform.position.y,
                            ((position.z - ship.transform.position.z) * percentage) + ship.transform.position.z
                        ));
                    }
                    else
                        lineRender.SetPosition(1, position);
                }
            }
        }
        else
        {
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
            creatingPath = true;
            this.ship = ship;
            this.lineObj = Instantiate(line);
            this.lineRender = lineObj.GetComponent<LineRenderer>();
            this.lineRender.SetPosition(0, ship.transform.position);
        }
    }

    public void interactWithShipExited()
    {
        if (creatingPath && !GameManager.Instance.gameRunning)
        {
            if (usingXr)
            {
                // ship.GetComponent<ShipMovement>().setPosition(rightHand.transform.position + transform.position, lineObj);

                Vector3 position = rightHand.transform.position + transform.position;
                float distance = Vector3.Distance(position, ship.transform.position);
                if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                {
                    float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                    ship.transform.position = new Vector3(
                            ((position.x - ship.transform.position.x) * percentage) + ship.transform.position.x,
                            ((position.y - ship.transform.position.y) * percentage) + ship.transform.position.y,
                            ((position.z - ship.transform.position.z) * percentage) + ship.transform.position.z
                        );
                }
                else
                    ship.transform.position = position;
                Destroy(lineObj);

                lineObj = null;
                lineRender = null;
            }
            else
            {
                // ship.GetComponent<ShipMovement>().setPosition(transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up, lineObj);

                Vector3 position = transform.position + playerNonXrCamera.transform.forward * 2 + Vector3.up;
                float distance = Vector3.Distance(position, ship.transform.position);
                if (distance > ship.GetComponent<ShipMovement>().maxDistance)
                {
                    float percentage = ship.GetComponent<ShipMovement>().maxDistance / distance;
                    ship.transform.position = new Vector3(
                            ((position.x - ship.transform.position.x) * percentage) + ship.transform.position.x,
                            ((position.y - ship.transform.position.y) * percentage) + ship.transform.position.y,
                            ((position.z - ship.transform.position.z) * percentage) + ship.transform.position.z
                        );
                }
                else
                    ship.transform.position = position;
                Destroy(lineObj);

                lineObj = null;
                lineRender = null;
            }
            creatingPath = false;
        }
    }
}
