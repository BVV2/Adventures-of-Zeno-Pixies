using System.Collections;

using UnityEngine;

using Fungus;


[
    CommandInfo(
        "Flow",
        "WaitUntil",
        "Waits Until a variable returns true"
    )
]
[AddComponentMenu("")]
public class VariableWaitUntil : VariableCondition {
    // Wait for this long - cannot be changed now
    public FloatData waitForSeconds_ = new FloatData(0.1f);
    
    
    public override void OnEnter() {
        //bool condition = EvaluateCondition();
        //Debug.Log("Condition is: " + condition.ToString());
        StartCoroutine(Waiter());
        
        //StartCoroutine(WaitUntilTrue());
    }
    
    
    IEnumerator Waiter() {
        // Evaluate every x seconds if a value is given, otherwise just do a classic WaitUntil
        if (waitForSeconds_ > 0f) {
            while (!EvaluateCondition()) {
                yield return new WaitForSeconds(waitForSeconds_);
            }
        }
        else {
            yield return new WaitUntil(
                () => EvaluateCondition()
            );
        }
        
        Continue();
    }
    
    
    public override bool OpenBlock() {
        return false;
    }
}
