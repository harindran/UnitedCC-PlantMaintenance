Public Class CardRenewalEntry
    Dim frmCardRenewalEntry As SAPbouiCOM.Form
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSHeader, oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OCRD"
    Dim boolstatus As Boolean
    Dim SQuery As String = ""
    Sub LoadCardRenewalEntry()
        Try
            oGFun.LoadXML(frmCardRenewalEntry, CardRenewalEntryFormID, CardRenewalEntryXML)
            frmCardRenewalEntry = oApplication.Forms.Item(CardRenewalEntryFormID)
            oDBDSHeader = frmCardRenewalEntry.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmCardRenewalEntry.DataSources.DBDataSources.Item(1)
            oMatrix = frmCardRenewalEntry.Items.Item("Matrix").Specific
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception
            oGFun.Msg("Load CardRenewalEntry Failed")
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            oGFun.LoadComboBoxSeries(frmCardRenewalEntry.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmCardRenewalEntry.Items.Item("t_docdate").Specific) ' Load Document Date
            frmCardRenewalEntry.ActiveItem = "t_expdtno"
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmCardRenewalEntry.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmCardRenewalEntry.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmCardRenewalEntry.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean

        Try
            ''Man(Name)
            'If frmCardRenewalEntry.Items.Item("t_tcode").Specific.value.Equals("") = True Then
            '    oApplication.StatusBar.SetText("Transfer Of Man Power Code Should Not Be Left Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If

            ''Camp Validation

            For k As Integer = 1 To oMatrix.VisualRowCount - 1
                If oMatrix.Columns.Item("nexprdt").Cells.Item(k).Specific.value.Equals("") = True Then
                    oGFun.StatusBarErrorMsg("Line [" & k & "] : Next Expiry Date Should Not Be Empty")
                    Return False
                End If
            Next


            Return True
        Catch ex As Exception
            oGFun.Msg("Validate all Function Failed: ")
        Finally
        End Try

    End Function
    Sub LoadCardDetails()
        Try

            Dim rsetCard As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim iNoOfExpDays As String = 0
            If Trim(oDBDSHeader.GetValue("U_NoExpDt", 0)).Equals("") = False Then iNoOfExpDays = oDBDSHeader.GetValue("U_NoExpDt", 0)
            sQuery = "SELECT A.U_empID,A.U_empName, b.LineID, B.U_Type,B.U_CardNo,B.U_ExpirDt,B.U_IssueDt,B.U_IsuePlce,b.Code FROM [@MIPL_PM_OLAC] A inner join [@MIPL_PM_LAC1] B ON A.Code=B.Code and CONVERT(DATETIME,B.U_ExpirDt,113) > GETDATE() - " & iNoOfExpDays & " and CONVERT(DATETIME,B.U_ExpirDt,113) <= GETDATE()   "
            rsetCard.DoQuery(sQuery)
            oMatrix.Clear()
            oDBDSDetail.Clear()
            rsetCard.MoveFirst()
            For i As Integer = 1 To rsetCard.RecordCount
                oDBDSDetail.InsertRecord(oDBDSDetail.Size)
                oDBDSDetail.Offset = oDBDSDetail.Size - 1
                oDBDSDetail.SetValue("LineID", oDBDSDetail.Offset, oDBDSDetail.Size)
                oDBDSDetail.SetValue("U_empID", oDBDSDetail.Offset, rsetCard.Fields.Item("U_empID").Value)
                oDBDSDetail.SetValue("U_empName", oDBDSDetail.Offset, rsetCard.Fields.Item("U_empName").Value)
                oDBDSDetail.SetValue("U_LineNum", oDBDSDetail.Offset, rsetCard.Fields.Item("LineID").Value)
                oDBDSDetail.SetValue("U_Code", oDBDSDetail.Offset, rsetCard.Fields.Item("Code").Value)
                oDBDSDetail.SetValue("U_CardType", oDBDSDetail.Offset, rsetCard.Fields.Item("U_Type").Value)
                oDBDSDetail.SetValue("U_CardNo", oDBDSDetail.Offset, rsetCard.Fields.Item("U_CardNo").Value)
                oDBDSDetail.SetValue("U_NCardNo", oDBDSDetail.Offset, rsetCard.Fields.Item("U_CardNo").Value)
                If Trim(rsetCard.Fields.Item("U_ExpirDt").Value).Equals("") = False Then oDBDSDetail.SetValue("U_ExpirDt", oDBDSDetail.Offset, CDate(rsetCard.Fields.Item("U_ExpirDt").Value).ToString("yyyyMMdd"))
                If Trim(rsetCard.Fields.Item("U_IssueDt").Value).Equals("") = False Then oDBDSDetail.SetValue("U_IssueDt", oDBDSDetail.Offset, CDate(rsetCard.Fields.Item("U_IssueDt").Value).ToString("yyyyMMdd"))
                If Trim(rsetCard.Fields.Item("U_IssueDt").Value).Equals("") = False Then oDBDSDetail.SetValue("U_NIsseDt", oDBDSDetail.Offset, CDate(rsetCard.Fields.Item("U_IssueDt").Value).ToString("yyyyMMdd"))
                oDBDSDetail.SetValue("U_IsuePlce", oDBDSDetail.Offset, rsetCard.Fields.Item("U_IsuePlce").Value)
                oDBDSDetail.SetValue("U_NIsuePlc", oDBDSDetail.Offset, rsetCard.Fields.Item("U_IsuePlce").Value)

                rsetCard.MoveNext()
            Next
            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            oGFun.Msg("Load Card Details Method Failed:" & ex.Message())
        Finally
        End Try
    End Sub

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.EventType
                Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                    Try
                        Dim oDataTable As SAPbouiCOM.DataTable
                        Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                        oDataTable = oCFLE.SelectedObjects
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False And frmCardRenewalEntry.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            If frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                            Select Case pVal.ItemUID
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "empid"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_empID", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                            oDBDSDetail.SetValue("U_empName", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                    End Select
                                Case "t_prebynam"
                                    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                            End Select
                        End If
                    Catch ex As Exception
                        oGFun.Msg("Choose From List Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "empid"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, pVal.Row, pVal.ColUID)
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Lost Focus Event Failed:" & ex.Message)
                    Finally
                    End Try

                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID

                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Got Focus Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID
                            Case "t_expdtno"
                                If pVal.BeforeAction = False And pVal.ItemChanged Then
                                    Me.LoadCardDetails()
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "issuedtn"
                                        If pVal.BeforeAction Then
                                            If oGFun.isDateCompare(oMatrix.Columns.Item("issuedt").Cells.Item(pVal.Row).Specific, oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific, "New Issue date should not be less then previouse issue date") = False Then BubbleEvent = False
                                        End If
                                    Case "nexprdt"
                                        If pVal.BeforeAction Then
                                            If oGFun.isDateCompare(oMatrix.Columns.Item("expirdt").Cells.Item(pVal.Row).Specific, oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific, "New Expire date should not be less then previouse Expire date") = False Then BubbleEvent = False
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Validate Event Failed:" & ex.Message())
                    End Try
                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmCardRenewalEntry.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmCardRenewalEntry.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Combo Select Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "1"

                                If pVal.BeforeAction = True And (frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                                If pVal.BeforeAction And (frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If boolstatus = False Then
                                        oApplication.MessageBox("Are you sure. You Cannot Change Anything After 'Add' Continue?", 1, "Ok", "Cancel")
                                        boolstatus = True
                                    End If
                                End If
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Click Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmCardRenewalEntry.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oGFun.Msg("Item Pressed Event Failed:")
        Finally
        End Try

    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281"
                    frmCardRenewalEntry.ActiveItem = "t_expdtno"
                Case "1282"
                    If pVal.BeforeAction = False Then
                        Me.InitForm()
                    End If
                Case "1293"
                    If pVal.BeforeAction = False Then oGFun.DeleteRow(oMatrix, oDBDSDetail)

            End Select
        Catch ex As Exception
            oGFun.Msg("Menu Event Failed:")
        Finally
        End Try
    End Sub

    Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    If BusinessObjectInfo.BeforeAction = True Then
                        If Me.ValidateAll() = False Then
                            System.Media.SystemSounds.Asterisk.Play()
                            BubbleEvent = False
                            Exit Sub
                        Else
                            oGFun.DeleteEmptyRowInFormDataEvent(oMatrix, "empid", oDBDSDetail)
                        End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    Try
                        oGFun.SetDocumentStatus(oDBDSHeader, frmCardRenewalEntry)
                    Catch ex As Exception
                        oGFun.StatusBarErrorMsg("Form Data event Failed : " & ex.Message)
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oGFun.Msg("Form Data Event Failed:")
        Finally
        End Try
    End Sub

    Sub RightClickEvent(ByRef EventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case EventInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK
                    If frmCardRenewalEntry.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction Then
                        If EventInfo.Row = oMatrix.VisualRowCount Then
                            frmCardRenewalEntry.EnableMenu("1293", False)
                        Else
                            frmCardRenewalEntry.EnableMenu("1293", True)
                        End If
                    End If
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub
End Class
