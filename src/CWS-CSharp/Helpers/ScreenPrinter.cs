using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CWS.CSharp.TMS;
using Response = CWS.CSharp.TPS.Response;

namespace CWS.CSharp.Helpers
{
    public static class ScreenPrinter
    {
        public static void PrintTransactionResponse(Response response, string operation = null)
        {
            if (response == null)
                return;

            if(!string.IsNullOrEmpty(operation))
                Console.WriteLine("\n********* " + operation + " " + response.Status + "! *********");

            if(!string.IsNullOrEmpty(response.StatusMessage))
                Console.WriteLine("    Status Message:    " + response.StatusMessage);
            if(!string.IsNullOrEmpty(response.TransactionId))
                Console.WriteLine("    Transaction ID:    " + response.TransactionId);
            if(!string.IsNullOrEmpty(response.TransactionState.ToString()))
                Console.WriteLine("    Transaction State: " + response.TransactionState);
            if(!string.IsNullOrEmpty(response.CaptureState.ToString()))
                Console.WriteLine("    Capture State:     " + response.CaptureState);

            Console.WriteLine("********* END TRANSACTION! *********");
        }   

        public static void PrintCaptureResponse(Response response, string operation = null)
        {
            if (response == null)
                return;

            if(!string.IsNullOrEmpty(operation))
                Console.WriteLine("\n********* " + operation + " " + response.Status + "! *********");

            if (!string.IsNullOrEmpty(response.StatusMessage))
                Console.WriteLine("    Status Message:    " + response.StatusMessage);
            if (!string.IsNullOrEmpty(response.TransactionId))
                Console.WriteLine("    Transaction ID:    " + response.TransactionId);
            if (!string.IsNullOrEmpty(response.TransactionState.ToString()))
                Console.WriteLine("    Transaction State: " + response.TransactionState);
            if (!string.IsNullOrEmpty(response.CaptureState.ToString()))
                Console.WriteLine("    Capture State:     " + response.CaptureState);

            Console.WriteLine("********* END CAPTURE! *********");
        }

        public static void PrintTransactionFamilies(List<FamilyDetail> fd)
        {
            if (fd == null || fd.Count == 0)
                return;

            var first = fd.First();
            Console.WriteLine("\n**** FAMILY DETAILS ****");
            Console.WriteLine("    Total Number of Family Details returned: " + fd.Count);
            Console.WriteLine("    Details on the first Family Detail in the list...");
            Console.WriteLine("        TransactionID: " + first.TransactionIds.First());
            Console.WriteLine("        CaptutureDateTime: " + first.CaptureDateTime);
            Console.WriteLine("        TransactionDateTime: " + first.TransactionMetaData.First().TransactionDateTime);
            Console.WriteLine("        Family ID: " + first.FamilyId);
            Console.WriteLine("        Family State: " + first.FamilyState);
            Console.WriteLine("        Service Key: " + first.ServiceKey);
            Console.WriteLine("**** END FAMILY DETAILS ****");
        }

        public static void PrintTransactionDetail(List<TransactionDetail> td)
        {
            if (td == null || td.Count == 0)
                return;

            var first = td.First();
            Console.WriteLine("\n**** TRANSACTION DETAILS ****");
            Console.WriteLine("    Total Number of Transaction Details returned: " + td.Count);
            Console.WriteLine("    Details on the first Transaction Detail in the list...");
            Console.WriteLine("        TransactionID: " + first.TransactionInformation.TransactionId);
            Console.WriteLine("        Amount: " + first.TransactionInformation.Amount);
            Console.WriteLine("        Transaction Date: " + first.TransactionInformation.TransactionTimestamp);
            Console.WriteLine("        CaptureState: " + first.TransactionInformation.CaptureState);
            Console.WriteLine("        Service Key: " + first.TransactionInformation.ServiceKey);
            Console.WriteLine("        Service Id: " + first.TransactionInformation.ServiceId);
            Console.WriteLine("**** END TRANSACTION DETAILS ****");
        }

        public static void PrintTransactionSummary(List<SummaryDetail> sd)
        {
            if (sd == null || sd.Count == 0)
                return;

            var first = sd.First();
            Console.WriteLine("\n**** TRANSACTION SUMMARY ****");
            Console.WriteLine("    Total Number of Transaction Details returned: " + sd.Count);
            Console.WriteLine("    Transaction Information on the first Transaction Summary in the list...");
            Console.WriteLine("        TransactionID: " + first.TransactionInformation.TransactionId);
            Console.WriteLine("        Amount: " + first.TransactionInformation.Amount);
            Console.WriteLine("        Transaction Date: " + first.TransactionInformation.TransactionTimestamp);
            Console.WriteLine("        CaptureState: " + first.TransactionInformation.CaptureState);
            Console.WriteLine("        Service Key: " + first.TransactionInformation.ServiceKey);
            Console.WriteLine("        Service Id: " + first.TransactionInformation.ServiceId);
            Console.WriteLine("    Family Information on the first Transaction Summary in the list...");
            Console.WriteLine("        Family Id: " + first.FamilyInformation.FamilyId);
            Console.WriteLine("        Family State: " + first.FamilyInformation.FamilyState);
            Console.WriteLine("**** END TRANSACTION SUMMARY ****");
        }
    }
}
