Class ActCarriedOut

    Dim frmActCarriedOut As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim link As SAPbouiCOM.LinkedButton
    Dim oDBDSDetail1, oDBDSDetail2, oDBDSDetail3 As SAPbouiCOM.DBDataSource
    Dim oMatrix1, oMatrix2, oMatrix3 As SAPbouiCOM.Matrix
    Dim cmb_status As SAPbouiCOM.ComboBox
    Dim Folder2 As SAPbouiCOM.Folder
    Dim UDOID As String = "OACO"
    Dim DeleteRowITEMUID As String = ""
    Dim typee As String
    Dim plantid As String
    Dim StrQuery As String = ""
    Dim SQuery As String = ""
    Sub LoadActCarriedOut()
        Try
            oGFun.LoadXML(frmActCarriedOut, ActCarriedOutFormID, ActCarriedOutXML)
            frmActCarriedOut = oApplication.Forms.Item(ActCarriedOutFormID)
            setReport(ActCarriedOutFormID)
            oDBDSHeader = frmActCarriedOut.DataSources.DBDataSources.Item(0)
            oDBDSDetail1 = frmActCarriedOut.DataSources.DBDataSources.Item(1)
            oDBDSDetail2 = frmActCarriedOut.DataSources.DBDataSources.Item(2)
            oDBDSDetail3 = frmActCarriedOut.DataSources.DBDataSources.Item(3)
            frmActCarriedOut.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Add, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oMatrix1 = frmActCarriedOut.Items.Item("mtx_0").Specific
            oMatrix2 = frmActCarriedOut.Items.Item("mtx_1").Specific
            oMatrix3 = frmActCarriedOut.Items.Item("mtx_2").Specific
            Folder2 = frmActCarriedOut.Items.Item("tab_1").Specific
            Dim caption As String
            If HANA Then
                caption = oGFun.getSingleValue("select distinct T1.""DimDesc"" from OPRC T0 inner join ODIM T1 on T0.""DimCode""=T1.""DimCode"" where ifnull(T1.""DimActive"",'Y')='Y' and T1.""DimCode""='1'")
            Else
                caption = oGFun.getSingleValue("select distinct T1.DimDesc from OPRC T0 inner join ODIM T1 on T0.DimCode=T1.DimCode where isnull(T1.DimActive,'Y')='Y' and T1.DimCode='1'")
            End If
            oMatrix2.Columns.Item("dimcode").TitleObject.Caption = caption
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub setReport(ByVal FormUID As String)
        Try
            frmActCarriedOut = oApplication.Forms.Item(FormUID)
            Dim rptTypeService As SAPbobsCOM.ReportTypesService
            'Dim newType As SAPbobsCOM.ReportType
            Dim newtypesParam As SAPbobsCOM.ReportTypesParams
            rptTypeService = oCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)
            newtypesParam = rptTypeService.GetReportTypeList
            Dim TypeCode As String
            If HANA Then
                TypeCode = oGFun.getSingleValue("Select ""CODE"" from RTYP where ""NAME""='CarriedOut'")
            Else
                TypeCode = oGFun.getSingleValue("Select CODE from RTYP where NAME='CarriedOut'")
            End If
            frmActCarriedOut.ReportType = TypeCode
            'Dim i As Integer
            'For i = 0 To newtypesParam.Count - 1
            '    If newtypesParam.Item(i).TypeName = "CarriedOut" And newtypesParam.Item(i).MenuID = "CarriedOut" Then
            '        frmActCarriedOut.ReportType = newtypesParam.Item(i).TypeCode
            '        Exit For
            '    End If
            'Next i
        Catch ex As Exception
            oApplication.StatusBar.SetText("setReport Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End Try
    End Sub

    Sub InitForm()
        Try
            frmActCarriedOut.Freeze(True)
            frmActCarriedOut.PaneLevel = 1
            oGFun.LoadComboBoxSeries(frmActCarriedOut.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmActCarriedOut.Items.Item("t_docdate").Specific) ' Load Document Date
            If HANA Then
                oGFun.setComboBoxValue(frmActCarriedOut.Items.Item("c_location").Specific, "Select ""Code"", ""Location"" from OLCT")
                oGFun.setComboBoxValue(frmActCarriedOut.Items.Item("t_type").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")
            Else
                oGFun.setComboBoxValue(frmActCarriedOut.Items.Item("c_location").Specific, "Select Code, Location from OLCT") 'Load Location)
                oGFun.setComboBoxValue(frmActCarriedOut.Items.Item("t_type").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If

            frmActCarriedOut.ActiveItem = "c_location"
            oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
            oGFun.SetNewLine(oMatrix3, oDBDSDetail3)
            'oMatrix2.Columns.Item("Stat").Visible = False
            oMatrix3.Columns.Item("Stat").Visible = False
            frmActCarriedOut.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmActCarriedOut.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmActCarriedOut.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_ename").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_ename").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("t_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("t_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_macno").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_macno").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_mdesc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_shano").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_shano").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_pclno").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActCarriedOut.Items.Item("txt_ename").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActCarriedOut.Items.Item("txt_macno").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActCarriedOut.Items.Item("txt_shano").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmActCarriedOut.Items.Item("txt_ftime").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmActCarriedOut.Items.Item("txt_ttime").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActCarriedOut.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActCarriedOut.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            ' frmActCarriedOut.Items.Item("t_freq").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmActCarriedOut.Items.Item("c_status").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Add, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try

            'If oDBDSHeader.GetValue("U_Type", 0).Trim = "" Then
            '    oApplication.StatusBar.SetText("Type Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '    frmActCarriedOut.ActiveItem = "t_type"
            '    Exit Function
            'End If
            If frmActCarriedOut.Items.Item("txt_pclno").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Check List No Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmActCarriedOut.ActiveItem = "txt_pclno"
                Exit Function
            End If
            If frmActCarriedOut.Items.Item("txt_macno").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("ID No Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmActCarriedOut.ActiveItem = "txt_macno"
                Exit Function
            End If
            If frmActCarriedOut.Items.Item("txt_shano").Specific.value.Equals(Trim("")) = True Then
                oApplication.StatusBar.SetText("Sch.Activity No Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                frmActCarriedOut.ActiveItem = "txt_shano"
                Exit Function
            End If
            'If frmActCarriedOut.Items.Item("txt_ename").Specific.value.Equals(Trim("")) = True Then
            '    oApplication.StatusBar.SetText("Employee Name Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '    frmActCarriedOut.ActiveItem = "txt_ename"
            '    Exit Function
            'End If

            'If frmActCarriedOut.Items.Item("txt_ftime").Specific.value.Equals(Trim("")) = True Then
            '    oApplication.StatusBar.SetText("From Time Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '    frmActCarriedOut.ActiveItem = "txt_ftime"
            '    Exit Function
            'End If
            'If frmActCarriedOut.Items.Item("txt_ttime").Specific.value.Equals(Trim("")) = True Then
            '    oApplication.StatusBar.SetText("To Time Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '    frmActCarriedOut.ActiveItem = "txt_ttime"
            '    Exit Function
            'End If
            'If oDBDSHeader.GetValue("U_Freqncy", 0).Trim = "" Then
            '    oApplication.StatusBar.SetText("Frequency Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '    frmActCarriedOut.ActiveItem = "t_freq"
            '    Exit Function
            'End If
            'oMatrix1.FlushToDataSource()
            If oMatrix1.VisualRowCount = 0 Then
                oApplication.StatusBar.SetText("Grid Details Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If
            'If oMatrix2.VisualRowCount > 0 Then
            '    For i As Integer = 1 To oMatrix2.VisualRowCount
            '        oMatrix2.GetLineData(i)
            '        If Trim(oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.Value).Equals("") = True Then
            '            oApplication.StatusBar.SetText("Row No: " & i & " Item ID Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '            Exit Function
            '        Else
            '            If CDbl(oMatrix2.Columns.Item("qty").Cells.Item(i).Specific.Value) <= 0 Then
            '                oApplication.StatusBar.SetText("Qty Cannot Be Left Empty or Less Than Zero.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '                Exit Function
            '            End If
            '        End If
            '    Next
            'End If
            Dim boolStatus As Boolean = False
            oMatrix1.FlushToDataSource()
            For i As Integer = 1 To oMatrix1.VisualRowCount
                oMatrix1.GetLineData(i)
                If oMatrix1.Columns.Item("status").Cells.Item(i).Specific.Selected.Value.Equals("P") = True Then
                    'oApplication.StatusBar.SetText("Row No: " & i & " Schedule Activity Cannot Be Left Empty.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    boolStatus = True
                    Return True
                End If
            Next

            If Trim(oDBDSHeader.GetValue("U_GINo", 0)).Equals("") Then
                boolStatus = True
                Return True
            End If
            
            'If Trim(oDBDSHeader.GetValue("U_JENo", 0)).Equals("") Then
            '    boolStatus = True
            '    Return True
            'End If
            
            'If boolStatus = False And Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") Then If oApplication.MessageBox("Do you want to close the Maintenance CarriedOut Status? You cannot change the document", 1, "Yes", "No") = 1 Then oDBDSHeader.SetValue("Status", 0, "C")
            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Validate Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            ValidateAll = False
        Finally
        End Try
    End Function

    Sub LoadGridDetails()
        Try
            If HANA Then
                SQuery = "SELECT B.""LineId"", B.""U_ActCode"" ,B.""U_ActName"",A.""U_SchedDt"" FROM ""@MIPL_PM_OACP"" A,""@MIPL_PM_ACP1"" B WHERE A.""DocEntry"" =B.""DocEntry"" AND B.""U_ActCode"" IS NOT NULL AND A.""DocNum"" ='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
            Else
                SQuery = "SELECT b.LineId, B.U_ActCode ,B.U_ActName,A.U_SchedDt FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP1] B WHERE A.DocEntry =B.DocEntry AND b.U_ActCode IS NOT NULL AND A.DocNum ='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'" '  AND B.U_freq ='" & oDBDSHeader.GetValue("U_Freqncy", 0).Trim & "'"
            End If

            Dim rsetFreq As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix1.Clear()
            oDBDSDetail1.Clear()

            rsetFreq.MoveFirst()
            For i As Integer = 1 To rsetFreq.RecordCount
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                oDBDSDetail1.SetValue("U_LineNum", oDBDSDetail1.Offset, rsetFreq.Fields.Item("LineID").Value)
                oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_ActCode").Value)
                oDBDSDetail1.SetValue("U_ActName", oDBDSDetail1.Offset, rsetFreq.Fields.Item("U_ActName").Value)
                rsetFreq.MoveNext()
            Next
            oMatrix1.LoadFromDataSource()

            oMatrix1.AutoResizeColumns()
            Dim WhsCode As String
            Dim oCmbType As SAPbouiCOM.ComboBox = frmActCarriedOut.Items.Item("t_type").Specific
            If oCmbType.Selected.Value = "VH" Then
                WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
            Else
                WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
            End If
            If HANA Then
                SQuery = "SELECT B.""U_ItemCode"" ,B.""U_ItemName"",B.""U_UOM"",B.""U_Quantity"",W.""AvgPrice"" , IFNULL(B.""U_Quantity"",0) * IFNULL(W.""AvgPrice"",0) ""Total""  FROM ""@MIPL_PM_OACP"" A,""@MIPL_PM_ACP2"" B , OITM i,OITW W  WHERE A.""DocEntry"" =B.""DocEntry"" AND B.""U_ItemCode"" IS NOT NULL AND B.""U_ItemCode"" = i.""ItemCode"" and i.""ItemCode""=w.""ItemCode"" and W.""WhsCode""='" & WhsCode & "' AND A.""DocNum"" ='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
            Else
                SQuery = "SELECT B.U_ItemCode ,B.U_ItemName,B.U_UOM,B.U_Quantity,W.AvgPrice , ISNULL(B.U_Quantity,0) * ISNULL(W.AvgPrice,0) Total  FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP2] B , OITM i,OITW W  WHERE A.DocEntry =B.DocEntry AND B.U_ItemCode IS NOT NULL AND b.U_ItemCode = i.ItemCode and i.ItemCode=w.ItemCode and W.WhsCode='" & WhsCode & "' AND A.DocNum ='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
            End If

            Dim rsetSpares As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix2.Clear()
            oDBDSDetail2.Clear()

            rsetSpares.MoveFirst()
            For j As Integer = 1 To rsetSpares.RecordCount
                oDBDSDetail2.InsertRecord(oDBDSDetail2.Size)
                oDBDSDetail2.Offset = oDBDSDetail2.Size - 1
                oDBDSDetail2.SetValue("LineID", oDBDSDetail2.Offset, j)
                oDBDSDetail2.SetValue("U_ItemCode", oDBDSDetail2.Offset, rsetSpares.Fields.Item("U_ItemCode").Value)
                oDBDSDetail2.SetValue("U_ItemName", oDBDSDetail2.Offset, rsetSpares.Fields.Item("U_ItemName").Value)
                oDBDSDetail2.SetValue("U_UOM", oDBDSDetail2.Offset, rsetSpares.Fields.Item("U_UOM").Value)
                oDBDSDetail2.SetValue("U_Quantity", oDBDSDetail2.Offset, rsetSpares.Fields.Item("U_Quantity").Value)
                oDBDSDetail2.SetValue("U_AvgPrice", oDBDSDetail2.Offset, rsetSpares.Fields.Item("AvgPrice").Value)
                oDBDSDetail2.SetValue("U_Total", oDBDSDetail2.Offset, rsetSpares.Fields.Item("Total").Value)
                rsetSpares.MoveNext()
            Next
            oMatrix2.LoadFromDataSource()
            oMatrix2.AutoResizeColumns()
            Me.CalculateGrandTotal()
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("LoadGridDetails Function Failed.." & ex.Message())
        Finally
        End Try
    End Sub

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction = True Then
                frmActCarriedOut = oApplication.Forms.Item(FormUID)
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Dim strqry As String
                            Select Case pVal.ItemUID
                                Case "txt_ename"
                                    ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "EmpCFL", "empID", "select empID from OHEM where dept='13' ")

                                Case "txt_shano"
                                    If HANA Then
                                        SQuery = " SELECT ""DocNum"" FROM ""@MIPL_PM_OACP"" WHERE ""U_MacNo""='" & oDBDSHeader.GetValue("U_MACNo", 0).Trim & "' and ""U_Type""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and ""Status"" = 'O'"
                                    Else
                                        SQuery = " SELECT DocNum FROM [@MIPL_PM_OACP] WHERE U_MacNo='" & oDBDSHeader.GetValue("U_MACNo", 0).Trim & "' and U_Type='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and Status = 'O'"
                                    End If

                                    oGFun.ChooseFromListFilteration(frmActCarriedOut, "CFLsano", "DocNum", SQuery)
                                Case "mtx_1"
                                    Select Case pVal.ColUID
                                        Case "itemcode"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqual(frmActCarriedOut, oMatrix2, "ITMCFL", "itemcode")
                                            oGFun.ChooseFromLisBefore_Spares(frmActCarriedOut, "ITMCFL")
                                            'oGFun.ChooseFromLisBefore(frmActCarriedOut, "ITMCFL", "InvntItem", "Y")
                                        Case "dimcode"
                                            oGFun.ChooseFromLisBefore(frmActCarriedOut, "DIMCFL", "DimCode", "1")
                                    End Select
                                Case "mtx_2"
                                    Select Case pVal.ColUID
                                        Case "empname"

                                            ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "MAN_CFL", "empID", "select empID from OHEM where dept='13' ")

                                    End Select
                                    'Case "txt_shano"
                                    ' strqry = " SELECT DocNum FROM [@MIPL_PM_OACP] WHERE U_MacNo ='" & oDBDSHeader.GetValue("U_MacNo", 0).Trim & "'"
                                    'oGFun.ChooseFromListFilteration(frmActCarriedOut, "CFLsano", "DocNum", strqry)

                                Case "txt_macno"

                                    Dim oTxt As SAPbouiCOM.EditText = frmActCarriedOut.Items.Item(pVal.ItemUID).Specific
                                    If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                                        If HANA Then
                                            StrQuery = "SELECT ""U_ItemCode""  from ""@MIPL_PM_OVHL"" where ""U_Location""='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "' "
                                        Else
                                            StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] where U_Location='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "' "
                                        End If

                                        oTxt.ChooseFromListUID = "OVHL_CFL"
                                        oTxt.ChooseFromListAlias = "U_ItemCode"
                                        '      oGFun.ChooseFromListFilteration(frmActCarriedOut, "OVHL_CFL", "U_ItemCode", StrQuery)
                                    Else
                                        If HANA Then
                                            StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" Where ""U_InsType""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and ""U_Location""='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "'"
                                        Else
                                            StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and U_Location='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "'"
                                        End If

                                        oTxt.ChooseFromListUID = "OMAC_CFL"
                                        oTxt.ChooseFromListAlias = "Code"
                                        ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "OMAC_CFL", "Code", StrQuery)
                                    End If

                                Case "t_IndentNo"
                                    If HANA Then
                                        strqry = "Select  A.""U_IndentNo"" from ""@INM_OPTS"" A,""@INM_PTS1"" B where A.""DocEntry""=B.""DocEntry"" and A.""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.""U_rejqty"">0 and B.""U_ItemCode"" Is Not Null And ""DocNum"" Not in (Select ""U_IndentNo"" from ""@INM_ORSE"" A ,""@INM_RSE1"" B where A.""DocEntry""=B.""DocEntry"" Group by ""U_IndentNo"")  Union  Select A.""U_IndentNo"" From ""@INM_OPTS"" A,""@INM_PTS1"" B,(Select ""U_Indentno"",Sum(""U_RejQty"") ""RejectQty"" From ""@INM_ORSE"" A ,""@INM_RSE1"" B  Where A.""DocEntry""=B.""DocEntry"" and ""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By ""U_IndentNo"",""U_Wrkordno"" ) C  Where A.""DocEntry""=B.""DocEntry"" And ""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (""U_rejqty"" - C.""rejectQty"")>0 "
                                    Else
                                        strqry = "Select  A.U_IndentNo from [@INM_OPTS] A,[@INM_PTS1] B where A.DocEntry=B.DocEntry and A.U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.U_rejqty>0 and B.U_ItemCode Is Not Null And DocNum Not in (Select U_IndentNo from [@INM_ORSE]A ,[@INM_RSE1] B where A.DocEntry=B.DocEntry Group by U_IndentNo)  Union  Select A.U_IndentNo From [@INM_OPTS] A,[@INM_PTS1] B,(Select U_Indentno,Sum(U_RejQty) RejectQty From [@INM_ORSE] A ,[@INM_RSE1] B  Where A.DocEntry=B.DocEntry and U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By U_IndentNo,U_Wrkordno ) C  Where A.DocEntry=B.DocEntry And U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (U_rejqty - C.rejectQty)>0 "
                                    End If

                                    'oGFun.ChooseFromListFilteration(frmActCarriedOut, "IndentCFL", "DocNum", strqry)

                            End Select

                        Catch ex As Exception
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        Select Case pVal.ItemUID
                            Case "BtnView"
                                Try
                                    Dim cflForm As SAPbouiCOM.Form
                                    If oApplication.Forms.Count > 0 Then
                                        For frm As Integer = 0 To oApplication.Forms.Count - 1
                                            If oApplication.Forms.Item(frm).UniqueID = "TRANVIEW" Then
                                                cflForm = oApplication.Forms.Item("TRANVIEW")
                                                cflForm.Close()
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If HANA Then
                                        StrQuery = "Select * from (Select T0.""DocEntry"",T0.""DocNum"",T0.""DocDate"",T0.""U_MCOutNo"" as ""CarriedOutNum"",'Document' as ""TranType"",T1.""ItemCode"",T1.""Dscription"",T1.""Quantity"" from OIGE T0 join IGE1 T1 on T0.""DocEntry""=T1.""DocEntry"" "
                                        StrQuery += vbCrLf + "Union all"
                                        StrQuery += vbCrLf + "Select T0.""DocEntry"",T0.""DocNum"",T0.""DocDate"",T0.""U_MCOutNo"" as ""CarriedOutNum"",'Draft' as ""TranType"",T1.""ItemCode"",T1.""Dscription"",T1.""Quantity"" from ODRF T0 join DRF1 T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""DocStatus""='O') A"
                                        StrQuery += vbCrLf + "where A.""CarriedOutNum""='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "' order by A.""DocEntry"" "
                                    Else
                                        StrQuery = "Select * from (Select T0.DocEntry,T0.DocNum,T0.DocDate,T0.U_MCOutNo as CarriedOutNum,'Document' as TranType,T1.ItemCode,T1.Dscription,T1.Quantity from OIGE T0 join IGE1 T1 on T0.DocEntry=T1.DocEntry "
                                        StrQuery += vbCrLf + "Union all"
                                        StrQuery += vbCrLf + "Select T0.DocEntry,T0.DocNum,T0.DocDate,T0.U_MCOutNo as CarriedOutNum,'Draft' as TranType,T1.ItemCode,T1.Dscription,T1.Quantity from ODRF T0 join DRF1 T1 on T0.DocEntry=T1.DocEntry where T0.DocStatus='O') A"
                                        StrQuery += vbCrLf + "where A.CarriedOutNum='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "' order by A.DocEntry "
                                    End If
                                    oTranDataFormID.LoadViewTranData(StrQuery, "60", "")
                                    'link = frmActCarriedOut.Items.Item("lkGI").Specific
                                    'link.LinkedObjectType = "-1"
                                    'Dim ActualEntry As String = ""
                                    'If HANA Then
                                    '    ActualEntry = oGFun.getSingleValue("Select T0.""DocEntry"" from ODRF T0 where ""ObjType""=60 and ifnull(T0.""DocStatus"",'')='O' and T0.""DocEntry""='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                                    'Else
                                    '    ActualEntry = oGFun.getSingleValue("Select T0.DocEntry from ODRF T0 where ObjType=60 and isnull(T0.DocStatus,'')='O' and T0.DocEntry='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                                    'End If
                                    'If ActualEntry = "" Then
                                    '    link.LinkedObjectType = "60"
                                    '    link.Item.LinkTo = "txtGI"
                                    'Else
                                    '    link.LinkedObjectType = "112"
                                    '    link.Item.LinkTo = "txtGI"
                                    'End If
                                Catch ex As Exception
                                End Try
                        End Select
                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Select Case pVal.ItemUID
                            Case "mtx_1"
                                Select Case pVal.ColUID
                                    Case "GIEntry"
                                        Try
                                            Dim ColItem As SAPbouiCOM.Column = oMatrix2.Columns.Item("GIEntry")
                                            Dim objlink As SAPbouiCOM.LinkedButton = ColItem.ExtendedObject
                                            Dim ActualEntry As String = ""
                                            If HANA Then
                                                ActualEntry = oGFun.getSingleValue("Select T0.""DocEntry"" from ODRF T0 where ""ObjType""=60 and ifnull(T0.""DocStatus"",'')='O' and T0.""DocEntry""='" & oDBDSDetail2.GetValue("U_GINo", pVal.Row - 1).Trim & "'")
                                            Else
                                                ActualEntry = oGFun.getSingleValue("Select T0.DocEntry from ODRF T0 where ObjType=60 and isnull(T0.DocStatus,'')='O' and T0.DocEntry='" & oDBDSDetail2.GetValue("U_GINo", pVal.Row - 1).Trim & "'")
                                            End If
                                            If ActualEntry = "" Then
                                                objlink.LinkedObjectType = "60"
                                                objlink.Item.LinkTo = "GIEntry"
                                            Else
                                                objlink.LinkedObjectType = "112"
                                                objlink.Item.LinkTo = "GIEntry"
                                            End If
                                        Catch ex As Exception
                                        End Try
                                End Select
                        End Select
                End Select
            Else
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE
                        If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then Exit Sub
                            Dim oDataTable As SAPbouiCOM.DataTable
                            Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                            oDataTable = oCFLE.SelectedObjects
                            Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oDataTable = oCFLE.SelectedObjects
                            If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                                Select Case pVal.ItemUID
                                    Case "txt_shano"
                                        oDBDSHeader.SetValue("U_SchPlanNo", 0, Trim(oDataTable.GetValue("DocNum", 0)))
                                        oDBDSHeader.SetValue("U_PMCNo", 0, Trim(oDataTable.GetValue("U_PMCNo", 0)))
                                        oDBDSHeader.SetValue("U_PMCName", 0, Trim(oDataTable.GetValue("U_PMCName", 0)))
                                        Me.LoadGridDetails()
                                        If oMatrix2.VisualRowCount = 0 Then
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                        End If
                                        'Me.LoadDetails()
                                    Case "txt_ename"
                                        oDBDSHeader.SetValue("U_empID", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_empName", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                    Case "txt_agency"
                                        oDBDSHeader.SetValue("U_agcode", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                        oDBDSHeader.SetValue("U_agcyname", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                    Case "txt_macno"
                                        If Not (oCFLE.SelectedObjects Is Nothing) Then
                                            oDBDSHeader.SetValue("U_MacNo", 0, Trim(oDataTable.GetValue("Code", 0)))
                                            oDBDSHeader.SetValue("U_MacName", 0, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                        End If
                                    Case "mtx_0"
                                        Select Case pVal.ColUID
                                            Case "actname"
                                                oMatrix1.FlushToDataSource()
                                                oDBDSDetail1.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail1.SetValue("U_ActName", pVal.Row - 1, oDataTable.GetValue("U_Activity", 0))
                                                oMatrix1.LoadFromDataSource()

                                                ' oMatrix1.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                            Case "Prentry"                                                '
                                                oMatrix1.FlushToDataSource()
                                                'oDBDSDetail1.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail1.SetValue("U_PREntry", pVal.Row - 1, oDataTable.GetValue("DocEntry", 0))
                                                oMatrix1.LoadFromDataSource()
                                        End Select
                                        oMatrix1.AutoResizeColumns()
                                    Case "mtx_1"
                                        Select Case pVal.ColUID
                                            Case "itemcode"
                                                'oDBDSDetail2.Clear()
                                                Dim oCmbType As SAPbouiCOM.ComboBox = frmActCarriedOut.Items.Item("t_type").Specific
                                                If oCmbType.Selected Is Nothing Then oGFun.oApplication.StatusBar.SetText("Please update header details...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning) : Exit Sub
                                                oMatrix2.FlushToDataSource()
                                                oDBDSDetail2.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail2.SetValue("U_ItemCode", pVal.Row - 1, oDataTable.GetValue("ItemCode", 0))
                                                oDBDSDetail2.SetValue("U_ItemName", pVal.Row - 1, oDataTable.GetValue("ItemName", 0))
                                                oDBDSDetail2.SetValue("U_UOM", pVal.Row - 1, oDataTable.GetValue("InvntryUom", 0))
                                                oDBDSDetail2.SetValue("U_Quantity", pVal.Row - 1, "1")

                                                Dim WhsCode, GetPrice As String
                                                If oCmbType.Selected.Value = "VH" Then
                                                    If HANA Then
                                                        WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                                                    Else
                                                        WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                                                    End If
                                                Else
                                                    If HANA Then
                                                        WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                                                    Else
                                                        WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                                                    End If
                                                End If
                                                If WhsCode <> "" Then
                                                    If HANA Then
                                                        GetPrice = oGFun.getSingleValue("select ""AvgPrice"" from OITW where ""ItemCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and ""WhsCode""='" & WhsCode & "'")
                                                    Else
                                                        GetPrice = oGFun.getSingleValue("select AvgPrice from OITW where ItemCode='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and WhsCode='" & WhsCode & "'")
                                                    End If
                                                    'oDBDSDetail2.SetValue("U_AvgPrice", pVal.Row - 1, oDataTable.GetValue("AvgPrice", 0))
                                                    If GetPrice <> "0" Then
                                                        oDBDSDetail2.SetValue("U_AvgPrice", pVal.Row - 1, GetPrice)
                                                        oDBDSDetail2.SetValue("U_Total", pVal.Row - 1, CDbl(GetPrice))
                                                    End If
                                                End If
                                                oMatrix2.LoadFromDataSource()
                                                Me.CalculateGrandTotal()
                                                oDBDSDetail2.Clear()
                                                oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, pVal.ColUID)
                                                oDBDSDetail2.Clear()
                                                oMatrix2.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                            Case "dimcode"
                                                'oDBDSDetail2.Clear()
                                                oMatrix2.FlushToDataSource()
                                                oDBDSDetail2.SetValue("U_Costcenter", pVal.Row - 1, Trim(oDataTable.GetValue("PrcCode", 0)))
                                                oMatrix2.LoadFromDataSource()
                                        End Select
                                        oMatrix2.AutoResizeColumns()
                                    Case "mtx_2"
                                        Select Case pVal.ColUID
                                            Case "empname"
                                                oMatrix3.FlushToDataSource()
                                                oDBDSDetail3.SetValue("U_empName", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                                oDBDSDetail3.SetValue("U_empID", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                                oDBDSDetail3.SetValue("U_HrCost", pVal.Row - 1, Trim(oDataTable.GetValue("U_HrCost", 0)))
                                                oDBDSDetail3.SetValue("U_ManHrs", pVal.Row - 1, "1")
                                                oDBDSDetail3.SetValue("U_TotCost", pVal.Row - 1, CDbl(Trim(oDataTable.GetValue("U_HrCost", 0))))
                                                'SQuery = "SELECT ((a.Addition - a.Deduction)/30)/8 Salary FROM ( SELECT CASE U_PayType WHEN 'A' THEN ISNULL(b.U_Amount,0) ELSE 0 END Addition,CASE U_PayType WHEN 'D' THEN ISNULL(b.U_Amount,0) ELSE 0 END Deduction   FROM [@INPR_OECI]  a INNER JOIN [@INPR_ECI4] b ON a.Code = b.Code  Where a.U_empID = '" & oDataTable.GetValue("empID", 0) & "' ) a "
                                                'Dim rsetQry As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                                                'If rsetQry.RecordCount > 0 Then
                                                '    oDBDSDetail3.SetValue("U_HrCost", pVal.Row - 1, rsetQry.Fields.Item("Salary").Value)
                                                '    oDBDSDetail3.SetValue("U_TotCost", pVal.Row - 1, CDbl(rsetQry.Fields.Item("Salary").Value) * CDbl(oDBDSDetail3.GetValue("U_ManHrs", pVal.Row - 1)))
                                                'End If
                                                oMatrix3.LoadFromDataSource()
                                                Me.CalculateGrandTotal()
                                                oDBDSDetail3.Clear()
                                                oGFun.SetNewLine(oMatrix3, oDBDSDetail3, oMatrix3.VisualRowCount, pVal.ColUID)
                                                oDBDSDetail3.Clear()
                                                oMatrix3.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End Select
                                        oMatrix3.AutoResizeColumns()
                                    Case "mtx_0"
                                        Select Case pVal.ColUID
                                            Case "OACT_CFL"
                                                oMatrix1.FlushToDataSource()
                                                For i As Integer = 1 To oMatrix1.VisualRowCount
                                                    '    If oMatrix1.Columns.Item("actname").Cells.Item(i).Specific.value.ToString.Trim() = Trim(oDataTable.GetValue("U_schacty", 0)).Trim Then
                                                    '        oGFun.StatusBarErrorMsg(oMatrix1.Columns.Item("actname").Cells.Item(i).Specific.value.ToString.Trim() & " Already Exists in the Table")
                                                    '        BubbleEvent = False
                                                    '        Exit Sub
                                                    '    End If
                                                Next
                                                Dim clno As String = "0"
                                                '= GetValue("U_ActName", 0)

                                                Dim rsetScheduleActivity As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                                Dim s As String
                                                If HANA Then
                                                    s = "SELECT D.""LineId"",D.""U_ActCode"",D.""U_ActName"",D.""U_freq"",D.""U_nxtschdt"",D.""U_remarks"",D.""U_schdt"" FROM ""@FAST_PM_SCHACT_A"" D,""@FAST_PM_SCHACT"" H WHERE H.""DocEntry""=D.""DocEntry"" AND D.""U_ActName""='" & Trim(clno) & "'"
                                                Else
                                                    s = "SELECT D.LineId,D.U_ActCode,D.U_ActName,D.U_freq,D.U_nxtschdt,D.U_remarks,D.U_schdt FROM [@FAST_PM_SCHACT_A] D,[@FAST_PM_SCHACT] H WHERE H.DocEntry=D.DocEntry AND D.U_ActName='" & Trim(clno) & "'"
                                                End If

                                                rsetScheduleActivity.DoQuery(s)
                                                For a As Integer = 1 To rsetScheduleActivity.RecordCount
                                                    oDBDSDetail1.Offset = pVal.Row - 1
                                                    oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_ActCode").Value)
                                                    oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_ActName").Value)
                                                    oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "Pending")
                                                    oDBDSDetail1.SetValue("U_freq", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_freq").Value)
                                                    Dim schdt As String = CDate(rsetScheduleActivity.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd")
                                                    oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, CDate(rsetScheduleActivity.Fields.Item("U_schdt").Value).ToString("yyyyMMdd"))
                                                    oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, schdt)
                                                    'oDBDSDetail.SetValue("U_schdt", oDBDSDetail.Offset, schedt)
                                                    oDBDSDetail1.SetValue("U_remarks", oDBDSDetail1.Offset, rsetScheduleActivity.Fields.Item("U_remarks").Value)
                                                    oMatrix1.SetLineData(pVal.Row)
                                                    If a < rsetScheduleActivity.RecordCount - 1 Then
                                                        rsetScheduleActivity.MoveNext()
                                                    End If
                                                Next
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
                                Case "mtx_1"
                                    Select Case pVal.ColUID
                                        Case "itemcode"
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, pVal.ColUID)
                                    End Select
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "actcode", "actname"
                                            oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, pVal.ColUID)
                                        Case "ItemCode"
                                            'select T1.ItemCode,(Select ItemName from OITM where ItemCode=T1.ItemCode) as ItemName,T1.LineTotal 
                                            'from OPRQ T0 join PRQ1 T1 on T0.DocEntry=T1.DocEntry where T0.DocEntry=$[@MIPL_PM_ACO1.U_PREntry]
                                            Dim rsetVal As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                            If oMatrix1.Columns.Item("ItemCode").Cells.Item(pVal.Row).Specific.String <> "" Then
                                                If HANA Then
                                                    StrQuery = "select T1.""ItemCode"",(Select ""ItemName"" from OITM where ""ItemCode""=T1.""ItemCode"") as ""ItemName"",T1.""LineTotal"" from OPRQ T0 join PRQ1 T1 on T0.""DocEntry""=T1.""DocEntry"" where T1.""ItemCode""='" & oMatrix1.Columns.Item("ItemCode").Cells.Item(pVal.Row).Specific.String & "'"
                                                    rsetVal.DoQuery(StrQuery)
                                                Else
                                                    StrQuery = "select T1.ItemCode,(Select ItemName from OITM where ItemCode=T1.ItemCode) as ItemName,T1.LineTotal from OPRQ T0 join PRQ1 T1 on T0.DocEntry=T1.DocEntry where T1.ItemCode='" & oMatrix1.Columns.Item("ItemCode").Cells.Item(pVal.Row).Specific.String & "'"
                                                    rsetVal.DoQuery(StrQuery)
                                                End If
                                                If rsetVal.RecordCount > 0 Then
                                                    'oDBDSDetail1.Offset = pVal.Row - 1
                                                    oMatrix1.Columns.Item("ItemName").Cells.Item(pVal.Row).Specific.String = rsetVal.Fields.Item(1).Value.ToString
                                                    oMatrix1.Columns.Item("Linetot").Cells.Item(pVal.Row).Specific.String = rsetVal.Fields.Item(2).Value.ToString
                                                    frmActCarriedOut.Refresh()
                                                    frmActCarriedOut.Update()
                                                    'oDBDSDetail1.SetValue("U_ItemName", oDBDSDetail1.Offset, rsetVal.Fields.Item(1).Value.ToString)
                                                    'oDBDSDetail1.SetValue("U_Linetot", oDBDSDetail1.Offset, rsetVal.Fields.Item(2).Value.ToString)
                                                    'oMatrix1.SetLineData(pVal.Row)
                                                End If
                                            End If
                                    End Select

                                Case "mtx_2"
                                    Select Case pVal.ColUID
                                        Case "empname"
                                            oGFun.SetNewLine(oMatrix3, oDBDSDetail3, oMatrix3.VisualRowCount, pVal.ColUID)
                                    End Select
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                        Try
                            Select Case pVal.ItemUID
                                Case "txt_shano"
                                    If pVal.BeforeAction = False And pVal.ItemChanged Then
                                        oDBDSHeader.SetValue("U_PMCNo", 0, "")
                                        oDBDSHeader.SetValue("U_PMCName", 0, "")
                                        'oMatrix1.Clear()
                                        'oDBDSDetail1.Clear()
                                        'oMatrix2.Clear()
                                        'oDBDSDetail2.Clear()
                                        'oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                    End If
                                Case "t_curread"
                                    If pVal.BeforeAction = False Then
                                        Dim str As String
                                        'If HANA Then
                                        '    str = "select * from ""@MIPL_PM_OACO"" where ""U_MACNo""='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "'"
                                        'Else
                                        '    str = "select * from [@MIPL_PM_OACO] where U_MACNo='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "'"
                                        'End If

                                        'Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        'rset.DoQuery(str)
                                        'If rset.RecordCount() > 0 Then
                                        '''''''''--------CheckList Reading---------------------------------------------------------------------------------------
                                        Dim str1 As String
                                        If HANA Then
                                            'str1 = "select ""U_Reading"" from ""@MIPL_PM_OPCL"" where ""U_category""='" & oDBDSHeader.GetValue("U_PMCName", 0).Trim & "'"
                                            str1 = "select ""U_Reading"" from ""@MIPL_PM_OPCL"" where ""Name""='" & oDBDSHeader.GetValue("U_PMCName", 0).Trim & "'"
                                        Else
                                            'str1 = "select U_Reading from [@MIPL_PM_OPCL] where U_category='" & oDBDSHeader.GetValue("U_PMCName", 0).Trim & "'"
                                            str1 = "select U_Reading from [@MIPL_PM_OPCL] where Name='" & oDBDSHeader.GetValue("U_PMCName", 0).Trim & "'"
                                        End If

                                        Dim rset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        rset1.DoQuery(str1)
                                        '''''''''----------MAX(CarriedOut Reading)-------------------------------------------------------------------------------
                                        Dim str2 As String
                                        If HANA Then
                                            str2 = "select  ""U_CurRead""  from ""@MIPL_PM_OACO"" where ""U_MACNo""='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "' and ""DocNum"" in (select MAX(""DocNum"")from ""@MIPL_PM_OACO"" where ""U_MACNo""='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                                        Else
                                            str2 = "select  U_CurRead  from [@MIPL_PM_OACO] where U_MACNo='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "' and DocNum in (select MAX(DocNum)from [@MIPL_PM_OACO] where U_MACNo='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                                        End If

                                        Dim rset2 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        rset2.DoQuery(str2)
                                        '''''''---------------------
                                        Dim checkreading As Double = rset1.Fields.Item("U_Reading").Value    ''''''''''''''CheckList Reading
                                        Dim precurrentreading As Double = CDbl(oDBDSHeader.GetValue("U_CurRead", 0).Trim) ' rset2.Fields.Item("U_CurRead").Value  ''''''''Prevouscurrentreading
                                        Dim x As Double = checkreading + precurrentreading
                                        If x = 0 Then
                                            frmActCarriedOut.Items.Item("t_servdue").Specific.value = 0.0
                                        Else
                                            oDBDSHeader.SetValue("U_Service", 0, x)
                                        End If
                                        'Else
                                        '    frmActCarriedOut.Items.Item("t_servdue").Specific.value = 0.0
                                        'End If
                                    End If
                               
                                Case "mtx_1"
                                    Select Case pVal.ColUID
                                        Case "qty", "price"
                                            If pVal.BeforeAction = False And pVal.ItemChanged Then
                                                frmActCarriedOut.Freeze(True)
                                                oMatrix2.FlushToDataSource()
                                                Dim dblValue As Double = CDbl(oDBDSDetail2.GetValue("U_Quantity", pVal.Row - 1)) * CDbl(oDBDSDetail2.GetValue("U_AvgPrice", pVal.Row - 1))
                                                oDBDSDetail2.SetValue("U_Total", pVal.Row - 1, dblValue)
                                                oMatrix2.LoadFromDataSource()
                                                oMatrix2.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                                Me.CalculateGrandTotal()
                                                frmActCarriedOut.Freeze(False)
                                            End If
                                    End Select
                                Case "mtx_2"
                                    Select Case pVal.ColUID
                                        Case "manhrs"
                                            If pVal.BeforeAction = False And pVal.ItemChanged Then
                                                frmActCarriedOut.Freeze(True)
                                                oMatrix3.FlushToDataSource()
                                                Dim dblValue As Double = CDbl(oDBDSDetail3.GetValue("U_ManHrs", pVal.Row - 1)) * CDbl(oDBDSDetail3.GetValue("U_HrCost", pVal.Row - 1))
                                                oDBDSDetail3.SetValue("U_TotCost", pVal.Row - 1, dblValue)
                                                oMatrix3.LoadFromDataSource()
                                                oMatrix3.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                                Me.CalculateGrandTotal()
                                                frmActCarriedOut.Freeze(False)
                                            End If
                                    End Select
                            End Select
                        Catch ex As Exception
                            frmActCarriedOut.Freeze(False)
                            oApplication.StatusBar.SetText("Validate Event  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try

                    Case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT
                        Try
                            Select Case pVal.ItemUID
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "status"
                                            If pVal.BeforeAction = False And pVal.ItemChanged = True And oMatrix1.Columns.Item("status").Cells.Item(pVal.Row).Specific.Selected.Value = "C" Then
                                                If oMatrix1.Columns.Item("actname").Cells.Item(pVal.Row).Specific.String <> "" Then oMatrix1.SetCellWithoutValidation(pVal.Row, "compdat", Now.Date.ToString("yyyyMMdd")) 'oMatrix1.Columns.Item("compdat").Cells.Item(pVal.Row).Specific.String = Now.Date.ToString("yyyyMMdd")
                                                oMatrix1.Columns.Item("remark").Cells.Item(pVal.Row).Click()
                                            End If
                                    End Select
                                Case "c_series"
                                    If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                        'Get the Serial Number Based On Series...
                                        Dim oCmbSerial As SAPbouiCOM.ComboBox = frmActCarriedOut.Items.Item("c_series").Specific
                                        Dim strSerialCode As String = oCmbSerial.Selected.Value
                                        Dim strDocNum As Long = frmActCarriedOut.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                        oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                    End If
                                Case "c_location"
                                    Try
                                        If pVal.BeforeAction = False And pVal.ItemChanged Then
                                            oDBDSHeader.SetValue("U_Type", 0, "")
                                            oDBDSHeader.SetValue("U_MacNo", 0, "")
                                            oDBDSHeader.SetValue("U_MacName", 0, "")
                                            oDBDSHeader.SetValue("U_SchPlanNo", 0, "")
                                            oDBDSHeader.SetValue("U_PMCNo", 0, "")
                                            oDBDSHeader.SetValue("U_PMCName", 0, "")
                                            'oMatrix1.Clear()
                                            'oDBDSDetail1.Clear()
                                            'oGFun.SetNewLine(oMatrix1, oDBDSDetail1, pVal.Row, "actname")
                                            'oMatrix2.Clear()
                                            'oDBDSDetail1.Clear()
                                            'oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                        End If
                                    Catch ex As Exception
                                        oApplication.StatusBar.SetText("Plant Combo Select Event  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    Finally
                                    End Try
                                Case "t_type"
                                    Try
                                        If pVal.BeforeAction = False And pVal.ItemChanged Then
                                            oDBDSHeader.SetValue("U_MacNo", 0, "")
                                            oDBDSHeader.SetValue("U_MacName", 0, "")
                                            oDBDSHeader.SetValue("U_SchPlanNo", 0, "")
                                            oDBDSHeader.SetValue("U_PMCNo", 0, "")
                                            oDBDSHeader.SetValue("U_PMCName", 0, "")
                                        End If
                                    Catch ex As Exception
                                        oApplication.StatusBar.SetText("Type Combo Select Event  Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    Finally
                                    End Try
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If (frmActCarriedOut.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE) Then
                                        'If (frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                        '    Dim HStatus As String = frmActCarriedOut.Items.Item("c_status").Specific.Selected.Description
                                        '    If oDBDSHeader.GetValue("Status", 0) = "C" Or oDBDSHeader.GetValue("Status", 0) = "L" Then
                                        '        If oApplication.MessageBox(HStatus & " a document is irreversible.Document status will be changed to " & HStatus & ".Do you want to Continue?", 1, "Yes", "No") <> 1 Then oDBDSHeader.SetValue("Status", 0, "O") : BubbleEvent = False : Exit Sub
                                        '        frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                                        '    End If
                                        'End If

                                        If Me.ValidateAll() = False Then
                                            System.Media.SystemSounds.Asterisk.Play()
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            BubbleEvent = False
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.ActionSuccess And frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "tab_0"
                                    If pVal.BeforeAction = False Then
                                        frmActCarriedOut.PaneLevel = 1
                                        frmActCarriedOut.Items.Item("tab_0").AffectsFormMode = False
                                        frmActCarriedOut.Settings.MatrixUID = "mtx_0"
                                    End If

                                Case "tab_1"
                                    If pVal.BeforeAction = False Then
                                        frmActCarriedOut.PaneLevel = 2
                                        frmActCarriedOut.Items.Item("tab_1").AffectsFormMode = False
                                        frmActCarriedOut.Settings.MatrixUID = "mtx_1"
                                        Field_Disable()
                                        'oDBDSDetail2.Clear()
                                        If oDBDSHeader.GetValue("Status", 0).Trim = "O" Then
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemcode")
                                        End If

                                    End If
                                Case "tab_2"
                                    If pVal.BeforeAction = False Then
                                        frmActCarriedOut.PaneLevel = 3
                                        frmActCarriedOut.Items.Item("tab_2").AffectsFormMode = False
                                        frmActCarriedOut.Settings.MatrixUID = "mtx_2"
                                        'oDBDSDetail3.Clear()
                                        oGFun.SetNewLine(oMatrix3, oDBDSDetail3, oMatrix3.VisualRowCount, "empname")
                                    End If
                                Case "link_mno"
                                    If pVal.BeforeAction = False Then
                                        Dim ocmb As SAPbouiCOM.ComboBox = frmActCarriedOut.Items.Item("t_type").Specific
                                        If ocmb.Selected.Value = "VH" Then
                                            oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSHeader.GetValue("U_MacNo", 0).Trim)
                                        Else
                                            oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oDBDSHeader.GetValue("U_MacNo", 0).Trim)
                                        End If
                                    End If
                                Case "link_sano"
                                    If pVal.BeforeAction = False Then
                                        oGFun.DoOpenLinkedObjectForm("OACP", "OACP", "t_docnum", oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim)
                                    End If

                                Case "link_chk"
                                    If pVal.BeforeAction = False Then
                                        oGFun.DoOpenLinkedObjectForm("OPCL", "OPCL", "t_code", oDBDSHeader.GetValue("U_PMCNo", 0).Trim)
                                    End If
                                Case "BtnJE"
                                    Dim Flag As Boolean = False
                                    If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        For i As Integer = 1 To oMatrix3.RowCount
                                            If oMatrix3.Columns.Item("empname").Cells.Item(i).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                                                If oMatrix3.Columns.Item("manhrs").Cells.Item(i).Specific.String = "" Or oMatrix3.Columns.Item("hrcost").Cells.Item(i).Specific.String = "" Then
                                                    Flag = True
                                                    oGFun.Msg("Please update the Line Level Data in ManHours Cost Tab...", "S", "E")
                                                End If
                                            End If
                                        Next
                                        If Flag = False Then
                                            If frmActCarriedOut.Items.Item("txtJE").Specific.String = "" Then
                                                JournalEntry()
                                            Else
                                                oGFun.Msg("Journal Entry Already Created...", "S", "E")
                                                Exit Sub
                                            End If
                                        End If

                                    End If
                                Case "BtnGI"
                                    Dim Flag As Boolean = False
                                    If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        If frmActCarriedOut.Items.Item("c_status").Specific.Selected.Value = "O" Then
                                            For i As Integer = 1 To oMatrix2.RowCount
                                                If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                                                    If oMatrix2.Columns.Item("qty").Cells.Item(i).Specific.String = "" Or oMatrix2.Columns.Item("price").Cells.Item(i).Specific.String = "" Or oMatrix2.Columns.Item("dimcode").Cells.Item(i).Specific.String = "" Then
                                                        Flag = True
                                                        oGFun.Msg("Please update the Line Level Data in Repair Parts Tab...", "S", "E")
                                                    End If
                                                End If
                                            Next
                                        End If
                                        If Flag = False Then
                                            'If frmActCarriedOut.Items.Item("txtGI").Specific.String = "" Then
                                            If frmActCarriedOut.Items.Item("c_status").Specific.Selected.Value = "O" Then
                                                GoodsIssue()
                                            Else
                                                cmb_status = frmActCarriedOut.Items.Item("c_status").Specific
                                                oGFun.Msg("Document Status is " & cmb_status.Selected.Description & ".Goods Issue will not be posted...", "S", "E")
                                                Exit Sub
                                            End If
                                            'Else
                                            'oGFun.Msg("Goods Issue Already Posted...", "S", "E")
                                            'Exit Sub
                                            'End If                                         
                                        End If
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Select Case pVal.ItemUID
                            Case "mtx_0"
                                Select Case pVal.ColUID
                                    Case "actname"
                                        If pVal.BeforeAction = False Then
                                            Dim txtval As String = oDBDSDetail1.GetValue("U_ActName", pVal.Row - 1).Trim
                                            oGFun.DoOpenLinkedObjectForm("OACT", "OACT", "txt_actvty", txtval)
                                        End If
                                End Select
                                'Case "mtx_2"
                                '    Select Case pVal.ColUID
                                '        Case "empname"
                                '            If pVal.BeforeAction = True Then
                                '                Dim ColItem As SAPbouiCOM.Column = oMatrix3.Columns.Item("empname")
                                '                Dim objlink As SAPbouiCOM.LinkedButton = ColItem.ExtendedObject
                                '                Try
                                '                    objlink.LinkedObjectType = "171"
                                '                    objlink.Item.LinkTo = "empid"
                                '                Catch ex As Exception

                                '                End Try
                                '            End If
                                '    End Select
                        End Select
                        'Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        '    Try
                        '        Dim strqry As String
                        '        Select Case pVal.ItemUID
                        '            Case "txt_ename"
                        '                If pVal.BeforeAction = False Then
                        '                    ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "EmpCFL", "empID", "select empID from OHEM where dept='13' ")
                        '                End If
                        '            Case "txt_shano"
                        '                If HANA Then
                        '                    SQuery = " SELECT ""DocNum"" FROM ""@MIPL_PM_OACP"" WHERE ""U_MacNo""='" & oDBDSHeader.GetValue("U_MACNo", 0).Trim & "' and ""Status"" = 'O'"
                        '                Else
                        '                    SQuery = " SELECT DocNum FROM [@MIPL_PM_OACP] WHERE U_MacNo='" & oDBDSHeader.GetValue("U_MACNo", 0).Trim & "' and Status = 'O'"
                        '                End If

                        '                oGFun.ChooseFromListFilteration(frmActCarriedOut, "CFLsano", "DocNum", SQuery)
                        '            Case "mtx_1"
                        '                Select Case pVal.ColUID
                        '                    Case "itemcode"
                        '                        If pVal.BeforeAction = False Then
                        '                            '   oGFun.ChooseFromListFilteration(frmActCarriedOut, "ITMCFL", "ItemCode", "Exec [_IND_Sp_PMD_Got_ItemCode]")
                        '                        End If
                        '                End Select
                        '            Case "mtx_2"
                        '                Select Case pVal.ColUID
                        '                    Case "empname"
                        '                        If pVal.BeforeAction = False Then
                        '                            ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "MAN_CFL", "empID", "select empID from OHEM where dept='13' ")
                        '                        End If
                        '                End Select
                        '            Case "txt_shano"
                        '                If pVal.BeforeAction = False Then
                        '                    ' strqry = " SELECT DocNum FROM [@MIPL_PM_OACP] WHERE U_MacNo ='" & oDBDSHeader.GetValue("U_MacNo", 0).Trim & "'"
                        '                    'oGFun.ChooseFromListFilteration(frmActCarriedOut, "CFLsano", "DocNum", strqry)
                        '                End If
                        '            Case "txt_macno"
                        '                If pVal.BeforeAction = False Then
                        '                    Dim oTxt As SAPbouiCOM.EditText = frmActCarriedOut.Items.Item(pVal.ItemUID).Specific
                        '                    If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                        '                        If HANA Then
                        '                            StrQuery = "SELECT ""U_ItemCode""  from ""@MIPL_PM_OVHL"" where ""U_Location""='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "' "
                        '                        Else
                        '                            StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] where U_Location='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "' "
                        '                        End If

                        '                        oTxt.ChooseFromListUID = "OVHL_CFL"
                        '                        oTxt.ChooseFromListAlias = "U_ItemCode"
                        '                        '      oGFun.ChooseFromListFilteration(frmActCarriedOut, "OVHL_CFL", "U_ItemCode", StrQuery)
                        '                    Else
                        '                        If HANA Then
                        '                            StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" Where ""U_InsType""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and ""U_Location""='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "'"
                        '                        Else
                        '                            StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "' and U_Location='" & oDBDSHeader.GetValue("U_Location", 0).Trim & "'"
                        '                        End If

                        '                        oTxt.ChooseFromListUID = "OMAC_CFL"
                        '                        oTxt.ChooseFromListAlias = "Code"
                        '                        ' oGFun.ChooseFromListFilteration(frmActCarriedOut, "OMAC_CFL", "Code", StrQuery)
                        '                    End If
                        '                End If
                        '            Case "t_IndentNo"
                        '                If HANA Then
                        '                    strqry = "Select  A.""U_IndentNo"" from ""@INM_OPTS"" A,""@INM_PTS1"" B where A.""DocEntry""=B.""DocEntry"" and A.""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.""U_rejqty"">0 and B.""U_ItemCode"" Is Not Null And ""DocNum"" Not in (Select ""U_IndentNo"" from ""@INM_ORSE"" A ,""@INM_RSE1"" B where A.""DocEntry""=B.""DocEntry"" Group by ""U_IndentNo"")  Union  Select A.""U_IndentNo"" From ""@INM_OPTS"" A,""@INM_PTS1"" B,(Select ""U_Indentno"",Sum(""U_RejQty"") ""RejectQty"" From ""@INM_ORSE"" A ,""@INM_RSE1"" B  Where A.""DocEntry""=B.""DocEntry"" and ""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By ""U_IndentNo"",""U_Wrkordno"" ) C  Where A.""DocEntry""=B.""DocEntry"" And ""U_WrkOrdNo""='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (""U_rejqty"" - C.""rejectQty"")>0 "
                        '                Else
                        '                    strqry = "Select  A.U_IndentNo from [@INM_OPTS] A,[@INM_PTS1] B where A.DocEntry=B.DocEntry and A.U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and B.U_rejqty>0 and B.U_ItemCode Is Not Null And DocNum Not in (Select U_IndentNo from [@INM_ORSE]A ,[@INM_RSE1] B where A.DocEntry=B.DocEntry Group by U_IndentNo)  Union  Select A.U_IndentNo From [@INM_OPTS] A,[@INM_PTS1] B,(Select U_Indentno,Sum(U_RejQty) RejectQty From [@INM_ORSE] A ,[@INM_RSE1] B  Where A.DocEntry=B.DocEntry and U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "'  Group By U_IndentNo,U_Wrkordno ) C  Where A.DocEntry=B.DocEntry And U_WrkOrdNo='" & oDBDSHeader.GetValue("U_WrkOrdNo", 0) & "' and (U_rejqty - C.rejectQty)>0 "
                        '                End If

                        'oGFun.ChooseFromListFilteration(frmActCarriedOut, "IndentCFL", "DocNum", strqry)

                        '        End Select
                        '                Catch ex As Exception
                        '    oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'Finally
                        'End Try
                End Select
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Item Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub Tool()
        Try
            Dim rsetTool As SAPbobsCOM.Recordset
            Dim cbotype As String = oDBDSHeader.GetValue("U_Type", 0).Trim
            Dim cboPlant As String = oDBDSHeader.GetValue("U_Location", 0).Trim
            Dim cfl As SAPbouiCOM.ChooseFromList
            Dim cons As SAPbouiCOM.Conditions
            Dim con As SAPbouiCOM.Condition
            Dim econ As New SAPbouiCOM.Conditions

            rsetTool = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            cfl = frmActCarriedOut.ChooseFromLists.Item("CFLtno")
            cfl.SetConditions(econ)
            cons = cfl.GetConditions()
            Dim s As String = "SELECT U_plantid,U_type,U_toolno FROM [@PROD_TOOLS_HEAD] WHERE U_plantid='" & cboPlant & "'AND U_type='" & cbotype & "'"
            If HANA Then
                rsetTool.DoQuery("SELECT ""U_plantid"",""U_type"",""U_toolno"" FROM ""@PROD_TOOLS_HEAD"" WHERE ""U_plantid""='" & cboPlant & "'AND ""U_type""='" & cbotype & "'")
            Else
                rsetTool.DoQuery("SELECT U_plantid,U_type,U_toolno FROM [@PROD_TOOLS_HEAD] WHERE U_plantid='" & cboPlant & "'AND U_type='" & cbotype & "'")
            End If

            rsetTool.MoveFirst()
            For i As Integer = 1 To rsetTool.RecordCount
                If i = (rsetTool.RecordCount) Then
                    con = cons.Add()
                    con.Alias = "U_toolno"
                    con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    con.CondVal = Trim(rsetTool.Fields.Item("U_toolno").Value)
                Else
                    con = cons.Add()
                    con.Alias = "U_toolno"
                    con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    con.CondVal = Trim(rsetTool.Fields.Item("U_toolno").Value)
                    con.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                End If
                rsetTool.MoveNext()
            Next
            cfl.SetConditions(cons)
            cfl.SetConditions(econ)
            cfl = frmActCarriedOut.ChooseFromLists.Item("CFLtn")
            cfl.SetConditions(cons)

            'System.Runtime.InteropServices.Marshal.ReleaseComObject(rsetTool)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Tool Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.MenuUID
                    Case "1283"
                        If oApplication.MessageBox("Removal of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                        'oApplication.MessageBox("Successfully removed entry " & oDBDSHeader.GetValue("DocNum", 0).Trim, , "OK")
                    Case "1284" 'Cancel
                        If oApplication.MessageBox("Cancelling of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                    Case "1286" 'Close
                        If oApplication.MessageBox("Closing of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1281" 'Find
                        frmActCarriedOut.Items.Item("c_status").Enabled = True
                        frmActCarriedOut.Items.Item("txtGI").Enabled = True
                        frmActCarriedOut.Items.Item("t_pclname").Enabled = True
                        frmActCarriedOut.Items.Item("t_servdue").Enabled = True
                        frmActCarriedOut.Items.Item("BtnGI").Enabled = False
                        frmActCarriedOut.Items.Item("BtnView").Enabled = False
                        oMatrix1.Item.Enabled = False
                        oMatrix2.Item.Enabled = False
                        oMatrix3.Item.Enabled = False
                    Case "1282"
                        Me.InitForm()
                    Case "1293"
                        Select Case DeleteRowITEMUID
                            Case "mtx_0"
                                oGFun.DeleteRow(oMatrix1, oDBDSDetail1)
                            Case "mtx_1"
                                oGFun.DeleteRow(oMatrix2, oDBDSDetail2)
                            Case "mtx_2"
                                oGFun.DeleteRow(oMatrix3, oDBDSDetail3)
                        End Select
                        Me.CalculateGrandTotal()
                    Case "1287"
                        oGFun.LoadComboBoxSeries(frmActCarriedOut.Items.Item("c_series").Specific, UDOID)
                        oGFun.LoadDocumentDate(frmActCarriedOut.Items.Item("t_docdate").Specific) ' Load Document Date
                        cmb_status = frmActCarriedOut.Items.Item("c_status").Specific
                        cmb_status.Select("O", SAPbouiCOM.BoSearchKey.psk_ByValue)
                        'frmActCarriedOut.Items.Item("c_status").Specific.String = "O"
                        frmActCarriedOut.Items.Item("t_curread").Specific.String = ""
                        frmActCarriedOut.Items.Item("t_servdue").Specific.String = ""
                        frmActCarriedOut.Items.Item("txtGI").Specific.String = ""
                    Case "1284"
                        'If frmActCarriedOut.Items.Item("c_status").Specific.String = "O" Then
                        '    frmActCarriedOut.Items.Item("c_status").Specific.String = "L"
                        '    'If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '    'frmActCarriedOut.Items.Item("1").Click()
                        'End If
                    Case "1292"
                        If Folder2.Selected Then
                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                        End If
                        'oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
                        'oGFun.SetNewLine(oMatrix3, oDBDSDetail3)
                End Select
            End If
           
        Catch ex As Exception
            oApplication.StatusBar.SetText("Menu Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub LoadDetails()
        Try
            'Dim rsetCondt As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim rsetScheduleActivity As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim docid As String = oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim
            Dim clno As String = oDBDSHeader.GetValue("U_PMCNo", 0).Trim

            'rsetCondt.DoQuery("select convert(nvarchar(10),getdate(),111)")
            Dim strGetItems As String
            strGetItems = "SELECT D.U_Schdt,D.LineId,D.U_ActCode,D.U_ActName,D.U_freq,D.U_nxtschdt,D.U_remarks FROM [@FAST_PM_SCHACT_A] D,[@FAST_PM_SCHACT] H WHERE H.DocNum=D.DocEntry AND H.DocNum='" & docid & "'"
            rsetScheduleActivity.DoQuery(strGetItems)
            oMatrix1.AddRow()
            oMatrix1.Clear()
            oDBDSDetail1.Clear()
            Dim j As Integer = 0
            For i As Integer = 0 To rsetScheduleActivity.RecordCount - 1
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = i
                ' oDBDSDetail.SetValue("U_schlinid", oDBDSDetail.Offset, rsetScheduleActivity.Fields.Item("LineId").Value)
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i + 1)
                oDBDSDetail1.SetValue("U_ActCode", i, rsetScheduleActivity.Fields.Item("U_ActCode").Value)
                oDBDSDetail1.SetValue("U_schacty", i, rsetScheduleActivity.Fields.Item("U_ActName").Value)
                oDBDSDetail1.SetValue("U_status", i, "Pending")
                oDBDSDetail1.SetValue("U_freq", i, rsetScheduleActivity.Fields.Item("U_freq").Value)


                Dim schdt As String = CDate(rsetScheduleActivity.Fields.Item("U_nxtschdt").Value).ToString("yyyyMMdd")
                Dim shedate As String = CDate(rsetScheduleActivity.Fields.Item("U_Schdt").Value).ToString("yyyyMMdd")
                oDBDSDetail1.SetValue("U_nxtschdt", i, schdt)
                oDBDSDetail1.SetValue("U_schedate", i, shedate)

                oDBDSDetail1.SetValue("U_actdt", i, schdt)
                oDBDSDetail1.SetValue("U_remarks", i, rsetScheduleActivity.Fields.Item("U_remarks").Value)
                If schdt <> "" Then
                    Dim schdtt As DateTime = DateTime.ParseExact(schdt, "yyyyMMdd", Nothing)
                    Dim freq As String = Trim(rsetScheduleActivity.Fields.Item("U_freq").Value)
                    If freq = "Daily" Then
                        schdtt = schdtt.AddDays(1)
                    ElseIf freq = "Weekly" Then
                        schdtt = schdtt.AddDays(7)
                    ElseIf freq = "Monthly" Then
                        schdt = schdtt.AddMonths(1)
                    ElseIf freq = "Annualy" Then
                        schdtt = schdtt.AddYears(1)
                    ElseIf freq = "Shift" Then
                        schdtt = schdtt
                    ElseIf freq = "Quarterly" Then
                        schdtt = schdtt.AddMonths(3)
                    ElseIf freq = "Half Yearly" Then
                        schdtt = schdtt.AddMonths(6)
                    End If
                    ' oDBDSDetail.SetValue("U_nxtschdt", oDBDSDetail.Offset, schdtt.ToString("yyyyMMdd"))
                    oDBDSDetail1.SetValue("U_actdt", oDBDSDetail1.Offset, schdtt.ToString("yyyyMMdd"))
                End If
                'rsetScheduleActivity.MoveNext()
                If i < rsetScheduleActivity.RecordCount - 1 Then
                    rsetScheduleActivity.MoveNext()
                End If
                j = i + 1
            Next
            'oMatrix1.AddRow()
            'oDBDSDetail1.SetValue("U_schlinid", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_schacty", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_status", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_freq", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_schedate", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_actdt", oDBDSDetail1.Offset, "")
            'oDBDSDetail1.SetValue("U_remarks", oDBDSDetail1.Offset, "")
            '' oMatrix1.SetLineData(j + 1)
            oMatrix1.LoadFromDataSource()
            oMatrix1.AutoResizeColumns()
        Catch ex As Exception
            oApplication.StatusBar.SetText("Scheduled Activity  Choose From List Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try
            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    If BusinessObjectInfo.BeforeAction Then
                        'If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                        If Me.ValidateAll() = False Then
                            System.Media.SystemSounds.Asterisk.Play()
                            'If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            BubbleEvent = False
                            Exit Sub
                            'Else
                            '    'If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        End If
                        'End If                       
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix1, "actname", oDBDSDetail1)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix2, "itemcode", oDBDSDetail2)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix3, "empname", oDBDSDetail3)
                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        If Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") = False Then
                            If HANA Then
                                SQuery = "UPDATE ""@MIPL_PM_OACP"" SET  ""Status"" = 'C' WHERE ""Status"" = 'O' AND ""DocNum"" = '" & Trim(oDBDSHeader.GetValue("U_SchPlanNo", 0)) & "' "
                            Else
                                SQuery = "UPDATE [@MIPL_PM_OACP] SET  Status = 'C' WHERE Status = 'O' AND DocNum = '" & Trim(oDBDSHeader.GetValue("U_SchPlanNo", 0)) & "' "
                            End If
                            Dim rsetUpdate As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                            oGFun.oApplication.StatusBar.SetText("Maintenance Plan status has been closed...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If

                    End If
                    'Dim GREntry As String = frmActCarriedOut.DataSources.DBDataSources.Item("@MIPL_PM_OACO").GetValue("DocEntry", 0)
                    'MsgBox(GREntry)
                    'If BusinessObjectInfo.ActionSuccess Then
                    '    If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                    '        'Shedule Date Update
                    '        Dim rsetUpdate As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    '        'Getting the frequency
                    '        Dim SchDateUpdate As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    '        Dim SchDateQry As String
                    '        If HANA Then
                    '            SchDateQry = "select ""U_Freqncy"" from ""@MIPL_PM_OPCL"" where ""Code"" in (select ""U_PMCNo""  from ""@MIPL_PM_OACO"" where ""DocNum""='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "')"
                    '        Else
                    '            SchDateQry = "select U_Freqncy from [@MIPL_PM_OPCL] where Code in (select U_PMCNo  from [@MIPL_PM_OACO] where DocNum='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "')"
                    '        End If
                    '        SchDateUpdate.DoQuery(SchDateQry)
                    '        Dim updatQry As String = ""
                    '        If SchDateUpdate.Fields.Item(0).Value = "1" Then
                    '            If HANA Then
                    '                updatQry = "update ""@MIPL_PM_OACP"" set ""U_SchedDt""=Add_days(Cast(Current_Date as timestamp),7) where ""DocNum""='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            Else
                    '                updatQry = "update [@MIPL_PM_OACP] set U_SchedDt=DATEADD(Day,7,GETDATE()) where DocNum='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            End If
                    '        ElseIf SchDateUpdate.Fields.Item(0).Value = "2" Then
                    '            If HANA Then
                    '                updatQry = "update ""@MIPL_PM_OACP"" set ""U_SchedDt""=Add_Months(Cast(Current_Date as timestamp),1) where ""DocNum""='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            Else
                    '                updatQry = "update [@MIPL_PM_OACP] set U_SchedDt=DATEADD(MONTH,1,GETDATE()) where DocNum='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            End If
                    '        ElseIf SchDateUpdate.Fields.Item(0).Value = "3" Then
                    '            If HANA Then
                    '                updatQry = "update ""@MIPL_PM_OACP"" set ""U_SchedDt""=Add_Months(Cast(Current_Date as timestamp),3) where ""DocNum""='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            Else
                    '                updatQry = "update [@MIPL_PM_OACP] set U_SchedDt=DATEADD(MONTH,3,GETDATE()) where DocNum='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            End If

                    '        ElseIf SchDateUpdate.Fields.Item(0).Value = "4" Then
                    '            If HANA Then
                    '                updatQry = "update ""@MIPL_PM_OACP"" set ""U_SchedDt""=Add_Months(Cast(Current_Date as timestamp),6) where ""DocNum""='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            Else
                    '                updatQry = "update [@MIPL_PM_OACP] set U_SchedDt=DATEADD(MONTH,6,GETDATE()) where DocNum='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            End If

                    '        ElseIf SchDateUpdate.Fields.Item(0).Value = "5" Then
                    '            If HANA Then
                    '                updatQry = "update ""@MIPL_PM_OACP"" set ""U_SchedDt""=Add_Years(Cast(Current_Date as timestamp),1) where ""DocNum""='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            Else
                    '                updatQry = "update [@MIPL_PM_OACP] set U_SchedDt=DATEADD(YEAR,1,GETDATE()) where DocNum='" & oDBDSHeader.GetValue("U_SchPlanNo", 0).Trim & "'"
                    '            End If
                    '        Else
                    '            updatQry = ""
                    '        End If
                    '        If updatQry <> "" Then
                    '            rsetUpdate.DoQuery(updatQry)
                    '        End If

                    '    End If
                    '    ' Update Status...
                    '    'Dim updat As Boolean = True
                    '    'For i As Integer = 0 To oMatrix1.VisualRowCount - 1
                    '    '    If oDBDSDetail1.GetValue("U_Status", i).Trim = "P" Then
                    '    '        updat = False
                    '    '    End If
                    '    'Next
                    '    'If updat = True Then
                    '    '    Dim updt As SAPbobsCOM.Recordset
                    '    '    If HANA Then
                    '    '        updt = oGFun.DoQuery("Update ""@MIPL_PM_OACO"" set ""Status""='C' where ""DocNum""='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "'")
                    '    '    Else
                    '    '        updt = oGFun.DoQuery("Update [@MIPL_PM_OACO] set Status='C' where DocNum='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "'")
                    '    '    End If
                    '    'End If
                    'End If

                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                        'Dim DocNum As String = oDBDSHeader.GetValue("DocNum", 0)
                        'Dim rsetStatus As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        'rsetStatus.DoQuery("select canceled,status from [@MIPL_PM_OACO] where DocNum='" & Trim(DocNum) & "'")
                        'If (rsetStatus.Fields.Item(0).Value = "Y" And rsetStatus.Fields.Item(1).Value = "C") Then
                        '    oDBDSHeader.SetValue("Status", 0, "Canceled")
                        '    frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        'ElseIf (rsetStatus.Fields.Item(0).Value = "N" And rsetStatus.Fields.Item(1).Value = "C") Then
                        '    oDBDSHeader.SetValue("Status", 0, "Closed")
                        '    frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        'Else
                        '    oDBDSHeader.SetValue("Status", 0, "Open")
                        '    frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        'End If
                        If Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") = False Then
                            If HANA Then
                                SQuery = "UPDATE ""@MIPL_PM_OACP"" SET  ""Status"" = 'C' WHERE ""Status"" = 'O' AND ""DocNum"" = '" & Trim(oDBDSHeader.GetValue("U_SchPlanNo", 0)) & "' "
                            Else
                                SQuery = "UPDATE [@MIPL_PM_OACP] SET  Status = 'C' WHERE Status = 'O' AND DocNum = '" & Trim(oDBDSHeader.GetValue("U_SchPlanNo", 0)) & "' "
                            End If
                            Dim rsetUpdate As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                            'oGFun.oApplication.StatusBar.SetText("Maintenance Plan status has been closed...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                        Dim EntryFlag As Boolean = False
                        Dim ActualEntry As String = ""
                        For i As Integer = 1 To oMatrix2.VisualRowCount
                            If oMatrix2.Columns.Item("GIEntry").Cells.Item(i).Specific.String <> "" Then
                                If HANA Then
                                    ActualEntry = oGFun.getSingleValue("Select T1.""DocEntry"" from ODRF T0 inner join OIGE T1 on T1.""ObjType""=T0.""ObjType"" and T0.""DocEntry""=T1.""draftKey"" where ifnull(T0.""DocStatus"",'')='C' and T1.""draftKey""='" & oMatrix2.Columns.Item("GIEntry").Cells.Item(i).Specific.String & "'")
                                Else
                                    ActualEntry = oGFun.getSingleValue("Select T1.DocEntry from ODRF T0 inner join OIGE T1 on T1.ObjType=T0.ObjType and T0.DocEntry=T1.draftKey where isnull(T0.DocStatus,'')='C' and T1.draftKey='" & oMatrix2.Columns.Item("GIEntry").Cells.Item(i).Specific.String & "'")
                                End If
                                If ActualEntry <> "" Then
                                    oMatrix2.Columns.Item("GIEntry").Cells.Item(i).Specific.String = ActualEntry
                                    EntryFlag = True
                                End If
                            End If
                        Next
                        If EntryFlag Then If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE : frmActCarriedOut.Items.Item("1").Click()
                        Field_Disable()
                        'If oDBDSHeader.GetValue("U_GINo", 0).Trim = "" Then
                        '    'frmActCarriedOut.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_Default)
                        '    'oDBDSHeader.SetValue("Status", 0, "O")
                        'Else
                        '    'frmActCarriedOut.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                        '    Dim ActualEntry As String = ""
                        '    If HANA Then
                        '        ActualEntry = oGFun.getSingleValue("Select T1.""DocEntry"" from ODRF T0 inner join OIGE T1 on T1.""ObjType""=T0.""ObjType"" and T0.""DocEntry""=T1.""draftKey"" where ifnull(T0.""DocStatus"",'')='C' and T1.""draftKey""='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                        '    Else
                        '        ActualEntry = oGFun.getSingleValue("Select T1.DocEntry from ODRF T0 inner join OIGE T1 on T1.ObjType=T0.ObjType and T0.DocEntry=T1.draftKey where isnull(T0.DocStatus,'')='C' and T1.draftKey='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                        '    End If
                        '    If ActualEntry <> "" Then
                        '        oDBDSHeader.SetValue("U_GINo", 0, ActualEntry)
                        '    End If
                        'End If
                        If oDBDSHeader.GetValue("Status", 0).Trim = "C" Or oDBDSHeader.GetValue("Status", 0).Trim.Equals("L") Then
                            'frmActCarriedOut.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        Else
                            frmActCarriedOut.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_Default)
                            frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        End If
                        frmActCarriedOut.Items.Item("BtnView").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        frmActCarriedOut.EnableMenu("1282", True)
                    End If
                    oDBDSDetail1.Clear()
                    oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, "actname")
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
                    Dim Matrix3, Matrix1, Matrix2 As SAPbouiCOM.Matrix
                    Matrix1 = frmActCarriedOut.Items.Item("mtx_0").Specific
                    Matrix2 = frmActCarriedOut.Items.Item("mtx_1").Specific
                    Matrix3 = frmActCarriedOut.Items.Item("mtx_2").Specific
                    If EventInfo.BeforeAction = True Then
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmActCarriedOut.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        frmActCarriedOut.EnableMenu("1284", False)
                        frmActCarriedOut.EnableMenu("1285", False)
                        frmActCarriedOut.EnableMenu("1286", False)
                        If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE Or frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmActCarriedOut.EnableMenu("1287", True)  'Duplicate
                            'frmActCarriedOut.EnableMenu("1283", True)
                        Else
                            frmActCarriedOut.EnableMenu("1287", False)
                        End If
                        If Folder2.Selected = True Then
                            Try
                                If EventInfo.ColUID = "lineid" Then
                                    If Matrix2.Columns.Item("itemcode").Cells.Item(EventInfo.Row).Specific.String <> "" Then
                                        frmActCarriedOut.EnableMenu("1292", True) 'Add Row Menu
                                    End If
                                End If

                                If Matrix2.Columns.Item("itemcode").Cells.Item(EventInfo.Row).Specific.String <> "" And Matrix2.Columns.Item("GIEntry").Cells.Item(EventInfo.Row).Specific.String <> "" Then
                                    frmActCarriedOut.EnableMenu("1293", False) 'Remove Row Menu
                                Else
                                    frmActCarriedOut.EnableMenu("1293", True) 'Remove Row Menu
                                End If
                            Catch ex As Exception
                            End Try

                        End If
                        If EventInfo.ItemUID = "mtx_0" Then
                            frmActCarriedOut.EnableMenu("1293", False)

                        ElseIf EventInfo.ItemUID = "mtx_1" Then
                            If EventInfo.Row = oMatrix1.VisualRowCount Then
                                frmActCarriedOut.EnableMenu("1293", False)
                            Else
                                frmActCarriedOut.EnableMenu("1293", True)
                            End If
                        ElseIf EventInfo.ItemUID = "mtx_2" Then
                            If EventInfo.Row = oMatrix1.VisualRowCount Then
                                frmActCarriedOut.EnableMenu("1293", False)
                            Else
                                frmActCarriedOut.EnableMenu("1293", True)
                            End If
                        Else
                            frmActCarriedOut.EnableMenu("1293", False) 'Remove Row Menu
                        End If
                        Dim aa As String = Trim(oDBDSHeader.GetValue("U_GINo", 0))
                        If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Trim(oDBDSHeader.GetValue("U_GINo", 0)) = "" Then
                                frmActCarriedOut.EnableMenu("1283", True) 'Remove
                                'frmActCarriedOut.EnableMenu("1286", True) 'Close
                                frmActCarriedOut.EnableMenu("1284", True) 'Cancel
                            End If
                            frmActCarriedOut.EnableMenu("1286", True) 'Close
                        End If
                    Else
                        frmActCarriedOut.EnableMenu("1292", False)
                        frmActCarriedOut.EnableMenu("1293", False) 'Remove Row Menu
                        frmActCarriedOut.EnableMenu("1283", False) 'Remove
                        frmActCarriedOut.EnableMenu("1286", False) 'Close
                        frmActCarriedOut.EnableMenu("1284", False) 'Cancel
                    End If
                   
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

    Sub CalculateGrandTotal()
        Try
            Dim dblActCost = 0, dblSpareCost As Double = 0
            frmActCarriedOut.Freeze(True)
            oMatrix2.FlushToDataSource()
            For i As Integer = 1 To oMatrix2.VisualRowCount
                dblActCost += CDbl(oDBDSDetail2.GetValue("U_Total", i - 1))
            Next

            oMatrix3.FlushToDataSource()
            For j As Integer = 1 To oMatrix3.VisualRowCount
                dblSpareCost += CDbl(oDBDSDetail3.GetValue("U_TotCost", j - 1))
            Next
            oDBDSHeader.SetValue("U_GrandTot", 0, dblActCost + dblSpareCost)
            frmActCarriedOut.Freeze(False)
        Catch ex As Exception
            frmActCarriedOut.Freeze(False)
            oGFun.StatusBarErrorMsg(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub Field_Disable()
        Try
            frmActCarriedOut.Freeze(True)
            If Folder2.Selected Then
                For i As Integer = 1 To oMatrix2.VisualRowCount
                    If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.string <> "" And oMatrix2.Columns.Item("GIEntry").Cells.Item(i).Specific.string <> "" Then
                        If oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.string = "O" Or oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.string = "C" Then
                            oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.string = "C"
                            oMatrix2.CommonSetting.SetCellEditable(i, 1, False)
                            oMatrix2.CommonSetting.SetCellEditable(i, 4, False)
                            oMatrix2.CommonSetting.SetCellEditable(i, 5, False)
                            oMatrix2.CommonSetting.SetCellEditable(i, 7, False)
                            oMatrix2.CommonSetting.SetCellEditable(i, 12, False)
                        End If
                    Else
                        oMatrix2.CommonSetting.SetCellEditable(i, 1, True)
                        oMatrix2.CommonSetting.SetCellEditable(i, 4, True)
                        oMatrix2.CommonSetting.SetCellEditable(i, 5, True)
                        oMatrix2.CommonSetting.SetCellEditable(i, 7, True)
                        oMatrix2.CommonSetting.SetCellEditable(i, 12, True)
                    End If
                Next
            End If
            frmActCarriedOut.Freeze(False)
        Catch ex As Exception
            frmActCarriedOut.Freeze(False)
        End Try
    End Sub

    Private Sub GoodsIssue()
        Dim objGoodsIssue As SAPbobsCOM.Documents
        Dim Quantity As Double
        Dim Retval As Integer
        Dim StrSql, Branch, Batchs, Serial, GINo, WhsCode As String
        Dim objrs As SAPbobsCOM.Recordset
        Dim Flag As Boolean = False
        objGoodsIssue = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
        objrs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        For i As Integer = 1 To oMatrix2.VisualRowCount
            If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                If oMatrix2.Columns.Item("qty").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("price").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("dimcode").Cells.Item(i).Specific.String <> "" Then
                    Flag = True
                End If
            End If
        Next
        If Flag = False Then oGFun.Msg("No more Data for posting the Goods Issue ...", "S", "E") : Exit Sub
        oGFun.oApplication.StatusBar.SetText("Goods Issue Creating Please wait...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Try
            If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            'AcctCode = oGFun.getSingleValue("select Top 1 ""U_GIGL"" from ""@MIPL_PM_GL"" where  ""Code""<>''")
            Dim oCmbType As SAPbouiCOM.ComboBox = frmActCarriedOut.Items.Item("t_type").Specific
            If oCmbType.Selected.Value = "VH" Then
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                End If
            Else
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_MACNo", 0) & "'")
                End If
            End If

            'If AcctCode = "" Then oGFun.oApplication.StatusBar.SetText("Please update the Accountcode in GL Mapping Details UDT...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            If WhsCode = "" Then oGFun.oApplication.StatusBar.SetText("Warehouse Code is missing for the specified entry in Machine Master...Please check", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            If Not oCompany.InTransaction Then oCompany.StartTransaction()
            frmActCarriedOut.Freeze(True)
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmActCarriedOut.Items.Item("t_docdate").Specific
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            objGoodsIssue.DocDate = DocDate  'Now.Date.ToString("yyyyMMdd") 
            ' objGoodsIssue.DocDueDate = DocDueDate
            objGoodsIssue.TaxDate = DocDate 'Now.Date.ToString("yyyyMMdd") 'TaxDate 'oCompany.GetCompanyDate
            If HANA Then
                Branch = oGFun.getSingleValue("select ""BPLId"" from OBPL where ifnull(""MainBPL"",'Y')='Y'")
            Else
                Branch = oGFun.getSingleValue("select BPLId from OBPL where isnull(MainBPL,'Y')='Y'")
            End If

            If Branch <> "" Then
                objGoodsIssue.BPL_IDAssignedToInvoice = Branch
            End If
            objGoodsIssue.Comments = "AutoGen thro' PM Addon-Maintenance Carriedout Posted on ->" & Now.ToString 'frmActCarriedOut.Items.Item("txtremark").Specific.string 
            objGoodsIssue.UserFields.Fields.Item("U_MCOutNo").Value = frmActCarriedOut.Items.Item("t_docnum").Specific.string
            For i As Integer = 1 To oMatrix2.VisualRowCount
                If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                    Quantity = oMatrix2.Columns.Item("qty").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.ItemCode = oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.UnitPrice = oMatrix2.Columns.Item("price").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.Quantity = Quantity
                    objGoodsIssue.Lines.WarehouseCode = WhsCode 'oMatrix1.Columns.Item("SubWhse").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.CostingCode = oMatrix2.Columns.Item("dimcode").Cells.Item(i).Specific.String
                    'objGoodsIssue.Lines.AccountCode = AcctCode

                    If HANA Then
                        Batchs = oGFun.getSingleValue("select ""ManBtchNum"" from OITM WHERE ""ItemCode""='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "'")
                        Serial = oGFun.getSingleValue("select ""ManSerNum"" from OITM WHERE ""ItemCode""='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "'")
                    Else
                        Batchs = oGFun.getSingleValue("select ManBtchNum from OITM WHERE ItemCode='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "'")
                        Serial = oGFun.getSingleValue("select ManSerNum from OITM WHERE ItemCode='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "'")
                    End If
                    If Batchs = "Y" And Serial = "N" Then
                        StrSql = "SELECT distinct I1.""BatchNum"" ""BatchSerial"", T4.""Quantity"",I1.""DocDate"""
                        StrSql += vbCrLf + " from IBT1 I1 join OIBT T4 on T4.""ItemCode""=I1.""ItemCode"" and I1.""BatchNum""=T4.""BatchNum"" and I1.""WhsCode"" = T4.""WhsCode"""
                        StrSql += vbCrLf + " where T4.""Quantity"">0 and I1.""ItemCode""='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "' and I1.""WhsCode""='" & WhsCode & "' order by I1.""DocDate"" "
                        objrs.DoQuery(StrSql)
                        Dim BQty As Double = 0, PendQty As Double = 0
                        BQty = Quantity '11
                        If objrs.RecordCount > 0 Then
                            If CDbl(objrs.Fields.Item("Quantity").Value) - BQty > 0 Then
                                PendQty = BQty
                            ElseIf CDbl(objrs.Fields.Item("Quantity").Value) - BQty < 0 Then
                                PendQty = CDbl(objrs.Fields.Item("Quantity").Value)
                            ElseIf CDbl(objrs.Fields.Item("Quantity").Value) - BQty = 0 Then
                                PendQty = Quantity
                            End If
                            For j As Integer = 0 To objrs.RecordCount - 1
                                objGoodsIssue.Lines.BatchNumbers.BatchNumber = CStr(objrs.Fields.Item("BatchSerial").Value)
                                objGoodsIssue.Lines.BatchNumbers.Quantity = PendQty
                                objGoodsIssue.Lines.BatchNumbers.Add()
                                If Quantity - PendQty > 0 Then
                                    BQty = Quantity - CDbl(objrs.Fields.Item("Quantity").Value) '1
                                    If BQty > 0 Then
                                        BQty = BQty
                                        If BQty <= 0 Then
                                            Exit For
                                        End If
                                        objrs.MoveNext()
                                        PendQty = Quantity - PendQty
                                    Else
                                        Exit For
                                    End If
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                        'If objrs.RecordCount > 0 Then
                        '    For j As Integer = 0 To objrs.RecordCount - 1
                        '        'objGoodsIssue.Lines.BatchNumbers.SetCurrentLine(j)
                        '        objGoodsIssue.Lines.BatchNumbers.BatchNumber = CStr(objrs.Fields.Item("BatchSerial").Value)
                        '        objGoodsIssue.Lines.BatchNumbers.Quantity = Quantity ' CDbl(objrs.Fields.Item("Quantity").Value) 'Quantity
                        '        objGoodsIssue.Lines.BatchNumbers.Add()
                        '        BQty = BQty - CDbl(objrs.Fields.Item("Quantity").Value)
                        '        If BQty > 0 Then
                        '            BQty = BQty
                        '            If BQty <= 0 Then
                        '                Exit For
                        '            End If
                        '            objrs.MoveNext()
                        '        Else
                        '            Exit For
                        '        End If
                        '    Next
                        'End If
                    ElseIf Batchs = "N" And Serial = "Y" Then
                        StrSql = "SELECT distinct T4.""IntrSerial"" ""BatchSerial"", T4.""Quantity"",I1.""DocDate"""
                        StrSql += vbCrLf + " from SRI1 I1 join OSRI T4 on T4.""ItemCode""=I1.""ItemCode"" and I1.""SysSerial""=T4.""SysSerial"" and I1.""WhsCode"" = T4.""WhsCode"""
                        StrSql += vbCrLf + " where T4.""Quantity"">0 and I1.""ItemCode""='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "' and I1.""WhsCode""='" & WhsCode & "' order by I1.""DocDate"" "
                        objrs.DoQuery(StrSql)
                        Dim SQty As Double = 0
                        SQty = Quantity '4
                        If objrs.RecordCount > 0 Then
                            For j As Integer = 0 To objrs.RecordCount - 1
                                'objGoodsIssue.Lines.SerialNumbers.SetCurrentLine(j)
                                objGoodsIssue.Lines.SerialNumbers.InternalSerialNumber = CStr(objrs.Fields.Item("BatchSerial").Value)
                                objGoodsIssue.Lines.SerialNumbers.Quantity = CDbl(1)
                                objGoodsIssue.Lines.SerialNumbers.Add()
                                SQty = SQty - CDbl(1) ' CDbl(objrs.Fields.Item("Quantity").Value)
                                If SQty > 0 Then
                                    SQty = SQty
                                    If SQty <= 0 Then
                                        Exit For
                                    End If
                                    objrs.MoveNext()
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                    End If
                    objGoodsIssue.Lines.Add()
                End If

            Next i

            Retval = objGoodsIssue.Add
            If Retval <> 0 Then
                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                oGFun.oApplication.StatusBar.SetText("GoodsIssue: " & oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Else
                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                GINo = oCompany.GetNewObjectKey()
                If frmActCarriedOut.Items.Item("txtGI").Specific.String = "" Then
                    frmActCarriedOut.Items.Item("txtGI").Specific.String = GINo
                Else
                    frmActCarriedOut.Items.Item("txtGI").Specific.String = frmActCarriedOut.Items.Item("txtGI").Specific.String & "," & GINo
                End If

                For j = 1 To oMatrix2.RowCount
                    If oMatrix2.Columns.Item("itemcode").Cells.Item(j).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                        oMatrix2.Columns.Item("Stat").Cells.Item(j).Specific.String = "C"
                        oMatrix2.Columns.Item("GIEntry").Cells.Item(j).Specific.String = GINo
                        oMatrix2.CommonSetting.SetRowEditable(j, False)
                    End If
                Next
                If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    frmActCarriedOut.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                End If
                oGFun.oApplication.StatusBar.SetText("Goods Issue Created Successfully...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            End If
            frmActCarriedOut.Freeze(False)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objGoodsIssue)
            GC.Collect()
            objrs = Nothing
        Catch ex As Exception
            frmActCarriedOut.Freeze(False)
            If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            oGFun.oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub JournalEntry()
        Try
            Dim DocEntry, Branch As String, BranchCode As String = ""
            Dim objrecset As SAPbobsCOM.Recordset
            Dim objjournalentry As SAPbobsCOM.JournalEntries
            objjournalentry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            If oMatrix3.Columns.Item("empname").Cells.Item(1).Specific.String = "" Then
                oGFun.oApplication.SetStatusBarMessage("Please update the data in ManHours Cost Tab...", SAPbouiCOM.BoMessageTime.bmt_Medium, True) : Exit Sub
            End If
            If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            objrecset = oGFun.DoQuery("Select Top 1 ""U_CreditGL"",""U_DebitGL"" from ""@MIPL_PM_GL"" where ""Code""<>'' ")
            If objrecset.RecordCount = 0 Then oGFun.oApplication.StatusBar.SetText("Please update the GL UDT...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            oGFun.oApplication.StatusBar.SetText("Journal Entry Creating Please wait...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            If Not oCompany.InTransaction Then oCompany.StartTransaction()
            'Dim Series As String = ""
            'Series = objGM.GetSeries("30", CDate(dtst1.Tables(Header).Rows(0)("RefDate")).ToString("yyyy-MM-dd"), Branch)
            'objjournalentry.Series = Series
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmActCarriedOut.Items.Item("t_docdate").Specific
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            objjournalentry.ReferenceDate = DocDate 'Now.Date.ToString("yyyyMMdd") 
            'objjournalentry.DueDate = Now.Date.ToString("yyyyMMdd") 'DocDate
            objjournalentry.TaxDate = DocDate 'Now.Date.ToString("yyyyMMdd") 
            objjournalentry.UserFields.Fields.Item("U_MCOutNo").Value = frmActCarriedOut.Items.Item("t_docnum").Specific.string
            objjournalentry.Memo = "Maintenance CarriedOut"
            objjournalentry.Reference = "Auto Posted through PM CarriedOut: " & Now.ToString

            For j = 1 To oMatrix3.RowCount
                If oMatrix3.Columns.Item("empname").Cells.Item(j).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                    If HANA Then
                        Branch = oGFun.getSingleValue("select ""MltpBrnchs"" from OADM")
                    Else
                        Branch = oGFun.getSingleValue("select MltpBrnchs from OADM")
                    End If

                    If Branch = "Y" Then
                        If HANA Then
                            BranchCode = oGFun.getSingleValue("select ""BPLId"" from OBPL where ifnull(""MainBPL"",'Y')='Y'")
                        Else
                            BranchCode = oGFun.getSingleValue("select BPLId from OBPL where isnull(MainBPL,'Y')='Y'")
                        End If
                    End If
                    If CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String) <> 0 Then
                        'objjournalentry.Lines.ShortName = "C0001"
                        objjournalentry.Lines.AccountCode = objrecset.Fields.Item("U_CreditGL").Value
                        objjournalentry.Lines.Credit = CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String)
                        objjournalentry.Lines.Debit = 0
                        objjournalentry.Lines.BPLID = BranchCode
                        objjournalentry.Lines.Add()
                        'objjournalentry.Lines.ShortName = "113000"
                        objjournalentry.Lines.AccountCode = objrecset.Fields.Item("U_DebitGL").Value
                        objjournalentry.Lines.Debit = CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String)
                        objjournalentry.Lines.Credit = 0
                        objjournalentry.Lines.BPLID = BranchCode
                        objjournalentry.Lines.Add()
                    End If
                    'If Matrix4.Columns.Item("CostCent").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.CostingCode = Matrix4.Columns.Item("CostCent").Cells.Item(j).Specific.String
                    'If Matrix4.Columns.Item("CostCent1").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.CostingCode2 = Matrix4.Columns.Item("CostCent1").Cells.Item(j).Specific.String
                    'If Matrix4.Columns.Item("CostCent2").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.CostingCode3 = Matrix4.Columns.Item("CostCent2").Cells.Item(j).Specific.String
                    'If Matrix4.Columns.Item("CostCent3").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.CostingCode4 = Matrix4.Columns.Item("CostCent3").Cells.Item(j).Specific.String
                    'If Matrix4.Columns.Item("CostCent4").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.CostingCode5 = Matrix4.Columns.Item("CostCent4").Cells.Item(j).Specific.String
                    'If Matrix4.Columns.Item("Proj").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.ProjectCode = Matrix4.Columns.Item("Proj").Cells.Item(j).Specific.String
                    'If oMatrix1.Columns.Item("Remarks").Cells.Item(j).Specific.String <> "" Then objjournalentry.Lines.Reference1 = oMatrix1.Columns.Item("Remarks").Cells.Item(j).Specific.String
                    'objjournalentry.Lines.LocationCode = ""
                End If
            Next

            If objjournalentry.Add <> 0 Then
                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                oGFun.oApplication.SetStatusBarMessage("Journal: " & oCompany.GetLastErrorDescription & "-" & oCompany.GetLastErrorCode, SAPbouiCOM.BoMessageTime.bmt_Medium, True)
            Else
                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                DocEntry = oCompany.GetNewObjectKey()
                oGFun.oApplication.SetStatusBarMessage("Journal Entry Created Successfully...", SAPbouiCOM.BoMessageTime.bmt_Medium, False)
                frmActCarriedOut.Items.Item("txtJE").Specific.String = DocEntry
                For j = 1 To oMatrix3.RowCount
                    If oMatrix3.Columns.Item("empname").Cells.Item(j).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                        oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "C"
                        'oMatrix3.CommonSetting.SetRowEditable(j, False)
                    End If
                Next
                If frmActCarriedOut.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    frmActCarriedOut.Items.Item("1").Click()
                End If
            End If
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objjournalentry)
            objrecset = Nothing
        Catch ex As Exception
            If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            oGFun.oApplication.SetStatusBarMessage("JE Posting Error" & oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Medium, True)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Public Sub LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean)
        Try
            frmActCarriedOut = oGFun.oApplication.Forms.Item(eventInfo.FormUID)
            'eventInfo.LayoutKey = frmActCarriedOut.Items.Item("t_docnum").Specific.string
            eventInfo.LayoutKey = frmActCarriedOut.DataSources.DBDataSources.Item("@MIPL_PM_OACO").GetValue("DocEntry", 0)
        Catch ex As Exception
        End Try
        
    End Sub


End Class