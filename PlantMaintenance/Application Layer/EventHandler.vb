'' <summary>
'' SAP has set of different events For access the controls.
'' In this module particularly using to control events.
'' 1) Menu Event using for while the User choose the menus to select the patricular form 
'' 2) Item Event using for to pass the event Function while user doing process
'' 3) Form Data Event Using to Insert,Update,Delete data on Date Base 
'' 4) Status Bar Event will be call when display message to user, message may be will come 
''    Warring or Error
'' </summary>
'' <remarks></remarks>
Module EventHandler
    Public oForm As SAPbouiCOM.Form
#Region " ... Common Variables For SAP ..."
    Public WithEvents oApplication As SAPbouiCOM.Application
#End Region

#Region "... 1) Menu Event ..."

    Private Sub oApplication_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles oApplication.MenuEvent
        Try

            If pVal.BeforeAction = False Then '1 
                Select Case pVal.MenuUID '2
                    Case SpecficationMasterFormID                                'Specification Master
                        If oGFun.FormExist(SpecficationMasterFormID) Then
                            oApplication.Forms.Item(SpecficationMasterFormID).Visible = True
                            oApplication.Forms.Item(SpecficationMasterFormID).Select()
                        Else
                            oSpecficationMaster.LoadSpecificationMaster()
                        End If
                    Case MachineMasterFormID                                     'MachineMaster
                        If oGFun.FormExist(MachineMasterFormID) Then
                            oApplication.Forms.Item(MachineMasterFormID).Visible = True
                            oApplication.Forms.Item(MachineMasterFormID).Select()
                        Else
                            oMachineMaster.LoadMachineMaster()
                        End If

                    Case CategoryMasterFormID                                     'CategoryMaster
                        If oGFun.FormExist(CategoryMasterFormID) Then
                            oApplication.Forms.Item(CategoryMasterFormID).Visible = True
                            oApplication.Forms.Item(CategoryMasterFormID).Select()
                        Else
                            oCategoryMaster.LoadCategoryMaster()
                        End If

                    Case ActCarriedOutFormID                                     'ActCarriedOut
                        If oGFun.FormExist(ActCarriedOutFormID) Then
                            oApplication.Forms.Item(ActCarriedOutFormID).Visible = True
                            oApplication.Forms.Item(ActCarriedOutFormID).Select()
                        Else
                            oActCarriedOut.LoadActCarriedOut()
                        End If
                    Case ActivityMasterFormID                                     'ActivityMaster
                        If oGFun.FormExist(ActivityMasterFormID) Then
                            oApplication.Forms.Item(ActivityMasterFormID).Visible = True
                            oApplication.Forms.Item(ActivityMasterFormID).Select()
                        Else
                            oActivityMaster.LoadActivityMaster()
                        End If
                        'Case TranDataFormID                                     'View Tran Data
                        '    If oGFun.FormExist(TranDataFormID) Then
                        '        oApplication.Forms.Item(TranDataFormID).Visible = True
                        '        oApplication.Forms.Item(TranDataFormID).Select()
                        '    Else
                        '        oTranDataFormID.LoadViewTranData()
                        '    End If
                    Case VehicleMasterFormID                                     'VehicleMaster
                        If oGFun.FormExist(VehicleMasterFormID) Then
                            oApplication.Forms.Item(VehicleMasterFormID).Visible = True
                            oApplication.Forms.Item(VehicleMasterFormID).Select()
                        Else
                            oVehicleMaster.LoadVehicleMaster()
                        End If
                    Case JobCardFormID                                     'JobCard
                        If oGFun.FormExist(JobCardFormID) Then
                            oApplication.Forms.Item(JobCardFormID).Visible = True
                            oApplication.Forms.Item(JobCardFormID).Select()
                        Else
                            oJobCard.LoadJobCard()
                        End If

                    Case LogSheetFormID                                     'LogSheet
                        If oGFun.FormExist(LogSheetFormID) Then
                            oApplication.Forms.Item(LogSheetFormID).Visible = True
                            oApplication.Forms.Item(LogSheetFormID).Select()
                        Else
                            OLogSheet.LoadfrmLogSheet()
                        End If
                    Case MoDemobilizationFormID                                     'LogSheet
                        If oGFun.FormExist(MoDemobilizationFormID) Then
                            oApplication.Forms.Item(MoDemobilizationFormID).Visible = True
                            oApplication.Forms.Item(MoDemobilizationFormID).Select()
                        Else
                            oMoDemobilization.LoadMoDemobilization()
                        End If

                    Case TestDriveResultFormID                                     'TestDriveResult
                        If oGFun.FormExist(TestDriveResultFormID) Then
                            oApplication.Forms.Item(TestDriveResultFormID).Visible = True
                            oApplication.Forms.Item(TestDriveResultFormID).Select()
                        Else
                            OTestDriveResult.LoadfrmTestDriveResult()
                        End If

                    Case VehicleCostAllocationFormID                                     'TestDriveResult
                        If oGFun.FormExist(VehicleCostAllocationFormID) Then
                            oApplication.Forms.Item(VehicleCostAllocationFormID).Visible = True
                            oApplication.Forms.Item(VehicleCostAllocationFormID).Select()
                        Else
                            oVehicleCostAllocation.LoadfrmVehicleCostAllocation()
                        End If

                    Case DirectCashPurchaseFormID                                     'DirectCashPurchase
                        If oGFun.FormExist(DirectCashPurchaseFormID) Then
                            oApplication.Forms.Item(DirectCashPurchaseFormID).Visible = True
                            oApplication.Forms.Item(DirectCashPurchaseFormID).Select()
                        Else
                            ODirectCashPurchase.LoadfrmDirectCashPurchase()
                        End If

                    Case FuelDistributionEntryFormID                                     'FuelDistributionEntry
                        If oGFun.FormExist(FuelDistributionEntryFormID) Then
                            oApplication.Forms.Item(FuelDistributionEntryFormID).Visible = True
                            oApplication.Forms.Item(FuelDistributionEntryFormID).Select()
                        Else
                            oFuelDistributionEntry.LoadfrmFuelDistributionEntry()
                        End If
                    Case PMCheckListFormID                                     'PMCheckList
                        If oGFun.FormExist(PMCheckListFormID) Then
                            oApplication.Forms.Item(PMCheckListFormID).Visible = True
                            oApplication.Forms.Item(PMCheckListFormID).Select()
                        Else
                            oPMCheckList.LoadPMCheckList()
                        End If
                    Case ActivityPlanFormID                                     'Activityplan
                        If oGFun.FormExist(ActivityPlanFormID) Then
                            oApplication.Forms.Item(ActivityPlanFormID).Visible = True
                            oApplication.Forms.Item(ActivityPlanFormID).Select()
                        Else
                            oActivityPlan.LoadActivityPlan()
                        End If

                    Case BreakDownSlipFormID                                     'BreakDownSlip
                        If oGFun.FormExist(BreakDownSlipFormID) Then
                            oApplication.Forms.Item(BreakDownSlipFormID).Visible = True
                            oApplication.Forms.Item(BreakDownSlipFormID).Select()
                        Else
                            oBreakDownSlip.LoadBreakDownSlip()
                        End If

                    Case WasteOilDisposalFormID                                     'WasteOilDisposal
                        If oGFun.FormExist(WasteOilDisposalFormID) Then
                            oApplication.Forms.Item(WasteOilDisposalFormID).Visible = True
                            oApplication.Forms.Item(WasteOilDisposalFormID).Select()
                        Else
                            oWasteOilDisposal.LoadfrmWasteOilDisposal()
                        End If

                    Case PaymentCertificateFormID
                        If oGFun.FormExist(PaymentCertificateFormID) Then
                            oApplication.Forms.Item(PaymentCertificateFormID).Visible = True
                            oApplication.Forms.Item(PaymentCertificateFormID).Select()
                        Else
                            oPaymentCertificate.LoadPaymentCertificate()
                        End If
                    Case TripMasterFormID                                     'ActivityMaster
                        If oGFun.FormExist(TripMasterFormID) Then
                            oApplication.Forms.Item(TripMasterFormID).Visible = True
                            oApplication.Forms.Item(TripMasterFormID).Select()
                        Else
                            oTripMaster.LoadTripMaster()
                        End If

                    Case LicenseDetailsFormID                                     'LicenseDetails
                        If oGFun.FormExist(LicenseDetailsFormID) Then
                            oApplication.Forms.Item(LicenseDetailsFormID).Visible = True
                            oApplication.Forms.Item(LicenseDetailsFormID).Select()
                        Else
                            oLicenseDetails.LoadLicenseDetails()
                        End If
                    Case CardRenewalEntryFormID
                        If oGFun.FormExist(CardRenewalEntryFormID) Then
                            oApplication.Forms.Item(CardRenewalEntryFormID).Visible = True
                            oApplication.Forms.Item(CardRenewalEntryFormID).Select()
                        Else
                            oCardRenewalEntry.LoadCardRenewalEntry()
                        End If
                    Case "3079"                'GoodsIssue
                        oGoodsIssue.LoadGoodsIssue()
                End Select '2

                oForm = oApplication.Forms.ActiveForm
                Dim oUDFForm As SAPbouiCOM.Form
                Select Case pVal.MenuUID '3
                    Case "1282", "1281", "1292", "1293", "1287", "519"
                        Select Case oForm.UniqueID
                            Case SpecficationMasterFormID
                                oSpecficationMaster.MenuEvent(pVal, BubbleEvent)
                            Case MachineMasterFormID                   'MachineMaster
                                oMachineMaster.MenuEvent(pVal, BubbleEvent)
                            Case VehicleMasterFormID                   'VehicleMaster
                                oVehicleMaster.MenuEvent(pVal, BubbleEvent)
                            Case ActivityMasterFormID                   'ActivityMaster
                                oActivityMaster.MenuEvent(pVal, BubbleEvent)
                            Case CategoryMasterFormID                   'CategoryMaster
                                oCategoryMaster.MenuEvent(pVal, BubbleEvent)

                            Case ActCarriedOutFormID                   'ActCarriedOut
                                oActCarriedOut.MenuEvent(pVal, BubbleEvent)
                            Case JobCardFormID                   'JobCard
                                oJobCard.MenuEvent(pVal, BubbleEvent)
                            Case LogSheetFormID                   'LogSheet
                                OLogSheet.MenuEvent(pVal, BubbleEvent)
                            Case MoDemobilizationFormID                   'LogSheet
                                oMoDemobilization.MenuEvent(pVal, BubbleEvent)
                            Case TestDriveResultFormID                   'TestDriveResult
                                OTestDriveResult.MenuEvent(pVal, BubbleEvent)
                            Case DirectCashPurchaseFormID                   'DirectCashPurchase
                                ODirectCashPurchase.MenuEvent(pVal, BubbleEvent)
                            Case FuelDistributionEntryFormID                   'FuelDistributionEntry
                                oFuelDistributionEntry.MenuEvent(pVal, BubbleEvent)
                            Case PMCheckListFormID                   'PMCheckList
                                oPMCheckList.MenuEvent(pVal, BubbleEvent)
                            Case ActivityPlanFormID                   'Activityplan
                                oActivityPlan.MenuEvent(pVal, BubbleEvent)
                            Case BreakDownSlipFormID                   'BreakDownSlip
                                oBreakDownSlip.MenuEvent(pVal, BubbleEvent)
                            Case WasteOilDisposalFormID                   'WasteOilDisposal
                                oWasteOilDisposal.MenuEvent(pVal, BubbleEvent)
                            Case PaymentCertificateFormID                   'PaymentCertificate
                                oPaymentCertificate.MenuEvent(pVal, BubbleEvent)
                            Case TripMasterFormID                   'TripMaster
                                oTripMaster.MenuEvent(pVal, BubbleEvent)
                            Case LicenseDetailsFormID                   'LicenseDetails
                                oLicenseDetails.MenuEvent(pVal, BubbleEvent)

                            Case CardRenewalEntryFormID
                                oCardRenewalEntry.MenuEvent(pVal, BubbleEvent)
                            Case Else
                                If oApplication.Forms.ActiveForm.TypeEx.Contains("720") Then
                                    oUDFForm = oApplication.Forms.Item(oApplication.Forms.ActiveForm.UDFFormUID)
                                    oUDFForm.Items.Item("U_MCOutNo").Enabled = False
                                    oUDFForm.Items.Item("U_JobNo").Enabled = False
                                End If
                        End Select '3
                    Case "1288", "1289", "1290", "1291"
                        If oApplication.Forms.ActiveForm.TypeEx.Contains("720") Then
                            oUDFForm = oApplication.Forms.Item(oApplication.Forms.ActiveForm.UDFFormUID)
                            If oUDFForm.Items.Item("U_MCOutNo").Specific.String <> "" Then
                                oUDFForm.Items.Item("U_MCOutNo").Enabled = False
                            ElseIf oUDFForm.Items.Item("U_JobNo").Specific.String <> "" Then
                                oUDFForm.Items.Item("U_JobNo").Enabled = False
                            End If
                        End If
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1283", "1284", "1286"
                        Select Case oForm.UniqueID
                            Case ActCarriedOutFormID                   'ActCarriedOut
                                oActCarriedOut.MenuEvent(pVal, BubbleEvent)
                            Case JobCardFormID                   'JobCard
                                oJobCard.MenuEvent(pVal, BubbleEvent)
                            Case SpecficationMasterFormID
                                oSpecficationMaster.MenuEvent(pVal, BubbleEvent)
                            Case MachineMasterFormID                   'MachineMaster
                                oMachineMaster.MenuEvent(pVal, BubbleEvent)
                            Case VehicleMasterFormID                   'VehicleMaster
                                oVehicleMaster.MenuEvent(pVal, BubbleEvent)
                            Case ActivityMasterFormID                   'ActivityMaster
                                oActivityMaster.MenuEvent(pVal, BubbleEvent)
                            Case CategoryMasterFormID                   'CategoryMaster
                                oCategoryMaster.MenuEvent(pVal, BubbleEvent)
                            Case PMCheckListFormID                   'PMCheckList
                                oPMCheckList.MenuEvent(pVal, BubbleEvent)
                            Case ActivityPlanFormID                   'Activityplan
                                oActivityPlan.MenuEvent(pVal, BubbleEvent)
                            Case BreakDownSlipFormID                   'BreakDownSlip
                                oBreakDownSlip.MenuEvent(pVal, BubbleEvent)
                        End Select
                End Select
                'If pVal.MenuUID = "526" Then '4
                '    oCompany.Disconnect()
                '    oApplication.StatusBar.SetText("Plant Maintenance AddOn is DisConnected . . .", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                '    End
                'End If '4
            End If '1

        Catch ex As Exception
            'oApplication.StatusBar.SetText("Purchase Menu Event Failed : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Private Sub oApplication_AppEvent(ByVal EventType As SAPbouiCOM.BoAppEventTypes) Handles oApplication.AppEvent
        Try
            Select Case EventType
                Case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged, SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged, SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition, SAPbouiCOM.BoAppEventTypes.aet_ShutDown
                    System.Windows.Forms.Application.Exit()
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Application Event Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region

