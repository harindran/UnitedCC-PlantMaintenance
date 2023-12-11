Class PaymentCertificate

    Dim frmPaymentCertificate As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oDBDSDetail1, oDBDSDetail2 As SAPbouiCOM.DBDataSource
    Dim oMatrix1, oMatrix2 As SAPbouiCOM.Matrix
    Dim UDOID As String = "OPAY"
    Dim DeleteRowITEMUID As String = ""
    Dim typee As String
    Dim plantid As String
    Dim StrQuery As String = ""

    Sub LoadPaymentCertificate()
        Try
            oGFun.LoadXML(frmPaymentCertificate, PaymentCertificateFormID, PaymentCertificateXML)
            frmPaymentCertificate = oApplication.Forms.Item(PaymentCertificateFormID)

            oDBDSHeader = frmPaymentCertificate.DataSources.DBDataSources.Item(0)
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmPaymentCertificate.Freeze(True)
            frmPaymentCertificate.PaneLevel = 1
            oGFun.LoadComboBoxSeries(frmPaymentCertificate.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmPaymentCertificate.Items.Item("t_docdate").Specific) ' Load Document Date
            'oGFun.setComboBoxValue(frmPaymentCertificate.Items.Item("cmb_plant").Specific, "Select Code, Location from OLCT") 'Load Location)
            'oGFun.setComboBoxValue(frmPaymentCertificate.Items.Item("c_Shift").Specific, "Select Code,U_SftType from [@INM_OSFT]") ' Load the location Combo Box...
            frmPaymentCertificate.ActiveItem = "t_type"
            frmPaymentCertificate.Freeze(False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmPaymentCertificate.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmPaymentCertificate.Items.Item("t_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmPaymentCertificate.Items.Item("t_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmPaymentCertificate.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmPaymentCertificate.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try

            If oDBDSHeader.GetValue("U_mactype", 0).Trim = "" Then
                oApplication.StatusBar.SetText("Type Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_type"
                Exit Function
            End If
            If oDBDSHeader.GetValue("U_macno", 0).Trim = "" Then
                oApplication.StatusBar.SetText("Machine No Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_macno"
                Exit Function
            End If
            If frmPaymentCertificate.Items.Item("txt_shano").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Sch.Activity No Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_shano"
                Exit Function
            End If
            If frmPaymentCertificate.Items.Item("txt_ename").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Employee Name Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_ename"
                Exit Function
            End If
            If frmPaymentCertificate.Items.Item("txt_ftime").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("From Time Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_ftime"
                Exit Function
            End If
            If frmPaymentCertificate.Items.Item("txt_ttime").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("To Time Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "txt_ttime"
                Exit Function
            End If
            If oDBDSHeader.GetValue("U_Freqncy", 0).Trim = "" Then
                oApplication.StatusBar.SetText("Frequency Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmPaymentCertificate.ActiveItem = "t_freq"
                Exit Function
            End If
            If oMatrix1.VisualRowCount > 0 Then
                For i As Integer = 1 To oMatrix1.VisualRowCount
                    oMatrix1.GetLineData(i)
                    If Trim(oMatrix1.Columns.Item("shcactivty").Cells.Item(i).Specific.Value).Equals("") = True Then
                        oApplication.StatusBar.SetText("Row No: " & i & " Schedule Activity Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Exit Function
                    End If
                Next
            ElseIf oMatrix1.VisualRowCount = 0 Then
                oApplication.StatusBar.SetText("Grid Details Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If
            'If oMatrix2.VisualRowCount > 0 Then
            '    For i As Integer = 1 To oMatrix2.VisualRowCount
            '        oMatrix2.GetLineData(i)
            '        If Trim(oMatrix2.Columns.Item("itemid").Cells.Item(i).Specific.Value).Equals("") = True Then
            '            oApplication.StatusBar.SetText("Row No: " & i & " Item ID Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '            Exit Function
            '        Else
            '            If CDbl(oMatrix2.Columns.Item("qty").Cells.Item(i).Specific.Value) <= 0 Then
            '                oApplication.StatusBar.SetText("Qty Cannot Be Left Empty or Less Than Zero.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '                Exit Function
            '            End If
            '        End If
            '    Next
            'End If

            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Validate Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            ValidateAll = False
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
                        Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oDataTable = oCFLE.SelectedObjects
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            Select Case pVal.ItemUID
                                Case "txt_shano"
                                    oDBDSHeader.SetValue("U_schactno", 0, Trim(oDataTable.GetValue("DocNum", 0)))
                                    oDBDSHeader.SetValue("U_pmcno", 0, Trim(oDataTable.GetValue("U_pmcno", 0)))
                                    Dim oFrequency As SAPbouiCOM.ComboBox = frmPaymentCertificate.Items.Item("t_freq").Specific
                                    Dim rsetFreq As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    StrQuery = "  SELECT B.U_actcode ,B.U_activity,U_schdt ,U_nxtschdt  FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP1] B    WHERE A.DocEntry =B.DocEntry  AND A.DocNum ='" & oDBDSHeader.GetValue("U_schactno", 0).Trim & "'  AND B.U_freq ='" & oDBDSHeader.GetValue("U_Freqncy", 0).Trim & "'"
                                    rsetFreq.DoQuery(StrQuery)
                                    oMatrix1.Clear()
                                    oDBDSDetail1.Clear()
                                    For i As Integer = 1 To rsetFreq.RecordCount
                                        oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                                        oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                                        oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                                        oDBDSDetail1.SetValue("U_actcode", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_actcode").Value)
                                        oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_activity").Value)
                                        oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "Pending")
                                        oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, CDate(rsetFreq.Fields.Item("U_schdt").Value).ToString("yyyyMMdd"))
                                        oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, CDate(rsetFreq.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd"))
                                    Next
                                    oMatrix1.LoadFromDataSource()
                                    'Me.LoadDetails()
                                Case "txt_ename"
                                    oDBDSHeader.SetValue("U_empID", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_empName", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                Case "txt_agency"
                                    oDBDSHeader.SetValue("U_agcode", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                    oDBDSHeader.SetValue("U_agcyname", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                Case "txt_macno"
                                    If Not (oCFLE.SelectedObjects Is Nothing) Then
                                        oDBDSHeader.SetValue("U_macno", 0, Trim(oDataTable.GetValue("U_ToolNo", 0)))
                                        oDBDSHeader.SetValue("U_macdesc", 0, Trim(oDataTable.GetValue("U_ToolName", 0)))
                                        oDBDSHeader.SetValue("U_pmcno", 0, Trim(oDataTable.GetValue("U_PMCheck", 0)))
                                    End If

                                Case "mtx_1"
                                    Select Case pVal.ColUID
                                        Case "itemid"
                                            oMatrix2.FlushToDataSource()
                                            oDBDSDetail2.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                            oDBDSDetail2.SetValue("U_itemid", pVal.Row - 1, oDataTable.GetValue("ItemCode", 0))
                                            oDBDSDetail2.SetValue("U_itemdesc", pVal.Row - 1, oDataTable.GetValue("ItemName", 0))
                                            oDBDSDetail2.SetValue("U_uom", pVal.Row - 1, oDataTable.GetValue("InvntryUom", 0))
                                            oDBDSDetail2.SetValue("U_stock", pVal.Row - 1, oDataTable.GetValue("OnHand", 0))
                                            oMatrix2.LoadFromDataSource()
                                            ' oMatrix1.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                    End Select
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "shcactivty"
                                            oMatrix1.FlushToDataSource()
                                            For i As Integer = 1 To oMatrix1.VisualRowCount
                                                '    If oMatrix1.Columns.Item("shcactivty").Cells.Item(i).Specific.value.ToString.Trim() = Trim(oDataTable.GetValue("U_schacty", 0)).Trim Then
                                                '        oGFun.StatusBarErrorMsg(oMatrix1.Columns.Item("shcactivty").Cells.Item(i).Specific.value.ToString.Trim() & " Already Exists in the Table")
                                                '        BubbleEvent = False
                                                '        Exit Sub
                                                '    End If
                                            Next
                                            Dim clno As String = "0"
                                            '= GetValue("U_activity", 0)

                                            Dim rsetScheduleActivity As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                            Dim s As String = "SELECT D.LineId,D.U_actcode,D.U_activity,D.U_freq,D.U_nxtschdt,D.U_remarks,D.U_schdt FROM [@FAST_PM_SCHACT_A] D,[@FAST_PM_SCHACT] H WHERE H.DocEntry=D.DocEntry AND D.U_activity='" & Trim(clno) & "'"
                                            rsetScheduleActivity.DoQuery(s)
                                            For a As Integer = 1 To rsetScheduleActivity.RecordCount
                                                oDBDSDetail1.Offset = pVal.Row - 1
                                                oDBDSDetail1.SetValue("U_actcode", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_actcode").Value)
                                                oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_activity").Value)
                                                oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "Pending")
                                                oDBDSDetail1.SetValue("U_freq", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_freq").Value)
                                                Dim schdt As String = CDate(rsetScheduleActivity.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd")
                                                oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, CDate(rsetScheduleActivity.Fields.Item("U_schdt").Value).ToString("yyyyMMdd"))
                                                oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, schdt)
                                                'oDBDSDetail.SetValue("U_schdt", oDBDSDetail.Offset, schedt)
                                                oDBDSDetail1.SetValue("U_remarks", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_remarks").Value)
                                                oMatrix1.SetLineData(pVal.Row)
                                                If a < rsetScheduleActivity.RecordCount - 1 Then
                                                    rsetScheduleActivity.MoveNext()
                                                End If
                                            Next
                                    End Select
                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try

                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "mtx_1"
                                Select Case pVal.ColUID
                                    Case "itemid"
                                        oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemid")
                                End Select
                            Case "mtx_0"
                                Select Case pVal.ColUID
                                    Case "schedate"
                                        oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, "schedate")
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try

                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmPaymentCertificate.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmPaymentCertificate.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If
                            Case "cmb_plant"
                                Try
                                    If pVal.BeforeAction = True Then
                                        plantid = oDBDSHeader.GetValue("U_plant", 0).Trim
                                    End If
                                    Dim strType As String = oDBDSHeader.GetValue("U_mactype", 0).Trim
                                    If pVal.BeforeAction = False And Trim(oDBDSHeader.GetValue("U_plant", 0)).Equals(plantid) = False Then
                                        oDBDSHeader.SetValue("U_mactype", 0, "")
                                        oDBDSHeader.SetValue("U_macno", 0, "")
                                        oDBDSHeader.SetValue("U_macdesc", 0, "")
                                        oDBDSHeader.SetValue("U_schactno", 0, "")
                                        oDBDSHeader.SetValue("U_pmcno", 0, "")
                                        oMatrix1.Clear()
                                        oDBDSDetail1.Clear()
                                        oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
                                        oMatrix2.Clear()
                                        oDBDSDetail1.Clear()
                                        oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                    End If
                                Catch ex As Exception
                                    oApplication.StatusBar.SetText("Plant Combo Select Event  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                Finally
                                End Try
                            Case "txt_type"
                                Try
                                    If pVal.BeforeAction = True Then
                                        typee = oDBDSHeader.GetValue("U_mactype", 0).Trim
                                    End If
                                    Dim strType As String = oDBDSHeader.GetValue("U_mactype", 0).Trim
                                    Dim oTxtMachine As SAPbouiCOM.EditText = frmPaymentCertificate.Items.Item("txt_macno").Specific

                                    frmPaymentCertificate.Items.Item("lbl_macno").Specific.caption = "Machine No"
                                    frmPaymentCertificate.Items.Item("lbl_mdesc").Specific.caption = "Machine Desc."
                                    oDBDSHeader.SetValue("U_macno", 0, "")
                                    oDBDSHeader.SetValue("U_macdesc", 0, "")
                                    oDBDSHeader.SetValue("U_schactno", 0, "")
                                    oDBDSHeader.SetValue("U_pmcno", 0, "")
                                Catch ex As Exception
                                    oApplication.StatusBar.SetText("Type Combo Select Event  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                Finally
                                End Try
                            Case "t_freq"
                                If pVal.BeforeAction = False Then
                                    Dim oFrequency As SAPbouiCOM.ComboBox = frmPaymentCertificate.Items.Item("t_freq").Specific
                                    Dim rsetFreq As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    StrQuery = "  SELECT B.U_actcode ,B.U_activity,U_schdt ,U_nxtschdt  FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP1] B    WHERE A.DocEntry =B.DocEntry  AND A.DocNum ='" & oDBDSHeader.GetValue("U_schactno", 0).Trim & "'  AND B.U_freq ='" & oFrequency.Selected.Value & "'"
                                    rsetFreq.DoQuery(StrQuery)
                                    oMatrix1.Clear()
                                    oDBDSDetail1.Clear()
                                    For i As Integer = 1 To rsetFreq.RecordCount
                                        oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                                        oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                                        oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                                        oDBDSDetail1.SetValue("U_actcode", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_actcode").Value)
                                        oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_activity").Value)
                                        oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "Pending")
                                        oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, CDate(rsetFreq.Fields.Item("U_schdt").Value).ToString("yyyyMMdd"))
                                        oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, CDate(rsetFreq.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd"))
                                    Next
                                    oMatrix1.LoadFromDataSource()
                                End If
                        End Select

                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                        'Else
                                        '    If frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        '        If oCompany.InTransaction = False Then oCompany.StartTransaction()
                                        '        BubbleEvent = False
                                        '        'If Me.TransactionManagement() = False Then
                                        '        '    If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                        '        '    BubbleEvent = False
                                        '        'End If
                                        '    End If
                                    End If
                                End If

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        BubbleEvent = False
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE

                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                            Case "tab_0"
                                If pVal.BeforeAction = False Then
                                    frmPaymentCertificate.PaneLevel = 1
                                    frmPaymentCertificate.Items.Item("tab_0").AffectsFormMode = False
                                    frmPaymentCertificate.Settings.MatrixUID = "mtx_0"
                                End If
                            Case "tab_1"
                                If pVal.BeforeAction = False Then
                                    frmPaymentCertificate.PaneLevel = 2
                                    frmPaymentCertificate.Items.Item("tab_1").AffectsFormMode = False
                                    frmPaymentCertificate.Settings.MatrixUID = "mtx_1"
                                End If
                            Case "link_mno"
                                If pVal.BeforeAction = False Then
                                    Dim txtval As String = oDBDSHeader.GetValue("U_macno", 0).Trim
                                    oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_ToolNo", txtval)
                                    '' Dim ocmb As SAPbouiCOM.ComboBox
                                    ''ocmb = frmPaymentCertificate.Items.Item("txt_type").Specific
                                    ''If ocmb.Selected.Value = "Machine" Or ocmb.Selected.Value = "Instrument" Then
                                    ''
                                    ''''ElseIf ocmb.Selected.Value = "Tool" Or ocmb.Selected.Value = "Die" Or ocmb.Selected.Value = "Fixture" Or ocmb.Selected.Value = "Mould" Then
                                    ''   Dim txtval As String = oDBDSHeader.GetValue("U_macno", 0).Trim
                                    '' oGFun.DoOpenLinkedObjectForm("TM", "TM", "t_tno", txtval)
                                    ''End
                                    ''End If
                                End If
                            Case "link_sano"
                                If pVal.BeforeAction = False Then
                                    Dim txtval As String = oDBDSHeader.GetValue("U_schactno", 0).Trim
                                    oGFun.DoOpenLinkedObjectForm("OACP", "OACP", "t_docnum", txtval)
                                End If

                            Case "link_chk"
                                If pVal.BeforeAction = False Then
                                    Dim txtval As String = oDBDSHeader.GetValue("U_pmcno", 0).Trim
                                    oGFun.DoOpenLinkedObjectForm("OPCL", "OPCL", "t_docnum", txtval)
                                End If
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Select Case pVal.ItemUID
                        Case "mtx_0"
                            Select Case pVal.ColUID
                                Case "shcactivty"
                                    If pVal.BeforeAction = False Then
                                        Dim txtval As String = oDBDSHeader.GetValue("U_schactno", 0).Trim
                                        oGFun.DoOpenLinkedObjectForm("FAST_PM_SCHACT", "FAST_PM_SCHACT", "t_docnum", txtval)
                                    End If
                            End Select
                    End Select
                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Dim strqry As String
                        Select Case pVal.ItemUID
                            Case "txt_shano"
                                If pVal.BeforeAction Then
                                    strqry = " SELECT DocNum FROM [@MIPL_PM_OACP] WHERE U_pmcno ='" & oDBDSHeader.GetValue("U_pmcno", 0).Trim & "' AND U_macno ='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "'"
                                    oGFun.ChooseFromListFilteration(frmPaymentCertificate, "CFLsano", "DocNum", strqry)
                                End If
                            Case "txt_macno"
                                If pVal.BeforeAction Then
                                    Dim ocmb As SAPbouiCOM.ComboBox = frmPaymentCertificate.Items.Item("txt_type").Specific
                                    StrQuery = "SELECT U_ToolNo    FROM [@MIPL_PM_OMAC] WHERE U_Instype  ='" & ocmb.Selected.Value & "'"
                                    oGFun.ChooseFromListFilteration(frmPaymentCertificate, "CFLmno", "U_ToolNo", StrQuery)
                                End If
                            Case "t_IndentNo"
                                strqry = "Select  A.U_IndentNo from [@INM_OPTS] A,[@INM_PTS1] B where A.DocEntry=B.DocEntry and A.U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.U_rejqty>0 and B.U_ItemCode Is Not Null And DocNum Not in (Select U_IndentNo from [@INM_ORSE]A ,[@INM_RSE1] B where A.DocEntry=B.DocEntry Group by U_IndentNo)  Union  Select A.U_IndentNo From [@INM_OPTS] A,[@INM_PTS1] B,(Select U_Indentno,Sum(U_RejQty) RejectQty From [@INM_ORSE] A ,[@INM_RSE1] B  Where A.DocEntry=B.DocEntry and U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By U_IndentNo,U_Wrkordno ) C  Where A.DocEntry=B.DocEntry And U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (U_rejqty - C.rejectQty)>0 "
                                oGFun.ChooseFromListFilteration(frmPaymentCertificate, "IndentCFL", "DocNum", strqry)

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
    Sub Tool()
        Try
            Dim rsetTool As SAPbobsCOM.Recordset
            Dim cbotype As String = oDBDSHeader.GetValue("U_mactype", 0).Trim
            Dim cboPlant As String = oDBDSHeader.GetValue("U_plant", 0).Trim
            Dim cfl As SAPbouiCOM.ChooseFromList
            Dim cons As SAPbouiCOM.Conditions
            Dim con As SAPbouiCOM.Condition
            Dim econ As New SAPbouiCOM.Conditions

            rsetTool = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            cfl = frmPaymentCertificate.ChooseFromLists.Item("CFLtno")
            cfl.SetConditions(econ)
            cons = cfl.GetConditions()
            Dim s As String = "SELECT U_plantid,U_type,U_toolno FROM [@PROD_TOOLS_HEAD] WHERE U_plantid='" & cboPlant & "'AND U_type='" & cbotype & "'"
            rsetTool.DoQuery("SELECT U_plantid,U_type,U_toolno FROM [@PROD_TOOLS_HEAD] WHERE U_plantid='" & cboPlant & "'AND U_type='" & cbotype & "'")
            rsetTool.MoveFirst()
            For i As Integer = 1 To rsetTool.RecordCount
                If i = (rsetTool.RecordCount) Then
                    con = cons.Add()
                    con.Alias = "U_toolno"
                    con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    con.CondVal = Trim(rsetTool.Fields.Item("U_toolno").Value)
                Else
                    con = cons.Add()
                    con.Alias = "U_toolno"
                    con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    con.CondVal = Trim(rsetTool.Fields.Item("U_toolno").Value)
                    con.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                End If
                rsetTool.MoveNext()
            Next
            cfl.SetConditions(cons)
            cfl.SetConditions(econ)
            cfl = frmPaymentCertificate.ChooseFromLists.Item("CFLtn")
            cfl.SetConditions(cons)

            'System.Runtime.InteropServices.Marshal.ReleaseComObject(rsetTool)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Tool Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub



    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1282"
                    Me.InitForm()
                Case "1293"
                    Select Case DeleteRowITEMUID
                        Case "mtx_0"
                            oGFun.DeleteRow(oMatrix1, oDBDSDetail1)
                        Case "mtx_1"
                            oGFun.DeleteRow(oMatrix2, oDBDSDetail2)
                    End Select
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Menu Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub LoadDetails()
        Try
            Dim rsetCondt As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim rsetScheduleActivity As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim docid As String = oDBDSHeader.GetValue("U_schactno", 0).Trim
            Dim clno As String = oDBDSHeader.GetValue("U_pmcno", 0).Trim

            rsetCondt.DoQuery("select convert(nvarchar(10),getdate(),111)")
            Dim strGetItems As String = "SELECT D.U_Schdt,D.LineId,D.U_actcode,D.U_activity,D.U_freq,D.U_nxtschdt,D.U_remarks FROM [@FAST_PM_SCHACT_A] D,[@FAST_PM_SCHACT] H WHERE H.DocNum=D.DocEntry AND H.DocNum='" & docid & "'"
            rsetScheduleActivity.DoQuery(strGetItems)
            oMatrix1.AddRow()
            oMatrix1.Clear()
            oDBDSDetail1.Clear()
            Dim j As Integer = 0
            For i As Integer = 0 To rsetScheduleActivity.RecordCount - 1
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = i
                ' oDBDSDetail.SetValue("U_schlinid", oDBDSDetail.Offset, rsetScheduleActivity.Fields.Item("LineId").Value)
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i + 1)
                oDBDSDetail1.SetValue("U_actcode", i, rsetScheduleActivity.Fields.Item("U_actcode").Value)
                oDBDSDetail1.SetValue("U_schacty", i, rsetScheduleActivity.Fields.Item("U_activity").Value)
                oDBDSDetail1.SetValue("U_status", i, "Pending")
                oDBDSDetail1.SetValue("U_freq", i, rsetScheduleActivity.Fields.Item("U_freq").Value)


                Dim schdt As String = CDate(rsetScheduleActivity.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd")
                Dim shedate As String = CDate(rsetScheduleActivity.Fields.Item("U_Schdt").Value).ToString("yyyyMMdd")
                oDBDSDetail1.SetValue("U_nxtschdt", i, schdt)
                oDBDSDetail1.SetValue("U_schedate", i, shedate)

                oDBDSDetail1.SetValue("U_actdt", i, schdt)
                oDBDSDetail1.SetValue("U_remarks", i, rsetScheduleActivity.Fields.Item("U_remarks").Value)
                If schdt <> "" Then
                    Dim schdtt As DateTime = DateTime.ParseExact(schdt, "yyyyMMdd", Nothing)
                    Dim freq As String = Trim(rsetScheduleActivity.Fields.Item("U_freq").Value)
                    If freq = "Daily" Then
                        schdtt = schdtt.AddDays(1)
                    ElseIf freq = "Weekly" Then
                        schdtt = schdtt.AddDays(7)
                    ElseIf freq = "Monthly" Then
                        schdt = schdtt.AddMonths(1)
                    ElseIf freq = "Annualy" Then
                        schdtt = schdtt.AddYears(1)
                    ElseIf freq = "Shift" Then
                        schdtt = schdtt
                    ElseIf freq = "Quarterly" Then
                        schdtt = schdtt.AddMonths(3)
                    ElseIf freq = "Half Yearly" Then
                        schdtt = schdtt.AddMonths(6)
                    End If
                    ' oDBDSDetail.SetValue("U_nxtschdt", oDBDSDetail.Offset, schdtt.ToString("yyyyMMdd"))
                    oDBDSDetail1.SetValue("U_actdt", oDBDSDetail1.Offset, schdtt.ToString("yyyyMMdd"))
                End If
                'rsetScheduleActivity.MoveNext()
                If i < rsetScheduleActivity.RecordCount - 1 Then
                    rsetScheduleActivity.MoveNext()
                End If
                j = i + 1
            Next
            'oMatrix1.AddRow()
            'oDBDSDetail1.SetValue("U_schlinid", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_actcode", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_freq", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_actdt", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_remarks", oDBDSDetail1.Offset, "")
            '' oMatrix1.SetLineData(j + 1)
            oMatrix1.LoadFromDataSource()
        Catch ex As Exception
            oApplication.StatusBar.SetText("Scheduled Activity  Choose From List Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    If BusinessObjectInfo.BeforeAction Then
                        If frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Me.ValidateAll() = False Then
                                System.Media.SystemSounds.Asterisk.Play()
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                BubbleEvent = False
                                Exit Sub
                            Else
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                        End If
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix1, "schedate", oDBDSDetail1)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix2, "itemid", oDBDSDetail2)
                    End If
                    Dim rsetUdtesch As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    Dim Activity, SchDate, Freq, Nextndt, Status, strSchedNo As String
                    strSchedNo = Trim(oDBDSHeader.GetValue("U_schactno", 0))
                    For i As Integer = 1 To oMatrix1.VisualRowCount
                        oDBDSDetail1.Offset = i - 1
                        If Trim(oMatrix1.Columns.Item("shcactivty").Cells.Item(i).Specific.Value).Equals("") = False Then
                            Activity = oDBDSDetail1.GetValue("U_schacty", oDBDSDetail1.Offset)
                            SchDate = oDBDSDetail1.GetValue("U_nxtschdt", oDBDSDetail1.Offset)
                            Freq = oDBDSDetail1.GetValue("U_freq", oDBDSDetail1.Offset)
                            Nextndt = oDBDSDetail1.GetValue("U_actdt", oDBDSDetail1.Offset)
                            Status = oDBDSDetail1.GetValue("U_status", oDBDSDetail1.Offset)
                            If Trim(Status).Equals("Completed") = True Then
                                Dim s As String = "Update [@FAST_PM_SCHACT_A]  SET U_schdt='" & Trim(SchDate) & "' , U_freq='" & Trim(Freq) & "',U_nxtschdt='" & Trim(Nextndt) & "' From [@FAST_PM_SCHACT] A,[@FAST_PM_SCHACT_A] B "
                                s += " WHERE A.DOCENTRY=B.DOCENTRY and A.DocNum='" & strSchedNo & "' and B.U_activity='" & Trim(Activity) & "'"
                                rsetUdtesch.DoQuery(s)
                            End If
                        End If
                    Next
                    ' End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                        Dim DocNum As String = oDBDSHeader.GetValue("DocNum", 0)
                        Dim rsetStatus As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        rsetStatus.DoQuery("select canceled,status from [@MIPL_PM_OACO] where DocNum='" & Trim(DocNum) & "'")
                        If (rsetStatus.Fields.Item(0).Value = "Y" And rsetStatus.Fields.Item(1).Value = "C") Then
                            oDBDSHeader.SetValue("Status", 0, "Canceled")
                            frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        ElseIf (rsetStatus.Fields.Item(0).Value = "N" And rsetStatus.Fields.Item(1).Value = "C") Then
                            oDBDSHeader.SetValue("Status", 0, "Closed")
                            frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        Else
                            oDBDSHeader.SetValue("Status", 0, "Open")
                            frmPaymentCertificate.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        End If
                    End If
                    oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemid")
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
                    DeleteRowITEMUID = EventInfo.ItemUID
                    Select Case EventInfo.ItemUID
                        Case "mtx_1"
                            If EventInfo.Row = oMatrix2.VisualRowCount Then
                                frmPaymentCertificate.EnableMenu("1293", False)
                            Else
                                frmPaymentCertificate.EnableMenu("1293", True)
                            End If
                        Case "mtx_0"
                            If EventInfo.Row = oMatrix1.VisualRowCount Then
                                frmPaymentCertificate.EnableMenu("1293", False)
                            Else
                                frmPaymentCertificate.EnableMenu("1293", True)
                            End If
                    End Select
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub
End Class