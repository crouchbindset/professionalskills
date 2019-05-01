using UnityEngine;
using Vuforia;

// Team 1 Augmented Reality Project
// @author Jack Fisher

public class COOPGame : MonoBehaviour, ITrackableEventHandler
{
    // Current player
    public CPlayer bluePlayer;
    // Opponent player
    public CPlayer yellowPlayer;

    //Trackable objects representing players cards
    public TrackableBehaviour blueTrackable;
    public TrackableBehaviour yellowTrackable;

    //Default position and scale vectors for augmented models
    public Vector3 position = new Vector3(0, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);

    //The current game turn
    public int turnCounter;

    //Deck attributes
    public string[] dinosaurAttributes = { "Name", "Speed", "Strength", "Agility" };
    public string[] vehicleAttributes = { "Name", "Speed", "Power", "Control" };
    public string[] deckAttributes;
    //Chosen deck to use, 0 = Dinosaurs, 1 = Vehicles
    public int chosenDeck = 0;

    public void useDinosaurs()
    {
        chosenDeck = 0;
    }
    public void useVehicles()
    {
        chosenDeck = 1;
    }

    //Start is called before the first frame update
    void Start()
    {
        //Create player objects, with player name parameter
        bluePlayer = new CPlayer("Blue Player");
        yellowPlayer = new CPlayer("Yellow Player");

        //Initialize the game at turn 1
        turnCounter = 1;

        // If trackables are set to objects, register them
        if (blueTrackable)
        {
            blueTrackable.RegisterTrackableEventHandler(this);
        }
        if (yellowTrackable)
        {
            yellowTrackable.RegisterTrackableEventHandler(this);
        }
    }

