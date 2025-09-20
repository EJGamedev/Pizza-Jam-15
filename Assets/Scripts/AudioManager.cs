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
    public float BPM;
    [Tooltip("The sheet is an object holding all the phrases")]
    public Transform sheet;
    [Tooltip("how many metres should the sheet move per beat")]
    public float metresPerBeat = 0.25f;
    [Tooltip("this object checks for phase collisions")]
    public Gate gate;

    [Header("Audio source")]
    public AudioSource mainSource;
    [HideInInspector]
    public List<Phrase> phrases;

    //this stuff is just to know when and if the song is over 
    bool sheetMoving;

    [HideInInspector]
    public int wholeNotes, eighthNotes, sixteenthNotes;
    private int eighthTracker, wholeTracker;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        if(mainSource == null)
        {
            mainSource = transform.AddComponent<AudioSource>();
            mainSource.loop = false;//loop disabled because level should end after the song is over instead of restarting
            //Herbie, if theres any other settings you want set here go ahead. but as long as you set it in the editor it will stay that way
        }

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

        timer = 60 / BPM / 4;
        Debug.Log(timer);
        StartCoroutine(SixteenthTimer());
    }

    void timeIncrementer()
    {
        sixteenthNotes++;

        if (eighthTracker < 2)
        {
            eighthTracker++;
        }
        else
        {
            eighthNotes++;
            eighthTracker = 1;
        }

        if (wholeTracker < 4)
        {
            wholeTracker++;
        }
        else
        {
            wholeNotes++;
            wholeTracker = 1;
        }

        Debug.Log(wholeNotes + " 8th:" +  eighthNotes + "  16th" + sixteenthNotes);

        StartCoroutine(SixteenthTimer());
    }


    IEnumerator SixteenthTimer()
    {

        yield return new WaitForSeconds(timer);


        timeIncrementer();
    }

    public void checkPhrases()
    {
        foreach(Phrase p in phrases)
        {
            if(p.beatDivisions == 1)
            {

            }
            else if(p.beatDivisions == 2)
            {//hi its EJ here. ill finish this code tomorrow cos its 3 in the morning and i did most of the architecture and now i just need to do the logic and im tired lol ok gn good luck

            }
            else if (p.beatDivisions == 4)
            {

            }
        }
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

    public void addPhrase(Phrase newPhrase)
    {
        phrases.Add(newPhrase);

        if(newPhrase.beatDivisions == 1)
        {
            newPhrase.startBeat = wholeNotes;
        }
        else if (newPhrase.beatDivisions == 2)
        {
            newPhrase.startBeat = eighthNotes;
        }
        else if (newPhrase.beatDivisions == 4)
        {
            newPhrase.startBeat = sixteenthNotes;
        }
        else
        {
            Debug.LogWarning("Hey, your phrase has an incompatible beat division");
        }

        if (newPhrase.source == null )
        {
            newPhrase.source = transform.AddComponent<AudioSource>();
        }

        newPhrase.source.loop = false;
        newPhrase.source.clip = newPhrase.clip;
        newPhrase.source.Play();
    }

    public void removePhrase(Phrase oldPhrase)
    {
        phrases.Remove(oldPhrase);
        Destroy(oldPhrase.source);
    }
}
