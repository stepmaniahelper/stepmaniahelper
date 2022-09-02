using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    [Serializable]
    public class CBpmSegment
    {
        public int nBeat;
        public double fBpm;
        public int nDurationInBeats;
        public double fDurationInMinutes;

        public CBpmSegment(int nBeat, double fBpm)
        {
            this.nBeat = nBeat;
            this.fBpm = fBpm;
            this.nDurationInBeats = 0;
            this.fDurationInMinutes = 0.0;
        }
    }
}
