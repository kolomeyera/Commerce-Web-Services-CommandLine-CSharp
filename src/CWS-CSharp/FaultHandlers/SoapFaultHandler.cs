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
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using CWS.CSharp.STS;
using CWS.CSharp.TPS;
using CWS.CSharp.TMS;
using AuthenticationFault = CWS.CSharp.STS.AuthenticationFault;
using CWSFault = CWS.CSharp.STS.CWSFault;
using CWSValidationResultFault = CWS.CSharp.STS.CWSValidationResultFault;
using ExpiredTokenFault = CWS.CSharp.STS.ExpiredTokenFault;
using InvalidTokenFault = CWS.CSharp.STS.InvalidTokenFault;

namespace CWS.CSharp.FaultHandlers
{
    public static class SoapFaultHandler
    {
        public static void HandleFaultException(FaultException ex)
        {
            var msgString = "";

            var exType = ex.GetType();
            var prop = exType.GetProperty("Detail");
            var faultDetail = prop.GetValue(ex, null);
            var faultType = faultDetail.GetType();
            
            // Secure Token Service (STS) Faults
            if (faultType.IsInstanceOfType(new AuthenticationFault())) // 406
            {
                var authenticationFault = faultDetail as AuthenticationFault;
                msgString = msgString + "***Fault Exception: AuthenticationFault!***\n";
                msgString = msgString + "    ErrorId:     " + authenticationFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + authenticationFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + authenticationFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new STSUnavailableFault())) // 412
            {
                var stsUnavailableFault = faultDetail as STSUnavailableFault;
                msgString = msgString + "***Fault Exception: STSUnavailableFault!***\n";
                msgString = msgString + "    ErrorId:     " + stsUnavailableFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + stsUnavailableFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + stsUnavailableFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new AuthorizationFault())) // 413
            {
                var authorizationFault = faultDetail as AuthorizationFault;
                msgString = msgString + "***Fault Exception: AuthorizationFault!***\n";
                msgString = msgString + "    ErrorId:     " + authorizationFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + authorizationFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + authorizationFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new ExpiredTokenFault())) // 5000
            {
                var expiredTokenFault = faultDetail as ExpiredTokenFault;
                msgString = msgString + "***Fault Exception: ExpiredTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + expiredTokenFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + expiredTokenFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + expiredTokenFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new InvalidTokenFault())) // 5005
            {
                var invalidTokenFault = faultDetail as InvalidTokenFault;
                msgString = msgString + "***Fault Exception: InvalidTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + invalidTokenFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + invalidTokenFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + invalidTokenFault.ProblemType + "\n";
            }
            // Service Information Service Faults
            if (faultType.IsInstanceOfType(new CWSFault())) // 207
            {
                var cwsFault = faultDetail as CWSFault;
                msgString = msgString + "***Fault Exception: CWSFault!***\n";
                msgString = msgString + "    ErrorId:     " + cwsFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + cwsFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + cwsFault.ProblemType + "\n";
            } 
            if (faultType.IsInstanceOfType(new CWSValidationResultFault())) // 225
            {
                var cwsValidationResultFault = faultDetail as CWSValidationResultFault;
                msgString = msgString + "***Fault Exception: CWSValidationResultFault!***\n";
                msgString = msgString + "    ErrorId:     " + cwsValidationResultFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + cwsValidationResultFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + cwsValidationResultFault.ProblemType + "\n";
                msgString = msgString + "    Error Count: " + cwsValidationResultFault.Errors.Count() + "\n";
                foreach (var error in cwsValidationResultFault.Errors)
                {
                    msgString = msgString + "        Location: " + error.RuleLocationKey + " ---- Message : " + error.RuleMessage + "\n";
                }
            }
            if (faultType.IsInstanceOfType(new SystemFault())) // 401
            {
                var cwsFault = faultDetail as CWSFault;
                msgString = msgString + "***Fault Exception: CWSFault!***\n";
                msgString = msgString + "    ErrorId:     " + cwsFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + cwsFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + cwsFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new ClaimNotFoundFault())) // 415
            {
                var claimNotFoundFault = faultDetail as ClaimNotFoundFault;
                msgString = msgString + "***Fault Exception: ClaimNotFoundFault!***\n";
                msgString = msgString + "    ErrorId:     " + claimNotFoundFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + claimNotFoundFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + claimNotFoundFault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new RelyingPartyNotAssociatedToSecurityDomainFault())) // 450
            {
                var fault = faultDetail as RelyingPartyNotAssociatedToSecurityDomainFault;
                msgString = msgString + "***Fault Exception: RelyingPartyNotAssociatedToSecurityDomainFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSServiceInformationUnavailableFault())) // 9998
            {
                var fault = faultDetail as CWSServiceInformationUnavailableFault;
                msgString = msgString + "***Fault Exception: CWSServiceInformationUnavailableFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            // Transaction Processing Faults
            if (faultType.IsInstanceOfType(new CWSInvalidMessageFormatFault())) // 306
            {
                var fault = faultDetail as CWSInvalidMessageFormatFault;
                msgString = msgString + "***Fault Exception: CWSInvalidMessageFormatFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSDeserializationFault())) // 312
            {
                var fault = faultDetail as CWSDeserializationFault;
                msgString = msgString + "***Fault Exception: CWSDeserializationFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSExtendedDataNotSupportedFault())) // 313
            {
                var fault = faultDetail as CWSExtendedDataNotSupportedFault;
                msgString = msgString + "***Fault Exception: CWSExtendedDataNotSupportedFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.CWSInvalidOperationFault())) // 315
            {
                var fault = faultDetail as TPS.CWSInvalidOperationFault;
                msgString = msgString + "***Fault Exception: CWSInvalidOperationFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.CWSOperationNotSupportedFault())) // 317
            {
                var fault = faultDetail as TPS.CWSOperationNotSupportedFault;
                msgString = msgString + "***Fault Exception: CWSOperationNotSupportedFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSTransactionFailedFault())) // 318
            {
                var fault = faultDetail as CWSTransactionFailedFault;
                msgString = msgString + "***Fault Exception: CWSTransactionFailedFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.CWSValidationResultFault())) // 325
            {
                var cwsValidationResultFault = faultDetail as TPS.CWSValidationResultFault;
                msgString = msgString + "***Fault Exception: CWSValidationResultFault!***\n";
                msgString = msgString + "    ErrorId:     " + cwsValidationResultFault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + cwsValidationResultFault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + cwsValidationResultFault.ProblemType + "\n";
                msgString = msgString + "    Error Count: " + cwsValidationResultFault.Errors.Count() + "\n";
                foreach (var error in cwsValidationResultFault.Errors)
                {
                    msgString = msgString + "        Location: " + error.RuleLocationKey + " ---- Message : " + error.RuleMessage + "\n";
                }
            }
            if (faultType.IsInstanceOfType(new TPS.CWSFault())) // 326
            {
                var fault = faultDetail as TPS.CWSFault;
                msgString = msgString + "***Fault Exception: CWSFault!***\n";
                msgString = msgString + "    ErrorId:          " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:        " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType:      " + fault.ProblemType + "\n";
                msgString = msgString + "    TransactionId:    " + fault.TransactionId + "\n";
                msgString = msgString + "    TransactionState: " + fault.TransactionId + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSTransactionAlreadySettledFault())) // 327
            {
                var fault = faultDetail as CWSTransactionAlreadySettledFault;
                msgString = msgString + "***Fault Exception: CWSTransactionAlreadySettledFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new CWSConnectionFault())) // 328
            {
                var fault = faultDetail as CWSConnectionFault;
                msgString = msgString + "***Fault Exception: CWSConnectionFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.CWSTransactionServiceUnavailableFault())) // 9999
            {
                var fault = faultDetail as CWSTransactionServiceUnavailableFault;
                msgString = msgString + "***Fault Exception: CWSTransactionServiceUnavailableFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.AuthenticationFault())) // 406
            {
                var fault = faultDetail as TPS.AuthenticationFault;
                msgString = msgString + "***Fault Exception: AuthenticationFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.ExpiredTokenFault())) // 5000
            {
                var fault = faultDetail as TPS.ExpiredTokenFault;
                msgString = msgString + "***Fault Exception: ExpiredTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TPS.InvalidTokenFault())) // 5005
            {
                var fault = faultDetail as TPS.InvalidTokenFault;
                msgString = msgString + "***Fault Exception: InvalidTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            // Transaction Management Faults
            if (faultType.IsInstanceOfType(new TMSOperationNotSupportedFault())) // 517
            {
                var fault = faultDetail as TMSOperationNotSupportedFault;
                msgString = msgString + "***Fault Exception: TMSOperationNotSupportedFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TMSTransactionFailedFault())) // 518
            {
                var fault = faultDetail as TMSTransactionFailedFault;
                msgString = msgString + "***Fault Exception: TMSTransactionFailedFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TMSUnavailableFault())) // 9999
            {
                var fault = faultDetail as TMSUnavailableFault;
                msgString = msgString + "***Fault Exception: TMSUnavailableFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TMS.AuthenticationFault())) // 406
            {
                var fault = faultDetail as TMS.AuthenticationFault;
                msgString = msgString + "***Fault Exception: AuthenticationFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TMS.ExpiredTokenFault())) // 5000
            {
                var fault = faultDetail as TMS.ExpiredTokenFault;
                msgString = msgString + "***Fault Exception: ExpiredTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }
            if (faultType.IsInstanceOfType(new TMS.InvalidTokenFault())) // 5005
            {
                var fault = faultDetail as TMS.InvalidTokenFault;
                msgString = msgString + "***Fault Exception: InvalidTokenFault!***\n";
                msgString = msgString + "    ErrorId:     " + fault.ErrorID + "\n";
                msgString = msgString + "    Operation:   " + fault.Operation + "\n";
                msgString = msgString + "    ProblemType: " + fault.ProblemType + "\n";
            }

            if (string.IsNullOrEmpty(msgString))
                Console.WriteLine("\n----------UNHANDLED EXCEPTION!!!----------\n    Message: " + ex.Message + "\n------------------------------------------\n");
            else
                Console.WriteLine(msgString);
        }        
    }
}
