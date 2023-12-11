Class TripMaster
    Dim frmTripMaster As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim UDOID As String = "OTRP"

    Sub LoadTripMaster()
        Try
            oGFun.LoadXML(frmTripMaster, TripMasterFormID, TripMasterXML)
            frmTripMaster = oApplication.Forms.Item(TripMasterFormID)
            oDBDSHeader = frmTripMaster.DataSources.DBDataSources.Item(0)
            'Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception
            oGFun.Msg("Load Activity Master Failed")
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OTRP]"))
            frmTripMaster.ActiveItem = "txt_name"
            'oGFun.setComboBoxValue(frmTripMaster.Items.Item("cmb_dept").Specific, "Select Code, Name from OUDP") 'Load Department)
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmTripMaster.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmTripMaster.Items.Item("cmb_cat").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmTripMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmTripMaster.Items.Item("cmb_dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmTripMaster.Items.Item("txt_actvty").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmTripMaster.Items.Item("txt_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If Trim(oDBDSHeader.GetValue("Code", 0)).Equals("") Then
                oGFun.StatusBarErrorMsg("Code should not be left empty...")
                Return False
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
                            If frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            Select Case pVal.ItemUID

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
                                If pVal.BeforeAction = True And (frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                                If pVal.ActionSuccess And frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
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
            Select Case pVal.MenuUID
                Case "1282"
                    If pVal.BeforeAction = False Then
                        Me.InitForm()
                    End If
            End Select
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
                        If frmTripMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OTRP]"))
                        End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.BeforeAction = False Then

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
                    If frmTripMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction Then

                    End If
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub



End Class