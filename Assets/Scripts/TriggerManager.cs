using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class TriggerManager : MonoBehaviour
{
    // LevelChangeTrigger | CorrectTrigger | WrongTrigger

    public BoxCollider trigger;

    public UnityEvent triggerEvent;

    [HideInInspector]
    public bool isMovingForward = true;

    void Start()
    {
        if (triggerEvent == null)
            triggerEvent = new UnityEvent();

        triggerEvent.AddListener(Ping);
    }

    void Update()
    {
        
    }

    void Ping()
    {
        Debug.Log("Ping");
    }

    void OnTriggerEnter(Collider other)
    {
        if(transform.gameObject.name == "LevelChangeTrigger")
        {
            Debug.Log("Level Change Trigger trigger touched");
        }
        else if (transform.gameObject.name == "CForwardTrigger")
        {
            Debug.Log("Correct forward trigger touched");
        }
        else if (transform.gameObject.name == "CBackwardTrigger")
        {
            Debug.Log("Correct backward trigger touched");
        }
        else if (transform.gameObject.name == "WForwardTrigger")
        {
            Debug.Log("Wrong forward trigger touched");
        }
        else if (transform.gameObject.name == "WBackwardTrigger")
        {
            Debug.Log("Wrong backward trigger touched");
        }
    }
}