    //Start is called before the first frame update
    void OnEnable()
    {
        //Create player objects, with player name parameter
        bluePlayer = new CPlayer("Blue Player");
        yellowPlayer = new CPlayer("Yellow Player");

        //Initialize the game at turn 1
        turnCounter = 1;

        // If trackables are set to objects, register them
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
        if (chosenDeck == 1)
        {
            deckAttributes = vehicleAttributes;
        }
        else
        {
            deckAttributes = dinosaurAttributes;
        }
        //Destroy any current children of the trackables
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
        //If the trackables are being tracked, create a child
        if (blueTrackable.GetComponent<TrackableBehaviour>().CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            //Get model from resources using player index
            Transform deckModel;
            if (chosenDeck == 1)
            {
                deckModel = Resources.Load<Transform>
                       ("Vehicles/" + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehicleName);
            }
            else
            {
                deckModel = Resources.Load<Transform>
                       ("Dinosaurs/" + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurName);
            }
            Transform blueTransform = Instantiate(deckModel) as Transform;
            //Add model as child to trackable
            blueTransform.parent = blueTrackable.transform;
            //Set model 3D position, scale and rotation
            blueTransform.localPosition = position;
            blueTransform.localScale = scale;
            blueTransform.localRotation = Quaternion.identity;
            //Set model as active (visible)
            blueTransform.gameObject.SetActive(true);
        }
        if (yellowTrackable.GetComponent<TrackableBehaviour>().CurrentStatus == TrackableBehaviour.Status.TRACKED && bluePlayer.HasChosen)
        {
            Transform deckModel;
            if (chosenDeck == 1)
            {
                deckModel = Resources.Load<Transform>
                       ("Vehicles/" + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehicleName);
            }
            else
            {
                deckModel = Resources.Load<Transform>
                       ("Dinosaurs/" + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurName);
            }
            Transform yellowTransform = Instantiate(deckModel) as Transform;
            yellowTransform.parent = yellowTrackable.transform;
            yellowTransform.localPosition = position;
            yellowTransform.localScale = scale;
            yellowTransform.localRotation = Quaternion.identity;
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
        //Loop back player deck index when end of deck is reached
        if (bluePlayer.DeckIndex >= bluePlayer.PlayerDinosaurs.Count)
        {
            bluePlayer.DeckIndex = 0;
        }
        if (yellowPlayer.DeckIndex >= yellowPlayer.PlayerDinosaurs.Count)
        {
            yellowPlayer.DeckIndex = 0;
        }

        GUIStyle labelStyle = new GUIStyle
        {
            fontSize = 36
        };
        labelStyle.alignment = TextAnchor.UpperCenter;

        GUIStyle cardDetailsLeft = new GUIStyle
        {
            fontSize = 36
        };
        cardDetailsLeft.normal.textColor = Color.blue;
        cardDetailsLeft.alignment = TextAnchor.UpperLeft;

        GUIStyle cardDetailsRight = new GUIStyle
        {
            fontSize = 36
        };
        cardDetailsRight.normal.textColor = Color.yellow;
        cardDetailsRight.alignment = TextAnchor.UpperRight;

        //Box at the top center to show current player turn
        Rect turnBox = new Rect(Screen.width / 2, 0, 0, 0);
        //Box in the center of the screen to show results
        Rect resultBox = new Rect(Screen.width / 2, Screen.width / 2, 0, 0);

        //If a player runs out of cards, end the game
        if (bluePlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.red;
            GUI.Label(resultBox, "You lost!", labelStyle);
            return;
        }
        else if (yellowPlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.green;
            GUI.Label(resultBox, "You won!", labelStyle);
            return;
        }

        switch (bluePlayer.TurnStatus)
        {
            case 1:
                labelStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You won this turn!", labelStyle);
                break;
            case 2:
                labelStyle.normal.textColor = Color.red;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You lost this turn!", labelStyle);
                break;
            case 3:
                labelStyle.normal.textColor = Color.white;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You drew this turn!", labelStyle);
                break;
            case 4:
                labelStyle.normal.textColor = Color.white;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "You must play your turn first", labelStyle);
                break;
        }

        if (turnCounter % 2 != 0)
        {
            labelStyle.normal.textColor = Color.blue;
            GUI.Label(turnBox, bluePlayer.PlayerName + "'s turn", labelStyle);
            bluePlayer.CurrentTurn = true;
            yellowPlayer.CurrentTurn = false;
        }
        else
        {
            labelStyle.normal.textColor = Color.yellow;
            GUI.Label(turnBox, yellowPlayer.PlayerName + "'s turn", labelStyle);
            bluePlayer.CurrentTurn = false;
            yellowPlayer.CurrentTurn = true;

        }

        labelStyle.normal.textColor = Color.blue;
        GUI.Label(new Rect(0, 0, 0, 0), "Score: " + bluePlayer.PlayerScore, cardDetailsLeft);

        labelStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(Screen.width, 0, 0, 0), "Score: " + yellowPlayer.PlayerScore, cardDetailsRight);

