Public Class TableCreation
    Dim ValidValueYesORNo = New String(,) {{"N", "No"}, {"Y", "Yes"}}
    Dim ValidValueOpenORClose = New String(,) {{"O", "Open"}, {"C", "Closed"}}

    Sub New()
        '  If oCompany.UserSignature <> 1 Then Exit Sub
        Me.TableCreation()
    End Sub

#Region "       ... Table Creation ...      "

    Sub TableCreation()
        '   Masters ...
        Me.CategoryMaster()
        Me.ActivityMaster()
        Me.SpecificationMaster()
        Me.MachineMaster()
        Me.VehicleMaster()
        Me.PMCheckList()

        ''   Transactions ...
        Me.ActivityPlan()
        Me.ActivityCarriedOut()
        Me.BeakDownSlip()
        Me.JobCard()
        'Me.MoDemobilization()
        'Me.TripMaster()
        'Me.LicenseDetails()
        'Me.LogSheet()
        'Me.FuelDistributionEntry()
        'Me.DirectCashPurchase()
        'Me.TestDriveResult()
        'Me.WasteOilDisposal()
        'Me.DriverTripDetails()
        'Me.VehicleAllocationReq()
        'Me.PaymentCertificate()
        'Me.VehicleCostAllocation()
        'Me.CardRenewalEntry()  
        oGFun.CreateTable("MIPL_PM_OMRC", "PM Manufacturers", SAPbobsCOM.BoUTBTableType.bott_NoObject)

        oGFun.CreateTable("MIPL_PM_TYPE", "PM Type Master", SAPbobsCOM.BoUTBTableType.bott_NoObject)
        oGFun.CreateUserFields("@MIPL_PM_TYPE", "TypeCode", "Type Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
        oGFun.CreateUserFields("@MIPL_PM_TYPE", "TypeName", "Type Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
        oGFun.CreateTable("MIPL_PM_ITGRP", "PM ItemGroup Master", SAPbobsCOM.BoUTBTableType.bott_NoObject)
        oGFun.CreateUserFields("OITM", "IGRP", "Item Group", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
        oGFun.CreateUserFields("OITM", "PrtNo", "Part No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
        oGFun.CreateUserFields("OIGE", "JobNo", "JobCard No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        oGFun.CreateUserFields("OJDT", "JobNo", "JobCard No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        oGFun.CreateUserFields("OIGE", "MCOutNo", "MCarriedOut No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        oGFun.CreateUserFields("OJDT", "MCOutNo", "MCarriedOut No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        oGFun.CreateUserFields("OHEM", "HrCost", "Hr Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        'oGFun.CreateTable("MIPL_PM_GL", "GL Mapping Details", SAPbobsCOM.BoUTBTableType.bott_NoObject)
        'oGFun.CreateUserFields("@MIPL_PM_GL", "CreditGL", "Credit GLCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
        'oGFun.CreateUserFields("@MIPL_PM_GL", "DebitGL", "Debit GLCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
        'oGFun.CreateUserFields("@MIPL_PM_GL", "GIGL", "GoodsIssue GLCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)

        oGFun.CreateTable("MIPL_PM_MM", "PM Measurement Master", SAPbobsCOM.BoUTBTableType.bott_NoObject)
        oGFun.CreateUserFields("@MIPL_PM_MM", "MCode", "Machine/Vehicle ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
        oGFun.CreateUserFields("@MIPL_PM_MM", "MName", "Machine/Vehicle ID Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
        oGFun.CreateUserFields("@MIPL_PM_MM", "DocDate", "Doc Date", SAPbobsCOM.BoFieldTypes.db_Date)
        oGFun.CreateUserFields("@MIPL_PM_MM", "Temp", "Temperature", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        oGFun.CreateUserFields("@MIPL_PM_MM", "Vibrate", "Vibration", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        oGFun.CreateUserFields("@MIPL_PM_MM", "Pressure", "Pressure", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        oGFun.CreateUserFields("@MIPL_PM_MM", "RunKM", "Running KM", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        oGFun.CreateUserFields("@MIPL_PM_MM", "RunHrs", "Running Hrs", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
        oGFun.CreateUserFields("@MIPL_PM_MM", "FuelCons", "Fuel Consumption", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)


    End Sub

#End Region

#Region "       ... Category Master ...     "

    Sub CategoryMaster()
        Try
            Me.CategoryMasterHead()
            If Not oGFun.UDOExists("MICAT") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Doc No"}, {"U_Type", "Type"}, {"Name", "Category"}}
                oGFun.RegisterUDO("MICAT", "PM Category Master", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OCAT", , , , , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Category Master Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub CategoryMasterHead()
        Try
            Dim cmb_CategoryType = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            oGFun.CreateTable("MIPL_PM_OCAT", "PM Category Master", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OCAT", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , , cmb_CategoryType)
            oGFun.CreateUserFields("@MIPL_PM_OCAT", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Category Master Header Table : " & ex.Message)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... Activity Master ...     "

    Sub ActivityMaster()
        Try
            Me.ActivityMasterHead()
            If Not oGFun.UDOExists("OACT") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}, {"U_activity", "Activity"}}
                oGFun.RegisterUDO("OACT", "PM Activity Master", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OACT")
                findAliasNDescription = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity  Master Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub ActivityMasterHead()
        Try
            oGFun.CreateTable("MIPL_PM_OACT", "PM Activity Master", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            oGFun.CreateUserFields("@MIPL_PM_OACT", "CatCode", "Category Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_OACT", "CatName", "Category Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OACT", "Activity", "Activity", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_OACT", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Master Header Table : " & ex.Message)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... Specification Master ...        "

    Sub SpecificationMaster()
        Try
            Me.SpecificationMasterHead()
            If Not oGFun.UDOExists("OSPC") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}, {"Name", "Spec. Description"}}
                oGFun.RegisterUDO("OSPC", "Specificaiton Master", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OSPC", , , , , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub SpecificationMasterHead()
        Try
            oGFun.CreateTable("MIPL_PM_OSPC", "Specification Master", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            oGFun.CreateUserFields("@MIPL_PM_OSPC", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Machine/Equipment Master ...        "

    Sub MachineMaster()
        Try

            Me.MachineMasterHead()
            Me.MachineEquipmentInsertDetail()
            Me.MachineEquipmentSpecDetail()
            Me.MachineEquipmentAuxiliaryDetail()
            Me.MachineEquipmentCriticalSparesDetail()

            If Not oGFun.UDOExists("OMAC") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}, {"U_ItemCode", "Machine Code"}, {"Name", "Machine Name"}}
                oGFun.RegisterUDO("OMAC", "Machine/Equipment Master", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OMAC", "MIPL_PM_MAC1", "MIPL_PM_MAC2", "MIPL_PM_MAC3", "MIPL_PM_MAC4", , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub MachineMasterHead()
        Try

            Dim WarrantyType = New String(,) {{"-", "-"}, {"A", "AMC"}, {"W", "Warranty"}, {"G", "Guarantee"}}
            Dim LifeType = New String(,) {{"Y", "Year"}, {"Q", "Quantity"}}
            Dim Type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}}

            oGFun.CreateTable("MIPL_PM_OMAC", "Machine/Equipment Master", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Location", "Lcoation", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Group", "Items Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ItemName", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ManuFact", "Manufactured", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RecPrd", "Recovery Period", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Depcn", "Depriciation", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Percentage)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Dept", "Department", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "CatCode", " Category Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "CatName", "Category Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "DefWhse", "Default Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OMAC", "InsType", "Instrument Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , "", Type)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "SerialNo", "Serial No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            ' oGFun.CreateUserFields("@MIPL_PM_OMAC", "Manufact", "Manufacturer", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PurFrom", "Purchase From", SAPbobsCOM.BoFieldTypes.db_Alpha, 15)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "IncharCd", "Tool Incharge", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "IncharNm", "Tool Incharge(Name)", SAPbobsCOM.BoFieldTypes.db_Alpha, 41)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "OwnrID", "Owner ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "OwnrNam", "Owner Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "InvNoDt", "Invice No./Date", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "InstalDt", "Install Date", SAPbobsCOM.BoFieldTypes.db_Date)

            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OMAC", "WAType", "Warrenty/AMC", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", WarrantyType, "-")
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "WACard", "Warranty Cardcode", SAPbobsCOM.BoFieldTypes.db_Alpha, 15)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AMCPONo", "AMC Order No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AMCPOEntry", "AMC Order Entry.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AMCPODate", "AMC Order Date", SAPbobsCOM.BoFieldTypes.db_Date)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "WarStDt", "Warrenty Start Dt", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "WarEndDt", "Warrenty End Dt", SAPbobsCOM.BoFieldTypes.db_Date)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PMCheck", "PM CheckList No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Strokes", "Strokes", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "SetTIme", "Setup Time in Mins", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RecdFreq", "Recondition Freq.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RecondDt", "Recondition Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PurFrmCd", "Suplier code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PurFrmNm", "Suplier name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ModelNo", "Model No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RegNo", "RegNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ChaID", "ChaID", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "MadeIn", "Made In", SAPbobsCOM.BoFieldTypes.db_Alpha, 3)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Weight", "Tool Weight", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "UOM", "UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AvailDay", "Available Day", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)

            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OMAC", "LifeType", "Life Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", LifeType, "")
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Side", "Tool Side", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Usage", "No of Usage", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Life", "Tool Life", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RatePrHR", "Cost per hr", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Note", "Note", SAPbobsCOM.BoFieldTypes.db_Memo, 800, SAPbobsCOM.BoFldSubTypes.st_None)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "LifeCycl", "Completed Life Cycle", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Strokqty", "Standard Strokes per Qty.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "StrkComp", "No.of Stroke Cmptd.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RecndStk", "After Recondition Cmptd.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AmtLife", "Amortisation Life.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AmtComp", "Amortisation Life Cmptd.", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "AmtValue", "Amortisation Value", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Amtrecd", "Amortisation Value Recovered", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "ToolImg", "Tool/Fixture Image", SAPbobsCOM.BoFieldTypes.db_Alpha, 250)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "StpCost", "Setup Time Cost Per Hour", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "OtrCost", "Other Cost Per Hour", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 800)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PurPrice", "Purchased Price", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "PurcDat", "Purchased Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "DatMul", "DatMul", SAPbobsCOM.BoFieldTypes.db_Date)


            oGFun.CreateUserFields("@MIPL_PM_OMAC", "CCont", "CCont", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "CBal", "CBal", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RunHours", "Running Hours", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Ltdate", "Ltdate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Capacity", "Capacity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "CapyUOM", "Capacity UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            Dim Status = New String(,) {{"A", "Active"}, {"H", "Hold"}, {"I", "Inactive"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OMAC", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", Status, "A")

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Attach1", "Attachment 1", SAPbobsCOM.BoFieldTypes.db_Memo, , SAPbobsCOM.BoFldSubTypes.st_Link)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Attach2", "Attachment 2", SAPbobsCOM.BoFieldTypes.db_Memo, , SAPbobsCOM.BoFldSubTypes.st_Link)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Attach3", "Attachment 3", SAPbobsCOM.BoFieldTypes.db_Memo, , SAPbobsCOM.BoFldSubTypes.st_Link)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Monthly", "Monthly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Weekly", "Weekly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Daily", "Daily", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Hourly", "Hourly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)

            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Temp", "Temperature", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Vibrate", "Vibration", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "Pressure", "Pressure", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OMAC", "RunKM", "Running KM", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            'oGFun.CreateUserFields("@MIPL_PM_OMAC", "RunHrs", "Running Hrs", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub MachineEquipmentInsertDetail()
        Try
            oGFun.CreateTable("MIPL_PM_MAC2", "Machine/Equipment Spare Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_MAC2", "ItemCode", "Spare No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_MAC2", "ItemName", "Spare Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub MachineEquipmentSpecDetail()
        Try
            oGFun.CreateTable("MIPL_PM_MAC1", "Machine/Equipment Spec. Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_MAC1", "SpecNo", "Spec. NO", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_MAC1", "SpecName", "Spec Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_MAC1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub MachineEquipmentAuxiliaryDetail()
        Try
            oGFun.CreateTable("MIPL_PM_MAC3", "Machine/Equip Aux. Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_MAC3", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_MAC3", "ItemDesc", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_MAC3", "Quant", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_MAC3", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 200)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub MachineEquipmentCriticalSparesDetail()
        Try
            Dim cmb_Type = New String(,) {{"L", "Local"}, {"O", "Overseas"}}
            Dim cmb_Priority = New String(,) {{"H", "High"}, {"M", "Medium"}, {"N", "Normal"}}
            oGFun.CreateTable("MIPL_PM_MAC4", "Machine/Equip Spares. Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_MAC4", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "ItemDesc", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "Quant", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "ItmGrp", "Item Group", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_MAC4", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, , , cmb_Type)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_MAC4", "Prior", "Priority", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, , , cmb_Priority)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "PrtNo", "Part No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "UOM", "UOM Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "MIN", "Min Inventory", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "Max", "Max Inventory", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "InStock", "In Stock", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_MAC4", "Attach", "Attachment", SAPbobsCOM.BoFieldTypes.db_Memo, , SAPbobsCOM.BoFldSubTypes.st_Link)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... VehicleMaster ...       "

    Sub VehicleMaster()
        Try
            Me.VehicleMasterHeader()
            Me.VehicleMasterDetail()
            Me.VehicleMasterAuxiliaryDetail()
            Me.VehicleMasterCriticalSparesDetail()

            If Not oGFun.UDOExists("MIVHL") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}, {"U_ItemCode", "Item Code"}}
                oGFun.RegisterUDO("MIVHL", "Vehicle Master Data", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OVHL", "MIPL_PM_VHL1", "MIPL_PM_VHL2", "MIPL_PM_VHL3", , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub VehicleMasterHeader()
        Try
            Dim FuelType = New String(,) {{"P", "Petrol"}, {"D", "Diesel"}, {"G", "Gasolin"}}
            oGFun.CreateTable("MIPL_PM_OVHL", "Vehicle Master Header", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            Dim status = New String(,) {{"A", "Active"}, {"I", "Inactive"}, {"S", "Sold"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OVHL", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", status, "A")

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, 150)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Make", "Make", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Model", "Model", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "ChaID", "ChaID", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 250)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuCmpy", "Company", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuPlcy", "Policy", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuSDat", "Start Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuEDat", "End Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsPymnt", "Payment", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuBal", "Deductible", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "ExprDate", "ExprDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "StExpDat", "StExpDat", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "CardCode", "Dealer Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "CardName", "Dealer Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PurDate", "PurDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PurMilg", "Running", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PurMgUOM", "UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PurCost", "Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PurCmds", "Comments", SAPbobsCOM.BoFieldTypes.db_Memo, 250)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InsuRmks", "Notes", SAPbobsCOM.BoFieldTypes.db_Memo, 800)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "EngNo", "Engine No", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OVHL", "FuelType", "Fuel Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", FuelType, "")
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Whse", "Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, 15)

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "CntryOrg", "Origin", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Color", "Color", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "VliDate", "Valididty Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "sctn", "Safty Certificate", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "DatMul", "Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "AAAserv", "Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Petcrdno", "Petrol card", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "ExpDate", "Expire Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Itmgrp", " Item group", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Location", " Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "CatCode", "Category Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "VechType", "Category Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "SeatCap", "Seat Capacity", SAPbobsCOM.BoFieldTypes.db_Numeric)

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "RegNo", "Registration No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "RecPerid", "Recovery Period", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Deprecn", "Deprication", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InvcNo", "Invoice Number", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "InvcDate", "Invoice Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "PolicyNm", "Police Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "RunHours", "RunHours", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Make", "Make", SAPbobsCOM.BoFieldTypes.db_Alpha, 150)

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Monthly", "Monthly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Weekly", "Weekly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Daily", "Daily", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Hourly", "Hourly", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "RunHours", "Running Hours", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)

            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Temp", "Temperature", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Vibrate", "Vibration", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "Pressure", "Pressure", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OVHL", "RunKM", "Running KM", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            'oGFun.CreateUserFields("@MIPL_PM_OMAC", "RunHrs", "Running Hrs", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)


        Catch ex As Exception
            oApplication.StatusBar.SetText("Vehicle Master Header failed to create", ex.Message)
        End Try
    End Sub

    Sub VehicleMasterDetail()
        Try
            oGFun.CreateTable("MIPL_PM_VHL1", "Vehicle Master Attachment", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_VHL1", "TrgtPath", "TrgtPath", SAPbobsCOM.BoFieldTypes.db_Memo, 800)
            oGFun.CreateUserFields("@MIPL_PM_VHL1", "ScrPath", "ScrPath", SAPbobsCOM.BoFieldTypes.db_Memo, 800)
            oGFun.CreateUserFields("@MIPL_PM_VHL1", "FileName", "FileName", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_VHL1", "FileExt", "FileExt", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_VHL1", "Date", "Date", SAPbobsCOM.BoFieldTypes.db_Date)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Vehicle Master Attachment failed to create", ex.Message)
        End Try
    End Sub

    Sub VehicleMasterAuxiliaryDetail()
        Try
            oGFun.CreateTable("MIPL_PM_VHL2", "Vehicle Master Aux. Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_VHL2", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_VHL2", "ItemDesc", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VHL2", "Quant", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_VHL2", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 200)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub VehicleMasterCriticalSparesDetail()
        Try
            Dim cmb_Type = New String(,) {{"L", "Local"}, {"O", "Overseas"}}
            Dim cmb_Priority = New String(,) {{"H", "High"}, {"M", "Medium"}, {"N", "Normal"}}
            oGFun.CreateTable("MIPL_PM_VHL3", "Vehicle Master Spares. Dets.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_VHL3", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "ItemDesc", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "Quant", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "ItmGrp", "Item Group", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_VHL3", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, , , cmb_Type)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_VHL3", "Prior", "Priority", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, , , cmb_Priority)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "PrtNo", "Part No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "UOM", "UOM Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "Whse", "Whse Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "MIN", "Min Inventory", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "Max", "Max Inventory", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "InStock", "In Stock", SAPbobsCOM.BoFieldTypes.db_Float, 10, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_VHL3", "Attach", "Attachment", SAPbobsCOM.BoFieldTypes.db_Memo, , SAPbobsCOM.BoFldSubTypes.st_Link)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub
#End Region

#Region "       ... PM Check List ...       "

    Sub PMCheckList()
        Try
            Me.PMCheckListHead()
            Me.PMCheckListDetail()
            If Not oGFun.UDOExists("OPCL") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Check List No."}, {"Name", "Check List Name"}}
                oGFun.RegisterUDO("OPCL", "PM Check List", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OPCL", "MIPL_PM_PCL1", , , , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create PM Check List Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub PMCheckListHead()
        Try
            Dim type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            Dim status = New String(,) {{"O", "Opened"}, {"C", "Closed"}}

            Dim freq = New String(,) {{"Y", "Yes"}, {"N", "No"}}
            oGFun.CreateTable("MIPL_PM_OPCL", "PM Check List Head", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            oGFun.CreateUserFields("@MIPL_PM_OPCL", "ChkType", "CheckList Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "DocDate", "Doc. Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "Dept", "Department", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OPCL", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", type, "MC")
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "category", "Category", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "catcode", "Category Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "prepby", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "authby", "Authorised By", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "precode", "PreBy Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "authcode", "AuthBy Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OPCL", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", status, "O")
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "Active", "Active", SAPbobsCOM.BoFieldTypes.db_Alpha, 1)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OPCL", "FrqYesNo", "FrequencyYesNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", freq, "N")
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "Freq", "Frequency", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)

            oGFun.CreateUserFields("@MIPL_PM_OPCL", "ChkName", "CheckList Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 150)

            Dim cmb_Frequency = New String(,) {{"0", "Daily"}, {"1", "Weekly"}, {"2", "Monthly"}, {"3", "Quarterly"}, {"4", "Half Yearly"}, {"5", "Yearly"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OPCL", "Freqncy", "Frequency List Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , cmb_Frequency)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "Reading", "Reading", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OPCL", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create PM Check List Header Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub PMCheckListDetail()
        Try
            oGFun.CreateTable("MIPL_PM_PCL1", "PM Check List Detail", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_PCL1", "activity", "Activity", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_PCL1", "actcode", "Activity Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_PCL1", "genobsv", "Parameters", SAPbobsCOM.BoFieldTypes.db_Alpha, 160, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_PCL1", "method", "Methods", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_PCL1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 800, SAPbobsCOM.BoFldSubTypes.st_None, "")

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create PM Check List Detail Table : " & ex.Message)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... TripMaster ...     "

    Sub TripMaster()
        Try
            Me.TripMasterHead()
            If Not oGFun.UDOExists("OTRP") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}}
                oGFun.RegisterUDO("OTRP", "TripMaster", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OTRP")
                findAliasNDescription = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity  Master Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub TripMasterHead()
        Try
            Dim type1 = New String(,) {{"L", "Lowbed"}, {"H", "Highbed"}, {"O", "OverSize Metrerial Shifting"}}
            oGFun.CreateTable("MIPL_PM_OTRP", "TripMaster", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            oGFun.CreateUserFields("@MIPL_PM_OTRP", "Code", "Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_OTRP", "TName", "TName", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OTRP", "TType", "TType", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", type1, "")
            oGFun.CreateUserFields("@MIPL_PM_OTRP", "TCharge", "TCharge", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_OTRP", "TAlownce", "TAlownce", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create TripMaster Header Table : " & ex.Message)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... LicenseDetails ...     "

    Sub LicenseDetails()
        Try
            Me.LicenseDetailsHead()
            Me.LicenseDetailsDet1()
            If Not oGFun.UDOExists("OLAC") Then
                Dim findAliasNDescription = New String(,) {{"Code", "Code"}}
                oGFun.RegisterUDO("OLAC", "LicenseHeader", SAPbobsCOM.BoUDOObjType.boud_MasterData, findAliasNDescription, "MIPL_PM_OLAC", "MIPL_PM_LAC1")
                findAliasNDescription = Nothing
            End If


        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity  Master Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub LicenseDetailsHead()
        Try

            oGFun.CreateTable("MIPL_PM_OLAC", "LicenseHeader", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            oGFun.CreateUserFields("@MIPL_PM_OLAC", "EmpID", "Employee Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_OLAC", "EmpName", "Employee Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create LicenseDetails Header Table : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub LicenseDetailsDet1()
        Try
            oGFun.CreateTable("MIPL_PM_LAC1", "LicenseDetails", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)

            oGFun.CreateUserFields("@MIPL_PM_LAC1", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "LCode", "License Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "LName", "License Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "CardNo", "CardNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "ExpirDt", "Expiry Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "IssueDt", "Issue Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_LAC1", "IsuePlce", "Issue Place", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create LicenseDet1  Table : " & ex.Message)
        Finally
        End Try
    End Sub




#End Region

    '   ... Transaction ...

#Region "       ... Activity Plan ..."
    Sub ActivityPlan()
        Try

            Me.ActivityPlanHeader()
            Me.ScheduleactivityPlanDetail()
            Me.ScheduleactivityItem()

            If Not oGFun.UDOExists("OACP") Then
                Dim findAliasNDescription = New String(,) {{"DocNum", "Doc. ID"}, {"U_DocDate", "Doc. Date"}, {"U_Dept", "Department"}}
                oGFun.RegisterUDO("OACP", "Maintenance Plan", SAPbobsCOM.BoUDOObjType.boud_Document, findAliasNDescription, "MIPL_PM_OACP", "MIPL_PM_ACP1", "MIPL_PM_ACP2", , , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Schedued Activity Plan: " & ex.Message)
        Finally
        End Try
    End Sub
    Sub ActivityPlanHeader()
        Try
            Dim type1 = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            oGFun.CreateTable("MIPL_PM_OACP", "PM Maintenance Plan", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OACP", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_OACP", "DocDate", "Doc. Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "Dept", "Department", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OACP", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", type1, "MC")
            oGFun.CreateUserFields("@MIPL_PM_OACP", "Category", "Category", SAPbobsCOM.BoFieldTypes.db_Alpha, 60)

            oGFun.CreateUserFields("@MIPL_PM_OACP", "MacNo", "Machine/Tool/Fixture/Inst No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "MacDesc", "Machine/Tool/Fixt./Inst Desc.", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "PMCNo", "Check List No", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "PMCName", "Check List Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "SchedDt", "Schedule Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "CatCode", "Category Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)


            oGFun.CreateUserFields("@MIPL_PM_OACP", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "PreByNam", "Prepared By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 250, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "AppByCod", "Approval By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "AppByNam", "Approved By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 250, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACP", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Schedue Activity Plan Header : " & ex.Message)
        Finally
        End Try
    End Sub
    Sub ScheduleactivityPlanDetail()
        Try
            oGFun.CreateTable("MIPL_PM_ACP1", "PM Maintenance Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_ACP1", "ActCode", "Activity Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_ACP1", "ActName", "Activity Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ACP1", "Parametr", "Parameters", SAPbobsCOM.BoFieldTypes.db_Alpha, 160)
            oGFun.CreateUserFields("@MIPL_PM_ACP1", "remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)

            ' oGFun.CreateUserFields("@MIPL_PM_ACP1", "schdt", "Schedule Date", SAPbobsCOM.BoFieldTypes.db_Date)
            ' oGFun.CreateUserFields("@MIPL_PM_ACP1", "Freq", "Frequency", SAPbobsCOM.BoFieldTypes.db_Alpha, 30, SAPbobsCOM.BoFldSubTypes.st_None, "")
            ' oGFun.CreateUserFields("@MIPL_PM_ACP1", "nxtschdt", "Next Schedule Date", SAPbobsCOM.BoFieldTypes.db_Date)
            ' oGFun.CreateUserFields("@MIPL_PM_ACP1", "Months", "Months", SAPbobsCOM.BoFieldTypes.db_Numeric)
            ' oGFun.CreateUserFields("@MIPL_PM_ACP1", "Years", "Years", SAPbobsCOM.BoFieldTypes.db_Numeric)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Schedule Activity Plan Detail : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub ScheduleactivityItem()
        Try
            oGFun.CreateTable("MIPL_PM_ACP2", "PM Maintenance Items", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_ACP2", "ItemCode", "Item ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_ACP2", "ItemName", "Item Description.", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ACP2", "UOM", "U.O.M", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            ' oGFun.CreateUserFields("@MIPL_PM_ACP2", "stock", "Stock", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity, "")
            oGFun.CreateUserFields("@MIPL_PM_ACP2", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity, "")
            oGFun.CreateUserFields("@MIPL_PM_ACP2", "remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Schedule Activity Plan Item : " & ex.Message)
        Finally
        End Try
    End Sub
#End Region

#Region "       ... Activity Carried Out ...        "
    Sub ActivityCarriedOut()
        Try

            Me.activitycarriedOutHeader()
            Me.ActivityCarriedOutDetail()
            Me.ActivityCarriedOutItem()
            Me.ActivityManHourCost()

            If Not oGFun.UDOExists("OACO") Then
                Dim findAliasNDescription = New String(,) {{"DocNum", "Doc. ID"}, {"U_DocDate", "Doc. Date"}}
                oGFun.RegisterUDO("OACO", "Activity Carried Out", SAPbobsCOM.BoUDOObjType.boud_Document, findAliasNDescription, "MIPL_PM_OACO", "MIPL_PM_ACO1", "MIPL_PM_ACO2", "MIPL_PM_ACO3", , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Carried Out: " & ex.Message)
        Finally
        End Try
    End Sub
    Sub activitycarriedOutHeader()
        Try
            Dim Type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            Dim Frequency = New String(,) {{"1", "Daily"}, {"2", "Weekly"}, {"3", "Monthly"}, {"4", "Quarterly"}, {"5", "Half Yearly"}, {"6", "Annualy"}, {"7", "Shift"}}

            oGFun.CreateTable("MIPL_PM_OACO", "Activity carried Out", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OACO", "DocDate", "Doc. Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OACO", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , "", Type, "")
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OACO", "Freqncy", "Frequency", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Frequency, "")
            oGFun.CreateUserFields("@MIPL_PM_OACO", "empId", "Employee ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "empName", "Employee Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "MACNo", "Machine/Tool/Fixture/Inst No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "MacName", "Machine/Tool/Fixt./Inst Desc.", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "SchPlanNo", "Maintenance Plan No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "PMCNo", "Check List No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "PMCName", "Check List Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "CurRead", "Current Reading", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "GrandTot", "Grand Total", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "Service", "Service Due", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)

            oGFun.CreateUserFields("@MIPL_PM_OACO", "GINo", "GoodsIssue No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OACO", "JENo", "JournalEntry No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Carried Out : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub ActivityCarriedOutDetail()
        Try
            oGFun.CreateTable("MIPL_PM_ACO1", "Activity Carried Out" + "Activities", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_ACO1", "LineNum", "Base Line Number", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "ActCode", "Activity Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "ActName", "Activity Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "Service", "Service", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "Satisfac", "Satisfactory", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "actCO", "Carried Out Details", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            Dim Status = New String(,) {{"P", "Pending"}, {"K", "Skipped"}, {"C", "Completed"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_ACO1", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Status, "P")
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 160, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "CompDat", "Completed Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "PREntry", "PR Entry", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "Linetot", "PR Linetot", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "ItemCode", "PR ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_ACO1", "ItemName", "PR ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Carried Out Activity : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub ActivityCarriedOutItem()
        Try
            oGFun.CreateTable("MIPL_PM_ACO2", "Activity Acrriedout" + "Items", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_ACO2", "ItemCode", "Item ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "ItemName", "Description.", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "UOM", "U.O.M", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "AvgPrice", "AvgPrice", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "Total", "Total", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "Costcenter", "Cost Center.", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_ACO2", "LStat", "Line Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , "", ValidValueOpenORClose, "O")
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "IssQty", "Issue Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "OpenQty", "Open Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_ACO2", "GINo", "GoodsIssue No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Carried Out Item : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub ActivityManHourCost()
        Try
            oGFun.CreateTable("MIPL_PM_ACO3", "Activity ManHour Cost", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_ACO3", "empID", "emp ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "empName", "Employee Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "ManHrs", "Man Hours", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "HrCost", "Hr Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "TotCost", "Total Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_ACO3", "Stat", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 8, SAPbobsCOM.BoFldSubTypes.st_None, , "O")
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Activity Carried Man Hours : " & ex.Message)
        Finally
        End Try
    End Sub
#End Region

#Region "       ... BeakDownSlip ...        "
    Sub BeakDownSlip()
        Try
            Me.BeakDownSlipHead()
            Me.BeakDownSlipDetail()
            If Not oGFun.UDOExists("OBDS") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "Document Number"}, {"U_DocDate", "Document Date"}}
                oGFun.RegisterUDO("OBDS", "Beak Down Slip ", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OBDS", "MIPL_PM_BDS1", , , , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Bread Down Slip Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub BeakDownSlipHead()
        Try
            Dim Type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            Dim BType = New String(,) {{"BDS", "Breakdown Slip"}, {"MRM", "Maintenance Request(Marine)"}, {"MRT", "Maintenance Request(Terminal)"}}

            oGFun.CreateTable("MIPL_PM_OBDS", "BeakDownSlip Head", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OBDS", "DocDate", "Document Date ", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            'oGFun.CreateUserFields("@MIPL_PM_OBDS", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OBDS", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , "", Type, "")
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "PrjCode", "Project Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "PrjName", "Project Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "ItemName", "ItemName ", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "RepairDt", "U_Repair Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "RptByCod", "Report By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "RptByNam", "Report By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "CmpDate", "Completion Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "Details", "Details", SAPbobsCOM.BoFieldTypes.db_Memo)

            oGFun.CreateUserFields("@MIPL_PM_OBDS", "SupByCod", "Supervisor Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "SupByNam", "Supervisor Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "PmdByCod", "PMD Manager Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "PmdByNam", "PMD Manager Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OBDS", "BDType", "Breakdown Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 5, , "", BType, "")
            oGFun.CreateUserFields("@MIPL_PM_OBDS", "AtchFile", "Attachment File", SAPbobsCOM.BoFieldTypes.db_Memo, 100, SAPbobsCOM.BoFldSubTypes.st_Link)

        Catch ex As Exception
            oApplication.StatusBar.SetText("BeakDownSlip Head Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub BeakDownSlipDetail()
        Try
            oGFun.CreateTable("MIPL_PM_BDS1", "BeakDownSlip Dets.", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_BDS1", "Analysis", "Analysis Details", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_BDS1", "ActSugg", "Action Suggested", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_BDS1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Job Card ...     "

    Sub JobCard()
        Try
            Me.JobCardHead()
            Me.JobCardDetails1()
            Me.JobCardDetails2()
            Me.JobCardDetails3()

            If Not oGFun.UDOExists("OJOC") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OJOC", "PMD JobCard", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OJOC", "MIPL_PM_JOC1", "MIPL_PM_JOC2", "MIPL_PM_JOC3", , , , , , , , , SAPbobsCOM.BoYesNoEnum.tYES)
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Table Creation Job Card Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub JobCardHead()
        Try
            oGFun.CreateTable("MIPL_PM_OJOC", " JobCard Header ", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OJOC", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)

            Dim Type = New String(,) {{"B", "Breakdown/Maintenance Request"}, {"S", "Service"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OJOC", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Type, "")
            Dim Type1 = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OJOC", "VHLType", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , , Type1, "MC")

            oGFun.CreateUserFields("@MIPL_PM_OJOC", "BrkDwNo", "Break Down No", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "ServType", "Service Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "VHLNo", "Vehicle Number", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "VHLName", "Vehicle Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "MainPlNo", "Maintenance Plan Number", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "RegExpDt", "Reg.Expiry Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "PrjCode", "Project Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "PrjName", "Project Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "DriveCod", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "DriveNam", "Drive Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "HrsKms", "Hrs/Kms", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "PreByCod", "Prepared By ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "PreByNam", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "MechCode", "Mechanic Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "MechName", "Mechanic Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "MnPlnCd", "MainPlan Cod", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "Remarks", "Remarsk", SAPbobsCOM.BoFieldTypes.db_Memo)

            oGFun.CreateUserFields("@MIPL_PM_OJOC", "GINo", "GoodsIssue No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OJOC", "JENo", "JournalEntry No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)

            oGFun.CreateUserFields("@MIPL_PM_OJOC", "GrandTot", "Grand Total", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Job Card Header failed to create", ex.Message)
        End Try
    End Sub

    Sub JobCardDetails1()
        Try
            oGFun.CreateTable("MIPL_PM_JOC1", "PMD Job Card Details1", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ActCode", "Activity Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ActName", "Activity Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ReprDat", "Repair Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "CompDat", "Completed Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "StartTim", "Start Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "EndTime", "End Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "AStartTim", "Actual Start Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "AEndTime", "Actual End Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ActCost", "Activity Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "MechCode", "Mechanic Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "MechName", "Mechanic Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            Dim Status = New String(,) {{"P", "Pending"}, {"K", "Skipped"}, {"C", "Completed"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_JOC1", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Status, "P")
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 254)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "PREntry", "PR Entry", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "Linetot", "PR Linetot", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ItemCode", "PR ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_JOC1", "ItemName", "PR ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Job Card Details1 failed to create", ex.Message)
        End Try
    End Sub

    Sub JobCardDetails2()
        Try
            oGFun.CreateTable("MIPL_PM_JOC2", "PMD Job Card Details2", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_JOC2", "ItemCode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "ItemName", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "Uom", "Unit of Measurement", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "AvgPrice", "Average Price", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "Total", "Total", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)
            Dim Status = New String(,) {{"P", "Pending"}, {"C", "Completed"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_JOC2", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Status, "P")
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 254)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_JOC2", "LStat", "Line Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, , "", ValidValueOpenORClose, "O")
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "Costcenter", "Cost Center.", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "IssQty", "Issue Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "OpenQty", "Open Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC2", "GINo", "GoodsIssue No", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Job Card Details2 failed to create", ex.Message)
        End Try
    End Sub

    Sub JobCardDetails3()
        Try
            oGFun.CreateTable("MIPL_PM_JOC3", "JobCard ManHour Cost", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_JOC3", "empID", "emp ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "empName", "Employee Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "ManHrs", "Man Hours", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "HrCost", "Hr Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "TotCost", "Total Cost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_JOC3", "Stat", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 8, SAPbobsCOM.BoFldSubTypes.st_None, , "O")
        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Job Card Details3 : " & ex.Message)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... Log Sheet ...        "

    Sub LogSheet()
        Try
            Me.LogSheetHead()
            Me.LogSheetDetails()

            If Not oGFun.UDOExists("OLOG") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OLOG", "LogSheet", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OLOG", "MIPL_PM_LOG1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("TableCreation Daily Form Entry Method Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub LogSheetHead()
        Try
            oGFun.CreateTable("MIPL_PM_OLOG", "Log Sheet Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OLOG", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            Dim WorkType = New String(,) {{"I", "Internal"}, {"E", "External"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OLOG", "WorkType", "Work Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", WorkType, "I")
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "OpCode", "Operator Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "OpName", "Operator Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            ' oGFun.CreateUserFields("@MIPL_PM_OLOG", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "CardCode", "Customer Code ", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "CardName", "Customer Name ", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "FromDate", "From Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "ToDate", "To Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "PreByNam", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "AppByCod", "Approved By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "AppByNam", "Approved by", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OLOG", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed To create Log sheet Header", ex.Message)
        End Try
    End Sub

    Sub LogSheetDetails()
        Try
            oGFun.CreateTable("MIPL_PM_LOG1", "Log Sheet Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}


            oGFun.CreateUserFields("@MIPL_PM_LOG1", "Date", "Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_LOG1", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", type, "MC")
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "IDNo", "ID No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "StartTim", "Starting Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "StartKm", "Starting Km", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "CloseTim", "Closing Time", SAPbobsCOM.BoFieldTypes.db_Date, 0, SAPbobsCOM.BoFldSubTypes.st_Time)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "CloseKm", "Closing Km", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "OTHours", "OT Hrs", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Measurement)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "Descript", "Description", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "VHLNo", "Vehicle No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 4)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "VHLCateg", "Vehicle Category", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "NoTrips", "No Of Trips", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TripLoc", "Trip Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)

            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TType", "TType", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TripCode", "TripCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TripName", "TripName", SAPbobsCOM.BoFieldTypes.db_Alpha, 200)

            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TripCost", "TripCost", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "TRemarks", "TRemarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_LOG1", "PrjCode", "PrjCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)

            oGFun.CreateUserFields("@MIPL_PM_LOG1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to create Log Sheet Details", ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Mobilization / Demobilization ...        "

    Sub MoDemobilization()
        Try
            Me.MoDemobilizationHeader()
            Me.MoDemobilizationDetails()

            If Not oGFun.UDOExists("OMOB") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}, {"U_DocDate", "Document Date"}}
                oGFun.RegisterUDO("OMOB", "PM Mobilization / Demobilization ", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OMOB", "MIPL_PM_MOB1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("TableCreation Daily Form Entry Method Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub MoDemobilizationHeader()
        Try
            oGFun.CreateTable("MIPL_PM_OMOB", "PM Mo-Demobilization Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            Dim Type1 = New String(,) {{"M", "Monthly"}, {"W", "Weekly"}, {"D", "Daily"}, {"H", "Hourly"}}
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_OMOB", "Freq", "Frequency", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Type1, "")

            oGFun.CreateUserFields("@MIPL_PM_OMOB", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "ReceivedBy", "ReceivedBy By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "ReceivedDate", "Received Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "DriverCode", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "DriverName", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "PreByNam", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "AppByCod", "Approved By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "AppByNam", "Approved by", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OMOB", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed To create Log sheet Header", ex.Message)
        End Try
    End Sub

    Sub MoDemobilizationDetails()
        Try
            oGFun.CreateTable("MIPL_PM_MOB1", "PM Mo-Demobilization Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim Type = New String(,) {{"V", "Vehicle"}, {"M", "Machine/Equipment"}}

            oGFun.CreateUserFieldsComboBox("@MIPL_PM_MOB1", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, , , Type, "V")
            oGFun.CreateUserFields("@MIPL_PM_MOB1", "VHLID", "Vehicle ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_MOB1", "VHLName", "Vehicle Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_MOB1", "ToLoc", "To Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_MOB1", "ReleasDt", "Release Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_MOB1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to create Log Sheet Details", ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Direct/Cash Purchase ..."

    Sub DirectCashPurchase()
        Try
            Me.DirectCashPurchaseHead()
            Me.DirectCashPurchaseDetails()

            If Not oGFun.UDOExists("ODCP") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("ODCP", "Direct/Cash Purchase", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_ODCP", "MIPL_PM_DCP1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Table Creation Direct Cash Purchase Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub DirectCashPurchaseHead()
        Try
            oGFun.CreateTable("MIPL_PM_ODCP", "Direct/Cash Purchase Head", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_ODCP", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "MRNo", "MR No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "InvoNo", "Invoice No.", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "PrjCode", "Project Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "PrjName", "Project Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "CardCode", "Vendor Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 15)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "CardName", "Vendor Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "TotAmt", "Total Amount", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "TotDis", "Total Discount", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "NetAmt", "Net Amount", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)


            oGFun.CreateUserFields("@MIPL_PM_ODCP", "PurByCod", "Purchased By", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "PurByNam", "PurchasedBy Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "StoreKpC", "Store Keeper Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_ODCP", "StoreKpN", "Store Keeper Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)

            oGFun.CreateUserFields("@MIPL_PM_ODCP", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Direct Cash Purchase Header failed to create", ex.Message)
        End Try
    End Sub

    Sub DirectCashPurchaseDetails()
        Try
            oGFun.CreateTable("MIPL_PM_DCP1", "Direct/Cash Purchase Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_DCP1", "ItemCode", "Material Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "ItemName", "Material Description", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "Unit", "Unit", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "Rate", "Rate", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Rate)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "DiscPrct", "Disc. Percentage", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Percentage)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "DiscAmt", "Dicount Amount", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Percentage)
            oGFun.CreateUserFields("@MIPL_PM_DCP1", "Amount", "Amount", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Sum)


        Catch ex As Exception
            oApplication.StatusBar.SetText("Direct Cash Purchase Details failed to create", ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Test Drive Result ..."

    Sub TestDriveResult()
        Try
            Me.TestDriveResultHead()
            Me.TestDriveResultDetails()

            If Not oGFun.UDOExists("OTDR") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OTDR", "PMD Test Drive Result ", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OTDR", "MIPL_PM_TDR1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Table Creation TestDriveResult Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub TestDriveResultHead()
        Try
            oGFun.CreateTable("MIPL_PM_OTDR", "Test Drive Result Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OTDR", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OTDR", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_OTDR", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_OTDR", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OTDR", "PreBy", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OTDR", "TestDate", "Test Date", SAPbobsCOM.BoFieldTypes.db_Date)



        Catch ex As Exception
            oApplication.StatusBar.SetText("TestDriveResult Header failed to create", ex.Message)
        End Try
    End Sub

    Sub TestDriveResultDetails()
        Try
            oGFun.CreateTable("MIPL_PM_TDR1", "Test Drive Result Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim FuelType = New String(,) {{"P", "Petrol"}, {"D", "Diesel"}, {"G", "Gasolin"}}

            oGFun.CreateUserFields("@MIPL_PM_TDR1", "IDno", "ID No", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "VehiNo", "Vehicle No", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "VHLName", "Vehicle Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "VHLRegNo", "Vehicle Reg.No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            'oGFun.CreateUserFields("@MIPL_PM_TDR1", "VehiNo", "Vehicle No", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "DriveCod", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "DriveNam", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Trailer", "Trailer", SAPbobsCOM.BoFieldTypes.db_Alpha, 40)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Trip", "Trip", SAPbobsCOM.BoFieldTypes.db_Alpha, 40)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Weight", "Weight", SAPbobsCOM.BoFieldTypes.db_Alpha, 9)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "UpDown", "Up &amp; Down", SAPbobsCOM.BoFieldTypes.db_Alpha, 40)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_TDR1", "FuelType", "Fuel Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", FuelType, "")
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Price", "Price", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Price)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Mileage", "Mileage", SAPbobsCOM.BoFieldTypes.db_Alpha, 40)
            oGFun.CreateUserFields("@MIPL_PM_TDR1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            'oGFun.CreateUserFields("@MIPL_PM_TDR1", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", "O")


        Catch ex As Exception
            oApplication.StatusBar.SetText("TestDriveResult Details failed to create", ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... BusMovementDetails ...      "

    Sub BusMovementDetails()
        Try
            Me.BusMovementDetailsHead()
            Me.BusMovementDetailsDetail1()

            If Not oGFun.UDOExists("OBMD") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OBMD", "BusMovementDetails", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OBMD", "MIPL_PM_BMD1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("BusMovementDetails Register Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub BusMovementDetailsHead()
        Try
            oGFun.CreateTable("MIPL_PM_OBMD", "BusMovementDetails", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OBMD", "FromDate", "From Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBMD", "ToDate", "To Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBMD", "DocDate", "DocDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OBMD", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_OBMD", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OBMD", "PreBy", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

        Catch ex As Exception
            oApplication.StatusBar.SetText("BusMovementDetailsHead  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try

    End Sub

    Sub BusMovementDetailsDetail1()
        Try
            oGFun.CreateTable("MIPL_PM_BMD1", "BusMovementDetailsDetail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_BMD1", "BusID", "Bus ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "Model", "Model", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "TripType", "Trip Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "SeatCap", "Seat Capacity", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "DriveCod", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "DriveNam", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 40)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "WMStren", "Workmen Strength", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "WMHours", "Workmen Hours", SAPbobsCOM.BoFieldTypes.db_Alpha, 150)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_BMD1", "VehiNo", "Vehicle No", SAPbobsCOM.BoFieldTypes.db_Alpha, 15, SAPbobsCOM.BoFldSubTypes.st_None, "")


        Catch ex As Exception
            oApplication.StatusBar.SetText("BusMovementDetailsDetail  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... FuelDistributionEntry ...       "

    Sub FuelDistributionEntry()
        Try
            Me.FuelDistributionEntryHead()
            Me.FuelDistributionEntryDetail1()

            If Not oGFun.UDOExists("OFDE") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OFDE", "Fuel Distribution Entry", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OFDE", "MIPL_PM_FDE1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Fuel Distribution Entry Register Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub FuelDistributionEntryHead()
        Try
            oGFun.CreateTable("MIPL_PM_OFDE", "Fuel Distribution Entry Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OFDE", "DocDate", "DocDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OFDE", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OFDE", "FeulDsDt", "Feul Distribution date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OFDE", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OFDE", "PreByNam", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OFDE", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Fuel Distribution Entry Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub FuelDistributionEntryDetail1()
        Try
            oGFun.CreateTable("MIPL_PM_FDE1", "Fuel Distribution Entry Detail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}
            Dim FuelType = New String(,) {{"P", "Petrol"}, {"D", "Diesel"}, {"G", "Gasolin"}}
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "Locatoin", "Site/Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "PrjCode", "Prj Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "PrjName", "Prj Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_FDE1", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2, SAPbobsCOM.BoFldSubTypes.st_None, "", type, "MC")


            oGFun.CreateUserFields("@MIPL_PM_FDE1", "RegNo", "Registration No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "VHLNo", "Vehicle No", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "VHLName", "Vehicle Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "DvrCode", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "DvrName", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_FDE1", "FuelType", "Fuel type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", FuelType, "")
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "Quantity", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "CurrKmtr", "Kilometers", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_FDE1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)




        Catch ex As Exception
            oApplication.StatusBar.SetText("FuelDistributionEntryDetail  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... DriverTripDetails ...       "

    Sub DriverTripDetails()
        Try
            Me.DriverTripDetailsHead()
            Me.DriverTripDetailsDetail1()

            If Not oGFun.UDOExists("ODTD") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("ODTD", "DriverTripDetails", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_ODTD", "MIPL_PM_DTD1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("DriverTripDetails Register Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub DriverTripDetailsHead()
        Try
            oGFun.CreateTable("MIPL_PM_ODTD", "DriverTripDetails Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_ODTD", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_ODTD", "Month", "Month", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_ODTD", "DocDate", "DocDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_ODTD", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)
            oGFun.CreateUserFields("@MIPL_PM_ODTD", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_ODTD", "PreByNm", "Prepared By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)


        Catch ex As Exception
            oApplication.StatusBar.SetText("DriverTripDetailsHead  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try

    End Sub

    Sub DriverTripDetailsDetail1()
        Try
            oGFun.CreateTable("MIPL_PM_DTD1", "DriverTripDetailsDetail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_DTD1", "DvrCode", "Driver Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_DTD1", "DvrName", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_DTD1", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_DTD1", "NoOfDays", "No Of Days", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_DTD1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 254)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DriverTripDetailsDetail  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

#End Region

#Region "       ... WasteOilDisposal ...        "

    Sub WasteOilDisposal()
        Try
            Me.WasteOilDisposalHead()
            Me.WasteOilDisposalDetails1()

            If Not oGFun.UDOExists("OWOD") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OWOD", "WasteOilDisposal", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OWOD", "MIPL_PM_WOD1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Table Creation Waste Oil Disposal Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub WasteOilDisposalHead()
        Try
            oGFun.CreateTable("MIPL_PM_OWOD", " WasteOilDisposal Header ", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OWOD", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 4)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "DeltoCod", "Delevered To Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "DeltoNm", "Delevered To Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "RefNo", "Reference Number", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "NoofDrum", "No of Drums", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "Gallons", "Gallons", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "DelidBy", "Delivered By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "Contact", "Contact", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "ChklstNo", "ChklstNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "TotQty", "Total Quantity", SAPbobsCOM.BoFieldTypes.db_Float, 0, SAPbobsCOM.BoFldSubTypes.st_Quantity)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "RecvdBy", "Recieved By", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)

            oGFun.CreateUserFields("@MIPL_PM_OWOD", "DelByCd", "Delivered By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OWOD", "RcvByCd", "Received By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Waste Oil Disposal Header failed to create", ex.Message)
        End Try
    End Sub

    Sub WasteOilDisposalDetails1()
        Try
            oGFun.CreateTable("MIPL_PM_WOD1", "WasteOilDisposal Details1", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim opinion = New String(,) {{"S", "Satisfaction"}, {"N", "Not Statisfaction"}}

            oGFun.CreateUserFields("@MIPL_PM_WOD1", "PntsChk", "Points Check", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_WOD1", "Stsfctry", "Satisfactory", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_WOD1", "NStsfry", "Non Satisfactory", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_WOD1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 254)
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_WOD1", "Opinion", "Opinion", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", opinion, "S")

        Catch ex As Exception
            oApplication.StatusBar.SetText("Waste Oil Disposal Details1 failed to create", ex.Message)
        End Try
    End Sub



#End Region

#Region "       ... Vehicle Allocation Request ...      "
    Sub VehicleAllocationReq()
        Try

            Me.VehicleAllocationReqHead()
            Me.VehicleAllocationReqDetail()
            If Not oGFun.UDOExists("OVAR") Then
                Dim findAliasNDescription = New String(,) {{"DocNum", "DocID"}, {"U_DocDate", "Doc Date"}}
                oGFun.RegisterUDO("OVAR", "Vehicle Allocation Request", SAPbobsCOM.BoUDOObjType.boud_Document, findAliasNDescription, "MIPL_PM_OVAR", "MIPL_PM_VAR1")
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Vehicle Allocation Request Table : " & ex.Message)
        Finally
        End Try
    End Sub
    Sub VehicleAllocationReqHead()
        Try
            oGFun.CreateTable("MIPL_PM_OVAR", "PM Check List Head", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OVAR", " Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "DocDate", "Doc. Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "ReqByNam", "Request By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "ReqByCod", "Request By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "AllByNam", "Alloted By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "AllByCod", "Alloted By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "AppByNam", "Approved By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "AppByCod", "Approved By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OVAR", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create Vehicle Allocation Request Header Table : " & ex.Message)
        Finally
        End Try
    End Sub
    Sub VehicleAllocationReqDetail()
        Try
            Dim Status = New String(,) {{"P", "Pending"}, {"A", "Allocated"}}


            oGFun.CreateTable("MIPL_PM_VAR1", "Veh_Alloc_Request Detail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            oGFun.CreateUserFields("@MIPL_PM_VAR1", "EquipCat", "Equipment Category", SAPbobsCOM.BoFieldTypes.db_Alpha, 30, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "MobilDat", "Required Mobilization Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "TillDate", "Required Till Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "Prjcode", "Prjcode", SAPbobsCOM.BoFieldTypes.db_Alpha, 30, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "PrjName", "PrjName", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, 150, SAPbobsCOM.BoFldSubTypes.st_None, "")
            oGFun.CreateUserFieldsComboBox("@MIPL_PM_VAR1", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", Status, "P")
            oGFun.CreateUserFields("@MIPL_PM_VAR1", "SplrName", "Suplier Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)



        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to Create PM Check List Detail Table : " & ex.Message)
        Finally
        End Try
    End Sub
#End Region

#Region "       ... Payment Certificate ...     "
    Sub PaymentCertificate()
        Try
            Me.PaymentCertificateHead()
            If Not oGFun.UDOExists("OPAY") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "Document Number"}, {"U_DocDate", "Document Date"}}
                oGFun.RegisterUDO("OPAY", "PaymentCertificate ", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OPAY")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("PaymentCertificate Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub PaymentCertificateHead()
        Try
            oGFun.CreateTable("MIPL_PM_OPAY", "PaymentCertificate Head", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Type", "Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoFldSubTypes.st_None)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Contract", "Contract", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Project", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, 150)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "SubConNam", "SubConNam", SAPbobsCOM.BoFieldTypes.db_Alpha, 120)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "OrigVal", "OrigVal", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "FinalVal", "FinalVal", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "DocDate", "DocDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "PerdEnd", "PerdEnd", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "PayCert", "PayCert", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "CertType", "CertType", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "WorkDone", "WorkDone", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Retent", "Retent", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Material", "Material", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "RecAdvnc", "RecAdvnc", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "NetAmt", "NetAmt", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "AmtPaid", "AmtPaid", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "DueAgan", "DueAgan", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "PreByNam", "PreByNam", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "OrderRef", "OrderRef", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "PayTerms", "PayTerms", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
            oGFun.CreateUserFields("@MIPL_PM_OPAY", "NatWork", "NatWork", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)


        Catch ex As Exception
            oApplication.StatusBar.SetText("PaymentCertificate Head Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region

#Region "       ... Vehicle Allocation Request ...      "

    Sub VehicleAllocationRequest()

        oGFun.CreateTable("VehicleAllocation", "VehicleAllocationRequest", SAPbobsCOM.BoUTBTableType.bott_NoObject)

        oGFun.CreateUserFields("@VehicleAllocation", "Code", "Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 20)
        oGFun.CreateUserFields("@VehicleAllocation", "RequestDocNo", "RequestDocNo", SAPbobsCOM.BoFieldTypes.db_Numeric)
        oGFun.CreateUserFields("@VehicleAllocation", "VehicleNo", "VehicleNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 120)
        oGFun.CreateUserFields("@VehicleAllocation", "Project", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)
        oGFun.CreateUserFields("@VehicleAllocation", "FromDate", "FromDate", SAPbobsCOM.BoFieldTypes.db_Date)
        oGFun.CreateUserFields("@VehicleAllocation", "ToDate", "ToDate", SAPbobsCOM.BoFieldTypes.db_Date)

    End Sub
#End Region

#Region "       ... VehicleCostAllocation ...        "

    Sub VehicleCostAllocation()
        Try
            Me.VCAllocationHead()
            Me.VCAllocationDetails()

            If Not oGFun.UDOExists("OVCA") Then
                Dim FindField As String(,) = New String(,) {{"DocNum", "DocNum"}}
                oGFun.RegisterUDO("OVCA", "VehicleCostAllocation", SAPbobsCOM.BoUDOObjType.boud_Document, FindField, "MIPL_PM_OVCA", "MIPL_PM_VCA1")
                FindField = Nothing
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("TableCreation Daily Form Entry Method Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub

    Sub VCAllocationHead()
        Try
            oGFun.CreateTable("MIPL_PM_OVCA", "VCAllocation Header", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OVCA", "DocDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date)
            'Dim WorkType = New String(,) {{"I", "Internal"}, {"E", "External"}}
            'oGFun.CreateUserFieldsComboBox("@MIPL_PM_OVCA", "WorkType", "Work Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, SAPbobsCOM.BoFldSubTypes.st_None, "", WorkType, "I")
            oGFun.CreateUserFields("@MIPL_PM_OVCA", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)
            oGFun.CreateUserFields("@MIPL_PM_OVCA", "Month", "Month", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)


        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed To create Log sheet Header", ex.Message)
        End Try
    End Sub

    Sub VCAllocationDetails()
        Try
            oGFun.CreateTable("MIPL_PM_VCA1", "VCAllocation Details", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            Dim type = New String(,) {{"MC", "Machine"}, {"EQ", "Equipment"}, {"IN", "Instrument"}, {"VH", "Vehicle"}}


            'oGFun.CreateUserFields("@MIPL_PM_VCA1", "VID", "VID", SAPbobsCOM.BoFieldTypes.db_Alpha, 30)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "VID", "VehicleID", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "VName", "VehicleName", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "RegNo", "RegNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 8)

            oGFun.CreateUserFields("@MIPL_PM_VCA1", "One", "One", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Two", "Two", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Three", "Three", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Four", "Four", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Five", "Five", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Six", "Six", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Seven", "Seven", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Eight", "Eight", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Nine", "Nine", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Ten", "Ten", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Eleven", "Eleven", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twelve", "Twelve", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Thirteen", "Thirteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Fourteen", "Fourteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Fifteen", "Fifteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Sixteen", "Sixteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Seventen", "Seventen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Eighteen", "Eighteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Nineteen", "Nineteen", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty", "Twenty", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty1", "Twenty1", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty2", "Twenty2", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty3", "Twenty3", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty4", "Twenty4", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty5", "Twenty5", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty6", "Twenty6", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty7", "Twenty7", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty8", "Twenty8", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Twenty9", "Twenty9", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Thirty", "Thirty", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_VCA1", "Thirty1", "Thirty1", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)


        Catch ex As Exception
            oApplication.StatusBar.SetText("Failed to create VehicleCostAllocation Details", ex.Message)
        End Try
    End Sub

#End Region

#Region "       ... Card Renewal Entry ...       "

    Sub CardRenewalEntry()
        Try
            Me.CardRenewalEntryHeader()
            Me.CardRenewalEntryDetail()

            If Not oGFun.UDOExists("OCRD") Then
                Dim findAliasNDescription = New String(,) {{"DocNum", "DocNum"}, {"U_DocDate", "Document Date"}}
                oGFun.RegisterUDO("OCRD", "Card Renewal Entry PMD", SAPbobsCOM.BoUDOObjType.boud_Document, findAliasNDescription, "MIPL_PM_OCRD", "MIPL_PM_CRD1")
                findAliasNDescription = Nothing
            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub CardRenewalEntryHeader()
        Try
            oGFun.CreateTable("MIPL_PM_OCRD", "Card Renewal Entry Header PMD", SAPbobsCOM.BoUTBTableType.bott_Document)

            oGFun.CreateUserFields("@MIPL_PM_OCRD", "DocDate", "DocDate", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_OCRD", "NoExpDt", "NoExpDt", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_OCRD", "PreByCod", "Prepared By Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_OCRD", "PreByNam", "Prepared By Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 101)
            oGFun.CreateUserFields("@MIPL_PM_OCRD", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Memo)

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub

    Sub CardRenewalEntryDetail()
        Try
            oGFun.CreateTable("MIPL_PM_CRD1", "Card Renewal Entry Detail PMD", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)


            oGFun.CreateUserFields("@MIPL_PM_CRD1", "empID", "Employee Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 11)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "empName", "Employee Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 120)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "LineNum", "Line Number", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "Code", "Code", SAPbobsCOM.BoFieldTypes.db_Numeric)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "CardType", "Card Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 2)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "CardNo", "Card Number", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "NCardNo", "New Card NO", SAPbobsCOM.BoFieldTypes.db_Alpha, 100)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "IssueDt", "Issue Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "NIsseDt", "New Issue Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "IsuePlce", "Issue Place", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "NIsuePlc", "New Issue Place", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "ExpirDt", "Expirty Date", SAPbobsCOM.BoFieldTypes.db_Date)
            oGFun.CreateUserFields("@MIPL_PM_CRD1", "NExpDt", "New Expirty Date", SAPbobsCOM.BoFieldTypes.db_Date)

            oGFun.CreateUserFields("@MIPL_PM_CRD1", "Remarks", "Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 254)

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        End Try
    End Sub
#End Region

End Class
