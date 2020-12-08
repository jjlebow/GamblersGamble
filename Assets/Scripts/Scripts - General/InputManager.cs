using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	public InputActionReference attack;
	public InputActionReference heavyAttack;

	private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of input Manager found");
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

	public InputActionReference FindReturnReference(string name)
	{
		if(name == "Attack")
		{
			Debug.Log("returningAttack");
			return attack;
		}
		else if(name == "HeavyAttack")
		{
			return heavyAttack;
		}
		else 
		{
			Debug.Log("returning null");
			return null;
		}
	}
}
