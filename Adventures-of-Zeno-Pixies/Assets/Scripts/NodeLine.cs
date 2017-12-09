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
        // only allow deletion if the pixie is at the end or start of the line
        if (thePixie_.collapsedNode_ == nodeStart_ || thePixie_.collapsedNode_ == nodeEnd_)
        {
            nodeStart_.DisconnectNode(nodeEnd_);
            nodeEnd_.DisconnectNode(nodeStart_);
        };
    }

}
