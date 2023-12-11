
Public Class VehicleMaster
    Dim frmVehicleMaster As SAPbouiCOM.Form
    Dim oDBDSHeader, oDBDSDetail, oDBDSAux, oDBDSSpare As SAPbouiCOM.DBDataSource
    Dim oMatrix, oMatrixAux, oMatrixSpare As SAPbouiCOM.Matrix
    Dim UDOID As String = "MIVHL"
    Dim oItemid As String
    Dim DeleteRowItemUID As String = ""
    Sub LoadVehicleMaster()
        Try
            oGFun.LoadXML(frmVehicleMaster, VehicleMasterFormID, VehicleMasterXML)
            frmVehicleMaster = oApplication.Forms.Item(VehicleMasterFormID)
            oDBDSHeader = frmVehicleMaster.DataSources.DBDataSources.Item(0)
            oDBDSDetail = frmVehicleMaster.DataSources.DBDataSources.Item(1)
            oDBDSAux = frmVehicleMaster.DataSources.DBDataSources.Item(2)
            oDBDSSpare = frmVehicleMaster.DataSources.DBDataSources.Item(3)
            oMatrix = frmVehicleMaster.Items.Item("m_attach").Specific
            oMatrixAux = frmVehicleMaster.Items.Item("MtxAux").Specific
            oMatrixSpare = frmVehicleMaster.Items.Item("MtxSpare").Specific
            Me.DefineModesForFields()
            Me.InitForm()
        Catch ex As Exception

        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmVehicleMaster.Items.Item("t_itemname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleMaster.Items.Item("t_cardname").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleMaster.Items.Item("t_validity").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmVehicleMaster.Items.Item("t_itemcode").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmVehicleMaster.Items.Item("t_chassis").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            ' frmVehicleMaster.Items.Item("t_name").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 2, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'frmVehicleMaster.Items.Item("t_name").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

        Catch ex As Exception
            oApplication.StatusBar.SetText("DefineModesForFields Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub

    Sub InitForm()
        Try
            frmVehicleMaster.Freeze(True)
            'If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then _
            If HANA Then
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("""@MIPL_PM_OVHL"""))
                'oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_itmgrp").Specific, "SELECT ""ItmsGrpCod"",""ItmsGrpNam"" FROM OITB")
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_itmgrp").Specific, "SELECT ""Code"",""Name"" FROM ""@MIPL_PM_ITGRP""")
                'oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_make").Specific, "select ""FirmCode"",""FirmName"" from OMRC")
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_make").Specific, "select ""Code"",""Name"" from ""@MIPL_PM_OMRC"" ")
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_cntryorg").Specific, "select ""Code"",""Name"" from OCRY")
            Else
                oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OVHL]"))
                'oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_itmgrp").Specific, "SELECT ItmsGrpCod,ItmsGrpNam FROM OITB") 'load itemgrpname
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_itmgrp").Specific, "SELECT ""Code"",""Name"" FROM [@MIPL_PM_ITGRP]")
                'oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_make").Specific, "select FirmCode,FirmName from [OMRC]")
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_make").Specific, "select Code,Name from [@MIPL_PM_OMRC]")
                ' If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then oDBDSHeader.SetValue("Code", 0, oGFun.GetCodeGeneration("[@MIPL_PM_OVHL]"))
                oGFun.setComboBoxValue(frmVehicleMaster.Items.Item("c_cntryorg").Specific, "select Code,Name from [OCRY]")
            End If
            oGFun.SetNewLine(oMatrixAux, oDBDSAux)
            oGFun.SetNewLine(oMatrixSpare, oDBDSSpare)
            oGFun.LoadLocationComboBox(frmVehicleMaster.Items.Item("c_location").Specific) ' Load the location Combo Box...
            frmVehicleMaster.ActiveItem = "c_location"
            frmVehicleMaster.PaneLevel = 2
            frmVehicleMaster.Freeze(False)
        Catch ex As Exception
            oApplication.StatusBar.SetText("InitForm Method Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            frmVehicleMaster.Freeze(False)
        Finally
        End Try
    End Sub
    Function ValidateAll() As Boolean
        Try
            If Not oGFun.isDateCompare(frmVehicleMaster.Items.Item("94").Specific, frmVehicleMaster.Items.Item("96").Specific, "Start Date Should Not Be Greater Than End date  ") Then Exit Function

            'ItemCode..
            If frmVehicleMaster.Items.Item("t_itemcode").Specific.value.Equals("") = True Then
                oApplication.StatusBar.SetText("ItemCode code Should Not Be Left Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If Trim(frmVehicleMaster.Items.Item("txtWhse").Specific.Value).Equals("") = True Then
                oApplication.StatusBar.SetText("Default Warehouse Should Not Be Left Empty!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            Dim GetDup As String
            If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                If Not frmVehicleMaster.Items.Item("c_itmgrp").Specific.Selected Is Nothing Then
                    If HANA Then
                        GetDup = oGFun.getSingleValue("SELECT count(""U_ItemCode"") FROM ""@MIPL_PM_OVHL"" where ""U_ItemCode""='" & frmVehicleMaster.Items.Item("t_itemcode").Specific.value & "' and ""U_Itmgrp""='" & frmVehicleMaster.Items.Item("c_itmgrp").Specific.Selected.value & "' group by ""U_ItemCode"" having count(""U_ItemCode"")>0")
                    Else
                        GetDup = oGFun.getSingleValue("SELECT count(U_ItemCode) FROM [@MIPL_PM_OVHL] where U_ItemCode='" & frmVehicleMaster.Items.Item("t_itemcode").Specific.value & "' and U_Itmgrp='" & frmVehicleMaster.Items.Item("c_itmgrp").Specific.Selected.value & "' group by U_ItemCode having count(U_ItemCode)>0")
                    End If
                    If IIf(GetDup = "", 0, GetDup) > 0 Then
                        oApplication.StatusBar.SetText("Duplicate found.Please check the ItemCode!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If
            End If


            'Chassis No..
            'If frmVehicleMaster.Items.Item("t_chassis").Specific.value.Equals("") = True Then
            '    oApplication.StatusBar.SetText("Chassis No Should Not Be Left Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    Return False
            'End If
            'If frmVehicleMaster.Items.Item("t_name").Specific.value.Equals("") = False And frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
            '    If oGFun.isDuplicate(frmVehicleMaster.Items.Item("t_name").Specific, "[@MIPL_PM_OCAT]", "Name", "Category Name") = False Then
            '        Return False
            '    End If
            'End If
            ValidateAll = True
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
                                Case "t_itemcode"
                                    'If pVal.BeforeAction = False Then
                                    'Dim StrQry As String
                                    'If HANA Then
                                    '    StrQry = "SELECT '" & oDBDSHeader.GetValue("U_Itmgrp", 0).Trim & "' ""ItmsGrpCod"" from OITM "
                                    'Else
                                    '    StrQry = "SELECT '" & oDBDSHeader.GetValue("U_Itmgrp", 0).Trim & "' ItmsGrpCod from OITM"
                                    'End If
                                    'oGFun.ChooseFromListFilteration(frmVehicleMaster, "ITEM_CFL", "ItmsGrpCod", StrQry)
                                    'oGFun.ChooseFromLisBefore2ColAlias(frmVehicleMaster, "ITEM_CFL", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "InvntItem", "Y")
                                    'oGFun.ChooseFromLisBefore(frmVehicleMaster, "ITEM_CFL", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim)
                                    oGFun.CFLConditionForHeaderItem(frmVehicleMaster, "ITEM_CFL", "U_IGRP", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim)
                                    'End If
                                Case "t_vehctyp"
                                    'If pVal.BeforeAction = False Then
                                    '    If HANA Then
                                    '        oGFun.ChooseFromListFilteration(frmVehicleMaster, "VEHCFL", "Name", "select ""Name"" from ""@MIPL_PM_OCAT"" where ""U_Type""='VH'")
                                    '    Else
                                    '        oGFun.ChooseFromListFilteration(frmVehicleMaster, "VEHCFL", "Name", "select Name from [@MIPL_PM_OCAT] where U_Type='VH'")
                                    '    End If
                                    'End If
                                    oGFun.ChooseFromLisBefore(frmVehicleMaster, "VEHCFL", "U_Type", "VH")
                                Case "MtxSpare"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqualInMatrix(frmVehicleMaster, oMatrixSpare, "CSpare", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            oGFun.CFLConditionForLineItem(frmVehicleMaster, oMatrixSpare, "CSpare", "U_IGRP", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            'oGFun.ChooseFromLisBefore(frmVehicleMaster, "CSpare", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim)
                                    End Select
                                Case "MtxAux"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqualInMatrix(frmVehicleMaster, oMatrixAux, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            oGFun.CFLConditionForLineItem(frmVehicleMaster, oMatrixAux, "CAux", "U_IGRP", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim, "1")
                                            'oGFun.ChooseFromLisBefore2ColAliasNotEqual(frmVehicleMaster, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim, "ItemCode", oDBDSHeader.GetValue("U_ItemCode", 0).Trim)
                                            'oGFun.ChooseFromLisBefore(frmVehicleMaster, "CAux", "ItmsGrpCod", oDBDSHeader.GetValue("U_Itmgrp", 0).Trim)
                                    End Select
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Select Case pVal.ItemUID
                            Case "1"
                                If pVal.BeforeAction = True And (frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
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
                            Dim oDataTable As SAPbouiCOM.DataTable
                            Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                            oDataTable = oCFLE.SelectedObjects
                            Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            If Not oDataTable Is Nothing And pVal.BeforeAction = False And frmVehicleMaster.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                Select Case pVal.ItemUID
                                    Case "t_itemcode"
                                        oDBDSHeader.SetValue("U_ItemCode", 0, Trim(oDataTable.GetValue("ItemCode", 0)))
                                        oDBDSHeader.SetValue("U_ItemName", 0, Trim(oDataTable.GetValue("ItemName", 0)))
                                        Dim Query As String
                                        If HANA Then
                                            Query = "select * from ""@MIPL_PM_MM"" where ""U_DocDate""=(Select Max(""U_DocDate"") from ""@MIPL_PM_MM"" where ""U_MCode""='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and ""U_MCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' order by ""Code"" desc"
                                        Else
                                            Query = "select * from [@MIPL_PM_MM] where U_DocDate=(Select Max(U_DocDate) from [@MIPL_PM_MM] where U_MCode='" & Trim(oDBDSHeader.GetValue("U_ItemCode", 0)) & "') and U_MCode='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' order by Code desc"
                                        End If
                                        rset.DoQuery(Query)
                                        If rset.RecordCount > 0 Then
                                            oDBDSHeader.SetValue("U_Vibrate", 0, Trim(rset.Fields.Item("U_Vibrate").Value.ToString))
                                            oDBDSHeader.SetValue("U_Temp", 0, Trim(rset.Fields.Item("U_Temp").Value.ToString))
                                            oDBDSHeader.SetValue("U_Pressure", 0, Trim(rset.Fields.Item("U_Pressure").Value.ToString))
                                            oDBDSHeader.SetValue("U_RunKM", 0, Trim(rset.Fields.Item("U_RunKM").Value.ToString))
                                            oDBDSHeader.SetValue("U_RunHours", 0, Trim(rset.Fields.Item("U_RunHrs").Value.ToString))
                                        End If
                                        If HANA Then
                                            Query = "SELECT ""FirmName"" FROM OMRC WHERE ""FirmCode"" =(SELECT ""FirmCode""  FROM OITM WHERE ""ItemCode""='" & oDBDSHeader.GetValue("U_ItemCode", 0).Trim & "') "
                                        Else
                                            Query = "SELECT FirmName FROM OMRC WHERE FirmCode =(SELECT FirmCode  FROM OITM WHERE ItemCode='" & oDBDSHeader.GetValue("U_ItemCode", 0).Trim & "') "
                                        End If
                                        rset.DoQuery(Query)
                                        oDBDSHeader.SetValue("U_Make", 0, rset.Fields.Item("FirmName").Value)
                                    Case "t_cardcode"
                                        oDBDSHeader.SetValue("U_CardCode", 0, Trim(oDataTable.GetValue("CardCode", 0)))
                                        oDBDSHeader.SetValue("U_CardName", 0, Trim(oDataTable.GetValue("CardName", 0)))
                                    Case "t_vehctyp"
                                        oDBDSHeader.SetValue("U_VechType", 0, Trim(oDataTable.GetValue("Name", 0)))
                                        oDBDSHeader.SetValue("U_CatCode", 0, Trim(oDataTable.GetValue("Code", 0)))
                                    Case "MtxAux"
                                        Select Case pVal.ColUID
                                            Case "1"
                                                oMatrixAux.FlushToDataSource()
                                                oDBDSAux.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                oDBDSAux.SetValue("U_ItemDesc", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                oDBDSAux.SetValue("U_Quant", pVal.Row - 1, "1")
                                                oMatrixAux.LoadFromDataSource()
                                                oMatrixAux.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End Select
                                        oMatrixAux.AutoResizeColumns()
                                    Case "MtxSpare"
                                        Dim ItmGrp As String
                                        Dim objRs As SAPbobsCOM.Recordset
                                        Select Case pVal.ColUID
                                            Case "1"
                                                oMatrixSpare.FlushToDataSource()
                                                oDBDSSpare.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                oDBDSSpare.SetValue("U_ItemDesc", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                oDBDSSpare.SetValue("U_Quant", pVal.Row - 1, "1")
                                                ItmGrp = oGFun.getSingleValue("select ""ItmsGrpNam"" from OITB where ""ItmsGrpCod""='" & Trim(oDataTable.GetValue("ItmsGrpCod", 0)) & "'")
                                                oDBDSSpare.SetValue("U_ItmGrp", pVal.Row - 1, ItmGrp)
                                                oDBDSSpare.SetValue("U_UOM", pVal.Row - 1, Trim(oDataTable.GetValue("InvntryUom", 0)))
                                                If frmVehicleMaster.Items.Item("txtWhse").Specific.String <> "" Then
                                                    objRs = oGFun.DoQuery("select ""OnHand"",""MinStock"",""MaxStock"" from OITW where ""ItemCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and ""WhsCode""='" & frmVehicleMaster.Items.Item("txtWhse").Specific.String & "'")
                                                    If objRs.RecordCount > 0 Then
                                                        'oDBDSSpare.SetValue("U_MIN", pVal.Row - 1, objRs.Fields.Item("MinStock").Value)
                                                        'oDBDSSpare.SetValue("U_MAX", pVal.Row - 1, objRs.Fields.Item("MaxStock").Value)
                                                        oDBDSSpare.SetValue("U_InStock", pVal.Row - 1, objRs.Fields.Item("OnHand").Value)
                                                    End If
                                                End If
                                                oMatrixSpare.LoadFromDataSource()
                                                oMatrixSpare.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End Select
                                        oMatrixSpare.AutoResizeColumns()
                                    Case "txtWhse"
                                        oDBDSHeader.SetValue("U_Whse", 0, Trim(oDataTable.GetValue("WhsCode", 0)))
                                      
                                End Select
                            End If

                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Choose From List Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                        Try
                            Select Case pVal.ItemUID
                                Case "MtxAux"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            oGFun.SetNewLine(oMatrixAux, oDBDSAux, pVal.Row, pVal.ColUID)
                                    End Select
                                Case "MtxSpare"
                                    Select Case pVal.ColUID
                                        Case "1"
                                            oGFun.SetNewLine(oMatrixSpare, oDBDSSpare, pVal.Row, pVal.ColUID)
                                    End Select
                                Case "txtWhse"
                                    Dim objRs As SAPbobsCOM.Recordset
                                    If frmVehicleMaster.Items.Item("txtWhse").Specific.String <> "" Then
                                        If oMatrixSpare.RowCount > 0 Then
                                            For i As Integer = 1 To oMatrixSpare.RowCount
                                                objRs = oGFun.DoQuery("select ""OnHand"",""MinStock"",""MaxStock"" from OITW where ""ItemCode""='" & Trim(oMatrixSpare.Columns.Item("1").Cells.Item(i).Specific.String) & "' and ""WhsCode""='" & frmVehicleMaster.Items.Item("txtWhse").Specific.String & "'")
                                                If objRs.RecordCount > 0 Then
                                                    oMatrixSpare.Columns.Item("9").Cells.Item(i).Specific.String = objRs.Fields.Item("MinStock").Value
                                                    oMatrixSpare.Columns.Item("10").Cells.Item(i).Specific.String = objRs.Fields.Item("MaxStock").Value
                                                    oMatrixSpare.Columns.Item("11").Cells.Item(i).Specific.String = objRs.Fields.Item("OnHand").Value
                                                End If
                                            Next
                                            
                                        End If
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Lost Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        'Try
                        '    Select Case pVal.ItemUID
                        '        Case "t_itemcode"
                        '            If pVal.BeforeAction = False Then
                        '                Dim StrQry As String
                        '                If HANA Then
                        '                    StrQry = "SELECT '" & oDBDSHeader.GetValue("U_Itmgrp", 0).Trim & "' ""ItmsGrpCod"" from OITM "
                        '                Else
                        '                    StrQry = "SELECT '" & oDBDSHeader.GetValue("U_Itmgrp", 0).Trim & "' ItmsGrpCod from OITM"
                        '                End If

                        '                oGFun.ChooseFromListFilteration(frmVehicleMaster, "ITEM_CFL", "ItmsGrpCod", StrQry)
                        '            End If
                        '        Case "t_vehctyp"
                        '            If pVal.BeforeAction = False Then
                        '                If HANA Then
                        '                    oGFun.ChooseFromListFilteration(frmVehicleMaster, "VEHCFL", "Name", "select ""Name"" from ""@MIPL_PM_OCAT"" where ""U_Type""='VH'")
                        '                Else
                        '                    oGFun.ChooseFromListFilteration(frmVehicleMaster, "VEHCFL", "Name", "select Name from [@MIPL_PM_OCAT] where U_Type='VH'")
                        '                End If

                        '            End If
                        '    End Select
                        'Catch ex As Exception
                        '    oApplication.StatusBar.SetText("Got Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'Finally
                        'End Try
                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                        Try
                            Select Case pVal.ItemUID
                                Case "96"
                                    If pVal.BeforeAction = False Then
                                        If Not oGFun.isDateCompare(frmVehicleMaster.Items.Item("94").Specific, frmVehicleMaster.Items.Item("96").Specific, "Start Date Should Not Be Greater Than End date  ") Then Exit Sub

                                    End If

                            End Select

                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Validate Focus Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                        Try
                            Select Case pVal.ItemUID
                                Case "c_itmgrp"
                                    If pVal.BeforeAction = False And pVal.ItemChanged Then
                                        frmVehicleMaster.Items.Item("t_itemcode").Specific.value = ""
                                        frmVehicleMaster.Items.Item("t_itemname").Specific.value = ""
                                        oDBDSHeader.SetValue("U_Make", 0, "")
                                    End If

                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Combo Select Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "b_browse"
                                    If pVal.BeforeAction Then If frmVehicleMaster.Items.Item(pVal.ItemUID).Enabled = False Then BubbleEvent = False
                                    If pVal.ActionSuccess Then
                                        If oMatrix.RowCount <> 0 Then
                                            If oMatrix.Columns.Item("trgtpath").Cells.Item(1).Specific.value.Equals("") Then
                                                oMatrix.Clear()
                                                oDBDSDetail.Clear()
                                            End If
                                        End If
                                        If oGFun.SetAttachMentFile(frmVehicleMaster, oDBDSHeader, oMatrix, oDBDSDetail) = False Then
                                            BubbleEvent = False
                                        End If
                                    End If
                                Case "b_display"
                                    If pVal.BeforeAction Then If frmVehicleMaster.Items.Item(pVal.ItemUID).Enabled = False Then BubbleEvent = False
                                    If pVal.ActionSuccess Then oGFun.OpenAttachment(oMatrix, oDBDSDetail, pVal.Row)

                                Case "b_delete"
                                    If pVal.BeforeAction Then If frmVehicleMaster.Items.Item(pVal.ItemUID).Enabled = False Then BubbleEvent = False
                                    If pVal.ActionSuccess Then
                                        oGFun.DeleteRowAttachment(frmVehicleMaster, oMatrix, oDBDSDetail, oMatrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder))
                                    End If
                                Case "m_attach"
                                    If pVal.BeforeAction = False And pVal.Row > 0 Then
                                        If oMatrix.IsRowSelected(pVal.Row) Then
                                            frmVehicleMaster.Items.Item("b_display").Enabled = True
                                            frmVehicleMaster.Items.Item("b_delete").Enabled = True
                                        End If
                                    End If
                            End Select
                        Catch ex As Exception
                            oGFun.StatusBarErrorMsg("Click Event Failed:" & ex.Message)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "m_attach"
                                    If pVal.BeforeAction = False Then oGFun.OpenAttachment(oMatrix, oDBDSDetail, pVal.Row)
                            End Select
                        Catch ex As Exception
                            oGFun.StatusBarErrorMsg("Item Double Click Event Failed : " & ex.Message)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.ActionSuccess And frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "f_register"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 4
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "f_insuranc"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 6
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "f_specific"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 2
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "f_purchase"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 3
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "f_attach"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 9
                                        frmVehicleMaster.Settings.MatrixUID = "m_attach"
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "ChildAux"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 10
                                        frmVehicleMaster.Settings.MatrixUID = "MtxAux"
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "Spare"
                                    If pVal.ActionSuccess Then
                                        frmVehicleMaster.Freeze(True)
                                        frmVehicleMaster.PaneLevel = 11
                                        frmVehicleMaster.Settings.MatrixUID = "MtxSpare"
                                        frmVehicleMaster.Freeze(False)
                                    End If
                                Case "ck_sctn"
                                    If pVal.BeforeAction = False Then
                                        Dim ockh As SAPbouiCOM.CheckBox = frmVehicleMaster.Items.Item("ck_sctn").Specific
                                        If ockh.Checked = True Then
                                            frmVehicleMaster.Items.Item("t_validity").Enabled = True
                                        Else
                                            frmVehicleMaster.Items.Item("t_validity").Enabled = False
                                        End If
                                    End If
                                Case "lk_vehtype"
                                    If pVal.BeforeAction = False Then
                                        Dim typvalue As String = frmVehicleMaster.Items.Item("t_vehctyp").Specific.value
                                        oApplication.ActivateMenuItem("OCAT")
                                        oForm = oApplication.Forms.Item("OCAT")
                                        oForm.Select()
                                        oForm.Freeze(True)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                        oForm.Items.Item("txt_cat").Enabled = True
                                        oForm.Items.Item("txt_cat").Specific.Value = Trim(typvalue)
                                        oForm.Items.Item("1").Click()
                                        oForm.Freeze(False)
                                    End If
                            End Select
                        Catch ex As Exception
                            oApplication.StatusBar.SetText("Item Pressed Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Finally
                        End Try
                End Select
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText("ItemEvent Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
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
                        oForm.Items.Item("t_pecidno").Enabled = True
                        oMatrixAux.Item.Enabled = False
                        oMatrixSpare.Item.Enabled = False
                    Case "1282"
                        Me.InitForm()
                    Case "1293"
                        'oGFun.DeleteRow(oMatrix, oDBDSDetail1)
                        If oItemid = "MtxAux" Then
                            oGFun.DeleteRow(oMatrixAux, oDBDSAux)
                        ElseIf oItemid = "MtxSpare" Then
                            oGFun.DeleteRow(oMatrixSpare, oDBDSSpare)
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
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrixAux, "1", oDBDSAux)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrixSpare, "1", oDBDSSpare)
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.ActionSuccess = True Then
                        oGFun.SetNewLine(oMatrixAux, oDBDSAux, oMatrixAux.VisualRowCount, "1")
                        oGFun.SetNewLine(oMatrixSpare, oDBDSSpare, oMatrixSpare.VisualRowCount, "1")
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
                            If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            frmVehicleMaster.Items.Item("1").Click()
                        End If
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
                    'If frmVehicleMaster.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE Then _
                    '      frmVehicleMaster.EnableMenu("1287", True)
                    DeleteRowItemUID = EventInfo.ItemUID
                    If EventInfo.BeforeAction = True Then
                        Try
                            If EventInfo.ItemUID <> "" Then
                                frmVehicleMaster.EnableMenu("772", True)
                            Else
                            End If
                        Catch ex As Exception
                        End Try
                        If frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmVehicleMaster.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            'frmVehicleMaster.EnableMenu("1283", True) 'Remove
                        End If

                        Select Case EventInfo.ItemUID
                            Case "MtxAux"
                                If EventInfo.Row = oMatrixAux.VisualRowCount Then
                                    frmVehicleMaster.EnableMenu("1293", False)
                                Else
                                    frmVehicleMaster.EnableMenu("1293", True)
                                End If

                            Case "MtxSpare"
                                If EventInfo.Row = oMatrixSpare.VisualRowCount Then
                                    frmVehicleMaster.EnableMenu("1293", False)
                                Else
                                    frmVehicleMaster.EnableMenu("1293", True)
                                End If
                        End Select

                    Else
                        frmVehicleMaster.EnableMenu("1293", False)
                        'frmVehicleMaster.EnableMenu("1283", False) 'Remove
                    End If


                    oItemid = EventInfo.ItemUID
            End Select
        Catch ex As Exception
            oApplication.StatusBar.SetText("Right Click Event Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Finally
        End Try
    End Sub
End Class

