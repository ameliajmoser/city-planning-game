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
    private Sprite characterArt;
    private Sprite profileArt;

    private int questsPassed = 0;
    private int questsFailed = 0;

    private List<string> triggeredKeys;

    public Character( string fileName )
    {
        mFileName = fileName;

        randomTweets = new List<Tweet>();
        goodTweets = new List<Tweet>();
        badTweets = new List<Tweet>();

        triggeredKeys = new List<string>();

        dialogueDict = new Dictionary<string, Dialogue>();

    
        loadXML();
        parseXML();

        // Load character sprite
        characterArt = Resources.Load<Sprite>( "Portraits/" + profilePath );
        profileArt = Resources.Load<Sprite>( "Profiles/" + profilePath );
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

    public Sprite GetCharacterArt()
    {
        return ( characterArt );
    }

    public Sprite GetProfileArt()
    {
        return ( profileArt );
    }

    public string GetDialogueKey()
    {
        int total = questsPassed + questsFailed;

        if ( total == 0 )
        {
            return null;
        }

        float percentage = (float) questsPassed / total;
        if ( percentage < 0.75 )
        {
            // We have failed
            if ( total == 1 )
            {
                if ( triggeredKeys.Contains( "Fail1" ) )
                {
                    return null;
                }

                triggeredKeys.Add( "Fail1" );
                return "Fail1";
            }
            else if ( total == 2 )
            {
                if ( triggeredKeys.Contains( "Fail2" ) )
                {
                    return null;
                }

                triggeredKeys.Add( "Fail2" );
                return "Fail2";   
            }
        }

        return null;
    }

    public string GetEndingDialogueKey()
    {
        int total = questsPassed + questsFailed;

        if ( total == 0 )
        {
            return null;
        }

        float percentage = (float) questsPassed / total;
        if ( percentage < 0.75 )
        {
            return "BadEnding";
        }

        return "GoodEnding";
    }
}
