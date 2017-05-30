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

        public LivePacketDeviceExtended(LivePacketDevice lpd)
        {
            LivePacketDevice = lpd;
        }

        public override string ToString()
        {
            return LivePacketDevice.Description;
        }
    }
}
