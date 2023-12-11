Class BreakDownSlip
    Dim frmBreakDownSlip As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OBDS"
    Dim StrQuery As String = ""

    Sub LoadBreakDownSlip()
        Try
            oGFun.LoadXML(frmBreakDownSlip, BreakDownSlipFormID, BreakDownSlipXML)
            frmBreakDownSlip = oApplication.Forms.Item(BreakDownSlipFormID)
            setReport(BreakDownSlipFormID)
            oDBDSHeader = frmBreakDownSlip.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmBreakDownSlip.DataSources.DBDataSources.Item(1)
            frmBreakDownSlip.PaneLevel = 1
            oMatrix = frmBreakDownSlip.Items.Item("Matrix").Specific
            Me.DefineModesForFields()
            ' frmBreakDownSlip.PaneLevel = 1
            Me.InitForm()
        Catch ex As Exception
            oGFun.Msg("Load Parameter Master Failed")
        Finally
        End Try
    End Sub
    Sub InitForm()
        Try
            oGFun.LoadComboBoxSeries(frmBreakDownSlip.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmBreakDownSlip.Items.Item("t_docdate").Specific) ' Load Document Date
            oGFun.LoadLocationComboBox(frmBreakDownSlip.Items.Item("c_location").Specific) ' Load the location Combo Box...
            frmBreakDownSlip.ActiveItem = "c_location"
            If HANA Then
                oGFun.setComboBoxValue(frmBreakDownSlip.Items.Item("t_type").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")
            Else
                oGFun.setComboBoxValue(frmBreakDownSlip.Items.Item("t_type").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If

            oGFun.SetNewLine(oMatrix, oDBDSDetail)
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmBreakDownSlip.Freeze(False)
        Finally
        End Try
    End Sub

    Private Sub setReport(ByVal FormUID As String)
        Try
            frmBreakDownSlip = oApplication.Forms.Item(FormUID)
            Dim rptTypeService As SAPbobsCOM.ReportTypesService
            'Dim newType As SAPbobsCOM.ReportType
            Dim newtypesParam As SAPbobsCOM.ReportTypesParams
            rptTypeService = oCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)
            newtypesParam = rptTypeService.GetReportTypeList
            Dim TypeCode As String
            If HANA Then
                TypeCode = oGFun.getSingleValue("Select ""CODE"" from RTYP where ""NAME""='Breakdown'")
            Else
                TypeCode = oGFun.getSingleValue("Select CODE from RTYP where NAME='Breakdown'")
            End If
            frmBreakDownSlip.ReportType = TypeCode
            'Dim i As Integer
            'For i = 0 To newtypesParam.Count - 1
            '    If newtypesParam.Item(i).TypeName = "Breakdown" And newtypesParam.Item(i).MenuID = "Breakdown" Then
            '        frmBreakDownSlip.ReportType = newtypesParam.Item(i).TypeCode
            '        Exit For
            '    End If
            'Next i
        Catch ex As Exception
            oApplication.StatusBar.SetText("setReport Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmBreakDownSlip.Items.Item("t_itemname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmBreakDownSlip.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmBreakDownSlip.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmBreakDownSlip.Items.Item("t_itemcode").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("t_rptbynam").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("t_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("t_prjname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("t_repairdt").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmBreakDownSlip.Items.Item("c_status").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Add, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean

        Try
            'Machine Number
            If oDBDSHeader.GetValue("U_ItemCode", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("M/C Equipment No. Should Not Be Left Empty")
                Return False
            End If


            If oDBDSHeader.GetValue("U_RepairDt", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Daet Of Repair Should Not Be Left Empty")
                Return False
            End If
            'If oDBDSHeader.GetValue("U_CmpDate", 0).Equals(Trim("")) = True Then
            '    oGFun.StatusBarErrorMsg("Completion Date Should Not Be Left Empty")
            '    Return False
            'End If
            'If oGFun.isDateCompare(frmBreakDownSlip.Items.Item("t_repairdt").Specific, frmBreakDownSlip.Items.Item("t_docdate").Specific, "Repair Date Should Not be Greater than  Document Date ") = False Then Return False
            'If oGFun.isDateCompare(frmBreakDownSlip.Items.Item("t_repairdt").Specific, frmBreakDownSlip.Items.Item("t_cmpdate").Specific, "Completion Date Should  be Greater than  Repair Date ") = False Then Return False

            If frmBreakDownSlip.Items.Item("et_break").Specific.value.Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Break Down Details Should Not Be Left Empty")
                Return False
            End If

            If oDBDSHeader.GetValue("U_PMDByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("PMD Manager Should Not Be Left Empty")
                Return False
            ElseIf oDBDSHeader.GetValue("U_SupByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Supervisor Should Not Be Left Empty")
                Return False
            End If

            Return True
        Catch ex As Exception
            oGFun.Msg("Validate all Function Failed: ")
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
                                Case "t_itemcode"
                                    If Not (oCFLE.SelectedObjects Is Nothing) Then
                                        oDBDSHeader.SetValue("U_ItemCode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_ItemName", 0, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                    End If
                                Case "t_prjname"
                                    oDBDSHeader.SetValue("U_PrjCode", 0, Trim(oDataTable.GetValue("PrjCode", 0)))
                                    oDBDSHeader.SetValue("U_PrjName", 0, Trim(oDataTable.GetValue("PrjName", 0)))
                                Case "t_rptbynam"
                                    oDBDSHeader.SetValue("U_RptByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_RptByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_pmdname"
                                    oDBDSHeader.SetValue("U_PMDByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PMDByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_supname"
                                    oDBDSHeader.SetValue("U_SupByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_SupByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                            End Select
                        End If
                    Catch ex As Exception
                        oGFun.Msg("Choose From List Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID
                            Case "t_prebydat"

                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Validate Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_itemcode"
                                If pVal.BeforeAction = False Then
                                    Dim oTxt As SAPbouiCOM.EditText = frmBreakDownSlip.Items.Item(pVal.ItemUID).Specific
                                    If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                                        If HANA Then
                                            StrQuery = "SELECT ""Code""  from ""@MIPL_PM_OVHL"" "
                                        Else
                                            StrQuery = "SELECT Code  from [@MIPL_PM_OVHL] "
                                        End If

                                        oTxt.ChooseFromListUID = "OVHL_CFL"
                                        oTxt.ChooseFromListAlias = "U_ItemCode"
                                        '  oGFun.ChooseFromListFilteration(frmBreakDownSlip, "OVHL_CFL", "Code", StrQuery)
                                    Else
                                        If HANA Then
                                            StrQuery = "select """" from ""@MIPL_PM_OMAC"" Where ""U_InsType""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                        Else
                                            StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                        End If

                                        oTxt.ChooseFromListUID = "OMAC_CFL"
                                        oTxt.ChooseFromListAlias = "Code"
                                        ' oGFun.ChooseFromListFilteration(frmBreakDownSlip, "OMAC_CFL", "Code", StrQuery)
                                    End If
                                End If

                            Case "t_rptbynam"
                                ' oGFun.ChooseFromListFilteration(frmBreakDownSlip, "RPTBY_CFL", "empID", "select empID from OHEM where dept='13' ")
                            Case "t_pmdname"
                                'oGFun.ChooseFromListFilteration(frmBreakDownSlip, "PMDCFL", "empID", "select empID from OHEM where dept='13'")
                            Case "t_supname"
                                'oGFun.ChooseFromListFilteration(frmBreakDownSlip, "SUPCFL", "empID", "select empID from OHEM where dept='13'")
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_repairdt"
                                If pVal.BeforeAction = False Then
                                    If oGFun.isDateCompare(frmBreakDownSlip.Items.Item(pVal.ItemUID).Specific, frmBreakDownSlip.Items.Item("t_docdate").Specific, "Repair Date Should Not be Greater than  Document Date ") = False Then BubbleEvent = False
                                    Exit Sub
                                End If
                            Case "t_cmpdate"
                                If pVal.BeforeAction = False Then
                                    If oGFun.isDateCompare(frmBreakDownSlip.Items.Item("t_repairdt").Specific, frmBreakDownSlip.Items.Item("t_cmpdate").Specific, "Completion Date Should  be Greater than  Repair Date ") = False Then BubbleEvent = False
                                    Exit Sub
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "workcot"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, pVal.Row, pVal.ColUID)
                                        End If
                                        frmBreakDownSlip.Freeze(True)
                                        oMatrix.AutoResizeColumns()
                                        frmBreakDownSlip.Freeze(False)
                                End Select
                        End Select
                    Catch ex As Exception
                        frmBreakDownSlip.Freeze(False)
                        oGFun.Msg("Lost Focus Event Failed:")
                    Finally
                    End Try
                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmBreakDownSlip.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmBreakDownSlip.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If
                            Case "t_type"
                                Dim ocmb As SAPbouiCOM.ComboBox = frmBreakDownSlip.Items.Item("t_type").Specific
                                If pVal.BeforeAction = False And pVal.ItemChanged = True Then
                                    frmBreakDownSlip.Items.Item("t_itemcode").Specific.value = ""
                                    frmBreakDownSlip.Items.Item("t_itemname").Specific.value = ""
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
                                If pVal.BeforeAction = True And (frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            Case "f_break"
                                If pVal.BeforeAction = False Then
                                    frmBreakDownSlip.PaneLevel = 1
                                End If
                            Case "f_analysis"
                                If pVal.BeforeAction = False Then
                                    frmBreakDownSlip.PaneLevel = 2
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
                                If pVal.ActionSuccess And frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If

                            Case "lnk_item"
                                If pVal.BeforeAction = False Then
                                    If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                                        oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSHeader.GetValue("U_ItemCode", 0).Trim)
                                    Else
                                        oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oDBDSHeader.GetValue("U_ItemCode", 0).Trim)
                                    End If
                                End If
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Item Pressed Event Failed:")
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oGFun.Msg("Item Event Failed:")
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281" 'Find                    
                    oMatrix.Item.Enabled = False
                Case "1284"
                    If pVal.BeforeAction = False Then
                        'Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        'If frmBreakDownSlip.Items.Item("t_type").Specific.value = "VH" Then
                        '    rset.DoQuery("Exec _IND_Sp_PMD_Vehicle_Activity_Updation_BreakDown '" & frmBreakDownSlip.Items.Item("t_docnum").Specific.value & "' ")
                        'Else
                        '    rset.DoQuery("Exec _IND_Sp_PMD_Machine_Activity_Updation_BreakDown '" & frmBreakDownSlip.Items.Item("t_docnum").Specific.value & "' ")
                        'End If
                    End If
                    Me.InitForm()
                Case "1282"
                    If pVal.BeforeAction = False Then
                        Me.InitForm()
                    End If
                Case "1293"
                    oGFun.DeleteRow(oMatrix, oDBDSDetail)
                Case "1287"
                    oGFun.LoadComboBoxSeries(frmBreakDownSlip.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
                    oGFun.LoadDocumentDate(frmBreakDownSlip.Items.Item("t_docdate").Specific)
                    frmBreakDownSlip.Items.Item("t_cmpdate").Specific.String = ""
                    Dim cmb_status As SAPbouiCOM.ComboBox
                    cmb_status = frmBreakDownSlip.Items.Item("c_status").Specific
                    cmb_status.Select("O", SAPbouiCOM.BoSearchKey.psk_ByValue)
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
                            oGFun.DeleteEmptyRowInFormDataEvent(oMatrix, "workcot", oDBDSDetail)
                        End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                        oGFun.SetDocumentStatus(oDBDSHeader, frmBreakDownSlip)
                    End If
                    'If frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
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
                    If EventInfo.BeforeAction = True Then
                        If frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If EventInfo.ItemUID = "Matrix" Then
                                If EventInfo.Row = oMatrix.VisualRowCount Then
                                    frmBreakDownSlip.EnableMenu("1293", False)
                                Else
                                    frmBreakDownSlip.EnableMenu("1293", True)
                                End If
                            Else
                                frmBreakDownSlip.EnableMenu("1293", False)
                            End If
                        End If
                        frmBreakDownSlip.EnableMenu("1284", False)
                        frmBreakDownSlip.EnableMenu("1285", False)
                        frmBreakDownSlip.EnableMenu("1286", False)
                        If frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE Or frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmBreakDownSlip.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmBreakDownSlip.EnableMenu("1287", True)  'Duplicate
                        Else
                            frmBreakDownSlip.EnableMenu("1287", False)
                        End If
                    Else
                        frmBreakDownSlip.EnableMenu("1293", False)
                    End If
                    
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

    Public Sub LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean)
        Try
            frmBreakDownSlip = oGFun.oApplication.Forms.Item(eventInfo.FormUID)
            'eventInfo.LayoutKey = frmBreakDownSlip.Items.Item("t_docnum").Specific.string
            eventInfo.LayoutKey = frmBreakDownSlip.DataSources.DBDataSources.Item("@MIPL_PM_OBDS").GetValue("DocEntry", 0)
        Catch ex As Exception
        End Try
    End Sub

End Class