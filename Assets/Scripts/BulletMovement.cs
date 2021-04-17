using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private GameObject target;
    public float speed = 1f;
    private float timer = 0;
    public float speedRotation = 0.5f;
    public float lifespan = 100f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        target = GameManager.Instance.friendlyShips[UnityEngine.Random.Range(0, GameManager.Instance.friendlyShips.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameRunning)
        {
            timer += Time.deltaTime;
            if (Vector3.Distance(target.transform.position, transform.position) < 2)
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
            if (timer > lifespan) Destroy(gameObject);
        }
    }
}
