using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeTrigger : MonoBehaviour {

    // What happens when the pixie is in this node

    public Node ownNode_;
    [HideInInspector]
    public Pixie thePixie_;
    [HideInInspector]
    public UI ui_;

    [Header("Add possible trigger effects here, e.g. win, death, etc")]
    public bool WinTrigger_;

    private bool pixieIsInNode_ = false;

    void Start()
    {
        thePixie_ = FindObjectOfType<Pixie>();
        ui_ = FindObjectOfType<UI>();

    }


    void Update()
    {
        if (ownNode_ == thePixie_.collapsedNode_)
        {
            if (!pixieIsInNode_)
            {
                StartCoroutine(TriggerEffect());
                pixieIsInNode_ = true;
            };
        } else if (pixieIsInNode_)
        {
            pixieIsInNode_ = false;
        }

    }

    IEnumerator TriggerEffect()
    {
        Debug.Log("Entered!");
        if (WinTrigger_)
        {
            ui_.ShowWin();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
