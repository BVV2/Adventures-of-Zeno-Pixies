using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour {

    public List<Node> connectedNodes_;
    public LineRenderer lineRenderer_normal;
    public GameObject nodeGraphic_;

	// Use this for initialization
	void Start () {

        ClearNulls();
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
    public void ClearNulls()
    {
        connectedNodes_.RemoveAll(item => item == null);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void AddLineRenderers()
    {
        // Destroy old colliders
        foreach (BoxCollider2D collider in GetComponentsInChildren<BoxCollider2D>(true))
        {
            Destroy(collider.gameObject);
        };
        int i = 0;
        lineRenderer_normal.positionCount = connectedNodes_.Count * 2;
        foreach (Node node in connectedNodes_)
        {
            //node.ConnectNode(this);
            lineRenderer_normal.SetPosition(i, node.transform.position);
            lineRenderer_normal.SetPosition(i + 1, transform.position);
            i += 2;
            // Add colliders
            BoxCollider2D newCollider = AddColliderToLine(lineRenderer_normal, node.transform.position, transform.position);
            NodeLine newNodeLine = newCollider.gameObject.AddComponent<NodeLine>();
            newNodeLine.nodeStart_ = this;
            newNodeLine.nodeEnd_ = node;
        }
    }

    public void ConnectNode(Node node)
    {
        if (!connectedNodes_.Contains(node))
        {
            connectedNodes_.Add(node);
            AddLineRenderers();
        };
    }
    public void DisconnectNode(Node node)
    {
        if (connectedNodes_.Contains(node))
        {
            connectedNodes_.Remove(node);
            AddLineRenderers();
        };
    }
    private BoxCollider2D AddColliderToLine(LineRenderer line, Vector2 startPoint, Vector2 endPoint)
    {
        //create the collider for the line
        BoxCollider2D lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider2D>();
        //set the collider as a child of your line
        lineCollider.transform.parent = line.transform;
        // get width of collider from line, but make it a bit bigger 
        float lineWidth = line.endWidth*3.5f;
        // get the length of the line using the Distance method
        float lineLength = Vector2.Distance(startPoint, endPoint);
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineCollider.transform.position = midPoint;


        //heres the beef of the function, Mathf.Atan2 wants the slope, be careful however because it wants it in a weird form
        //it will divide for you so just plug in your (y2-y1),(x2,x1)
        float angle = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));
        //Vector3 targetDir = startPoint - endPoint;
        //float angle = Vector3.Angle(targetDir, transform.forward);

        // angle now holds our answer but it's in radians, we want degrees
        // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
        angle *= Mathf.Rad2Deg;

        //were interested in the inverse so multiply by -1
        //angle *= -1;
        // now apply the rotation to the collider's transform, carful where you put the angle variable
        // in 3d space you don't wan't to rotate on your y axis
        lineCollider.transform.Rotate(0, 0, angle);
        // Returns the created linecollider
        return lineCollider;
    }
}
