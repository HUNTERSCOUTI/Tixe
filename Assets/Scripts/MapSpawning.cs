using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class MapSpawning : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private GameObject AnomalyMapOne;
    [SerializeField] private GameObject NormalMapOne;
    //[SerializeField] private List<GameObject> AnomalyMapList;
    //[SerializeField] private List<GameObject> NormalMapList;

    private Section anomalyMapOneInstance;
    private Section normalMapOneInstance;
    //private List<Section> anomalyMapOneInstanceList;
    //private List<Section> normalMapOneInstanceList;

    [Header("Signs")]
    [SerializeField] private GameObject ForwardLevelSign;  // + on Z Axis
    [SerializeField] private GameObject BackwardLevelSign; // - on Z Axis

    private GameObject LevelSign; //Changes to Correct/Wrong depending on trigger

    [Header("Player")]
    [SerializeField] private Transform Player;

    private Vector3 lastSectionSpawnPosition;
    private Vector3 currectDirection;

    private readonly float defaultBlockSize = 8f;

    private readonly float sectionLength = 78f;
    private readonly float sectionWidth = 48f;
    private readonly float levelSignLength = 20f;

    public List<Section> currentSpawnedMapItems = new();

    [SerializeField] private float score;
    
    void Start()
    {

        score = -1f;

        lastSectionSpawnPosition = new Vector3(0f, 0f, 0f);

        //For loop to instansiate all prefabs
        anomalyMapOneInstance = InstantiateSection(AnomalyMapOne, true);
        normalMapOneInstance = InstantiateSection(NormalMapOne, false);

        //Base map shown to player
        LoadStartMap();
    }

    void Update()
    {

    }

    public void Points(Triggers triggerReceived)
    {
        Section nextSection = null;
        if (triggerReceived == Triggers.Correct)
        {
            score++;
            nextSection = SpawnNextSection();
            currectDirection = currentSpawnedMapItems[0].AnomalyMap ? Vector3.back : Vector3.forward;
            SpawnLevelSignForward(nextSection.AnomalyMap);
        }
        else if (triggerReceived == Triggers.Wrong)
        {
            score = 0;
            nextSection = SpawnNextSection();
            currectDirection = currentSpawnedMapItems[0].AnomalyMap ? Vector3.forward : Vector3.back;
            SpawnLevelSignBackward(nextSection.AnomalyMap);
        }
        else if (triggerReceived == Triggers.LevelChange)
        {
            DespawnAndReplaceSection();
            currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[0].enabled = false;
            currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[1].enabled = true;
            currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[2].enabled = true;
        }
    }

    Section SpawnNextSection()
    {
        Section sectionToSpawn = DetermineNextSection();

        Vector3 spawnPos = CalcSectionSpawnPosition();

        Vector3 rot = sectionToSpawn.SectionObject.transform.rotation.eulerAngles;

        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[0].enabled = true;
        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[1].enabled = false;
        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[2].enabled = false;

        Instantiate(sectionToSpawn.SectionObject, spawnPos, Quaternion.Euler(rot), transform);

        lastSectionSpawnPosition = spawnPos;

        currentSpawnedMapItems.Add(sectionToSpawn);

        return sectionToSpawn;
    }
    
    GameObject SpawnLevelSignForward(bool isAnomalyMap)
    {
        // + Z Axis
        Vector3 forwardSpawnPos = CalcForwardLevelSignPos();

        GameObject forwardSign;

        if (!isAnomalyMap)
            forwardSign  = Instantiate(ForwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);

        else if (isAnomalyMap)
            forwardSign  = Instantiate(BackwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);

        else // Spawning start map
            forwardSign  = Instantiate(ForwardLevelSign, forwardSpawnPos, Quaternion.identity, transform);

        return forwardSign;
    }
    
    GameObject SpawnLevelSignBackward(bool isAnomalyMap)
    {
        // - Z Axis
        Vector3 backwardSpawnPos = CalcBackwardLevelSignPos();

        GameObject backwardSign;

        if (!isAnomalyMap)
            backwardSign = Instantiate(ForwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);

        else if (isAnomalyMap)
            backwardSign = Instantiate(BackwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);

        else // Spawning start map
            backwardSign = Instantiate(BackwardLevelSign, backwardSpawnPos, Quaternion.identity, transform);

        return backwardSign;
    }
    
    void DespawnAndReplaceSection()
    {
        Destroy(currentSpawnedMapItems[0].WrongLevelArea);
        Destroy(currentSpawnedMapItems[0].SectionObject);
        Destroy(currentSpawnedMapItems[0].CorrectLevelArea);
        SpawnLevelSignBackward(currentSpawnedMapItems[1].AnomalyMap);
        currentSpawnedMapItems.Remove(currentSpawnedMapItems[0]);
    }
    
    Vector3 CalcSectionSpawnPosition()
    {
        Vector3 spawnPosition = lastSectionSpawnPosition;

            Vector3 forwardOffset = Vector3.forward * (sectionLength + levelSignLength);

            Vector3 pivotOffset = new(defaultBlockSize, 0f, 0f);

            Vector3 leftOffset = (Vector3.left * sectionWidth) + pivotOffset;

            spawnPosition += leftOffset + forwardOffset;

            return spawnPosition;
        /* BACKWARDS
            //Vector3 forwardOffset = Vector3.back * sectionLength;

            Vector3 leftOffset = Vector3.right * sectionWidth;

            spawnPosition += leftOffset;

            return spawnPosition;
        */
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

    Section DetermineNextSection()
    {
        GameObject nextSection;

        System.Random rnd = new();
        int result = rnd.Next(0, 2);

        nextSection = result == 0 ? anomalyMapOneInstance.SectionObject : normalMapOneInstance.SectionObject;

        Section sectionInstance = new()
        {
            SectionObject = nextSection,
            AnomalyMap = result == 0
        };

        return sectionInstance;
    }

    GameObject DetermineLevelSign()
    {
        GameObject nextSection = null;

        return nextSection;
    }

    private Section InstantiateSection(GameObject prefab, bool isAnomalyMap)
    {
        Section section = new()
        {
            // Set properties of the Section object
            AnomalyMap = isAnomalyMap,
            SectionObject = prefab
        };

        return section;
    }

    void LoadStartMap()
    {
        GameObject startMap = Instantiate(normalMapOneInstance.SectionObject, lastSectionSpawnPosition, Quaternion.identity, transform);
        Section startSection = InstantiateSection(startMap, false);
        GameObject forwardSign = SpawnLevelSignForward(startSection.AnomalyMap);
        GameObject backwardSign = SpawnLevelSignBackward(startSection.AnomalyMap);

        startSection.CorrectLevelArea = forwardSign;
        startSection.WrongLevelArea = backwardSign;

        currentSpawnedMapItems.Add(startSection);

        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[0].enabled = false;
        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[1].enabled = true;
        currentSpawnedMapItems[0].SectionObject.GetComponentsInChildren<BoxCollider>()[2].enabled = true;
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
