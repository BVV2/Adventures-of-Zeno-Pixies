using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeProbability
{
    // Class containing a node, and the probability that the pixie is there
    public Node node_;
    public float probability_;
}

public class Pixie : MonoBehaviour {

    

    // The node it actually is at
    public Node collapsedNode_;
    // Its own list of connected nodes, going out from the initial collapsedNode_
    public List<Node> connectedNodes_ = new List<Node> { };
    public List<NodeProbability> nodeProbabilities_ = new List<NodeProbability> { };
    // How many levels deep will it find connected nodes?
    private int recursions_ = 3;
	// Use this for initialization
	void Start () {
        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        PopulateNodeList(true);
        StartCoroutine(QuantumMovement());
    }
	
    public void Collapse()
    {
        // Collapses pixie location
        // Get index for random weighted nodeprobabiity
        List<int> weights = new List<int> { };
        foreach (NodeProbability np in nodeProbabilities_)
        {
            weights.Add((int)np.probability_);
        };
        int index = GetRandomWeightedIndex(weights.ToArray());

        Node newNode = nodeProbabilities_[index].node_;
        // Assign node
        collapsedNode_ = newNode;
        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        PopulateNodeList(true);

    }

    public void PopulateNodeList(bool cleanPopulate = false)
    {
        // Populates nodeProbability list based on connectedNodes_
        if (cleanPopulate)
        {
            nodeProbabilities_.Clear();
        };
        foreach (Node node in connectedNodes_)
        {
            if (!ContainsNode(node))
            {
                NodeProbability newNodeProbability = new NodeProbability();
                newNodeProbability.node_ = node;
                newNodeProbability.probability_ = 0f;
                nodeProbabilities_.Add(newNodeProbability);
            };
        }
    }

    public bool ContainsNode(Node node)
    {
        // Returns whether the list of NodeProbabilities already contains the node in question
        foreach (NodeProbability np in nodeProbabilities_)
        {
            if (np.node_ == node)
            {
                return true;
            }
        }
        return false;
    }
    public void ClearNulls()
    {
        List<NodeProbability> removeList = new List<NodeProbability> { };
        // Clears out nodeProbabilities that have been deleted
        foreach (NodeProbability np in nodeProbabilities_)
        {
            if (!connectedNodes_.Contains(np.node_))
            {
                removeList.Add(np);
            }
        }
        nodeProbabilities_.RemoveAll(item=>removeList.Contains(item));
    }

    public IEnumerator QuantumMovement()
    {
        float probability = 1f;
        while (enabled)
        {
            // Update list
            PopulateNodeList();
            // Pick a connected node
            NodeProbability randomNode = nodeProbabilities_[Random.Range(0, nodeProbabilities_.Count)];
            // Increase or decrease its probability
            bool up = (Random.Range(0, 1f) < .5f);
            if (up)
            {
                if (probability > 0.01f)
                {
                    float randomProb = Random.Range(0f, probability);
                    randomNode.probability_ += randomProb;
                    probability -= randomProb;
                }
            }
            else
            {
                if (randomNode.probability_ > 0)
                {
                    float randomProb = Random.Range(0f, randomNode.probability_);
                    randomNode.probability_ -= randomProb;
                    probability += randomProb;
                }
            }
            randomNode.node_.transform.localScale = new Vector3(randomNode.probability_, randomNode.probability_, randomNode.probability_);
            yield return new WaitForSeconds(.1f);

        }
    }

    public int GetRandomWeightedIndex(int[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        int t;
        int i;
        int w = 0;
        for (i = 0; i < weights.Length; i++)
        {
            if (weights[i] >= 0) w += weights[i];
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            if (weights[i] <= 0f) continue;

            s += (float)weights[i] / weights.Length;
            if (s >= r) return i;
        }

        return -1;
    }

    // Update is called once per frame
    void Update () {

        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        ClearNulls();
        

    }
}
