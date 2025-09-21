using UnityEngine;

public class Entity : MonoBehaviour
{
    

    public string entityName;

    public bool selected;
    public GameObject selectionBox;

    //public Vector3[] nodes;

    private void Awake()
    {
        selectionBox.SetActive(false);
    }
}
