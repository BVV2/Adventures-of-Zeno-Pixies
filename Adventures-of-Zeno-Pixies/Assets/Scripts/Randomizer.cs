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
        int killswitch = 0;
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
                    if (Vector3.Distance(PositionList[i], PositionList[j]) < 4f)
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
                killswitch++;
            }
            if (killswitch == 50)
            {
                Debug.Log("Breaking out from infinite position generating! The job was stopped on " + i.ToString());
                i = num + 1;             
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

    public void starter()
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
        GameObject UI = GameObject.Find("MainUICanvas");
        UI.gameObject.GetComponent<UI>().thePixie_ = NPixie.gameObject.GetComponent<Pixie>();
        for (int i = 0; i < NodePos.Count; i++)
        {
            GameObject NewNode = GameObject.Instantiate((GameObject)Resources.Load("NodePrefab"), NodePos[i], Quaternion.identity);
            NewNode.gameObject.name = "Node " + i.ToString();
            NodeList.Add(NewNode.gameObject.GetComponent<Node>());
        }
        Debug.Log("Nodes generated");
        int[] SE = StartEnd(NodePos);
        GameObject StartNode = GameObject.Find("Node " + SE[0].ToString());
        GameObject EndNode = GameObject.Find("Node " + SE[1].ToString());
        NPixie.gameObject.GetComponent<Pixie>().collapsedNode_ = StartNode.gameObject.GetComponent<Node>();
        EndNode.gameObject.GetComponent<NodeTrigger>().type_ = NodeTypes.WIN;
        //EndNode.gameObject.GetComponent<NodeTrigger>().ChangeGraphic();

        StartCoroutine(Connector(SE[0]));     
    }

    public IEnumerator Connector(int StartNode)
    {
        for (int i = 0; i < NodePos.Count; i++)
        {
            if ((Vector3.Distance(NodePos[StartNode], NodePos[i]) < 10f) && (StartNode != i) && NodeList[StartNode].connectedNodes_.Count < 4)
            {
                NodeList[StartNode].ConnectNode(NodeList[i]);
                yield return new WaitForSeconds(2f);

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
    }

}
