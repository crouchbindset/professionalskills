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

    //Load text from file
    public CGetDialogue getDialogue;

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
        bluePlayer = new CPlayer(getDialogue.GetDialogue("blue_player"));
        yellowPlayer = new CPlayer(getDialogue.GetDialogue("yellow_player"));

        //Set up game so blue player starts
        bluePlayer.CurrentTurn = true;
        yellowPlayer.CurrentTurn = false;

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
        bluePlayer = new CPlayer(getDialogue.GetDialogue("blue_player"));
        yellowPlayer = new CPlayer(getDialogue.GetDialogue("yellow_player"));

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
                       (getDialogue.GetDialogue("category_vehicles") + bluePlayer.PlayerVehicles[bluePlayer.DeckIndex].VehicleName);
            }
            else
            {
                deckModel = Resources.Load<Transform>
                       (getDialogue.GetDialogue("category_dinosaurs") + bluePlayer.PlayerDinosaurs[bluePlayer.DeckIndex].DinosaurName);
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
                       (getDialogue.GetDialogue("category_vehicles") + yellowPlayer.PlayerVehicles[yellowPlayer.DeckIndex].VehicleName);
            }
            else
            {
                deckModel = Resources.Load<Transform>
                       (getDialogue.GetDialogue("category_dinosaurs") + yellowPlayer.PlayerDinosaurs[yellowPlayer.DeckIndex].DinosaurName);
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
        //Create button and text styles
        GUIStyle buttonStyle = new GUIStyle("button")
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
        };
        GUIStyle labelStyle = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter
        };

        GUIStyle cardDetailsLeft = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft
        };
        cardDetailsLeft.normal.textColor = Color.blue;

        GUIStyle cardDetailsRight = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperRight
        };
        cardDetailsRight.normal.textColor = Color.yellow;

        //Box at the top center to show current player turn
        Rect turnBox = new Rect(Screen.width / 2, 20, 0, 0);

        //Player scores
        labelStyle.normal.textColor = Color.blue;
        GUI.Label(new Rect(0, 0, 0, 0), getDialogue.GetDialogue("score") + bluePlayer.PlayerScore, cardDetailsLeft);
        labelStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(Screen.width, 0, 0, 0), getDialogue.GetDialogue("score") + yellowPlayer.PlayerScore, cardDetailsRight);

        //Decide which players turn it is, update turn display and play turn
        if (bluePlayer.CurrentTurn)
        {
            labelStyle.normal.textColor = Color.blue;
            GUI.Label(turnBox, bluePlayer.PlayerName + getDialogue.GetDialogue("turn_append"), labelStyle);
            playTurn(bluePlayer, yellowPlayer, Color.blue, Color.yellow);
        }
        else
        {
            labelStyle.normal.textColor = Color.yellow;
            GUI.Label(turnBox, yellowPlayer.PlayerName + getDialogue.GetDialogue("turn_append"), labelStyle);
            playTurn(yellowPlayer, bluePlayer, Color.yellow, Color.blue);
        }
    }

    public void playTurn(CPlayer currentPlayer, CPlayer opponentPlayer, Color playerColour, Color opponentColour)
    {
        //Styles for button and labels
        GUIStyle buttonStyle = new GUIStyle("button")
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
        };
        GUIStyle labelStyle = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter
        };

        GUIStyle cardDetailsLeft = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft
        };
        cardDetailsLeft.normal.textColor = playerColour;

        GUIStyle cardDetailsRight = new GUIStyle
        {
            fontSize = 36,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperRight
        };
        cardDetailsRight.normal.textColor = opponentColour;

        //Box in the center of the screen to show results
        Rect resultBox = new Rect(Screen.width / 2, Screen.width / 2, 0, 0);

        //If a player runs out of cards, end the game
        if (currentPlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.red;
            GUI.Label(resultBox, getDialogue.GetDialogue("lose_message"), labelStyle);
            return;
        }
        else if (opponentPlayer.PlayerDinosaurs.Count <= 0)
        {
            labelStyle.normal.textColor = Color.green;
            GUI.Label(resultBox, getDialogue.GetDialogue("win_message"), labelStyle);
            return;
        }

        //Display turn status if turn has ended
        switch (currentPlayer.TurnStatus)
        {
            case 1:
                labelStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), getDialogue.GetDialogue("win_turn"), labelStyle);
                break;
            case 2:
                labelStyle.normal.textColor = Color.red;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), getDialogue.GetDialogue("lose_turn"), labelStyle);
                break;
            case 3:
                labelStyle.normal.textColor = Color.white;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), getDialogue.GetDialogue("draw_turn"), labelStyle);
                break;
            case 4:
                labelStyle.normal.textColor = Color.white;
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), getDialogue.GetDialogue("turn_not_played"), labelStyle);
                break;
        }

        //Choose which deck to get cards and models from
        if (chosenDeck == 1)
        {
            GUI.Label(new Rect(20, Screen.height / 2 - 100, 0, 0), "Name: " + currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleName, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2 - 50, 0, 0), "Speed: " + currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleSpeed, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2, 0, 0), "Power: " + currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehiclePower, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2 + 50, 0, 0), "Agility: " + currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleAgility, cardDetailsLeft);
        }
        else
        {
            GUI.Label(new Rect(20, Screen.height / 2 - 100, 0, 0), "Name: " + currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurName, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2 - 50, 0, 0), "Speed: " + currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurSpeed, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2, 0, 0), "Power: " + currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurStrength, cardDetailsLeft);
            GUI.Label(new Rect(20, Screen.height / 2 + 50, 0, 0), "Agility: " + currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurAgility, cardDetailsLeft);
        }
        //Only show opponents attributes after player has chosen
        if (currentPlayer.HasChosen)
        {
            if (chosenDeck == 1)
            {
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 - 75, 0, 0), "Name: " + opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleName, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 - 25, 0, 0), "Speed: " + opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleSpeed, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 + 25, 0, 0), "Power: " + opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehiclePower, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 + 75, 0, 0), "Agility: " + opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleAgility, cardDetailsRight);
            }
            else
            {
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 - 75, 0, 0), "Name: " + opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurName, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 - 25, 0, 0), "Speed: " + opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurSpeed, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 + 25, 0, 0), "Power: " + opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurStrength, cardDetailsRight);
                GUI.Label(new Rect(Screen.width - 20, Screen.height / 2 + 75, 0, 0), "Agility: " + opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurAgility, cardDetailsRight);
            }
        }
        //If the player hasn't chosen yet, display buttons to choose attribute
        if (!currentPlayer.HasChosen)
        {
            //pick attribute button
            if (GUI.Button(new Rect(Screen.width / 2 - 550, Screen.height - 200, 300, 100), "Speed", buttonStyle))
            {
                if (chosenDeck == 1)
                {
                    //Compare attributes, win
                    if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleSpeed > opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleSpeed)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    //loss
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleSpeed < opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleSpeed)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    //draw
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleSpeed == opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleSpeed)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
                else
                {
                    if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurSpeed > opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurSpeed)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurSpeed < opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurSpeed)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurSpeed == opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurSpeed)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 200, 300, 100), "Power", buttonStyle))
            {
                if (chosenDeck == 1)
                {
                    //Compare attributes, win
                    if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehiclePower > opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehiclePower)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    //loss
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehiclePower < opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehiclePower)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    //draw
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehiclePower == opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehiclePower)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
                else
                {
                    if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurStrength > opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurStrength)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurStrength < opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurStrength)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurStrength == opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurStrength)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height - 200, 300, 100), "Agility", buttonStyle))
            {

                if (chosenDeck == 1)
                {
                    //Compare attributes, win
                    if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleAgility > opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleAgility)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    //loss
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleAgility < opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleAgility)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    //draw
                    else if (currentPlayer.PlayerVehicles[currentPlayer.DeckIndex].VehicleAgility == opponentPlayer.PlayerVehicles[opponentPlayer.DeckIndex].VehicleAgility)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
                else
                {
                    if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurAgility > opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurAgility)
                    {
                        currentPlayer.TurnStatus = 1;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurAgility < opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurAgility)
                    {
                        currentPlayer.TurnStatus = 2;
                    }
                    else if (currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex].DinosaurAgility == opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex].DinosaurAgility)
                    {
                        currentPlayer.TurnStatus = 3;
                    }
                    currentPlayer.HasChosen = true;
                }
            }
        }
        //If the player has chosen the attribute, show button to go to next turn
        if (currentPlayer.HasChosen)
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 150, Screen.height - 200, 300, 100), "Next turn", buttonStyle))
            {
                //If player won, add other players card to deck and continue
                if (currentPlayer.TurnStatus == 1)
                {
                    currentPlayer.PlayerScore += 1;
                    currentPlayer.PlayerDinosaurs.Add(opponentPlayer.PlayerDinosaurs[opponentPlayer.DeckIndex]);
                    opponentPlayer.PlayerDinosaurs.RemoveAt(opponentPlayer.DeckIndex);
                }
                //If player loses, give card to opponent and switch players
                else if (currentPlayer.TurnStatus == 2)
                {
                    currentPlayer.CurrentTurn = false;
                    opponentPlayer.CurrentTurn = true;
                    opponentPlayer.PlayerScore += 1;
                    opponentPlayer.PlayerDinosaurs.Add(currentPlayer.PlayerDinosaurs[currentPlayer.DeckIndex]);
                    currentPlayer.PlayerDinosaurs.RemoveAt(currentPlayer.DeckIndex);
                }
                //Reset player values and increment current card index for players
                currentPlayer.TurnStatus = 0;
                currentPlayer.HasChosen = false;
                currentPlayer.DeckIndex++;
                opponentPlayer.DeckIndex++;
            }
        }
    }
}
