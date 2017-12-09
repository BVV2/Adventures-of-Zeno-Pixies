using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLine : MonoBehaviour
{

    // Class attached to the box colliders, containing the two connecting nodes
    public Node nodeStart_;
    public Node nodeEnd_;

    private Pixie thePixie_;

    void Start()
    {
        thePixie_ = FindObjectOfType<Pixie>();
        if (thePixie_ == null)
        {
            Debug.LogWarning("No pixie found! Aaaah!");
        };
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked node: " + transform.root.name);
        float manaCost = 10f;
       

        if (UI.isObserving_)
        {
            manaCost = 0f;
        };
        if (UI.manaReserve_ > manaCost)
        {
            // only allow deletion if the nodes are a part of the pixie's network!
            if (thePixie_.connectedNodes_.Contains(nodeStart_) || thePixie_.connectedNodes_.Contains(nodeEnd_))
            {
                nodeStart_.DisconnectNode(nodeEnd_);
                nodeEnd_.DisconnectNode(nodeStart_);
                UI.ReduceMana(manaCost);
            };
        };
    }

}
