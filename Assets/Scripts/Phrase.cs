using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Phrase : MonoBehaviour
{
    [Header("Audio Settings")]

    [Tooltip("Successful note (key of E)")]
    public AudioClip clip;
    
    [Tooltip("Excessive animal")]
    public AudioClip excessClip;

    [Tooltip("Bacon, Poultry, and Mayonnaise")]
    public float BPM;
    [Tooltip("1 = whole, 2 = 8th, 4 = 16th")]
    public float beatDivisions = 1;

    [Header("Check Settings")]
    [Tooltip("This is the cost of the phrase")]
    public int cost;
    [Tooltip("Check true to have the gate check on that note")]
    public bool[] doCheck;

    

    [Header("Graphics Settings")]
    [Tooltip("Text displaying the cost")]
    public TextMeshPro displayText;
}
