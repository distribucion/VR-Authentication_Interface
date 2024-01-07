using System.IO;
using UnityEngine;
using HuggingFace.API;


namespace OpenAI
{
    public class OpenAIController : MonoBehaviour
    {
        public string inputField;

        private AudioClip clip;
        private byte[] bytes;
        private bool recording;


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartRecording();
                Debug.Log("Barra espaciadora pulsada");
            }

            if (recording && Microphone.GetPosition(null) >= clip.samples)
            {
                Debug.Log("Grabacion terminada");
                StopRecording();
            }
        }

        private void StartRecording()
        {
            clip = Microphone.Start(null, false, 5, 44100);
            recording = true;
        }

        private void StopRecording()
        {
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * clip.channels];
            clip.GetData(samples, 0);
            bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
            recording = false;
            SendRecording();
        }

        private void SendRecording()
        {
            HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
            {
                //text.color = Color.white;
                inputField = response;
                Debug.Log(response);

            }, error =>
            {
                //text.color = Color.red;
                inputField = error;
            });

        }


        private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
        {
            using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
            {
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write("RIFF".ToCharArray());
                    writer.Write(36 + samples.Length * 2);
                    writer.Write("WAVE".ToCharArray());
                    writer.Write("fmt ".ToCharArray());
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)channels);
                    writer.Write(frequency);
                    writer.Write(frequency * channels * 2);
                    writer.Write((ushort)(channels * 2));
                    writer.Write((ushort)16);
                    writer.Write("data".ToCharArray());
                    writer.Write(samples.Length * 2);

                    foreach (var sample in samples)
                    {
                        writer.Write((short)(sample * short.MaxValue));
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
