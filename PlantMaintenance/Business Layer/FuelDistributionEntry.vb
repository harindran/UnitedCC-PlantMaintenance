Public Class FuelDistributionEntry
    Dim frmFuelDistributionEntry As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OFDE"
    Dim StrQuery As String = ""
    Sub LoadfrmFuelDistributionEntry()
        Try
            oGFun.LoadXML(frmFuelDistributionEntry, FuelDistributionEntryFormID, FuelDistributionEntryXML)
            frmFuelDistributionEntry = oApplication.Forms.Item(FuelDistributionEntryFormID)
            oDBDSHeader = frmFuelDistributionEntry.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmFuelDistributionEntry.DataSources.DBDataSources.Item(1)
            oMatrix = frmFuelDistributionEntry.Items.Item("Matrix").Specific
            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmFuelDistributionEntry.Freeze(True)
            oGFun.LoadComboBoxSeries(frmFuelDistributionEntry.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmFuelDistributionEntry.Items.Item("t_docdate").Specific)
            'oGFun.LoadLocationComboBox(frmFuelDistributionEntry.Items.Item("c_location").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            oGFun.LoadLocationComboBox(oMatrix.Columns.Item("site").Cells.Item(1).Specific)
            'frmFuelDistributionEntry.ActiveItem = "c_location"
            frmFuelDistributionEntry.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmFuelDistributionEntry.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            'frmFuelDistributionEntry.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmFuelDistributionEntry.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmFuelDistributionEntry.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmFuelDistributionEntry.Items.Item("t_Todate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)


        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If oMatrix.Columns.Item("site").Cells.Item(1).Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Site Detail should not be left Empty...")
                Return False
            End If
            If frmFuelDistributionEntry.Items.Item("t_prebynm").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Prepared By should not be left Empty..")
                Return False
            End If
            'If frmFuelDistributionEntry.Items.Item("c_location").Specific.value = "" Then
            '    oGFun.StatusBarErrorMsg("Location Should not be left Empty..")
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Leave Date Validation Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            Return False
        Finally
        End Try
    End Function
    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.EventType
                Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                    Try
                        Dim oDataTable As SAPbouiCOM.DataTable
                        Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                        oDataTable = oCFLE.SelectedObjects
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            Select Case pVal.ItemUID
                                Case "t_prebynm"
                                    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "contact"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_Contact", pVal.Row - 1, Trim(oDataTable.GetValue("PrjCode", 0)))
                                            oMatrix.LoadFromDataSource()
                                        Case "vehicno"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_VHLNo", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                            If Trim(oDBDSDetail.GetValue("U_Type", pVal.Row - 1)).Equals("VH") Then
                                                oDBDSDetail.SetValue("U_VHLName", pVal.Row - 1, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                                oDBDSDetail.SetValue("U_RegNo", pVal.Row - 1, Trim(oDataTable.GetValue("U_RegNo", 0)))
                                                oDBDSDetail.SetValue("U_FuelType", pVal.Row - 1, Trim(oDataTable.GetValue("U_FuelType", 0)))
                                            End If
                                            oMatrix.LoadFromDataSource()
                                        Case "dvrcode"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_DvrCode", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                            oDBDSDetail.SetValue("U_DvrName", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                            oMatrix.LoadFromDataSource()
                                        Case "prjcode"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_PrjCode", pVal.Row - 1, Trim(oDataTable.GetValue("PrjCode", 0)))
                                            oDBDSDetail.SetValue("U_PrjName", pVal.Row - 1, Trim(oDataTable.GetValue("PrjName", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                    End Select

                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "vehicno"
                                        If pVal.BeforeAction = False Then
                                            Dim ocmb As SAPbouiCOM.ComboBox = oMatrix.Columns.Item("type").Cells.Item(pVal.Row).Specific
                                            If ocmb.Selected.Value = "VH" Then
                                                oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSDetail.GetValue("U_VHLNo", pVal.Row - 1).Trim)
                                            Else
                                                oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oDBDSDetail.GetValue("U_VHLNo", pVal.Row - 1).Trim)
                                            End If
                                        End If
                                End Select
                        End Select

                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Matrix_Link_Pressed Event failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                    Try
                        Select Case pVal.ItemUID
                            Case "matrix"
                                Select Case pVal.ColUID
                                    Case "halfdays"
                                        'If pVal.BeforeAction = False Then CalculateNoOfDays(pVal.Row)
                                End Select
                        End Select
                    Catch ex As Exception

                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Select Case pVal.ItemUID
                        Case "t_FromDate", "t_Todate"
                            Dim StrDocDate As String = oDBDSHeader.GetValue("U_DocDate", 0).Trim
                            Dim strFromDate As String = oDBDSHeader.GetValue("U_FromDate", 0).Trim
                            Dim strToDate As String = oDBDSHeader.GetValue("U_Todate", 0).Trim
                            If pVal.Before_Action Then
                                If oGFun.isValidFrAndToDate(StrDocDate, strFromDate) = False Then
                                    oApplication.StatusBar.SetText("From Date Should be Greater Than Or Document Date ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                End If
                                'LveDateValidation(strFromDate, strToDate)
                            End If

                    End Select

                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmFuelDistributionEntry.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmFuelDistributionEntry.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "type"
                                        If pVal.BeforeAction = False And pVal.ItemChanged Then
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_VHLNo", pVal.Row - 1, "")
                                            oDBDSDetail.SetValue("U_VHLName", pVal.Row - 1, "")
                                            oDBDSDetail.SetValue("U_RegNo", pVal.Row - 1, "")
                                            oDBDSDetail.SetValue("U_FuelType", pVal.Row - 1, "")
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item("vehicno").Cells.Item(pVal.Row).Click()
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try

                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_prebynm"
                                oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "EMPCFL", "empID", "select empID from OHEM where dept='13' ")
                                'Case "Matrix"
                                '    Select Case pVal.ColUID
                                '        Case "dvrcode"
                                '            oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "DVRCFL", "empID", "select empID from OHEM where dept='13' ")
                                '    End Select
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "vehicno"
                                        If pVal.BeforeAction = False Then
                                            oMatrix.FlushToDataSource()
                                            Dim oTxt As SAPbouiCOM.Column = oMatrix.Columns.Item(pVal.ColUID)

                                            If Trim(oDBDSDetail.GetValue("U_Type", pVal.Row - 1)).Equals("VH") Then
                                                StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] "
                                                oTxt.ChooseFromListUID = "OVHL_CFL"
                                                oTxt.ChooseFromListAlias = "U_ItemCode"
                                                oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "OVHL_CFL", "U_ItemCode", StrQuery)
                                            Else
                                                StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSDetail.GetValue("U_Type", pVal.Row - 1).Trim & "'"
                                                oTxt.ChooseFromListUID = "OMAC_CFL"
                                                oTxt.ChooseFromListAlias = "Code"
                                                oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "OMAC_CFL", "Code", StrQuery)
                                            End If
                                        End If
                                    Case "dvrcode"
                                        oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "DVRCFL", "empID", "select empID from OHEM where dept='13' ")

                                End Select

                            Case "t_prebynm"
                                If pVal.BeforeAction = False Then
                                    'Dim strquery As String = "EMPFilteration"
                                    'oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "EMPCFL", "empID", strquery)
                                End If

                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "dvrcode"
                                        If pVal.BeforeAction = False Then
                                            'Dim strquery As String = "EMPFilteration"
                                            'oGFun.ChooseFromListFilteration(frmFuelDistributionEntry, "DVRCFL", "empID", strquery)
                                        End If
                                End Select

                        End Select
                    Catch ex As Exception
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "site"
                                        If pVal.Before_Action = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, pVal.Row, pVal.ColUID)
                                        End If

                                End Select
                        End Select

                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                            Case "lk_pay"
                                If pVal.BeforeAction = False Then
                                    Me.DoOpenLinkedObjectForm("PayPeriod", "PayPeriod", "t_code", oDBDSHeader.GetValue("U_PayPerid", 0).Trim)
                                End If

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281"
                    frmFuelDistributionEntry.ActiveItem = "t_empid"
                Case "1282"
                    Me.InitForm()
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Menu Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    Try
                        If BusinessObjectInfo.BeforeAction Then
                            If Me.ValidateAll() = False Then
                                System.Media.SystemSounds.Asterisk.Play()
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If
                        If BusinessObjectInfo.ActionSuccess Then
                            For i As Integer = 1 To oMatrix.VisualRowCount
                                Dim ocmb As SAPbouiCOM.ComboBox = oMatrix.Columns.Item("type").Cells.Item(i).Specific
                                Dim pmdid As String = oMatrix.Columns.Item("vehicno").Cells.Item(i).Specific.value
                                Dim currs As Double = oMatrix.Columns.Item("kmtr").Cells.Item(i).Specific.value
                                Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                If ocmb.Selected.Value = "MC" Then
                                    rset.DoQuery("Exec [_IND_Sp_PMD_MachineMaster] '" & currs & "','" & pmdid & "'")
                                Else
                                    rset.DoQuery("Exec [_IND_Sp_PMD_VehicleMaster]  '" & currs & "','" & pmdid & "'")
                                End If
                            Next
                            If frmFuelDistributionEntry.Items.Item("k_approved").Specific.Checked Then
                                Dim NoOfLeave As Double = 0
                                If oDBDSHeader.GetValue("U_NoDayLev", 0).Trim.Equals("") = False Then NoOfLeave = oDBDSHeader.GetValue("U_NoDayLev", 0).Trim
                                Dim sQuery As String = " UPDATE  [@INPR_ECI6]  SET U_LveTaken = U_LveTaken + " & NoOfLeave & " ,U_BalLeave = U_TotalLve + U_BalLvpYr - (U_LveTaken + " & NoOfLeave & ")   FROM [@INPR_OECI] a ,[@INPR_ECI6] b WHERE a.Code = b.Code " _
                                & " AND a.U_empID = '" & oDBDSHeader.GetValue("U_empID", 0).Trim & "' AND U_LveCode ='" & oDBDSHeader.GetValue("U_LeavCode", 0).Trim & "' "
                                Dim rsetQuery As SAPbobsCOM.Recordset = oGFun.DoQuery(sQuery)
                                Dim dtFromDate, dtToDate As Date
                                If frmFuelDistributionEntry.Items.Item("t_LvSanFDt").Specific.Value.Equals("") And frmFuelDistributionEntry.Items.Item("t_LvSanTDt").Specific.Value.Equals("") Then
                                    dtFromDate = DateTime.ParseExact(frmFuelDistributionEntry.Items.Item("t_FromDate").Specific.Value, "yyyyMMdd", Nothing)
                                    dtToDate = DateTime.ParseExact(frmFuelDistributionEntry.Items.Item("t_Todate").Specific.Value, "yyyyMMdd", Nothing)
                                Else
                                    dtFromDate = DateTime.ParseExact(frmFuelDistributionEntry.Items.Item("t_LvSanFDt").Specific.Value, "yyyyMMdd", Nothing)
                                    dtToDate = DateTime.ParseExact(frmFuelDistributionEntry.Items.Item("t_LvSanTDt").Specific.Value, "yyyyMMdd", Nothing)
                                End If


                                Dim YesNo As String = oDBDSHeader.GetValue("U_AnualLve", 0).Trim & oDBDSHeader.GetValue("U_PaidLve", 0)
                                While dtFromDate <= dtToDate

                                    sQuery = " Update [@INPR_DAS1] SET U_AttStatus = (Select CASE '" & YesNo & "' When 'YN'  THEN 'AL'  WHEN 'NY'  THEN 'PL' WHEN 'NN'  THEN 'LP'  END   Status)   FROM [@INPR_ODAS] a , [@INPR_DAS1] b  WHERE a.DocEntry = b.DocEntry AND  " _
                                    & " b.U_empID = '" & oDBDSHeader.GetValue("U_empID", 0).Trim & "' AND a.U_Payperid = '" & oDBDSHeader.GetValue("U_PayPerid", 0).Trim & "' AND  U_AttdDate = Convert(DateTime,'" & CDate(dtFromDate).ToString("yyyyMMdd") & "',112) "
                                    rsetQuery = oGFun.DoQuery(sQuery)
                                    dtFromDate = dtFromDate.AddDays(1)
                                End While
                                frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE

                            End If
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Form Data Add ,Update Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        BubbleEvent = False
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess Then
                        If oDBDSHeader.GetValue("U_Approved", 0).Trim.Equals("Y") Or oDBDSHeader.GetValue("Status", 0).Trim.Equals("C") Then
                            frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        Else
                            frmFuelDistributionEntry.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        End If
                    End If

            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Form Data Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub RightClickEvent(ByRef EventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case EventInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK
                    If frmFuelDistributionEntry.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
    Sub DoOpenLinkedObjectForm(ByVal FormUniqueID As String, ByVal ActivateMenuItem As String, ByVal FindItemUID As String, ByVal FindItemUIDValue As String)
        Try
            Dim oForm As SAPbouiCOM.Form
            Dim Bool As Boolean = False

            For frm As Integer = 0 To oApplication.Forms.Count - 1
                If oApplication.Forms.Item(frm).UniqueID = FormUniqueID Then
                    oForm = oApplication.Forms.Item(FormUniqueID)
                    oForm.Close()
                    Exit For
                End If
            Next
            If Bool = False Then
                oApplication.ActivateMenuItem(ActivateMenuItem)
                oForm = oApplication.Forms.Item(ActivateMenuItem)
                oForm.Select()
                oForm.Freeze(True)
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                oForm.Items.Item(FindItemUID).Specific.Value = Trim(FindItemUIDValue)
                oForm.Items.Item("1").Click()
                oForm.Freeze(False)
            End If
        Catch ex As Exception

        Finally
        End Try
    End Sub
End Class
