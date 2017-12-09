using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour {

    public List<Node> connectedNodes_;
    public LineRenderer lineRenderer_normal;

	// Use this for initialization
	void Start () {

        foreach (Node node in connectedNodes_)
        {
            node.ConnectNode(this);
        };
		
	}

    public List<Node> ReturnAllSubNodes(int recursions)
    {
        List<Node> returnList = new List<Node> {this };
        // Returns all connected nodes; recursions are necessary to prevent stack overflow
        if (recursions > 0)
        {
            recursions -= 1;
            for (int i = 0; i < connectedNodes_.Count; i++)
            {
                returnList.AddRange(connectedNodes_[i].ReturnAllSubNodes(recursions));
            }
        };
        // Remove duplicates (Linq)
        returnList = returnList.Distinct().ToList<Node>();

        return returnList;
    }
	
	// Update is called once per frame
	void Update () {

        int i = 0;
        lineRenderer_normal.positionCount = connectedNodes_.Count*2;
        foreach (Node node in connectedNodes_)
        {
            //node.ConnectNode(this);
            lineRenderer_normal.SetPosition(i, node.transform.position);
            lineRenderer_normal.SetPosition(i + 1, transform.position);
            i += 2;

        }


	}

    public void ConnectNode(Node node)
    {
        if (!connectedNodes_.Contains(node))
        {
            connectedNodes_.Add(node);
        };
    }
    public void DisconnectNode(Node node)
    {
        if (connectedNodes_.Contains(node))
        {
            connectedNodes_.Remove(node);
        };
    }
}
