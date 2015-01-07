using UnityEngine;
using System.Collections;
using Leap;

public abstract class GestureTracker {

	/****************
	 *  HelpMessage *
	 ****************/

	public struct HelpMessage{
		public string message;
		public Texture2D image;

		public HelpMessage(string msg, Texture2D img){
			this.message = msg;
			this.image = img;
		}
	}
	
	/****************
	 *  References  *
	 ****************/
	
	protected MainManager manager;
	private HelpMessage[] helpMessages;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public GestureTracker(MainManager mainManager, HelpMessage[] helpMessagesRef){
		this.manager = mainManager;
		this.helpMessages = helpMessagesRef;
	}
	
	/******************
	 *     Getter     *
	 ******************/

	public HelpMessage[] GetHelpMessages(){
		return helpMessages;
	}
	
	/******************
	 *  Declaration   *
	 ******************/
	
	public virtual void OnLoad (){}
	public abstract void OnUpdate ();
	public virtual void OnUnload (){}
	
}
