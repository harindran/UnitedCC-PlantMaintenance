Public Class MoDemobilization
    Dim frmMoDemobilization As SAPbouiCOM.Form
    Dim oDBDSHeader, oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim UDOID As String = "OMOB"

    Sub LoadMoDemobilization()
        Try
            oGFun.LoadXML(frmMoDemobilization, MoDemobilizationFormID, MoDemobilizationXML)
            frmMoDemobilization = oApplication.Forms.Item(MoDemobilizationFormID)

            oDBDSHeader = frmMoDemobilization.DataSources.DBDataSources.Item("@MIPL_PM_OMOB")
            oDBDSDetail = frmMoDemobilization.DataSources.DBDataSources.Item("@MIPL_PM_MOB1")

            oMatrix = frmMoDemobilization.Items.Item("matrix").Specific

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Load MoDemobiliztion Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmMoDemobilization.Freeze(True)
            frmMoDemobilization.PaneLevel = 1
            oGFun.LoadComboBoxSeries(frmMoDemobilization.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmMoDemobilization.Items.Item("t_docdate").Specific) ' Load Document Date
            oGFun.LoadLocationComboBox(frmMoDemobilization.Items.Item("c_location").Specific)

            frmMoDemobilization.ActiveItem = "c_location"
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            oGFun.LoadLocationComboBox(oMatrix.Columns.Item("toloc").Cells.Item(1).Specific)
            If frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                oMatrix.Columns.Item("toloc").Editable = True
            Else
                oMatrix.Columns.Item("toloc").Editable = False
            End If
            frmMoDemobilization.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmMoDemobilization.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmMoDemobilization.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmMoDemobilization.Items.Item("c_freq").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            frmMoDemobilization.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmMoDemobilization.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMoDemobilization.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMoDemobilization.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If oDBDSHeader.GetValue("U_Location", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Location should not be left empty...")
                Return False
            ElseIf oDBDSHeader.GetValue("U_Freq", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Frequency should not be left empty...")
                Return False
            ElseIf oDBDSHeader.GetValue("U_PreByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Prepared By should not be left empty...")
                Return False
            ElseIf oDBDSHeader.GetValue("U_AppByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Approved By should not be left empty...")
                Return False
            End If

            oMatrix.FlushToDataSource()
            Dim iRowCout As Integer = 1
            If oMatrix.VisualRowCount > 1 Then iRowCout = oMatrix.VisualRowCount - 1

            If oMatrix.VisualRowCount = 0 Then
                oGFun.StatusBarErrorMsg("Vehicle details should not be left empty...")
                Return False
            End If

            For i As Integer = 1 To iRowCout
                If Trim(oDBDSDetail.GetValue("U_VHLID", i - 1)).Equals("") Then
                    oGFun.StatusBarErrorMsg("Line [" & i & "] Vehicle details shoudl not be left empty...")
                    Exit Function
                End If
            Next

            If oDBDSHeader.GetValue("U_PreByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Prepared by should not be left empty...")
                Return False
            ElseIf oDBDSHeader.GetValue("U_AppByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Approved by should not be left empty...")
                Return False
            End If


            If frmMoDemobilization.Items.Item("t_prebynam").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Prepared By should not be left empty...")
                frmMoDemobilization.Items.Item("t_prebynam").Click()
                Return False
            ElseIf frmMoDemobilization.Items.Item("t_appbynam").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Accepted By should not be left empty...")
                frmMoDemobilization.Items.Item("t_appbynam").Click()
                Return False
            End If


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
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            Select Case pVal.ItemUID
                                Case "t_appbynam"
                                    oDBDSHeader.SetValue("U_AppByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_AppByNam", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                Case "t_prebynam"
                                    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                Case "t_drivrnam"
                                    oDBDSHeader.SetValue("U_DriverCode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_DriverName", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                Case "matrix"
                                    Select Case pVal.ColUID
                                        Case "vhlid"
                                            Try
                                                oMatrix.FlushToDataSource()
                                                oDBDSDetail.SetValue("U_VHLID", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                                oDBDSDetail.SetValue("U_VHLName", pVal.Row - 1, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                                oMatrix.LoadFromDataSource()
                                                oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            Catch ex As Exception

                                            End Try
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
                            Case "matrix"
                                Select Case pVal.ColUID
                                    Case "vhlid"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, pVal.ColUID)
                                        End If
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
                                If frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmMoDemobilization.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmMoDemobilization.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                    oDBDSHeader.SetValue("DocNum", 0, strDocNum)
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
                                If pVal.BeforeAction = True And (frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "matrix"
                                Select Case pVal.ColUID
                                    Case "activity"
                                        If pVal.BeforeAction = False Then
                                            oGFun.DoOpenLinkedObjectForm("OACT", "OACT", "txt_actvty", oMatrix.Columns.Item("activity").Cells.Item(pVal.Row).Specific.value)
                                        End If
                                    Case "vhlid"
                                        If pVal.BeforeAction = False Then
                                            If Trim(oDBDSDetail.GetValue("U_Type", pVal.Row - 1)).Equals("V") Then
                                                oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.value)
                                            Else
                                                oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.value)
                                            End If
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oGFun.StatusBarErrorMsg("Matrix Link Pressed Event Failed:" & ex.Message)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID

                            Case "matrix"
                                Select Case pVal.ColUID
                                    Case "releasdt"
                                        If pVal.BeforeAction = True Then
                                            Dim docdat As String = oDBDSHeader.GetValue("U_DocDate", 0).Trim
                                            'Dim ddate As Date = DateTime.ParseExact(docdat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                            For i As Integer = 0 To oMatrix.VisualRowCount - 2
                                                Dim reldat As String = oMatrix.Columns.Item("releasdt").Cells.Item(i + 1).Specific.value
                                                'Dim rdate As Date = DateTime.ParseExact(reldat, "yyyyMMdd", Nothing).ToString("MM\/dd\/yyyy")
                                                If docdat > reldat Or docdat = reldat Then
                                                    oApplication.StatusBar.SetText("Line :" & i + 1 & " Release Should Not Be Less Than Document Date", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                    BubbleEvent = False
                                                End If
                                            Next
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Validate Event vailed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_appbynam"
                                'oGFun.ChooseFromListFilteration(frmMoDemobilization, "CFLauthby", "empID", "select empID from OHEM where dept='13' ")
                            Case "t_prebynam"
                                'oGFun.ChooseFromListFilteration(frmMoDemobilization, "CFLpreby", "empID", "select empID from OHEM where dept='13' ")
                            Case "matrix"
                                Select Case pVal.ColUID
                                    Case "vhlid"
                                        If pVal.BeforeAction = False And frmMoDemobilization Is Nothing = False Then
                                            oMatrix.FlushToDataSource()
                                            If Trim(oMatrix.Columns.Item("type").Cells.Item(pVal.Row).Specific.value).Equals("V") Then
                                                oMatrix.Columns.Item(pVal.ColUID).ChooseFromListUID = "VHL_CFL"
                                                oMatrix.Columns.Item(pVal.ColUID).ChooseFromListAlias = "Code"
                                                oGFun.ChooseFromListFilteration(frmMoDemobilization, "VHL_CFL", "U_Location", "SELECT '" & Trim(oDBDSHeader.GetValue("U_Location", 0)) & "' U_Location")
                                            Else
                                                oMatrix.Columns.Item(pVal.ColUID).ChooseFromListUID = "MAC_CFL"
                                                oMatrix.Columns.Item(pVal.ColUID).ChooseFromListAlias = "Code"
                                                oGFun.ChooseFromListFilteration(frmMoDemobilization, "MAC_CFL", "U_Location", "SELECT '" & Trim(oDBDSHeader.GetValue("U_Location", 0)) & "' U_Location")
                                            End If
                                        End If
                                End Select
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

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281"
                    frmMoDemobilization.ActiveItem = "t_docnum"
                Case "1282"
                    Me.InitForm()
                Case "matrix"
                    oGFun.DeleteRow(oMatrix, oDBDSDetail)
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
                    If BusinessObjectInfo.BeforeAction Then
                        If Me.ValidateAll() = False Then
                            System.Media.SystemSounds.Asterisk.Play()
                            BubbleEvent = False
                            Exit Sub
                        End If
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix, oMatrix.Columns.Item(2).UniqueID, oDBDSDetail)
                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, oMatrix.Columns.Item(2).UniqueID)
                       
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess Then
                        oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, oMatrix.Columns.Item(2).UniqueID)
                    End If
                    If frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmMoDemobilization.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                        oMatrix.Columns.Item("toloc").Editable = False
                    Else
                        oMatrix.Columns.Item("toloc").Editable = True
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
                    Select Case EventInfo.ItemUID
                        Case "matrix"
                            If EventInfo.Row = oMatrix.VisualRowCount Then
                                frmMoDemobilization.EnableMenu("1293", False)
                            Else
                                frmMoDemobilization.EnableMenu("1293", True)
                            End If
                    End Select
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

End Class
