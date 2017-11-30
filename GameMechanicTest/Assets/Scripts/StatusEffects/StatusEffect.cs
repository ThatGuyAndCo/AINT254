using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect {

	public int c_counter;

	protected virtual void Tick();

	protected virtual void ApplyEffect ();

	protected virtual void RemoveEffect ();

	protected virtual void ResetCounter ();
}
