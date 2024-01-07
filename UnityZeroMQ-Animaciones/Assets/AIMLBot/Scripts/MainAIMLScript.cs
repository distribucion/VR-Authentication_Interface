using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainAIMLScript : MonoBehaviour
{

    private TextAsset[] aimlFiles;
    private List<string> aimlXmlDocumentListFileName = new List<string>();
    private List<XmlDocument> aimlXmlDocumentList = new List<XmlDocument>();
    
    private TextAsset GlobalSettings, GenderSubstitutions, Person2Substitutions, PersonSubstitutions, Substitutions, DefaultPredicates, Splitters;
    
    private ChatbotMobileWeb bot;
    //public InputField OutputBox;

    private string questionToAsk = "";
    public float timeRemaining;
    public string questionAnswered;

    void Start()
    {
        bot = new ChatbotMobileWeb();
        LoadFilesFromConfigFolder();
        bot.LoadSettings(GlobalSettings.text, GenderSubstitutions.text, Person2Substitutions.text, PersonSubstitutions.text, Substitutions.text, DefaultPredicates.text, Splitters.text);
        TextAssetToXmlDocumentAIMLFiles();
        bot.loadAIMLFromXML(aimlXmlDocumentList.ToArray(), aimlXmlDocumentListFileName.ToArray());
        bot.LoadBrain();
        //Debug.Log("Start testing");
        //StartCoroutine("TesterCoroutine");
    }
    /*private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Start testing");
            StartCoroutine("TesterCoroutine");


            timeRemaining = 50;
        }
    }*/

    IEnumerator TesterCoroutine()
    {
        //questionToAsk = voiceMessage.inputField;
        Debug.Log(questionToAsk);
        SendQuestionToRobot();
        yield return new WaitForSeconds(1f);
    }

    public void SendQuestionToBot(string question)
    {
        Debug.Log("[MainAIMLScript][]SendQuestionToBot] Recibida pregunta");
        questionToAsk = question;
        StartCoroutine("SendQuesToBot");
    }
    /// <summary>
    /// Button to send the question to the robot
    /// </summary>
    public void SendQuestionToRobot()
    {
        StartCoroutine("SendQuesToBot");
    }

    IEnumerator SendQuesToBot()
    {
        //if (string.IsNullOrEmpty(InputBox.text) == false)
        //{
        // Response Bot AIML
        //Debug.Log(InputBox.text);
        //var answer = bot.getOutput(InputBox.text);
        Debug.Log("PreguntaBot: " + questionToAsk);
            var answer = bot.getOutput(questionToAsk);
            //InputBox.text = string.Empty;

            //OutputBox.text = ("Typing.");
            //yield return new WaitForSeconds(0.3f);
            //OutputBox.text = ("Typing..");
            //yield return new WaitForSeconds(0.6f);
            //OutputBox.text = ("Typing...");
            yield return new WaitForSeconds(0.3f);

            // Response BotAIml in the Chat window
            questionAnswered = answer;
            Debug.Log("RespuestaBot: " + questionAnswered);
        //}
    }

    void LoadFilesFromConfigFolder()
    {
        GlobalSettings = Resources.Load<TextAsset>("config/Settings");
        GenderSubstitutions = Resources.Load<TextAsset>("config/GenderSubstitutions");
        Person2Substitutions = Resources.Load<TextAsset>("config/Person2Substitutions");
        PersonSubstitutions = Resources.Load<TextAsset>("config/PersonSubstitutions");
        Substitutions = Resources.Load<TextAsset>("config/Substitutions");
        DefaultPredicates = Resources.Load<TextAsset>("config/DefaultPredicates");
        Splitters = Resources.Load<TextAsset>("config/Splitters");
    }

    void TextAssetToXmlDocumentAIMLFiles()
    {
        aimlFiles = Resources.LoadAll<TextAsset>("aiml");
        foreach (TextAsset aimlFile in aimlFiles)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(aimlFile.text);
                aimlXmlDocumentListFileName.Add(aimlFile.name);
                aimlXmlDocumentList.Add(xmlDoc);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }


    void OnDisable()
    {
        bot.SaveBrain();
    }


}
