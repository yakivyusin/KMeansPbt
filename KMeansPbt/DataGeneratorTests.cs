using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;
using Xunit;

namespace KMeansPbt
{
    public class DataGeneratorTests
    {
        [Property]
        public Property AllPointsUnique_WithZeroDupParameter(PositiveInt x)
        {
            var generator = new DataGenerator();
            var data = generator.Generate((uint)x.Get);

            return data
                .All(x1 => !data.Any(x2 => x1.Features.SequenceEqual(x2.Features) && x1 != x2))
                .ToProperty();
        }

        [Property]
        public Property LastPointDuplicated_WithNotZeroDupParameter(PositiveInt x, PositiveInt y)
        {
            var generator = new DataGenerator();
            var data = generator.Generate((uint)x.Get, (uint)y.Get);
            var uniquePart = data.Take(x.Get).ToArray();
            var copyPart = data.Skip(x.Get).ToArray();

            return copyPart
                .All(x1 => x1.Features.SequenceEqual(uniquePart.Last().Features))
                .ToProperty();
        }

        [Property]
        public Property AllDuplicates_DifferentObjects(PositiveInt x, PositiveInt y)
        {
            var generator = new DataGenerator();
            var data = generator.Generate((uint)x.Get, (uint)y.Get);
            var duplicates = data.Skip(x.Get - 1).ToArray();

            return duplicates
                .All(x1 => duplicates.All(x2 => !Object.ReferenceEquals(x1, x2) || x1 == x2))
                .ToProperty();
        }
    }
}
