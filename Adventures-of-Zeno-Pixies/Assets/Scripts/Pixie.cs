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
    // Set to false when you want the randomness to stop
    protected bool notObserving_ = true;

    // Init
    void Start () {
        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        PopulateNodeList(true);
        // Set the returned node to be 100 probability
        NodeProbability np = ReturnNP(collapsedNode_);
        np.probability_ = 1f;
        StartCoroutine(QuantumMovement());
        StartCoroutine(VisualizeProbabilityChange());
    }
	
    public void Collapse()
    {
        // Collapses pixie location
        // Get index for random weighted nodeprobabiity
        notObserving_ = false;
        List<int> weights = new List<int> { };
        foreach (NodeProbability np in nodeProbabilities_)
        {
            int weight = (int)(np.probability_ * 100f);
            weights.Add(weight);
            Debug.Log(weight);
        };
        int index = GetRandomWeightedIndex(weights.ToArray());
        Debug.Log("Index: " + index.ToString());
        Node newNode = nodeProbabilities_[index].node_;
        // Assign node
        collapsedNode_ = newNode;
        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        PopulateNodeList(true);
        // Set the returned node to be 100 probability
        NodeProbability newNP = ReturnNP(collapsedNode_);
        newNP.probability_ = 1f;
        

    }
    public void StopObserving()
    {
        // Restart quantum movement
        notObserving_ = true;
        StartCoroutine(QuantumMovement());
    }

    public IEnumerator VisualizeProbabilityChange()
    {
        // Constantly visualizes the changes in probability
        while (true)
        {
            for (int i = 0; i < nodeProbabilities_.Count; i++)
            {
                NodeProbability np = nodeProbabilities_[i];
                LeanTween.scale(np.node_.gameObject, new Vector3(np.probability_, np.probability_, np.probability_), .1f);
            }
            yield return new WaitForSeconds(0.15f);
        }
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
    public NodeProbability ReturnNP(Node node)
    {
        // Returns the node probability containing the node in question, or null if not found

        foreach (NodeProbability np in nodeProbabilities_)
        {
            if (np.node_ == node)
            {
                return np;
            }
        }
        return null;
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
        while (notObserving_)
        {
            // Update list
            PopulateNodeList();
            // Pick a connected node
            NodeProbability randomNode = nodeProbabilities_[Random.Range(0, nodeProbabilities_.Count)];
            // Increase or decrease its probability
            bool up = (Random.Range(0, 1f) < .5f);
            if (up)
            {
                float randomProb = Random.Range(0f, probability);
                if ((probability - randomProb) >= 0f)
                {
                    float newNodeProbability = Mathf.Clamp(randomNode.probability_ + randomProb, 0f, 1f);
                    float newTotalProbability = Mathf.Clamp(probability - randomProb, 0f, 1f);
                    randomNode.probability_ = newNodeProbability;
                    probability = newTotalProbability;
                }
            }
            else
            {
                float randomProb = Random.Range(0f, randomNode.probability_);
                if ((probability+randomProb)<= 1f)
                {
                    float newNodeProbability = Mathf.Clamp(randomNode.probability_ - randomProb, 0f, 1f);
                    float newTotalProbability = Mathf.Clamp(probability + randomProb, 0f, 1f);
                    randomNode.probability_ = newNodeProbability;
                    probability = newTotalProbability;
                }
            }
            
            yield return new WaitForSeconds(.1f);

        }
    }

    public int GetRandomWeightedIndex(int[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;
        int weightTotal = 0;
        foreach (int w in weights)
        {
            weightTotal += w;
        }
        int result = 0, total = 0;
        int randVal = Random.Range(0, weightTotal + 1);
        for (result = 0; result < weights.Length; result++)
        {
            total += weights[result];
            if (total >= randVal) break;
        }
        return result;
    }

    // Update is called once per frame
    void Update () {

        // Update connected nodes, and clear any that are null (i.e. connections severed)
        connectedNodes_ = collapsedNode_.ReturnAllSubNodes(recursions_);
        ClearNulls();
        // Move pixie to current collapsednode
        transform.position = collapsedNode_.transform.position;
        

    }
}
