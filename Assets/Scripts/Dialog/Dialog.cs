using UnityEngine;

public class Dialog : MonoBehaviour
{
    public string characterName = "";
    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    public bool playOnStart = false;

    private void Start()
    {
        if(playOnStart == true)
        {
            PlayDialog();
        }
    }

    // Use this for initialization
    void PlayDialog()
    {
        if (scriptToLoad != null)
        {
            var dialogRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogRunner.AddScript(scriptToLoad);
            dialogRunner.StartDialogue(talkToNode);
        }
    }
}
