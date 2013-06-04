using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CWS.CSharp.STS;
using CWS.CSharp.TMS;
using AccountType = CWS.CSharp.TPS.AccountType;
using AddressInfo = CWS.CSharp.STS.AddressInfo;
using AlternativeMerchantData = CWS.CSharp.TPS.AlternativeMerchantData;
using ApplicationData = CWS.CSharp.STS.ApplicationData;
using ApplicationLocation = CWS.CSharp.STS.ApplicationLocation;
using BankcardInterchangeData = CWS.CSharp.TPS.BankcardInterchangeData;
using BankcardMerchantData = CWS.CSharp.STS.BankcardMerchantData;
using BankcardTenderData = CWS.CSharp.TPS.BankcardTenderData;
using BankcardTransaction = CWS.CSharp.TPS.BankcardTransaction;
using BankcardTransactionData = CWS.CSharp.TPS.BankcardTransactionData;
using BankcardTransactionDataPro = CWS.CSharp.TPS.BankcardTransactionDataPro;
using BankcardTransactionPro = CWS.CSharp.TPS.BankcardTransactionPro;
using BillPayment = CWS.CSharp.TPS.BillPayment;
using CVDataProvided = CWS.CSharp.TPS.CVDataProvided;
using CardData = CWS.CSharp.TPS.CardData;
using CardSecurityData = CWS.CSharp.TPS.CardSecurityData;
using CustomerPresent = CWS.CSharp.STS.CustomerPresent;
using EncryptionType = CWS.CSharp.STS.EncryptionType;
using EntryMode = CWS.CSharp.STS.EntryMode;
using ExistingDebt = CWS.CSharp.TPS.ExistingDebt;
using GoodsType = CWS.CSharp.TPS.GoodsType;
using HardwareType = CWS.CSharp.STS.HardwareType;
using IndustryType = CWS.CSharp.STS.IndustryType;
using IsTaxExempt = CWS.CSharp.TPS.IsTaxExempt;
using Level2Data = CWS.CSharp.TPS.Level2Data;
using MerchantProfileMerchantData = CWS.CSharp.STS.MerchantProfileMerchantData;
using PINCapability = CWS.CSharp.STS.PINCapability;
using ReadCapability = CWS.CSharp.STS.ReadCapability;
using RequestACI = CWS.CSharp.STS.RequestACI;
using RequestAdvice = CWS.CSharp.STS.RequestAdvice;
using RequestCommercialCard = CWS.CSharp.TPS.RequestCommercialCard;
using Tax = CWS.CSharp.TPS.Tax;
using TaxExempt = CWS.CSharp.TPS.TaxExempt;
using Transaction = CWS.CSharp.TPS.Transaction;
using TransactionReportingData = CWS.CSharp.TPS.TransactionReportingData;
using TypeCardType = CWS.CSharp.TPS.TypeCardType;
using TypeISOCountryCodeA3 = CWS.CSharp.STS.TypeISOCountryCodeA3;
using TypeISOCurrencyCodeA3 = CWS.CSharp.STS.TypeISOCurrencyCodeA3;
using TypeISOLanguageCodeA3 = CWS.CSharp.STS.TypeISOLanguageCodeA3;
using TypeStateProvince = CWS.CSharp.STS.TypeStateProvince;

namespace CWS.CSharp.GenerateDataObjects
{
    public static class DataGenerator
    {
        public static ApplicationData CreateApplicationData()
        {
            var appData = new ApplicationData();
            appData.SoftwareVersion = ConfigurationManager.AppSettings["SoftwareVersion"];
            appData.SoftwareVersionDate = Convert.ToDateTime(ConfigurationManager.AppSettings["SoftwareVersionDate"]);
            appData.DeviceSerialNumber = ConfigurationManager.AppSettings["DeviceSerialNumber"];
            appData.ApplicationAttended = Convert.ToBoolean(ConfigurationManager.AppSettings["ApplicationAttended"]);
            appData.ApplicationLocation = (ApplicationLocation)Enum.Parse(typeof(ApplicationLocation), ConfigurationManager.AppSettings["ApplicationLocation"]);
            appData.PTLSSocketId = ConfigurationManager.AppSettings["PTLSSocketId"];
            appData.EncryptionType = (EncryptionType)Enum.Parse(typeof(EncryptionType), ConfigurationManager.AppSettings["EncryptionType"]);
            appData.ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            appData.SerialNumber = ConfigurationManager.AppSettings["SerialNumber"];
            appData.HardwareType = (HardwareType)Enum.Parse(typeof(HardwareType), ConfigurationManager.AppSettings["HardwareType"]);
            appData.PINCapability = (PINCapability)Enum.Parse(typeof(PINCapability), ConfigurationManager.AppSettings["PINCapability"]);
            appData.ReadCapability = (ReadCapability)Enum.Parse(typeof(ReadCapability), ConfigurationManager.AppSettings["ReadCapability"]);
            return appData;
        }

