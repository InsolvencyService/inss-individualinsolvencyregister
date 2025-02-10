namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class ExtractVolumes
    {
        private int _totalEntries;
        private int _totalBanks;
        private int _totalIVAs;
        private int _newBanks;
        private int _totalDros;

        public ExtractVolumes() 
        { 
            _totalEntries = 0;
            _totalBanks = 0;
            _totalIVAs = 0;
            _newBanks = 0;
            _totalDros = 0;
        }

        public int TotalEntries { get => _totalEntries; internal set => _totalEntries = value; }
        public int TotalBanks { get => _totalBanks; internal set => _totalBanks = value; }
        public int TotalIVAs { get => _totalIVAs; internal set => _totalIVAs = value; }
        public int NewBanks { get => _newBanks; internal set => _newBanks = value; }
        public int TotalDros { get => _totalDros; internal set => _totalDros = value; }
    }
}