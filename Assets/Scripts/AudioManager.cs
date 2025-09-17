using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Tooltip("Click Track - accompanies entire level")]
    public AudioClip mainTrack;
    [Tooltip("Failed note - played when animal is missed")]
    public AudioClip failedClip;
    [Tooltip("Bermuda, Panama, and Mexico")]
    public int BPM;
    [Tooltip("The sheet is an object holding all the phrases")]
    public Transform sheet;
    [Tooltip("how many metres should the sheet move per beat")]
    public float metresPerBeat = 0.25f;

    //hi, wasn't expecting you here.
    //these are private because i'll have them automatically set themselves
    [Header("Audio sources (do not set)")]
    public AudioSource mainSource;
    public AudioSource phraseSource;
    [HideInInspector]
    public AudioClip phraseClip;

    //this stuff is just to know when and if the song is over 
    bool sheetMoving;

    private int wholeNotes, eighthNotes, sixteenthNotes;
    private int eighthTracker, wholeTracker;
    private bool WNreactivate, ENreactivate, SNreactivate;
    

    // Start is called before the first frame update
    void Start()
    {
        if (mainTrack != null)
        {
            mainSource.clip = mainTrack;
        }
        else
        {
            Debug.LogWarning("line 40: why do you not have a source in place if you're pressing play \nQwQ");
        }

        mainSource.Play();

        sheetMoving = true;

        StartCoroutine(SixteenthIncrement());
    }

    void timeIncrementer()
    {
        Debug.Log(wholeNotes + " " +  eighthNotes + " " + sixteenthNotes);

        StartCoroutine(SixteenthIncrement());
    }


    IEnumerator SixteenthIncrement()
    {
        
        yield return new WaitForSeconds(60 / BPM / 4);

        sixteenthNotes++;

        if (eighthTracker < 2)
        {
            eighthTracker++;
        }
        else
        {
            eighthNotes++;
            eighthTracker = 0;
        }

        if (wholeTracker < 4)
        {
            wholeTracker++;
        }
        else
        {
            wholeNotes++;
            wholeTracker = 0;
        }

        timeIncrementer();
    }

    // Update is called once per frame
    void Update()
    {
        moveSheet();
    }

    void moveSheet()
    {
        if (!sheetMoving){return;}//if the sheet isn't moving then the song is over and we dont need to bother running the code

        sheet.transform.localPosition = sheet.transform.localPosition - transform.up * metresPerBeat * BPM/60 * Time.deltaTime;
    }

    void checkPhrase()
    {

    }
}
