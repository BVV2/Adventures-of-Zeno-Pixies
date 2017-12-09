using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 randomer()
    {
        Vector3 vec = new Vector3();
        vec.x = Random.Range(0, Screen.width);
        vec.y = Random.Range(0, Screen.height);
        vec.z = 0f;
        return vec;
    }

    public List<Vector3> GenerateNodePosition (int num, int grouping = 4)
    {
        
        List <Vector3> PositionList = new List<Vector3>();
        for (int i=0; i < num; i++)
        {
            PositionList.Add(randomer());
        }
        for (int i = 0; i < num; i++)
        {
            bool distMax = true;
            bool distMin = false;
            int connections = 0;
            for (int j = 0; j < num; j++)
            {
                if (i != j)
                {
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 1f)
                        distMin = true;
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 10f)
                    {
                        distMax = false;
                        connections++;
                    }
                }
            }
            if (distMax||distMin||connections>grouping||connections<1)
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
        List<Vector3> NodePos = new List<Vector3>();
        NodePos = GenerateNodePosition(Mathf.RoundToInt(Amount.gameObject.GetComponent<Slider>().value), Mathf.RoundToInt(Grouping.gameObject.GetComponent<Slider>().value));
        for (int i = 0; i < NodePos.Count; i++)
        {
            GameObject NewNode = GameObject.Instantiate((GameObject)Resources.Load("NodePrefab"), NodePos[i], Quaternion.identity);
            NewNode.gameObject.name = "Node " + i.ToString();
        }
        Vector3 pos = randomer();
        GameObject NPixie = GameObject.Instantiate((GameObject)Resources.Load("PixiePrefab"), pos, Quaternion.identity);
        int[] SE = StartEnd(NodePos);
        GameObject StartNode = GameObject.Find("Node " + SE[0].ToString());
        GameObject EndNode = GameObject.Find("Node " + SE[1].ToString());
        NPixie.gameObject.GetComponent<Pixie>().collapsedNode_ = StartNode.gameObject.GetComponent<Node>();
        EndNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.WIN;
    }

}