#Region " ... 2) Item Event ..."
    Private Sub oApplication_ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
        Try
            ' oApplication.Forms.Item("0").Items.Item("2").Width = oApplication.Forms.Item("0").Items.Item("2").Width + 20
            Select Case pVal.FormUID

                Case SpecficationMasterFormID
                    oSpecficationMaster.ItemEvent(SpecficationMasterFormID, pVal, BubbleEvent)
                Case MachineMasterFormID                                                          'MachineMaster
                    oMachineMaster.ItemEvent(MachineMasterFormID, pVal, BubbleEvent)
                Case VehicleMasterFormID                                                          'VehicleMaster
                    oVehicleMaster.ItemEvent(VehicleMasterFormID, pVal, BubbleEvent)

                Case CategoryMasterFormID                                                          'CategoryMaster
                    oCategoryMaster.ItemEvent(CategoryMasterFormID, pVal, BubbleEvent)

                Case ActCarriedOutFormID                                                          'ActCarriedOut
                    oActCarriedOut.ItemEvent(ActCarriedOutFormID, pVal, BubbleEvent)

                Case ActivityMasterFormID                                                          'ActivityMaster
                    oActivityMaster.ItemEvent(ActivityMasterFormID, pVal, BubbleEvent)

                Case JobCardFormID                                                          'JobCard
                    oJobCard.ItemEvent(JobCardFormID, pVal, BubbleEvent)
                Case VehicleCostAllocationFormID                                                          'JobCard
                    oVehicleCostAllocation.ItemEvent(VehicleCostAllocationFormID, pVal, BubbleEvent)

                Case TranDataFormID
                    oTranDataFormID.ItemEvent(TranDataFormID, pVal, BubbleEvent)

                Case LogSheetFormID                                                          'LogSheet
                    OLogSheet.ItemEvent(LogSheetFormID, pVal, BubbleEvent)
                Case MoDemobilizationFormID                                                           'LogSheet
                    oMoDemobilization.ItemEvent(MoDemobilizationFormID, pVal, BubbleEvent)
                Case TestDriveResultFormID                                                          'TestDriveResult
                    OTestDriveResult.ItemEvent(TestDriveResultFormID, pVal, BubbleEvent)
                Case DirectCashPurchaseFormID                                                          'DirectCashPurchase
                    ODirectCashPurchase.ItemEvent(DirectCashPurchaseFormID, pVal, BubbleEvent)
                Case FuelDistributionEntryFormID                                                          'FuelDistributionEntry
                    oFuelDistributionEntry.ItemEvent(FuelDistributionEntryFormID, pVal, BubbleEvent)
                Case PMCheckListFormID                                                          'PMCheckList
                    oPMCheckList.ItemEvent(PMCheckListFormID, pVal, BubbleEvent)
                Case ActivityPlanFormID                                                          'Activityplan
                    oActivityPlan.ItemEvent(ActivityPlanFormID, pVal, BubbleEvent)
                Case BreakDownSlipFormID                                                          'BreakDownSlip
                    oBreakDownSlip.ItemEvent(BreakDownSlipFormID, pVal, BubbleEvent)
                Case WasteOilDisposalFormID                                                          'WasteOilDisposal
                    oWasteOilDisposal.ItemEvent(WasteOilDisposalFormID, pVal, BubbleEvent)
                Case PaymentCertificateFormID               'PaymentCertificate
                    oPaymentCertificate.ItemEvent(PaymentCertificateFormID, pVal, BubbleEvent)
                Case TripMasterFormID               'PaymentCertificate
                    oTripMaster.ItemEvent(TripMasterFormID, pVal, BubbleEvent)

                Case LicenseDetailsFormID               'LicenseDetails
                    oLicenseDetails.ItemEvent(LicenseDetailsFormID, pVal, BubbleEvent)

                Case CardRenewalEntryFormID
                    oCardRenewalEntry.ItemEvent(CardRenewalEntryFormID, pVal, BubbleEvent)
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Production ItemEvent Failed : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region


