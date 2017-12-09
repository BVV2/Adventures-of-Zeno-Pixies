using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLine : MonoBehaviour
{

    // Class attached to the box colliders, containing the two connecting nodes
    public Node nodeStart_;
    public Node nodeEnd_;

    public void OnMouseDown()
    {
        Debug.Log("Clicked node: " + transform.root.name);
        nodeStart_.DisconnectNode(nodeEnd_);
        nodeEnd_.DisconnectNode(nodeStart_);
    }

}
