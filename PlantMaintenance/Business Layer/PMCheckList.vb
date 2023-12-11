Class PMCheckList

    Dim frmPMCheckList As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim DeleteRowITEMUID As String = ""
    Dim UDOID As String = "OPCL"
    Dim StrQuery As String = ""
    Sub LoadPMCheckList()
        Try
            oGFun.LoadXML(frmPMCheckList, PMCheckListFormID, PMCheckListXML)
            frmPMCheckList = oApplication.Forms.Item(PMCheckListFormID)
            setReport(PMCheckListFormID)
            oDBDSHeader = frmPMCheckList.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmPMCheckList.DataSources.DBDataSources.Item(1)
            oMatrix = frmPMCheckList.Items.Item("Matrix").Specific
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmPMCheckList.Freeze(True)
            frmPMCheckList.PaneLevel = 1
            If HANA Then
                If frmPMCheckList.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OPCL"""))
                oGFun.setComboBoxValue(frmPMCheckList.Items.Item("c_type").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")
            Else
                If frmPMCheckList.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OPCL]"))
                oGFun.setComboBoxValue(frmPMCheckList.Items.Item("c_type").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If

            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            frmPMCheckList.Freeze(False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmPMCheckList.Freeze(False)
        Finally
        End Try
    End Sub

    Private Sub setReport(ByVal FormUID As String)
        Try
            frmPMCheckList = oApplication.Forms.Item(FormUID)
            Dim rptTypeService As SAPbobsCOM.ReportTypesService
            'Dim newType As SAPbobsCOM.ReportType
            Dim newtypesParam As SAPbobsCOM.ReportTypesParams
            rptTypeService = oCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)
            newtypesParam = rptTypeService.GetReportTypeList
            Dim TypeCode As String
            If HANA Then
                TypeCode = oGFun.getSingleValue("Select ""CODE"" from RTYP where ""NAME""='PMCheckList'")
            Else
                TypeCode = oGFun.getSingleValue("Select CODE from RTYP where NAME='PMCheckList'")
            End If
            frmPMCheckList.ReportType = TypeCode
            'Dim i As Integer
            'For i = 0 To newtypesParam.Count - 1
            '    If newtypesParam.Item(i).TypeName = "PMCheckList" And newtypesParam.Item(i).MenuID = "PMCheckList" Then
            '        frmPMCheckList.ReportType = newtypesParam.Item(i).TypeCode
            '        Exit For
            '    End If
            'Next i
        Catch ex As Exception
            oApplication.StatusBar.SetText("setReport Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmPMCheckList.Visible = True
            frmPMCheckList.Items.Item("t_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmPMCheckList.Items.Item("txt_authby").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmPMCheckList.Items.Item("c_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            Dim objcombo As SAPbouiCOM.ComboBox
            objcombo = frmPMCheckList.Items.Item("c_freqncy").Specific
            If objcombo.Value = Nothing Then
                oGFun.StatusBarErrorMsg("Frequency Should Not Be Left Empty")
                Return False
            End If

            If frmPMCheckList.Items.Item("t_name").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Check List Name Should Not Be Left Empty")
                Return False
            ElseIf oDBDSHeader.GetValue("U_category", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Category Should Not Be Left Empty")
                Return False
            ElseIf oDBDSHeader.GetValue("U_prepby", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Prepared By Should Not Be Left Empty")
                Return False
            ElseIf oDBDSHeader.GetValue("U_authby", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Authorized By Should Not Be Left Empty")
                Return False
            ElseIf oDBDSDetail.GetValue("U_activity", 0).Equals(Trim("")) Then
                oGFun.StatusBarErrorMsg("Grid Details Should Not Be Left Empty")
                oMatrix.Columns.Item("activity").Cells.Item(oMatrix.RowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
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

            If pVal.BeforeAction = True Then
                frmPMCheckList = oApplication.Forms.Item(FormUID)
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Select Case pVal.ItemUID

                                Case "t_WrkOrdNo"
                                    'StrQuery = "Select  A.U_Wrkordno from [@INM_OPTS] A,[@INM_PTS1] B where A.DocEntry=B.DocEntry  and U_rejqty>0 and B.U_ItemCode Is Not Null And DocNum Not in (Select U_wrkOrdNo from [@INM_ORSE]A ,[@INM_RSE1] B where A.DocEntry=B.DocEntry Group by U_wrkordno) Union  Select A.U_WrkOrdNo  From [@INM_OPTS] A,[@INM_PTS1] B,(Select U_Wrkordno,Sum(U_RejQty) RejectQty From [@INM_ORSE] A ,[@INM_RSE1] B  Where A.DocEntry=B.DocEntry Group By U_Wrkordno ) C  Where A.DocEntry=B.DocEntry and (U_rejqty - C.rejectQty)>0 "
                                    'oGFun.ChooseFromListFilteration(frmPMCheckList, "WOCFL", "DocNum", StrQuery)
                                Case "txt_cat"
                                    If HANA Then
                                        StrQuery = "Select ""Code"",""Name"" from ""@MIPL_PM_OCAT"" where ""U_Type""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                    Else
                                        StrQuery = "Select Code,Name from [@MIPL_PM_OCAT] where U_Type='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                    End If

                                    oGFun.ChooseFromListFilteration(frmPMCheckList, "CFLcat", "Code", StrQuery)
                                Case "t_IndentNo"
                                    'StrQuery = "Select  A.U_IndentNo from [@INM_OPTS] A,[@INM_PTS1] B where A.DocEntry=B.DocEntry and A.U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.U_rejqty>0 and B.U_ItemCode Is Not Null And DocNum Not in (Select U_IndentNo from [@INM_ORSE]A ,[@INM_RSE1] B where A.DocEntry=B.DocEntry Group by U_IndentNo)  Union  Select A.U_IndentNo From [@INM_OPTS] A,[@INM_PTS1] B,(Select U_Indentno,Sum(U_RejQty) RejectQty From [@INM_ORSE] A ,[@INM_RSE1] B  Where A.DocEntry=B.DocEntry and U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By U_IndentNo,U_Wrkordno ) C  Where A.DocEntry=B.DocEntry And U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (U_rejqty - C.rejectQty)>0 "
                                    'oGFun.ChooseFromListFilteration(frmPMCheckList, "IndentCFL", "DocNum", StrQuery)
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "activity"
                                            If pVal.BeforeAction = False Then
                                                If HANA Then
                                                    StrQuery = "SELECT '" & Trim(oDBDSHeader.GetValue("U_CatCode", 0)).Trim & "' ""U_CatCode"" from ""@MIPL_PM_OCAT""  "
                                                Else
                                                    StrQuery = "SELECT '" & Trim(oDBDSHeader.GetValue("U_CatCode", 0)).Trim & "' U_CatCode from [@MIPL_PM_OCAT] "
                                                End If
                                                oGFun.ChooseFromListFilteration(frmPMCheckList, "CFLactvty", "U_CatCode", StrQuery)
                                            End If
                                    End Select
                            End Select

                        Catch ex As Exception
                            oApplication.StatusBar.SetText("CHOOSE_FROM_LIST Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.BeforeAction = True And (frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                End Select
            Else
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            'Dim otxt As SAPbouiCOM.EditText
                            Dim oDataTable As SAPbouiCOM.DataTable
                            Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                            oDataTable = oCFLE.SelectedObjects
                            Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                                If frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                Select Case pVal.ItemUID
                                    Case "txt_authby"
                                        oDBDSHeader.SetValue("U_authcode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_authby", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                    Case "txt_preby"
                                        oDBDSHeader.SetValue("U_precode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_prepby", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))

                                    Case "Matrix"
                                        Select Case pVal.ColUID
                                            Case "activity"
                                                For i As Integer = 1 To oMatrix.VisualRowCount
                                                    If Trim(oDBDSDetail.GetValue("U_Activity", i - 1)).Equals(Trim(oDataTable.GetValue("U_Activity", 0))) Then
                                                        oGFun.Msg(oDataTable.GetValue("U_Activity", 0) & " already Exits the Table")
                                                    End If
                                                Next
                                                oMatrix.FlushToDataSource()
                                                oDBDSDetail.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail.SetValue("U_Activity", pVal.Row - 1, oDataTable.GetValue("U_Activity", 0))
                                                oDBDSDetail.SetValue("U_actcode", pVal.Row - 1, oDataTable.GetValue("Code", 0))
                                                oMatrix.LoadFromDataSource()
                                                oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                                oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, "activity")

                                        End Select

                                    Case "txt_cat"
                                        oDBDSHeader.SetValue("U_catcode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_category", 0, Trim(oDataTable.GetValue("Name", 0)))



                                End Select
                            End If
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try

                    Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                        Try
                            Select Case pVal.ItemUID
                                Case "txt_preby"
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                        Try
                            Select Case pVal.ItemUID
                                Case "c_type"
                                    If pVal.ItemChanged Then
                                        oMatrix.Clear()
                                        oGFun.SetNewLine(oMatrix, oDBDSDetail)
                                        frmPMCheckList.Items.Item("txt_cat").Specific.value = ""
                                    End If
                                Case "c_freq"
                                    If pVal.ItemChanged Then
                                        'Me.setControlbasedOnFreq()
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.ActionSuccess And frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "link_cat"
                                    If pVal.Before_Action = False Then
                                        oGFun.DoOpenLinkedObjectForm("OCAT", "OCAT", "txt_code", Trim(oDBDSHeader.GetValue("U_catcode", 0)))
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
                                        Case "activity"
                                            If pVal.Before_Action = False Then
                                                oGFun.DoOpenLinkedObjectForm("OACT", "OACT", "txt_code", oDBDSDetail.GetValue("U_actcode", pVal.Row - 1))
                                            End If
                                    End Select
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Matrix Link Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                End Select

            End If

        Catch ex As Exception
            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.MenuUID
                    Case "1283"
                        If oApplication.MessageBox("Removal of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1281"
                        frmPMCheckList.ActiveItem = "t_code"
                        oMatrix.Item.Enabled = False
                    Case "1282"
                        Me.InitForm()
                    Case "1293"
                        oGFun.DeleteRow(oMatrix, oDBDSDetail)
                End Select
            End If
           
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
                        If frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Me.ValidateAll() = False Then
                                System.Media.SystemSounds.Asterisk.Play()
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                BubbleEvent = False
                                Exit Sub
                            Else
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            oGFun.DeleteEmptyRowInFormDataEvent(oMatrix, "activity", oDBDSDetail)

                        End If
                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, "activity")
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess Then
                        'Me.setControlbasedOnFreq()
                        oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, "activity")
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
                    If EventInfo.BeforeAction = True Then
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmPMCheckList.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmPMCheckList.EnableMenu("1283", True) 'Remove
                        End If
                        If frmPMCheckList.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If EventInfo.ItemUID = "Matrix" Then
                                If EventInfo.Row = oMatrix.VisualRowCount Then
                                    frmPMCheckList.EnableMenu("1293", False)
                                Else
                                    frmPMCheckList.EnableMenu("1293", True)
                                End If
                            Else
                                frmPMCheckList.EnableMenu("1293", False)
                            End If
                        Else
                            frmPMCheckList.EnableMenu("1293", False)
                        End If
                    Else
                        frmPMCheckList.EnableMenu("1293", False)
                        frmPMCheckList.EnableMenu("1283", False) 'Remove
                        Select Case EventInfo.ItemUID
                            Case "grd_pcl"
                                If EventInfo.Row = oMatrix.VisualRowCount Then
                                    frmPMCheckList.EnableMenu("1293", False)
                                Else
                                    frmPMCheckList.EnableMenu("1293", True)
                                End If
                        End Select
                    End If
                    
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

    Sub setControlbasedOnFreq()
        Try
            Dim freqCombo As SAPbouiCOM.ComboBox = frmPMCheckList.Items.Item("c_freq").Specific
            'oDBDSHeader.SetValue("U_Freqncy", 0, "")
            'oDBDSHeader.SetValue("U_Reading", 0, "")
            If freqCombo.Selected.Value = "N" Then
                frmPMCheckList.Items.Item("c_freqncy").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                frmPMCheckList.Items.Item("t_reading").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                frmPMCheckList.Items.Item("c_freqncy").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                frmPMCheckList.Items.Item("t_reading").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Else '
                frmPMCheckList.Items.Item("c_freqncy").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                frmPMCheckList.Items.Item("t_reading").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                frmPMCheckList.Items.Item("c_freqncy").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                frmPMCheckList.Items.Item("t_reading").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            End If
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("setControl on Freq. Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Public Sub LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean)
        Try
            frmPMCheckList = oGFun.oApplication.Forms.Item(eventInfo.FormUID)
            'eventInfo.LayoutKey = frmBreakDownSlip.Items.Item("t_docnum").Specific.string
            eventInfo.LayoutKey = frmPMCheckList.DataSources.DBDataSources.Item("@MIPL_PM_OPCL").GetValue("DocEntry", 0)
        Catch ex As Exception
        End Try
    End Sub




End Class