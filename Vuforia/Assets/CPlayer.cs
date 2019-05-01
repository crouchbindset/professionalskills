using System.Collections.Generic;
using System;

[Serializable]
public class CPlayer
{
    //Create a deck object, contains initial deck (list) setups
    readonly CDecks PlayerDecks = new CDecks();

    //Constructor
    public CPlayer(string name)
    {
        //Player object name
        PlayerName = name;
        //Player object score
        PlayerScore = 0;
        //Current deck (list) index
        DeckIndex = 0;
        //Turn status, 0 = none, 1 = won, 2 = lost, 3 = drew
        TurnStatus = 0;
        //Has the player chosen an attribute to play?
        HasChosen = false;
        //Is it currently this players turn?
        CurrentTurn = false;

        //Create decks (lists) of card type objects
        PlayerDinosaurs = PlayerDecks.Dinosaurs;
        PlayerVehicles = PlayerDecks.Vehicles;

        //Shuffle decks (lists) into random order
        Shuffle<CDinosaur>(PlayerDinosaurs);
        Shuffle<CVehicle>(PlayerVehicles);
    }

    //Shuffle a list into a random order
    public static void Shuffle<T>(IList<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var range = UnityEngine.Random.Range(i, count);
            var temp = list[i];
            list[i] = list[range];
            list[range] = temp;
        }
    }

    //Is it currently this players turn?
    public bool CurrentTurn { get; set;  }

    //Has the player chosen an attribute to play?
    public bool HasChosen { get; set;  }

    //Turn status getter and setter, 0 = none, 1 = won, 2 = lost, 3 = drew
    public int TurnStatus { get; set; }

    //Deck index getter and setter
    public int DeckIndex { get; set; }

    //Score getter and setter
    public int PlayerScore { get; set; }

    //Name getter
    public string PlayerName { get; }

    //Dinosaur deck (list) getter
    public List<CDinosaur> PlayerDinosaurs { get; }

    //Vehicle deck (list) getter
    public List<CVehicle> PlayerVehicles { get; }
}