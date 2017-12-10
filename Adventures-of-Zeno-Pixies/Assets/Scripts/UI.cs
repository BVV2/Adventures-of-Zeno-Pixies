using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum NodeTypes
{
    NORMAL = 0000,
    WIN = 1000,
    HEALTH_DOWN = 2000,
    HEALTH_UP = 3000,
    MANA_DOWN = 4000,
    MANA_UP = 5000,
    OBJECTIVE = 6000
}

[System.Serializable]
public class NodeTypeGraphic
{
    public string name_;
    public NodeTypes nodeType_ = NodeTypes.NORMAL;
    public Sprite graphic_;
}

public class UI : MonoBehaviour {

    // Yes I know, could use another class, but this is just faster for me to code
    [Header("Starting Stats")]
    // Seconds you can keep observing non-stop.
    public static float concentration_ = 10f;
    // Drink from here whenever you press observe
    public static float manaReserve_ = 100f;
    // Pixie health. Reaches 0 = dead pixie, game over
    public static float pixieHealth_ = 100f;
    // Time, reaches 0 = game over
    public static float timer_ = 180f;
    // Objectives, must be 0 before the Win trigger can be activated
    public static int objectives_ = 0;

    [Header("UI objects")]
    public GameObject winPanel_;
    public GameObject losePanel_;
    public Button startObserve_;
    public Button stopObserve_;
    public Text timerText_;
    public Text objectivesLeftText_;

    public Image concentrationImage_;
    public Image manaReserveImage_;
    public Text manaReserveText_;
    public Image healthReserveImage_;
    public Text healthReserveText_;

    public NodeTypeGraphic[] nodeGraphics_;

    // Change this to be a nice number.
    static float concentrationMax_ = 10f;
    static float manaReserveMax_ = 100f;
    static float pixieHealthMax_ = 100f;

    public Pixie thePixie_;
    private float oldHealth_;
    private float oldMana_;

    public static bool isObserving_;

	// Use this for initialization
	void Start () {

        // Make sure timescale is ok.
        Time.timeScale = 1f;

        startObserve_.onClick.AddListener(delegate { StartObserving(); });
        stopObserve_.onClick.AddListener(delegate { StopObserving(); });

        thePixie_ = FindObjectOfType<Pixie>();
        if (thePixie_ == null)
        {
            Debug.LogWarning("No pixie found! aaah!");
        };
        StartCoroutine(TimerCountDown());
        oldMana_ = manaReserve_;
        oldHealth_ = pixieHealth_;

        // Add as many objectives as we find the types in the game
        foreach (NodeTrigger nt in FindObjectsOfType<NodeTrigger>())
        {
            if (nt.type_ == NodeTypes.OBJECTIVE)
            {
                objectives_ += 1;
            };
        };
        

	}
	
