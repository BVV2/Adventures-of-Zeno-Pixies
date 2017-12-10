﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour {




    public List<Vector3> NodePos;
    public List<Node> NodeList;
    // Use this for initialization
    void Start () {

        NodePos = new List<Vector3>();
        NodeList = new List<Node>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 randomer()
    {
        Vector3 vec = new Vector3();
        vec.x = Random.Range(-15f, 15f);
        vec.y = Random.Range(-15f, 15f);
        vec.z = 0f;
        return vec;
    }

    public List<Vector3> GenerateNodePosition (int num, int grouping = 4)
    {
        int killswitch = 0;
        List<Vector3> PositionList = new List<Vector3>();
        for (int i=0; i < num; i++) // first random draft of the list
        {
            PositionList.Add(randomer());
        }
        for (int i = 0; i < num; i++) // analyzing each position in match with others
        {
            
            bool distMin = false;
            int connections = 0;
            for (int j = 0; j < num; j++)
            {
                if (i != j)
                {
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 4f) //not too close
                        distMin = true;
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 10f)//counting possible connections
                    {                       
                        connections++;
                    }
                }
            }
            if (distMin||connections>grouping||connections<2) //if too close or too many in one place
            {
                PositionList.RemoveAt(i);
                PositionList.Add(randomer());       //delete and try again
                i -= 1;
                killswitch++;
            }
            if (killswitch == 90) // if too many optimization attempts are done - breaking the cycle with last position list version
            {
                Debug.Log("Breaking out from infinite position generating! The job was stopped on " + i.ToString());
                i = num + 1;             
            }
        }
        
        return PositionList;
    }

    public int[] StartEnd(List<Vector3> list) //approximation of best start and end points
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

    public void starter() //function to generate level out of dynamic settings and starting button
    {
        GameObject Amount = GameObject.Find("NodeAmount");
        GameObject Grouping = GameObject.Find("Grouping");
        GameObject Difficulty = GameObject.Find("Difficulty");
        int amount = Mathf.RoundToInt(Amount.gameObject.GetComponent<Slider>().value);
        int group = Mathf.RoundToInt(Grouping.gameObject.GetComponent<Slider>().value);
        int diff = Mathf.RoundToInt(Difficulty.gameObject.GetComponent<Slider>().value);
        Spawner(amount, group, diff);
    }

        public void Spawner(int amount, int grouping, int difficulty)
    {
        
        Debug.Log("Spawn " + amount.ToString() + " nodes with " + grouping.ToString());
        NodePos = GenerateNodePosition(amount, grouping);
        Debug.Log("Node position list generated");
        Vector3 pos = randomer();
        GameObject NPixie = GameObject.Instantiate((GameObject)Resources.Load("PixiePrefab"), pos, Quaternion.identity);
        Debug.Log("Pixie spawned");
        GameObject GUI = GameObject.Find("MainUICanvas");
        GUI.gameObject.GetComponent<UI>().thePixie_ = NPixie.gameObject.GetComponent<Pixie>(); //make IU see the pixie
        
        for (int i = 0; i < NodePos.Count; i++) //instantiation loop
        {
            GameObject NewNode = GameObject.Instantiate((GameObject)Resources.Load("NodePrefab"), NodePos[i], Quaternion.identity);
            NewNode.gameObject.name = "Node " + i.ToString();
            NodeList.Add(NewNode.gameObject.GetComponent<Node>());
        }
        Debug.Log("Nodes generated");

        int[] SE = StartEnd(NodePos); //finding and assigning start and end nodes
        GameObject StartNode = GameObject.Find("Node " + SE[0].ToString());
        GameObject EndNode = GameObject.Find("Node " + SE[1].ToString());
        NPixie.gameObject.GetComponent<Pixie>().collapsedNode_ = StartNode.gameObject.GetComponent<Node>();
        EndNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.WIN;
        //EndNode.gameObject.GetComponent<NodeTrigger>().ChangeGraphic();
        Specializer(SE[0],SE[1], difficulty); // make every node special
        StartCoroutine(Connector(SE[0]));  //make first random connector (buggy)   
    }

    public IEnumerator Connector(int StartNode) //should connect to up to 4 nodes close to start node
    {
        for (int i = 0; i < NodePos.Count; i++)
        {
            if ((Vector3.Distance(NodePos[StartNode], NodePos[i]) < 10f) && (StartNode != i) && NodeList[StartNode].connectedNodes_.Count < 4)
            {
                NodeList[StartNode].ConnectNode(NodeList[i]);
                yield return new WaitForSeconds(1f);

            }
        }
    }
    
    private void Specializer(int start, int end, int difficulty)
    {
        
        for (int i = 0; i < NodeList.Count; i++)
        {
            if ((i!=start)&&(i!=end))
            {
                
                
                if (Random.Range(0, 100f) > 50f) // roll dice for specialization
                {
                    GameObject SNode = GameObject.Find("Node " + i.ToString());
                    if (Random.Range(0, 100f) > (50f / (2 / difficulty))) // roll dice for good or bad
                    {//good
                        if (Random.Range(0, 100f) > 50f) // roll dice for mana or health
                        {//mana
                            SNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.MANA_UP;
                            Debug.Log("Node " + i.ToString() + " state is changed to MANA_UP");
                        }
                        else
                        {//health
                            SNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.HEALTH_UP;
                            Debug.Log("Node " + i.ToString() + " state is changed to HEALTH_UP");
                        }
                    }
                    else
                    {//bad
                        if (Random.Range(0, 100f) > 50f) // roll dice for mana or health
                        {//mana
                            SNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.MANA_DOWN;
                            Debug.Log("Node " + i.ToString() + " state is changed to MANA_DOWN");
                        }
                        else
                        {//health
                            SNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.HEALTH_DOWN;
                            Debug.Log("Node " + i.ToString() + " state is changed to HEALTH_DOWN");
                        }

                    }

                }
                
            }
        }

        for (int r = 0; r < difficulty; r++) //take 1-3 nodes from list and turn them in objectives
        {
            int pointer = Mathf.RoundToInt(Random.Range(0, NodeList.Count));
            if ((pointer != start) && (pointer != end)&& (GameObject.Find("Node " + pointer.ToString()).gameObject.GetComponent<NodeTrigger>().type_!=NodeTypes.OBJECTIVE))
            {
                GameObject.Find("Node " + pointer.ToString()).gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.OBJECTIVE;
            }
            else
            {
                r--; //if requirements not matching - try again
            }
        }
    }

}
