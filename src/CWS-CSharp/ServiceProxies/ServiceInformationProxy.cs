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
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CWS.CSharp.FaultHandlers;
using CWS.CSharp.STS;
using IPC.CommonLibrary;
using schemas.ipcommerce.com.CWS.v2._0.ServiceInformation.Rest;


namespace CWS.CSharp.ServiceProxies
{
    public class ServiceInformationProxy
    {
        private static string _msgFormat = ConfigurationManager.AppSettings["MsgFormat"];
        private static readonly string RestBaseUri = ConfigurationManager.AppSettings["RestBaseURI"] + "/SvcInfo";
        private static string _identityToken = ""; // Used to resign on in the event of an expired token.
        
        public string SignOn(string identityToken = null)
        {
            if (!string.IsNullOrEmpty(identityToken))
                _identityToken = identityToken;

            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.SignOnWithToken(_identityToken);
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
                // No body is required for SignOn in the HttpWebRequest.
                var requestString = RestBaseUri + "/token";

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.GET.ToString();
                request.Credentials = new NetworkCredential(_identityToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";

                try
                {
                    return RestHelper.GetResponse(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }

        public ServiceInformation GetServiceInformation(string sessionToken)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.GetServiceInformation(sessionToken);
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
                // No body is required for GetServiceInformation in the HttpWebRequest.
                var requestString = RestBaseUri + "/serviceInformation";

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.GET.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                try
                {
                    return RestHelper.GetResponse<ServiceInformation>(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }

        public string SaveApplicationData(string sessionToken, ApplicationData applicationData)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.SaveApplicationData(sessionToken, applicationData);
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
                var requestString = RestBaseUri + "/appProfile";

                var request = RestHelper.CreateRestRequest<ApplicationData>(applicationData, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    return RestHelper.GetResponse(request, isJson);
                }
                catch(Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }

        public ApplicationData GetApplicationData(string sessionToken, string applicationProfileId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.GetApplicationData(sessionToken, applicationProfileId);
                    }
                    catch (FaultException ex)
                    {
                        if (ex.Message.Contains("No application data has been saved yet")) // Application data does not exist. (Deleted or not created)
                            return null;
                        SoapFaultHandler.HandleFaultException(ex);
                    }
                }
            }
            else // REST JSON or XML
            {
                var isJson = string.Equals(_msgFormat, MessageFormat.JSON.ToString());
                var requestString = RestBaseUri + "/appProfile/" + applicationProfileId;

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.GET.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                try
                {
                    return RestHelper.GetResponse<ApplicationData>(request, isJson);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No application data has been saved yet")) // Application data does not exist. (Deleted or not created)
                        return null;
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return null;
        }

        public void DeleteApplicationData(string sessionToken, string applicationProfileId)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        client.DeleteApplicationData(sessionToken, applicationProfileId);
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
                // No body is required for GetServiceInformation in the HttpWebRequest.
                var requestString = RestBaseUri + "/appProfile/" + applicationProfileId;

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.DELETE.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                try
                {
                    RestHelper.GetResponse(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
        }

        public bool IsMerchantProfileInitialized(string sessionToken, string serviceId, string merchantProfileId, TenderType tenderType)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.IsMerchantProfileInitialized(sessionToken, serviceId, merchantProfileId, tenderType);
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
                // No body is required for GetServiceInformation in the HttpWebRequest.
                var requestString = RestBaseUri + "/merchProfile/" + merchantProfileId + "/OK?serviceId=" + serviceId;

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.GET.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                try
                {
                    return Boolean.Parse(RestHelper.GetResponse(request, isJson));
                }
                catch(Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
            return false;
        }

        public void SaveMerchantProfiles(string sessionToken, string serviceId, TenderType tenderType, List<MerchantProfile> merchantProfiles )
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        // Referencing the STS service directly requires the merchant profiles to be in an arry instead of type List. 
                        // When generating proxies from the WSDL you can specify to use lists instead of arrays.
                        client.SaveMerchantProfiles(sessionToken, serviceId, tenderType, merchantProfiles.ToArray());
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
                var requestString = RestBaseUri + "/merchProfile?serviceId=" + serviceId;

                var request = RestHelper.CreateRestRequest(merchantProfiles, requestString, HttpMethod.PUT, sessionToken, isJson);
                try
                {
                    RestHelper.GetResponse(request, isJson);
                }
                catch(Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
        }

        public void DeleteMerchantProfile(string sessionToken, string serviceId, string merchantProfileId, TenderType tenderType)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        client.DeleteMerchantProfile(sessionToken, merchantProfileId, serviceId, tenderType);
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
                // No body is required for GetServiceInformation in the HttpWebRequest.
                var requestString = RestBaseUri + "/merchProfile/" + merchantProfileId + "?serviceId=" + serviceId;

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.DELETE.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                try
                {
                    RestHelper.GetResponse(request, isJson);
                }
                catch(Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
            }
        }

        public List<MerchantProfile> GetMerchantProfiles(string sessionToken, string serviceId, TenderType tenderType, string merchantProfileId = null)
        {
            if (_msgFormat == MessageFormat.SOAP.ToString())
            {
                using (var client = new CWSServiceInformationClient(ConfigurationManager.AppSettings["Bindings.StsSoap"]))
                {
                    try
                    {
                        return client.GetMerchantProfiles(sessionToken, serviceId, tenderType).ToList();
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

                // For REST, to return all Merchant Profiles by merchantProfileId. Otherwise return all Merchant Profiles by service ID. 
                var requestString = "";
                if(!string.IsNullOrEmpty(merchantProfileId))
                    requestString = RestBaseUri + "/merchProfile?merchantProfileId=" + merchantProfileId;
                else
                    requestString = RestBaseUri + "/merchProfile?serviceId=" + serviceId;

                HttpWebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
                request.Method = HttpMethod.GET.ToString();
                request.Credentials = new NetworkCredential(sessionToken, "");

                request.ContentType = isJson ? "application/json" : "application/xml";
                // You'll need to use the Rest merchant profile Id Data Contract for serializing. This is then converted to use the standard data contract.
                var restMerchProfiles = new List<MerchantProfileId>();
                try
                {
                    restMerchProfiles = RestHelper.GetResponseList<MerchantProfileId>(request, isJson);
                }
                catch (Exception ex)
                {
                    RestFaultHandler.HandleFaultException(ex, isJson);
                }
                
                var merchProfiles = new List<MerchantProfile>();
                foreach (var rmp in restMerchProfiles)
                {
                    var mp = new MerchantProfile();
                    mp.ProfileId = rmp.id;
                    mp.ServiceId = rmp.href.Split('=')[1];
                    merchProfiles.Add(mp);
                }
                return merchProfiles;
            }
            return null;
        }
    }
}
