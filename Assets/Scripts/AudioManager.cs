using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Tooltip("Click Track - accompanies entire level")]
    public AudioClip clickTrack;
    [Tooltip("Failed note - played when animal is missed")]
    public AudioClip failedClip;
    [Tooltip("Bermuda, Panama, and Mexico")]
    public int BPM;
    [Tooltip("The sheet is an object holding all the phrases")]
    public Transform sheet; //wondering if sheet can be contructed from an array or json or something...
    [Tooltip("how many metres should the sheet move per beat")]
    public float metresPerBeat = 5f; //... that way changing metersPerBeat won't mean having to move all the phrases

    //hi, wasn't expecting you here.
    //these are private because i'll have them automatically set themselves
    [Header("Audio sources")]
    public AudioSource mainSource;
    public AudioSource phraseSource;
    [HideInInspector]
    public AudioClip phraseClip;

    //this stuff is just to know when and if the song is over 
    bool sheetMoving;


    // Start is called before the first frame update
    void Start()
    {
        if (clickTrack != null)
        {
            mainSource.clip = clickTrack;
        }
        else
        {
            Debug.Log("why do you not have a source in place if you're pressing play \nQwQ");
        }

        mainSource.Play();

        sheetMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveSheet();
    }

    void moveSheet()
    {
        if (!sheetMoving){return;}//if the sheet isn't moving then the song is over and we dont need to bother running the code

        sheet.transform.localPosition = sheet.transform.localPosition + transform.right * metresPerBeat * BPM/60 * Time.deltaTime;
    }

    void checkPhrase()
    {

    }
}
