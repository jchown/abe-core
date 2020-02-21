using System.Threading.Tasks;

namespace abe.core
{
    public interface IScanner
    {
        bool CanScanFile(string filename);

        FileNameReferences ScanFile(string filename);
    }
}