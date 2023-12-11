Public Class MachineMaster
    Dim frmMachineMaster As SAPbouiCOM.Form
    Dim oMatSpecify, oMatInsert, oMatAux, oMatSpare As SAPbouiCOM.Matrix
    Dim oDBDSHeader, oDBDSSpecify, oDBDSInsert, oDBDSNote, oDBDSImages, oDBDSGenInfo, oDBDSAmortision, oDBDSAux, oDBDSSpare As SAPbouiCOM.DBDataSource
    Dim oItemid, Rowid As String
    Dim DeleteRowItemUID As String = ""
    Dim UDOID As String = "OMAC"
    Dim StrQuery As String = ""
    Sub LoadMachineMaster()
        Try
            oGFun.LoadXML(frmMachineMaster, MachineMasterFormID, MachineMasterXML)
            frmMachineMaster = oApplication.Forms.Item(MachineMasterFormID)
            oDBDSHeader = frmMachineMaster.DataSources.DBDataSources.Item(0)
            oDBDSSpecify = frmMachineMaster.DataSources.DBDataSources.Item("@MIPL_PM_MAC1")
            oDBDSInsert = frmMachineMaster.DataSources.DBDataSources.Item("@MIPL_PM_MAC2")
            oDBDSAux = frmMachineMaster.DataSources.DBDataSources.Item("@MIPL_PM_MAC3")
            oDBDSSpare = frmMachineMaster.DataSources.DBDataSources.Item("@MIPL_PM_MAC4")

            oMatSpecify = frmMachineMaster.Items.Item("Specify").Specific
            oMatInsert = frmMachineMaster.Items.Item("Insert").Specific
            oMatAux = frmMachineMaster.Items.Item("MtxAux").Specific
            oMatSpare = frmMachineMaster.Items.Item("MtxSpare").Specific

            Me.DefineModesForFields()
            Me.InitForm()

        Catch ex As Exception

        End Try
    End Sub

    Sub InitForm()
        Try
            frmMachineMaster.Freeze(True)
            'oGFun.LoadComboBoxSeries(frmMachineMaster.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            'oGFun.LoadDocumentDate(frmMachineMaster.Items.Item("t_docdate").Specific) ' Load Document Date
            oGFun.LoadLocationComboBox(frmMachineMaster.Items.Item("c_location").Specific) ' Load the location Combo Box...
            oGFun.LoadDepartmentComboBox(frmMachineMaster.Items.Item("c_Dept").Specific) ' Load the Department Combo Box...
            If HANA Then
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OMAC"""))
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_MadeIn").Specific, "Select ""Code"",""Name"" from OCRY")
                'oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_group").Specific, "SELECT ""ItmsGrpCod"",""ItmsGrpNam"" FROM OITB")
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_group").Specific, "SELECT ""Code"",""Name"" FROM ""@MIPL_PM_ITGRP""")
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_Instype").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" where ""U_TypeCode""<>'VH' ")
            Else
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OMAC]"))
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_MadeIn").Specific, "Select Code,Name from OCRY")
                'oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_group").Specific, "SELECT ItmsGrpCod,ItmsGrpNam FROM OITB")
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_group").Specific, "SELECT ""Code"",""Name"" FROM [@MIPL_PM_ITGRP]")
                oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_Instype").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] where U_TypeCode<>'VH'")
            End If
          
            'oGFun.setComboBoxValue(frmMachineMaster.Items.Item("c_manufact").Specific, "Select FirmCode,FirmName FROM OMRC")

            oGFun.SetNewLine(oMatSpecify, oDBDSSpecify) 'Set new line in Matrix
            oGFun.SetNewLine(oMatInsert, oDBDSInsert) 'Set new line in Attach Folder Matrix
            oGFun.SetNewLine(oMatAux, oDBDSAux)
            oGFun.SetNewLine(oMatSpare, oDBDSSpare)
            frmMachineMaster.Items.Item("GenInfo").Specific.Select()
            frmMachineMaster.PaneLevel = 1
            frmMachineMaster.Freeze(False)
            frmMachineMaster.ActiveItem = "c_location"
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmMachineMaster.Freeze(False)
        Finally
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmMachineMaster.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmMachineMaster.Items.Item("t_ItemName").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("c_group").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("c_Dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("c_Dept").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_Category").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_Category").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_DefWhse").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_manufact").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_manufact").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmMachineMaster.Items.Item("t_ItemCode").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmMachineMaster.Items.Item("c_group").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmMachineMaster.Items.Item("t_code").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            'frmMachineMaster.Items.Item("t_ToolNo").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 8, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' This method is used to generate document number
    ''' </summary>

    Function ValidateAll() As Boolean
        Try
            'HEAD
            Dim rs As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            If Trim(oDBDSHeader.GetValue("U_Location", 0)).Equals("") = True Then
                oApplication.StatusBar.SetText("Location Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                Dim GetDup As String
                If Not frmMachineMaster.Items.Item("c_Instype").Specific.Selected Is Nothing And Not frmMachineMaster.Items.Item("c_group").Specific.Selected Is Nothing Then
                    If HANA Then
                        GetDup = oGFun.getSingleValue("SELECT ifnull(count(""U_ItemCode""),0) FROM ""@MIPL_PM_OMAC"" where ""U_ItemCode""='" & frmMachineMaster.Items.Item("t_ItemCode").Specific.value & "' and ""U_InsType""='" & frmMachineMaster.Items.Item("c_Instype").Specific.Selected.value & "' and ""U_Group""='" & frmMachineMaster.Items.Item("c_group").Specific.Selected.value & "' group by ""U_ItemCode"" having count(""U_ItemCode"")>0")
                    Else
                        GetDup = oGFun.getSingleValue("SELECT isnull(count(U_ItemCode),0) FROM [@MIPL_PM_OMAC] where U_ItemCode='" & frmMachineMaster.Items.Item("t_ItemCode").Specific.value & "' and U_InsType='" & frmMachineMaster.Items.Item("c_Instype").Specific.Selected.value & "' and U_Group='" & frmMachineMaster.Items.Item("c_group").Specific.Selected.value & "'  group by U_ItemCode having count(U_ItemCode)>0")
                    End If
                    If IIf(GetDup = "", 0, GetDup) > 0 Then
                        oApplication.StatusBar.SetText("Duplicate found.Please check the ItemCode!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If


            End If
            
            'If Trim(frmMachineMaster.Items.Item("t_ToolNo").Specific.Value).Equals("") = True Then
            '    oApplication.StatusBar.SetText("Machine/Instrument ID Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If

            'If frmMachineMaster.Items.Item("t_ToolNo").Specific.value.Equals("") = False And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
            '    If oGFun.isDuplicate(frmMachineMaster.Items.Item("t_ToolNo").Specific, "[@MIPL_PM_OMAC]", "U_ToolNo", "Item Name") = False Then
            '        Return False
            '    End If
            'End If

            'If Trim(frmMachineMaster.Items.Item("t_ToolName").Specific.Value).Equals("") = True Then
            '    oApplication.StatusBar.SetText("Machine/Instrument Name Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If
            If Trim(frmMachineMaster.Items.Item("t_DefWhse").Specific.Value).Equals("") = True Then
                oApplication.StatusBar.SetText("Default Warehouse Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If Trim(oDBDSHeader.GetValue("U_Dept", 0)).Equals("") = True Then
                oApplication.StatusBar.SetText("Department Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If Trim(oDBDSHeader.GetValue("U_Group", 0)).Equals("") = True Then
                oApplication.StatusBar.SetText("Group Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If Trim(oDBDSHeader.GetValue("U_Instype", 0)).Equals("1") = True And _
            CDbl(frmMachineMaster.Items.Item("t_availday").Specific.Value) = 0 Then
                oApplication.StatusBar.SetText("Avail Per Day Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If Trim(oDBDSHeader.GetValue("U_Instype", 0)).Equals("1") = True And _
            CDbl(frmMachineMaster.Items.Item("t_RatePrHR").Specific.Value) = 0 Then
                oApplication.StatusBar.SetText("Rate per Hr Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            'For i As Integer = 0 To oMatAux.VisualRowCount - 2
            '    If oDBDSAux.GetValue("U_EmpName", i).Trim.Equals("") Then
            '        oGFun.StatusBarErrorMsg("Employee Name Should Not Be Left Empty in Line No : " & i + 1)
            '        Return False
            '    End If
            '    If oDBDSAux.GetValue("U_ManHrs", i).Trim > 0 = False Then
            '        oGFun.StatusBarErrorMsg("Man Hours Should Be Greater than Zero in Line No : " & i + 1)
            '        Return False
            '    End If
            'Next

            'For i As Integer = 0 To oMatSpare.VisualRowCount - 2
            '    If oDBDSSpare.GetValue("U_EmpName", i).Trim.Equals("") Then
            '        oGFun.StatusBarErrorMsg("Employee Name Should Not Be Left Empty in Line No : " & i + 1)
            '        Return False
            '    End If
            '    If oDBDSSpare.GetValue("U_ManHrs", i).Trim > 0 = False Then
            '        oGFun.StatusBarErrorMsg("Man Hours Should Be Greater than Zero in Line No : " & i + 1)
            '        Return False
            '    End If
            'Next

            'If Trim(oDBDSHeader.GetValue("U_Instype", 0)).Equals("T") = True And _
            ' CDbl(frmMachineMaster.Items.Item("t_Strokqty").Specific.Value) = 0 Then
            '    oApplication.StatusBar.SetText("Stand. Storkes Per Qty Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If

            'If Trim(oDBDSHeader.GetValue("U_Instype", 0)).Equals("T") = True And _
            'CDbl(frmMachineMaster.Items.Item("t_RecdFreq").Specific.Value) = 0 Then
            '    oApplication.StatusBar.SetText("Recondition Freq. Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If

            If Trim(oDBDSHeader.GetValue("U_Group", 0)).Equals("") = True Then
                oApplication.StatusBar.SetText("Group Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If


            If Trim(frmMachineMaster.Items.Item("t_Category").Specific.Value).Equals("") = True Then
                oApplication.StatusBar.SetText("Category Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If oDBDSHeader.GetValue("U_WAType", 0).Equals("-") = False Then
                If Trim(frmMachineMaster.Items.Item("t_InstalDt").Specific.Value).Equals("") = False And Trim(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value).Equals("") = False Then
                    If DateDiff(DateInterval.Day, DateTime.ParseExact(frmMachineMaster.Items.Item("t_InstalDt").Specific.Value, "yyyyMMdd", Nothing), DateTime.ParseExact(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value, "yyyyMMdd", Nothing)) < 0 Then
                        oApplication.StatusBar.SetText("Warranty Start Date Should Be Greater Than or Equal to Installation Date", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If
                If Trim(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value).Equals("") = True Then
                    ' MsgBox(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value)
                    oApplication.StatusBar.SetText("Warranty Start  Should Not Be Left Empty!!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                End If
                'Warranty Start and End Date
                If ((Trim(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value).Equals("") = True And Trim(frmMachineMaster.Items.Item("t_WarEndDt").Specific.Value).Equals("") = False) Or (Trim(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value).Equals("") = False And Trim(frmMachineMaster.Items.Item("t_WarEndDt").Specific.Value).Equals("") = True)) Then
                    oApplication.StatusBar.SetText("Warranty Start And End Date  Should Not Be Left Empty!!!", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                ElseIf Trim(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value).Equals("") = False And Trim(frmMachineMaster.Items.Item("t_WarEndDt").Specific.Value).Equals("") = False Then
                    If DateDiff(DateInterval.Day, DateTime.ParseExact(frmMachineMaster.Items.Item("t_WarStDt").Specific.Value, "yyyyMMdd", Nothing), DateTime.ParseExact(frmMachineMaster.Items.Item("t_WarEndDt").Specific.Value, "yyyyMMdd", Nothing)) < 0 Then
                        oApplication.StatusBar.SetText("Warranty End Date Should Be Greater Than Or Equal to Warranty Start Date", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If
            End If



            'Last and Next Calibration Date
            'MsgBox(oDBDSHeader.GetValue("U_CalibReq", 0))
            'If Trim(oDBDSHeader.GetValue("U_CalibReq", 0)).Equals("Y") = True Then

            '    If ((Trim(frmMachineMaster.Items.Item("t_LstCalDt").Specific.Value).Equals("")) = True) Then
            '        oApplication.StatusBar.SetText("Last Calibration and Next Calibration Date Should Not Be Left Empty!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '        Return False
            '    ElseIf Trim(frmMachineMaster.Items.Item("t_LstCalDt").Specific.Value).Equals("") = False And Trim(frmMachineMaster.Items.Item("t_NxtCalDt").Specific.Value).Equals("") = False Then
            '        If DateDiff(DateInterval.Day, DateTime.ParseExact(frmMachineMaster.Items.Item("t_LstCalDt").Specific.Value, "yyyyMMdd", Nothing), DateTime.ParseExact(frmMachineMaster.Items.Item("t_NxtCalDt").Specific.Value, "yyyyMMdd", Nothing)) < 0 Then
            '            oApplication.StatusBar.SetText("Next Calibration Date Should Be Greater Than Or Equal To Last Calibration Date", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '            Return False
            '        End If
            '    End If
            'End If


            ValidateAll = True
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
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Select Case pVal.ItemUID
                                Case ""
                                    'oGFun.ChooseFromListFilteration(frmMachineMaster, "OPOR_CFL", "CardCode", Trim(oDBDSHeader.GetValue("U_WACard", 0)))
                                Case "t_PMCheck"
                                    'If HANA Then
                                    '    StrQuery = "SELECT ""Code"" FROM ""@MIPL_PM_OPCL"" WHERE ""U_catcode"" ='" & oDBDSHeader.GetValue("U_catcode", 0).Trim & "' AND ""U_Active"" ='Y'"
                                    'Else
                                    '    StrQuery = "SELECT Code FROM [@MIPL_PM_OPCL] WHERE U_catcode ='" & oDBDSHeader.GetValue("U_catcode", 0).Trim & "' AND U_Active ='Y'"
                                    'End If
                                    'oGFun.ChooseFromListFilteration(frmMachineMaster, "PMCCFL", "Code", StrQuery)
                                    oGFun.ChooseFromLisBefore2ColAlias(frmMachineMaster, "PMCCFL", "U_catcode", oDBDSHeader.GetValue("U_catcode", 0).Trim, "U_Active", "Y")
                                Case "t_ToolNo"
                                    If HANA Then
                                        oGFun.ChooseFromListFilteration(frmMachineMaster, "MacInsCFL", "ItemCode", "Select ""ItemCode"" From OITM  Where ""ItmsGrpCod"" = '" & oDBDSHeader.GetValue("U_Group", 0).Trim & "' And ""ItemCode"" Not in(select ""U_ToolNo"" from ""@MIPL_PM_OMAC"")")
                                    Else
                                        oGFun.ChooseFromListFilteration(frmMachineMaster, "MacInsCFL", "ItemCode", "Select ItemCode From OITM  Where ItmsGrpCod = '" & oDBDSHeader.GetValue("U_Group", 0).Trim & "' And ItemCode Not in(select U_ToolNo from [@MIPL_PM_OMAC])")
                                    End If
                                Case "t_Category"
                                    Dim oCmb As SAPbouiCOM.ComboBox = frmMachineMaster.Items.Item("c_Instype").Specific
                                    'Dim strquery As String
                                    'If HANA Then
                                    '    strquery = "SELECT ""Code"" FROM ""@MIPL_PM_OCAT"" WHERE ""U_Type"" ='" & oCmb.Selected.Value & "'"
                                    'Else
                                    '    strquery = "SELECT Code FROM [@MIPL_PM_OCAT] WHERE U_Type ='" & oCmb.Selected.Value & "'"
                                    'End If
                                    'oGFun.ChooseFromListFilteration(frmMachineMaster, "CFLcat", "Code", strquery)
                                    If Not oCmb.Selected Is Nothing Then
                                        oGFun.ChooseFromLisBefore(frmMachineMaster, "CFLcat", "U_Type", oCmb.Selected.Value)
                                    End If

                                Case "t_Incharge"
                                    'Dim strquery As String = "EMPFilteration"
                                    'oGFun.ChooseFromListFilteration(frmMachineMaster, "InchargeCFL", "empID", strquery)
                                Case "t_ItemCode"
                                    'If HANA Then
                                    '    StrQuery = "select ""ItemCode"" from OITM where ""ItmsGrpCod""='" & oDBDSHeader.GetValue("U_Group", 0).Trim & "'"
                                    'Else
                                    '    StrQuery = "select ItemCode from OITM where ItmsGrpCod='" & oDBDSHeader.GetValue("U_Group", 0).Trim & "'"
                                    'End If
                                    'oGFun.ChooseFromListFilteration(frmMachineMaster, "MacInsCFL", "ItemCode", StrQuery)
                                    ' oGFun.ChooseFromLisBefore2ColAlias(frmMachineMaster, "MacInsCFL", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim, "InvntItem", "Y")
                                    'oGFun.ChooseFromLisBefore(frmMachineMaster, "MacInsCFL", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim)
                                    oGFun.CFLConditionForHeaderItem(frmMachineMaster, "MacInsCFL", "U_IGRP", oDBDSHeader.GetValue("U_Group", 0).Trim)
                                Case "MtxSpare"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqualInMatrix(frmMachineMaster, oMatSpare, "CSpare", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            oGFun.CFLConditionForLineItem(frmMachineMaster, oMatSpare, "CSpare", "U_IGRP", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            'oGFun.ChooseFromLisBefore(frmMachineMaster, "CSpare", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim)
                                    End Select
                                Case "MtxAux"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqualInMatrix(frmMachineMaster, oMatAux, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            'oGFun.ChooseFromLisBefore(frmMachineMaster, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim)
                                            oGFun.CFLConditionForLineItem(frmMachineMaster, oMatAux, "CAux", "U_IGRP", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                    End Select
                                Case "Insert"
                                    Select Case pVal.ColUID
                                        Case "Insertno"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqualInMatrix(frmMachineMaster, oMatInsert, "InsertCFL", "U_IGRP", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "Insertno")
                                            oGFun.CFLConditionForLineItem(frmMachineMaster, oMatInsert, "InsertCFL", "U_IGRP", oDBDSHeader.GetValue("U_Group", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "Insertno")
                                            'oGFun.ChooseFromLisBefore(frmMachineMaster, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Group", 0).Trim)
                                    End Select
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Me.ValidateAll() = False Then
                                        System.Media.SystemSounds.Asterisk.Play()
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                        End Select
                End Select
            Else
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            If frmMachineMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                Dim Rset As SAPbobsCOM.Recordset
                                Rset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                Dim oDataTable As SAPbouiCOM.DataTable
                                Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                                oDataTable = oCFLE.SelectedObjects
                                If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                                    If frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                    Select Case pVal.ItemUID
                                        Case "t_ItemCode"
                                            oDBDSHeader.SetValue("U_ItemCode", 0, Trim(oDataTable.GetValue("ItemCode", 0)))
                                            oDBDSHeader.SetValue("U_ItemName", 0, Trim(oDataTable.GetValue("ItemName", 0)))
                                            oDBDSHeader.SetValue("U_DefWhse", 0, Trim(oDataTable.GetValue("DfltWH", 0)))
                                            oDBDSHeader.SetValue("U_Manufact", 0, Trim(oDataTable.GetValue("FirmCode", 0)))
                                            Dim Query As String
                                            If HANA Then
                                                Query = "select * from ""@MIPL_PM_MM"" where ""U_DocDate""=(Select Max(""U_DocDate"") from ""@MIPL_PM_MM"" where ""U_MCode""='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and ""U_MCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' order by ""Code"" desc"
                                            Else
                                                Query = "select * from [@MIPL_PM_MM] where U_DocDate=(Select Max(U_DocDate) from [@MIPL_PM_MM] where U_MCode='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and U_MCode='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' order by Code desc"
                                            End If
                                            Rset.DoQuery(Query)
                                            If Rset.RecordCount > 0 Then
                                                oDBDSHeader.SetValue("U_Vibrate", 0, Trim(Rset.Fields.Item("U_Vibrate").Value.ToString))
                                                oDBDSHeader.SetValue("U_Temp", 0, Trim(Rset.Fields.Item("U_Temp").Value.ToString))
                                                oDBDSHeader.SetValue("U_Pressure", 0, Trim(Rset.Fields.Item("U_Pressure").Value.ToString))
                                                oDBDSHeader.SetValue("U_RunKM", 0, Trim(Rset.Fields.Item("U_RunKM").Value.ToString))
                                                oDBDSHeader.SetValue("U_RunHours", 0, Trim(Rset.Fields.Item("U_RunHrs").Value.ToString))
                                            End If
                                           
                                        Case "t_ownrnam"
                                            oDBDSHeader.SetValue("U_OwnrID", 0, Trim(oDataTable.GetValue("empID", 0)))
                                            oDBDSHeader.SetValue("U_OwnrNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                        Case "t_Incharnm"
                                            oDBDSHeader.SetValue("U_IncharCd", 0, Trim(oDataTable.GetValue("empID", 0)))
                                            oDBDSHeader.SetValue("U_IncharNm", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                        Case "t_purfrmnm"
                                            oDBDSHeader.SetValue("U_PurFrmCd", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                            oDBDSHeader.SetValue("U_PurFrmNm", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                        Case "t_PMCheck"
                                            oDBDSHeader.SetValue("U_PMCheck", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        Case "t_Category"
                                            oDBDSHeader.SetValue("U_CatCode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                            oDBDSHeader.SetValue("U_CatName", 0, Trim(oDataTable.GetValue("Name", 0)))
                                        Case "t_ccont"
                                            oDBDSHeader.SetValue("U_CCont", 0, Trim(oDataTable.GetValue("PrjCode", 0)))

                                        Case "t_amccard"
                                            oDBDSHeader.SetValue("U_WACard", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                        Case "t_DefWhse"
                                            oDBDSHeader.SetValue("U_DefWhse", 0, Trim(oDataTable.GetValue("WhsCode", 0)))
                                        Case "t_amcpono"
                                            oDBDSHeader.SetValue("U_AMCPONo", 0, Trim(oDataTable.GetValue("DocNum", 0)))
                                            oDBDSHeader.SetValue("U_AMCPOEntry", 0, Trim(oDataTable.GetValue("DocEntry", 0)))
                                            oDBDSHeader.SetValue("U_AMCPODate", 0, CDate(Trim(oDataTable.GetValue("DocDate", 0))).ToString("yyyyMMdd"))
                                        Case "Insert"
                                            Select Case pVal.ColUID
                                                Case "Insertno"
                                                    oMatInsert.FlushToDataSource()
                                                    oDBDSInsert.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                    oDBDSInsert.SetValue("U_ItemName", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                    oMatInsert.LoadFromDataSource()
                                                    oMatInsert.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                                    oGFun.SetNewLine(oMatInsert, oDBDSInsert, pVal.Row, "Insertno")
                                            End Select
                                            oMatInsert.AutoResizeColumns()
                                        Case "Specify"
                                            Select Case pVal.ColUID
                                                Case "SpecNo"
                                                    oMatSpecify.FlushToDataSource()
                                                    oDBDSSpecify.SetValue("U_Specno", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                                    oDBDSSpecify.SetValue("U_SpecName", pVal.Row - 1, Trim(oDataTable.GetValue("Name", 0)))
                                                    oDBDSSpecify.SetValue("U_Remarks", pVal.Row - 1, Trim(oDataTable.GetValue("U_Remarks", 0)))
                                                    oMatSpecify.LoadFromDataSource()
                                                    oMatSpecify.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            End Select
                                            oMatSpecify.AutoResizeColumns()
                                        Case "MtxAux"
                                            Select Case pVal.ColUID
                                                Case "1"
                                                    oMatAux.FlushToDataSource()
                                                    oDBDSAux.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                    oDBDSAux.SetValue("U_ItemDesc", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                    oDBDSAux.SetValue("U_Quant", pVal.Row - 1, "1")
                                                    oMatAux.LoadFromDataSource()
                                                    oMatAux.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            End Select
                                            oMatAux.AutoResizeColumns()
                                        Case "MtxSpare"
                                            Dim ItmGrp As String
                                            Dim objRs As SAPbobsCOM.Recordset
                                            Select Case pVal.ColUID
                                                Case "1"
                                                    oMatSpare.FlushToDataSource()
                                                    oDBDSSpare.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                    oDBDSSpare.SetValue("U_ItemDesc", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                    oDBDSSpare.SetValue("U_Quant", pVal.Row - 1, "1")
                                                    ItmGrp = oGFun.getSingleValue("select ""ItmsGrpNam"" from OITB where ""ItmsGrpCod""='" & Trim(oDataTable.GetValue("ItmsGrpCod", 0)) & "'")
                                                    oDBDSSpare.SetValue("U_ItmGrp", pVal.Row - 1, ItmGrp)
                                                    oDBDSSpare.SetValue("U_UOM", pVal.Row - 1, Trim(oDataTable.GetValue("InvntryUom", 0)))
                                                    If frmMachineMaster.Items.Item("t_DefWhse").Specific.String <> "" Then
                                                        objRs = oGFun.DoQuery("select ""OnHand"",""MinStock"",""MaxStock"" from OITW where ""ItemCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and ""WhsCode""='" & frmMachineMaster.Items.Item("t_DefWhse").Specific.String & "'")
                                                        If objRs.RecordCount > 0 Then
                                                            'oDBDSSpare.SetValue("U_MIN", pVal.Row - 1, objRs.Fields.Item("MinStock").Value)
                                                            'oDBDSSpare.SetValue("U_MAX", pVal.Row - 1, objRs.Fields.Item("MaxStock").Value)
                                                            oDBDSSpare.SetValue("U_InStock", pVal.Row - 1, objRs.Fields.Item("OnHand").Value)
                                                        End If
                                                    End If
                                                    oMatSpare.LoadFromDataSource()
                                                    oMatSpare.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                            End Select
                                            oMatSpare.AutoResizeColumns()
                                    End Select
                                End If
                            End If
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        'Try
                        '    Select Case pVal.ItemUID
                        '        Case ""
                        '            oGFun.ChooseFromListFilteration(frmMachineMaster, "OPOR_CFL", "CardCode", Trim(oDBDSHeader.GetValue("U_WACard", 0)))
                        '        Case "t_PMCheck"
                        '            If HANA Then
                        '                StrQuery = "SELECT ""Code"" FROM ""@MIPL_PM_OPCL"" WHERE ""U_catcode"" ='" & oDBDSHeader.GetValue("U_catcode", 0).Trim & "' AND ""U_Active"" ='Y'"
                        '            Else
                        '                StrQuery = "SELECT Code FROM [@MIPL_PM_OPCL] WHERE U_catcode ='" & oDBDSHeader.GetValue("U_catcode", 0).Trim & "' AND U_Active ='Y'"
                        '            End If
                        '            oGFun.ChooseFromListFilteration(frmMachineMaster, "PMCCFL", "Code", StrQuery)
                        '        Case "t_ToolNo"
                        '            oGFun.ChooseFromListFilteration(frmMachineMaster, "MacInsCFL", "ItemCode", "Select ItemCode From OITM  Where ItmsGrpCod = '" & oDBDSHeader.GetValue("U_Group", 0).Trim & "' And ItemCode Not in(select U_ToolNo from [@MIPL_PM_OMAC])")
                        '        Case "t_Category"
                        '            Dim oCmb As SAPbouiCOM.ComboBox = frmMachineMaster.Items.Item("c_Instype").Specific
                        '            Dim strquery As String
                        '            If HANA Then
                        '                strquery = "SELECT ""Code"" FROM ""@MIPL_PM_OCAT"" WHERE ""U_Type"" ='" & oCmb.Selected.Value & "'"
                        '            Else
                        '                strquery = "SELECT Code FROM [@MIPL_PM_OCAT] WHERE U_Type ='" & oCmb.Selected.Value & "'"
                        '            End If
                        '            oGFun.ChooseFromListFilteration(frmMachineMaster, "CFLcat", "Code", strquery)
                        '        Case "t_Incharge"
                        '            'Dim strquery As String = "EMPFilteration"
                        '            'oGFun.ChooseFromListFilteration(frmMachineMaster, "InchargeCFL", "empID", strquery)
                        '        Case "t_ItemCode"
                        '            If HANA Then
                        '                StrQuery = "select ""ItemCode"" from OITM where ""ItmsGrpCod""='" & oDBDSHeader.GetValue("U_Group", 0).Trim & "'"
                        '            Else
                        '                StrQuery = "select ItemCode from OITM where ItmsGrpCod='" & oDBDSHeader.GetValue("U_Group", 0).Trim & "'"
                        '            End If
                        '            oGFun.ChooseFromListFilteration(frmMachineMaster, "MacInsCFL", "ItemCode", StrQuery)
                        '    End Select
                        'Catch ex As Exception
                        '    oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'Finally
                        'End Try
                    Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                        Try
                            Select Case pVal.ItemUID
                                Case "Specify"
                                    Select Case pVal.ColUID
                                        Case "SpecNo"
                                            oGFun.SetNewLine(oMatSpecify, oDBDSSpecify, pVal.Row, "SpecNo")
                                    End Select
                                Case "t_Usage"
                                    If pVal.BeforeAction = False Then frmMachineMaster.Items.Item("t_Life").Specific.value = CDbl(frmMachineMaster.Items.Item("t_Side").Specific.value) * CDbl(frmMachineMaster.Items.Item("t_Usage").Specific.value)

                                Case "t_WarEndDt"
                                    If pVal.BeforeAction = False Then
                                        Dim srtDate As String = oDBDSHeader.GetValue("U_WarStDt", 0).Trim
                                        Dim endDate As String = oDBDSHeader.GetValue("U_WarEndDt", 0).Trim
                                        If srtDate <> "" And endDate <> "" Then
                                            If oGFun.isValidFrAndToDate(srtDate, endDate) = False Then
                                                oGFun.StatusBarErrorMsg("End Date should be greater than Start Date..")
                                            End If
                                        End If
                                    End If
                                Case "t_DefWhse"
                                    Dim objRs As SAPbobsCOM.Recordset
                                    If frmMachineMaster.Items.Item("t_DefWhse").Specific.String <> "" Then
                                        If oMatSpare.RowCount > 0 Then
                                            For i As Integer = 1 To oMatSpare.RowCount
                                                objRs = oGFun.DoQuery("select ""OnHand"",""MinStock"",""MaxStock"" from OITW where ""ItemCode""='" & oMatSpare.Columns.Item("1").Cells.Item(i).Specific.String & "' and ""WhsCode""='" & frmMachineMaster.Items.Item("t_DefWhse").Specific.String & "'")
                                                If objRs.RecordCount > 0 Then
                                                    oMatSpare.Columns.Item("9").Cells.Item(i).Specific.String = objRs.Fields.Item("MinStock").Value
                                                    oMatSpare.Columns.Item("10").Cells.Item(i).Specific.String = objRs.Fields.Item("MaxStock").Value
                                                    oMatSpare.Columns.Item("11").Cells.Item(i).Specific.String = objRs.Fields.Item("OnHand").Value
                                                End If
                                            Next
                                        End If
                                    End If
                                Case "MtxAux"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            oGFun.SetNewLine(oMatAux, oDBDSAux, pVal.Row, pVal.ColUID)
                                    End Select
                                Case "MtxSpare"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            oGFun.SetNewLine(oMatSpare, oDBDSSpare, pVal.Row, pVal.ColUID)
                                    End Select

                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try


                    Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                        Try
                            Select Case pVal.ItemUID
                                Case "t_WAType"
                                    If pVal.BeforeAction = False And pVal.ItemChanged Then
                                        Dim amctype As SAPbouiCOM.ComboBox = frmMachineMaster.Items.Item("t_WAType").Specific
                                        If (amctype.Selected.Value = "A") Or (amctype.Selected.Value = "G") Then
                                            frmMachineMaster.Items.Item("t_amccard").Enabled = True
                                            frmMachineMaster.Items.Item("t_amccard").Specific.value = ""
                                            frmMachineMaster.Items.Item("t_amcpono").Enabled = True
                                            frmMachineMaster.Items.Item("t_amcpono").Specific.value = ""
                                        Else
                                            frmMachineMaster.Items.Item("t_amccard").Enabled = False
                                            frmMachineMaster.Items.Item("t_amccard").Specific.value = ""
                                            frmMachineMaster.Items.Item("t_amcpono").Enabled = False
                                            frmMachineMaster.Items.Item("t_amcpono").Specific.value = ""
                                        End If
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "imgbtn"
                                    If pVal.BeforeAction = False Then
                                        If frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                            If pVal.BeforeAction = False Then
                                                If oCompany.BitMapPath.Length <= 0 Then
                                                    oApplication.StatusBar.SetText("BitMap Path folder not defined, or Attchment folder has been changed or removed. [Message 131-102]", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                    Exit Select
                                                End If
                                                Dim strFileName As String = oGFun.FindFile()
                                                frmMachineMaster.Items.Item("picturebox").Visible = True
                                                If strFileName.Equals("") Then Return
                                                Dim Picture0 As SAPbouiCOM.PictureBox = frmMachineMaster.Items.Item("picturebox").Specific
                                                Picture0.Picture = strFileName
                                            End If
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

                                Case "GenInfo"
                                    frmMachineMaster = oApplication.Forms.Item(FormUID)
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 1
                                        frmMachineMaster.Items.Item("GenInfo").AffectsFormMode = False
                                    End If
                                Case "Tech_Spec"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 2
                                        frmMachineMaster.Items.Item("Tech_Spec").AffectsFormMode = False
                                        frmMachineMaster.Settings.MatrixUID = "Specify"
                                        oGFun.SetNewLine(oMatSpecify, oDBDSSpecify, , "SpecNo")
                                    End If
                                Case "Insert_Det"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 3
                                        frmMachineMaster.Items.Item("Insert_Det").AffectsFormMode = False
                                        frmMachineMaster.Settings.MatrixUID = "Insert"
                                        oGFun.SetNewLine(oMatInsert, oDBDSInsert, , "Insertno")
                                        oMatInsert.AutoResizeColumns()
                                    End If
                                Case "Amor_Det"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 4
                                        frmMachineMaster.Items.Item("Amor_Det").AffectsFormMode = False
                                        'frmMachineMaster.Settings.MatrixUID = "t_AmtLife"
                                    End If
                                Case "Note"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 5
                                        frmMachineMaster.Items.Item("Note").AffectsFormMode = False
                                        'frmMachineMaster.Settings.MatrixUID = ""
                                    End If
                                Case "Image"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 6
                                        frmMachineMaster.Items.Item("Image").AffectsFormMode = False
                                        'frmMachineMaster.Settings.MatrixUID = ""
                                    End If
                                Case "ChildAux"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 7
                                        frmMachineMaster.Items.Item("ChildAux").AffectsFormMode = False
                                        'frmMachineMaster.Settings.MatrixUID = ""
                                        oGFun.SetNewLine(oMatAux, oDBDSAux, , "1")
                                    End If
                                Case "Spare"
                                    If pVal.BeforeAction = False Then
                                        frmMachineMaster.PaneLevel = 8
                                        frmMachineMaster.Items.Item("Spare").AffectsFormMode = False
                                        'frmMachineMaster.Settings.MatrixUID = ""
                                        oGFun.SetNewLine(oMatSpare, oDBDSSpare, , "1")
                                    End If
                                Case "1"
                                    If pVal.ActionSuccess And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "lk_cat"
                                    If pVal.Before_Action = False Then
                                        Dim str As String = frmMachineMaster.Items.Item("t_Catcode").Specific.value
                                        oApplication.ActivateMenuItem("OCAT")
                                        Dim oForm As SAPbouiCOM.Form
                                        oForm = oApplication.Forms.Item("OCAT")
                                        oForm.Select()
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                        Dim txt As SAPbouiCOM.EditText = oForm.Items.Item("txt_code").Specific
                                        txt.Value = str
                                        oForm.Items.Item("1").Click()
                                    End If
                                Case "lk_pmchk"
                                    If pVal.Before_Action = False Then
                                        Dim str As String = frmMachineMaster.Items.Item("t_PMCheck").Specific.value
                                        oApplication.ActivateMenuItem("OPCL")
                                        Dim oForm As SAPbouiCOM.Form
                                        oForm = oApplication.Forms.Item("OPCL")
                                        oForm.Select()
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                        Dim txt As SAPbouiCOM.EditText = oForm.Items.Item("t_code").Specific
                                        txt.Value = str
                                        oForm.Items.Item("1").Click()
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try

                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                        Try
                            If frmMachineMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                If pVal.BeforeAction = False Then
                                    Select Case pVal.ItemUID
                                        'Tool Category
                                        Case "t_Category"
                                            If Trim(oDBDSHeader.GetValue("U_CatName", 0)).Equals("") = True Then
                                                oApplication.StatusBar.SetText(" Category should not be left empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                            End If
                                            'Department
                                        Case "c_Dept"
                                            If Trim(oDBDSHeader.GetValue("U_Dept", 0)).Equals("") = True Then
                                                oApplication.StatusBar.SetText("Department Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                            End If
                                            'Default Location
                                        Case "t_DefWhse"
                                            If Trim(oDBDSHeader.GetValue("U_DefWhse", 0)).Equals("") = True Then
                                                oApplication.StatusBar.SetText("Default Wharehouse Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                            End If
                                    End Select
                                End If
                            End If
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Validate Event Failed" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Select Case pVal.ItemUID
                            Case "Specify"
                                Select Case pVal.ColUID
                                    Case "SpecNo"
                                        If pVal.Before_Action = False Then
                                            Dim str As String = oMatSpecify.Columns.Item("SpecNo").Cells.Item(pVal.Row).Specific.value
                                            oApplication.ActivateMenuItem("OSPC")
                                            Dim oForm As SAPbouiCOM.Form
                                            oForm = oApplication.Forms.Item("OSPC")
                                            oForm.Select()
                                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                            Dim txt As SAPbouiCOM.EditText = oForm.Items.Item("t_code").Specific
                                            txt.Value = str
                                            oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End If

                                End Select
                        End Select
                End Select


            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("ItemEvent Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
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
                    Case "1281" 'Find
                        oForm.Items.Item("t_code").Enabled = True
                        oForm.Items.Item("t_amccard").Enabled = True
                        oForm.Items.Item("t_amcpono").Enabled = True
                        oForm.Items.Item("t_amcpodat").Enabled = True
                        oForm.Items.Item("t_Incharcd").Enabled = True
                        oForm.Items.Item("t_ownrid").Enabled = True
                        oMatAux.Item.Enabled = False
                        oMatInsert.Item.Enabled = False
                        oMatSpare.Item.Enabled = False
                        oMatSpecify.Item.Enabled = False
                    Case "1282"
                        Me.InitForm()
                    Case "1293"
                        If oItemid = "Specify" Then
                            oGFun.DeleteRow(oMatSpecify, oDBDSSpecify)
                        ElseIf oItemid = "Insert" Then
                            oGFun.DeleteRow(oMatInsert, oDBDSInsert)
                        ElseIf oItemid = "MtxAux" Then
                            oGFun.DeleteRow(oMatAux, oDBDSAux)
                        ElseIf oItemid = "MtxSpare" Then
                            oGFun.DeleteRow(oMatSpare, oDBDSSpare)
                        End If
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
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatSpecify, "SpecNo", oDBDSSpecify)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatInsert, "Insertno", oDBDSInsert)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatAux, "1", oDBDSAux)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatSpare, "1", oDBDSSpare)

                    End If
                    'If oMatSpecify.VisualRowCount > 1 Then
                    '    oGFun.SetNewLine(oMatSpecify, oDBDSSpecify, oMatSpecify.VisualRowCount, "SpecNo")
                    'Else
                    '    oGFun.SetNewLine(oMatSpecify, oDBDSSpecify)
                    'End If
                    'If oMatInsert.VisualRowCount > 1 Then
                    '    oGFun.SetNewLine(oMatInsert, oDBDSInsert, oMatInsert.VisualRowCount, "Insertno")
                    'Else
                    '    oGFun.SetNewLine(oMatInsert, oDBDSInsert)
                    'End If

                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess = True Then
                        oGFun.SetNewLine(oMatSpecify, oDBDSSpecify, oMatSpecify.VisualRowCount, "SpecNo")
                        oGFun.SetNewLine(oMatInsert, oDBDSInsert, oMatInsert.VisualRowCount, "Insertno")
                        oGFun.SetNewLine(oMatAux, oDBDSAux, oMatAux.VisualRowCount, "1")
                        oGFun.SetNewLine(oMatSpare, oDBDSSpare, oMatSpare.VisualRowCount, "1")
                    End If
                    Dim Query As String
                    If HANA Then
                        Query = "select * from ""@MIPL_PM_MM"" where ""U_DocDate""=(Select Max(""U_DocDate"") from ""@MIPL_PM_MM"" where ""U_MCode""='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and ""U_MCode""='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "' order by ""Code"" desc"
                    Else
                        Query = "select * from [@MIPL_PM_MM] where U_DocDate=(Select Max(U_DocDate) from [@MIPL_PM_MM] where U_MCode='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and U_MCode='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "' order by Code desc"
                    End If
                    Dim RSet As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    RSet.DoQuery(Query)
                    If RSet.RecordCount > 0 Then
                        If Trim(oDBDSHeader.GetValue("U_Vibrate", 0)) = 0 Or Trim(oDBDSHeader.GetValue("U_Temp", 0)) = 0 Or Trim(oDBDSHeader.GetValue("U_Pressure", 0)) = 0 Or Trim(oDBDSHeader.GetValue("U_RunKM", 0)) = 0 Or Trim(oDBDSHeader.GetValue("U_RunHours", 0)) = 0 Then
                            oDBDSHeader.SetValue("U_Vibrate", 0, Trim(RSet.Fields.Item("U_Vibrate").Value.ToString))
                            oDBDSHeader.SetValue("U_Temp", 0, Trim(RSet.Fields.Item("U_Temp").Value.ToString))
                            oDBDSHeader.SetValue("U_Pressure", 0, Trim(RSet.Fields.Item("U_Pressure").Value.ToString))
                            oDBDSHeader.SetValue("U_RunKM", 0, Trim(RSet.Fields.Item("U_RunKM").Value.ToString))
                            oDBDSHeader.SetValue("U_RunHours", 0, Trim(RSet.Fields.Item("U_RunHrs").Value.ToString))
                            If frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            frmMachineMaster.Items.Item("1").Click()
                        End If
                    End If
                    RSet = Nothing
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
                    DeleteRowItemUID = EventInfo.ItemUID
                    If EventInfo.BeforeAction Then
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmMachineMaster.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            'frmMachineMaster.EnableMenu("1283", True) 'Remove
                        End If
                        Select Case EventInfo.ItemUID
                            Case "Specify"
                                If EventInfo.Row = oMatSpecify.VisualRowCount Then
                                    frmMachineMaster.EnableMenu("1293", False)
                                Else
                                    frmMachineMaster.EnableMenu("1293", True)
                                End If
                            Case "Insert"
                                If EventInfo.Row = oMatSpecify.VisualRowCount Then
                                    frmMachineMaster.EnableMenu("1293", False)
                                Else
                                    frmMachineMaster.EnableMenu("1293", True)
                                End If
                            Case "MtxAux"
                                If EventInfo.Row = oMatAux.VisualRowCount Then
                                    frmMachineMaster.EnableMenu("1293", False)
                                Else
                                    frmMachineMaster.EnableMenu("1293", True)
                                End If
                            Case "MtxSpare"
                                If EventInfo.Row = oMatSpare.VisualRowCount Then
                                    frmMachineMaster.EnableMenu("1293", False)
                                Else
                                    frmMachineMaster.EnableMenu("1293", True)
                                End If
                        End Select
                        frmMachineMaster.EnableMenu("1287", False)
                        oItemid = EventInfo.ItemUID
                        If frmMachineMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE And EventInfo.BeforeAction Then
                            If EventInfo.ItemUID = "Specify" And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                frmMachineMaster.EnableMenu("1293", True)
                                Rowid = EventInfo.Row
                            End If
                            If EventInfo.ItemUID = "Insert" And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                frmMachineMaster.EnableMenu("1293", True)
                                Rowid = EventInfo.Row
                            End If
                            If EventInfo.ItemUID = "MtxAux" And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                frmMachineMaster.EnableMenu("1293", True)
                                Rowid = EventInfo.Row
                            End If
                            If EventInfo.ItemUID = "MtxSpare" And frmMachineMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                frmMachineMaster.EnableMenu("1293", True)
                                Rowid = EventInfo.Row
                            End If
                        End If
                    Else
                        frmMachineMaster.EnableMenu("1293", False)
                        frmMachineMaster.EnableMenu("1283", False) 'Remove
                    End If
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

 
End Class
