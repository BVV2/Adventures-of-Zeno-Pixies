using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class TriggerAudio {
    public string name_;
    public NodeTypes type_;
    public AudioClip triggerSound_;
}

public class NodeTrigger : MonoBehaviour {

    // What happens when the pixie is in this node

    public Node ownNode_;
    [HideInInspector]
    public Pixie thePixie_;
    [HideInInspector]
    public UI ui_;

    [Header("Add possible trigger effects here, e.g. win, death, etc")]
    public NodeTypes type_ = NodeTypes.NORMAL;

    public AudioSource audiosource_;
    public TriggerAudio[] triggerAudio_;


    private bool pixieIsInNode_ = false;

    void Start()
    {
        thePixie_ = FindObjectOfType<Pixie>();
        ui_ = FindObjectOfType<UI>();
        ChangeGraphic();

    }

    public AudioClip GetSound(NodeTypes type) {

        foreach (TriggerAudio audio in triggerAudio_) {
            if (audio.type_ == type) {
                return audio.triggerSound_;
            }
        }
        return null;

    }

    public void ChangeGraphic()
    {
        // Changes icon to match the trigger type, if any
        SpriteRenderer actualGraphic = ownNode_.nodeGraphic_.GetComponentInChildren<SpriteRenderer>();
        Debug.Log("Actual graphic: " + actualGraphic);
        Sprite typeSprite = ui_.GetNodeTypeSprite(type_);
        Debug.Log("TypeSprite: " + typeSprite);
        actualGraphic.sprite = typeSprite;
        // Change color if it's an objective star.
        if (type_ == NodeTypes.OBJECTIVE)
        {
            actualGraphic.color = Color.yellow;
        }
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
        Debug.Log("Trigger!!");
        switch (type_)
        {
            case NodeTypes.WIN:
                {
                    if (UI.objectives_ == 0)
                    {
                        ui_.ShowWin();
                        
                    };
                    break;
                };
            case NodeTypes.OBJECTIVE:
                {
                    UI.CompleteObjective(1);
                    type_ = NodeTypes.NORMAL;
                    ChangeGraphic();
                    break;
                };
            case NodeTypes.HEALTH_DOWN:
                {
                    UI.ReduceHealth(15f);
                    break;
                };
            case NodeTypes.HEALTH_UP:
                {
                    UI.AddHealth(15f);
                    break;
                };
            case NodeTypes.MANA_DOWN:
                {
                    UI.ReduceMana(15f);
                    break;
                };
            case NodeTypes.MANA_UP:
                {
                    UI.AddMana(15f);
                    break;
                };
            case NodeTypes.NORMAL:
                {
                    break;
                };
        }
        audiosource_.PlayOneShot(GetSound(type_));

                yield return new WaitForEndOfFrame();
    }
}
