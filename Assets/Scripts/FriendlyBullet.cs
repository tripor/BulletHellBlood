using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBullet : MonoBehaviour
{

    private GameObject target;
    private Vector3 startPosition;
    public float speedRotation = 0.5f;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameRunning)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 10)
            {
                transform.LookAt(target.transform.position);
                transform.rotation *= Quaternion.Euler(90, 0, 0);
            }
            else
            {
                Vector3 dir = target.transform.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation * Quaternion.Euler(-90, 0, 0), rot, speedRotation * Time.deltaTime) * Quaternion.Euler(90, 0, 0);
            }
            transform.position += transform.up * Time.deltaTime * speed;
        }
    }

    public void setTarget(GameObject ship)
    {
        if (ship == null)
        {
            Destroy(gameObject);
            return;
        }
        startPosition = transform.position;
        target = ship;
    }
}
