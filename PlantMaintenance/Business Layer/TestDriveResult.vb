Public Class TestDriveResult
    Dim frmTestDriveResult As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OTDR"

    Sub LoadfrmTestDriveResult()
        Try
            oGfun.LoadXML(frmTestDriveResult, TestDriveResultFormID, TestDriveResultXML)
            frmTestDriveResult = oApplication.Forms.Item(TestDriveResultFormID)
            oDBDSHeader = frmTestDriveResult.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmTestDriveResult.DataSources.DBDataSources.Item(1)
            oMatrix = frmTestDriveResult.Items.Item("Matrix").Specific

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmTestDriveResult.Freeze(True)
            oGfun.LoadComboBoxSeries(frmTestDriveResult.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmTestDriveResult.Items.Item("t_docdate").Specific)
            oGFun.LoadLocationComboBox(frmTestDriveResult.Items.Item("c_location").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            frmTestDriveResult.ActiveItem = "c_location"
            frmTestDriveResult.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmTestDriveResult.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmTestDriveResult.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmTestDriveResult.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmTestDriveResult.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmTestDriveResult.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmTestDriveResult.Items.Item("t_empname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmTestDriveResult.Items.Item("t_empid").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)



        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try

            If Trim(oDBDSHeader.GetValue("U_Location", 0)).Equals("") Then
                oApplication.StatusBar.SetText("Location should not be left empty...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf Trim(oDBDSHeader.GetValue("U_TestDate", 0)).Equals("") Then
                oApplication.StatusBar.SetText("Date should not be left empty...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Exit Function
            End If
            Dim boolEmpty As Boolean = True
            oMatrix.FlushToDataSource()
            For i As Integer = 1 To oMatrix.VisualRowCount
                If Trim(oDBDSDetail.GetValue("U_IDno", i - 1)).Equals("") = False Then boolEmpty = False

            Next
            If boolEmpty Then
                oGFun.StatusBarErrorMsg("Vehicle details should not be left empty...")
                Exit Function
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
                                Case "t_preby"
                                    oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PreBy", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "idno"
                                            If pVal.BeforeAction = False Then
                                                oMatrix.FlushToDataSource()
                                                oDBDSDetail.SetValue("U_IDno", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                                oDBDSDetail.SetValue("U_VehiNo", pVal.Row - 1, Trim(oDataTable.GetValue("U_ItemCode", 0)))
                                                oDBDSDetail.SetValue("U_VHLName", pVal.Row - 1, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                                oDBDSDetail.SetValue("U_VHLRegNo", pVal.Row - 1, Trim(oDataTable.GetValue("U_RegNo", 0)))
                                                oDBDSDetail.SetValue("U_FuelType", pVal.Row - 1, Trim(oDataTable.GetValue("U_FuelType", 0)))
                                                oMatrix.LoadFromDataSource()
                                                oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                            End If
                                        Case "drivenam"
                                            If pVal.BeforeAction = False Then
                                                oMatrix.FlushToDataSource()
                                                oDBDSDetail.SetValue("U_DriveCod", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                                oDBDSDetail.SetValue("U_DriveNam", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                                oMatrix.LoadFromDataSource()
                                                oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                            End If
                                    End Select
                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_preby"
                                oGFun.ChooseFromListFilteration(frmTestDriveResult, "PRECFL", "empID", "select empID from OHEM where dept='13' ")
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "drivenam"
                                        oGFun.ChooseFromListFilteration(frmTestDriveResult, "DVR_CFL", "empID", "select empID from OHEM where dept='13' ")
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End Try
                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmTestDriveResult.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmTestDriveResult.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmTestDriveResult.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
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
                                If pVal.BeforeAction = True And (frmTestDriveResult.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmTestDriveResult.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "idno"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, pVal.ColUID)
                                        End If
                                End Select

                        End Select
                    Catch ex As Exception
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmTestDriveResult.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
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
                    frmTestDriveResult.ActiveItem = "t_empid"
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
                    If frmTestDriveResult.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
End Class
