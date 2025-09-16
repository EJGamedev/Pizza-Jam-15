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
    public float metresPerBeat = 5f;

    //hi, wasn't expecting you here.
    //these are private because i'll have them automatically set themselves
    private AudioSource mainSource;
    private AudioSource phraseSource;
    private AudioClip phraseClip;

    //this stuff is just to know when and if the song is over 
    bool sheetMoving;


    // Start is called before the first frame update
    void Start()
    {
        if(mainSource == null && gameObject.GetComponent<AudioSource>() == null)
        {
            mainSource = gameObject.AddComponent<AudioSource>();

            if (mainTrack != null)
            {
                mainSource.clip = mainTrack;
            }
            else
            {
                Debug.Log("why do you not have a source in place if you're pressing play \nQwQ");
            }
        }
        else if (gameObject.GetComponent<AudioSource>())
        {
            mainSource = gameObject.GetComponent<AudioSource>();
            if (mainTrack != null)
            {
                mainSource.clip = mainTrack;
            }
            else
            {
                Debug.Log("why do you not have a source in place if you're pressing play \nQwQ");
            }
        }
        else
        {
            if(mainTrack != null) 
            {
                mainSource.clip = mainTrack; 
            }
            else
            {
                Debug.Log("why do you not have a source in place if you're pressing play \nQwQ");
            }
            
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
}
