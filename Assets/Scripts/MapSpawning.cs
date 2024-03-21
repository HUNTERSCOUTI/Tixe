using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class MapSpawning : MonoBehaviour
{
    private TriggerManager triggerManager;

    [Header("Maps")]
    public GameObject AnomalyMapOne;
    public GameObject NormalMapOne;

    [Header("Signs")]
    public GameObject ForwardLevelSign;  // + on Z Axis
    public GameObject BackwardLevelSign; // - on Z Axis

    private GameObject LevelSign; //Changes to Correct/Wrong depending on trigger

    [Header("Player")]
    public Transform Player;

    private Vector3 lastSectionSpawnPosition;

    private readonly float defaultBlockSize = 8f;

    private readonly float sectionLength = 78f;
    private readonly float sectionWidth = 48f;
    private readonly float levelSignLength = 20f;

    public List<GameObject> currentSpawnedMapItems;

    [SerializeField] int score = 0;
    
    bool forward = true; // temporary

    void Start()
    {

        triggerManager = FindObjectOfType<TriggerManager>();
        if (triggerManager == null)
            Debug.Log("TRIGGER MANAGER NULL ERROR");

        lastSectionSpawnPosition = new Vector3(0f, 0f, 0f);

        //Base map shown to player
        GameObject startMap = Instantiate(NormalMapOne, lastSectionSpawnPosition, Quaternion.identity, transform);
        (GameObject forwardSign, GameObject backwardSign) = SpawnLevelSigns(NormalMapOne);

        currentSpawnedMapItems.Add(backwardSign);
        currentSpawnedMapItems.Add(startMap);
        currentSpawnedMapItems.Add(forwardSign);

        //SpawnNextSection();
    }

    void Update()
    {

    }

    public void Points(bool correctTrigger)
    {
        Debug.Log("TRIGGER: " + correctTrigger);
        if (correctTrigger)
            score++;
        else
            score = 0;
    }

    void SpawnNextSection()
    {
        GameObject sectionToSpawn = DetermineNextSection();

        Vector3 spawnPos = CalcSectionSpawnPosition();

        Vector3 rot = sectionToSpawn.transform.rotation.eulerAngles;

        Instantiate(sectionToSpawn, spawnPos, Quaternion.Euler(rot), transform);

        lastSectionSpawnPosition = spawnPos;

        (GameObject forwardSign, GameObject backwardSign) = SpawnLevelSigns(sectionToSpawn);

        currentSpawnedMapItems.Add(backwardSign);
        currentSpawnedMapItems.Add(sectionToSpawn);
        currentSpawnedMapItems.Add(forwardSign);
    }

    (GameObject, GameObject) SpawnLevelSigns(GameObject sectionSpawning)
    {
        // + Z Axis
        Vector3 forwardSpawnPos = CalcForwardLevelSignPos();

        // - Z Axis
        Vector3 backwardSpawnPos = CalcBackwardLevelSignPos();

        GameObject forwardSign;
        GameObject backwardSign;

        if (sectionSpawning.name.Contains("NormalSection"))
        {
            forwardSign  = Instantiate(ForwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);
            backwardSign = Instantiate(ForwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);
        }
        else if (sectionSpawning.name.Contains("AnomalySection"))
        {
            forwardSign  = Instantiate(BackwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);
            backwardSign = Instantiate(BackwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);
        }
        else // Spawning start map
        {
            Debug.Log("Start Map Spawned");
            forwardSign  = Instantiate(ForwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);
            backwardSign = Instantiate(BackwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);
        }

        return (forwardSign, backwardSign);
    }
    
    Vector3 CalcSectionSpawnPosition()
    {
        Vector3 spawnPosition = lastSectionSpawnPosition;

        if (forward)
        {
            Vector3 forwardOffset = Vector3.forward * (sectionLength + levelSignLength);

            Vector3 pivotOffset = new(defaultBlockSize, 0f, 0f);

            Vector3 leftOffset = (Vector3.left * sectionWidth) + pivotOffset;

            spawnPosition += leftOffset + forwardOffset;

            return spawnPosition;
        }
        else
        {
            //Vector3 forwardOffset = Vector3.back * sectionLength;

            Vector3 leftOffset = Vector3.right * sectionWidth;

            spawnPosition += leftOffset;

            return spawnPosition;
        }
    }

    //Forwards being + on the Z axis
    Vector3 CalcForwardLevelSignPos()
    {
        Vector3 spawnPosition = lastSectionSpawnPosition;

        Vector3 forwardOffset = Vector3.forward * levelSignLength + new Vector3(0f, 0f, sectionLength);

        Vector3 leftOffset = Vector3.left * (sectionWidth - defaultBlockSize);

        spawnPosition += forwardOffset + leftOffset;

        return spawnPosition;
    }

    //Backwards being - on the Z axis
    Vector3 CalcBackwardLevelSignPos()
    {
        Vector3 spawnPosition = lastSectionSpawnPosition;

        //Vector3 backwardOffset = Vector3.back * levelSignLength;

        //spawnPosition += backwardOffset;

        return spawnPosition;
    }



    GameObject DetermineNextSection()
    {
        GameObject nextSection;

        System.Random rnd = new();

        nextSection = rnd.Next(0, 2) == 1 ? AnomalyMapOne : NormalMapOne;

        return nextSection;
    }

    GameObject DetermineLevelSign()
    {
        GameObject nextSection = null;

        return nextSection;
    }

    float GetLengthOfBlock(string blockName)
    {
        float length = blockName switch
        {
            "AnomalySectionOne" => 78f,
            "NormalSectionOne" => 78f,
            "BackwardLevel" => 20f,
            "ForwardLevel" => 20f,
            _ => 8f,
        };
        return length;
    }

    float GetWidth(string blockName)
    {
        return blockName switch
        {
            "AnomalySectionOne" => 48f,
            "NormalSectionOne"  => 48f,
            _ => 8f,
        };
    }
}
