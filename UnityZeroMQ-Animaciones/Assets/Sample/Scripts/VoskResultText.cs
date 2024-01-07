using UnityEngine;
using UnityEngine.UI;
public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    
    public MainAIMLScript mainAIMLScript;
    //public Text ResultText;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        Debug.Log(obj);
        string textResult = "Recognized: ";
        string questionToAsk = "";
        //ResultText.text = "Recognized: ";
        var result = new RecognitionResult(obj);
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                textResult += "\n ---------- \n";
            }

            textResult += result.Phrases[0].Text + " | " + "Confidence: " + result.Phrases[0].Confidence;
            questionToAsk = result.Phrases[0].Text;
        }
        Debug.Log(textResult);
        mainAIMLScript.SendQuestionToBot(questionToAsk);
    }
}
