Public Class DirectCashPurchase
    Dim frmDirectCashPurchase As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oMatrix As SAPbouiCOM.Matrix
    Dim oDBDSDetail As SAPbouiCOM.DBDataSource
    Dim UDOID As String = "ODCP"
    Dim StrQuery As String = ""
    Dim SQuery As String = ""
    Sub LoadfrmDirectCashPurchase()
        Try
            oGfun.LoadXML(frmDirectCashPurchase, DirectCashPurchaseFormID, DirectCashPurchaseXML)
            frmDirectCashPurchase = oApplication.Forms.Item(DirectCashPurchaseFormID)
            oDBDSHeader = frmDirectCashPurchase.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmDirectCashPurchase.DataSources.DBDataSources.Item(1)
            oMatrix = frmDirectCashPurchase.Items.Item("Matrix").Specific
            'Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Load Direct Cash Purchase Failed : " & ex.Message)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmDirectCashPurchase.Freeze(True)

            oGfun.LoadComboBoxSeries(frmDirectCashPurchase.Items.Item("c_series").Specific, UDOID)
            oGFun.LoadDocumentDate(frmDirectCashPurchase.Items.Item("t_docdate").Specific)
            oGFun.LoadLocationComboBox(frmDirectCashPurchase.Items.Item("c_location").Specific)
            oGFun.SetNewLine(oMatrix, oDBDSDetail)
            frmDirectCashPurchase.ActiveItem = "t_procode"

            frmDirectCashPurchase.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmDirectCashPurchase.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmDirectCashPurchase.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_empname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_empid").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmDirectCashPurchase.Items.Item("t_payperid").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            frmDirectCashPurchase.Items.Item("t_Design").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_National").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_LLFromDt").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_BLvPreyr").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_BLvCurYr").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_PvLSalDu").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("50").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            frmDirectCashPurchase.Items.Item("42").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_PassNo").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_BasicSal").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_LLToDt").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_BalLeave").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmDirectCashPurchase.Items.Item("t_NodayLev").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            frmDirectCashPurchase.Items.Item("t_payperid").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmDirectCashPurchase.Items.Item("t_FromDate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmDirectCashPurchase.Items.Item("t_Todate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)


        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If frmDirectCashPurchase.Items.Item("t_procode").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Project Code should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "t_procode"
                Return False
            End If
            If frmDirectCashPurchase.Items.Item("c_location").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Location Should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "c_location"
                Return False
            End If
            If frmDirectCashPurchase.Items.Item("t_mrno").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("MRNo-Material Requistion No Should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "t_mrno"
                Return False
            End If
            If frmDirectCashPurchase.Items.Item("t_purno").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Purchased By Should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "t_purno"
                Return False
            End If

            If frmDirectCashPurchase.Items.Item("t_invono").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Invoice Number Should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "t_invono"
                Return False
            End If

            If frmDirectCashPurchase.Items.Item("t_storekee").Specific.value = "" Then
                oGFun.StatusBarErrorMsg("Store Keeper Should not be left Empty..")
                frmDirectCashPurchase.ActiveItem = "t_storekee"
                Return False
            End If

            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Validation Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            Return False
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
                                Case "t_purbynam"
                                    oDBDSHeader.SetValue("U_PurByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_PurByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_strkpnam"
                                    oDBDSHeader.SetValue("U_StoreKpC", 0, Trim(oDataTable.GetValue("empID", 0)))
                                    oDBDSHeader.SetValue("U_StoreKpN", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                Case "t_payperid"
                                    oDBDSHeader.SetValue("U_PayPerid", 0, Trim(oDataTable.GetValue("Code", 0)))
                                Case "t_prjname"
                                    oDBDSHeader.SetValue("U_PrjCode", 0, oDataTable.GetValue("PrjCode", 0))
                                    oDBDSHeader.SetValue("U_PrjName", 0, oDataTable.GetValue("PrjName", 0))
                                Case "t_vencode"
                                    oDBDSHeader.SetValue("U_CardCode", 0, oDataTable.GetValue("CardCode", 0))
                                    oDBDSHeader.SetValue("U_CardName", 0, oDataTable.GetValue("CardName", 0))
                                Case "t_mrno"
                                    oDBDSHeader.SetValue("U_MrNo", 0, oDataTable.GetValue("DocNum", 0))
                                    Dim RsetItem As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    StrQuery = "SELECT B.U_ItemCode ItemCode ,b.U_ItemName ItemName,U_PurUOM Uom ,U_Quantity  Qty FROM [@INP_OIND] A,[@INP_IND1] B WHERE A.DocEntry =B.DocEntry AND A.DocNum ='" & oDBDSHeader.GetValue("U_MrNo", 0).Trim & "'"
                                    RsetItem.DoQuery(StrQuery)
                                    oMatrix.Clear()
                                    oDBDSDetail.Clear()
                                    RsetItem.MoveFirst()
                                    For i As Integer = 1 To RsetItem.RecordCount
                                        oDBDSDetail.InsertRecord(oDBDSDetail.Size)
                                        oDBDSDetail.Offset = oDBDSDetail.Size - 1
                                        oDBDSDetail.SetValue("LineID", oDBDSDetail.Offset, oDBDSDetail.Size)
                                        oDBDSDetail.SetValue("U_ItemCode", oDBDSDetail.Offset, Trim(RsetItem.Fields.Item("ItemCode").Value))
                                        oDBDSDetail.SetValue("U_ItemName", oDBDSDetail.Offset, Trim(RsetItem.Fields.Item("ItemName").Value))
                                        oDBDSDetail.SetValue("U_Unit", oDBDSDetail.Offset, Trim(RsetItem.Fields.Item("Uom").Value))
                                        oDBDSDetail.SetValue("U_Quantity", oDBDSDetail.Offset, Trim(RsetItem.Fields.Item("Qty").Value))
                                        RsetItem.MoveNext()
                                    Next
                                    oMatrix.LoadFromDataSource()
                                Case "Matrix"
                                    Select Case pVal.ColUID
                                        Case "matcode"
                                            oMatrix.FlushToDataSource()
                                            oDBDSDetail.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                            oDBDSDetail.SetValue("U_ItemName", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                            oMatrix.LoadFromDataSource()
                                            oGFun.SetNewLine(oMatrix, oDBDSDetail)
                                    End Select
                            End Select
                        End If
                    Catch ex As Exception
                        oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                    Select Case pVal.ItemUID
                        Case "Matrix"
                            Select Case pVal.ColUID
                                Case "matdes"
                                    If pVal.BeforeAction = False Then
                                        oGFun.SetNewLine(oMatrix, oDBDSDetail, pVal.Row, pVal.ColUID)
                                    End If
                            End Select
                    End Select
                Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                    Try
                        Select Case pVal.ItemUID
                            Case "Matrix"
                                Select Case pVal.ColUID
                                    Case "qty", "rate", "discprct"
                                        If pVal.BeforeAction = False And pVal.ItemChanged Then
                                            oMatrix.FlushToDataSource()
                                            Dim dblQuantity As Double = CDbl(oDBDSDetail.GetValue("U_Quantity", pVal.Row - 1))
                                            Dim dblRate As Double = CDbl(oDBDSDetail.GetValue("U_Rate", pVal.Row - 1))
                                            Dim dblPercent As Double = CDbl(oDBDSDetail.GetValue("U_DiscPrct", pVal.Row - 1))
                                            Dim dblDisAmt As Double = (dblQuantity * dblRate) * (dblPercent / 100)

                                            oDBDSDetail.SetValue("U_DiscAmt", pVal.Row - 1, dblDisAmt)
                                            oDBDSDetail.SetValue("U_Amount", pVal.Row - 1, (dblQuantity * dblRate) - dblDisAmt)

                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            Me.CalculateTotalSum()
                                        End If
                                    Case "discamt"
                                        If pVal.BeforeAction = False And pVal.ItemChanged Then
                                            oMatrix.FlushToDataSource()
                                            Dim dblQuantity As Double = CDbl(oDBDSDetail.GetValue("U_Quantity", pVal.Row - 1))
                                            Dim dblRate As Double = CDbl(oDBDSDetail.GetValue("U_Rate", pVal.Row - 1))
                                            Dim dblDisAmt As Double = CDbl(oDBDSDetail.GetValue("U_DiscAmt", pVal.Row - 1))

                                            Dim dblPercent As Double = dblDisAmt / (dblQuantity * dblRate) * 100

                                            oDBDSDetail.SetValue("U_DiscPrct", pVal.Row - 1, dblPercent)
                                            oDBDSDetail.SetValue("U_DiscAmt", pVal.Row - 1, dblDisAmt)
                                            oDBDSDetail.SetValue("U_Amount", pVal.Row - 1, (dblQuantity * dblRate) - dblDisAmt)
                                            oMatrix.LoadFromDataSource()
                                            oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            Me.CalculateTotalSum()
                                        End If
                                End Select
                        End Select
                    Catch ex As Exception
                        oGFun.StatusBarErrorMsg("Validate function Failed : " & ex.Message)
                    Finally
                    End Try
                   

                Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                    Try
                        Select Case pVal.ItemUID
                            Case "c_series"
                                If frmDirectCashPurchase.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                    'Get the Serial Number Based On Series...
                                    Dim oCmbSerial As SAPbouiCOM.ComboBox = frmDirectCashPurchase.Items.Item("c_series").Specific
                                    Dim strSerialCode As String = oCmbSerial.Selected.Value
                                    Dim strDocNum As Long = frmDirectCashPurchase.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
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
                                If pVal.BeforeAction = True And (frmDirectCashPurchase.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmDirectCashPurchase.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                            Case "t_purbynam"
                                oGFun.ChooseFromListFilteration(frmDirectCashPurchase, "PURCFL", "empID", "select empID from OHEM where dept='13' ")
                            Case "t_strkpnam"
                                oGFun.ChooseFromListFilteration(frmDirectCashPurchase, "STRKCFL", "empID", "select empID from OHEM where dept='13' ")
                            Case "t_mrno"
                                sQuery = "SELECT DocNum FROM [@INP_OIND] WHERE Status ='O' AND U_Location ='" & oDBDSHeader.GetValue("U_Location", 0).Trim() & "'"
                                oGFun.ChooseFromListFilteration(frmDirectCashPurchase, "MR_CFL", "DocNum", sQuery)
                        End Select
                    Catch ex As Exception
                        oGFun.StatusBarErrorMsg("Got Focus event Failed : " & ex.Message)
                    Finally
                    End Try

                Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                    Try
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.ActionSuccess And frmDirectCashPurchase.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
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
                    frmDirectCashPurchase.ActiveItem = "t_empid"
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
                    If frmDirectCashPurchase.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction = True Then

                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub CalculateTotalSum()
        Try
            Dim dblTotAmount = 0, dblTotDisAmt = 0, dblNetAmt As Double = 0
            For i As Integer = 1 To oMatrix.VisualRowCount

                dblTotAmount += CDbl(oDBDSDetail.GetValue("U_Quantity", i - 1)) * CDbl(oDBDSDetail.GetValue("U_Rate", i - 1))
                dblTotDisAmt += CDbl(oDBDSDetail.GetValue("U_DiscAmt", i - 1))

            Next
            oDBDSHeader.SetValue("U_TotAmt", 0, dblTotAmount)
            oDBDSHeader.SetValue("U_TotDis", 0, dblTotDisAmt)
            oDBDSHeader.SetValue("U_NetAmt", 0, dblTotAmount - dblTotDisAmt)
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("Calculate Sum Total Failded : " & ex.Message)
        Finally
        End Try
    End Sub

End Class
