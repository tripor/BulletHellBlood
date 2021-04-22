using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public class Pattern
    {
        public GameObject Bullet;
        [Min(0)]
        public int amount = 0;
    }

    public float speed = 1f;

    public float firingFrequency = 1;
    public int repetitionOfPatterns = 1;

    private float timer = 0;
    private int currentRepetition;
    private int currentPattern;

    public List<Pattern> patterns = new List<Pattern>();
    public float radiusCheck = 2f;
    public LayerMask mask;

    private GameObject targetShip;


    void Start()
    {
        timer = 0;
        currentRepetition = 0;
        currentPattern = 0;
        targetShip = GameManager.Instance.friendlyShips[UnityEngine.Random.Range(0, GameManager.Instance.friendlyShips.Count)];
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > firingFrequency)
        {
            timer = 0;
            fireBullets();
        }
    }
    private void fireBullets()
    {
        currentRepetition++;

        int amount = this.patterns[currentPattern].amount;
        GameObject prefab = this.patterns[currentPattern].Bullet;
        float angles = 360f / amount;
        float angle = 0;
        for (int i = 0; i < amount; i++)
        {
            Instantiate(prefab, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, angle));
            angle += angles;
        }

        if (currentRepetition == repetitionOfPatterns)
        {
            currentRepetition = 0;
            currentPattern++;
            if (currentPattern >= patterns.Count) currentPattern = 0;
        }
    }
}