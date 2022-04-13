using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Character
{
    [System.Serializable]
    public struct Tweet
    {
        public string header;
        public string body;
    }

    public struct Dialogue
    {
        public string name;
        public List<string> slides;
    }

    private string mFileName;
    private XmlDocument xmlDoc;

    private List<Tweet> randomTweets;
    private List<Tweet> goodTweets;
    private List<Tweet> badTweets;
    private Dictionary<string, Dialogue> dialogueDict;
    private string profilePath;
    private string characterName;

    private int questsPassed = 0;
    private int questsFailed = 0;

    public Character( string fileName )
    {
        mFileName = fileName;

        randomTweets = new List<Tweet>();
        goodTweets = new List<Tweet>();
        badTweets = new List<Tweet>();

        dialogueDict = new Dictionary<string, Dialogue>();

    
        loadXML();
        parseXML();
    }

    private void loadXML()
    {
        xmlDoc = new XmlDocument();
        
        var xmlDocText = Resources.Load<TextAsset>( mFileName );
        xmlDoc.LoadXml( xmlDocText.text );
    }

    private void parseXML()
    {
        XmlNode character = xmlDoc.SelectSingleNode( "character" );
        characterName = character.SelectSingleNode( "name" ).InnerText;
        profilePath = character.SelectSingleNode( "sprite" ).InnerText;

        // Random tweets
        foreach( XmlElement node in xmlDoc.SelectNodes( "character/tweets/random/tweet" ) )
        {
            Tweet tempTweet = new Tweet();
            tempTweet.header = characterName;
            tempTweet.body = node.InnerText.Trim();

            randomTweets.Add( tempTweet );
        }

        foreach( XmlElement node in xmlDoc.SelectNodes( "character/dialogue/entry" ) )
        {
            Dialogue dialogue = new Dialogue();
            dialogue.name = characterName;
            dialogue.slides = new List<string>();
            foreach (XmlElement slide in node.SelectNodes( "dialogue/slide" ) )
            {
                dialogue.slides.Add(slide.InnerText.Trim());
            }
            
            dialogueDict.Add(node.SelectSingleNode( "key" ).InnerText, dialogue);
        }
    }

    public Tweet GetRandomTweet()
    {
        int randInt = Random.Range( 0, randomTweets.Count - 1 );
		return randomTweets[randInt];
    }

    public Dialogue GetDialogue(string key)
    {
        return dialogueDict[key];
    }
  
    public void passQuest()
    {
        questsPassed += 1;
    }

    public void failQuest()
    {
        questsFailed += 1;
    }

    public int getPassedQuests()
    {
        return ( questsPassed );
    }

    public int getFailedQuests()
    {
        return ( questsFailed );
    }

    public string getCharacterName()
    {
        return ( characterName );
    }
}
