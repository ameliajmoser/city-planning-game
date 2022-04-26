using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Thanks for code from https://forum.unity.com/threads/solved-how-would-you-implement-phone-text-messenger-style-ui.692311/
// The dialogue manager is resposible for adding new dialogue boxes to the conversion when requested
public class DialogueManager : MonoBehaviour {

	[SerializeField]
	private DialoguePopup dialoguePopup;

	[SerializeField]
	private GameObject dialogueContainer;

	[SerializeField]
	private GameObject textBoxPrefab;

	[Range(1.0f, 60.0f)]
    [SerializeField]
	private float interval = 10.0f;

	[SerializeField]
	private int postSpacing = 5;

	RectTransform containerRectTrans;

	// Keeps track of the transform of the previous addition
	private RectTransform lastRectTrans = null;

	// Stop coroutine
	private bool _stopSpawn;

	// List of characters to select tweet from
	private List<Character> characters;

	private List<Quest.Message> messages;

	void Awake ()
	{
		// Init characters
		characters = new List<Character>();
		// The string we pass to characters is the xml file name minus the extension
		characters.Add( new Character( "Steve" ) ); // concerned citizen 1
		characters.Add( new Character( "David" ) ); // not for profit leader
		characters.Add( new Character( "Betsy" ) ); // school board president
		characters.Add( new Character( "Gerald" ) ); // union leader
		characters.Add( new Character( "Mayor" ) ); //mayor marshall
		characters.Add( new Character( "Gulliver" ) ); // oil baron
		characters.Add( new Character( "Nancy" ) ); // concerned citizen 2
		characters.Add( new Character( "Marcia" ) ); // celebrity 1
		characters.Add( new Character( "Mark" ) ); //tech guy
		characters.Add( new Character( "Weather-Channel" ) ); // city weather channel
		characters.Add( new Character( "Sports-Channel" ) ); // city sports channel
		characters.Add( new Character( "Blank" ) );

		// Messages
		messages = new List<Quest.Message>();

		// Dialogue container
		containerRectTrans = dialogueContainer.GetComponent<RectTransform>();

		// Start coroutine
		_stopSpawn = false;
		StartCoroutine( SpawnDialogue() );
	}

	// Spawn dialogue box every x seconds
	private IEnumerator SpawnDialogue()
	{
		while ( !_stopSpawn )
		{
			// Check if not null
			if ( textBoxPrefab )
			{
				if ( messages.Count > 0 )
				{
					pushMessage( messages[0] );
					messages.RemoveAt( 0 );
				}
				else
				{
					Character character = GetRandomCharacter();
					Character.Tweet tweet = character.GetRandomTweet();

					AddDialogueBox( character, tweet );
				}
			}

			yield return new WaitForSeconds( interval );
		}
	}

	// Add box
	void AddDialogueBox( Character character, Character.Tweet tweet )
	{
		RectTransform containerRectTrans = dialogueContainer.GetComponent<RectTransform>();

		GameObject newBox = Instantiate(textBoxPrefab, dialogueContainer.transform, false) as GameObject;
		RectTransform newRectTrans = newBox.GetComponent<RectTransform>();

		// If this isn't the first dialog box being added
		if (lastRectTrans != null)
		{
			Vector2 newPos = new Vector2(lastRectTrans.localPosition.x, 
										 lastRectTrans.localPosition.y - (newRectTrans.rect.height+postSpacing));

			newRectTrans.localPosition = newPos;

		}

		lastRectTrans = newRectTrans;

		CheckContainerLength();

		// Update text
		var dialogue = newBox.GetComponent<TweetDialogue>();
		dialogue.messageHeader.text = tweet.header;
		dialogue.messageBody.text = tweet.body;
		dialogue.profile.sprite = character.GetProfileArt();

		// Push scroll bar to bottom of feed
		GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
	}

	// Checks if the dialogue items have ran off the container length and adjusts accordingly
	void CheckContainerLength()
	{
		// If the last item goes off the bottom edge of the container
		if (containerRectTrans.rect.y > lastRectTrans.localPosition.y)
		{	
			float extendDistance = Mathf.Abs(lastRectTrans.rect.y) + lastRectTrans.rect.height/2;
			
			// Resizing the container extends it in both directions, so we must reposition it accordingly 
			Vector2 newPos = new Vector2(containerRectTrans.localPosition.x, 
										 containerRectTrans.localPosition.y - extendDistance/2);

			containerRectTrans.sizeDelta = new Vector2(0f, containerRectTrans.sizeDelta.y + extendDistance);
			containerRectTrans.localPosition = newPos;
		}
	}

	// Set spawn bool value
	public void SetSpawn( bool stopSpawn )
	{
		_stopSpawn = stopSpawn;
	}

	private Character GetRandomCharacter()
	{
		int randInt = Random.Range( 0, characters.Count - 1 );
		return characters[randInt];
	}

	public List<Character> getCharacters()
	{
		return ( characters );
	}

	public void pushMessage( Quest.Message message )
	{	
		Character character = getCharacter( message.character );

		if ( character != null )
		{
			Character.Tweet tweet = message.tweet;
			AddDialogueBox( character, tweet );
		}
	}

	public void AddMessage( Quest.Message message )
	{
		messages.Add( message );
	}

	public Character getCharacter( string name )
	{
		foreach( Character character in characters )
		{	
			if ( character.getCharacterName().Equals( name ) )
			{
				return character;
			}
		}

		return null;
	}

	public void QueueDialoguePopup( Character character, string key )
	{
		dialoguePopup.QueueDialoguePopup( character, key );
	}

	public bool DialoguePopupOpen()
	{
		return ( dialoguePopup.IsOpen() );
	}
}
