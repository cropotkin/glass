using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NAudio;
using NAudio.Wave;

namespace glass
{
    class Program
    {
        static void Main(string[] aArgs)
        {
            var program = new Program();

            try
            {
                program.Run(aArgs);
            }
            catch (ApplicationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Program()
        {
        }

        public void Run(string[] aArgs)
        {
            if (aArgs.Length != 2)
            {
                Help();

                return;
            }

            var samples = LoadSamples(aArgs[0]);


        }

        private void Help()
        {
            Console.WriteLine("glass [inputfile] [outputfiel]");
        }

        private IEnumerable<float[]> LoadSamples(string aPath)
        {
            switch (Path.GetExtension(aPath).ToLowerInvariant())
            {
                case "wav":
                    return LoadSamplesWav(aPath);
                case "mp3":
                    return LoadSamplesMp3(aPath);
                default:
                    throw new ApplicationException("audio file not recognised");
            }
        }

        private IEnumerable<float[]> LoadSamplesWav(string aPath)
        {
            using (var reader = new WaveFileReader(aPath))
            {
                var frames = new List<float[]>();

                while (true)
                {
                    var frame = reader.ReadNextSampleFrame();

                    if (frame == null)
                    {
                        return frames;
                    }

                    frames.Add(frame);
                }
            }
        }

        IEnumerable<float[]> DecompressMp3(Mp3Frame aFrame)
        {
            /*
            WaveFormat waveFormat = new Mp3WaveFormat(aFrame.SampleRate, aFrame.ChannelMode == ChannelMode.Mono ? 1 : 2, aFrame.FrameLength, aFrame.BitRate);

            var decompressor = new AcmMp3FrameDecompressor(waveFormat);

            var bufferedWaveProvider = new BufferedWaveProvider(decompressor.OutputFormat);
            
            int decompressed = decompressor..DecompressFrame(aFrame,, buffer, 0);
            */

            yield return new float[] { 0, 0 };
        }


        private IEnumerable<float[]> LoadSamplesMp3(string aPath)
        {
            using (var reader = new Mp3FileReader(aPath))
            {
                var frames = new List<float[]>();

                while (true)
                {
                    var frame = reader.ReadNextFrame();

                    if (frame == null)
                    {
                        return frames;
                    }

                    frames.AddRange(DecompressMp3(frame));
                }
            }
        }
    }
}
