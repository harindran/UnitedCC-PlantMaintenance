Class CategoryMaster
    Dim frmCategoryMaster As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim UDOID As String = "MICAT"

    Sub LoadCategoryMaster()
        Try
            oGFun.LoadXML(frmCategoryMaster, CategoryMasterFormID, CategoryMasterXML)
            frmCategoryMaster = oApplication.Forms.Item(CategoryMasterFormID)
            oDBDSHeader = frmCategoryMaster.DataSources.DBDataSources.Item(0)

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception
            oGFun.Msg("Load Activity Master Failed" & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            If HANA Then
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OCAT"""))
                oGFun.setComboBoxValue(frmCategoryMaster.Items.Item("cmb_type").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")
            Else
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OCAT]"))
                oGFun.setComboBoxValue(frmCategoryMaster.Items.Item("cmb_type").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If

            frmCategoryMaster.ActiveItem = "cmb_type"
            'oGFun.setComboBoxValue(frmCategoryMaster.Items.Item("cmb_dept").Specific, "Select Code, Name from OUDP") 'Load Department)
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmCategoryMaster.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmCategoryMaster.Items.Item("txt_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmCategoryMaster.Items.Item("cmb_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmCategoryMaster.Items.Item("txt_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmCategoryMaster.Items.Item("cmb_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmCategoryMaster.Items.Item("txt_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If oDBDSHeader.GetValue("U_Type", 0).Trim = "" Then
                oApplication.StatusBar.SetText("Type Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If frmCategoryMaster.Items.Item("txt_cat").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Category Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If frmCategoryMaster.Items.Item("txt_cat").Specific.Value.Trim <> "" Then
                Dim type As String = oDBDSHeader.GetValue("U_type", 0).Trim.ToUpper
                Dim cat As String = Trim(frmCategoryMaster.Items.Item("txt_cat").Specific.value).ToUpper
                '.oDBDSHeader.GetValue("U_category", 0).Trim.ToUpper()
                Dim StrQry As String
                Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                If frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                    If HANA Then
                        StrQry = "SELECT * FROM ""@MIPL_PM_OCAT"" where ""U_Type""='" & type & "'  and UPPER(""Name"") ='" & cat & "' "
                    Else
                        StrQry = "SELECT * FROM [@MIPL_PM_OCAT] where U_Type='" & type & "'  and UPPER(Name) ='" & cat & "' "
                    End If

                    rset.DoQuery(StrQry)
                    If rset.RecordCount <> 0 Then
                        oGFun.Msg("Category Already Exists")
                        Return False
                    End If
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
                                If pVal.BeforeAction = True And (frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                                If pVal.ActionSuccess And frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
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
                        If frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If HANA Then
                                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OCAT"""))
                            Else
                                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OCAT]"))
                            End If

                        End If
                    End If

                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    frmCategoryMaster.DefButton = ""
                    If BusinessObjectInfo.BeforeAction = False Then
                        'Dim rsetMachine, rsetTool, rsetChecklist As SAPbobsCOM.Recordset
                        'rsetMachine = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        'rsetTool = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        'rsetChecklist = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        'Dim s As String = "select count(*) from [@PROD_MACHINE_HEAD] where u_maccat='" & Trim(oDBDSHeader.GetValue("u_category", 0)) & "'"
                        'rsetMachine.DoQuery("select count(*) from [@PROD_MACHINE_HEAD] where u_maccat='" & Trim(oDBDSHeader.GetValue("u_category", 0)) & "'")
                        'rsetTool.DoQuery("select count(*) from [@prod_tools_head] where U_toolcat='" & Trim(oDBDSHeader.GetValue("u_category", 0)) & "'")
                        'rsetChecklist.DoQuery("select count(*) from [@FAST_PM_PCL] where U_category='" & Trim(oDBDSHeader.GetValue("u_category", 0)) & "'")
                        'If rsetMachine.Fields.Item(0).Value > 0 Or rsetTool.Fields.Item(0).Value > 0 Or rsetChecklist.Fields.Item(0).Value > 0 Then
                        '    frmCategoryMaster.Items.Item("txt_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                        'Else
                        '    frmCategoryMaster.Items.Item("txt_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        'End If

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
                                frmCategoryMaster.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmCategoryMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmCategoryMaster.EnableMenu("1283", True) 'Remove
                        End If
                    Else
                    frmCategoryMaster.EnableMenu("1283", False)
                    End If

            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub



End Class