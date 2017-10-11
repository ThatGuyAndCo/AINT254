using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour{

	public static List<TurnObject> s_turnOrderList = new List<TurnObject> ();

	public static void CallNextTurn(){
		TurnObject l_nextTurn = new TurnObject(null, 0);
		int l_lowestDelay = int.MaxValue;

		for(int l_count = s_turnOrderList.Count - 1; l_count >= 0; l_count--){
			if (s_turnOrderList [l_count].c_delayValue < l_lowestDelay && s_turnOrderList[l_count].c_character != null) {
				l_lowestDelay = s_turnOrderList [l_count].c_delayValue;
				l_nextTurn = s_turnOrderList[l_count];
			}
		}

		l_nextTurn.c_character.SendMessage ("Attack");
	}
}

public class TurnObject{
	public GameObject c_character;
	public int c_delayValue;
	public int c_myIndex;

	public TurnObject(GameObject l_character, int l_initialDelay){
		c_character = l_character;
		c_delayValue = l_initialDelay;
	}
}
