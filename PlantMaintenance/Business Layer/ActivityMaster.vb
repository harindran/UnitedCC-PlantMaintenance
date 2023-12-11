Class ActivityMaster
    Dim frmActivityMaster As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim UDOID As String = "OACT"
    Dim SQuery As String = ""
    Sub LoadActivityMaster()
        Try
            oGFun.LoadXML(frmActivityMaster, ActivityMasterFormID, ActivityMasterXML)
            frmActivityMaster = oApplication.Forms.Item(ActivityMasterFormID)
            oDBDSHeader = frmActivityMaster.DataSources.DBDataSources.Item(0)
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception
            oGFun.Msg("Load Activity Master Failed")
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            If HANA Then
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OACT"""))
            Else
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OACT]"))
            End If

            frmActivityMaster.ActiveItem = "cmb_cat"
            'oGFun.setComboBoxValue(frmActivityMaster.Items.Item("cmb_dept").Specific, "Select Code, Name from OUDP") 'Load Department)
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmActivityMaster.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmActivityMaster.Items.Item("cmb_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActivityMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmActivityMaster.Items.Item("cmb_dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityMaster.Items.Item("txt_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If Trim(oDBDSHeader.GetValue("U_CatCode", 0)).Equals("") Then
                oGFun.StatusBarErrorMsg("Category should not be left empty...")
                Return False
            End If

            If frmActivityMaster.Items.Item("txt_actvty").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Activity should not be left empty...")
                frmActivityMaster.Items.Item("txt_actvty").Click()
                Return False
            End If
            If frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                Dim act As String = Trim(frmActivityMaster.Items.Item("txt_actvty").Specific.value).ToUpper
                If HANA Then
                    SQuery = "SELECT * FROM ""@MIPL_PM_OACT"" WHERE ""U_CatCode"" ='" & Trim(oDBDSHeader.GetValue("U_CatCode", 0)) & "' AND UPPER(""U_Activity"") ='" & act.ToUpper & "'"  'Trim(oDBDSHeader.GetValue("U_Activity", 0))
                Else
                    SQuery = "SELECT * FROM [@MIPL_PM_OACT] WHERE U_CatCode ='" & Trim(oDBDSHeader.GetValue("U_CatCode", 0)) & "' AND U_Activity ='" & act.ToUpper & "'"
                End If

                Dim rsetDuplicate As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                If rsetDuplicate.RecordCount > 0 Then
                    oGFun.Msg("Activity is already exits the same category...")
                    Return False
                End If
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
                        Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                            If frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            Select Case pVal.ItemUID
                                Case "cmb_cat"
                                    oDBDSHeader.SetValue("U_CatCode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                    oDBDSHeader.SetValue("U_CatName", 0, Trim(oDataTable.GetValue("Name", 0)))
                            End Select
                        End If
                    Catch ex As Exception
                        oGFun.Msg("Choose From List Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID

                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Lost Focus Event Failed:")
                    Finally
                    End Try
                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Combo Select Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
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
                                If pVal.ActionSuccess And frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If
                            Case "lk_cat"
                                If pVal.Before_Action = False Then
                                    Dim str As String = frmActivityMaster.Items.Item("t_catcode").Specific.value
                                    oApplication.ActivateMenuItem("OCAT")
                                    Dim oForm As SAPbouiCOM.Form
                                    oForm = oApplication.Forms.Item("OCAT")
                                    oForm.Select()
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                    Dim txt As SAPbouiCOM.EditText = oForm.Items.Item("txt_code").Specific
                                    txt.Value = str
                                    oForm.Items.Item("1").Click()
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
            If pVal.BeforeAction Then
                Select Case pVal.MenuUID
                    Case "1283"
                        If oApplication.MessageBox("Removal of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1282"
                        If pVal.BeforeAction = False Then
                            Me.InitForm()
                        End If
                End Select
            End If
           
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
                        End If
                        If frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If HANA Then
                                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OACT"""))
                            Else
                                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OACT]"))
                            End If

                        End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.BeforeAction = False Then
                        Dim sActCode As String = Trim(oDBDSHeader.GetValue("Code", 0))
                        If HANA Then
                            SQuery = "SELECT ""U_actcode"" FROM ""@MIPL_PM_PCL1""  WHERE ""U_actcode"" IS NOT NULL AND ""U_actcode"" = '" & sActCode & "'   UNION SELECT ""U_ActCode"" FROM ""@MIPL_PM_ACP1""  WHERE ""U_ActCode"" IS NOT NULL AND  ""U_ActCode"" =  '" & sActCode & "' " _
                       & " UNION SELECT ""U_ActCode"" FROM ""@MIPL_PM_ACO1""  WHERE ""U_ActCode"" IS NOT NULL AND  ""U_ActCode"" =  '" & sActCode & "' UNION SELECT ""U_ActCode"" FROM ""@MIPL_PM_JOC1""  WHERE ""U_ActCode"" IS NOT NULL AND  ""U_ActCode"" =  '" & sActCode & "' "
                        Else
                            SQuery = "SELECT U_actcode FROM [@MIPL_PM_PCL1]  WHERE U_ActCode IS NOT NULL AND U_ActCode = '" & sActCode & "'   UNION SELECT U_ActCode FROM [@MIPL_PM_ACP1]  WHERE U_ActCode IS NOT NULL AND  U_ActCode =  '" & sActCode & "' " _
                       & " UNION SELECT U_ActCode FROM [@MIPL_PM_ACO1]  WHERE U_ActCode IS NOT NULL AND  U_ActCode =  '" & sActCode & "' UNION SELECT U_ActCode FROM [@MIPL_PM_JOC1]  WHERE U_ActCode IS NOT NULL AND  U_ActCode =  '" & sActCode & "' "
                        End If
                       
                        Dim rsetRemove As SAPbobsCOM.Recordset = oGFun.DoQuery(sQuery)

                        If rsetRemove.RecordCount > 0 Then
                            frmActivityMaster.EnableMenu("1283", False)
                            frmActivityMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                        Else
                            frmActivityMaster.EnableMenu("1283", True)
                            frmActivityMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        End If

                    End If
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
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmActivityMaster.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActivityMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmActivityMaster.EnableMenu("1283", True) 'Remove
                        End If
                    Else
                        frmActivityMaster.EnableMenu("1283", False) 'Remove
                    End If
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub



End Class