	// Update is called once per frame
	void Update () {
		
        
        // Mana drain while observing!
        if (isObserving_)
        {
            if (concentration_ > 0f)
            {
                concentration_ -= 0.02f;
            }
            else
            {
                StopObserving();
            };
        }
        concentrationImage_.fillAmount = concentration_ / concentrationMax_;
        manaReserveImage_.fillAmount = manaReserve_ / manaReserveMax_;
        healthReserveImage_.fillAmount = pixieHealth_ / pixieHealthMax_;
        manaReserveText_.text = ((int)manaReserve_).ToString() + "/" + ((int)manaReserveMax_).ToString();
        healthReserveText_.text = ((int)pixieHealth_).ToString() + "/" + ((int)pixieHealthMax_).ToString();
        if (objectives_ > 0)
        {
            objectivesLeftText_.text = "x " + objectives_.ToString();
        }
        else
        {
            objectivesLeftText_.text = "Go to the exit!";
        };
        if (pixieHealth_ == 0f)
        {
            ShowFail();
        }

        if (oldHealth_ != pixieHealth_)
        {
            GameObject parentObj = healthReserveImage_.transform.parent.gameObject;
            if (!LeanTween.isTweening(parentObj))
            {
                LeanTween.scale(parentObj, parentObj.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), 1.5f).setEase(LeanTweenType.punch);
                if (oldHealth_ < pixieHealth_)
                {
                    LeanTween.textColor(healthReserveText_.rectTransform, Color.green, 1f).setEase(LeanTweenType.punch);
                }
                else
                {
                    LeanTween.textColor(healthReserveText_.rectTransform, Color.red, 1f).setEase(LeanTweenType.punch);
                }
            };
            oldHealth_ = pixieHealth_;
            
        };
        if (oldMana_ != manaReserve_)
        {
            GameObject parentObj = manaReserveImage_.transform.parent.gameObject;
            if (!LeanTween.isTweening(parentObj))
            {
                LeanTween.scale(parentObj, parentObj.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), 1.5f).setEase(LeanTweenType.punch);
                if (oldMana_ < manaReserve_)
                {
                    LeanTween.textColor(manaReserveText_.rectTransform, Color.green, 1.5f).setEase(LeanTweenType.punch);
                }
                else
                {
                    LeanTween.textColor(manaReserveText_.rectTransform, Color.red, 1.5f).setEase(LeanTweenType.punch);
                };
                oldMana_ = manaReserve_;
            };
        }

    }

    public Sprite GetNodeTypeSprite(NodeTypes type)
    {
        foreach (NodeTypeGraphic entry in nodeGraphics_)
        {
            if (entry.nodeType_ == type)
            {
                return entry.graphic_;
            };
        };
        return null;
    }

    public void StartObserving()
    {
        // refill concentration to max, if there is any!
        if (concentration_ < concentrationMax_)
        {
            float missingConcentration = concentrationMax_ - concentration_;
            if (manaReserve_ > missingConcentration)
            {
                manaReserve_ -= missingConcentration;
                concentration_ = concentrationMax_;
            }
            else
            {
                concentration_ += manaReserve_;
                manaReserve_ = 0f;
            };
        };
        isObserving_ = true;
        startObserve_.gameObject.SetActive(false);
        stopObserve_.gameObject.SetActive(true);
        thePixie_.Collapse();
    }
    public void StopObserving()
    {
        startObserve_.gameObject.SetActive(true);
        stopObserve_.gameObject.SetActive(false);
        thePixie_.StopObserving();
        isObserving_ = false;
    }

    public void ShowWin()
    {
        Time.timeScale = 0.1f;
        winPanel_.SetActive(true);
    }

    public void ShowFail()
    {
        Time.timeScale = 0.1f;
        losePanel_.SetActive(true);
    }

    public void Quit()
    {
        // To menu
        SceneManager.LoadScene("IntroScene");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public float GetMaxMana() {
        return manaReserveMax_;
    }

    public static void ReduceMana(float amount)
    {
        if (manaReserve_ > amount)
        {
            manaReserve_ -= amount;
        }
        else
        {
            manaReserve_ = 0f;
        };
    }
    public static void AddMana(float amount)
    {
        if ((amount + manaReserve_) < manaReserveMax_)
        {
            manaReserve_ += amount;
        }
        else
        {
            manaReserve_ = manaReserveMax_;
        };
    }
    public static void ReduceHealth(float amount)
    {
        if (pixieHealth_ > amount)
        {
            pixieHealth_ -= amount;
        }
        else
        {
            pixieHealth_ = 0f;
        };
    }
    public static void AddHealth(float amount)
    {
        if ((amount + pixieHealth_) < pixieHealthMax_)
        {
            pixieHealth_ += amount;
        }
        else
        {
            pixieHealth_ = pixieHealthMax_;
        };
    }
    public static void CompleteObjective(int amount)
    {
        if (objectives_ > amount)
        {
            objectives_ -= amount;
        }
        else
        {
            objectives_ = 0;
        };
    }

    public IEnumerator TimerCountDown()
    {
        while (timer_>0f)
        {
            TimeSpan t = TimeSpan.FromSeconds(timer_);

            string answer = string.Format("{0:D2}m:{1:D2}s",
                            t.Minutes,
                            t.Seconds);
            timerText_.text = answer;
            yield return new WaitForSeconds(0.1f);
            timer_ -= 0.1f;
        };
        ShowFail();
    }

}
