using System;
using Vosk;
using NAudio.Wave;

namespace VoiceAssistant
{
    public static class SpeechRecognition
    {
        public static string RecognizeSpeech()
        {
            Vosk.Vosk.SetLogLevel(0);
           var modelPath = Environment.GetEnvironmentVariable("VOSK_MODEL_PATH") ?? throw new InvalidOperationException("VOSK_MODEL_PATH environment variable is not set.");
            var model = new Model(modelPath);

            using (var waveIn = new WaveInEvent())
            {
                waveIn.WaveFormat = new WaveFormat(16000, 1);
                var rec = new VoskRecognizer(model, 16000.0f);

                waveIn.DataAvailable += (s, e) =>
                {
                    if (rec.AcceptWaveform(e.Buffer, e.BytesRecorded))
                    {
                        Console.WriteLine(rec.Result());
                    }
                    else
                    {
                        Console.WriteLine(rec.PartialResult());
                    }
                };

                waveIn.StartRecording();
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
                waveIn.StopRecording();
            }

            return "Recognition complete";
        }
    }
}