#Region " ... 3) FormDataEvent ..."
    Private Sub oApplication_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles oApplication.FormDataEvent
        Try
            Select Case BusinessObjectInfo.FormUID
                Case SpecficationMasterFormID
                    oSpecficationMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case MachineMasterFormID                                              'MachineMaster
                    oMachineMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case VehicleMasterFormID                                              'VehicleMaster
                    oVehicleMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case CategoryMasterFormID                                              'CategoryMaster
                    oCategoryMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case ActivityMasterFormID                                              'ActivityMaster
                    oActivityMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case ActCarriedOutFormID                                              'ActCarriedOut
                    oActCarriedOut.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case JobCardFormID                                              'JobCard
                    oJobCard.FormDataEvent(BusinessObjectInfo, BubbleEvent)

                Case VehicleCostAllocationFormID                                              'VehicleCostAllocation
                    oVehicleCostAllocation.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case LogSheetFormID                                              'LogSheet
                    OLogSheet.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case MoDemobilizationFormID                                               'LogSheet
                    oMoDemobilization.FormDataEvent(BusinessObjectInfo, BubbleEvent)

                Case TestDriveResultFormID                                              'TestDriveResult
                    OTestDriveResult.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case DirectCashPurchaseFormID                                              'DirectCashPurchase
                    ODirectCashPurchase.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case FuelDistributionEntryFormID                                              'FuelDistributionEntry
                    oFuelDistributionEntry.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case PMCheckListFormID                                              'PMCheckList
                    oPMCheckList.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case ActivityPlanFormID                                              'Activityplan
                    oActivityPlan.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case BreakDownSlipFormID                                              'BreakDownSlip
                    oBreakDownSlip.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case WasteOilDisposalFormID                                              'WasteOilDisposal
                    oWasteOilDisposal.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case PaymentCertificateFormID                       'PaymentCertificate
                    oPaymentCertificate.FormDataEvent(BusinessObjectInfo, BubbleEvent)

                Case TripMasterFormID                       'TripMaster
                    oTripMaster.FormDataEvent(BusinessObjectInfo, BubbleEvent)
                Case LicenseDetailsFormID                       'LicenseDetails
                    oLicenseDetails.FormDataEvent(BusinessObjectInfo, BubbleEvent)

                Case CardRenewalEntryFormID
                    oCardRenewalEntry.FormDataEvent(BusinessObjectInfo, BubbleEvent)
            End Select


            Try
                Dim objform As SAPbouiCOM.Form
                Dim objMatrix As SAPbouiCOM.Matrix
                Dim Query As String = ""
                objform = oApplication.Forms.Item(BusinessObjectInfo.FormUID)
                If BusinessObjectInfo.BeforeAction = False Then
                    If objform.Title = "PM Measurement Master" Then
                        'MsgBox("1")
                        objMatrix = objform.Items.Item("3").Specific
                        Dim ItemCode As String = objMatrix.Columns.Item("U_MCode").Cells.Item(objMatrix.VisualRowCount).Specific.String
                        If ItemCode <> "" Then
                            Dim RSet As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            If HANA Then
                                Query = "UPDATE T0 SET ""U_Vibrate"" = T1.""U_Vibrate"", ""U_Temp"" = T1.""U_Temp"", ""U_Pressure"" = T1.""U_Pressure"", ""U_RunKM"" = T1.""U_RunKM"", ""U_RunHours"" = T1.""U_RunHrs"" "
                                Query += vbCrLf + "FROM ""@MIPL_PM_OMAC"" T0 INNER JOIN (SELECT TOP 1 ""U_MCode"", ""U_Vibrate"", ""U_Temp"", ""U_Pressure"", ""U_RunKM"", ""U_RunHrs"" FROM ""@MIPL_PM_MM"" WHERE ""U_DocDate"" = (SELECT MAX(""U_DocDate"") FROM ""@MIPL_PM_MM"" where ""U_MCode""='" & ItemCode & "') "
                                Query += vbCrLf + "AND ""U_MCode"" = '" & ItemCode & "') AS T1 ON T0.""U_ItemCode"" = T1.""U_MCode"" WHERE T0.""U_ItemCode"" = '" & ItemCode & "'"
                            Else
                                Query = "update T0 set U_Vibrate=T1.U_Vibrate,U_Temp=T1.U_Temp,U_Pressure=T1.U_Pressure,U_RunKM=T1.U_RunKM,U_RunHours =T1.U_RunHrs"
                                Query += vbCrLf + "from [@MIPL_PM_OMAC] T0 inner join (select Top 1 U_MCode,U_Vibrate,U_Temp,U_Pressure,U_RunKM,U_RunHrs from [@MIPL_PM_MM] where U_DocDate=(Select Max(U_DocDate) from [@MIPL_PM_MM] where U_MCode='" & ItemCode & "')"
                                Query += vbCrLf + "and U_MCode='" & ItemCode & "') T1 on T0.U_ItemCode=T1.U_MCode where T0.U_ItemCode='" & ItemCode & "'"
                            End If
                            RSet.DoQuery(Query)
                            RSet = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            If HANA Then
                                Query = "UPDATE T0 SET ""U_Vibrate"" = T1.""U_Vibrate"", ""U_Temp"" = T1.""U_Temp"", ""U_Pressure"" = T1.""U_Pressure"", ""U_RunKM"" = T1.""U_RunKM"", ""U_RunHours"" = T1.""U_RunHrs"" "
                                Query += vbCrLf + "FROM ""@MIPL_PM_OVFL"" T0 INNER JOIN (SELECT TOP 1 ""U_MCode"", ""U_Vibrate"", ""U_Temp"", ""U_Pressure"", ""U_RunKM"", ""U_RunHrs"" FROM ""@MIPL_PM_MM"" WHERE ""U_DocDate"" = (SELECT MAX(""U_DocDate"") FROM ""@MIPL_PM_MM"" where ""U_MCode""='" & ItemCode & "') "
                                Query += vbCrLf + "AND ""U_MCode"" = '" & ItemCode & "') AS T1 ON T0.""U_ItemCode"" = T1.""U_MCode"" WHERE T0.""U_ItemCode"" = '" & ItemCode & "'"
                            Else
                                Query = "update T0 set U_Vibrate=T1.U_Vibrate,U_Temp=T1.U_Temp,U_Pressure=T1.U_Pressure,U_RunKM=T1.U_RunKM,U_RunHours =T1.U_RunHrs"
                                Query += vbCrLf + "from [@MIPL_PM_OVHL] T0 inner join (select Top 1 U_MCode,U_Vibrate,U_Temp,U_Pressure,U_RunKM,U_RunHrs from [@MIPL_PM_MM] where U_DocDate=(Select Max(U_DocDate) from [@MIPL_PM_MM] where U_MCode='" & ItemCode & "')"
                                Query += vbCrLf + "and U_MCode='" & ItemCode & "') T1 on T0.U_ItemCode=T1.U_MCode where T0.U_ItemCode='" & ItemCode & "'"
                            End If
                            RSet.DoQuery(Query)
                            RSet = Nothing
                        End If
                       
                    End If
                End If
                
            Catch ex As Exception

            End Try
        Catch ex As Exception
            oApplication.StatusBar.SetText("FormDataEvent Failed : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region

#Region " ... 4) Status Bar Event ..."
    Public Sub oApplication_StatusBarEvent(ByVal Text As String, ByVal MessageType As SAPbouiCOM.BoStatusBarMessageType) Handles oApplication.StatusBarEvent
        Try
            If MessageType = SAPbouiCOM.BoStatusBarMessageType.smt_Warning Or MessageType = SAPbouiCOM.BoStatusBarMessageType.smt_Error Then
                System.Media.SystemSounds.Asterisk.Play()
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Production StatusBarEvent Event Failed : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region

#Region " ... 5) Set Event Filter ..."
    Public Sub SetEventFilter()
        Try
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message)
        Finally
        End Try
    End Sub
