
Public Class SpecificationMaster
    Dim frmSpecificationMaster As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim Bool As Boolean = False
    Dim UDOID As String = "OSPM"

    Sub LoadSpecificationMaster()
        Try
            oGFun.LoadXML(frmSpecificationMaster, SpecficationMasterFormID, SpecficationMasterXML)
            frmSpecificationMaster = oApplication.Forms.Item(SpecficationMasterFormID)
            oDBDSHeader = frmSpecificationMaster.DataSources.DBDataSources.Item(0)

            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Load Specification Master Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmSpecificationMaster.Freeze(True)
            If HANA Then
                If frmSpecificationMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then oDBDSHeader.SetValue("Code", oDBDSHeader.Offset, oGFun.GetCodeGeneration("""@MIPL_PM_OSPC"""))
            Else
                If frmSpecificationMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then oDBDSHeader.SetValue("Code", oDBDSHeader.Offset, oGFun.GetCodeGeneration("[@MIPL_PM_OSPC]"))
            End If

            frmSpecificationMaster.ActiveItem = "t_specdesc"
            frmSpecificationMaster.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmSpecificationMaster.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmSpecificationMaster.Items.Item("t_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try

            'Specification No
            If frmSpecificationMaster.Items.Item("t_code").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Specification No. Should Not Be Left Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                If HANA Then
                    If oGFun.isDuplicate(frmSpecificationMaster.Items.Item("t_specdesc").Specific, """@MIPL_PM_OSPC""", """Name""", "Spec. Desc.") = False Then
                        Return False
                    End If
                Else
                    If oGFun.isDuplicate(frmSpecificationMaster.Items.Item("t_specdesc").Specific, "[@MIPL_PM_OSPC]", "Name", "Spec. Desc.") = False Then
                        Return False
                    End If
                End If
                
            End If
            'Specfication Description
            If frmSpecificationMaster.Items.Item("t_specdesc").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Specification Description Should Not Be Left Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
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

                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID

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
                                If pVal.BeforeAction = True And (frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                                If pVal.ActionSuccess And frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Me.InitForm()
                                End If

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try

                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Validate Event Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
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
            If pVal.BeforeAction Then
                Select Case pVal.MenuUID
                    Case "1283"
                        If oApplication.MessageBox("Removal of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1282"
                        Me.InitForm()
                    Case "1281"
                        frmSpecificationMaster.ActiveItem = "t_code"
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
                    If BusinessObjectInfo.BeforeAction = True Then
                        If Me.ValidateAll() = False Then
                            System.Media.SystemSounds.Asterisk.Play()
                            BubbleEvent = False
                            Exit Sub
                        End If
                        If frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If HANA Then
                                oDBDSHeader.SetValue("Code", oDBDSHeader.Offset, oGFun.GetCodeGeneration("""@MIPL_PM_OSPC"""))
                            Else
                                oDBDSHeader.SetValue("Code", oDBDSHeader.Offset, oGFun.GetCodeGeneration("[@MIPL_PM_OSPC]"))
                            End If

                        End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess = True Then
                        oApplication.Menus.Item("1283").Enabled = False
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
                    If EventInfo.BeforeAction = True Then
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmSpecificationMaster.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmSpecificationMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmSpecificationMaster.EnableMenu("1283", True) 'Remove
                        End If
                    Else
                        frmSpecificationMaster.EnableMenu("1283", False)
                    End If
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

End Class
