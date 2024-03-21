using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class TriggerManager : MonoBehaviour
{
    public BoxCollider trigger;

    public GameEvent onTriggerTouched;

    [SerializeField] bool correctTrigger;
    //[SerializeField] bool levelChangeTrigger;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        onTriggerTouched.Raise(correctTrigger);
    }
}

