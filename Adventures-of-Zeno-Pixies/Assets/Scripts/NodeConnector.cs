using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnector : MonoBehaviour {

    // Attached to the node graphic, lets you connect two nodes together

    public Node parentNode_;
    public LineRenderer mouseRenderer_;
    protected bool isMouseDown_ = false;

	// Use this for initialization
	void Start () {
		
	}

    public void OnMouseDown()
    {
        Debug.Log("Mouse down");
        mouseRenderer_.positionCount = 2;
        isMouseDown_ = true;
    }
    public void OnMouseUp()
    {
        Debug.Log("Mouse up");
        isMouseDown_ = false;
        // Check if there are any nodes nearby
        RaycastHit2D hit;
        //Ray ray = Camera.main.ScreenPointToRay(mouseRenderer_.GetPosition(1));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseRenderer_.GetPosition(1), 1f);
        if (colliders.Length > 0)
        {
            Debug.Log("hit!");
            foreach (Collider2D coll in colliders)
            {
                Debug.Log("Hit: " + coll.transform.name);
                // We only want to hit the actual graphic, not the linerenderers; the graphic has the nodeconnector script, so we can look for that
                NodeConnector collidedNode = coll.transform.GetComponent<NodeConnector>();
                if (collidedNode != null)
                {
                    parentNode_.ConnectNode(collidedNode.parentNode_);
                };
            }
        }
        else {
            Debug.Log("no hit!");
        }
        // Remove line
        mouseRenderer_.positionCount = 0;
            
                


    }
	
	// Update is called once per frame
	void Update () {
        if (isMouseDown_) {
            mouseRenderer_.SetPosition(0, transform.position);
            mouseRenderer_.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
             }
    }
}
