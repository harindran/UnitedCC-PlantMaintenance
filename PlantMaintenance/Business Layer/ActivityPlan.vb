Class ActivityPlan

    Dim frmActivityPlan As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oDBDSDetail1, oDBDSDetail2 As SAPbouiCOM.DBDataSource
    Dim oMatrix1, oMatrix2 As SAPbouiCOM.Matrix
    Dim UDOID As String = "OACP"
    Dim canno As String
    Dim DeleteRowITEMUID As String = ""
    Dim bool As Boolean = False
    Dim StrQuery As String = ""
    Dim SQuery As String = ""
    Sub LoadActivityPlan()
        Try
            oGFun.LoadXML(frmActivityPlan, ActivityPlanFormID, ActivityPlanXML)
            frmActivityPlan = oApplication.Forms.Item(ActivityPlanFormID)
            setReport(ActivityPlanFormID)
            oDBDSHeader = frmActivityPlan.DataSources.DBDataSources.Item(0)
            oDBDSDetail1 = frmActivityPlan.DataSources.DBDataSources.Item(1)
            oDBDSDetail2 = frmActivityPlan.DataSources.DBDataSources.Item(2)

            oMatrix1 = frmActivityPlan.Items.Item("mtx_0").Specific
            oMatrix2 = frmActivityPlan.Items.Item("mtx_1").Specific

            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmActivityPlan.Freeze(True)
            frmActivityPlan.PaneLevel = 1
            oGFun.LoadComboBoxSeries(frmActivityPlan.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmActivityPlan.Items.Item("t_docdate").Specific) ' Load Document Date
            If HANA Then
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("cmb_dept").Specific, "Select ""Code"", ""Name"" from OUDP")
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("c_location").Specific, "Select ""Code"", ""Location"" from OLCT")
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("c_type").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")

            Else
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("cmb_dept").Specific, "Select Code, Name from OUDP") 'Load Department)
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("c_location").Specific, "Select Code, Location from OLCT") 'Load Location)
                oGFun.setComboBoxValue(frmActivityPlan.Items.Item("c_type").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If

            'oGFun.setComboBoxValue(frmActivityPlan.Items.Item("c_Shift").Specific, "Select Code,U_SftType from [@INM_OSFT]") ' Load the location Combo Box...
            frmActivityPlan.ActiveItem = "c_location"
            oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)

            frmActivityPlan.Freeze(False)

        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmActivityPlan.Freeze(False)
        Finally
        End Try
    End Sub

    Private Sub setReport(ByVal FormUID As String)
        Try
            frmActivityPlan = oApplication.Forms.Item(FormUID)
            Dim rptTypeService As SAPbobsCOM.ReportTypesService
            'Dim newType As SAPbobsCOM.ReportType
            Dim newtypesParam As SAPbobsCOM.ReportTypesParams
            rptTypeService = oCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)
            newtypesParam = rptTypeService.GetReportTypeList
            Dim TypeCode As String
            If HANA Then
                TypeCode = oGFun.getSingleValue("Select ""CODE"" from RTYP where ""NAME""='ActivityPlan'")
            Else
                TypeCode = oGFun.getSingleValue("Select CODE from RTYP where NAME='ActivityPlan'")
            End If
            frmActivityPlan.ReportType = TypeCode
            'Dim i As Integer
            'For i = 0 To newtypesParam.Count - 1
            '    If newtypesParam.Item(i).TypeName = "ActivityPlan" And newtypesParam.Item(i).MenuID = "ActivityPlan" Then
            '        frmActivityPlan.ReportType = newtypesParam.Item(i).TypeCode
            '        Exit For
            '    End If
            'Next i
        Catch ex As Exception
            oApplication.StatusBar.SetText("setReport Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmActivityPlan.Visible = True
            frmActivityPlan.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("cmb_dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("cmb_dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("c_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActivityPlan.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmActivityPlan.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmActivityPlan.Items.Item("txt_mdesc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean
        Try
            If oDBDSHeader.GetValue("U_macno", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Machine Number Should Not Be Left Empty")
                frmActivityPlan.ActiveItem = "txt_macno"
                Return False
            ElseIf oDBDSHeader.GetValue("U_pmcno", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Check List Number Should Not Be Left Empty")
                frmActivityPlan.ActiveItem = "t_pmcname"
                Return False
            ElseIf oDBDSHeader.GetValue("U_SchedDt", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Schedule Date Should Not Be Left Empty")
                Return False
            ElseIf oDBDSHeader.GetValue("U_PreByNam", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Prepared By Should Not Be Left Empty")
                frmActivityPlan.ActiveItem = "t_prebynam"
                Return False
            ElseIf oDBDSHeader.GetValue("U_AppByCod", 0).Equals(Trim("")) = True Then
                oGFun.StatusBarErrorMsg("Approved By Should Not Be Left Empty")
                frmActivityPlan.ActiveItem = "t_appbynam"
                Return False
            End If
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmActivityPlan.Items.Item("t_schedt").Specific
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            If HANA Then
                SQuery = "SELECT * FROM ""@MIPL_PM_OACP"" WHERE ""DocNum"" <> '" & Trim(oDBDSHeader.GetValue("DocNum", 0)) & "' and To_Varchar(""U_SchedDt"",'yyyyMMdd')  > '" & DocDate.ToString("yyyyMMdd") & "' AND ""U_MacNo"" ='" & Trim(oDBDSHeader.GetValue("U_MacNo", 0)) & "' AND ""U_PMCNo"" ='" & Trim(oDBDSHeader.GetValue("U_PMCNo", 0)) & "'"
            Else
                SQuery = "SELECT * FROM [@MIPL_PM_OACP] WHERE DocNum <> '" & Trim(oDBDSHeader.GetValue("DocNum", 0)) & "' and Format(U_SchedDt,'yyyyMMdd')  > '" & DocDate.ToString("yyyyMMdd") & "'  AND U_MacNo ='" & Trim(oDBDSHeader.GetValue("U_MacNo", 0)) & "' AND U_PMCNo ='" & Trim(oDBDSHeader.GetValue("U_PMCNo", 0)) & "'"
            End If
            
            Dim rsetPMChk As SAPbobsCOM.Recordset = oGFun.DoQuery(sQuery)
            If rsetPMChk.RecordCount > 0 Then
                oGFun.StatusBarErrorMsg("Activity Plan [" & rsetPMChk.Fields.Item("DocNum").Value & "] is already exists for [" & Trim(oDBDSHeader.GetValue("U_MacNo", 0)) & "] and Check List No.[" & Trim(oDBDSHeader.GetValue("U_PMCNo", 0)) & "]")
                Return False
            End If
            'For i As Integer = 1 To oMatrix1.VisualRowCount - 1
            '    If oMatrix1.Columns.Item("activity").Cells.Item(i).Specific.Equals(Trim("")) = False Then
            '        If oMatrix1.Columns.Item("schdt").Cells.Item(i).Specific.Equals(Trim("")) = True Then
            '            oApplication.StatusBar.SetText("Schedule Date Should Be Left Empty......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '            Return False
            '        ElseIf oMatrix1.Columns.Item("schdt").Cells.Item(i).Specific.Equals(Trim("")) = False Then
            '            Dim c As String = oMatrix1.Columns.Item("activity").Cells.Item(i).Specific.value

            '            Dim schdt As String = oMatrix1.Columns.Item("schdt").Cells.Item(i).Specific.value
            '            Dim docdt As String = oDBDSHeader.GetValue("U_DocDate", 0).Trim
            '            If schdt < docdt Then
            '                oApplication.StatusBar.SetText("Schedule Date Should Be Equal Or Greater then Doc.Date.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '                Return False
            '            End If
            '        End If
            '        Dim b As String = oDBDSDetail1.GetValue("U_schdt", i - 1).Trim
            '        If oMatrix1.Columns.Item("freq").Cells.Item(i).Specific.Equals(Trim("")) = False Then
            '            '  oGFun.StatusBarErrorMsg("Frequency By Should Not Be Left Empty")
            '            ' Return False
            '        End If
            '    End If
            'Next
            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText("Validate Function Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            ValidateAll = False
        Finally
        End Try

    End Function

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction = True Then
                frmActivityPlan = oApplication.Forms.Item(FormUID)
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Select Case pVal.ItemUID
                                Case "t_appbynam"
                                    'oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLauthby", "empID", "select empID from OHEM where dept='13' ")
                                Case "t_prebynam"
                                    'StrQuery = "select empID from OHEM where dept='13' "
                                    'oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLpreby", "empID", StrQuery)
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "activity"
                                            If HANA Then
                                                StrQuery = "SELECT ""U_Activity""  FROM ""@MIPL_PM_OACT"" WHERE ""Code""  IN (select a.""U_actcode"" ""actcode"" from ""@MIPL_PM_PCL1"" a,""@MIPL_PM_OPCL"" b where a.""Code""=b.""Code""  and b.""Code""='" & Trim(oDBDSHeader.GetValue("U_pmcno", 0).Trim) & "')"
                                            Else
                                                StrQuery = "SELECT U_activity  FROM [@MIPL_PM_OACT] WHERE Code  IN (select a.U_actcode actcode from [@MIPL_PM_PCL1] a,[@MIPL_PM_OPCL] b where a.code=b.code  and b.Code='" & Trim(oDBDSHeader.GetValue("U_pmcno", 0).Trim) & "')"
                                            End If
                                            oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLact", "U_activity", StrQuery)
                                    End Select
                                Case "t_category"
                                    If HANA Then
                                        StrQuery = "Select ""Code"",""Name"" from ""@MIPL_PM_OCAT"" where ""U_Type""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                    Else
                                        StrQuery = "Select Code,Name from [@MIPL_PM_OCAT] where U_Type='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                    End If

                                    oGFun.ChooseFromListFilteration(frmActivityPlan, "CATCFL", "Code", StrQuery)
                                Case "mtx_1"
                                    Select Case pVal.ColUID
                                        Case "itemid"
                                            oGFun.ChooseFromLisBefore2ColAliasNotEqual(frmActivityPlan, oMatrix2, "ItemCFL", "itemid")
                                    End Select
                                Case "t_pmcname"
                                    'oGFun.ChooseFromLisBefore(frmActivityPlan, "PMCFL", "U_Active", "Y")
                                    oGFun.ChooseFromLisBefore2ColAlias(frmActivityPlan, "PMCFL", "U_Active", "Y", "U_catcode", oDBDSHeader.GetValue("U_CatCode", 0).Trim)
                                    'If HANA Then
                                    '    StrQuery = "Select ""Code"",""Name"" from ""@MIPL_PM_OPCL"" where ""U_catcode""='" & oDBDSHeader.GetValue("U_CatCode", 0).Trim & "'"
                                    'Else
                                    '    StrQuery = "Select Code,Name from [@MIPL_PM_OPCL] where U_catcode='" & oDBDSHeader.GetValue("U_CatCode", 0).Trim & "'"
                                    'End If
                                    'oGFun.ChooseFromListFilteration(frmActivityPlan, "PMCFL", "Code", StrQuery)
                                Case "txt_macno"
                                    Dim oTxt As SAPbouiCOM.EditText = frmActivityPlan.Items.Item(pVal.ItemUID).Specific
                                    If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                                        If HANA Then
                                            StrQuery = "SELECT ""U_ItemCode""  from ""@MIPL_PM_OVHL"" Where ""U_VechType"" = '" & Trim(oDBDSHeader.GetValue("U_Category", 0)) & "' "
                                        Else
                                            StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] Where U_VechType = '" & Trim(oDBDSHeader.GetValue("U_Category", 0)) & "' "
                                        End If
                                        oTxt.ChooseFromListUID = "OVHL_CFL"
                                        oTxt.ChooseFromListAlias = "U_ItemCode"
                                        oGFun.ChooseFromListFilteration(frmActivityPlan, "OVHL_CFL", "U_ItemCode", StrQuery)
                                    Else
                                        If HANA Then
                                            StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" where ""U_CatName""='" & oDBDSHeader.GetValue("U_Category", 0).Trim & "'"
                                        Else
                                            StrQuery = "select Code from [@MIPL_PM_OMAC] where U_CatName='" & oDBDSHeader.GetValue("U_Category", 0).Trim & "'" ' and U_InsType='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                                        End If
                                        oTxt.ChooseFromListUID = "OMAC_CFL"
                                        oTxt.ChooseFromListAlias = "Code"
                                        oGFun.ChooseFromListFilteration(frmActivityPlan, "OMAC_CFL", "Code", StrQuery)
                                    End If
                                Case "txt_macno"

                                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    If HANA Then
                                        StrQuery = "select ""U_ToolNo"",""U_ToolName"" from ""@MIPL_PM_OMAC"" where ""U_Category""='" & oDBDSHeader.GetValue("U_category", 0).Trim & "'"
                                    Else
                                        StrQuery = "select U_ToolNo,U_ToolName from [@MIPL_PM_OMAC] where U_Category='" & oDBDSHeader.GetValue("U_category", 0).Trim & "'"
                                    End If

                                    oGFun.ChooseFromListFilteration(frmActivityPlan, "OMAC_CFL", "U_ToolNo", StrQuery)

                                Case "t_chklist"
                                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    If HANA Then
                                        StrQuery = "Select ""Code"" from ""@MIPL_PM_OPCL"" where ""U_category""='" & oDBDSHeader.GetValue("U_category", 0).Trim & "' and ""Code"" not in (select ""U_PMCNo"" ""ChkltCd"" from ""@MIPL_PM_OACP"" where ""U_MacNo""='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                                    Else
                                        StrQuery = "Select Code from [@MIPL_PM_OPCL] where U_Category='" & oDBDSHeader.GetValue("U_category", 0).Trim & "' and Code not in (select U_pmcno ChkltCd from [@MIPL_PM_OACP] where U_macno='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                                    End If
                                    oGFun.ChooseFromListFilteration(frmActivityPlan, "PMCFL", "Code", StrQuery)

                                Case "t_chktab"

                                    oMatrix1.Columns.Item("schdt").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)

                            End Select

                        Catch ex As Exception
                        End Try
                End Select
            Else
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE
                        If frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try

                            Dim oDataTable As SAPbouiCOM.DataTable
                            Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                            oDataTable = oCFLE.SelectedObjects
                            Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                                Select Case pVal.ItemUID
                                    Case "txt_macno"
                                        oDBDSHeader.SetValue("U_macno", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_macdesc", 0, Trim(oDataTable.GetValue("U_ItemName", 0)))
                                    Case "t_pmcname"
                                        oDBDSHeader.SetValue("U_PMCNo", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_PMCName", 0, Trim(oDataTable.GetValue("Name", 0)))
                                        Me.LoadDetail1(oDBDSHeader.GetValue("U_pmcno", 0).Trim)
                                    Case "t_appbynam"
                                        oDBDSHeader.SetValue("U_AppByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_AppByNam", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                    Case "t_prebynam"
                                        oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("firstName", 0)) & "," & Trim(oDataTable.GetValue("lastName", 0)))
                                    Case "t_category"
                                        oDBDSHeader.SetValue("U_CatCode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_category", 0, Trim(oDataTable.GetValue("Name", 0)))
                                    Case "mtx_0"
                                        Select Case pVal.ColUID
                                            Case "activity"
                                                oMatrix1.FlushToDataSource()
                                                oDBDSDetail1.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail1.SetValue("U_ActCode", pVal.Row - 1, oDataTable.GetValue("U_Activity", 0))
                                                oMatrix1.LoadFromDataSource()
                                                oMatrix1.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                        End Select
                                        oMatrix1.AutoResizeColumns()
                                    Case "mtx_1"
                                        Select Case pVal.ColUID
                                            Case "itemid"
                                                oMatrix2.FlushToDataSource()
                                                oDBDSDetail2.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                oDBDSDetail2.SetValue("U_ItemName", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                oDBDSDetail2.SetValue("U_UOM", pVal.Row - 1, Trim(oDataTable.GetValue("InvntryUom", 0)))
                                                oDBDSDetail2.SetValue("U_Quantity", pVal.Row - 1, "1")
                                                oMatrix2.LoadFromDataSource()
                                                oMatrix2.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End Select
                                        oMatrix2.AutoResizeColumns()
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
                                        Case "itemid"
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemid")
                                    End Select
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "activity"
                                            'oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, "activity")
                                        Case "schdt"
                                            If pVal.BeforeAction = False Then
                                                oMatrix1.GetLineData(pVal.Row)
                                                If oDBDSDetail1.GetValue("U_schdt", oDBDSDetail1.Offset).Trim <> "" Then
                                                    Dim schdt As String = oDBDSDetail1.GetValue("U_schdt", oDBDSDetail1.Offset).Trim
                                                    Dim docdt As String = oDBDSHeader.GetValue("U_DocDate", 0).Trim
                                                    If schdt < docdt Then
                                                        oApplication.StatusBar.SetText("Schedule Date should be Equal to or Greater than Document Date.......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                    End If
                                                ElseIf oDBDSDetail1.GetValue("U_schdt", oDBDSDetail1.Offset).Trim = "" Then
                                                    oApplication.StatusBar.SetText("Schedule Date should be Left Empty......!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                End If
                                            End If
                                            Dim dt As String = oDBDSDetail1.GetValue("U_schdt", oDBDSDetail1.Offset).Trim
                                            If dt <> "" Then
                                                Dim schdt As DateTime = DateTime.ParseExact(dt, "yyyyMMdd", Nothing)
                                                Dim freq As String = oDBDSDetail1.GetValue("U_Freq", oDBDSDetail1.Offset).Trim
                                                If freq = "Daily" Then
                                                    schdt = schdt.AddDays(1)
                                                ElseIf freq = "Weekly" Then
                                                    schdt = schdt.AddDays(7)
                                                ElseIf freq = "Monthly" Then
                                                    schdt = schdt.AddMonths(1)
                                                ElseIf freq = "Annualy" Then
                                                    schdt = schdt.AddYears(1)
                                                ElseIf freq = "Shift then" Then
                                                    schdt = dt
                                                ElseIf freq = "Quarterly" Then
                                                    schdt = schdt.AddMonths(3)
                                                ElseIf freq = "Half Yearly" Then
                                                    schdt = schdt.AddMonths(6)
                                                End If
                                                oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, schdt.ToString("yyyyMMdd"))
                                                oMatrix1.SetLineData(pVal.Row)
                                            Else
                                                oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, "")
                                                oMatrix1.SetLineData(pVal.Row)
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
                                Case "c_series"
                                    If frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                        'Get the Serial Number Based On Series...
                                        Dim oCmbSerial As SAPbouiCOM.ComboBox = frmActivityPlan.Items.Item("c_series").Specific
                                        Dim strSerialCode As String = oCmbSerial.Selected.Value
                                        Dim strDocNum As Long = frmActivityPlan.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                        oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                    End If
                                Case "c_type"
                                    Try
                                        If pVal.ItemChanged And pVal.BeforeAction = False Then
                                            oMatrix1.Clear()
                                            oMatrix2.Clear()
                                            oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                            oDBDSHeader.SetValue("U_MacNo", 0, "")
                                            oDBDSHeader.SetValue("U_macdesc", 0, "")
                                            oDBDSHeader.SetValue("U_PMCNo", 0, "")
                                            oDBDSHeader.SetValue("U_PMCName", 0, "")
                                            oDBDSHeader.SetValue("U_SchedDt", 0, "")
                                            oDBDSHeader.SetValue("U_Category", 0, "")
                                        End If
                                    Catch ex As Exception
                                        oGFun.StatusBarErrorMsg("Combo select event Failed : " & ex.Message)
                                    Finally
                                    End Try
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "freq"
                                            If pVal.BeforeAction = False Then
                                                oMatrix1.GetLineData(pVal.Row)
                                                Dim dt As String = oDBDSDetail1.GetValue("U_schdt", oDBDSDetail1.Offset).Trim
                                                If dt <> "" Then
                                                    Dim schdt As DateTime = DateTime.ParseExact(dt, "yyyyMMdd", Nothing)
                                                    Dim freq As String = oDBDSDetail1.GetValue("U_Freq", oDBDSDetail1.Offset).Trim
                                                    If freq = "Daily" Then
                                                        schdt = schdt.AddDays(1)
                                                    ElseIf freq = "Weekly" Then
                                                        schdt = schdt.AddDays(7)
                                                    ElseIf freq = "Monthly" Then
                                                        schdt = schdt.AddMonths(1)
                                                    ElseIf freq = "Annualy" Then
                                                        schdt = schdt.AddYears(1)
                                                    ElseIf freq = "Shift then" Then
                                                        schdt = dt
                                                    ElseIf freq = "Quarterly" Then
                                                        schdt = schdt.AddMonths(3)
                                                    ElseIf freq = "Half Yearly" Then
                                                        schdt = schdt.AddMonths(6)
                                                    End If
                                                    oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, schdt.ToString("yyyyMMdd"))
                                                    oMatrix1.SetLineData(pVal.Row)
                                                Else
                                                    oDBDSDetail1.SetValue("U_nxtschdt", oDBDSDetail1.Offset, "")
                                                    oMatrix1.SetLineData(pVal.Row)
                                                End If
                                            End If
                                    End Select
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.BeforeAction = True And (frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                                    If pVal.ActionSuccess And frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "lk_chk"
                                    If pVal.BeforeAction = False Then
                                        oGFun.DoOpenLinkedObjectForm("OPCL", "OPCL", "t_code", Trim(oDBDSHeader.GetValue("U_pmcno", 0)))
                                    End If
                                Case "lk_cat"
                                    If pVal.BeforeAction = False Then
                                        oGFun.DoOpenLinkedObjectForm("OCAT", "OCAT", "txt_cat", Trim(oDBDSHeader.GetValue("U_category", 0)))
                                    End If
                                Case "link_mno"
                                    If pVal.BeforeAction = False Then
                                        Dim ocmb As SAPbouiCOM.ComboBox = frmActivityPlan.Items.Item("c_type").Specific
                                        If ocmb.Selected.Value = "VH" Then
                                            oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSHeader.GetValue("U_MacNo", 0).Trim)
                                        Else
                                            oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oDBDSHeader.GetValue("U_MacNo", 0).Trim)
                                        End If
                                    End If
                                Case "tab_0"
                                    If pVal.BeforeAction = False Then
                                        frmActivityPlan.PaneLevel = 1
                                        frmActivityPlan.Items.Item("tab_0").AffectsFormMode = False
                                        oGFun.SetNewLine(oMatrix1, oDBDSDetail1, , "activity")
                                    End If
                                Case "tab_1"
                                    If pVal.BeforeAction = False Then
                                        frmActivityPlan.PaneLevel = 2
                                        frmActivityPlan.Items.Item("tab_1").AffectsFormMode = False
                                        oGFun.SetNewLine(oMatrix2, oDBDSDetail2, , "itemid")
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Try
                            Select Case pVal.ItemUID
                                Case "mtx_0"
                                    Select Case pVal.ColUID
                                        Case "activity"
                                            If pVal.BeforeAction = False Then
                                                oGFun.DoOpenLinkedObjectForm("OACT", "OACT", "txt_actvty", oMatrix1.Columns.Item("activity").Cells.Item(pVal.Row).Specific.value)
                                            End If
                                    End Select
                            End Select
                        Catch ex As Exception
                            oGFun.StatusBarErrorMsg("Matrix Link Pressed Event Failed:" & ex.Message)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                        Try
                            Select Case pVal.ItemUID
                                Case "t_schedt"
                                    If pVal.ItemChanged And pVal.BeforeAction Then
                                        If oGFun.isDateCompare(frmActivityPlan.Items.Item("t_docdate").Specific, frmActivityPlan.Items.Item("t_schedt").Specific, "Schedule Date should be greater than Docdate..") = False Then BubbleEvent = False
                                    End If

                            End Select
                        Catch ex As Exception
                            oGFun.StatusBarErrorMsg("Validate Event Failed:" & ex.Message)
                        Finally
                        End Try
                        'Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        '    Try
                        '        Select Case pVal.ItemUID
                        '            Case "t_appbynam"
                        '                'oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLauthby", "empID", "select empID from OHEM where dept='13' ")
                        '            Case "t_prebynam"
                        '                'StrQuery = "select empID from OHEM where dept='13' "
                        '                'oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLpreby", "empID", StrQuery)

                        '            Case "mtx_0"
                        '                Select Case pVal.ColUID
                        '                    Case "activity"
                        '                        If HANA Then
                        '                            StrQuery = "SELECT ""U_Activity""  FROM ""@MIPL_PM_OACT"" WHERE ""Code""  IN (select a.""U_actcode"" ""actcode"" from ""@MIPL_PM_PCL1"" a,""@MIPL_PM_OPCL"" b where a.""Code""=b.""Code""  and b.""Code""='" & Trim(oDBDSHeader.GetValue("U_pmcno", 0).Trim) & "')"
                        '                        Else
                        '                            StrQuery = "SELECT U_activity  FROM [@MIPL_PM_OACT] WHERE Code  IN (select a.U_actcode actcode from [@MIPL_PM_PCL1] a,[@MIPL_PM_OPCL] b where a.code=b.code  and b.Code='" & Trim(oDBDSHeader.GetValue("U_pmcno", 0).Trim) & "')"
                        '                        End If
                        '                        oGFun.ChooseFromListFilteration(frmActivityPlan, "CFLact", "U_activity", StrQuery)
                        '                End Select
                        '            Case "t_category"
                        '                If HANA Then
                        '                    StrQuery = "Select ""Code"",""Name"" from ""@MIPL_PM_OCAT"" where ""U_Type""='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                        '                Else
                        '                    StrQuery = "Select Code,Name from [@MIPL_PM_OCAT] where U_Type='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                        '                End If

                        '                oGFun.ChooseFromListFilteration(frmActivityPlan, "CATCFL", "Code", StrQuery)
                        '            Case "t_pmcname"
                        '                If HANA Then
                        '                    StrQuery = "Select ""Code"",""Name"" from ""@MIPL_PM_OPCL"" where ""U_catcode""='" & oDBDSHeader.GetValue("U_CatCode", 0).Trim & "'"
                        '                Else
                        '                    StrQuery = "Select Code,Name from [@MIPL_PM_OPCL] where U_catcode='" & oDBDSHeader.GetValue("U_CatCode", 0).Trim & "'"
                        '                End If

                        '                oGFun.ChooseFromListFilteration(frmActivityPlan, "PMCFL", "Code", StrQuery)
                        '            Case "txt_macno"
                        '                Dim oTxt As SAPbouiCOM.EditText = frmActivityPlan.Items.Item(pVal.ItemUID).Specific
                        '                If Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("VH") Then
                        '                    If HANA Then
                        '                        StrQuery = "SELECT ""U_ItemCode""  from ""@MIPL_PM_OVHL"" Where ""U_VechType"" = '" & Trim(oDBDSHeader.GetValue("U_Category", 0)) & "' "
                        '                    Else
                        '                        StrQuery = "SELECT U_ItemCode  from [@MIPL_PM_OVHL] Where U_VechType = '" & Trim(oDBDSHeader.GetValue("U_Category", 0)) & "' "
                        '                    End If
                        '                    oTxt.ChooseFromListUID = "OVHL_CFL"
                        '                    oTxt.ChooseFromListAlias = "U_ItemCode"
                        '                    oGFun.ChooseFromListFilteration(frmActivityPlan, "OVHL_CFL", "U_ItemCode", StrQuery)
                        '                Else
                        '                    If HANA Then
                        '                        StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" where ""U_CatName""='" & oDBDSHeader.GetValue("U_Category", 0).Trim & "'"
                        '                    Else
                        '                        StrQuery = "select Code from [@MIPL_PM_OMAC] where U_CatName='" & oDBDSHeader.GetValue("U_Category", 0).Trim & "'" ' and U_InsType='" & oDBDSHeader.GetValue("U_Type", 0).Trim & "'"
                        '                    End If
                        '                    oTxt.ChooseFromListUID = "OMAC_CFL"
                        '                    oTxt.ChooseFromListAlias = "Code"
                        '                    oGFun.ChooseFromListFilteration(frmActivityPlan, "OMAC_CFL", "Code", StrQuery)
                        '                End If
                        '            Case "txt_macno"
                        '                If pVal.BeforeAction = False Then
                        '                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        '                    If HANA Then
                        '                        StrQuery = "select ""U_ToolNo"",""U_ToolName"" from ""@MIPL_PM_OMAC"" where ""U_Category""='" & oDBDSHeader.GetValue("U_category", 0).Trim & "'"
                        '                    Else
                        '                        StrQuery = "select U_ToolNo,U_ToolName from [@MIPL_PM_OMAC] where U_Category='" & oDBDSHeader.GetValue("U_category", 0).Trim & "'"
                        '                    End If

                        '                    oGFun.ChooseFromListFilteration(frmActivityPlan, "OMAC_CFL", "U_ToolNo", StrQuery)
                        '                End If
                        '            Case "t_chklist"
                        '                If pVal.BeforeAction = False Then
                        '                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        '                    If HANA Then
                        '                        StrQuery = "Select ""Code"" from ""@MIPL_PM_OPCL"" where ""U_category""='" & oDBDSHeader.GetValue("U_category", 0).Trim & "' and ""Code"" not in (select ""U_PMCNo"" ""ChkltCd"" from ""@MIPL_PM_OACP"" where ""U_MacNo""='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                        '                    Else
                        '                        StrQuery = "Select Code from [@MIPL_PM_OPCL] where U_Category='" & oDBDSHeader.GetValue("U_category", 0).Trim & "' and Code not in (select U_pmcno ChkltCd from [@MIPL_PM_OACP] where U_macno='" & oDBDSHeader.GetValue("U_macno", 0).Trim & "') "
                        '                    End If
                        '                    oGFun.ChooseFromListFilteration(frmActivityPlan, "PMCFL", "Code", StrQuery)
                        '                End If
                        '            Case "t_chktab"
                        '                If pVal.Before_Action = False Then
                        '                    oMatrix1.Columns.Item("schdt").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        '                End If
                        '        End Select
                        '                Catch ex As Exception
                        '    oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'Finally
                        'End Try
                End Select
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub LoadDetail1(ByVal clno As String)
        Try
            Dim rsetLoadItem As SAPbobsCOM.Recordset

            rsetLoadItem = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim strqry As String
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmActivityPlan.Items.Item("t_docdate").Specific
            If oEdit.Value = "" Then
                frmActivityPlan.Items.Item("t_docdate").Specific.String = Now.Date.ToString("yyyyMMdd")
            End If
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            If HANA Then
                strqry = "select a.""U_actcode"",a.""U_activity"",a.""U_genobsv"",Case when b.""U_Freqncy""=0 then TO_VARCHAR((select ADD_DAYS('" & DocDate.ToString("yyyyMMdd") & "', 1) from dummy),'yyyyMMdd') "  'Current_Date
                strqry += vbCrLf + "when b.""U_Freqncy""=1 then TO_VARCHAR((select ADD_DAYS('" & DocDate.ToString("yyyyMMdd") & "', 7) from dummy),'yyyyMMdd')"
                strqry += vbCrLf + "when b.""U_Freqncy""=2 then TO_VARCHAR((select ADD_MONTHS('" & DocDate.ToString("yyyyMMdd") & "', 1) from dummy),'yyyyMMdd')"
                strqry += vbCrLf + "when b.""U_Freqncy""=3 then TO_VARCHAR((select ADD_MONTHS('" & DocDate.ToString("yyyyMMdd") & "', 3) from dummy),'yyyyMMdd')"
                strqry += vbCrLf + "when b.""U_Freqncy""=4 then TO_VARCHAR((select ADD_MONTHS('" & DocDate.ToString("yyyyMMdd") & "', 6) from dummy),'yyyyMMdd')"
                strqry += vbCrLf + "when b.""U_Freqncy""=5 then TO_VARCHAR((select ADD_YEARS('" & DocDate.ToString("yyyyMMdd") & "', 1) from dummy),'yyyyMMdd') end  ""U_Freqncy"""
                strqry += vbCrLf + "from ""@MIPL_PM_PCL1"" a,""@MIPL_PM_OPCL"" b where a.""Code""=b.""Code""  and b.""Code""='" & Trim(clno) & "'"
            Else
                strqry = "select a.U_actcode,a.U_activity,a.U_genobsv,Case when b.U_Freqncy=0 then Format((Select DATEADD(DAY, 1, '" & DocDate.ToString("yyyyMMdd") & "' )),'yyyyMMdd') "   'GetDate()
                strqry += vbCrLf + "when b.U_Freqncy=1 then Format((Select DATEADD(WEEK, 1, '" & DocDate.ToString("yyyyMMdd") & "')),'yyyyMMdd')"
                strqry += vbCrLf + "when b.U_Freqncy=2 then Format((Select DATEADD(MONTH, 1, '" & DocDate.ToString("yyyyMMdd") & "')),'yyyyMMdd')"
                strqry += vbCrLf + "when b.U_Freqncy=3 then Format((Select DATEADD(MONTH, 3, '" & DocDate.ToString("yyyyMMdd") & "')),'yyyyMMdd')"
                strqry += vbCrLf + "when b.U_Freqncy=4 then Format((Select DATEADD(MONTH, 6, '" & DocDate.ToString("yyyyMMdd") & "')),'yyyyMMdd')"
                strqry += vbCrLf + "when b.U_Freqncy=5 then Format((Select DATEADD(YEAR, 1, '" & DocDate.ToString("yyyyMMdd") & "')),'yyyyMMdd') end  U_Freqncy"
                strqry += vbCrLf + "from [@MIPL_PM_PCL1] a,[@MIPL_PM_OPCL] b where a.Code=b.Code  and b.Code='" & Trim(clno) & "'"
            End If

            rsetLoadItem.DoQuery(strqry)
            rsetLoadItem.MoveFirst()
            oMatrix1.Clear()
            oMatrix1.AddRow()
            oDBDSDetail1.Clear()
            If rsetLoadItem.RecordCount > 0 Then
                oDBDSHeader.SetValue("U_SchedDt", 0, rsetLoadItem.Fields.Item(3).Value)
                For i As Integer = 0 To rsetLoadItem.RecordCount - 1
                    oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                    oDBDSDetail1.Offset = i
                    oDBDSDetail1.SetValue("LineId", oDBDSDetail1.Offset, i + 1)
                    oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetLoadItem.Fields.Item(0).Value)
                    oDBDSDetail1.SetValue("U_ActName", oDBDSDetail1.Offset, rsetLoadItem.Fields.Item(1).Value)
                    oDBDSDetail1.SetValue("U_Parametr", oDBDSDetail1.Offset, rsetLoadItem.Fields.Item(2).Value)
                    rsetLoadItem.MoveNext()
                Next
            End If
            
            oMatrix1.LoadFromDataSource()
            oMatrix1.AutoResizeColumns()
            oMatrix2.Clear()
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
        Catch ex As Exception
            oApplication.StatusBar.SetText("Load Detail1 Failed: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case "1281" 'Find
                    frmActivityPlan.ActiveItem = "t_docnum"
                    frmActivityPlan.Items.Item("c_status").Enabled = True
                    oMatrix1.Item.Enabled = False
                    oMatrix2.Item.Enabled = False
                Case "1282"
                    Me.InitForm()
                Case "1293"
                    Select Case DeleteRowITEMUID
                        Case "mtx_0"
                            oGFun.DeleteRow(oMatrix1, oDBDSDetail1)
                        Case "mtx_1"
                            oGFun.DeleteRow(oMatrix2, oDBDSDetail2)
                    End Select
                Case "1287"
                    oGFun.LoadComboBoxSeries(frmActivityPlan.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
                    oGFun.LoadDocumentDate(frmActivityPlan.Items.Item("t_docdate").Specific) ' Load Document Date
                    frmActivityPlan.Items.Item("t_schedt").Specific.String = ""
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
                        If frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Me.ValidateAll() = False Then
                                System.Media.SystemSounds.Asterisk.Play()
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                BubbleEvent = False
                                Exit Sub
                            Else
                                If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                        End If
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix1, "activity", oDBDSDetail1)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix2, "itemid", oDBDSDetail2)
                    End If
                    'If BusinessObjectInfo.ActionSuccess Then
                    '    oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, "activity")
                    '    oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemid")
                    'End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess = True Then
                        'oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.VisualRowCount, "activity")
                        oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.VisualRowCount, "itemid")
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
                    'Dim Matrix0, Matrix1 As SAPbouiCOM.Matrix
                    'Matrix0 = frmActivityPlan.Items.Item("mtx_0").Specific
                    'Matrix1 = frmActivityPlan.Items.Item("mtx_1").Specific
                    If EventInfo.BeforeAction = True Then
                        If frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If EventInfo.ItemUID = "mtx_1" Then
                                If EventInfo.Row = oMatrix2.VisualRowCount Then
                                    frmActivityPlan.EnableMenu("1293", False)
                                Else
                                    frmActivityPlan.EnableMenu("1293", True)
                                End If
                            Else
                                frmActivityPlan.EnableMenu("1293", False)
                            End If
                        End If
                        frmActivityPlan.EnableMenu("1284", False)
                        frmActivityPlan.EnableMenu("1285", False)
                        frmActivityPlan.EnableMenu("1286", False)
                        If frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE Or frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmActivityPlan.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmActivityPlan.EnableMenu("1287", True)  'Duplicate
                        Else
                            frmActivityPlan.EnableMenu("1287", False)
                        End If
                    Else
                        frmActivityPlan.EnableMenu("1293", False)
                    End If
                    
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

    Public Sub LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean)
        Try
            frmActivityPlan = oGFun.oApplication.Forms.Item(eventInfo.FormUID)
            'eventInfo.LayoutKey = frmActivityPlan.Items.Item("t_docnum").Specific.string
            eventInfo.LayoutKey = frmActivityPlan.DataSources.DBDataSources.Item("@MIPL_PM_OACP").GetValue("DocEntry", 0)
        Catch ex As Exception
        End Try
        
    End Sub



End Class