using System.Collections.Generic;
using UnityEngine;
using Vuforia;

// Add AI player turn
// Bool to check who's turn
// If AI players turn, chosen attribute is whichever is highest rated
// Have a delay to show which was chosen and compare the values
// Move onto next turn

public class Player : MonoBehaviour, ITrackableEventHandler
{
    private GameSetup game = new GameSetup();
    private string turnStatus;
    private List<Dinosaur> playerDeck;
    private List<Dinosaur> AIDeck;
    private List<Dinosaur> drawPile = new List<Dinosaur>();
    private int turnNumber = 0;
    private int AITurn = 0;
    private bool playersTurn = true;

    private TrackableBehaviour playerCard;

    public static void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerDeck = new List<Dinosaur>(game.DinosaurDeck);
        Shuffle<Dinosaur>(playerDeck);
        AIDeck = new List<Dinosaur>(game.DinosaurDeck);
        Shuffle<Dinosaur>(AIDeck);

        if (playerDeck != null)
        {
            playerCard = GetComponent<TrackableBehaviour>();

            if (playerCard)
            {
                playerCard.RegisterTrackableEventHandler(this);
            }
        }
    }

    public void OnTrackableStateChanged(
         TrackableBehaviour.Status previousStatus,
         TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
         newStatus == TrackableBehaviour.Status.TRACKED)
        {
            //disable any pre-existing augmentation
            for (int i = 0; i < playerCard.transform.childCount; i++)
            {
                Transform child = playerCard.transform.GetChild(i);
                child.gameObject.SetActive(false);
            }

            Transform myModelTrf = Instantiate(Resources.Load<Transform>("Dinosaurs/" + playerDeck[turnNumber].DinosaurName)) as Transform;
            myModelTrf.parent = playerCard.transform;
            myModelTrf.localPosition = new Vector3(0f, 0f, 0f);
            myModelTrf.localRotation = Quaternion.identity;
            myModelTrf.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            myModelTrf.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DrawOutline(Rect r, string t, int strength, GUIStyle style)
    {
        GUI.color = Color.black;
        int i;
        for (i = -strength; i <= strength; i++)
        {
            GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
            GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
        }
        for (i = -strength + 1; i <= strength - 1; i++)
        {
            GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
            GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
        }
        GUI.color = Color.white;
        GUI.Label(new Rect(r.x, r.y, r.width, r.height), t, style);
    }

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle
        {
            fontSize = 20
        };
        myStyle.normal.textColor = Color.white;

        if (AIDeck.Count == 0)
        {
            DrawOutline(new Rect(200, 200, 100, 20), "You won!", 2, myStyle);
            return;
        }
        else if (playerDeck.Count == 0)
        {
            DrawOutline(new Rect(200, 200, 100, 20), "You lost!", 2, myStyle);
            return;
        }

        Texture starIcon = Instantiate(Resources.Load("Star")) as Texture;
        if (playerCard == true)
        {
            DrawOutline(new Rect(10, 10, 100, 20), playerDeck[turnNumber].DinosaurName + " " + (turnNumber + 1) + "/" + playerDeck.Count, 2, myStyle);
            DrawOutline(new Rect(210, 10, 100, 20), AIDeck[AITurn].DinosaurName + " " + (AITurn + 1) + "/" + AIDeck.Count, 2, myStyle);
            DrawOutline(new Rect(400, 10, 100, 20), "Draw pile: " + drawPile.Count, 2, myStyle);
            if (turnStatus != null)
            {
                DrawOutline(new Rect(500, 50, 200, 20), "You " + turnStatus + "!", 2, myStyle);
            }
            if (playersTurn == true)
            {
                if (GUI.Button(new Rect(10, 50, 100, 20), "Size: " + playerDeck[turnNumber].DinosaurSize))
                {
                    if (playerDeck[turnNumber].DinosaurSize < AIDeck[AITurn].DinosaurSize)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = false;
                    }
                    else if (playerDeck[turnNumber].DinosaurSize > AIDeck[AITurn].DinosaurSize)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (GUI.Button(new Rect(10, 100, 100, 20), "Speed: " + playerDeck[turnNumber].DinosaurSpeed))
                {
                    if (playerDeck[turnNumber].DinosaurSpeed < AIDeck[AITurn].DinosaurSpeed)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = false;
                    }
                    else if (playerDeck[turnNumber].DinosaurSpeed > AIDeck[AITurn].DinosaurSpeed)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (GUI.Button(new Rect(10, 150, 100, 20), "Age: " + playerDeck[turnNumber].DinosaurAge))
                {
                    if (playerDeck[turnNumber].DinosaurAge < AIDeck[AITurn].DinosaurAge)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = false;
                    }
                    else if (playerDeck[turnNumber].DinosaurAge > AIDeck[AITurn].DinosaurAge)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (GUI.Button(new Rect(10, 200, 100, 20), "Strength: " + playerDeck[turnNumber].DinosaurStrength))
                {
                    if (playerDeck[turnNumber].DinosaurStrength < AIDeck[AITurn].DinosaurStrength)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = false;
                    }
                    else if (playerDeck[turnNumber].DinosaurStrength > AIDeck[AITurn].DinosaurStrength)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = true;
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
            }
            else
            {
                int topValue = AIDeck[AITurn].DinosaurSize;
                string chosenValue = "size";
                if (AIDeck[AITurn].DinosaurSpeed > topValue)
                {
                    topValue = AIDeck[AITurn].DinosaurSpeed;
                    chosenValue = "speed";
                }
                if (AIDeck[AITurn].DinosaurAge > topValue)
                {
                    topValue = AIDeck[AITurn].DinosaurAge;
                    chosenValue = "age";
                }
                if (AIDeck[AITurn].DinosaurStrength > topValue)
                {
                    topValue = AIDeck[AITurn].DinosaurStrength;
                    chosenValue = "strength";
                }
                if (chosenValue == "size")
                {
                    DrawOutline(new Rect(300, 500, 200, 20), "AI chose " + chosenValue, 2, myStyle);
                    if (playerDeck[turnNumber].DinosaurSize < AIDeck[AITurn].DinosaurSize)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else if (playerDeck[turnNumber].DinosaurSize > AIDeck[AITurn].DinosaurSize)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = true;
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (chosenValue == "speed")
                {
                    DrawOutline(new Rect(300, 500, 200, 20), "AI chose " + chosenValue, 2, myStyle);
                    if (playerDeck[turnNumber].DinosaurSpeed < AIDeck[AITurn].DinosaurSpeed)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else if (playerDeck[turnNumber].DinosaurSpeed > AIDeck[AITurn].DinosaurSpeed)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = true;
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (chosenValue == "age")
                {
                    DrawOutline(new Rect(300, 500, 200, 20), "AI chose " + chosenValue, 2, myStyle);
                    if (playerDeck[turnNumber].DinosaurAge < AIDeck[AITurn].DinosaurAge)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                    }
                    else if (playerDeck[turnNumber].DinosaurAge > AIDeck[AITurn].DinosaurAge)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = true;
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
                if (chosenValue == "strength")
                {
                    DrawOutline(new Rect(300, 500, 200, 20), "AI chose " + chosenValue, 2, myStyle);
                    if (playerDeck[turnNumber].DinosaurStrength < AIDeck[AITurn].DinosaurStrength)
                    {
                        turnStatus = "lost";
                        AIDeck.Add(playerDeck[turnNumber]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                AIDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = false;
                    }
                    else if (playerDeck[turnNumber].DinosaurStrength > AIDeck[AITurn].DinosaurStrength)
                    {
                        turnStatus = "won";
                        playerDeck.Add(AIDeck[AITurn]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        if (drawPile.Count > 0)
                        {
                            for (int i = 0; i < drawPile.Count; i++)
                            {
                                playerDeck.Add(drawPile[i]);
                            }
                            drawPile.Clear();
                        }
                        playersTurn = true;
                    }
                    else
                    {
                        turnStatus = "drew";
                        drawPile.Add(AIDeck[AITurn]);
                        drawPile.Add(playerDeck[turnNumber]);
                        AIDeck.Remove(AIDeck[AITurn]);
                        playerDeck.Remove(playerDeck[turnNumber]);
                    }
                    ++turnNumber;
                    ++AITurn;
                    if (turnNumber >= playerDeck.Count)
                    {
                        turnNumber = 0;
                    }
                    if (AITurn >= AIDeck.Count)
                    {
                        AITurn = 0;
                    }
                }
            }
            yield return new WaitForSeconds(3);
            for (int i = 0; i < playerDeck[turnNumber].DinosaurSize; i++)
            {
                DrawOutline(new Rect(0, 70, 200, 20), "Size:", 2, myStyle);
                GUI.Label(new Rect(50 + (30 * (i + 1)), 70, 30, 30), starIcon);
            }
            for (int i = 0; i < playerDeck[turnNumber].DinosaurSpeed; i++)
            {
                DrawOutline(new Rect(0, 120, 200, 20), "Speed:", 2, myStyle);
                GUI.Label(new Rect(50 + (30 * (i + 1)), 120, 30, 30), starIcon);
            }
            for (int i = 0; i < playerDeck[turnNumber].DinosaurAge; i++)
            {
                DrawOutline(new Rect(0, 170, 200, 20), "Age:", 2, myStyle);
                GUI.Label(new Rect(50 + (30 * (i + 1)), 170, 30, 30), starIcon);
            }
            for (int i = 0; i < playerDeck[turnNumber].DinosaurStrength; i++)
            {
                DrawOutline(new Rect(0, 220, 200, 20), "Strength:", 2, myStyle);
                GUI.Label(new Rect(50 + (30 * (i + 1)), 220, 30, 30), starIcon);
            }
            for (int i = 0; i < AIDeck[AITurn].DinosaurSize; i++)
            {
                DrawOutline(new Rect(250, 70, 200, 20), "Size:", 2, myStyle);
                GUI.Label(new Rect((280 + (30 * (i + 1))), 70, 30, 30), starIcon);
            }
            for (int i = 0; i < AIDeck[AITurn].DinosaurSpeed; i++)
            {
                DrawOutline(new Rect(250, 120, 200, 20), "Speed:", 2, myStyle);
                GUI.Label(new Rect((280 + (30 * (i + 1))), 120, 30, 30), starIcon);
            }
            for (int i = 0; i < AIDeck[AITurn].DinosaurAge; i++)
            {
                DrawOutline(new Rect(250, 170, 200, 20), "Age:", 2, myStyle);
                GUI.Label(new Rect((280 + (30 * (i + 1))), 170, 30, 30), starIcon);
            }
            for (int i = 0; i < AIDeck[AITurn].DinosaurStrength; i++)
            {
                DrawOutline(new Rect(250, 220, 200, 20), "Strength:", 2, myStyle);
                GUI.Label(new Rect((300 + (30 * (i + 1))), 220, 30, 30), starIcon);
            }
        }
    }
}