        if (chosenDeck == 1)
        {
            GUI.Label(new Rect(0, Screen.height / 2 - 75, 0, 0), deckAttributes[0] + ": " + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehicleName, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 - 25, 0, 0), deckAttributes[1] + ": " + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehicleSpeed, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 + 25, 0, 0), deckAttributes[2] + ": " + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehiclePower, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 + 75, 0, 0), deckAttributes[3] + ": " + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehicleAgility, cardDetailsLeft);
        }
        else
        {
            GUI.Label(new Rect(0, Screen.height / 2 - 75, 0, 0), deckAttributes[0] + ": " + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurName, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 - 25, 0, 0), deckAttributes[1] + ": " + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurSpeed, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 + 25, 0, 0), deckAttributes[2] + ": " + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurStrength, cardDetailsLeft);
            GUI.Label(new Rect(0, Screen.height / 2 + 75, 0, 0), deckAttributes[3] + ": " + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurAgility, cardDetailsLeft);
        }
        if (bluePlayer.CurrentTurn)
        {
            //Only show opponents attributes after player has chosen
            if (bluePlayer.HasChosen)
            {
                if (chosenDeck == 1)
                {
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 - 75, 0, 0), deckAttributes[0] + ": " + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehicleName, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 - 25, 0, 0), deckAttributes[1] + ": " + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehicleSpeed, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 + 25, 0, 0), deckAttributes[2] + ": " + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehiclePower, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 + 75, 0, 0), deckAttributes[3] + ": " + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehicleAgility, cardDetailsRight);
                }
                else
                {
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 - 75, 0, 0), deckAttributes[0] + ": " + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurName, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 - 25, 0, 0), deckAttributes[1] + ": " + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurSpeed, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 + 25, 0, 0), deckAttributes[2] + ": " + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurStrength, cardDetailsRight);
                    GUI.Label(new Rect(Screen.width, Screen.height / 2 + 75, 0, 0), deckAttributes[3] + ": " + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurAgility, cardDetailsRight);
                }
            }
            //If the player hasn't chosen yet, display buttons to choose attribute
            if (!bluePlayer.HasChosen)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height - 200, 100, 50), deckAttributes[1]))
                {
                    if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurSpeed > yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurSpeed)
                    {
                        bluePlayer.TurnStatus = 1;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurSpeed < yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurSpeed)
                    {
                        bluePlayer.TurnStatus = 2;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurSpeed == yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurSpeed)
                    {
                        bluePlayer.TurnStatus = 3;
                    }
                    bluePlayer.HasChosen = true;
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 200, 100, 50), deckAttributes[2]))
                {
                    if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurStrength > yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurStrength)
                    {
                        bluePlayer.TurnStatus = 1;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurStrength < yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurStrength)
                    {
                        bluePlayer.TurnStatus = 2;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurStrength == yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurStrength)
                    {
                        bluePlayer.TurnStatus = 3;
                    }
                    bluePlayer.HasChosen = true;
                }

                if (GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height - 200, 100, 50), deckAttributes[3]))
                {
                    if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurAgility > yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurAgility)
                    {
                        bluePlayer.TurnStatus = 1;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurAgility < yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurAgility)
                    {
                        bluePlayer.TurnStatus = 2;
                    }
                    else if (bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurAgility == yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurAgility)
                    {
                        bluePlayer.TurnStatus = 3;
                    }
                    bluePlayer.HasChosen = true;
                }
            }
            //If the player has chosen the attribute, show button to go to next turn
            if (bluePlayer.HasChosen)
            {
                if (GUI.Button(new Rect((Screen.width / 2) - 100, Screen.height - 100, 200, 50), "Next turn"))
                {
                    if (bluePlayer.TurnStatus != 1 && bluePlayer.TurnStatus != 2 && bluePlayer.TurnStatus != 3)
                    {
                        bluePlayer.TurnStatus = 4;
                    }
                    else
                    {
                        if (bluePlayer.TurnStatus == 1)
                        {
                            bluePlayer.PlayerScore += 1;
                            bluePlayer.PlayerDinosaurs.Add(yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex]);
                            yellowPlayer.PlayerDinosaurs.RemoveAt(yellowPlayer.DeckIndex);
                        }
                        else if (bluePlayer.TurnStatus == 2)
                        {
                            yellowPlayer.PlayerScore += 1;
                            yellowPlayer.PlayerDinosaurs.Add(bluePlayer.PlayerDinosaurs[yellowPlayer.DeckIndex]);
                            bluePlayer.PlayerDinosaurs.RemoveAt(yellowPlayer.DeckIndex);
                        }
                        else if (bluePlayer.TurnStatus == 3)
                        {

                        }
                        bluePlayer.TurnStatus = 0;
                        bluePlayer.HasChosen = false;
                        turnCounter++;
                        bluePlayer.DeckIndex++;
                        yellowPlayer.DeckIndex++;
                    }
                }
            }
        }
    }
}
