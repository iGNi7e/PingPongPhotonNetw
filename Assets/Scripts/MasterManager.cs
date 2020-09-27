using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableObject
{
	#region Singletone declaration

	private static MasterManager _instance = null;

	public static MasterManager Instance
	{
		get
		{
			if (_instance == null)
			{
				MasterManager[] results = Resources.FindObjectsOfTypeAll<MasterManager>();
				if (results.Length == 0)
				{
					Debug.LogError("Length of instance Master Manager is null!");
					return null;
				}
				if ( results.Length > 1 ) {
					Debug.LogError( "Length of instance Master Manager is " + results.Length );
					return null;
				}

				_instance = results[0];
			}

			return _instance;
		}
	}

	#endregion

	[SerializeField] private GameSettings _gameSettings;

	public static GameSettings GameSettings => Instance._gameSettings;
}
