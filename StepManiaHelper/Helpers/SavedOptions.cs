using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepManiaHelper.Helpers
{
    public enum EParseTypes
    {
        None,
        Unparsed,
        All
    }

    public enum EFolderTypes
    {
        Filter,
        CustomSongPack
    }

    public enum ESearchTypes
    {
        AND,
        OR
    }

    [Serializable]
    public class CSavedFolder
    {
        public string Name { get; set; }
        public EFolderTypes Type { get; set; }

        public CSavedFolder(string Name, EFolderTypes Type)
        {
            this.Name = Name;
            this.Type = Type;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class CSearchOperand
    {
        public string Property { get; set; }
        public string OpCode { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class CSavedSearch
    {
        public string Name { get; set; }
        public ESearchTypes Type { get; set; }
        public List<CSearchOperand> Operands { get; set; }

        public CSavedSearch(string Name, List<CSearchOperand> Operands = null)
        {
            this.Name = Name;
            this.Operands = Operands ?? new List<CSearchOperand>();
        }

        public void ReplaceWith(CSavedSearch Replacement)
        {
            this.Name = Replacement.Name;
            this.Type = Replacement.Type;
            this.Operands.AddRange(Replacement.Operands ?? new List<CSearchOperand>());
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class CSavedOptions
    {
        public string SongDirectory { get; set; }
        public Boolean SearchForNewSongs { get; set; }
        public Boolean DetectOnlyDisplayedData { get; set; }
        public Boolean IncludeAlreadyFiltered { get; set; }
        public Boolean LoadSaveFile { get; set; }
        public Boolean SaveFileAfterParse { get; set; }
        public EParseTypes ParseType { get; set; }

        public Boolean FilterExactDuplicates { get; set; }
        public Boolean FilterNonPad { get; set; }
        public Boolean FilterAltSongs { get; set; }
        public int FilterAltSongsIndex { get; set; }
        public Boolean FilterLiers { get; set; }
        public int FilterLiersIndex { get; set; }
        public Boolean FilterHard { get; set; }
        public int FilterHardValue { get; set; }
        public Boolean FilterFast { get; set; }
        public int FilterFastValue { get; set; }
        public Boolean FilterSlow { get; set; }
        public int FilterSlowValue { get; set; }
        public Boolean FilterVbpm { get; set; }
        public int FilterVbpmIndex { get; set; }
        public Boolean FilterStops { get; set; }
        public int FilterStopsValue { get; set; }

        public List<string> SongColumns { get; set; }
        public List<string> DiffColumns { get; set; }

        public List<CSavedFolder> Folders { get; set; }
        public List<CSavedSearch> Searches { get; set; }
    }
}
