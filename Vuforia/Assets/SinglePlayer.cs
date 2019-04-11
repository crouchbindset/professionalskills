using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SinglePlayer : MonoBehaviour, ITrackableEventHandler
{
    //A player to act as the user
    public CPlayer bluePlayer;
    //A player to act as the AI opponent
    public CPlayer yellowPlayer;

    //Integer counters for tracking turn number and deck index
    private int turnCounter;
    private int blueIndex;
    private int yellowIndex;
    private string turnStatus = "";

    //Name of the current turns player
    public string currentPlayer;

    //Trackable objects representing physical cards
    public TrackableBehaviour blueTrackable;
    public TrackableBehaviour yellowTrackable;

    //Vector position and scale for augmented models
    public Vector3 position = new Vector3(0, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);


    //Start is called before the first frame update
    void Start()
    {
        //Create player objects, with player name parameter
        bluePlayer = new CPlayer("Blue Player");
        yellowPlayer = new CPlayer("Yellow Player");

        //Initialise at turn 1
        turnCounter = 1;
        blueIndex = 0;
        yellowIndex = 0;

        if (blueTrackable)
        {
            blueTrackable.RegisterTrackableEventHandler(this);
        }
        if (yellowTrackable)
        {
            yellowTrackable.RegisterTrackableEventHandler(this);
        }
    }

    //Update is called once per frame
    void Update()
    {
        for (int i = 0; i < blueTrackable.transform.childCount; i++)
        {
            Transform child = blueTrackable.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < yellowTrackable.transform.childCount; i++)
        {
            Transform child = yellowTrackable.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        if (blueTrackable.GetComponent<TrackableBehaviour>().CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {

            Transform blueTransform = Instantiate(Resources.Load<Transform>("Dinosaurs/" + bluePlayer.PlayerDinosaurs[blueIndex].DinosaurName)) as Transform;

            blueTransform.parent = blueTrackable.transform;
            blueTransform.localPosition = position;
            blueTransform.localRotation = Quaternion.identity;
            blueTransform.localScale = scale;

            blueTransform.gameObject.SetActive(true);
        }

        if (yellowTrackable.GetComponent<TrackableBehaviour>().CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            Transform yellowTransform = Instantiate(Resources.Load<Transform>("Dinosaurs/" + yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurName)) as Transform;

            yellowTransform.parent = yellowTrackable.transform;
            yellowTransform.localPosition = position;
            yellowTransform.localRotation = Quaternion.identity;
            yellowTransform.localScale = scale;

            yellowTransform.gameObject.SetActive(true);
        }
    }

    public void OnTrackableStateChanged(
         TrackableBehaviour.Status previousStatus,
         TrackableBehaviour.Status newStatus)
    {

    }

    //GUI is called once per frame and handles GUI elements
    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle
        {
            fontSize = 36
        };
        labelStyle.alignment = TextAnchor.UpperCenter;

        if (blueIndex >= bluePlayer.PlayerDinosaurs.Count)
        {
            blueIndex = 0;
        }

        if (yellowIndex >= yellowPlayer.PlayerDinosaurs.Count)
        {
            yellowIndex = 0;
        }

        if (bluePlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You lost the game!", labelStyle);
            return;
        }
        else if (yellowPlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.green;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You won the game!", labelStyle);
            return;
        }

        GUIStyle cardDetailsLeft = new GUIStyle
        {
            fontSize = 36
        };
        cardDetailsLeft.normal.textColor = Color.blue;

        GUIStyle cardDetailsRight = new GUIStyle
        {
            fontSize = 36
        };
        cardDetailsRight.normal.textColor = Color.yellow;
        cardDetailsRight.alignment = TextAnchor.UpperRight;

        Rect turnBox = new Rect(Screen.width / 2, 0, 0, 0);
        Rect resultBox = new Rect(Screen.width / 2, Screen.width / 2, 0, 0);

        switch (turnStatus)
        {
            case "won":
                labelStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You won this turn!", labelStyle);
                break;
            case "lost":
                labelStyle.normal.textColor = Color.red;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You lost this turn!", labelStyle);
                break;
            case "drew":
                labelStyle.normal.textColor = Color.white;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You drew this turn!", labelStyle);
                break;
        }

        if (turnCounter % 2 != 0)
        {
            currentPlayer = bluePlayer.PlayerName;
            labelStyle.normal.textColor = Color.blue;
        }
        else
        {
            currentPlayer = yellowPlayer.PlayerName;
            labelStyle.normal.textColor = Color.yellow;
        }

        GUI.Label(turnBox, currentPlayer + "'s turn", labelStyle);

        labelStyle.normal.textColor = Color.blue;
        GUI.Label(new Rect(0, 0, 0, 0), "Score: " + bluePlayer.PlayerScore, cardDetailsLeft);

        labelStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(Screen.width, 0, 0, 0), "Score: " + yellowPlayer.PlayerScore, cardDetailsRight);

        GUI.Label(new Rect(0, Screen.height / 2 - 75, 0, 0), "Name: " + bluePlayer.PlayerDinosaurs[blueIndex].DinosaurName, cardDetailsLeft);
        GUI.Label(new Rect(0, Screen.height / 2 - 25, 0, 0), "Speed: " + bluePlayer.PlayerDinosaurs[blueIndex].DinosaurSpeed, cardDetailsLeft);
        GUI.Label(new Rect(0, Screen.height / 2 + 25, 0, 0), "Strength: " + bluePlayer.PlayerDinosaurs[blueIndex].DinosaurStrength, cardDetailsLeft);
        GUI.Label(new Rect(0, Screen.height / 2 + 75, 0, 0), "Agility: " + bluePlayer.PlayerDinosaurs[blueIndex].DinosaurAgility, cardDetailsLeft);

        GUI.Label(new Rect(Screen.width, Screen.height / 2 - 75, 0, 0), "Name: " + yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurName, cardDetailsRight);
        GUI.Label(new Rect(Screen.width, Screen.height / 2 - 25, 0, 0), "Speed: " + yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurSpeed, cardDetailsRight);
        GUI.Label(new Rect(Screen.width, Screen.height / 2 + 25, 0, 0), "Strength: " + yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurStrength, cardDetailsRight);
        GUI.Label(new Rect(Screen.width, Screen.height / 2 + 75, 0, 0), "Agility: " + yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurAgility, cardDetailsRight);

        if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height - 200, 100, 50), "Speed"))
        {
            if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurSpeed > yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurSpeed)
            {
                turnStatus = "won";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurSpeed < yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurSpeed)
            {
                turnStatus = "lost";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurSpeed == yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurSpeed)
            {
                turnStatus = "drew";
            }
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 200, 100, 50), "Strength"))
        {
            if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurStrength > yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurStrength)
            {
                turnStatus = "won";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurStrength < yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurStrength)
            {
                turnStatus = "lost";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurStrength == yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurStrength)
            {
                turnStatus = "drew";
            }
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height - 200, 100, 50), "Agility"))
        {
            if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurAgility > yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurAgility)
            {
                turnStatus = "won";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurAgility < yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurAgility)
            {
                turnStatus = "lost";
            }
            else if (bluePlayer.PlayerDinosaurs[blueIndex].DinosaurAgility == yellowPlayer.PlayerDinosaurs[yellowIndex].DinosaurAgility)
            {
                turnStatus = "drew";
            }
        }

        if (GUI.Button(new Rect((Screen.width / 2) - 100, Screen.height - 100, 200, 50), "Next turn"))
        {
            if (turnStatus == "won")
            {
                bluePlayer.PlayerScore += 1;
                bluePlayer.PlayerDinosaurs.Add(yellowPlayer.PlayerDinosaurs[yellowIndex]);
                yellowPlayer.PlayerDinosaurs.RemoveAt(yellowIndex);
            }
            else if (turnStatus == "lost")
            {
                yellowPlayer.PlayerScore += 1;
                yellowPlayer.PlayerDinosaurs.Add(bluePlayer.PlayerDinosaurs[yellowIndex]);
                bluePlayer.PlayerDinosaurs.RemoveAt(yellowIndex);
            }
            else if (turnStatus == "drew")
            {

            }
            turnStatus = "";
            turnCounter++;
            blueIndex++;
            yellowIndex++;
        };
    }
}
