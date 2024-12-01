using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace StepManiaHelper
{
    public abstract class CFilterLogic
    {
        protected static string strStepmaniaSongFolderPath;

        static CFilterLogic()
        {
            CFilterLogic.strStepmaniaSongFolderPath = "";
        }

        public CFilterLogic()
        {

        }

        internal virtual void Filter(Options OutputForm, List<CSong> lstSongs)
        {

        }

        public virtual string GetResultString()
        {
            return "";
        }

        static public void SetStepmaniaSongFolderPath(string strFolderPath)
        {
            CFilterLogic.strStepmaniaSongFolderPath = strFolderPath;
        }

        static public void ClearSongFlags(List<CSong> lstSongs)
        {
            // Loop through all the parsed songs
            foreach (CSong ParsedSong in lstSongs)
            {
                ParsedSong.bFlagged = false;
                ParsedSong.bAlreadyScanned = false;
            }
        }
    }
}
