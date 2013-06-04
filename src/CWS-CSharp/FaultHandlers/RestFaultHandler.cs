/* Copyright (c) 2004-2012 IP Commerce, INC. - All Rights Reserved.
 *
 * This software and documentation is subject to and made
 * available only pursuant to the terms of an executed license
 * agreement, and may be used only in accordance with the terms
 * of said agreement. This software may not, in whole or in part,
 * be copied, photocopied, reproduced, translated, or reduced to
 * any electronic medium or machine-readable form without
 * prior consent, in writing, from IP Commerce, INC.
 *
 * Use, duplication or disclosure by the U.S. Government is subject
 * to restrictions set forth in an executed license agreement
 * and in subparagraph (c)(1) of the Commercial Computer
 * Software-Restricted Rights Clause at FAR 52.227-19; subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013, subparagraph (d) of the Commercial
 * Computer Software--Licensing clause at NASA FAR supplement
 * 16-52.227-86; or their equivalent.
 *
 * Information in this software is subject to change without notice
 * and does not represent a commitment on the part of IP Commerce.
 * 
 * Sample Code is for reference Only and is intended to be used for educational purposes. It's the responsibility of 
 * the software company to properly integrate into thier solution code that best meets thier production needs. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using schemas.ipcommerce.com.CWS.v2._0.Rest;

namespace CWS.CSharp.FaultHandlers
{
    public static class RestFaultHandler
    {
        public static void HandleFaultException(Exception ex, bool isJson)
        {
            var errorResponse = new ErrorResponse();
            try
            {
                if (isJson)
                {
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(ex.Message));
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (ErrorResponse));
                    errorResponse = ser.ReadObject(ms) as ErrorResponse;
                    ms.Close();
                }
                else
                {
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(ex.Message));
                    DataContractSerializer ser = new DataContractSerializer(typeof (ErrorResponse));
                    errorResponse = ser.ReadObject(ms) as ErrorResponse;
                    ms.Close();
                }
            }
            catch(Exception)
            {
                errorResponse = null; // Error serializing 
                Console.WriteLine("\n------ An unexpected exception was thrown -----");
                Console.WriteLine("    Exception Message: \n        " + ex.Message);
                Console.WriteLine("\n-----------------------------------------------");
            }

            if (errorResponse != null)
            {
                switch (errorResponse.ErrorId)
                {
                    // CWS Specific Faults - Service Information Faults
                    case 207:
                        Console.WriteLine("***Fault Exception: CWSFault!***");
                        break;
                    case 208:
                        Console.WriteLine("***Fault Exception: CWSInvalidOperationFault!***");
                        break;
                    case 225:
                        Console.WriteLine("***Fault Exception: CWSValidationResultFault!***");
                        break;
                    case 401:
                        Console.WriteLine("***Fault Exception: SystemFault!***");
                        break;
                    case 415:
                        Console.WriteLine("***Fault Exception: ClaimNotFoundFault!***");
                        break;
                    case 416:
                        Console.WriteLine("***Fault Exception: AccessClaimNotFoundFault!***");
                        break;
                    case 420:
                        Console.WriteLine("***Fault Exception: DuplicateClaimFault!***");
                        break;
                    case 421:
                        Console.WriteLine("***Fault Exception: DuplicateUserFault!***");
                        break;
                    case 422:
                        Console.WriteLine("***Fault Exception: ClaimTypeNotAllowedFault!***");
                        break;
                    case 423:
                        Console.WriteLine("***Fault Exception: ClaimSecurityDomainMismatchFault!***");
                        break;
                    case 424:
                        Console.WriteLine("***Fault Exception: ClaimPropertyValidationFault!***");
                        break;
                    case 450:
                        Console.WriteLine("***Fault Exception: RelyingPartyNotAssociatedToSecurityDomainFault!***");
                        break;
                    case 8888:
                        Console.WriteLine("***Fault Exception: CWSSignOnServiceUnavailableFault!***");
                        break;
                    case 9998:
                        Console.WriteLine("***Fault Exception: CWSServiceInformationUnavailableFault!***");
                        break;
                    case 9999:
                        Console.WriteLine("***Fault Exception: CWSTransactionServiceUnavailableFault! / TMSUnavailableFault!***");
                        break;
                    // CWS Specific Faults - Transaction Processing Faults
                    case 306:
                        Console.WriteLine("***Fault Exception: CWSInvalidMessageFormatFault!***");
                        break;
                    case 312:
                        Console.WriteLine("***Fault Exception: CWSDeserializationFault!***");
                        break;
                    case 313:
                        Console.WriteLine("***Fault Exception: CWSExtendedDataNotSupportedFault!***");
                        break;
                    case 314:
                        Console.WriteLine("***Fault Exception: CWSInvalidServiceConfigFault!***");
                        break;
                    case 315:
                        Console.WriteLine("***Fault Exception: CWSInvalidOperationFault!***");
                        break;
                    case 317:
                        Console.WriteLine("***Fault Exception: CWSOperationNotSupportedFault!***");
                        break;
                    case 318:
                        Console.WriteLine("***Fault Exception: CWSTransactionFailedFault!***");
                        break;
                    case 325:
                        Console.WriteLine("***Fault Exception: CWSValidationResultFault!***");
                        break;
                    case 326:
                        Console.WriteLine("***Fault Exception: CWSFault!***");
                        break;
                    case 327:
                        Console.WriteLine("***Fault Exception: CWSTransactionAlreadySettledFault!***");
                        break;
                    case 328:
                        Console.WriteLine("***Fault Exception: CWSConnectionFault!***");
                        break;
                    // CWS Specific Faults - Transaction Processing Faults
                    case 506:
                        Console.WriteLine("***Fault Exception: TMSInvalidMessageFormatFault!***");
                        break;
                    case 517:
                        Console.WriteLine("***Fault Exception: TMSOperationNotSupportedFault!***");
                        break;
                    case 518:
                        Console.WriteLine("***Fault Excpetion: TMSTransactionFailedFault!***");
                        break;


                    // STS Specific Faults
                    case 406:
                        Console.WriteLine("***Fault Exception: AuthenticationFault!***");
                        break;
                    case 412:
                        Console.WriteLine("***Fault Exception: STSUnavailableFault!***");
                        break;
                    case 413:
                        Console.WriteLine("***Fault Exception: AuthorizationFault!***");
                        break;
                    case 5000:
                        Console.WriteLine("***Fault Exception: CWSUnknownServiceKeyFault (ExpiredTokenFault)!***");
                        Console.WriteLine("    Resign on with original Identity Token....");
                        break;
                    case 5005:
                        Console.WriteLine("***Fault Exception: InvalidTokenFault!***");
                        break;
                    default:
                        Console.WriteLine("***UNKONWN FAULT: Error Id of " + errorResponse.ErrorId + " ***");
                        break;
                }

                if(!string.IsNullOrEmpty(errorResponse.Operation))
                    Console.WriteLine("    Operation: " + errorResponse.Operation);
                if(!string.IsNullOrEmpty(errorResponse.Reason))
                    Console.WriteLine("    Reason: " + errorResponse.Reason);
                if (errorResponse.Messages != null && errorResponse.Messages.Count > 0)
                    for (int i = 0; i < errorResponse.Messages.Count; i++ )
                    {
                        Console.WriteLine("    Message " + i + ":" + errorResponse.Messages[i] + "\n");
                    }
                if (errorResponse.ValidationErrors != null && errorResponse.ValidationErrors.Count > 0)
                {
                    Console.WriteLine("    Validation Errors: ");
                    foreach (var validationError in errorResponse.ValidationErrors)
                    {
                        Console.WriteLine("        Location: " + validationError.RuleLocationKey);
                        Console.WriteLine("        Message : " + validationError.RuleMessage);
                        Console.WriteLine("        Txn Id  : " + validationError.TransactionId + "\n");
                    }
                }
            }
        }
    }
}
