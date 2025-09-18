using TMPro;
using UnityEngine;

public class Phrase : MonoBehaviour
{
    [Header("Audio Settings")]

    [Tooltip("Successful note")]
    public AudioClip clip;
    
    [Tooltip("Excessive creature")]
    public AudioClip excessClip;

    [Tooltip("1 = whole, 2 = eighth, 4 = sixteenth")]
    public float beatDivisions = 1;

    [Header("Check Settings")]
    [Tooltip("This is the creature required")]
    public string creature;
    [Tooltip("This is the number of creatures needed for the phrase")]
    public int cost;
    [Tooltip("Check true to have the gate check on that note")]
    public bool[] doCheck;

    

    [Header("Graphics Settings")]
    [Tooltip("Text displaying the cost")]
    public TextMeshPro displayText;

    //these two variables accommodate the audio manager
    //it stores where in the list it is as well as what beat the phrase was activated on all within the phrase itself
    //its public so the audio manager can access it easily but hideininspector because its not meant to be edited by us at all
    [HideInInspector] public int startBeat;
    [HideInInspector] public AudioSource source;

    private void Awake()
    {
        displayText.text = cost.ToString();
    }
}