        public static List<MerchantProfile> CreateMerchantProfiles()
        {
            var merchProfile = new MerchantProfile();
            merchProfile.ProfileId = ConfigurationManager.AppSettings["MerchantProfileId"];
            merchProfile.LastUpdated = DateTime.Now;

            merchProfile.MerchantData = new MerchantProfileMerchantData();
            merchProfile.MerchantData.CustomerServiceInternet = ConfigurationManager.AppSettings["CustomerServiceInternet"];
            merchProfile.MerchantData.CustomerServicePhone = ConfigurationManager.AppSettings["CustomerServicePhone"];
            merchProfile.MerchantData.Language = (TypeISOLanguageCodeA3)Enum.Parse(typeof(TypeISOLanguageCodeA3), ConfigurationManager.AppSettings["Language"]);
            merchProfile.MerchantData.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            merchProfile.MerchantData.Name = ConfigurationManager.AppSettings["Name"];
            merchProfile.MerchantData.Phone = ConfigurationManager.AppSettings["Phone"];

            merchProfile.MerchantData.BankcardMerchantData = new BankcardMerchantData();
            merchProfile.MerchantData.BankcardMerchantData.ABANumber = "128965";
            merchProfile.MerchantData.BankcardMerchantData.AgentChain = "1289";
            merchProfile.MerchantData.BankcardMerchantData.AgentBank = "1289";
            merchProfile.MerchantData.BankcardMerchantData.AcquirerBIN = "1248";
            merchProfile.MerchantData.BankcardMerchantData.Location = "108";
            merchProfile.MerchantData.BankcardMerchantData.PrintCustomerServicePhone = false;
            merchProfile.MerchantData.BankcardMerchantData.ReimbursementAttribute = "X";
            merchProfile.MerchantData.BankcardMerchantData.SettlementAgent = "0001";
            merchProfile.MerchantData.BankcardMerchantData.SharingGroup = "0001";
            merchProfile.MerchantData.BankcardMerchantData.StoreId = "0001";
            merchProfile.MerchantData.BankcardMerchantData.SecondaryTerminalId = "751";
            merchProfile.MerchantData.BankcardMerchantData.TimeZoneDifferential = "750";
            merchProfile.MerchantData.BankcardMerchantData.IndustryType =(IndustryType)Enum.Parse(typeof(IndustryType), ConfigurationManager.AppSettings["IndustryType"]);
            merchProfile.MerchantData.BankcardMerchantData.SIC = "4599"; // Required, Standard Industry Code
            merchProfile.MerchantData.BankcardMerchantData.ClientNumber = "1234"; // Optional - Required for Chase
            merchProfile.MerchantData.BankcardMerchantData.Aggregator = Boolean.Parse(ConfigurationManager.AppSettings["Pro_IncludeAlternativeMerchantData"]);
            merchProfile.MerchantData.BankcardMerchantData.TerminalId = "001";

            merchProfile.MerchantData.Address = new AddressInfo();
            merchProfile.MerchantData.Address.Street1 = ConfigurationManager.AppSettings["MerchStreet1"];
            merchProfile.MerchantData.Address.Street2 = string.IsNullOrEmpty(ConfigurationManager.AppSettings["MerchStreet2"]) ? null : ConfigurationManager.AppSettings["MerchStreet2"];
            merchProfile.MerchantData.Address.City = ConfigurationManager.AppSettings["MerchCity"];
            merchProfile.MerchantData.Address.StateProvince = (TypeStateProvince)Enum.Parse(typeof(TypeStateProvince), ConfigurationManager.AppSettings["MerchStateProvince"]);
            merchProfile.MerchantData.Address.PostalCode = ConfigurationManager.AppSettings["MerchPostalCode"];
            merchProfile.MerchantData.Address.CountryCode = (TypeISOCountryCodeA3)Enum.Parse(typeof(TypeISOCountryCodeA3), ConfigurationManager.AppSettings["MerchCountryCode"]);
            
            merchProfile.TransactionData = new MerchantProfileTransactionData();
            merchProfile.TransactionData.BankcardTransactionDataDefaults = new BankcardTransactionDataDefaults();
            merchProfile.TransactionData.BankcardTransactionDataDefaults.CurrencyCode = (TypeISOCurrencyCodeA3)Enum.Parse(typeof(TypeISOCurrencyCodeA3), ConfigurationManager.AppSettings["CurrencyCode"]);
            merchProfile.TransactionData.BankcardTransactionDataDefaults.CustomerPresent =(CustomerPresent)Enum.Parse(typeof (CustomerPresent), ConfigurationManager.AppSettings["CustomerPresent"]);
            merchProfile.TransactionData.BankcardTransactionDataDefaults.RequestACI = (RequestACI) Enum.Parse(typeof (RequestACI), ConfigurationManager.AppSettings["RequestACI"]);
            merchProfile.TransactionData.BankcardTransactionDataDefaults.EntryMode = (EntryMode) Enum.Parse(typeof (EntryMode), ConfigurationManager.AppSettings["TxnData_EntryMode"]);
            merchProfile.TransactionData.BankcardTransactionDataDefaults.RequestAdvice = RequestAdvice.NotCapable; // [NotSet,NotCapable,Capable]
            
            var merchProfiles = new List<MerchantProfile>();
            merchProfiles.Add(merchProfile);
            return merchProfiles;
        }

