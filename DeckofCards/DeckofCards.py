import random

class Card:
    """
    A class representing a card.
    Attributes:
        rank: The rank of the card.
        suit: The suit of the card.
    """

    def __init__(self, rank, suit):
        self.rank = rank
        self.suit = suit

    def __str__(self):
        return f"{self.rank} of {self.suit}"

class Deck:
    """
    A class representing a deck of playing cards.
    Attributes:
        deck: A list of Card objects.
    """

    def __init__(self):
        ranks = ["Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"]
        suits = ["Clubs", "Diamonds", "Hearts", "Spades"]
        self.deck = [Card(rank, suit) for rank in ranks for suit in suits]

    def shuffle(self):
        """
        Shuffles the deck of cards.
        """
        random.shuffle(self.deck)

    def deal(self):
        """
        Deals a card from the deck.

        Returns:
            The dealt card.
        """
        if not self.deck:
            raise IndexError("Deck is empty")
        return self.deck.pop()

    def count(self):
        """
        Returns the number of cards remaining in the deck.
        """
        return len(self.deck)

def main():
    print("Card Dealer\n")

    deck = Deck()
    deck.shuffle()
    print(f"I have shuffled a deck of {deck.count()} cards.\n")

    while True:
        try:
            num_cards = int(input("How many cards would you like?: "))
            if num_cards <= 0:
                print("You need to draw at least one card.\n")
                continue
            break
        except ValueError:
            print("Please enter a valid number.\n")

    print("\nHere are your cards:")
    for _ in range(num_cards):
        card = deck.deal()
        print(card)

    print(f"\nThere are {deck.count()} cards left in the deck.\n")

    print("Good luck!")

if __name__ == "__main__":
    main()

