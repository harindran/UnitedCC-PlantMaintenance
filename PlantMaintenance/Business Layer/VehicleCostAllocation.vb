Public Class VehicleCostAllocation
    Dim frmVehicleCostAllocation As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OVCA"

    Sub LoadfrmVehicleCostAllocation()
        Try
            oGfun.LoadXML(frmVehicleCostAllocation, VehicleCostAllocationFormID, VehicleCostAllocationXML)
            frmVehicleCostAllocation = oApplication.Forms.Item(VehicleCostAllocationFormID)
            oDBDSHeader = frmVehicleCostAllocation.DataSources.DBDataSources.Item("@MIPL_PM_OVCA")
            oDBDSDetail = frmVehicleCostAllocation.DataSources.DBDataSources.Item("@MIPL_PM_VCA1")
            oMatrix = frmVehicleCostAllocation.Items.Item("Matrix").Specific

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Load Log Sheet Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmVehicleCostAllocation.Freeze(True)
            oGfun.LoadComboBoxSeries(frmVehicleCostAllocation.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmVehicleCostAllocation.Items.Item("t_docdate").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            frmVehicleCostAllocation.ActiveItem = "t_year"
            frmVehicleCostAllocation.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmVehicleCostAllocation.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmVehicleCostAllocation.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleCostAllocation.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleCostAllocation.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleCostAllocation.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If frmVehicleCostAllocation.Items.Item("t_year").Specific.Value.Equals("") Then
                oApplication.StatusBar.SetText("Year Should Not Left Empty ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            ElseIf frmVehicleCostAllocation.Items.Item("t_month").Specific.Value.Equals("") Then
                oApplication.StatusBar.SetText("Month Should Not Left Empty ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            For i As Integer = 0 To oMatrix.VisualRowCount - 1
                If oDBDSDetail.GetValue("U_VID", i).Trim = "P" Then
                    oApplication.StatusBar.SetText("Line:" & i & "Vendor ID Should Not Left Empty ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            Next

            ValidateAll = True
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
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            Select Case pVal.ItemUID
                                'Case "t_opcode"
                                '    oDBDSHeader.SetValue("U_OpCode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                '    oDBDSHeader.SetValue("U_OpName", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                'Case "t_preby"
                                '    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                '    oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "vid"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_VID", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                            oDBDSDetail.SetValue("U_VName", pVal.Row - 1, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                            oDBDSDetail.SetValue("U_RegNo", pVal.Row - 1, Trim(oDataTable.GetValue("U_RegNo", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                    End Select
                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmVehicleCostAllocation.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmVehicleCostAllocation.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmVehicleCostAllocation.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If

                            Case "t_month"
                                If pVal.BeforeAction = False And pVal.ItemChanged = True Then
                                    Dim year As String = frmVehicleCostAllocation.Items.Item("t_year").Specific.value
                                    Dim month As SAPbouiCOM.ComboBox = frmVehicleCostAllocation.Items.Item("t_month").Specific
                                    Dim str As String = "Select U_Month from [@MIPL_PM_OVCA] where U_Year ='" & year & "' "
                                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    rset.DoQuery(str)
                                    If rset.Fields.Item("U_Month").Value = month.Selected.Value Then
                                        oApplication.StatusBar.SetText("This Month Has Been Already Exist", SAPbouiCOM.BoMessageTime.bmt_Short)
                                        BubbleEvent = False
                                    End If
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
                                If pVal.BeforeAction = True And (frmVehicleCostAllocation.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmVehicleCostAllocation.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                            Case "t_mattab"
                                'If pVal.BeforeAction = False Then
                                '    oMatrix.Columns.Item("pecno").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                'End If
                            Case "Matrix"
                                Select Case pVal.ColUID

                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "vid"
                                        If pVal.BeforeAction = False Then
                                            oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSDetail.GetValue("U_VID", pVal.Row - 1))
                                        End If
                                End Select

                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Lost Focus Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "vid"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, pVal.ColUID)
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Lost Focus Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID

                            Case "Matrix"
                                Select Case pVal.ColUID
                                    'Case "date"
                                    '    Dim fromdat As String = oDBDSHeader.GetValue("U_FromDate", 0).Trim
                                    '    Dim todat As String = oDBDSHeader.GetValue("U_ToDate", 0).Trim
                                    '    Dim fdate As Date = DateTime.ParseExact(fromdat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                    '    Dim tdate As Date = DateTime.ParseExact(todat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                    '    For i As Integer = 0 To oMatrix.VisualRowCount - 1
                                    '        Dim disdat As String = oMatrix.Columns.Item("date").Cells.Item(i + 1).Specific.value
                                    '        Dim ddate As Date = DateTime.ParseExact(disdat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                    '        If ddate < fdate Or ddate > tdate Then
                                    '            oGFun.StatusBarErrorMsg("Date should come between from and todate")
                                    '            BubbleEvent = False
                                    '        End If
                                    '    Next
                                    '    If pVal.ItemChanged And pVal.BeforeAction Then
                                    '        If oGFun.isDateCompare(frmVehicleCostAllocation.Items.Item("t_fromdate").Specific, frmVehicleCostAllocation.Items.Item("t_todate").Specific, "To Date should not be less than From Date ") = False Then BubbleEvent = False
                                    '    End If
                                    'Case "starttim", "closetim"
                                    '    If pVal.BeforeAction And pVal.ItemChanged Then
                                    '        If (Trim(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value).Equals("") = False And Trim(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value).Equals("") = False) Then
                                    '            If (CDbl(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value) <> 0 And CDbl(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value) <> 0) Then
                                    '                If CDbl(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value) > CDbl(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value) Then
                                    '                    oGFun.StatusBarErrorMsg("Start time should not be greater than end time")
                                    '                    BubbleEvent = False
                                    '                End If
                                    '            End If
                                    '        End If
                                    '    End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Validate Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmVehicleCostAllocation.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Item Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

   

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281"
                    frmVehicleCostAllocation.ActiveItem = "t_empid"
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

                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Form Data Add ,Update Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        BubbleEvent = False
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess Then

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
                    If frmVehicleCostAllocation.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

End Class
