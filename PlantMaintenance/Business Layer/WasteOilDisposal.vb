Public Class WasteOilDisposal
    Dim frmWasteOilDisposal As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "OWOD"

    Sub LoadfrmWasteOilDisposal()
        Try
            oGfun.LoadXML(frmWasteOilDisposal, WasteOilDisposalFormID, WasteOilDisposalXML)
            frmWasteOilDisposal = oApplication.Forms.Item(WasteOilDisposalFormID)
            oDBDSHeader = frmWasteOilDisposal.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmWasteOilDisposal.DataSources.DBDataSources.Item(1)
            oMatrix = frmWasteOilDisposal.Items.Item("Matrix").Specific

            'Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmWasteOilDisposal.Freeze(True)
            oGfun.LoadComboBoxSeries(frmWasteOilDisposal.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmWasteOilDisposal.Items.Item("t_docdate").Specific)
            oGFun.LoadLocationComboBox(frmWasteOilDisposal.Items.Item("c_location").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            frmWasteOilDisposal.ActiveItem = "c_location"
            frmWasteOilDisposal.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmWasteOilDisposal.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmWasteOilDisposal.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmWasteOilDisposal.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmWasteOilDisposal.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmWasteOilDisposal.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmWasteOilDisposal.Items.Item("t_empname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmWasteOilDisposal.Items.Item("t_empid").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
           

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If oDBDSHeader.GetValue("U_Location", 0).Trim = "" Then
                oGFun.StatusBarErrorMsg("Location Should Not be left Empty")
                Return False
            ElseIf frmWasteOilDisposal.Items.Item("t_deltocd").Specific.value = "" Then
                oGFun.StatusBarErrorMsg(" Delivered To Should Not be left Empty")
                frmWasteOilDisposal.Items.Item("t_deltocd").Click()
                Return False
            ElseIf frmWasteOilDisposal.Items.Item("t_contact").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Contact Person Should Not be left Empty")
                frmWasteOilDisposal.Items.Item("t_contact").Click()
                Return False
            ElseIf frmWasteOilDisposal.Items.Item("t_chklstno").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Check List No Should Not be left Empty")
                frmWasteOilDisposal.Items.Item("t_chklstno").Click()
                Return False
            ElseIf frmWasteOilDisposal.Items.Item("t_delidby").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Delivered By Should Not be left Empty")
                frmWasteOilDisposal.Items.Item("t_delidby").Click()
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
                                Case "t_delidby"
                                    oDBDSHeader.SetValue("U_DelByCd", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_DelidBy", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_deltocd"
                                    oDBDSHeader.SetValue("U_DeltoCod", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                    oDBDSHeader.SetValue("U_DeltoNm", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                Case "t_recdby"
                                    oDBDSHeader.SetValue("U_RcvByCd", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_RecvdBy", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
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
                                If frmWasteOilDisposal.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmWasteOilDisposal.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmWasteOilDisposal.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
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
                                If pVal.BeforeAction = True And (frmWasteOilDisposal.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmWasteOilDisposal.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                            Case "t_delidby"
                                oGFun.ChooseFromListFilteration(frmWasteOilDisposal, "DELCFL", "empID", "select empID from OHEM where dept='13' ")
                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "pntschk"
                                        If pVal.BeforeAction = False Then
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail, oMatrix.VisualRowCount, pVal.ColUID)
                                        End If
                                End Select

                        End Select
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmWasteOilDisposal.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
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
                    frmWasteOilDisposal.ActiveItem = "t_empid"
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
                    If frmWasteOilDisposal.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
    
End Class
