using System.Linq;
using Archivarius.Algorithms;

namespace Archivarius.Utils.Managers
{
    public class AlgorithmManager
    {
        private readonly Algorithm[] algorithms;

        public AlgorithmManager(Algorithm[] algorithms)
        {
            this.algorithms = algorithms;
        }
        
        public Algorithm GetAlgorithmByType(AlgorithmType type) => algorithms.First(algorithm => algorithm.Type.Equals(type));

        public Algorithm GetAlgorithmByExtension(string extension) =>
            algorithms.First(algorithm => algorithm.IsResolveFileType(extension));
    }
}