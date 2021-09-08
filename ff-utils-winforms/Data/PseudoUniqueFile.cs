namespace Nmkoder.Data
{
    class PseudoUniqueFile
    {
        public string path;
        public long filesize;

        public PseudoUniqueFile (string pathArg, long filesizeArg)
        {
            path = pathArg;
            filesize = filesizeArg;
        }
    }
}
