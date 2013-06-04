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
using System.Configuration;
using System.Linq;
using System.Text;
using CWS.CSharp.GenerateDataObjects;
using CWS.CSharp.Helpers;
using CWS.CSharp.STS;
using CWS.CSharp.ServiceProxies;
using CWS.CSharp.TMS;
using CWS.CSharp.TPS;
using ElectronicCheckingTransaction = CWS.CSharp.TPS.ElectronicCheckingTransaction;
using StoredValueTransaction = CWS.CSharp.TPS.StoredValueTransaction;

namespace CWS.CSharp
{
    class CwsSampleCodeStarter
    {
        private static string _identityToken = ConfigurationManager.AppSettings["IdentityToken"];
        private static string _sessionToken = "";

        private static string _applicationProfileId = "";
        private static string _merchantProfileId = ConfigurationManager.AppSettings["MerchantProfileId"];
        private static string _bcpServiceId = "";
        private static string _eckServiceId = "";
        private static string _svaServiceId = "";

        private static ServiceInformationProxy _svcClient = new ServiceInformationProxy();
        private static TransactionProcessingProxy _txnClient = new TransactionProcessingProxy();
        private static TransactionManagementProxy _mgmtClient = new TransactionManagementProxy();

        static void Main(string[] args)
        {
            Console.BufferWidth = 150;
            //Console.BufferHeight = Console.LargestWindowHeight;
            Console.BufferHeight = 60;
            Console.SetWindowSize(Console.BufferWidth, Console.BufferHeight);
            
            #region Step 1: SignOn Authentication
            Console.WriteLine("Attempting to sign in...");
            _sessionToken = _svcClient.SignOn(_identityToken);
            Console.WriteLine(!string.IsNullOrEmpty(_sessionToken) ? "Successfully signed in!\n" : "ERROR Signing in!\n");
            #endregion
            
            #region Step 2: Managing Application Configuration Data
            Console.WriteLine("Attempting to save application data...");
            var applicationData = DataGenerator.CreateApplicationData();
            _applicationProfileId = _svcClient.SaveApplicationData(_sessionToken, applicationData);
            if(!string.IsNullOrEmpty(_applicationProfileId))
                Console.WriteLine("Application saved successfully! Application Profile ID: " + _applicationProfileId + "\n");
            else Console.WriteLine("ERROR Saving Application!\n");
            #endregion

            #region Step 3: Retrieving Service Information
            Console.WriteLine("Getting service information...");
            var serviceInfo = _svcClient.GetServiceInformation(_sessionToken);
            #endregion

            #region Step 4: Managing Merchant Profiles
            if (serviceInfo.BankcardServices.Any())
            {
                // If there are multiple services you'll want to use them as desired. The sample will only focus on the first one.
                var bankcardService = serviceInfo.BankcardServices.First();
                _bcpServiceId = bankcardService.ServiceId;
                Console.WriteLine("There are " + serviceInfo.BankcardServices.Count() + " bankcard services available. Using serviceId " + _bcpServiceId + "\n");

                if (!_svcClient.IsMerchantProfileInitialized(_sessionToken, _bcpServiceId, _merchantProfileId, TenderType.Credit))
                {
                    Console.WriteLine("Merchant Profile " + _merchantProfileId + " does not exist. Attempting to save...");
                    _svcClient.SaveMerchantProfiles(_sessionToken, _bcpServiceId, TenderType.Credit, DataGenerator.CreateMerchantProfiles());
                    if (_svcClient.IsMerchantProfileInitialized(_sessionToken, _bcpServiceId, _merchantProfileId, TenderType.Credit))
                        Console.WriteLine("Merchant Profile " + _merchantProfileId + " saved successfully!\n");

                } else Console.WriteLine("Merchant Profile " + _merchantProfileId + " exists. Skipping SaveMerchantProfiles.");
            }
            if(serviceInfo.ElectronicCheckingServices.Any())
            {
                // If there are multiple services you'll want to use them as desired. The sample will only focus on the first one.
                var eckServce = serviceInfo.ElectronicCheckingServices.First();
                _eckServiceId = eckServce.ServiceId;
                Console.WriteLine("There are " + serviceInfo.BankcardServices.Count() + " electronic checking services available. Using serviceId " + _eckServiceId + "\n");

                if (!_svcClient.IsMerchantProfileInitialized(_sessionToken, _eckServiceId, _merchantProfileId, TenderType.PINDebit))
                {
                    Console.WriteLine("Merchant Profile " + _merchantProfileId + " does not exist. Attempting to save...");
                    _svcClient.SaveMerchantProfiles(_sessionToken, _eckServiceId, TenderType.PINDebit, DataGenerator.CreateMerchantProfiles());
                    if (_svcClient.IsMerchantProfileInitialized(_sessionToken, _eckServiceId, _merchantProfileId, TenderType.PINDebit))
                        Console.WriteLine("Merchant Profile " + _merchantProfileId + " saved successfully!\n");

                }
                else Console.WriteLine("Merchant Profile " + _merchantProfileId + " exists. Skipping SaveMerchantProfiles.");
            }
            if(serviceInfo.StoredValueServices.Any())
            {
                // If there are multiple services you'll want to use them as desired. The sample will only focus on the first one.
                var svaService = serviceInfo.ElectronicCheckingServices.First();
                _svaServiceId = svaService.ServiceId;
                Console.WriteLine("There are " + serviceInfo.BankcardServices.Count() + " stroed value services available. Using serviceId " + _svaServiceId + "\n");

                if (!_svcClient.IsMerchantProfileInitialized(_sessionToken, _svaServiceId, _merchantProfileId, TenderType.NotSet))
                {
                    Console.WriteLine("Merchant Profile " + _merchantProfileId + " does not exist. Attempting to save...");
                    _svcClient.SaveMerchantProfiles(_sessionToken, _svaServiceId, TenderType.NotSet, DataGenerator.CreateMerchantProfiles());
                    if (_svcClient.IsMerchantProfileInitialized(_sessionToken, _svaServiceId, _merchantProfileId, TenderType.NotSet))
                        Console.WriteLine("Merchant Profile " + _merchantProfileId + " saved successfully!\n");

                }
                else Console.WriteLine("Merchant Profile " + _merchantProfileId + " exists. Skipping SaveMerchantProfiles.");
            }

            // GetMerchantProfiles 
            var merchantProfiles = new List<MerchantProfile>();
            if(!string.IsNullOrEmpty(_bcpServiceId))
            {
                Console.WriteLine("Getting Merchant Profiles with service ID: " + _bcpServiceId);
                merchantProfiles = _svcClient.GetMerchantProfiles(_sessionToken, _bcpServiceId, TenderType.Credit);
            }
            else if (!string.IsNullOrEmpty(_eckServiceId))
            {
                Console.WriteLine("Getting Merchant Profiles with service ID: " + _eckServiceId);
                merchantProfiles = _svcClient.GetMerchantProfiles(_sessionToken, _eckServiceId, TenderType.PINDebit);
            }
            else if (!string.IsNullOrEmpty(_svaServiceId))
            {
                Console.WriteLine("Getting Merchant Profiles with service ID: " + _svaServiceId);
                merchantProfiles = _svcClient.GetMerchantProfiles(_sessionToken, _svaServiceId, TenderType.NotSet);
            }
            Console.WriteLine("There are " + merchantProfiles.Count + " merchant profiles related to this serviceId.");
            foreach (var merchantProfile in merchantProfiles)
            {
                Console.WriteLine("    Merchant Profile Id: " + merchantProfile.ProfileId);
                Console.WriteLine("        Service Name: " + merchantProfile.ServiceName);
            }
            #endregion
            
            #region Step 5: Authorizing Transactions
            var bankcardTransaction = DataGenerator.CreateBankcardTransaction();
            var eckTransaction = new ElectronicCheckingTransaction(); // TODO create ECK Transaction
            var svaTransaction = new StoredValueTransaction(); // TODO Create SVA Transaction
            
            var txnIdForAdjustAndCapture = "";
            var txnIdForUndo = "";
            var txnIdsForCaptureAll = new List<string>();
            var txnIdsForCaptureSelective = new List<string>();
            var txnIdForReturnById = "";
            // Bankcard Services - Authorize
            if(!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.Authorize)
            {
                Console.WriteLine("\nBankcard Processing: Creating transaction to adjust and then capture...");
                var response = _txnClient.Authorize(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId,_bcpServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "AUTHORIZE");
                if(response != null)
                    txnIdForAdjustAndCapture = !string.IsNullOrEmpty(response.TransactionId) ? response.TransactionId : null;

                Console.WriteLine("\nBankcard Processing: Creating transaction to undo...");
                var response2 = _txnClient.Authorize(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                ScreenPrinter.PrintTransactionResponse(response2, "AUTHORIZE");
                if (response2 != null)
                    txnIdForUndo = !string.IsNullOrEmpty(response2.TransactionId) ? response2.TransactionId : null;

                Console.WriteLine("\nBankcard Processing: Creating 3 transactions to be used to Capture All...");
                for (int i = 0; i < 3; i++)
                {
                    var response3 = _txnClient.Authorize(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                    ScreenPrinter.PrintTransactionResponse(response3, "AUTHORIZE");
                    if (response3 != null && !string.IsNullOrEmpty(response3.TransactionId))
                        txnIdsForCaptureAll.Add(response3.TransactionId);
                }

                Console.WriteLine("\nBankcard Processing: Creating 3 transactions to be used to Capture Selective...");
                for (int i = 0; i < 3; i++)
                {
                    var response4 = _txnClient.Authorize(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                    ScreenPrinter.PrintTransactionResponse(response4, "AUTHORIZE");
                    if (response4 != null && !string.IsNullOrEmpty(response4.TransactionId))
                        txnIdsForCaptureSelective.Add(response4.TransactionId);
                }
            }
            // Bankcard Services - Authorize and Capture
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.AuthAndCapture)
            {
                Console.WriteLine("\nBankcard Processing: Creating authorize and capture transaction to return by id...");
                var response = _txnClient.AuthorizeAndCapture(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "AUTHORIZE AND CAPTURE");
                if (response != null)
                    txnIdForReturnById = !string.IsNullOrEmpty(response.TransactionId) ? response.TransactionId : null;
            }
            // Electronic Checking Services - Authorize
            if (!string.IsNullOrEmpty(_eckServiceId) && serviceInfo.ElectronicCheckingServices.First().Operations.Authorize)
            {
                Console.WriteLine("\nElectronic Checking Service: Creating 3 transactions to be used to Capture All...");
                for (int i = 0; i < 3; i++)
                {
                    var response = _txnClient.Authorize(_sessionToken, eckTransaction, _applicationProfileId, _merchantProfileId, _eckServiceId);
                    ScreenPrinter.PrintTransactionResponse(response, "AUTHORIZE");
                    if (response != null && !string.IsNullOrEmpty(response.TransactionId))
                        txnIdsForCaptureAll.Add(response.TransactionId);
                }
            }
            // Stored Value Services - Authorize
            if (!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.Authorize)
            {
                Console.WriteLine("\nStored Value Service: Creating transaction to adjust and then capture...");
                var response = _txnClient.Authorize(_sessionToken, svaTransaction, _applicationProfileId, _merchantProfileId, _svaServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "AUTHORIZE");
                if (response != null)
                    txnIdForAdjustAndCapture = !string.IsNullOrEmpty(response.TransactionId) ? response.TransactionId : null;

                Console.WriteLine("\nStored Value Service: Creating transaction to undo...");
                var response2 = _txnClient.Authorize(_sessionToken, svaTransaction, _applicationProfileId, _merchantProfileId, _svaServiceId);
                ScreenPrinter.PrintTransactionResponse(response2, "AUTHORIZE");
                if (response2 != null)
                    txnIdForUndo = !string.IsNullOrEmpty(response2.TransactionId) ? response2.TransactionId : null;
            }
            // Stored Value Services - Authorize and Capture
            if(!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.AuthAndCapture)
            {
                Console.WriteLine("\nStored Value Service: Creating authorize and capture transaction to return by id...");
                var response = _txnClient.AuthorizeAndCapture(_sessionToken, svaTransaction, _applicationProfileId, _merchantProfileId, _svaServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "AUTHORIZE AND CAPTURE");
                if (response != null)
                    txnIdForReturnById = !string.IsNullOrEmpty(response.TransactionId) ? response.TransactionId : null;
            }
            #endregion

            //_txnClient.Capture(_sessionToken, )

            #region Step 6: Adjusting and Voiding Transactions
            
            // Bankcard Services - Adjust
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.Adjust)
            {
                Console.WriteLine("\nBankcard Services: Adjust on transaction " + txnIdForAdjustAndCapture + "...");
                var adjust = new Adjust { Amount = 5.00m, TransactionId = txnIdForAdjustAndCapture };
                var response = _txnClient.Adjust(_sessionToken, adjust, _applicationProfileId, _bcpServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "ADJUST AUTH(" + txnIdForAdjustAndCapture + ")");
                if(response != null) // Save the new txn guid, you'll use the most recent txn guid associated with the transaction for capture later.
                    txnIdForAdjustAndCapture = !string.IsNullOrEmpty(response.TransactionId) ? response.TransactionId : null;
            }
            // Bankcard Services - Undo
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.Undo && !string.IsNullOrEmpty(txnIdForUndo))
            {
                Console.WriteLine("\nBankcard Services: Undo on transaction " + txnIdForUndo + "...");
                var bankcardUndo = new BankcardUndo(){TransactionId = txnIdForUndo, PINDebitReason = PINDebitUndoReason.NotSet, ForceVoid = false};
                var response = _txnClient.Undo(_sessionToken, bankcardUndo, _applicationProfileId, _bcpServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "UNDO (" + txnIdForUndo + ")");
            }
            // Electronic Checking Services - Undo
            if (!string.IsNullOrEmpty(_eckServiceId) && serviceInfo.ElectronicCheckingServices.First().Operations.Undo && !string.IsNullOrEmpty(txnIdForUndo))
            {
                Console.WriteLine("\nElectronic Checking Services: Undo on transaction " + txnIdForUndo + "...");
                var response = _txnClient.Undo(_sessionToken, new Undo() { TransactionId = txnIdForUndo }, _applicationProfileId, _eckServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "UNDO AUTH(" + txnIdForUndo + ")");
            }
            // Stored Value Services - Undo
            if (!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.Undo && !string.IsNullOrEmpty(txnIdForUndo))
            {
                Console.WriteLine("Stored Value Services: Undo on transaction " + txnIdForUndo + "...");
                var response = _txnClient.Undo(_sessionToken, new Undo() { TransactionId = txnIdForUndo }, _applicationProfileId, _svaServiceId);
                ScreenPrinter.PrintTransactionResponse(response, "UNDO AUTH(" + txnIdForUndo + ")");
            }
            #endregion

            #region Step 7: Capturing Transaction for Settlement

            // Bankcard Services - Capture
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.Capture && !string.IsNullOrEmpty(txnIdForAdjustAndCapture))
            {
                Console.WriteLine("\nBankcard Services: Capture on transaction " + txnIdForAdjustAndCapture + "...");
                var chargeType = ConfigurationManager.AppSettings["IndustryType"] == "Retail" ? ChargeType.RetailOther : ChargeType.NotSet;
                var capture = new BankcardCapture() { TransactionId = txnIdForAdjustAndCapture, ChargeType = chargeType};
                if (ConfigurationManager.AppSettings["IndustryType"] == "MOTO") 
                    capture.ShipDate = DateTime.Now;
                var response = _txnClient.Capture(_sessionToken, capture, _applicationProfileId, _bcpServiceId);
                if(response != null)
                    ScreenPrinter.PrintCaptureResponse(response, "CAPTURE (" + txnIdForAdjustAndCapture + ")");
                // Incase the returnById txnId was not set earlier, we will use this Id to return later.
                if (string.IsNullOrEmpty(txnIdForReturnById))
                    txnIdForReturnById = txnIdForAdjustAndCapture;
            }
            // Bankcard Services - CaptureSelective
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.CaptureSelective && txnIdsForCaptureSelective.Count > 0)
            {
                Console.WriteLine("\nBankcard Services: CaptureSelective on transactions" + "...");
                var chargeType = ConfigurationManager.AppSettings["IndustryType"] == "Retail" ? ChargeType.RetailOther : ChargeType.NotSet;
                var captures = new List<BankcardCapture>();
                foreach (var txnId in txnIdsForCaptureSelective)
                {
                    var capture = new BankcardCapture() { TransactionId = txnId, ChargeType = chargeType };
                    if (ConfigurationManager.AppSettings["IndustryType"] == "MOTO")
                        capture.ShipDate = DateTime.Now;
                    captures.Add(capture);
                }
                var response = _txnClient.CaptureSelective<BankcardCapture>(_sessionToken, txnIdsForCaptureSelective, captures, _applicationProfileId, _bcpServiceId);
                if (response != null)
                    foreach (var r in response)
                        ScreenPrinter.PrintCaptureResponse(r, "CAPTURE (" + r.TransactionId + ")");
            }
            // Bankcard Services - CaptureAll
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.CaptureAll && txnIdsForCaptureAll.Count > 0)
            {
                Console.WriteLine("\nBankcard Services: CaptureAll on transactions" + "...");
                var chargeType = ConfigurationManager.AppSettings["IndustryType"] == "Retail" ? ChargeType.RetailOther : ChargeType.NotSet;
                var captures = new List<BankcardCapture>();
                foreach (var txnId in txnIdsForCaptureAll)
                {
                    var capture = new BankcardCapture() { TransactionId = txnId, ChargeType = chargeType };
                    if (ConfigurationManager.AppSettings["IndustryType"] == "MOTO")
                        capture.ShipDate = DateTime.Now;
                    captures.Add(capture);
                }
                var response = _txnClient.CaptureAll<BankcardCapture>(_sessionToken, captures, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                if (response != null)
                    foreach (var r in response)
                        ScreenPrinter.PrintCaptureResponse(r, "CAPTURE (" + r.TransactionId + ")");
            }
            // Electronic Checking Services - Capture All
            if (!string.IsNullOrEmpty(_eckServiceId) && serviceInfo.ElectronicCheckingServices.First().Operations.CaptureAll && txnIdsForCaptureAll.Count > 0)
            {
                Console.WriteLine("\nElectronic Checking Services: CaptureAll on transactions" + "...");
                var captures = txnIdsForCaptureAll.Select(txnId => new Capture() {TransactionId = txnId}).ToList();
                var response = _txnClient.CaptureAll<Capture>(_sessionToken, captures, _applicationProfileId, _merchantProfileId, _eckServiceId);
                if (response != null)
                    foreach (var r in response)
                        ScreenPrinter.PrintCaptureResponse(r, "CAPTURE (" + r.TransactionId + ")");
            }
            // Stored Value Services - Capture 
            if (!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.Capture && !string.IsNullOrEmpty(txnIdForAdjustAndCapture))
            {
                Console.WriteLine("\nStored Value Services: Capture on transaction " + txnIdForAdjustAndCapture + "...");
                var capture = new Capture() { TransactionId = txnIdForAdjustAndCapture};
                var response = _txnClient.Capture(_sessionToken, capture, _applicationProfileId, _svaServiceId);
                if (response != null)
                    ScreenPrinter.PrintCaptureResponse(response, "CAPTURE (" + txnIdForAdjustAndCapture + ")");
            }

            #endregion

            #region Step 8: Refunding Transactions
            // If the txnIdForReturnById was not set by this point, attempt to authorize and capture a new transaction. 
            if(!string.IsNullOrEmpty(_bcpServiceId) && string.IsNullOrEmpty(txnIdForReturnById))
            {
                var authResponse = _txnClient.Authorize(_sessionToken, bankcardTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                if (authResponse != null)
                {
                    var chargeType = ConfigurationManager.AppSettings["IndustryType"] == "Retail"? ChargeType.RetailOther : ChargeType.NotSet;
                    var capture = new BankcardCapture() {TransactionId = authResponse.TransactionId, ChargeType = chargeType};
                    if (ConfigurationManager.AppSettings["IndustryType"] == "MOTO")
                        capture.ShipDate = DateTime.Now;
                    var cList = new List<BankcardCapture>();
                    cList.Add(capture);
                    var capResponse = _txnClient.CaptureAll<BankcardCapture>(_sessionToken, cList, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                    if (capResponse != null)
                        txnIdForReturnById = authResponse.TransactionId;
                }
            }

            if(!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.ReturnById && !string.IsNullOrEmpty(txnIdForReturnById))
            {
                Console.WriteLine("\nBankcard Services: Return by id on " + txnIdForReturnById+ "...");
                // Specify an amount in the BankcardReturn if performing a partial return.
                // BankcardTenderData is required for PIN Debit transactions.
                var returnById = new BankcardReturn() {TransactionId = txnIdForReturnById};
                var response = _txnClient.ReturnById(_sessionToken, returnById, _applicationProfileId, _bcpServiceId);
                if (response != null)
                    ScreenPrinter.PrintTransactionResponse(response, "RETURN BY ID (" + txnIdForReturnById+ ")");
            }
            if (!string.IsNullOrEmpty(_bcpServiceId) && serviceInfo.BankcardServices.First().Operations.ReturnUnlinked)
            {
                Console.WriteLine("\nBankcard Services: Return Unlinked Transaction" + "...");
                var response = _txnClient.ReturnUnlinked(_sessionToken, bankcardTransaction, _applicationProfileId,_merchantProfileId, _bcpServiceId);
                if (response != null)
                    ScreenPrinter.PrintTransactionResponse(response, "RETURN UNLINKED");
            }
            if (!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.ReturnById && !string.IsNullOrEmpty(txnIdForReturnById))
            {
                Console.WriteLine("\nStored Value Services: Return by id on " + txnIdForReturnById + "...");
                var returnById = new Return() { TransactionId = txnIdForReturnById};
                var response = _txnClient.ReturnById(_sessionToken, returnById, _applicationProfileId, _svaServiceId);
                if (response != null)
                    ScreenPrinter.PrintTransactionResponse(response, "RETURN BY ID (" + txnIdForReturnById + ")");
            }
            if (!string.IsNullOrEmpty(_svaServiceId) && serviceInfo.StoredValueServices.First().Operations.ReturnUnlinked)
            {
                Console.WriteLine("\nStored Value Services: Return Unlinked Transaction" + "...");
                var response = _txnClient.ReturnUnlinked(_sessionToken, svaTransaction, _applicationProfileId, _merchantProfileId, _bcpServiceId);
                if (response != null)
                    ScreenPrinter.PrintTransactionResponse(response, "RETURN UNLINKED");
            }
            #endregion

            #region Step 9: Optional Operations
            // TODO Add Content for optional operations. They can include: 
            // Bankcard Processing (BCP): Acknowledge, Disburse, QueryAccount, Verify, RequestTransaction (Leave out Acknowledge and Dispurse in sample)
            // Electronic Checking (ECK): QueryAccount 
            // Stored Value Account (SVA): QueryAccount, ManageAccount, ManageAccountById  
            #endregion 

            #region Transaction Management Services

            Console.WriteLine("\n***Begin Transaction Managment Services***\n");

            var queryTransactionParameters = DataGenerator.CreateQueryTransactionParameters(QueryType.AND);
            var pagingParameters = DataGenerator.CreatePagingParameters();

            // QueryTransactionFamilies
            Console.WriteLine("Querying transaction Families...");
            var queryTransactionFamilies = _mgmtClient.QueryTransactionFamilies(_sessionToken, queryTransactionParameters, pagingParameters);
            ScreenPrinter.PrintTransactionFamilies(queryTransactionFamilies);

            //QueryTransactionsDetails
            var txnIds = new List<string>();
            if (queryTransactionFamilies[0] != null)
                txnIds.Add(queryTransactionFamilies[0].TransactionIds.First());
            if (queryTransactionFamilies[1] != null)
                txnIds.Add(queryTransactionFamilies[1].TransactionIds.First());
            if (queryTransactionFamilies[2] != null)
                txnIds.Add(queryTransactionFamilies[2].TransactionIds.First());

            queryTransactionParameters = DataGenerator.CreateQueryTransactionParameters(QueryType.OR, txnIds);
            queryTransactionParameters.TransactionDateRange = null;
            Console.WriteLine("\nQuery Transaction Details on the first " + txnIds.Count + " transaction ids returned from Query Transaciton Families...");
            var queryTransactionsDetail = _mgmtClient.QueryTransactionsDetail(_sessionToken, queryTransactionParameters, TransactionDetailFormat.CWSTransaction, pagingParameters, includeRelated: false);
            ScreenPrinter.PrintTransactionDetail(queryTransactionsDetail);

            Console.WriteLine("\nQuery Transaction Summaries on the first " + txnIds.Count + " transaction ids returned from Query Transaction Families...");
            var queryTransactionsSummary = _mgmtClient.QueryTransactionsSummary(_sessionToken, queryTransactionParameters, pagingParameters, true);
            ScreenPrinter.PrintTransactionSummary(queryTransactionsSummary);

            Console.WriteLine("\n***End Transaction Managment Services***\n");

            #endregion

            // Cleaning up after the sample code by deleting the merchant profile and the application data. 
            Console.WriteLine("\n****CLEAN UP****\n");
            Console.WriteLine("Deleting Merchant Profile " + _merchantProfileId + ".");
            _svcClient.DeleteMerchantProfile(_sessionToken, _bcpServiceId, _merchantProfileId, TenderType.Credit);
            if (!_svcClient.IsMerchantProfileInitialized(_sessionToken, _bcpServiceId, _merchantProfileId, TenderType.Credit))
                Console.WriteLine("Merchant Profile " + _merchantProfileId + " deleted successfully!");
            
            Console.WriteLine("\nDeleting Application Data for Application Profile Id " + _applicationProfileId + "...");
            _svcClient.DeleteApplicationData(_sessionToken, _applicationProfileId);
            if (_svcClient.GetApplicationData(_sessionToken, _applicationProfileId) == null)
                Console.WriteLine("Application Data deleted successfully!");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
