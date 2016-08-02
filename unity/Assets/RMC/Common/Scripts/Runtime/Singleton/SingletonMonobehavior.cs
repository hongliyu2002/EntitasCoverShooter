/**
 * Copyright (C) 2005-2015 by Rivello Multimedia Consulting (RMC).                    
 * code [at] RivelloMultimediaConsulting [dot] com                                                  
 *                                                                      
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the      
 * "Software"), to deal in the Software without restriction, including  
 * without limitation the rights to use, copy, modify, merge, publish,  
 * distribute, sublicense, and#or sell copies of the Software, and to   
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:                                            
 *                                                                      
 * The above copyright notice and this permission notice shall be       
 * included in all copies or substantial portions of the Software.      
 *                                                                      
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,      
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF   
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR    
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.                                      
 */
// Marks the right margin of code *******************************************************************


//--------------------------------------
//  Imports
//--------------------------------------
using UnityEngine;

//--------------------------------------
//  Namespace
//--------------------------------------
namespace RMC.Common.Singleton
{
	
	//--------------------------------------
	//  Namespace Properties
	//--------------------------------------
	
	
	//--------------------------------------
	//  Class Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	/// <summary>
	/// GOALS: Easily allow a Singleton to be added to hierarchy at runtime with full MonoBehavior access and predictable lifecycle.
	/// Usage:
	///
	/// First -- public class GameController : SingletonMonobehavior<GameController> 
	///
	/// Then -- GameController.Instance.SomePublicMethod(). Nice!
	/// </summary>
	public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		//	GETTER / SETTER
		/// <summary>
		/// Do not call this from another scope within OnDestroy(). Instead use IsInstantiated()
		/// </summary>
		private static T _Instance; //Harmless 'suggestion' appears here in some code-editors. Known issue.
		public static T Instance
		{

			//NOTE: Its recommended to wrap any calls to this getter with a IsInstanced() to prevent undesired instantiation. Optional.
			get
			{
				if (!IsInstantiated())
				{
					Instantiate();
				}
				return _Instance;
			}
			set
			{
				_Instance = value;
			}
			
		}


				
		/// <summary>
		/// 
		/// NOTE: Calling this will NEVER instantiate a new instance. That is useful and safe to call in any destructors / OnDestroy()
		/// 
		/// </summary>
		/// <returns><c>true</c> if is instantiated; otherwise, <c>false</c>.</returns>
		public static bool IsInstantiated()
		{
			return _Instance != null;
		}
		
		
		// 	PUBLIC

		public delegate void OnInstantiateCompletedDelegate (T instance);
		public static OnInstantiateCompletedDelegate OnInstantiateCompleted;

		public delegate void OnDestroyingDelegate (T instance);
		public static OnDestroyingDelegate OnDestroying;
		
		
		// 	PRIVATE
		
		
		//--------------------------------------
		// 	Constructor / Creation
		//--------------------------------------	

		
		/// <summary>
		/// Instantiate this instance. 
		/// 	1. Attempts to find an existing GameObject that matches (There will be 0 or 1 at any time)
		/// 	2. Creates GameObject with name of subclass
		/// 	3. Persists by default (optional)
		/// 	4. Predictable life-cycle.
		/// 
		/// </summary>
		public static T Instantiate ()
		{
			
			if (!IsInstantiated())
			{
				T t  = GameObject.FindObjectOfType<T>();
                GameObject go = null;
                if (t != null)
                {
                    go = t.gameObject;
                }

				if (go == null)
				{
					go 			= new GameObject ();
					_Instance 	= go.AddComponent<T>();
				}
				else
				{
					_Instance = go.GetComponent<T>();
				}
				
				go.name 		= _Instance.GetType().Name;
				DontDestroyOnLoad (go);

				if (OnInstantiateCompleted != null)
				{
					OnInstantiateCompleted (_Instance);
				}
			}
			return _Instance;
		}

		virtual protected void Awake()
		{
			Instantiate();
		}

		/// <summary>
		/// Destroys all memory/references associated with the instance
		/// </summary>
		public static void Destroy()
		{
			
			if (IsInstantiated())
			{
				if (OnDestroying != null)
				{
					OnDestroying (_Instance);
				}

				// NOTE: Use 'DestroyImmediate'. At runtime its less important, but occasionally editor classes will call Destroy();
				DestroyImmediate (_Instance.gameObject);

				_Instance = null;
			}
		}
		
		//--------------------------------------
		// 	Unity Methods
		//--------------------------------------
		
		
		//--------------------------------------
		// 	Methods
		//--------------------------------------

		//	PUBLIC


		
		//	PRIVATE
		protected virtual void OnDestroy ()
		{
			//override as needed
		}

		
		//--------------------------------------
		// 	Event Handlers
		//--------------------------------------
	}
}

