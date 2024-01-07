using System.Runtime.InteropServices;
using UnityEngine;
using UI = UnityEngine.UI;


sealed class TextToSpeech : MonoBehaviour
{
    [SerializeField] public string _text;
    public MainAIMLScript botAnswer;

    public void Update()
    {
        if (botAnswer.questionAnswered != _text)
        {
            _text = botAnswer.questionAnswered;
            StartSpeech();
        }
    }

    public void StartSpeech()
      => ttsrust_say(_text);

    #if !UNITY_EDITOR && (UNITY_IOS || UNITY_WEBGL)
        const string _dll = "__Internal";
    #else
        const string _dll = "ttsrust";
    #endif

    [DllImport(_dll)] static extern void ttsrust_say(string text);

}
