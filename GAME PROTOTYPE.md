# Game Prototype Details

## Game Themes

The game will have two themes, for example; Dinosaurs, Animals, Cars, Super Heroes

Each card within a theme will have a set of categories in which it will have values which will be used to compared cards against one another.

For simplicity, the card values should be a star rating from 1-5*.

### Animals/Dinosaurs

* Lifespan (On average how long do they live?)
* Speed (Top speed?)
* Strength (Relative strength?)
* Size (How large can they be?)

### Vehicles

* Cost (How expensive can they be?)
* Speed (How fast can they go?)
* Size (How big are they?)
* Power (Relative power, something like horsepower?)

## Game Cards

Each player will have a card which will augment their current playable card.

Each card will belong to a theme (see above), and will have its own values for each category. This will need to be associated with the game object with the model.

## Game Modes

The application could have two modes.

### Top Trumps

[Top Trumps Wiki](https://en.wikipedia.org/wiki/Top_Trumps)

1. First player chooses a category to play from their top most card, and compares this value with their opponent.
2. The player whose card has the best (usually highest) value wins both cards and places them at the bottom of their deck.
3. In case of a draw, both cards are placed in the middle and the first player picks a new category from their next card.
4. The winner of the previous round is the first player of the next round.
5. Players are eliminated when they lose their last card, and the winner is the player who obtains the whole pack.

### Quick head-to-head

1. First player picks a category and compares it with opponent.
2. Player with the best score on their card wins and gains +1 point and both cards are discarded.
3. Next player then picks a category from their topmost card.
4. Continue until predetermined number of rounds are reached or players decide to end game.
