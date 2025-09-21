using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioSource failedSource;
    [HideInInspector]
    public List<Phrase> phrases = new List<Phrase>();

    //this stuff is just to know when and if the song is over 
    bool sheetMoving;

    [HideInInspector]
    public int wholeNotes, eighthNotes, sixteenthNotes;
    private int eighthTracker, wholeTracker;

    private float timer;

    private List<Entity> entities = new List<Entity>();
    private float m1;

    // Start is called before the first frame update
    void Start()
    {
        if(mainSource == null)
        {
            mainSource = transform.AddComponent<AudioSource>();
            mainSource.loop = false;//loop disabled because level should end after the song is over instead of restarting
            mainSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            //Herbie, if theres any other settings you want set here go ahead. but as long as you set it in the editor it will stay that way
        }

        if(failedSource == null)
        {
            failedSource = transform.AddComponent<AudioSource>();
            failedSource.loop = false;
            failedSource.clip = failedClip;
            failedSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
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

        //Debug.Log(wholeNotes + " 8th:" +  eighthNotes + "  16th" + sixteenthNotes);

        StartCoroutine(SixteenthTimer());
        checkPhrases();
    }


    IEnumerator SixteenthTimer()
    {

        yield return new WaitForSeconds(timer);


        timeIncrementer();
    }

    private List<Phrase> blacklistedPhrases = new List<Phrase>();
    public void checkPhrases()
    {

        foreach(Phrase p in phrases)//loop each phrase thats been detected
        {
            bool success = false;

            

            if (p.beatDivisions == 1)//check if the phrase we're currently on is a whole note phrase
            {
                if (p.index < wholeNotes - p.startBeat && p.doCheck.Length > p.index)//this is to check and update the index of the phrase to prepare for the next loop
                {
                    foreach (Entity e in entities)//loop all selected entities
                    {
                        if (e.entityName == p.creature && p.doCheck[p.index])//if the entity matches the requirement AND its supposed to be checked now
                        {
                            p.source.mute = false;
                            success = true;
                            removeEntity(e);
                            //points here if we're doing that
                            break;
                        }
                        else if (!p.doCheck[p.index])//this is so that if its not meant to be checked then it wont fail itself
                        {
                            success = true;
                            break;//if doCheck is false then it will only do one check and then stop instead of looping all entities
                                  // it also won't remove any entities from the list
                        }


                    }

                    p.index++;
                    if (!success)//if while looping entities no match was found then it should count as a failure and will play the fail sfx
                    {
                        p.source.mute = true;
                        failedSource.Play();
                        //point deduction or failstate buildup if we're doing that
                    }
                }
                else if(p.index >= p.doCheck.Length)
                {
                    //phrases.Remove(p);
                    blacklistedPhrases.Add(p);
                }
            }
            else if(p.beatDivisions == 2)//check if the phrase we're currently on is an eighth note phrase
            {
                if (p.index < eighthNotes - p.startBeat && p.doCheck.Length > p.index)//this is to check and update the index of the phrase to prepare for the next loop
                {
                    foreach (Entity e in entities)//loop all selected entities
                    {
                        if (e.entityName == p.creature && p.doCheck[p.index])//if the entity matches the requirement AND its supposed to be checked now
                        {
                            p.source.mute = false;
                            success = true;
                            removeEntity(e);
                            //points here if we're doing that
                            break;
                        }
                        else if (!p.doCheck[p.index])//this is so that if its not meant to be checked then it wont fail itself
                        {
                            success = true;
                            break;//if doCheck is false then it will only do one check and then stop instead of looping all entities
                                  // it also won't remove any entities from the list
                        }


                    }

                    p.index++;
                    if (!success)//if while looping entities no match was found then it should count as a failure and will play the fail sfx
                    {
                        p.source.mute = false;
                        p.source.mute = true;
                        failedSource.Play();
                        //point deduction or failstate buildup if we're doing that
                    }
                }
                else if (p.index >= p.doCheck.Length)
                {
                    //phrases.Remove(p);
                    blacklistedPhrases.Add(p);
                }
            }
            else if (p.beatDivisions == 4)//check if the phrase we're currently on is a sixteenth note phrase
            {
                if (p.index < sixteenthNotes - p.startBeat && p.doCheck.Length > p.index)//this is to check and update the index of the phrase to prepare for the next loop
                {
                    foreach (Entity e in entities)//loop all selected entities
                    {
                        if (e.entityName == p.creature && p.doCheck[p.index])//if the entity matches the requirement AND its supposed to be checked now
                        {
                            p.source.mute = false;
                            success = true;
                            removeEntity(e);
                            //points here if we're doing that
                            break;
                        }
                        else if (!p.doCheck[p.index])//this is so that if its not meant to be checked then it wont fail itself
                        {
                            success = true;
                            break;//if doCheck is false then it will only do one check and then stop instead of looping all entities
                                  // it also won't remove any entities from the list
                        }


                    }

                    p.index++;
                    if (!success)//if while looping entities no match was found then it should count as a failure and will play the fail sfx
                    {
                        p.source.mute = true;
                        failedSource.Play();
                        //point deduction or failstate buildup if we're doing that
                    }
                }
                else if (p.index >= p.doCheck.Length)
                {
                    //phrases.Remove(p);
                    blacklistedPhrases.Add(p);
                }
            }
        }

        if(blacklistedPhrases.Any())
        {
            foreach (Phrase p in blacklistedPhrases)
            {
                removePhrase(p);
            }
            blacklistedPhrases.Clear();
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        m1 = Input.GetAxis("Fire1");

        moveSheet();

        input();
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
        newPhrase.source.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void removePhrase(Phrase oldPhrase)
    {
        phrases.Remove(oldPhrase);
        Destroy(oldPhrase.source);
    }

    bool clicked;
    public void input()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Construct a ray from the current mouse coordinates

        if(Input.GetAxis("Cancel") > 0) { SceneManager.LoadScene("Menu"); }

        if (m1 > 0 && !clicked)
        {
            clicked = true;
            RaycastHit hit;
            //Ray ray = cam.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out hit))
            {
                Entity target = hit.collider.gameObject.GetComponent<Entity>();

                if (target != null)
                {
                    bool hasBeenAdded = false;
                    foreach (Entity e in entities)
                    {
                        if (e == target)
                        {
                            hasBeenAdded = true;
                        }
                    }

                    if (!hasBeenAdded)
                    {

                        addEntity(target);
                    }
                    else
                    {
                        removeEntity(target);
                    }
                }
            }
        }

        if(m1 <= 0)
        {
            clicked=false;
        }
    }

    public void addEntity( Entity target )
    {
        entities.Add(target);
        target.selected = true;
        target.selectionBox.SetActive(true);
        Debug.Log(target.entityName);
    }

    public void removeEntity( Entity target )
    {

        target.selected = false;
        target.selectionBox.SetActive(false);
        entities.Remove(target);
        Debug.Log("removed " + target.entityName);
    }
}
