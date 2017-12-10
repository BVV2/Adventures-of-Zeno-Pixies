using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour {
    public int Difficulty;
    public int Amount;
    public int Group;
    public Randomizer Rand;
	// Use this for initialization
	void Start () {
        Rand.Spawner(Amount, Group, Difficulty);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
