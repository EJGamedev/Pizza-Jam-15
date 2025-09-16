using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    public AudioManager am;
    public float sheetOffset = 0f;

    void Start()
    {
        Vector3 scale = new Vector3(1, 4*am.metresPerBeat, 1);
        Vector3 offset = new Vector3(0, sheetOffset, 0);
        List<Vector3> positions = new List<Vector3>();

        foreach (Transform t in GetComponentsInChildren <Transform> ()) {
                positions.Add(Vector3.Scale(t.position, scale) + offset);
        }
        for (int a = 0; a < GetComponentsInChildren <Transform>().Length;) {
                GetComponentsInChildren <Transform>()[a].position = positions[a++];
        } 
    }
}
