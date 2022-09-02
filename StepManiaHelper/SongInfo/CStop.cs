using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    [Serializable]
    public class CStop
    {
        public int nBeat;
        public double fDurationInMs;

        public CStop(int nBeat, double fDurationInMs)
        {
            this.nBeat = nBeat;
            this.fDurationInMs = fDurationInMs;
        }
    }
}
