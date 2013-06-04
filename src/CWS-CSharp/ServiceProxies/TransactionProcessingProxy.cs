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
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CWS.CSharp.FaultHandlers;
using CWS.CSharp.STS;
using CWS.CSharp.TPS;
using IPC.CommonLibrary;
using schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest;
using Adjust = CWS.CSharp.TPS.Adjust;
using Capture = CWS.CSharp.TPS.Capture;
using Undo = CWS.CSharp.TPS.Undo;


namespace CWS.CSharp.ServiceProxies
{
    public class TransactionProcessingProxy
    {
        private static string _msgFormat = ConfigurationManager.AppSettings["MsgFormat"];
        private static readonly string RestBaseUri = ConfigurationManager.AppSettings["RestBaseURI"] + "/Txn";

        #region Authorize
        public Response Authorize(string sessionToken, Transaction transaction, string applicationProfileId, string merchantProfileId, string workflowId)
        {
            if(_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.Authorize(sessionToken, transaction, applicationProfileId, merchantProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restAuthTxn = new AuthorizeTransaction();
                restAuthTxn.ApplicationProfileId = applicationProfileId;
                restAuthTxn.MerchantProfileId = merchantProfileId;
                
                // Since 'transaction' references the service reference, this needs to be converted to use the Generated Proxie data contracts 
                // If using REST this step should be avoided by using the generated proxies directly throughout your application. 
                Type type = transaction.GetType();
                if(type == typeof(BankcardTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardTransaction>(transaction);
                else if (type == typeof(BankcardTransactionPro))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.Pro.BankcardTransactionPro>(transaction);
                else if (type == typeof(ElectronicCheckingTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.ElectronicChecking.ElectronicCheckingTransaction>(transaction);
                else if (type == typeof(StoredValueTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.StoredValue.StoredValueTransaction>(transaction);
                else if (type == typeof(Transaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Transaction>(transaction);

                var request = RestHelper.CreateRestRequest<AuthorizeTransaction>(restAuthTxn, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    if(isJson) 
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML the specifc expect response needs to be passed so that it can be deserialized properly. 
                    if (type == typeof(BankcardTransactionPro) || type == typeof(BankcardTransaction))
                        return RestHelper.GetResponse<BankcardTransactionResponsePro>(request, isJson);
                    if (type == typeof(ElectronicCheckingTransaction))
                        return RestHelper.GetResponse<ElectronicCheckingTransactionResponse>(request, isJson);
                    if (type == typeof(StoredValueTransaction))
                        return RestHelper.GetResponse<StoredValueTransactionResponse>(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson); 
                }
            }
            return null;
        }
        #endregion 

        #region AuthorizeAndCapture
        public Response AuthorizeAndCapture(string sessionToken, Transaction transaction, string applicationProfileId, string merchantProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.AuthorizeAndCapture(sessionToken, transaction, applicationProfileId, merchantProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restAuthTxn = new AuthorizeAndCaptureTransaction();
                restAuthTxn.ApplicationProfileId = applicationProfileId;
                restAuthTxn.MerchantProfileId = merchantProfileId;

                // Since 'transaction' references the service reference, this needs to be converted to use the Generated Proxie data contracts 
                // If using REST this step should be avoided by using the generated proxies directly throughout your application. 
                Type type = transaction.GetType();
                if (type == typeof(BankcardTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardTransaction>(transaction);
                else if (type == typeof(BankcardTransactionPro))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.Pro.BankcardTransactionPro>(transaction);
                else if (type == typeof(ElectronicCheckingTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.ElectronicChecking.ElectronicCheckingTransaction>(transaction);
                else if (type == typeof(StoredValueTransaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.StoredValue.StoredValueTransaction>(transaction);
                else if (type == typeof(Transaction))
                    restAuthTxn.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Transaction>(transaction);

                var request = RestHelper.CreateRestRequest<AuthorizeAndCaptureTransaction>(restAuthTxn, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML the specifc expect response needs to be passed so that it can be deserialized properly. 
                    if (type == typeof(BankcardTransactionPro) || type == typeof(BankcardTransaction))
                        return RestHelper.GetResponse<BankcardTransactionResponsePro>(request, isJson);
                    if (type == typeof(ElectronicCheckingTransaction))
                        return RestHelper.GetResponse<ElectronicCheckingTransactionResponse>(request, isJson);
                    if (type == typeof(StoredValueTransaction))
                        return RestHelper.GetResponse<StoredValueTransactionResponse>(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson); 
                }
            }
            return null;
        }
        #endregion

        #region Adjust
        public Response Adjust(string sessionToken, Adjust adjust, string applicationProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.Adjust(sessionToken, adjust, applicationProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId + "/" + adjust.TransactionId;
                var restAdjust = new schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Adjust();
                restAdjust.ApplicationProfileId = applicationProfileId;
                restAdjust.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Adjust>(adjust);

                var request = RestHelper.CreateRestRequest<schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Adjust>(restAdjust, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML and Adjust we do not know the specific type we will be getting back from the server. 
                    // For this we can get the string response back determine what type it is from this response 
                    // and then turn it into the desired object. 
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardTransactionResponsePro"))
                        return RestHelper.GetCWSObjectFromXml<BankcardTransactionResponsePro>(strResponse);
                    if (strResponse.Contains("ElectronicCheckingTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<ElectronicCheckingTransactionResponse>(strResponse);
                    if (strResponse.Contains("StoredValueTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<StoredValueTransactionResponse>(strResponse);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson); 
                }
            }
            return null;
        }
        #endregion

        #region Undo
        public Response Undo(string sessionToken, Undo undo, string applicationProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.Undo(sessionToken, undo, applicationProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId + "/" + undo.TransactionId;
                var restUndo = new schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Undo();
                restUndo.ApplicationProfileId = applicationProfileId;

                if(undo.GetType() == typeof(BankcardUndo))
                    restUndo.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardUndo>(undo);
                else
                    restUndo.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Undo>(undo);

                var request = RestHelper.CreateRestRequest<schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Undo>(restUndo, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML and Undo we do not know the specific type we will be getting back from the server. 
                    // For this we can get the string response back determine what type it is from this response 
                    // and then turn it into the desired object. 
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardTransactionResponsePro"))
                        return RestHelper.GetCWSObjectFromXml<BankcardTransactionResponsePro>(strResponse);
                    if (strResponse.Contains("ElectronicCheckingTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<ElectronicCheckingTransactionResponse>(strResponse);
                    if (strResponse.Contains("StoredValueTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<StoredValueTransactionResponse>(strResponse);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }
        #endregion

        #region Capture
        public Response Capture(string sessionToken, Capture capture, string applicationProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.Capture(sessionToken, capture, applicationProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId + "/" + capture.TransactionId;
                var restCapture = new schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Capture();
                restCapture.ApplicationProfileId = applicationProfileId;

                if(capture.GetType() == typeof(BankcardCapture))
                    restCapture.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardCapture>(capture);
                else
                    restCapture.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Capture>(capture);

                var request = RestHelper.CreateRestRequest<schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.Capture>(restCapture, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML and Undo we do not know the specific type we will be getting back from the server. 
                    // For this we can get the string response back determine what type it is from this response 
                    // and then turn it into the desired object. 
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardCaptureResponsePro"))
                        return RestHelper.GetCWSObjectFromXml<BankcardCaptureResponsePro>(strResponse);
                    if (strResponse.Contains("ElectronicCheckingCaptureResponse"))
                        return RestHelper.GetCWSObjectFromXml<ElectronicCheckingCaptureResponse>(strResponse);
                    if (strResponse.Contains("StoredValueCaptureResponse"))
                        return RestHelper.GetCWSObjectFromXml<StoredValueCaptureResponse>(strResponse);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }
        #endregion

        #region CaptureSelective
        public List<Response> CaptureSelective<T>(string sessionToken, List<string> transactionIds, List<T> captures, string applicationProfileId, string workflowId)
        {
            Type type = typeof(T);

            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                var bankcardCaptures = new List<BankcardCapture>();
                var regularCaptures = new List<Capture>();
                if (type == typeof(BankcardCapture))
                {
                    foreach (var capture in captures)
                    {
                        bankcardCaptures.Add(capture as BankcardCapture);
                    }
                }
                else
                {
                    foreach (var capture in captures)
                    {
                        regularCaptures.Add(capture as Capture);
                    }
                }

                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        if (bankcardCaptures.Count > 0)
                            return client.CaptureSelective(sessionToken, transactionIds.ToArray(), bankcardCaptures.ToArray(), applicationProfileId, workflowId).ToList();
                        return client.CaptureSelective(sessionToken, transactionIds.ToArray(), regularCaptures.ToArray(), applicationProfileId, workflowId).ToList();
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restCaptureSelective = new schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.CaptureSelective();
                restCaptureSelective.ApplicationProfileId = applicationProfileId;
                // For Capture Selective in REST DifferenceData maybe set to null unless there is a change in the transaction since authorization.
                restCaptureSelective.DifferenceData = null;
                restCaptureSelective.TransactionIds = transactionIds;

                var request = RestHelper.CreateRestRequest<schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.CaptureSelective>(restCaptureSelective, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    // The response is an array of specific capture response. First step is to get the CWS Object from the list. 
                    // With JSON the conversion needs the fully typed namespace. 
                    // After that it is coverted to the shorter namespace defined by the service reference. 
                    // The last step converts this list to the basic Reponse type inorder to return. The full bankcard response object has more information you may want.
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardCaptureResponsePro") && isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.Pro.BankcardCaptureResponsePro>(strResponse);
                        var list = response.Select(bankcardCaptureResponsePro => Utilities.SwapObjectsNamespace<TPS.BankcardCaptureResponsePro>(bankcardCaptureResponsePro)).ToList();
                        return ConvertToBasicResponseList<TPS.BankcardCaptureResponsePro>(list);
                    }
                    else if (strResponse.Contains("BankcardCaptureResponse") && isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardCaptureResponse>(strResponse);
                        var list = response.Select(bankcardCaptureResponse => Utilities.SwapObjectsNamespace<TPS.BankcardCaptureResponse>(bankcardCaptureResponse)).ToList();
                        return ConvertToBasicResponseList<TPS.BankcardCaptureResponse>(list);
                    }
                    else if (isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Capture>(strResponse);
                        var list = response.Select(captureResponse => Utilities.SwapObjectsNamespace<TPS.Capture>(captureResponse)).ToList();
                        return ConvertToBasicResponseList<TPS.Capture>(list);
                    }
                    else if (!isJson) // XML
                    {
                        return RestHelper.GetCWSObjectListFromXml<TPS.Response>(strResponse);
                    }
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }
        #endregion

        #region CaptureAll
        public List<Response> CaptureAll<T>(string sessionToken, List<T> captures, string applicationProfileId, string merchantProfileId, string workflowId, List<string> batchIds = null )
        {
            Type type = typeof(T);

            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                var bankcardCaptures = new List<BankcardCapture>();
                var regularCaptures = new List<Capture>();
                if (type == typeof(BankcardCapture))
                {
                    foreach (var capture in captures)
                    {
                        bankcardCaptures.Add(capture as BankcardCapture);
                    }
                }
                else
                {
                    foreach (var capture in captures)
                    {
                        regularCaptures.Add(capture as Capture);
                    }
                }

                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        if(bankcardCaptures.Count > 0 && batchIds != null)
                            return client.CaptureAll(sessionToken, bankcardCaptures.ToArray(), batchIds.ToArray(), applicationProfileId, merchantProfileId, workflowId).ToList();
                        if(bankcardCaptures.Count > 0)
                            return client.CaptureAll(sessionToken, bankcardCaptures.ToArray(), null, applicationProfileId, merchantProfileId, workflowId).ToList();
                        if(batchIds != null)
                            return client.CaptureAll(sessionToken, regularCaptures.ToArray(), batchIds.ToArray(), applicationProfileId, merchantProfileId, workflowId).ToList();
                        return client.CaptureAll(sessionToken, regularCaptures.ToArray(), null, applicationProfileId, merchantProfileId, workflowId).ToList();
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restCaptureAll = new schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.CaptureAll();
                restCaptureAll.ApplicationProfileId = applicationProfileId;
                restCaptureAll.MerchantProfileId = merchantProfileId;
                // For Capture All in REST DifferenceData maybe set to null as well as batchId's so that all transactions will be batched together. 
                // If anything changed on a transaction between authorization and capture, the difference data needs to reflect this.
                restCaptureAll.DifferenceData = null;
                restCaptureAll.BatchIds = batchIds;

                var request = RestHelper.CreateRestRequest<schemas.ipcommerce.com.CWS.v2._0.Transactions.Rest.CaptureAll>(restCaptureAll, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    // The response is an array of specific capture response. First step is to get the CWS Object from the list. 
                    // With JSON the conversion needs the fully typed namespace. 
                    // After that it is coverted to the shorter namespace defined by the service reference. 
                    // The last step converts this list to the basic Reponse type inorder to return. The full bankcard response object has more information you may want.
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardCaptureResponsePro") && isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.Pro.BankcardCaptureResponsePro>(strResponse);
                        var list = response.Select(bankcardCaptureResponsePro => Utilities.SwapObjectsNamespace<TPS.BankcardCaptureResponsePro>(bankcardCaptureResponsePro)).ToList();
                        return ConvertToBasicResponseList<TPS.BankcardCaptureResponsePro>(list);
                    }
                    else if (strResponse.Contains("BankcardCaptureResponse") && isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardCaptureResponse>(strResponse);
                        var list = response.Select(bankcardCaptureResponse => Utilities.SwapObjectsNamespace<TPS.BankcardCaptureResponse>(bankcardCaptureResponse)).ToList();
                        return ConvertToBasicResponseList<TPS.BankcardCaptureResponse>(list);  
                    }
                    else if(isJson)
                    {
                        var response = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.Transactions.Capture>(strResponse);
                        var list = response.Select(captureResponse => Utilities.SwapObjectsNamespace<TPS.Capture>(captureResponse)).ToList();
                        return ConvertToBasicResponseList<TPS.Capture>(list);  
                    }
                    else if (!isJson) // XML
                    {
                        return RestHelper.GetCWSObjectListFromXml<TPS.Response>(strResponse);
                    }
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }
        #endregion

        #region ReturnById
        public Response ReturnById(string sessionToken, Return differenceData, string applicationProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.ReturnById(sessionToken, differenceData, applicationProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restReturnById = new ReturnById();
                restReturnById.ApplicationProfileId = applicationProfileId;

                // Since 'differenceData' references the service reference, this needs to be converted to use the Generated Proxie data contracts 
                // If using REST this step should be avoided by using the generated proxies directly throughout your application. 
                Type type = differenceData.GetType();
                if (type == typeof(BankcardReturn))
                    restReturnById.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardReturn>(differenceData);
                else if (type == typeof(Return))
                    restReturnById.DifferenceData = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Return>(differenceData);

                var request = RestHelper.CreateRestRequest<ReturnById>(restReturnById, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML the specifc expect response needs to be passed so that it can be deserialized properly. 
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardTransactionResponsePro"))
                        return RestHelper.GetCWSObjectFromXml<BankcardTransactionResponsePro>(strResponse);
                    if (strResponse.Contains("ElectronicCheckingTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<ElectronicCheckingTransactionResponse>(strResponse);
                    if (strResponse.Contains("StoredValueTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<StoredValueTransactionResponse>(strResponse);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson); 
                }
            }
            return null;
        }
        #endregion 

        #region ReturnUnlinked
        public Response ReturnUnlinked(string sessionToken, Transaction transaction, string applicationProfileId, string merchantProfileId, string workflowId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CwsTransactionProcessingClient(ConfigurationManager.AppSettings["Bindings.TxnSoap"]))
                {
                    try
                    {
                        return client.ReturnUnlinked(sessionToken, transaction, applicationProfileId, merchantProfileId, workflowId);
                    }
                    catch (FaultException ex)
                    {
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/" + workflowId;
                var restReturnTransaction = new ReturnTransaction();
                restReturnTransaction.ApplicationProfileId = applicationProfileId;
                restReturnTransaction.MerchantProfileId = merchantProfileId;
                
                Type type = transaction.GetType();
                if (type == typeof(BankcardTransaction))
                    restReturnTransaction.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.BankcardTransaction>(transaction);
                else if (type == typeof(BankcardTransactionPro))
                    restReturnTransaction.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Bankcard.Pro.BankcardTransactionPro>(transaction);
                else if (type == typeof(ElectronicCheckingTransaction))
                    restReturnTransaction.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.ElectronicChecking.ElectronicCheckingTransaction>(transaction);
                else if (type == typeof(StoredValueTransaction))
                    restReturnTransaction.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.StoredValue.StoredValueTransaction>(transaction);
                else if (type == typeof(Transaction))
                    restReturnTransaction.Transaction = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.Transactions.Transaction>(transaction);

                var request = RestHelper.CreateRestRequest<ReturnTransaction>(restReturnTransaction, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    if (isJson)
                        return RestHelper.GetResponse<Response>(request, isJson);
                    // For XML the specifc expect response needs to be passed so that it can be deserialized properly. 
                    var strResponse = RestHelper.GetResponse(request, isJson, false);
                    if (strResponse.Contains("BankcardTransactionResponsePro"))
                        return RestHelper.GetCWSObjectFromXml<BankcardTransactionResponsePro>(strResponse);
                    if (strResponse.Contains("ElectronicCheckingTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<ElectronicCheckingTransactionResponse>(strResponse);
                    if (strResponse.Contains("StoredValueTransactionResponse"))
                        return RestHelper.GetCWSObjectFromXml<StoredValueTransactionResponse>(strResponse);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson); 
                }
            }
            return null;
        }
        #endregion 

        private static List<Response> ConvertToBasicResponseList<T>(List<T> inputList )
        {
            Type type = typeof (T).BaseType;
            var list = new List<Response>();
            foreach (var single in inputList)
            {
                var response = new Response();
                foreach (var propInfo in type.GetProperties())
                {
                    
                    foreach (var prop in response.GetType().GetProperties())
                    {
                        if(prop == propInfo)
                        {
                            var propValue = propInfo.GetValue(single, null);
                            prop.SetValue(response, propValue, null);
                        }
                    }
                    
                }
                list.Add(response);
            }
            return list;
        }
    }
}

