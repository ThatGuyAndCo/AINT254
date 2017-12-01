using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StatusEffect {

	void Tick ();

	void ApplyEffect ();

	void RemoveEffect ();

	void ResetCounter ();
}
