using Commons.Music.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchMacroPad
{
    public class LaunchPadManager
    {
        private IMidiAccess access;
        private IMidiInput input;
        private IMidiOutput output;

        public LaunchPadManager(IMidiAccess access, IMidiInput input, IMidiOutput output)
        {

            if(input == null || output == null)
            {
                throw new ArgumentException("Please choose the connected Launchpad S before going further");
            }

            this.access = access;
            this.input = input;
            this.output = output;

            StartListener();

        }

        private void StartListener()
        {

            Task.Run(async () =>
            {
                //for (int j = 0; j < 127; j++)
                //{
                for (int i = 0; i < 127; i++)
                {
                    output.Send(new byte[] { MidiEvent.NoteOn, (byte)i, 75 }, 0, 3, 0);
                }
                //}

                await Task.Delay(10);

                for (int i = 0; i < 127; i++)
                {
                    output.Send(new byte[] { MidiEvent.NoteOn, (byte)i, 0 }, 0, 3, 0);
                }
            });
        }

        public void SendNote(int note, int value)
        {
            Task.Run(() =>
            {
                output.Send(new byte[] { MidiEvent.NoteOn, (byte)note, (byte)value }, 0, 3, 0);
            });
        }
    }
}
