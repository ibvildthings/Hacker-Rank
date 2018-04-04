using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancedBrackets : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string s = "({[[]]}))))}]}()[]]{{[][]{{({(){[[]]}(()){{}{([])}}{()}()[][[{}]]}({})[])[]}{([])}{}[]}{}[]{{{{({{}}[])}}}()({}{[{}][][{[{[([])](){}}]}]([])})}[{}]}}{}[{}]])({})([{}[]]){{[][]}}})]()]()}}}{({[]}()[])}{}(()()[[{()[({{{}()}[]}{})[]]}]{{}{([[]{([])[{}]()}[[([])][[()[()[]{{[][(([()])){}]}}]{}[([]{([{}{}]{}{{}[([])]})()[]{((())())}{[{{}()}()]}})[({[[[[]]]{}]})]]]]]])}}])";
		string result = isBalanced(s);
		Debug.Log(result);
	}

	//function to check if it is a matching bracket
	bool isMatchingBracket(char bracketA, char bracketB){
		if (bracketA == '(' && bracketB == ')')
			return true;

		if (bracketA == '{' && bracketB == '}')
			return true;

		if (bracketA == '[' && bracketB == ']')
			return true;

		return false;
	}
	
	// Update is called once per frame
	string isBalanced (string s) {
		//check if there are even number of characters
		if (s.Length % 2 != 0)
			return "NO";

		//stack to process the string
		Stack <char> myStack = new Stack<char>();

		foreach (char bracket in s)
		{
			//if it is an opening bracket, push it in the stack
			if (bracket == '(' || bracket == '[' || bracket == '{') {
				//Debug.Log ("Pushing bracket - " + bracket);
				myStack.Push (bracket);
			}
			else {
				//if it is a closing bracket, we peek in the stack 
				//to see if it matches with the one on top of the stack
				char bracketOnTop = myStack.Peek();


				if (isMatchingBracket (bracketOnTop, bracket)) {
					myStack.Pop ();
				} else {
					return "NO";
				}
			}

		}

		if (myStack.Count == 0)
			return "YES";
		else
			return "NO";
	}
}
