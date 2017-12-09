using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    // Yes I know, could use another class, but this is just faster for me to code
    [Header("Starting Stats")]
    // Seconds you can keep observing non-stop.
    public float concentration_ = 10f;
    // Drink from here whenever you press observe
    public float manaReserve_ = 100f;
    // Pixie health. Reaches 0 = dead pixie, game over
    public float pixieHealth_ = 100f;

    [Header("UI objects")]
    public GameObject winPanel_;
    public Button startObserve_;
    public Button stopObserve_;

    public Image concentrationImage_;
    public Image manaReserveImage_;

    // Change this to be a nice number.
    private float concentrationMax_ = 10f;
    private float manaReserveMax_ = 100f;

    private Pixie thePixie_;

    public static bool isObserving_;

	// Use this for initialization
	void Start () {

        startObserve_.onClick.AddListener(delegate { StartObserving(); });
        stopObserve_.onClick.AddListener(delegate { StopObserving(); });

        thePixie_ = FindObjectOfType<Pixie>();
        if (thePixie_ == null)
        {
            Debug.LogWarning("No pixie found! aaah!");
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
        else
        {// slowly recharge manapool when not observing
            if (manaReserve_ < manaReserveMax_)
            {
                manaReserve_ += 0.005f;
            };
        }
        concentrationImage_.fillAmount = concentration_ / concentrationMax_;
        manaReserveImage_.fillAmount = manaReserve_ / manaReserveMax_;

        


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
        winPanel_.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
