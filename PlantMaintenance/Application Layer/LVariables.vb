
Module LVariables
#Region " ... General Purpose ..."

    Public v_RetVal, v_ErrCode As Long
    Public v_ErrMsg As String = ""
    Public addonName As String = "Maintenance"
    Public oCompany As SAPbobsCOM.Company
    'Attachment Option
    Public ShowFolderBrowserThread As Threading.Thread
    Public BankFileName As String
    Public boolModelForm As Boolean = False
    Public boolModelFormID As String = ""
    Public oGFun As New GFun(addonName)
    Public HANA As Boolean = True
    'Public HANA As Boolean = False
    Public HWKEY() As String = New String() {"X1658164414", "X1211807750"}

#End Region

#Region " ... Common For Forms ..."

    'ActivityMaster
    Public ActivityMasterFormID As String = "OACT"
    Public ActivityMasterXML As String = "ActivityMaster.xml"
    Public oActivityMaster As New ActivityMaster
    'Tran Data
    Public TranDataFormID As String = "TRANVIEW"
    Public TranDataFormIDXML As String = "TranView.xml"
    Public oTranDataFormID As New ClsViewTranList
    'CategoryMaster
    Public CategoryMasterFormID As String = "OCAT"
    Public CategoryMasterXML As String = "CategoryMaster.xml"
    Public oCategoryMaster As New CategoryMaster
    'PMCheckList
    Public PMCheckListFormID As String = "OPCL"
    Public PMCheckListXML As String = "PMCheckList.xml"
    Public oPMCheckList As New PMCheckList
    'ActivityPlan
    Public ActivityPlanFormID As String = "OACP"
    Public ActivityPlanXML As String = "ActivityPlan.xml"
    Public oActivityPlan As New ActivityPlan
    'Mobilization & Demobiliztion 
    Public MoDemobilizationFormID = "OMOB", MoDemobilizationXML As String = "MoDemobilization.xml"
    Public oMoDemobilization As New MoDemobilization
    'FuelDistributionEntry
    Public FuelDistributionEntryFormID As String = "OFDE"
    Public FuelDistributionEntryXML As String = "FuelDistributionEntry.xml"
    Public oFuelDistributionEntry As New FuelDistributionEntry
    'ActCarriedOut
    Public ActCarriedOutFormID As String = "OACO"
    Public ActCarriedOutXML As String = "ActCarriedOut.xml"
    Public oActCarriedOut As New ActCarriedOut
    'BreakDownSlip
    Public BreakDownSlipFormID As String = "OBDS"
    Public BreakDownSlipXML As String = "BreakDownSlip.xml"
    Public oBreakDownSlip As New BreakDownSlip
    'LogSheet
    Public LogSheetFormID As String = "OLOG"
    Public LogSheetXML As String = "LogSheet.xml"
    Public OLogSheet As New LogSheet
    'TestDriveResult
    Public TestDriveResultFormID As String = "OTDR"
    Public TestDriveResultXML As String = "TestDriveResult.xml"
    Public OTestDriveResult As New TestDriveResult
    'DirectCashPurchase
    Public DirectCashPurchaseFormID As String = "ODCP"
    Public DirectCashPurchaseXML As String = "DirectCashPurchase.xml"
    Public ODirectCashPurchase As New DirectCashPurchase
    'Specification Master
    Public SpecficationMasterFormID As String = "OSPC"
    Public SpecficationMasterXML As String = "SpecificationMaster.xml"
    Public oSpecficationMaster As New SpecificationMaster
    'Machine Master
    Public MachineMasterFormID As String = "OMAC"
    Public MachineMasterXML As String = "MachineMaster.xml"
    Public oMachineMaster As New MachineMaster
    'Vehicle Master
    Public VehicleMasterFormID As String = "OVHL"
    Public VehicleMasterXML As String = "VehicleMaster.xml"
    Public oVehicleMaster As New VehicleMaster
    'Job Card
    Public JobCardFormID As String = "OJOC"
    Public JobCardXML As String = "JobCard.xml"
    Public oJobCard As New JobCard
    'Waste Oil Disposal
    Public WasteOilDisposalFormID As String = "OWOD"
    Public WasteOilDisposalXML As String = "WasteOilDisposal.xml"
    Public oWasteOilDisposal As New WasteOilDisposal
    'Payment Certificate
    Public PaymentCertificateFormID As String = "OPAY"
    Public PaymentCertificateXML As String = "PaymentCertificate.xml"
    Public oPaymentCertificate As New PaymentCertificate

    'TripMaster
    Public TripMasterFormID As String = "OTRP"
    Public TripMasterXML As String = "TripMaster.xml"
    Public oTripMaster As New TripMaster

    'VehicleCostAllocation
    Public VehicleCostAllocationFormID As String = "OVCA"
    Public VehicleCostAllocationXML As String = "VehicleCostAllocation.xml"
    Public oVehicleCostAllocation As New VehicleCostAllocation

    'LicenseDetails
    Public LicenseDetailsFormID As String = "OLAC"
    Public LicenseDetailsXML As String = "LicenseDetails.xml"
    Public oLicenseDetails As New LicenseDetails
    'Card Renewal Entry 
    Public CardRenewalEntryFormID = "OCRD", CardRenewalEntryXML As String = "CardRenewalEntry.xml"
    Public oCardRenewalEntry As New CardRenewalEntry

    'Goods issue

    Public oGoodsIssue As New ClsGoodsIssue
#End Region

End Module