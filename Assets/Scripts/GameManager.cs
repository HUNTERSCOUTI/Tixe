using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    MapSpawning mapSpawner;
    TriggerManager triggerManager;

    [Space]
    [SerializeField]
    private float PlayerLevel = -1f; //-1 being the star level where the player gets a preview of the normal map.

    private string[] anomalyMap = { "CORRECTLEVEL", "TRIGGERAREA", "PLAYROOM", "TRIGGERAREA", "WRONGLEVEL" };
    private string[] normalMap  = { "WRONGLEVEL", "TRIGGERAREA", "PLAYROOM", "TRIGGERAREA", "CORRECTLEVEL" };

    void Start()
    {
        triggerManager = FindObjectOfType<TriggerManager>();
        mapSpawner = FindAnyObjectByType<MapSpawning>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        
    }
}
