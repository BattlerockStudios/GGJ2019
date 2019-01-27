using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public string characterName = "";
    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    // Use this for initialization
    void Start()
    {
        if (scriptToLoad != null)
        {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().AddScript(scriptToLoad);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
