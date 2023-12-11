Class LicenseDetails

    Dim frmLicenseDetails As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oDBDSDetail, oDBDSDetail1 As SAPbouiCOM.DBDataSource
    Dim oMatrix, oMatrix1 As SAPbouiCOM.Matrix
    Dim DeleteRowITEMUID As String = ""
    Dim UDOID As String = "OLAC"

    Sub LoadLicenseDetails()
        Try
            oGFun.LoadXML(frmLicenseDetails, LicenseDetailsFormID, LicenseDetailsXML)
            frmLicenseDetails = oApplication.Forms.Item(LicenseDetailsFormID)
            oDBDSHeader = frmLicenseDetails.DataSources.DBDataSources.Item("@MIPL_PM_OLAC")
            oDBDSDetail = frmLicenseDetails.DataSources.DBDataSources.Item("@MIPL_PM_LAC1")
            'oDBDSDetail1 = frmLicenseDetails.DataSources.DBDataSources.Item("@MIPL_PM_CALD2")
            oMatrix = frmLicenseDetails.Items.Item("Matrix1").Specific
            'oMatrix1 = frmLicenseDetails.Items.Item("Matrix2").Specific
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmLicenseDetails.Freeze(True)
            frmLicenseDetails.PaneLevel = 1
            oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OLAC]"))
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            ' oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
            frmLicenseDetails.ActiveItem = "t_empid"
            frmLicenseDetails.PaneLevel = 1
            frmLicenseDetails.Freeze(False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmLicenseDetails.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmLicenseDetails.Visible = True
            'frmLicenseDetails.Items.Item("txt_authby").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmLicenseDetails.Items.Item("c_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try


            If frmLicenseDetails.Items.Item("t_empid").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Employee Name Should Not Be Left Empty")
                Return False
            End If

            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Validate All Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            ValidateAll = False
        Finally
        End Try

    End Function

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.EventType
                Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                    Try
                        'Dim otxt As SAPbouiCOM.EditText
                        Dim oDataTable As SAPbouiCOM.DataTable
                        Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                        oDataTable = oCFLE.SelectedObjects
                        Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            Select Case pVal.ItemUID
                                Case "t_empid"
                                    oDBDSHeader.SetValue("U_EmpID", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_EmpName", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try

                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix1"
                                Select Case pVal.ColUID
                                    Case "type"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, pVal.Row, pVal.ColUID)
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
                            
                        End Select


                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmLicenseDetails.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmLicenseDetails.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        BubbleEvent = False
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmLicenseDetails.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
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
                            Case "Matrix"
                                Select Case pVal.ColUID
                                 
                                End Select
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Matrix Link Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "t_empcode"
                                If pVal.BeforeAction = False Then
                                    oGFun.ChooseFromListFilteration(frmLicenseDetails, "EMPCFL", "empID", "select empID from OHEM where dept='13' ")
                                End If
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
                    frmLicenseDetails.ActiveItem = "t_docnum"
                Case "1282"
                    Me.InitForm()
                Case "1293"
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
                        If frmLicenseDetails.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmLicenseDetails.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Me.ValidateAll() = False Then
                                System.Media.SystemSounds.Asterisk.Play()
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                BubbleEvent = False
                                Exit Sub
                            Else
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            oGFun.DeleteEmptyRowInFormDataEvent(oMatrix, "lcode", oDBDSDetail)
                            'oGFun.DeleteEmptyRowInFormDataEvent(oMatrix1, "cccode", oDBDSDetail1)

                        End If
                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, "lcode")
                        'oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, "cccode")
                    End If
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
                    DeleteRowITEMUID = EventInfo.ItemUID
                    Select Case EventInfo.ItemUID
                        Case "grd_pcl"
                            If EventInfo.Row = oMatrix.VisualRowCount Then
                                frmLicenseDetails.EnableMenu("1293", False)
                            Else
                                frmLicenseDetails.EnableMenu("1293", True)
                            End If
                    End Select
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

End Class