        public static Transaction CreateBankcardTransaction()
        {
            var tenderData = new BankcardTenderData();
            tenderData.CardData = new CardData();

            if (ConfigurationManager.AppSettings["TxnData_IndustryType"] == "Retail")
            {
                tenderData.CardData.CardType = TypeCardType.Visa;
                tenderData.CardData.Expire = "1012";
                tenderData.CardData.PAN = "5454545454545454";
                tenderData.CardData.Track1Data = "B4111111111111111^IPCOMMERCE/TESTCARD^10121010454500415000010";
                
            }
            if (ConfigurationManager.AppSettings["TxnData_IndustryType"] == "Restaurant")
            {
                tenderData.CardData.CardType = TypeCardType.Visa;
                tenderData.CardData.Expire = "1012";
                tenderData.CardData.PAN = "5454545454545454";
                tenderData.CardData.Track1Data = "B4111111111111111^IPCOMMERCE/TESTCARD^10121010454500415000010";
            }
            if (ConfigurationManager.AppSettings["TxnData_IndustryType"] == "MOTO")
            {
                tenderData.CardData.CardholderName = "John Doe";
                tenderData.CardData.CardType = TypeCardType.MasterCard;
                tenderData.CardData.Expire = "1012";
                tenderData.CardData.PAN = "5454545454545454";
                tenderData.CardSecurityData = new CardSecurityData();
                tenderData.CardSecurityData.CVData = "111";
                tenderData.CardSecurityData.CVDataProvided = CVDataProvided.Provided;
            }
            if (ConfigurationManager.AppSettings["TxnData_IndustryType"] == "Ecommerce")
            {
                tenderData.CardData.CardholderName = "John Doe";
                tenderData.CardData.CardType = TypeCardType.Visa;
                tenderData.CardData.Expire = "1012";
                tenderData.CardData.PAN = "5454545454545454";
                tenderData.CardSecurityData = new CardSecurityData();
                tenderData.CardSecurityData.CVData = "111";
                tenderData.CardSecurityData.CVDataProvided = CVDataProvided.Provided;
            }

            /*
             * Note: not all processors support the new Alternative Merchant Data object
             * 		 See else statement below for alternate format of Soft Descriptors
             */
            var altMerchData = new AlternativeMerchantData();
            var reportingData = new TransactionReportingData();
            if(Boolean.Parse(ConfigurationManager.AppSettings["TxnData_SoftDescriptors"]))
            {
                altMerchData.Name = "AltMerchName";
                altMerchData.MerchantId = "122234";
                altMerchData.Description = "Blue Bottle";
                altMerchData.CustomerServiceInternet = "test@altmerch.com";
                altMerchData.CustomerServicePhone = "303 5551212";
                altMerchData.Address = new TPS.AddressInfo();
                altMerchData.Address.Street1 = "123 Test Street";
                altMerchData.Address.City = "Denver";
                altMerchData.Address.StateProvince = "CO";
                altMerchData.Address.PostalCode = "80202";
                altMerchData.Address.CountryCode = TPS.TypeISOCountryCodeA3.USA;
            }
            /*
             * Note: older processors support this way of Soft Descriptors/Alternative Merchant Data
             * 		 the combination of your top level MerchantProfile->MerchantName with MerchantProfile->CustomerServiceInternet
             * 		 combined with the ReportingData->Description will make the soft descriptor format
             */
            else
            {
                reportingData.Description = "AltMerchName";
            }

            var interchangeData = new BankcardInterchangeData();
            if(Boolean.Parse(ConfigurationManager.AppSettings["Pro_InterchangeData"]))
            {
                interchangeData.BillPayment = BillPayment.Recurring; // Any time BillPayInd is set to either "DeferredBilling", "Installment", "SinglePayment" or "Recurring", CustomerPresent should be set to "BillPayment"
                interchangeData.RequestCommercialCard = RequestCommercialCard.NotSet;
                interchangeData.ExistingDebt = ExistingDebt.NotExistingDebt;
                interchangeData.TotalNumberOfInstallments = 1; //Used for Installment
                interchangeData.CurrentInstallmentNumber = 1; // Send 1 for the first payment and any number greater than 1 for the remaining payments.
            }
            
            if(Boolean.Parse(ConfigurationManager.AppSettings["Pro_IncludeLevel2OrLevel3Data"]))
            {
                var txn = new BankcardTransactionPro();
                txn.TenderData = tenderData;
                txn.ReportingData = reportingData;
                txn.InterchangeData = interchangeData;

                var transactionData = new BankcardTransactionDataPro();
                transactionData.AlternativeMerchantData = altMerchData;

                transactionData.Amount = 10.00m;
                transactionData.CashBackAmount = 0.00m;
                transactionData.TipAmount = 0.00m;
                transactionData.AccountType = AccountType.NotSet;
                transactionData.CustomerPresent = (TPS.CustomerPresent)Enum.Parse(typeof(CustomerPresent), ConfigurationManager.AppSettings["TxnData_CustomerPresent"]);
                transactionData.EmployeeId = "12"; // Used for Retail, Restaurant, MOTO
                transactionData.EntryMode = (TPS.EntryMode)Enum.Parse(typeof(EntryMode), ConfigurationManager.AppSettings["TxnData_EntryMode"]);
                transactionData.GoodsType = GoodsType.DigitalGoods; // DigitalGoods - PhysicalGoods - Used only for Ecommerce
                //transactionData.AccountType = AccountType.CheckingAccount; // SavingsAccount, CheckingAccount used only for PINDebit
                transactionData.CurrencyCode = TPS.TypeISOCurrencyCodeA3.USD;
                transactionData.SignatureCaptured = false; // Required
                transactionData.IsQuasiCash = false; // Optional
                transactionData.IsPartialShipment = false; // Optional
                transactionData.TransactionDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
                transactionData.OrderNumber = "12345";
                transactionData.LaneId = "1";

                // Level 2 Data
                transactionData.Level2Data = new Level2Data();
                transactionData.Level2Data.BaseAmount = 9.00m;
                transactionData.Level2Data.OrderNumber = "12345";
                transactionData.Level2Data.Tax = new Tax();
                transactionData.Level2Data.Tax.Amount = 1.00m;
                transactionData.Level2Data.TaxExempt = new TaxExempt();
                transactionData.Level2Data.TaxExempt.IsTaxExempt = IsTaxExempt.NotExemptTaxInfoNotProvided;
                transactionData.Level2Data.DestinationPostal = "80211";
                transactionData.Level2Data.OrderDate = DateTime.Now;
                transactionData.Level2Data.Description = "level2Description";
                
                txn.TransactionData = transactionData;
                return txn;
            }
            else
            {
                var txn = new BankcardTransaction();
                txn.TenderData = tenderData;
                txn.ReportingData = reportingData;
                txn.TransactionData = new BankcardTransactionData();
                txn.TransactionData.AlternativeMerchantData = altMerchData;

                txn.TransactionData.Amount = 10.00m;
                txn.TransactionData.CashBackAmount = 0.00m;
                txn.TransactionData.TipAmount = 0.00m;
                txn.TransactionData.AccountType = AccountType.NotSet;
                txn.TransactionData.CustomerPresent = (TPS.CustomerPresent)Enum.Parse(typeof(CustomerPresent), ConfigurationManager.AppSettings["TxnData_CustomerPresent"]);
                txn.TransactionData.EmployeeId = "12"; // Used for Retail, Restaurant, MOTO
                txn.TransactionData.EntryMode =(TPS.EntryMode)Enum.Parse(typeof (EntryMode), ConfigurationManager.AppSettings["TxnData_EntryMode"]);
                txn.TransactionData.GoodsType = GoodsType.DigitalGoods; // DigitalGoods - PhysicalGoods - Used only for Ecommerce
                //txn.TransactionData.AccountType = AccountType.CheckingAccount; // SavingsAccount, CheckingAccount used only for PINDebit
                txn.TransactionData.CurrencyCode = TPS.TypeISOCurrencyCodeA3.USD;
                txn.TransactionData.SignatureCaptured = false; // Required
                txn.TransactionData.IsQuasiCash = false; // Optional
                txn.TransactionData.IsPartialShipment = false; // Optional
                txn.TransactionData.TransactionDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
                txn.TransactionData.OrderNumber = "12345";
                txn.TransactionData.LaneId = "1";
                
                return txn;
            }
        }

