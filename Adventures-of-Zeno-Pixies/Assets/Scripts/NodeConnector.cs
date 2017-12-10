using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnector : MonoBehaviour {

    // Attached to the node graphic, lets you connect two nodes together

    public Node parentNode_;
    public LineRenderer mouseRenderer_;
    protected bool isMouseDown_ = false;
    [HideInInspector]
    protected Pixie thePixie_;

    private float maxLineLength_ = 10f;

	// Use this for initialization
	void Start () {
        thePixie_ = FindObjectOfType<Pixie>();
        if (thePixie_ == null)
        {
            Debug.LogError("No pixie found! Must add pixie.");
        };
	}

    void OnMouseEnter()
    {
        if (thePixie_.collapsedNode_ == parentNode_ && UI.isObserving_)
        {
            MouseCursorGlitter.glitterColor_ = Color.blue;
            MouseCursorGlitter.glitterSpeed_ = MouseCursorGlitter.glitterSpeed_ * 2;
        };
    }
    void OnMouseExit()
    {
        MouseCursorGlitter.glitterColor_ = MouseCursorGlitter.defaultColor_;
        MouseCursorGlitter.glitterSpeed_ = MouseCursorGlitter.defaultSpeed_;
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse down");
        // Only allow this if the pixie is attached to the parentNode, and if we're observing
        if (UI.isObserving_)
        {
            if (thePixie_.collapsedNode_ == parentNode_)
            {
                mouseRenderer_.positionCount = 2;
                isMouseDown_ = true;
            };
        };
    }
    public void OnMouseUp()
    {
        Debug.Log("Mouse up");
        if (UI.isObserving_)
        {
            if (thePixie_.collapsedNode_ == parentNode_)
            {
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
                        if (collidedNode != null && collidedNode != this)
                        {
                            parentNode_.ConnectNode(collidedNode.parentNode_);
                            break;
                        };
                    }
                }
                else {
                    Debug.Log("no hit!");
                }
                // Remove line
                mouseRenderer_.positionCount = 0;
            };
        };
            
                


    }
	
	// Update is called once per frame
	void Update () {
        if (isMouseDown_) {
            mouseRenderer_.SetPosition(0, transform.position);
            // Only allow lines of a certain length
            if (Vector2.Distance(mouseRenderer_.GetPosition(0), Camera.main.ScreenToWorldPoint(Input.mousePosition)) < maxLineLength_)
            mouseRenderer_.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
             }
        else
        {
            // Clever maths here to keep following the cursor without actually getting longer
        }
    }
}
