using UnityEngine;
using Vuforia;

public class ModelSwapper : MonoBehaviour
{

    public TrackableBehaviour theTrackable;
    public GameObject theReplacement;
    public GameObject theOriginal;

    private bool mSwapModel = false;
    private bool mSwapped = false;

    void Start()
    {
        if (theTrackable == null)
        {
            Debug.Log("Warning: Trackable not set!");
        }
        theReplacement.SetActive(false);
        theOriginal.SetActive(true);
    }

    void Update()
    {
        if (mSwapModel && theTrackable != null)
        {
            SwapModel();
            mSwapModel = false;
        }
    }

    void OnGUI()
    {
        if (mSwapped == false)
        {
            GUI.Label(new Rect(50, 110, 200, 50), "Current model is a Megalodon!");
            if (GUI.Button(new Rect(50, 50, 200, 50), "Swap to Sabre Tooth"))
            {
                mSwapModel = true;
            }
        }
        else
        {
            GUI.Label(new Rect(50, 110, 200, 50), "Current model is a Sabre Tooth!");
            if (GUI.Button(new Rect(50, 50, 200, 50), "Swap to Megalodon"))
            {
                mSwapModel = true;
            }
        }
    }

    private void SwapModel()
    {
        if (mSwapped == false)
        {
            theOriginal.SetActive(false);
            theReplacement.SetActive(true);
        }
        else
        {
            theReplacement.SetActive(false);
            theOriginal.SetActive(true);
        }
        mSwapped = !mSwapped;
    }
}