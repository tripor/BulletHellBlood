using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipLogic : MonoBehaviour
{
    [Serializable]
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


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        currentRepetition = 0;
        currentPattern = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameRunning)
        {
            transform.position += transform.forward * Time.deltaTime;
            timer += Time.deltaTime;
            if (timer > firingFrequency)
            {
                timer = 0;
                fireBullets();
            }
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
            Instantiate(prefab, transform.position, Quaternion.Euler(angle, 0f, 0f));
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
