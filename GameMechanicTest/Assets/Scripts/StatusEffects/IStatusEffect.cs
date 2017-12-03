using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect {

	/// <summary>
	/// Increases the counter for the instance of the script.
	/// </summary>
	void Tick ();

	/// <summary>
	/// Runs checks to ensure the effect cannot stack (overrides stronger buff or half resets counter of stronger debuff if a stack is attempted)
	/// </summary>
	void PreApply ();

	/// <summary>
	/// Applies the buff/debuff to the list of Status Effects on the afflicted character.
	/// </summary>
	void ApplyEffect ();

	/// <summary>
	/// Applies the buff/debuff to the list of Status Effects on the afflicted character.
	/// </summary>
	void RemoveEffect ();

	/// <summary>
	/// Resets the counter to half of the max number of turns (if more than half the turns have passed).
	/// </summary>
	void ResetCounter ();

	/// <summary>
	/// Gets the magnitude of the buff/debuff.
	/// </summary>
	/// <returns>The magnitude.</returns>
	int GetMagnitude ();
}
