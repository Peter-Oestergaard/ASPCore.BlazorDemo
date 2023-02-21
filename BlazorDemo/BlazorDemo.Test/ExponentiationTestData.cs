namespace BlazorDemo.Test;

using System.Collections;

public class ExponentiationTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new Object[] {-1, 2};
        yield return new Object[] {-1, -2};
        yield return new Object[] {1, 2};
        yield return new Object[] {1, -2};
        yield return new Object[] {-1.5, 2};
        yield return new Object[] {-1.5, -2};
        yield return new Object[] {1.5, 2};
        yield return new Object[] {1.5, -2};
        yield return new Object[] {-1, 2.5};
        yield return new Object[] {-1, -2.5};
        yield return new Object[] {1, 2.5};
        yield return new Object[] {1, -2.5};
        yield return new Object[] {-1.5, 3.5};
        yield return new Object[] {-1.5, -3.5};
        yield return new Object[] {1.5, 3.5};
        yield return new Object[] {1.5, -3.5};
        yield return new Object[] {-1, double.MaxValue};
        yield return new Object[] {-1, double.MaxValue * -1};
        yield return new Object[] {1, double.MaxValue};
        yield return new Object[] {1, double.MaxValue * -1};
        yield return new Object[] {-1.5, double.MaxValue};
        yield return new Object[] {-1.5, double.MaxValue * -1};
        yield return new Object[] {1.5, double.MaxValue};
        yield return new Object[] {1.5, double.MaxValue * -1};
        yield return new Object[] {double.MaxValue, -1};
        yield return new Object[] {double.MaxValue * -1, -1};
        yield return new Object[] {double.MaxValue, 1};
        yield return new Object[] {double.MaxValue * -1, 1};
        yield return new Object[] {double.MaxValue, -1.5};
        yield return new Object[] {double.MaxValue * -1, -1.5};
        yield return new Object[] {double.MaxValue, 1.5};
        yield return new Object[] {double.MaxValue * -1, 1.5};
        yield return new Object[] {double.MaxValue, double.MaxValue};
        yield return new Object[] {double.MaxValue * -1, double.MaxValue};
        yield return new Object[] {double.MaxValue, double.MaxValue * -1};
        yield return new Object[] {double.MaxValue * -1, double.MaxValue * -1};
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}