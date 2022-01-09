using System;
using System.Collections.Generic;
using System.Linq;
using Archivarius.Algorithms;

namespace Archivarius.Utils.Managers
{
    public class AlgorithmManager
    {
        private readonly AbstractAlgorithm[] algorithms;
        public AlgorithmManager(AbstractAlgorithm[] algorithms) => this.algorithms = algorithms;

        public AbstractAlgorithm GetAlgorithmByType(AlgorithmType type)
        {
            var algorithm = algorithms.FirstOrDefault(algorithm => algorithm.Type.Equals(type));
            return algorithm ?? throw new ArgumentException("You use wrong algorithm type");
        }

        public AbstractAlgorithm GetAlgorithmByExtension(string extension)
        {
            var algorithm = algorithms.FirstOrDefault(algorithm => algorithm.Extension.Equals(extension));
            return algorithm ?? throw new ArgumentException("Unknown archive extension");
        }

        public List<string> GetResolvedArchiveExtensions() => algorithms.Select(algorithm => algorithm.Extension)
                                                                        .ToList();

        public List<AlgorithmType> GetResolvedAlgorithmTypes() => algorithms.Select(algorithm => algorithm.Type)
                                                                    .ToList();
    }
}