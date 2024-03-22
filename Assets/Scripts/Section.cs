using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section
{
    public GameObject SectionObject { get; set; }

    public GameObject CorrectLevelArea { get; set; }
    public GameObject WrongLevelArea { get; set; }

    public bool AnomalyMap { get; set; }
}
