using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour {




    public List<Vector3> NodePos;
    public List<Node> NodeList;
    public Camera MCamera;
    // Use this for initialization
    void Start () {

        NodePos = new List<Vector3>();
        NodeList = new List<Node>();
        MCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 randomer()
    {
        Vector3 vec = new Vector3();
        vec.x = Random.Range(-10f, 10f);
        vec.y = Random.Range(-8f, 8f);
        vec.z = 0f;
        return vec;
    }

    public List<Vector3> GenerateNodePosition (int num, int grouping = 4)
    {

        List<Vector3> PositionList = new List<Vector3>();
        for (int i=0; i < num; i++)
        {
            PositionList.Add(randomer());
        }
        for (int i = 0; i < num; i++)
        {
            
            bool distMin = false;
            int connections = 0;
            for (int j = 0; j < num; j++)
            {
                if (i != j)
                {
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 3f)
                        distMin = true;
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 10f)
                    {                       
                        connections++;
                    }
                }
            }
            if (distMin||connections>grouping||connections<2)
            {
                PositionList.RemoveAt(i);
                PositionList.Add(randomer());
                i -= 1;
            }
        }
        
        return PositionList;
    }

    public int[] StartEnd(List<Vector3> list)
    {
        int[] StartEnd = new int[2];
        for (int i = 0; i < list.Count; i++)
        {
            float MaxDist = Vector3.Distance(list[0], list[1]);
            for (int o = 0; o < list.Count; o++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (Vector3.Distance(list[i], list[j]) > MaxDist)
                    {
                        MaxDist = Vector3.Distance(list[i], list[j]);
                        StartEnd[0] = i;
                        StartEnd[1] = j;
                    }
                }
            }
        }
        return StartEnd;

    }

        public void Spawner()
    {
        GameObject Difficulty = GameObject.Find("Difficulty");
        GameObject Amount = GameObject.Find("NodeAmount");
        GameObject Grouping = GameObject.Find("Grouping");
        Debug.Log("Spawn " + Mathf.RoundToInt(Amount.gameObject.GetComponent<Slider>().value).ToString() + " nodes with " + Mathf.RoundToInt(Grouping.gameObject.GetComponent<Slider>().value).ToString());
        NodePos = GenerateNodePosition(Mathf.RoundToInt(Amount.gameObject.GetComponent<Slider>().value), Mathf.RoundToInt(Grouping.gameObject.GetComponent<Slider>().value));
        Debug.Log("Node position list generated");
        for (int i = 0; i < NodePos.Count; i++)
        {
            GameObject NewNode = GameObject.Instantiate((GameObject)Resources.Load("NodePrefab"), NodePos[i], Quaternion.identity);
            NewNode.gameObject.name = "Node " + i.ToString();
            NodeList.Add(NewNode.gameObject.GetComponent<Node>());
        }
        Debug.Log("Nodes generated");
        Vector3 pos = randomer();
        GameObject NPixie = GameObject.Instantiate((GameObject)Resources.Load("PixiePrefab"), pos, Quaternion.identity);
        Debug.Log("Pixie spawned");
        int[] SE = StartEnd(NodePos);
        GameObject StartNode = GameObject.Find("Node " + SE[0].ToString());
        GameObject EndNode = GameObject.Find("Node " + SE[1].ToString());
        NPixie.gameObject.GetComponent<Pixie>().collapsedNode_ = StartNode.gameObject.GetComponent<Node>();
        EndNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.WIN;
        //Connectorring();
    }

    public void Connectorring()
    {
        int[,] connectionMatrix = new int[NodePos.Count, NodePos.Count];
        for (int i = 0; i < NodePos.Count; i++)
        {
            int connectionLimit = 0;
            int j = 0;
            while ((connectionLimit <= 2) || (j < NodePos.Count))
            {

                if ((Vector3.Distance(NodePos[i], NodePos[j]) < 10f) && (i != j))
                {
                    if (connectionMatrix[i, j] == 1)
                    {
                        connectionLimit++;
                    }
                    else
                    {
                        if (Random.Range(0, 100f) > 50f)
                        {
                            connectionMatrix[i, j] = 1;
                            connectionMatrix[j, i] = 1;
                            connectionLimit++;
                        }
                    }

                }
                j++;
            }

        }
        Debug.Log("Connection matrix finished!");
        for (int i = 0; i < NodePos.Count; i++)
        {
            for (int j = 0; j < NodePos.Count; j++)
            {
                if (connectionMatrix[i,j]==1)
                {
                    NodeList[i].connectedNodes_.Add(NodeList[j]);
                    NodeList[i].ConnectNode(NodeList[j]);
                    Debug.Log("Node interconnected");
                }
            }
        }
    }

}