        public static QueryTransactionsParameters CreateQueryTransactionParameters(QueryType queryType = QueryType.OR, List<string> transactionIds = null)
        {
            var qtp = new QueryTransactionsParameters();

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["QueryType"]))
                qtp.QueryType = (QueryType)Enum.Parse(typeof(QueryType), ConfigurationManager.AppSettings["QueryType"]); // Required
            else
                qtp.QueryType = queryType;

            if (transactionIds != null)
                qtp.TransactionIds = transactionIds.ToArray();

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CaptureStartDate"]) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["CaptureEndDate"]))
            {
                qtp.CaptureDateRange = new DateRange();
                qtp.CaptureDateRange.StartDateTime = DateTime.Parse(ConfigurationManager.AppSettings["CaptureStartDate"]);
                qtp.CaptureDateRange.EndDateTime = DateTime.Parse(ConfigurationManager.AppSettings["CaptureEndDate"]);
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TransactionStartDate"]) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["TransactionEndDate"]))
            {
                qtp.TransactionDateRange = new DateRange();
                qtp.TransactionDateRange.StartDateTime = DateTime.Parse(ConfigurationManager.AppSettings["TransactionStartDate"]);
                qtp.TransactionDateRange.EndDateTime = DateTime.Parse(ConfigurationManager.AppSettings["TransactionEndDate"]);
            }
            else
            {
                qtp.TransactionDateRange = new DateRange();
                qtp.TransactionDateRange.StartDateTime = DateTime.Now.AddDays(-5);
                qtp.TransactionDateRange.EndDateTime = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsAcknowledged"]))
                qtp.IsAcknowledged = (BooleanParameter)Enum.Parse(typeof(BooleanParameter), ConfigurationManager.AppSettings["IsAcknowledged"]);

            // Other optional fields.
            //qtp.Amounts // List<Decimal> - Collection of specific transaction amount values from transaciton requests matching the amount authorized, returned, adjusted, or captured.
            //qtp.ApprovalCodes // List<String> - Collection of specific ApprovalCode values from transaction responses.
            //qtp.BatchIds // List<String> - Collection of specific BatchId values from transaction responses.
            //qtp.CaptureStates // List<CaptureState> - Collection of capture states.
            //qtp.CardTypes // List<TypeCardType> - Collection of TypeCardTypes
            //qtp.CustomerIds // List<String> - Collection of specific customerId values from transaction requests.
            //qtp.IsAcknowledged // Boolean - Indicates wheather the transaction was acknowledged.
            //qtp.MerchantProfileIds // List<String> - Collection of specific Merchant Profile values from transaction requests.
            //qtp.OrderNumbers // List<String> - Collection of specific OrderNumber values from transaction requests.
            //qtp.ServiceIds // List<String> - Collection of ServiceIds
            //qtp.ServiceKeys // List<String> - Collection of ServiceKeys
            //qtp.TransactionClassTypePairs // List<String> - Collection of specific transaction class/transaction type paris.
            //qtp.TransactionStates // List<TransactionState> - Collection of specific trasaction states
            return qtp;
        }

        public static PagingParameters CreatePagingParameters()
        {
            return new PagingParameters()
                       {
                           Page = int.Parse(ConfigurationManager.AppSettings["PageNumber"]),
                           PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"])
                       };
        }
    }
}
