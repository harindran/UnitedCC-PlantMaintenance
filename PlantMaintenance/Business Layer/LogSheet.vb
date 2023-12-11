Public Class LogSheet
    Dim frmLogSheet As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OLOG"
    Dim StrQuery As String = ""
    Sub LoadfrmLogSheet()
        Try
            oGfun.LoadXML(frmLogSheet, LogSheetFormID, LogSheetXML)
            frmLogSheet = oApplication.Forms.Item(LogSheetFormID)
            oDBDSHeader = frmLogSheet.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmLogSheet.DataSources.DBDataSources.Item(1)
            oMatrix = frmLogSheet.Items.Item("Matrix").Specific

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Load Log Sheet Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmLogSheet.Freeze(True)
            oGfun.LoadComboBoxSeries(frmLogSheet.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmLogSheet.Items.Item("t_docdate").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            oGFun.LoadLocationComboBox(oMatrix.Columns.Item("location").Cells.Item(1).Specific)
            frmLogSheet.ActiveItem = "t_opcode"
            frmLogSheet.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmLogSheet.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmLogSheet.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmLogSheet.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmLogSheet.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmLogSheet.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmLogSheet.Items.Item("c_worktype").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmLogSheet.Items.Item("t_opcode").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If frmLogSheet.Items.Item("t_prebycd").Specific.Value.Equals("") Then
                oApplication.StatusBar.SetText("Prepared By Should Not Left Empty ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            ElseIf frmLogSheet.Items.Item("t_appbycd").Specific.Value.Equals("") Then
                oApplication.StatusBar.SetText("Approved By Should Not Left Empty ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If



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
                                Case "t_opcode"
                                    oDBDSHeader.SetValue("U_OpCode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_OpName", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_preby"
                                    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_appby"
                                    oDBDSHeader.SetValue("U_AppByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_AppByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_cusname"
                                    oDBDSHeader.SetValue("U_CardCode", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                    oDBDSHeader.SetValue("U_CardName", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "pecno"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_IDNo", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                            If Trim(oDBDSDetail.GetValue("U_Type", pVal.Row - 1)).Equals("VH") Then
                                                oDBDSDetail.SetValue("U_VHLNo", pVal.Row - 1, Trim(oDataTable.GetValue("U_RegNo", 0)))
                                                oDBDSDetail.SetValue("U_VHLCateg", pVal.Row - 1, Trim(oDataTable.GetValue("U_VechType", 0)))
                                            End If
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        Case "tripcode"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_TripCode", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                            oDBDSDetail.SetValue("U_TripName", pVal.Row - 1, Trim(oDataTable.GetValue("U_TName", 0)))

                                            oDBDSDetail.SetValue("U_TripCost", pVal.Row - 1, Trim(oDataTable.GetValue("U_TCharge", 0)))
                                            'oDBDSDetail.SetValue("U_TType", pVal.Row - 1, Trim(oDataTable.GetValue("U_TType", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        Case "prjcode"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_Prjcode", pVal.Row - 1, Trim(oDataTable.GetValue("PrjCode", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
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
                                If frmLogSheet.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmLogSheet.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmLogSheet.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "triploc"
                                        If pVal.BeforeAction = False Then
                                            Dim fromLoc As SAPbouiCOM.ComboBox = oMatrix.Columns.Item("location").Cells.Item(pVal.Row).Specific
                                            Dim toLoc As SAPbouiCOM.ComboBox = oMatrix.Columns.Item("triploc").Cells.Item(pVal.Row).Specific
                                            If toLoc.Selected.Value = fromLoc.Selected.Value Then
                                                oApplication.StatusBar.SetText("Trip Location should not be equal From Location", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            End If
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
                                If pVal.BeforeAction = True And (frmLogSheet.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmLogSheet.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                                If pVal.BeforeAction = False Then
                                    oMatrix.Columns.Item("pecno").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                End If
                            Case "t_opcode"
                                If pVal.BeforeAction = False Then
                                    oGFun.ChooseFromListFilteration(frmLogSheet, "OPRCFL", "empID", "select empID from OHEM where dept='13' ")
                                End If
                            Case "t_preby"
                                If pVal.BeforeAction = False Then
                                    oGFun.ChooseFromListFilteration(frmLogSheet, "PRECFL", "empID", "select empID from OHEM where dept='13' ")
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "pecno"
                                        If pVal.BeforeAction = False Then
                                            oMatrix.FlushToDataSource()
                                            Dim oTxt As SAPbouiCOM.Column = oMatrix.Columns.Item(pVal.ColUID)
                                            If Trim(oDBDSDetail.GetValue("U_Type", pVal.Row - 1)).Equals("VH") Then
                                                StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] "
                                                oTxt.ChooseFromListUID = "OVHL_CFL"
                                                oTxt.ChooseFromListAlias = "U_ItemCode"
                                                oGFun.ChooseFromListFilteration(frmLogSheet, "OVHL_CFL", "U_ItemCode", StrQuery)
                                            Else
                                                StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSDetail.GetValue("U_Type", pVal.Row - 1).Trim & "'"
                                                oTxt.ChooseFromListUID = "OMAC_CFL"
                                                oTxt.ChooseFromListAlias = "Code"
                                                oGFun.ChooseFromListFilteration(frmLogSheet, "OMAC_CFL", "Code", StrQuery)
                                            End If
                                        End If
                                    Case "tripcode"
                                        If pVal.BeforeAction = False Then
                                            oGFun.ChooseFromListFilteration(frmLogSheet, "TRIPCFL", "Code", "select Code from [@MIPL_PM_OTRP] where U_TType='" & oMatrix.Columns.Item("triptype").Cells.Item(pVal.Row).Specific.value & "' ")
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "type"
                                        If pVal.BeforeAction = False Then

                                        End If
                                    Case "pecno"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, pVal.ColUID)
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Lost Focus Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "tripcode"
                                        If pVal.BeforeAction = False Then
                                            oGFun.DoOpenLinkedObjectForm("OTRP", "OTRP", "txt_code", oMatrix.Columns.Item("tripcode").Cells.Item(pVal.Row).Specific.value)
                                        End If
                                        'Case "prjcode"
                                        '    If pVal.BeforeAction = False Then
                                        '        oGFun.DoOpenLinkedObjectForm("63", "63", "3", oMatrix.Columns.Item("tripcode").Cells.Item(pVal.Row).Specific.value)
                                        '    End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oGFun.StatusBarErrorMsg("Matrix Link Pressed Event Failed:" & ex.Message)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID
                            Case "t_fromdate", "t_todate"
                                If pVal.ItemChanged And pVal.BeforeAction Then
                                    If oGFun.isDateCompare(frmLogSheet.Items.Item("t_fromdate").Specific, frmLogSheet.Items.Item("t_todate").Specific, "To Date should not be less than From Date ") = False Then BubbleEvent = False
                                End If
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "date"
                                        Dim fromdat As String = oDBDSHeader.GetValue("U_FromDate", 0).Trim
                                        Dim todat As String = oDBDSHeader.GetValue("U_ToDate", 0).Trim
                                        If fromdat <> "" And todat <> "" Then
                                            Dim fdate As Date = DateTime.ParseExact(fromdat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                            Dim tdate As Date = DateTime.ParseExact(todat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                            For i As Integer = 0 To oMatrix.VisualRowCount - 1
                                                Dim disdat As String = oMatrix.Columns.Item("date").Cells.Item(i + 1).Specific.value
                                                Dim ddate As Date = DateTime.ParseExact(disdat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                                If ddate < fdate Or ddate > tdate Then
                                                    oGFun.StatusBarErrorMsg("Date should come between from and todate")
                                                    BubbleEvent = False
                                                End If
                                            Next
                                        Else
                                            oApplication.StatusBar.SetText("Should Give The From And To Date", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        End If
                                        If pVal.ItemChanged And pVal.BeforeAction Then
                                            If oGFun.isDateCompare(frmLogSheet.Items.Item("t_fromdate").Specific, frmLogSheet.Items.Item("t_todate").Specific, "To Date should not be less than From Date ") = False Then BubbleEvent = False
                                        End If
                                    Case "starttim", "closetim"
                                        If pVal.BeforeAction And pVal.ItemChanged Then
                                            If (Trim(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value).Equals("") = False And Trim(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value).Equals("") = False) Then
                                                If (CDbl(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value) <> 0 And CDbl(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value) <> 0) Then
                                                    If CDbl(oMatrix.Columns.Item("starttim").Cells.Item(pVal.Row).Specific.Value) > CDbl(oMatrix.Columns.Item("closetim").Cells.Item(pVal.Row).Specific.Value) Then
                                                        oGFun.StatusBarErrorMsg("Start time should not be greater than end time")
                                                        BubbleEvent = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Validate Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmLogSheet.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
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

    Sub LoadPECnoCFL(ByVal objtype As String)
        Try
            Dim ocfls As SAPbouiCOM.ChooseFromListCollection
            ocfls = frmLogSheet.ChooseFromLists
            Dim ocfl As SAPbouiCOM.ChooseFromList
            Dim cflcrepa As SAPbouiCOM.ChooseFromListCreationParams
            cflcrepa = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)
            cflcrepa.MultiSelection = False
            cflcrepa.ObjectType = objtype
            cflcrepa.UniqueID = "PEC_CFL"
            ocfl = ocfls.Add(cflcrepa)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Load VHL CFL Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281"
                    frmLogSheet.ActiveItem = "t_empid"
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
                    If frmLogSheet.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
    
End Class
