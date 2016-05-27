# NSubstitute Extensions

This code uses NSubstitute. See the [NSubstitute](http://nsubstitute.github.io/).

## InOrder

NSubstitute provides an `InOrder` function to test that calls were made in a specified order on a mocked object.

### Drawback

For some UUT threaded or async code, it was possible that many functions were repeatedly called, possibly in a repetitive cycle. This meant that the mocked object could receive many more calls than specified in the `InOrder` function. When this happens, the test fails regardless of whether the sequence was seen at the start.

One approach could be to fix the test, however, sometimes it's just desirable to implement a new function. That's why `InOrderStarts` was developed.

## InOrderStarts

Firstly `InOrderStarts` has the same signature as `InOrder` and can be used interchangeably. The implementation is largely the same, it's just the matching calls detected on the mocked object are truncated to the query specification defined in the call to `InOrderStarts`.

### Usage

The following code illustrates usage. Some functions on an mocked object called `model` are called (this would normally be done via the UUT, but here the functions are called directly to illustrate). If the NSubstitute `InOrder` function was used, the test would fail because more calls were made to the functions than expected. Here, the first two function calls must be `Function1()` and `Function2()` in order.

```c#
[TestMethod]
public void Received_NSubstituteReceivesInOrder_ReceivesInOrder()
{
    // arrange
    var model = Substitute.For<IExampleModel>();

    // act
    model.Function1();
    model.Function2();
    model.Function1();
    model.Function1();

    // assert
    NSubstituteExtension.Received.InOrderStarts(() =>
    {
        model.Function1();
        model.Function2();
    });
}
```
