using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore
{
    public class LivePacketDeviceExtended
    {
        public LivePacketDevice LivePacketDevice { get; set; }
        public int Index { get; set; }

        public LivePacketDeviceExtended(LivePacketDevice lpd, int index)
        {
            LivePacketDevice = lpd;
            Index = index;
        }

        public override string ToString()
        {
            return LivePacketDevice.Description;
        }

        private static List<LivePacketDeviceExtended> mAllLocalMachine = null;
        public static List<LivePacketDeviceExtended> AllLocalMachine
        {
            get
            {
                if (mAllLocalMachine == null)
                {
                    mAllLocalMachine = new List<LivePacketDeviceExtended>();
                    for (int i = 0; i < LivePacketDevice.AllLocalMachine.Count; i++)
                    {
                        mAllLocalMachine.Add(new LivePacketDeviceExtended(LivePacketDevice.AllLocalMachine[i], i));
                    }
                }
                return mAllLocalMachine;
            }
        }
    }
}
