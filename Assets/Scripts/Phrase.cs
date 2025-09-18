using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private List<int> noteByIndex = new List<int>();
    private float metresPerBeat = 1.0f; // Only works for MPB = 1 :(

    [Header("Graphics Settings")]
    [Tooltip("Text displaying the cost")]
    public TextMeshPro displayText;
    public Material holdMaterial;
    public Material noteMaterial;

    private GameObject displayHold;
    private Vector3 holdScale, holdPosition;

    // private GameObject[] displayNotes;


    private void Awake()
    {
        float holdLength = (doCheck.Length / beatDivisions) / metresPerBeat;

        displayText.text = cost.ToString();

        if (holdLength <= 1) { holdLength = 1; }
        else { holdLength += 0.5f; }
        displayHold = GameObject.CreatePrimitive(PrimitiveType.Cube);
        displayHold.transform.parent = this.transform;
        displayHold.GetComponent<Renderer>().material = holdMaterial;
        Debug.Log(holdLength);
        displayHold.transform.localScale = new Vector3(0.75f, holdLength, 1.05f);
        displayHold.transform.position = this.transform.position;
        displayHold.transform.position += new Vector3(0.0f, (holdLength/8.0f) - 0.125f, 0.0f);

        /* THIS BREAKS THE GAME FOR SOME REASON :/

        for (int a = 0; a < doCheck.Length;) {
            if (doCheck[a] == true) {
                noteByIndex.Add(a);
                }
        }
        for (int a = 0; a < noteByIndex.Count;) {
            if (a == 0) { continue; }
            float noteDistance = (noteByIndex[a] / beatDivisions) * metresPerBeat;
            displayNotes[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            displayNotes[a].transform.parent = this.transform;
            displayNotes[a].GetComponent<Renderer>().material = noteMaterial;
            displayHold.transform.localPosition = new Vector3(0.0f, noteDistance, 0.0f);
        } 
        */
    }
}