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
using System.ServiceModel;
using System.Text;
using CWS.CSharp.FaultHandlers;
using CWS.CSharp.STS;
using CWS.CSharp.TMS;
using IPC.CommonLibrary;
using Ipc.TMS;
using schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.Rest;
using TMSOperationsClient = CWS.CSharp.TMS.TMSOperationsClient;

namespace CWS.CSharp.ServiceProxies
{
    public class TransactionManagementProxy
    {
        private static string _msgFormat = ConfigurationManager.AppSettings["MsgFormat"];
        private static readonly string RestBaseUri = ConfigurationManager.AppSettings["RestBaseURI"] + "/DataServices/TMS";

        #region QueryTransactionFamilies
        public List<FamilyDetail> QueryTransactionFamilies(string sessionToken, QueryTransactionsParameters queryTransactionsParameters, PagingParameters pagingParameters)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new TMSOperationsClient(ConfigurationManager.AppSettings["Bindings.MgmtSoap"]))
                {
                    try
                    {
                        return client.QueryTransactionFamilies(sessionToken, queryTransactionsParameters, pagingParameters).ToList();
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
                var requestString = RestBaseUri + "/transactionsFamily";
                var restQtf = new QueryTransactionsFamilies();
                
                // Convert the namespace from service reference to the generated proxies used by rest.
                restQtf.QueryTransactionsParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.QueryTransactionsParameters>(queryTransactionsParameters);
                restQtf.PagingParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.PagingParameters>(pagingParameters);

                var request = RestHelper.CreateRestRequest<QueryTransactionsFamilies>(restQtf, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    var responseStr = RestHelper.GetResponse(request, isJson, false);
                    if (isJson)
                    {
                        var list = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.FamilyDetail>(responseStr);
                        return list.Select(familyDetail => Utilities.SwapObjectsNamespace<TMS.FamilyDetail>(familyDetail)).ToList();
                    }
                    else
                    {
                        return RestHelper.GetCWSObjectListFromXml<TMS.FamilyDetail>(responseStr);
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

        #region QueryTransactionsDetail
        public List<TransactionDetail> QueryTransactionsDetail(string sessionToken, QueryTransactionsParameters queryTransactionsParameters, TransactionDetailFormat transactionDetailFormat,PagingParameters pagingParameters, Boolean includeRelated)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new TMSOperationsClient(ConfigurationManager.AppSettings["Bindings.MgmtSoap"]))
                {
                    try
                    {
                        return client.QueryTransactionsDetail(sessionToken, queryTransactionsParameters, transactionDetailFormat, includeRelated, pagingParameters).ToList();
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
                var requestString = RestBaseUri + "/transactionsDetail";
                var restQtd = new QueryTransactionsDetail();
                restQtd.IncludeRelated = includeRelated;
                restQtd.TransactionDetailFormat = (schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.TransactionDetailFormat)(transactionDetailFormat);

                // Convert the namespace from service reference to the generated proxies used by rest.
                restQtd.QueryTransactionsParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.QueryTransactionsParameters>(queryTransactionsParameters);
                restQtd.PagingParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.PagingParameters>(pagingParameters);

                var request = RestHelper.CreateRestRequest<QueryTransactionsDetail>(restQtd, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    var responseStr = RestHelper.GetResponse(request, isJson, false);
                    if (isJson)
                    {
                        var list = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.TransactionDetail>(responseStr);
                        return list.Select(transactionDetail => Utilities.SwapObjectsNamespace<TMS.TransactionDetail>(transactionDetail)).ToList();
                    }
                    else
                    {
                        return RestHelper.GetCWSObjectListFromXml<TMS.TransactionDetail>(responseStr);
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

        #region QueryTransactionsSummary
        public List<SummaryDetail> QueryTransactionsSummary(string sessionToken, QueryTransactionsParameters queryTransactionsParameters, PagingParameters pagingParameters, Boolean includeRelated)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new TMSOperationsClient(ConfigurationManager.AppSettings["Bindings.MgmtSoap"]))
                {
                    try
                    {
                        return client.QueryTransactionsSummary(sessionToken, queryTransactionsParameters, includeRelated, pagingParameters).ToList();
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
                var requestString = RestBaseUri + "/transactionsSummary";
                var restQts = new QueryTransactionsSummary();
                restQts.IncludeRelated = includeRelated;
                
                // Convert the namespace from service reference to the generated proxies used by rest.
                restQts.QueryTransactionsParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.QueryTransactionsParameters>(queryTransactionsParameters);
                restQts.PagingParameters = Utilities.SwapObjectsNamespace<schemas.ipcommerce.com.CWS.v2._0.DataServices.PagingParameters>(pagingParameters);

                var request = RestHelper.CreateRestRequest<QueryTransactionsSummary>(restQts, requestString, HttpMethod.POST, sessionToken, isJson);
                try
                {
                    var responseStr = RestHelper.GetResponse(request, isJson, false);
                    if (isJson)
                    {
                        var list = RestHelper.GetCWSObjectListFromJson<schemas.ipcommerce.com.CWS.v2._0.DataServices.TMS.SummaryDetail>(responseStr);
                        return list.Select(transactionSummary => Utilities.SwapObjectsNamespace<TMS.SummaryDetail>(transactionSummary)).ToList();
                    }
                    else
                    {
                        return RestHelper.GetCWSObjectListFromXml<TMS.SummaryDetail>(responseStr);
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
    }
}