#End Region

#Region " ... 6) Right Click Event ..."
    Private Sub oApplication_RightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean) Handles oApplication.RightClickEvent
        Try
            'Delete the User Creation Menus from Main Menu ..
            'If oApplication.Menus.Item("1280").SubMenus.Exists("SizeBreakUp") = True Then oApplication.Menus.Item("1280").SubMenus.RemoveEx("SizeBreakUp")           
            Select Case eventInfo.FormUID
                Case SpecficationMasterFormID
                    oSpecficationMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case MachineMasterFormID                                                          'MachineMaster
                    oMachineMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case ActivityMasterFormID                                                          'ActivityMaster
                    oActivityMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case CategoryMasterFormID
                    oCategoryMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case ActCarriedOutFormID                                                          'ActCarriedOut
                    oActCarriedOut.RightClickEvent(eventInfo, BubbleEvent)
                Case VehicleMasterFormID                                                          'VehicleMaster
                    oVehicleMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case JobCardFormID                                                          'JobCard
                    oJobCard.RightClickEvent(eventInfo, BubbleEvent)
                Case VehicleCostAllocationFormID                                                          'JobCard
                    oVehicleCostAllocation.RightClickEvent(eventInfo, BubbleEvent)
                Case LogSheetFormID                                                          'LogSheet
                    OLogSheet.RightClickEvent(eventInfo, BubbleEvent)
                Case TestDriveResultFormID                                                          'TestDriveResult
                    OTestDriveResult.RightClickEvent(eventInfo, BubbleEvent)
                Case DirectCashPurchaseFormID                                                          'DirectCashPurchase
                    ODirectCashPurchase.RightClickEvent(eventInfo, BubbleEvent)
                Case FuelDistributionEntryFormID                                                          'FuelDistributionEntry
                    oFuelDistributionEntry.RightClickEvent(eventInfo, BubbleEvent)
                Case WasteOilDisposalFormID                                                          'WasteOilDisposal
                    oWasteOilDisposal.RightClickEvent(eventInfo, BubbleEvent)
                Case PaymentCertificateFormID               'PaymentCertificate
                    oPaymentCertificate.RightClickEvent(eventInfo, BubbleEvent)
                Case PMCheckListFormID
                    oPMCheckList.RightClickEvent(eventInfo, BubbleEvent)
                Case ActivityPlanFormID
                    oActivityPlan.RightClickEvent(eventInfo, BubbleEvent)
                Case BreakDownSlipFormID
                    oBreakDownSlip.RightClickEvent(eventInfo, BubbleEvent)
                Case TripMasterFormID
                    oTripMaster.RightClickEvent(eventInfo, BubbleEvent)
                Case LicenseDetailsFormID
                    oLicenseDetails.RightClickEvent(eventInfo, BubbleEvent)
                Case CardRenewalEntryFormID
                    oCardRenewalEntry.RightClickEvent(eventInfo, BubbleEvent)

            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText(addonName & " : Right Click Event Failed : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
#End Region

#Region "...8) Layout Event ..."

    Private Sub objApplication_LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean) Handles oApplication.LayoutKeyEvent

        'BubbleEvent = True
        If eventInfo.BeforeAction = True Then
            If eventInfo.FormUID.Contains(ActCarriedOutFormID) Then
                oActCarriedOut.LayoutKeyEvent(eventInfo, BubbleEvent)
            ElseIf eventInfo.FormUID.Contains(ActivityPlanFormID) Then
                oActivityPlan.LayoutKeyEvent(eventInfo, BubbleEvent)
            ElseIf eventInfo.FormUID.Contains(BreakDownSlipFormID) Then
                oBreakDownSlip.LayoutKeyEvent(eventInfo, BubbleEvent)
            ElseIf eventInfo.FormUID.Contains(JobCardFormID) Then
                oJobCard.LayoutKeyEvent(eventInfo, BubbleEvent)
            ElseIf eventInfo.FormUID.Contains(PMCheckListFormID) Then
                oPMCheckList.LayoutKeyEvent(eventInfo, BubbleEvent)
            End If
        End If
    End Sub
#End Region

    
End Module
