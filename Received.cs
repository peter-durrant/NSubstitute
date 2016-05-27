using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute.Core;
using NSubstitute.Core.SequenceChecking;

namespace NSubstituteExtension
{
   public static class Received
   {
      public static void InOrderStarts(Action calls)
      {
         var queryResults = SubstitutionContext.Current.RunQuery(calls);
         var results = new Results(queryResults);
         new SequenceInOrderAssertion().Assert(results);
      }

      private class Results : IQueryResults
      {
         private readonly IQueryResults _results;

         public Results(IQueryResults results)
         {
            _results = results;
         }

         public IEnumerable<ICall> MatchingCallsInOrder()
         {
            return _results.MatchingCallsInOrder().Take(QuerySpecification().Count());
         }

         public IEnumerable<CallSpecAndTarget> QuerySpecification()
         {
            return _results.QuerySpecification();
         }
      }
   }
}
