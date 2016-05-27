using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Extensions.Test
{
   [TestClass]
   public class ReceivedTest
   {
      public interface IExampleModel
      {
         void Function1();
         void Function2();
         void Function3();
      }

      private void CallMethods(IExampleModel model)
      {
         model.Function1();
         model.Function2();
         model.Function3();
      }

      /// <summary>
      /// Count of received calls on model - order is unimportant.
      /// </summary>
      [TestMethod]
      public void Received_NSubstituteReceivedCalls_ReceivesExactly()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         model.Received(1).Function1();
         model.Received(1).Function2();
         model.Received(1).Function3();
      }

      /// <summary>
      /// model receives exactly the calls specified in order.
      /// </summary>
      [TestMethod]
      public void Received_NSubstituteReceivesInOrder_ReceivesInOrder()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         Received.InOrder(() =>
         {
            model.Function1();
            model.Function2();
            model.Function3();
         });
      }

      /// <summary>
      /// model receives the calls specified in order - other calls were made to the mocked object.
      /// </summary>
      [TestMethod]
      public void Received_NSubstituteReceivesInOrder_ReceivesInOrderPartial()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         Received.InOrder(() =>
         {
            model.Function1();
            model.Function2();
         });
      }

      /// <summary>
      /// model receives the calls specified in order - other calls were made to the mocked object.
      /// </summary>
      [TestMethod]
      public void Received_NSubstituteReceivesInOrderStarts_ReceivesInOrderPartial()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         NSubstituteExtension.Received.InOrderStarts(() =>
         {
            model.Function1();
            model.Function2();
         });
      }

      /// <summary>
      /// This test demonstrates that only the exact sequence of calls on model are permitted.
      /// In this case, the test of InOrder fails because more calls were detected than specified.
      /// </summary>
      [TestMethod]
      [ExpectedException(typeof(NSubstitute.Exceptions.CallSequenceNotFoundException))]
      public void Received_NSubstituteReceivesInOrder_ThrowsExceptionTooManyItemsReceieved()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);
         CallMethods(model);

         // assert
         Received.InOrder(() =>
         {
            model.Function1();
            model.Function2();
            model.Function3();
         });
      }

      /// <summary>
      /// This test demonstrates that any sequence of calls are permitted on model so long as they start in the correct order.
      /// </summary>
      [TestMethod]
      public void Received_NSubstituteExtensionInOrderStarts_ReceivesInOrder()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);
         CallMethods(model);

         // assert
         NSubstituteExtension.Received.InOrderStarts(() =>
         {
            model.Function1();
            model.Function2();
            model.Function3();
         });
      }

      #region Tests that do not make enough calls

      /// <summary>
      /// This test demonstrates that more calls were expected on model than received - InOrder.
      /// In this case, the test of InOrder fails because more calls were specified than detected.
      /// </summary>
      [TestMethod]
      [ExpectedException(typeof(NSubstitute.Exceptions.CallSequenceNotFoundException))]
      public void Received_NSubstituteTooManyMethodCallsExpected_ThrowsException()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         Received.InOrder(() =>
         {
            model.Function1();
            model.Function2();
            model.Function3();
            // additional function call
            model.Function1();
         });
      }

      /// <summary>
      /// This test demonstrates that more calls were expected on model than received - InOrderStarts.
      /// In this case, the test of InOrderStarts fails because more calls were specified than detected.
      /// </summary>
      [TestMethod]
      [ExpectedException(typeof(NSubstitute.Exceptions.CallSequenceNotFoundException))]
      public void Received_NSubstituteExtensionInOrderStartsTooManyMethodCallsExpected_ThrowsException()
      {
         // arrange
         var model = Substitute.For<IExampleModel>();

         // act
         CallMethods(model);

         // assert
         NSubstituteExtension.Received.InOrderStarts(() =>
         {
            model.Function1();
            model.Function2();
            model.Function3();
            // additional function call
            model.Function1();
         });
      }

      #endregion
   }
}