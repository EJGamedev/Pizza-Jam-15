using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Tooltip("Creatures captured in the gate. do not edit")]
    public List<Entity> creatures;
    [Tooltip("drag the camera here when setting up level")]
    public AudioManager audioManager;


    private Phrase currentPhrase;
    private int creatureIndex;
    // Start is called before the first frame update
    void Start()
    {
        if(audioManager == null)
        {
            audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Phrase>() != null)
        {
            currentPhrase = other.GetComponent<Phrase>();
            audioManager.phraseClip = currentPhrase.clip;
            audioManager.phraseSource.Play();
            StartCoroutine(TimeIncrement());
        }
        else if(other.GetComponent<Entity>() != null)
        {
            creatures.Add(other.GetComponent<Entity>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            creatures.Remove(other.GetComponent<Entity>());
        }
    }

    void runPhrase()
    {
        if(creatureIndex < creatures.Count)
        {
            if (currentPhrase.doCheck[creatureIndex])
            {
                if (creatures[creatureIndex].entityName == currentPhrase.creature) 
                {
                    audioManager.phraseSource.mute = false;
                    //add points ig
                    Debug.Log("points");
                }
                else
                {
                    audioManager.phraseSource.mute = true;
                    //dont add points and either mute or switch to failure
                    Debug.Log("deductions");
                }
                creatureIndex++;
            }
            StartCoroutine(TimeIncrement());
        }
        else if(currentPhrase.cost <= 0)
        {
            if (currentPhrase.doCheck[creatureIndex])
            {
                audioManager.phraseSource.mute = false;
                //add points ig
                Debug.Log("points");
                creatureIndex++;
            }
            StartCoroutine(TimeIncrement());

        }
        else if (creatureIndex >= creatures.Count && currentPhrase.cost > 0)
        {
            creatureIndex = 0;
        }

    }

    IEnumerator TimeIncrement()
    {

        if(currentPhrase != null) 
        {
            runPhrase();
            yield return new WaitForSeconds(60 / currentPhrase.BPM / currentPhrase.beatDivisions);
        }
        else
        {

        }

    }
}
