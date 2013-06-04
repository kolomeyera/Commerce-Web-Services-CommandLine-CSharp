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

____________________________________________________________________________________________________________________

NOTE: You will need to obtain an IdentityToken from your Solutions Consultant in order to run this project successfully.

Step 1:  In the app.config file enter the IdentityToken given to you. Change the MsgFormat to either SOAP, XML (REST), or 
	JSON (REST) depending	on the how you chose to send the messages. Edit any other values needed in the app.config. 

Step 2:  Run the project, an application profile and merchant profile will be created. Transactions will then be processed
	using this service information. When the project completes its run the merchant profile and application data will be deleted. 
	The main program is located in CwsSampleCodeStarter.cs and is broken into regions based that mimic the commerce docs.

Folders
	FaultHandlers
		RestFaultHandler - Processes REST faults
		SoapFaultHandler - Prcoesses SOAP faults
	GeneratedDataObjects
		DataGenerator - Creates objects needed for creating application data, merchant profile data, and transaction data. 
	GeneratedProxies
		The files in this directory were created from the CWSDataServicesBilling.wsdl, CWSServiceInformation.wsdl, 
		CWSTransactionManagement.wsdl, and CwsTransactionProcessing.wsdl. The classes created from these wsdls maybe used
		directly or directly from the services references (or both). The class files with REST at the end of their name
		are only used for REST implementations, and were generated using the CWS REST schemas. 
	Helpers
		ScreenPrinter - Used to write information throughout the program to the console. 
	ServiceProxies
		ServiceInformationProxy, TransactionManagementProxy, and TransactionProcessingProxy contain the main information
		about how to create your proxies used to interface with CWS. 
		

*** Common Values per Industry Type ***

Industry: Ecommerce
	TxnData_IndustryType = 'Ecommerce'
	TxnData_CustomerPresent = 'Ecommerce'
	ApplicationAttended = false;
	ApplicationLocation = 'OffPremises'
	PINCapability = 'PINNotSupported'
	ReadCapability = 'KeyOnly'
	EntryMode = 'Keyed'
	
Industry: MOTO
	TxnData_IndustryType = 'MOTO'
	TxnData_CustomerPresent = 'MOTO'
	ApplicationAttended = false;
	ApplicationLocation = 'OffPremises'
	PINCapability = 'PINNotSupported'
	ReadCapability = 'KeyOnly'
	EntryMode = 'Keyed'
	
Industry: Retail
	TxnData_IndustryType = 'Retail'
	TxnData_CustomerPresent = 'Present'
	ApplicationAttended = true;
	ApplicationLocation = 'OnPremises'
	PINCapability = 'PINNotSupported'
	ReadCapability = 'HasMSR'
	EntryMode = 'TrackDataFromMSR'
											
Industry: Restaurant
	TxnData_IndustryType = 'Restaurant'
	TxnData_CustomerPresent = 'Present'
	ApplicationAttended = true;
	ApplicationLocation = 'OffPremises'
	PINCapability = 'PINNotSupported'
	ReadCapability = 'HasMSR'
	EntryMode = 'TrackDataFromMSR'											