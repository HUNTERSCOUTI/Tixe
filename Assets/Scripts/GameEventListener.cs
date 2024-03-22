using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;

    public LevelChoiceEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Triggers trigger)
    {
        Response.Invoke(trigger);
    }
}

[System.Serializable]
public class LevelChoiceEvent : UnityEvent<Triggers> { }

public enum Triggers
{
    LevelChange,
    Correct,
    Wrong
}

