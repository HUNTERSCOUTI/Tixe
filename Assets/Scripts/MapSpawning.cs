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
    public GameObject CorrectLevelSign;
    public GameObject WrongLevelSign;

    private GameObject LevelSign; //Changes to Correct/Wrong depending on trigger

    [Header("Player")]
    public Transform Player;

    private Vector3 lastSpawnPosition;
    private float previousBlockLength = 0f;
    private float previousBlockOffset = 0f;
    private float previousBlockWidth = 0;

    bool forward = true; // temporary

    void Start()
    {
        triggerManager = FindObjectOfType<TriggerManager>();
        if (triggerManager == null)
            Debug.Log("TRIGGER MANAGER NULL ERROR");

        lastSpawnPosition = new Vector3(0f, 0f, 0f);
        SpawnNextSection();
    }

    void Update()
    {

    }

    void SpawnNextSection()
    {
        GameObject sectionToSpawn = DetermineNextSection();

        (float blockWidth, float blockOffset) = GetWidthAndOffsetX(sectionToSpawn.name);
        float blockLength = GetLengthOfBlock(sectionToSpawn.name);

        Vector3 spawnPos = CalculateSpawnPosition(blockWidth);

        Vector3 rot = sectionToSpawn.transform.rotation.eulerAngles;
        //if (!forward)
            //rot = new Vector3(rot.x, rot.y + 180, rot.z);

        Instantiate(sectionToSpawn, spawnPos, Quaternion.Euler(rot), transform);

        lastSpawnPosition = spawnPos;

        previousBlockOffset = blockOffset;
        previousBlockLength = blockLength;
        previousBlockWidth = blockWidth;
        forward = !forward;
    }

    void SpawnLevelSignBehind()
    {
        GameObject levelSignToSpawn = DetermineLevelSign();
    }
    
    Vector3 CalculateSpawnPosition(float blockWidth)
    {
        Vector3 spawnPosition = lastSpawnPosition;

        if (forward)
        {
            Vector3 forwardOffset = Vector3.forward * previousBlockLength;

            Vector3 leftOffset = Vector3.left * previousBlockOffset;

            spawnPosition += (previousBlockWidth > blockWidth ? leftOffset : Vector3.zero) + forwardOffset;

            return spawnPosition;
        } else
        {
            //Vector3 forwardOffset = Vector3.back * previousBlockLength;

            Vector3 blockOffsetRight = new(8f, 0f, 0f);

            Vector3 leftOffset = (Vector3.right * previousBlockOffset) + blockOffsetRight;

            spawnPosition += (previousBlockWidth > blockWidth ? leftOffset : Vector3.zero);

            return spawnPosition;
        }
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

    /*
    GameObject DetermineNextBlock()
    {
        GameObject nextBlock = null;

        switch (sequenceCounter)
        {
            case 0:
                nextBlock = PlayRoom;
                break;
            case 1:
                nextBlock = TriggerArea;
                break;
            case 2:
                LevelSign = forward ? CorrectLevelSign : WrongLevelSign;
                nextBlock = LevelSign;
                forward = !forward;
                break;
            default:
                nextBlock = TriggerArea;
                sequenceCounter = -1;
                break;
        }

        sequenceCounter++;
        return nextBlock;
    }
    */
    float GetLengthOfBlock(string blockName)
    {
        float length = blockName switch
        {
            "AnamalySectionOne" => 98f,
            "NormalSectionOne" => 98f,
            "BackwardLevel" => 20f,
            "ForwardLevel" => 20f,
            _ => 8f,
        };
        return length;
    }

    (float, float) GetWidthAndOffsetX(string blockName)
    {
        return blockName switch
        {
            //             Width, Pivot Offset
            "AnamalySectionOne" => (48f, 40f),
            "NormalSectionOne"  => (48f, 40f),
            _ => (8f, 0f),
        };
    }
}
