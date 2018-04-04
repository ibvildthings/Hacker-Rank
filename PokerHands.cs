using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PokerHands : MonoBehaviour{

	// Use this for initialization
	void Start () {
		Hand PlayerOne, PlayerTwo;

		PlayerOne.hand = "4D 6S 9H QH TC"; 
		PlayerTwo.hand = "3D 6D 7H KD QS";


		int result = PlayerOne.CompareTo (PlayerTwo);

		if (result == 1)
			Debug.Log ("Player 1");
		else if (result == -1)
			Debug.Log ("Player 2");
		else
			Debug.Log ("Draw");
	}

	//this struct hand contains the cards and derives properties
	//such as whether it is a flush or not
	struct Hand: IComparable <Hand> {

		//method to implement IComparable
		//compares the two hands and delivers a verdict
		public int CompareTo (Hand anotherHand)
		{

			//Debug.Log("Royal Flush");
			int result = CompareHands (anotherHand, royalFlush, anotherHand.royalFlush);
			if (result != 0) return result;

			//Debug.Log("Straight Flush");
			result = CompareHands (anotherHand, straightFlush, anotherHand.straightFlush);
			if (result != 0) return result;

			//Debug.Log("Four of a kind");
			result = CompareHands (anotherHand, fourOfAKind, anotherHand.fourOfAKind);
			if (result != 0) return result;

			//Debug.Log("Full House");
			result = CompareHands (anotherHand, fullHouse, anotherHand.fullHouse);
			if (result != 0) return result;

			//Debug.Log("Flush");
			result = CompareHands (anotherHand, flush, anotherHand.flush);
			if (result != 0) return result;

			//Debug.Log("Straight");
			result = CompareHands (anotherHand, straight, anotherHand.straight);
			if (result != 0) return result;

			//Debug.Log("3 of a Kind");
			result = CompareHands (anotherHand, threeOfAKind, anotherHand.threeOfAKind);
			if (result != 0) return result;

			//Debug.Log("2 Pairs");
			result = CompareHands (anotherHand, twoPairs, anotherHand.twoPairs);
			if (result != 0) return result;

			//Debug.Log("A Pair");
			result = CompareHands (anotherHand, onePair, anotherHand.onePair);
			if (result != 0) return result;

			//Debug.Log("High Card");
			return tieBreaker (anotherHand);


		}//end CompareTo




		//compares two bools and returns an int
		int CompareHands(Hand handTwo, bool propertyOne, bool propertyTwo )
		{
			if (propertyOne && !propertyTwo)	return 1;
			if (!propertyOne && propertyTwo)	return -1;
			if (propertyOne && propertyTwo)	
				return tieBreaker (handTwo);

			return 0; //if all else fails
		} //end compareHands



		//used to resolve ties by comparing the rank value of all the cards
		int tieBreaker(Hand Two)
		{
			Hand One = this;

			int range = One.ranksByFrequency.Count;

			//use this to make sure the index doesn't go out of bounds
			if (range > Two.ranksByFrequency.Count)
				range = Two.ranksByFrequency.Count;

			for (int i = 0; i < range; i++) {
				if (One.ranksByFrequency [i] > Two.ranksByFrequency [i])
					return 1;

				if (One.ranksByFrequency [i] < Two.ranksByFrequency [i])
					return -1;
			}

			return 0;

		} //end tiebreaker


		/*--------------------- Various Properties ------------------*/

		public string hand; //eg. "5C, 5D, 7S, 6H, 13C";

		//Rank the cards from highest to lowest
		//convert J, Q, K and Ace into numbers
		// eg. returns a list [13, 7, 6, 5, 5];
		List<int> rank 
		{
			get	{
				string [] cards = hand.Split(' ');
				List<int> _rank = new List<int> ();

				for(int i = 0; i < cards.Length; i++) {
					//remove the suit of the card and only store the value
					string _card = cards[i].Remove(cards[i].Length - 1); 

					//convert from a string to an int representation
					//convert J, Q, K and Ace into numbers
					switch (_card)
					{
					case "1":
					case "2":
					case "3":
					case "4":
					case "5":
					case "6":
					case "7":
					case "8":
					case "9":
					case "10":
						_rank.Add( Int32.Parse (_card) );
						break;
					case "T":
						_rank.Add (10);
						break;
					case "J":
						_rank.Add (11);
						break;
					case "Q":
						_rank.Add (12);
						break;
					case "K":
						_rank.Add (13);
						break;
					case "A":
						_rank.Add (14);
						break;
					} //end switch


				}//end for

				_rank.Sort ();
				_rank.Reverse();
				return _rank; //return sorted list in descending order
			}
		}


		//returns the highest card in the hand
		int highCard
		{
			get{
				//the ranks are stored in a descending order
				return rank [0];
			}
		}


		//Club - 1, Diamond - 2, Heart - 3, Spade - 4
		//returns a list of the suits of the cards
		// eg. [2, 3, 1, 1]
		int[] suit
		{
			get{
				
				string[] cards = hand.Split (' ');

				//an array to store the suits
				int[] _suit = new int[cards.Length];

				for (int i = 0; i < cards.Length; i++) {
					//this discards the rank and gives us the suit
					char card = cards [i][cards [i].Length - 1];

					//convert strings into an int representation
					switch (card) {
					case 'C':
						_suit [i] = 1;
						break;
					case 'D':
						_suit [i] = 2;
						break;
					case 'H':
						_suit [i] = 3;
						break;
					case 'S':
						_suit [i] = 4;
						break;
					default:
						_suit [i] = 0;
						break;
					}

				}

				return _suit;
			}
		}	//end suit


		//method to print the suit of the card
		//used for debugging
		public void printSuit()
		{
			foreach(int card in suit)
				Debug.Log(card);
		}


		//this returns the rank followed by its frequency
		//eg. [13:1, 7:1, 6:1, 5:2]
		Dictionary<int, int> rankFrequency {
			
			get{
				Dictionary<int, int> _rankFrequency = new Dictionary<int, int> ();

				//array with only the distinct ranks
				List<int> distinctRanks = rank.Distinct().ToList();

				//count the frequency for every rank
				foreach(int _distinctRank in distinctRanks)
				{
					int frequency_count = 0;

					foreach (int l_rank in rank) {
						if (_distinctRank == l_rank)
							frequency_count++;	
					}
					_rankFrequency.Add (_distinctRank, frequency_count);
				}

				return _rankFrequency;

			}	// end get
		}	//end rankFrequency

		//returns ranks ordered by frequency and then rank value in descending order
		//eg. [5, 13, 7, 6]
		List<int> ranksByFrequency {

			get{
				List<int> _ranksByFrequency = new List<int> ();

				//the max frequency can be 5
				for(int i = 5; i > 0; i--){
					foreach (KeyValuePair<int, int> _rankFrequency in rankFrequency) {
						if (_rankFrequency.Value == i)
							_ranksByFrequency.Add (_rankFrequency.Key);
					}
				}

				return _ranksByFrequency;

			}	// end get
		}	//end rankFrequency




		/*------------------- Test Methods -----------------------*/

		//test to see if there is a pair in the hand
		bool onePair {

			get{
				
				foreach(KeyValuePair<int, int> _frequencyRank in rankFrequency)	{
					if (_frequencyRank.Value == 2)
						return true;
				}//end for each

				return false;
			}
		}

		//test to see if there are 2 pairs in the hand
		bool twoPairs {

			get{

				int count = 0;

				foreach(KeyValuePair<int, int> _frequencyRank in rankFrequency)	{
					if (_frequencyRank.Value >= 2)
						count++;
				}//end for each

				if(count > 1)
					return true;
				else
					return false;

			}
		}

		//test to see if there are 3 of a kind in the hand
		bool threeOfAKind {

			get{
				foreach(KeyValuePair<int, int> _frequencyRank in rankFrequency)	{
					if (_frequencyRank.Value == 3)
						return true;
				}//end for each

				return false;
			}
		}

		//test to see if there is a straight in the hand
		bool straight {
			
			get{ 
				for (int i = rank [0]; i > rank [0] - 5; i = i - 1) {
					//return false if rank doesn't contain every element from the max to max - 5
					if (!rank.Contains (i))
						return false;
				}
				return true;
			}
		}

		//test to see if there is a flush in the hand
		bool flush {
			
			get {
				//list with only the distinct elements
				List<int> distinctSuits = suit.Distinct ().ToList ();

				if (distinctSuits.Count == 1)
					return true;
				else
					return false;
			}
		}

		//test to see if there is a full house in the hand
		bool fullHouse {
			
			get {
				if (threeOfAKind && onePair)
					return true;
				else
					return false;
			}
		}

		//test to see if there are four of a kind in the hand
		bool fourOfAKind {
			
			get {
				foreach(KeyValuePair<int, int> _frequencyRank in rankFrequency)	{
					if (_frequencyRank.Value == 4)
						return true;
				}

				return false;
			}
		}

		//test to see if there is a straight flush in the hand
		bool straightFlush {
			
			get {
				if (flush && straight)
					return true;
				else
					return false;
			}
		}

		//test to see if there is a royal flush in the hand
		bool royalFlush {
			
			get {
				//in a royal flush the highest card will be an Ace
				if (highCard != 14)
					return false;

				//if there is an Ace, check if there is a straight flush
				return straightFlush;
			}
		}

	}
}
