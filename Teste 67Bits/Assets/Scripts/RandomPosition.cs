using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    Vector3 positionMagnitude;

    [SerializeField] private float TimerNewPosition;
    [SerializeField] private float TimerNewPositionC;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimerNewPositionC -= Time.deltaTime;
        if (TimerNewPositionC <= 0)
        {
            TimerNewPositionC = TimerNewPosition;
            positionMagnitude.x = Random.Range(-18, 10);
            positionMagnitude.z = Random.Range(-3, 24);
        }
        gameObject.transform.position = new Vector3(positionMagnitude.x, 0, positionMagnitude.z);
    }
}
