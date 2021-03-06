using System;
using System.Collections.Generic;
using Xunit;

namespace ComparableIntervals.Tests {
    public class IntervalTest {
        [Theory]
        [MemberData(nameof(IntersectionData))]
        public void Intersection(Interval<int> first, Interval<int> second, Interval<int> expected) {
            var actual = first.Intersection(second);
            Verify(expected, actual);
        }

        [Theory]
        [MemberData(nameof(UnionData))]
        public void Union(Interval<int> first, Interval<int> second, Interval<int> expected) {
            var actual = first.Union(second);
            Verify(expected, actual);
        }

        [Theory]
        [MemberData(nameof(AdjacentData))]
        public void Adjacent<T>(Interval<T> first, Interval<T> second, bool expected) where T : IComparable {
            Assert.Equal(expected, first.IsAdjacentTo(second));
        }

        [Theory]
        [MemberData(nameof(InvalidBoundsData))]
        public void InvalidBounds(Bound<int> lower, Bound<int> upper) {
            Assert.Throws<ArgumentException>(() => Interval<int>.FromBounds(lower, upper));
        }

        private static void Verify<T>(Interval<T> expected, Interval<T> actual) where T : IComparable {
            if (!expected.IsEmpty() && !actual.IsEmpty() && (expected.UpperBound.IsNegativeInfinity() ||
                                                             expected.UpperBound.IsPositiveInfinity() ||
                                                             expected.LowerBound.IsNegativeInfinity() ||
                                                             expected.LowerBound.IsPositiveInfinity())) {
                // Cannot compare infinity bounds: for the test, comparing the string representation is enough.
                Assert.Equal(expected.ToString(), actual.ToString());
            } else {
                Assert.Equal(expected, actual);
            }
        }

        public static IEnumerable<object[]> IntersectionData =>
            new List<object[]> {
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(0, 1), Interval<int>.Closed(1, 1)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(5, 6), Interval<int>.Closed(5, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(4, 7), Interval<int>.Closed(4, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(2, 4), Interval<int>.Closed(2, 4)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(0, 6), Interval<int>.Closed(1, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(6, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(0, 1), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(5, 6), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(4, 7), Interval<int>.Open(4, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(2, 4), Interval<int>.Open(2, 4)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(0, 6), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(6, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(0, 1), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(5, 6), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(4, 7), Interval<int>.OpenClosed(4, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(2, 4), Interval<int>.Open(2, 4)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(0, 6), Interval<int>.Closed(1, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(6, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(0, 1), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(5, 6), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(4, 7), Interval<int>.ClosedOpen(4, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(2, 4), Interval<int>.Closed(2, 4)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(0, 6), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(6, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(1, 5), Interval<int>.Closed(1, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(1, 5), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(1, 5), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Empty(), Interval<int>.Empty()},
                new object[] {Interval<int>.Empty(), Interval<int>.Open(1, 5), Interval<int>.Empty()},
                new object[] {
                    Interval<int>.Closed(1, 5),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.Closed(1, 5)
                },
                new object[] {
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                },
            };

        public static IEnumerable<object[]> UnionData =>
            new List<object[]> {
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(4, 6), Interval<int>.Closed(1, 6)},
                new object[] {Interval<int>.Closed(1, 4), Interval<int>.Closed(4, 6), Interval<int>.Closed(1, 6)},
                new object[] {Interval<int>.Closed(4, 6), Interval<int>.Closed(5, 7), Interval<int>.Closed(4, 7)},
                new object[] {Interval<int>.Closed(4, 6), Interval<int>.Closed(6, 7), Interval<int>.Closed(4, 7)},
                new object[] {Interval<int>.Closed(1, 3), Interval<int>.Closed(5, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(4, 6), Interval<int>.Open(1, 6)},
                new object[] {Interval<int>.Open(1, 4), Interval<int>.Open(4, 6), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(4, 6), Interval<int>.Open(5, 7), Interval<int>.Open(4, 7)},
                new object[] {Interval<int>.Open(4, 6), Interval<int>.Open(6, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 3), Interval<int>.Open(5, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(4, 6), Interval<int>.OpenClosed(1, 6)},
                new object[] {Interval<int>.Open(1, 4), Interval<int>.Closed(4, 6), Interval<int>.OpenClosed(1, 6)},
                new object[] {Interval<int>.Open(4, 6), Interval<int>.Closed(5, 7), Interval<int>.OpenClosed(4, 7)},
                new object[] {Interval<int>.Open(4, 6), Interval<int>.Closed(6, 7), Interval<int>.OpenClosed(4, 7)},
                new object[] {Interval<int>.Open(1, 3), Interval<int>.Closed(5, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(4, 6), Interval<int>.ClosedOpen(1, 6)},
                new object[] {Interval<int>.Closed(1, 4), Interval<int>.Open(4, 6), Interval<int>.ClosedOpen(1, 6)},
                new object[] {Interval<int>.Closed(4, 6), Interval<int>.Open(5, 7), Interval<int>.ClosedOpen(4, 7)},
                new object[] {Interval<int>.Closed(4, 6), Interval<int>.Open(6, 7), Interval<int>.ClosedOpen(4, 7)},
                new object[] {Interval<int>.Closed(1, 3), Interval<int>.Open(5, 7), Interval<int>.Empty()},
                new object[] {Interval<int>.Empty(), Interval<int>.Open(5, 7), Interval<int>.Open(5, 7)},
                new object[] {Interval<int>.Closed(1, 3), Interval<int>.Empty(), Interval<int>.Closed(1, 3)},
                new object[] {Interval<int>.Empty(), Interval<int>.Empty(), Interval<int>.Empty()},
                new object[] {
                    Interval<int>.Closed(1, 5),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                },
                new object[] {
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                },
            };

        public static IEnumerable<object[]> AdjacentData =>
            new List<object[]> {
                new object[] {Interval<int>.Closed(1, 2), Interval<int>.Closed(2, 3), false},
                new object[] {Interval<int>.ClosedOpen(1, 2), Interval<int>.Closed(2, 3), true},
                new object[] {Interval<int>.Closed(2, 3), Interval<int>.Closed(1, 2), false},
                new object[] {Interval<int>.ClosedOpen(1, 2), Interval<int>.Closed(2, 3), true},
                new object[] {Interval<int>.ClosedOpen(1, 2), Interval<int>.Closed(3, 4), false},
                new object[] {Interval<int>.Closed(1, 2), Interval<int>.Closed(3, 4), true},
                new object[] {Interval<double>.Closed(1, 2), Interval<double>.Closed(3, 4), false},
                new object[] {
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Closed(1)),
                    Interval<int>.FromBounds(Bound<int>.Closed(2), Bound<int>.PositiveInfinity()),
                    true
                },
                new object[] {
                    Interval<double>.FromBounds(Bound<double>.NegativeInfinity(), Bound<double>.Closed(1)),
                    Interval<double>.FromBounds(Bound<double>.Closed(2), Bound<double>.PositiveInfinity()),
                    false
                },
                new object[] {
                    Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Open(1)),
                    Interval<int>.FromBounds(Bound<int>.Closed(1), Bound<int>.PositiveInfinity()),
                    true
                },
            };

        public static IEnumerable<object[]> InvalidBoundsData =>
            new List<object[]> {
                new object[] {Bound<int>.PositiveInfinity(), Bound<int>.Closed(0)},
                new object[] {Bound<int>.Closed(0), Bound<int>.NegativeInfinity()},
                new object[] {Bound<int>.Closed(1), Bound<int>.Open(-1)},
            };
    }
}