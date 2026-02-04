using System;
using System.Numerics;

namespace CraterSprite;
public abstract class CraterMath
{
    public static T MoveTo<T>(T input, T destination, T rate)
        where T : ISignedNumber<T>, IComparable<T>
    {
        var delta = destination - input;
        if (T.Abs(delta).CompareTo(rate) < 0)
        {
            return destination;
        }

        return input + (T.IsNegative(delta) ? -rate : rate);
    }
}