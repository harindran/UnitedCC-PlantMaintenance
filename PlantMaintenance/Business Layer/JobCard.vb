Class JobCard
    Dim frmJobCard As SAPbouiCOM.Form
    Dim oDBDSHeader, oDBDSDetail1, oDBDSDetail2, oDBDSDetail3 As SAPbouiCOM.DBDataSource
    Dim oMatrix1, oMatrix2, oMatrix3 As SAPbouiCOM.Matrix
    Dim UDOID As String = "OJOC"
    Dim sDelRowMatrix As String = ""
    Dim boolstatus As Boolean
    Dim StrQuery As String = ""
    Dim link As SAPbouiCOM.LinkedButton
    Dim cmb_status As SAPbouiCOM.ComboBox
    Dim Folder2 As SAPbouiCOM.Folder
    Dim SQuery As String = ""
    Sub LoadJobCard()
        Try
            oGFun.LoadXML(frmJobCard, JobCardFormID, JobCardXML)
            frmJobCard = oApplication.Forms.Item(JobCardFormID)
            setReport(JobCardFormID)
            oDBDSHeader = frmJobCard.DataSources.DBDataSources.Item(0)
            oDBDSDetail1 = frmJobCard.DataSources.DBDataSources.Item(1)
            oDBDSDetail2 = frmJobCard.DataSources.DBDataSources.Item(2)
            oDBDSDetail3 = frmJobCard.DataSources.DBDataSources.Item(3)
            oMatrix1 = frmJobCard.Items.Item("Matrix1").Specific
            oMatrix2 = frmJobCard.Items.Item("Matrix2").Specific
            oMatrix3 = frmJobCard.Items.Item("Matrix3").Specific
            Folder2 = frmJobCard.Items.Item("f_reprpart").Specific
            frmJobCard.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Add, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Dim caption As String
            If HANA Then
                caption = oGFun.getSingleValue("select distinct T1.""DimDesc"" from OPRC T0 inner join ODIM T1 on T0.""DimCode""=T1.""DimCode"" where ifnull(T1.""DimActive"",'Y')='Y' and T1.""DimCode""='1'")
            Else
                caption = oGFun.getSingleValue("select distinct T1.DimDesc from OPRC T0 inner join ODIM T1 on T0.DimCode=T1.DimCode where isnull(T1.DimActive,'Y')='Y' and T1.DimCode='1'")
            End If
            oMatrix2.Columns.Item("dimcode").TitleObject.Caption = caption
            Me.DefineModesForFields()
            frmJobCard.PaneLevel = 1
            Me.InitForm()
            'Dim objcombo As SAPbouiCOM.ComboBox
            'objcombo = frmJobCard.Items.Item("c_type").Specific
            'objcombo.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly
            'objcombo = frmJobCard.Items.Item("c_location").Specific
            'objcombo.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly
        Catch ex As Exception
            oGFun.Msg("Load Parameter Master Failed" & ex.Message)
        Finally
        End Try
    End Sub
    Sub InitForm()
        Try
            oGFun.LoadComboBoxSeries(frmJobCard.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
            oGFun.LoadDocumentDate(frmJobCard.Items.Item("t_docdate").Specific) ' Load Document Date
            oGFun.LoadLocationComboBox(frmJobCard.Items.Item("c_location").Specific) ' Load the location Combo Box...
            If HANA Then
                oGFun.setComboBoxValue(frmJobCard.Items.Item("c_vhltype").Specific, "Select ""U_TypeCode"", ""U_TypeName"" from ""@MIPL_PM_TYPE"" ")
            Else
                oGFun.setComboBoxValue(frmJobCard.Items.Item("c_vhltype").Specific, "Select U_TypeCode,U_TypeName from [@MIPL_PM_TYPE] ")
            End If
            oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
            oGFun.SetNewLine(oMatrix3, oDBDSDetail3)
            frmJobCard.ActiveItem = "c_location"
            'oMatrix2.Columns.Item("Stat").Visible = False
            oMatrix3.Columns.Item("Stat").Visible = False
        Catch ex As Exception
            oGFun.Msg("InitForm Method Failed:")
            frmJobCard.Freeze(False)
        Finally
        End Try
    End Sub

    Private Sub setReport(ByVal FormUID As String)
        Try
            frmJobCard = oApplication.Forms.Item(FormUID)
            Dim rptTypeService As SAPbobsCOM.ReportTypesService
            'Dim newType As SAPbobsCOM.ReportType
            Dim newtypesParam As SAPbobsCOM.ReportTypesParams
            rptTypeService = oCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)
            newtypesParam = rptTypeService.GetReportTypeList
            Dim TypeCode As String
            If HANA Then
                TypeCode = oGFun.getSingleValue("Select ""CODE"" from RTYP where ""NAME""='AJobCard'")
            Else
                TypeCode = oGFun.getSingleValue("Select CODE from RTYP where NAME='AJobCard'")
            End If
            frmJobCard.ReportType = TypeCode
            'Dim i As Integer
            'For i = 0 To newtypesParam.Count - 1
            '    If newtypesParam.Item(i).TypeName = "AJobCard" And newtypesParam.Item(i).MenuID = "AJobCard" Then
            '        frmJobCard.ReportType = newtypesParam.Item(i).TypeCode
            '        Exit For
            '    End If
            'Next i
        Catch ex As Exception
            oApplication.StatusBar.SetText("setReport Failed:" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End Try
    End Sub

    Sub DefineModesForFields()
        Try
            frmJobCard.Items.Item("t_docdate").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmJobCard.Items.Item("t_docnum").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            frmJobCard.Items.Item("c_series").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("c_location").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("c_type").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("t_servtype").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("c_vhltype").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("t_vehicno").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            frmJobCard.Items.Item("t_regexpdt").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'frmJobCard.Items.Item("c_status").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Add, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        Catch ex As Exception
            oGFun.Msg("DefineModesForFields Method Failed:")
        Finally
        End Try
    End Sub

    Function ValidateAll() As Boolean

        Try
            ''Machine Number
            'If oDBDSHeader.GetValue("U_ItemCode", 0).Equals(Trim("")) = True Then
            '    oGFun.StatusBarErrorMsg("M/C Equipment No. Should Not Be Left Empty")
            '    Return False
            'End If
            ''Corrective Actions
            'If (frmJobCard.Items.Item("t_cmpldet").Specific.value).Equals("") = True Then
            '    oGFun.StatusBarErrorMsg("Corrective Actions Should Not Be Left Empty")
            '    Return False
            'End If

            'If oDBDSHeader.GetValue("U_CmplDets", 0).Trim <> ("") = True Then
            '    If oDBDSHeader.GetValue("U_IsuByNam", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_IsuDate", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_IsuTime", 0).Equals(Trim("")) Then
            '        oGFun.StatusBarErrorMsg("Issued By  Or Issued Date or Issued Time Should Not Be Left Empty")
            '        Return False
            '    End If
            '    'End If
            'End If
            'If oDBDSHeader.GetValue("U_CrtAct", 0) <> (Trim("")) = True Then
            '    If oDBDSHeader.GetValue("U_PreByNam", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_PreDate", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_PreTime", 0).Equals(Trim("")) Then
            '        oGFun.StatusBarErrorMsg("Prepared By Or Prepared Date Or Prepared Time Should Not Be Left Empty")
            '        Return False
            '    End If
            'End If
            'If oDBDSHeader.GetValue("U_Analysis", 0) <> (Trim("")) = True Then
            '    If oDBDSHeader.GetValue("U_AnsByNam", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_AnsDate", 0).Equals(Trim("")) Or oDBDSHeader.GetValue("U_AnsTime", 0).Equals(Trim("")) Then
            '        oGFun.StatusBarErrorMsg("Analysis By Or Analysis Date Or Analysis Time Should Not Be Left Empty")
            '        Return False
            '    End If
            'End If
            'Grid validation
            'item consumption
            'If (frmJobCard.Items.Item("t_drivenam").Specific.value).Equals("") = True Then
            '    oGFun.StatusBarErrorMsg("Driver Name Should Not Be Left Empty")
            '    Return False
            'End If
            If (frmJobCard.Items.Item("t_hrskms").Specific.value).Equals("") = True Then
                oGFun.StatusBarErrorMsg("Current Hrs/Km Should Not Be Left Empty")
                Return False
            End If
            'If (frmJobCard.Items.Item("t_macnam").Specific.value).Equals("") = True Then
            '    oGFun.StatusBarErrorMsg("Chargehand Name Should Not Be Left Empty")
            '    Return False
            'End If
            If (frmJobCard.Items.Item("t_prebynam").Specific.value).Equals("") = True Then
                oGFun.StatusBarErrorMsg("PreparedBy Name Should Not Be Left Empty")
                Return False
            End If
            'If oMatrix2.VisualRowCount = 0 Then
            '    oGFun.StatusBarErrorMsg("Repair Parts Grid Should Not Be Left Empty")
            '    Return False
            'End If
            For i As Integer = 0 To oMatrix2.VisualRowCount - 2
                If oDBDSDetail2.GetValue("U_ItemCode", i).Trim.Equals("") Then
                    oGFun.StatusBarErrorMsg("ItemCode Should Not Be Left Empty in Line No : " & i + 1)
                    Return False
                End If
            Next
            'Manpower Grid
            'If oMatrix3.VisualRowCount > 1 = False Then
            '    oGFun.StatusBarErrorMsg("ManHour Cost Grid Should Not Be Left Empty")
            '    Return False
            'End If
            For i As Integer = 0 To oMatrix3.VisualRowCount - 2
                If oDBDSDetail3.GetValue("U_EmpName", i).Trim.Equals("") Then
                    oGFun.StatusBarErrorMsg("Employee Name Should Not Be Left Empty in Line No : " & i + 1)
                    Return False
                End If
                If oDBDSDetail3.GetValue("U_ManHrs", i).Trim > 0 = False Then
                    oGFun.StatusBarErrorMsg("Man Hours Should Be Greater than Zero in Line No : " & i + 1)
                    Return False
                End If
            Next
            'Status Closing warning Message ...
            Dim boolStatus As Boolean = False
            oMatrix1.FlushToDataSource()
            For i As Integer = 0 To oMatrix1.VisualRowCount - 1
                If Trim(oDBDSDetail1.GetValue("U_ActName", i)).Equals("") = False Then
                    If Trim(oDBDSDetail1.GetValue("U_Status", i)).Equals("P") Then
                        'oApplication.StatusBar.SetText("Line :" & i + 1 & " Status Should Not be Pending")
                        boolStatus = True
                        Return True 'Return False
                    End If
                End If
            Next

            'oMatrix1.FlushToDataSource()
            'For j As Integer = 0 To oMatrix2.VisualRowCount - 1
            '    If Trim(oDBDSDetail2.GetValue("U_ItemCode", j)).Equals("") = False Then
            '        If Trim(oDBDSDetail2.GetValue("U_Status", j)).Equals("P") Then
            '            boolStatus = True
            '            Return True
            '        End If
            '    End If
            'Next

            'oMatrix1.FlushToDataSource()
            If Trim(oDBDSHeader.GetValue("U_GINo", 0)).Equals("") Then
                boolStatus = True
                Return True
            End If
            'If Trim(oDBDSHeader.GetValue("U_JENo", 0)).Equals("") Then
            '    boolStatus = True
            '    Return True
            'End If
        
            '   oApplication.MessageBox("Do you want to close the JobCard Status? You cannot change the document", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, True)
            'If boolStatus = False And Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") Then If oApplication.MessageBox("Do you want to close the JobCard Status? You cannot change the document", 1, "Yes", "No") = 1 Then oDBDSHeader.SetValue("Status", 0, "C")



            Return True
        Catch ex As Exception
            oGFun.Msg("Validate all Function Failed: ")
        Finally
        End Try

    End Function

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction = True Then
                frmJobCard = oApplication.Forms.Item(FormUID)
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Select Case pVal.ItemUID
                                Case "t_drivenam"
                                    'oGFun.ChooseFromListFilteration(frmJobCard, "DVRCFL", "empID", "select empID from OHEM where dept='13' ")
                                Case "t_prebynam"
                                    'oGFun.ChooseFromListFilteration(frmJobCard, "PRECFL", "empID", "select empID from OHEM where dept='13' ")
                                Case "t_macnam"
                                    'oGFun.ChooseFromListFilteration(frmJobCard, "MECH_CFL", "empID", "select empID from OHEM where dept='13' ")
                                Case "Matrix2"
                                    Select Case pVal.ColUID
                                        Case "itemcode"
                                            'oGFun.ChooseFromLisBefore(frmJobCard, "ITMCFL", "InvntItem", "Y")
                                            ' oGFun.ChooseFromLisBefore2ColAliasNotEqual(frmJobCard, oMatrix2, "ITMCFL", "itemcode")
                                            oGFun.ChooseFromLisBefore_Spares(frmJobCard, "ITMCFL")
                                        Case "dimcode"
                                            oGFun.ChooseFromLisBefore(frmJobCard, "DIMCFL", "DimCode", "1")
                                    End Select
                                Case "Matrix3"
                                    Select Case pVal.ColUID
                                        Case "empid"
                                            If pVal.BeforeAction = False Then
                                                oGFun.ChooseFromListFilteration(frmJobCard, "MAN_CFL", "empID", "select empID from OHEM where dept='13' ")
                                            End If
                                    End Select
                                Case "t_brkdwno"
                                    If HANA Then
                                        SQuery = " SELECT ""DocNum"" FROM ""@MIPL_PM_OBDS"" WHERE ""Status"" = 'O'"
                                    Else
                                        SQuery = " SELECT DocNum FROM [@MIPL_PM_OBDS] WHERE Status = 'O'"
                                    End If

                                    oGFun.ChooseFromListFilteration(frmJobCard, "BDSCFL", "DocNum", SQuery)
                                Case "t_mainplno"
                                    If HANA Then
                                        SQuery = " SELECT ""Code"" FROM ""@MIPL_PM_OPCL"" where ""U_Type""='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "' and ""U_Active""='Y'"
                                    Else
                                        SQuery = " SELECT Code FROM [@MIPL_PM_OPCL] where U_Type='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "' and U_Active='Y'"
                                    End If
                                    oGFun.ChooseFromListFilteration(frmJobCard, "PMCFL", "Code", SQuery)
                                Case "t_vehicno"
                                    Dim oTxt As SAPbouiCOM.EditText = frmJobCard.Items.Item(pVal.ItemUID).Specific

                                    If Trim(oDBDSHeader.GetValue("U_VHLType", 0)).Equals("VH") Then
                                        If HANA Then
                                            StrQuery = "SELECT ""U_ItemCode"" FROM ""@MIPL_PM_OVHL"" "
                                        Else
                                            StrQuery = "SELECT U_ItemCode FROM [@MIPL_PM_OVHL] "
                                        End If

                                        oTxt.ChooseFromListUID = "OVHL_CFL"
                                        oTxt.ChooseFromListAlias = "U_ItemCode"
                                        oGFun.ChooseFromListFilteration(frmJobCard, "OVHL_CFL", "U_ItemCode", StrQuery)
                                    Else
                                        If HANA Then
                                            StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" Where ""U_InsType""='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                                        Else
                                            StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                                        End If

                                        oTxt.ChooseFromListUID = "OMAC_CFL"
                                        oTxt.ChooseFromListAlias = "Code"
                                        oGFun.ChooseFromListFilteration(frmJobCard, "OMAC_CFL", "Code", StrQuery)
                                    End If
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
                                        StrQuery = "Select * from (Select T0.""DocEntry"",T0.""DocNum"",T0.""DocDate"",T0.""U_JobNo"" as ""JobCardNum"",'Document' as ""TranType"",T1.""ItemCode"",T1.""Dscription"",T1.""Quantity"" from OIGE T0 join IGE1 T1 on T0.""DocEntry""=T1.""DocEntry"" "
                                        StrQuery += vbCrLf + "Union all"
                                        StrQuery += vbCrLf + "Select T0.""DocEntry"",T0.""DocNum"",T0.""DocDate"",T0.""U_JobNo"" as ""JobCardNum"",'Draft' as ""TranType"",T1.""ItemCode"",T1.""Dscription"",T1.""Quantity"" from ODRF T0 join DRF1 T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""DocStatus""='O') A"
                                        StrQuery += vbCrLf + "where A.""JobCardNum""='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "' order by A.""DocEntry"" "
                                    Else
                                        StrQuery = "Select * from (Select T0.DocEntry,T0.DocNum,T0.DocDate,T0.U_JobNo as JobCardNum,'Document' as TranType,T1.ItemCode,T1.Dscription,T1.Quantity from OIGE T0 join IGE1 T1 on T0.DocEntry=T1.DocEntry "
                                        StrQuery += vbCrLf + "Union all"
                                        StrQuery += vbCrLf + "Select T0.DocEntry,T0.DocNum,T0.DocDate,T0.U_JobNo as JobCardNum,'Draft' as TranType,T1.ItemCode,T1.Dscription,T1.Quantity from ODRF T0 join DRF1 T1 on T0.DocEntry=T1.DocEntry where T0.DocStatus='O') A"
                                        StrQuery += vbCrLf + "where A.JobCardNum='" & oDBDSHeader.GetValue("DocNum", 0).Trim & "' order by A.DocEntry "
                                    End If
                                    oTranDataFormID.LoadViewTranData(StrQuery, "60", "")
                                    '    link = frmJobCard.Items.Item("lkGI").Specific
                                    '    link.LinkedObjectType = "-1"
                                    '    Dim ActualEntry As String = ""
                                    '    If HANA Then
                                    '        ActualEntry = oGFun.getSingleValue("Select T0.""DocEntry"" from ODRF T0 where ifnull(T0.""DocStatus"",'')='O' and T0.""DocEntry""='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                                    '    Else
                                    '        ActualEntry = oGFun.getSingleValue("Select T0.DocEntry from ODRF T0 where isnull(T0.DocStatus,'')='O' and T0.DocEntry='" & oDBDSHeader.GetValue("U_GINo", 0).Trim & "'")
                                    '    End If
                                    '    If ActualEntry = "" Then
                                    '        link.LinkedObjectType = "60"
                                    '        link.Item.LinkTo = "txtGI"
                                    '    Else
                                    '        link.LinkedObjectType = "112"
                                    '        link.Item.LinkTo = "txtGI"
                                    '    End If
                                Catch ex As Exception
                                End Try
                        End Select
                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Select Case pVal.ItemUID
                            Case "Matrix2"
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
                        If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable
                            Dim oCFLE As SAPbouiCOM.ChooseFromListEvent = pVal
                            oDataTable = oCFLE.SelectedObjects
                            If Not oDataTable Is Nothing And pVal.BeforeAction = False Then
                                Select Case pVal.ItemUID
                                    Case "t_brkdwno"
                                        Dim rsetMcType As SAPbobsCOM.Recordset
                                        oDBDSHeader.SetValue("U_BrkDwNo", 0, Trim(oDataTable.GetValue("DocEntry", 0)))
                                        If HANA Then
                                            SQuery = " select ""U_Type"" from ""@MIPL_PM_OBDS"" where ""DocNum""='" & Trim(oDataTable.GetValue("DocNum", 0)) & "' "
                                        Else
                                            SQuery = " select U_Type from [@MIPL_PM_OBDS] where DocNum='" & Trim(oDataTable.GetValue("DocNum", 0)) & "' "
                                        End If

                                        Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        rset.DoQuery(SQuery)
                                        If rset.Fields.Item("U_Type").Value = "VH" Then
                                            If HANA Then
                                                SQuery = "SELECT ""Code"", ""U_ItemCode"", ""U_DatMul""  FROM ""@MIPL_PM_OVHL"" WHERE ""Code"" = '" & Trim(oDataTable.GetValue("U_ItemCode", 0)) & "'"
                                            Else
                                                SQuery = "SELECT Code, U_ItemCode, U_DatMul  FROM [@MIPL_PM_OVHL] WHERE Code = '" & Trim(oDataTable.GetValue("U_ItemCode", 0)) & "'"
                                            End If

                                            rsetMcType = oGFun.DoQuery(SQuery)
                                            If rsetMcType.RecordCount > 0 Then
                                                oDBDSHeader.SetValue("U_VHLType", 0, "VH")
                                                oDBDSHeader.SetValue("U_VHLNo", 0, Trim(rsetMcType.Fields.Item("Code").Value))
                                                oDBDSHeader.SetValue("U_VHLName", 0, Trim(rsetMcType.Fields.Item("U_ItemCode").Value))
                                                Dim a As String = CDate(rsetMcType.Fields.Item("U_DatMul").Value).ToString("yyyyMMdd")
                                                oDBDSHeader.SetValue("U_RegExpDt", 0, a)
                                            End If
                                        Else
                                            If HANA Then
                                                SQuery = "SELECT ""Code"", ""U_ItemCode"",""U_InsType"", ""U_DatMul""  FROM ""@MIPL_PM_OMAC"" WHERE ""Code"" = '" & Trim(oDataTable.GetValue("U_ItemCode", 0)) & "'"
                                            Else
                                                SQuery = "SELECT Code, U_ItemCode, U_InsType, U_DatMul  FROM [@MIPL_PM_OMAC] WHERE Code = '" & Trim(oDataTable.GetValue("U_ItemCode", 0)) & "'"
                                            End If
                                            rsetMcType = oGFun.DoQuery(SQuery)
                                            If rsetMcType.RecordCount > 0 Then
                                                oDBDSHeader.SetValue("U_VHLType", 0, "")
                                                oDBDSHeader.SetValue("U_VHLType", 0, Trim(rsetMcType.Fields.Item("U_InsType").Value))
                                                oDBDSHeader.SetValue("U_VHLNo", 0, Trim(rsetMcType.Fields.Item("Code").Value))
                                                oDBDSHeader.SetValue("U_VHLName", 0, Trim(rsetMcType.Fields.Item("U_ItemCode").Value))
                                                Dim a As String = CDate(rsetMcType.Fields.Item("U_DatMul").Value).ToString("yyyyMMdd")
                                                oDBDSHeader.SetValue("U_RegExpDt", 0, a)
                                            End If
                                        End If
                                        oDBDSHeader.SetValue("U_PrjCode", 0, Trim(oDataTable.GetValue("U_PrjCode", 0)))
                                        oDBDSHeader.SetValue("U_PrjName", 0, Trim(oDataTable.GetValue("U_PrjName", 0)))
                                        Me.LoadActionSuggest()
                                    Case "t_vehicno"
                                        oDBDSHeader.SetValue("U_VHLNo", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_VHLName", 0, Trim(oDataTable.GetValue("U_ItemCode", 0)))
                                    Case "t_mainplno"
                                        oDBDSHeader.SetValue("U_MnPlnCd", 0, Trim(oDataTable.GetValue("Code", 0)))
                                        oDBDSHeader.SetValue("U_MainPlNo", 0, Trim(oDataTable.GetValue("Name", 0)))
                                        'Me.LoadMaintenancePlanDets()
                                        Me.LoadPMChecklistDets()
                                    Case "PRJCFL"
                                        oDBDSHeader.SetValue("U_PrjCode", 0, Trim(oDataTable.GetValue("PrjCode", 0)))
                                        oDBDSHeader.SetValue("U_PrjName", 0, Trim(oDataTable.GetValue("PrjName", 0)))
                                    Case "t_prebynam"
                                        oDBDSHeader.SetValue("U_PreByCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_PreByNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                    Case "t_drivenam"
                                        oDBDSHeader.SetValue("U_DriveCod", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_DriveNam", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                    Case "t_project"
                                        oDBDSHeader.SetValue("U_PrjCode", 0, Trim(oDataTable.GetValue("PrjCode", 0)))
                                        oDBDSHeader.SetValue("U_PrjName", 0, Trim(oDataTable.GetValue("PrjName", 0)))
                                    Case "t_macnam"
                                        oDBDSHeader.SetValue("U_MechCode", 0, Trim(oDataTable.GetValue("empID", 0)))
                                        oDBDSHeader.SetValue("U_MechName", 0, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                    Case "Matrix1"
                                        Select Case pVal.ColUID
                                            Case "actcode", "actname"
                                                'Try
                                                '    Dim pCFL As SAPbouiCOM.ISBOChooseFromListEventArg
                                                '    Dim pVal1 As SAPbouiCOM.SBOItemEventArg
                                                '    pCFL = pVal1
                                                '    If Not pCFL.SelectedObjects Is Nothing Then
                                                '        Try
                                                '            oMatrix1.Columns.Item("actcode").Cells.Item(pVal.Row).Specific.String = pCFL.SelectedObjects.Columns.Item("CardCode").Cells.Item(0).Value
                                                '        Catch ex As Exception
                                                '        End Try

                                                '    End If
                                                '    oMatrix1.Columns.Item("actcode").Cells.Item(pVal.Row).Specific.String = Trim(oDataTable.GetValue("Code", 0))
                                                '    oMatrix1.Columns.Item("actname").Cells.Item(pVal.Row).Specific.String = Trim(oDataTable.GetValue("U_Activity", 0))
                                                'Catch ex As Exception
                                                'End Try
                                              
                                                oMatrix1.FlushToDataSource()
                                                oDBDSDetail1.SetValue("U_ActCode", pVal.Row - 1, Trim(oDataTable.GetValue("Code", 0)))
                                                oDBDSDetail1.SetValue("U_ActName", pVal.Row - 1, Trim(oDataTable.GetValue("U_Activity", 0)))
                                                oMatrix1.LoadFromDataSource()
                                            Case "mechname"
                                                oMatrix1.FlushToDataSource()
                                                oDBDSDetail1.SetValue("U_MechCode", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                                oDBDSDetail1.SetValue("U_MechName", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
                                                oMatrix1.LoadFromDataSource()
                                            Case "Prentry"
                                                oMatrix1.FlushToDataSource()
                                                'oDBDSDetail1.SetValue("LineId", pVal.Row - 1, pVal.Row)
                                                oDBDSDetail1.SetValue("U_PREntry", pVal.Row - 1, oDataTable.GetValue("DocEntry", 0))
                                                oMatrix1.LoadFromDataSource()
                                        End Select
                                        oMatrix1.AutoResizeColumns()
                                    Case "Matrix2"
                                        Select Case pVal.ColUID
                                            Case "itemcode"
                                                Dim oCmbType As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
                                                If oCmbType.Selected Is Nothing Then oGFun.oApplication.StatusBar.SetText("Please update header details...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning) : Exit Sub
                                                oMatrix2.FlushToDataSource()
                                                oDBDSDetail2.SetValue("U_ItemCode", pVal.Row - 1, Trim(oDataTable.GetValue("ItemCode", 0)))
                                                oDBDSDetail2.SetValue("U_ItemName", pVal.Row - 1, Trim(oDataTable.GetValue("ItemName", 0)))
                                                oDBDSDetail2.SetValue("U_Uom", pVal.Row - 1, Trim(oDataTable.GetValue("InvntryUom", 0)))
                                                oDBDSDetail2.SetValue("U_Quantity", pVal.Row - 1, "1")

                                                Dim WhsCode, GetPrice As String
                                                If oCmbType.Selected.Value = "VH" Then
                                                    If HANA Then
                                                        WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                                                    Else
                                                        WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                                                    End If
                                                Else
                                                    If HANA Then
                                                        WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                                                    Else
                                                        WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                                                    End If
                                                End If

                                                If WhsCode <> "" Then
                                                    If HANA Then
                                                        GetPrice = oGFun.getSingleValue("select ""AvgPrice"" from OITW where ""ItemCode""='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and ""WhsCode""='" & WhsCode & "'")
                                                    Else
                                                        GetPrice = oGFun.getSingleValue("select AvgPrice from OITW where ItemCode='" & Trim(oDataTable.GetValue("ItemCode", 0)) & "' and WhsCode='" & WhsCode & "'")
                                                    End If
                                                    If GetPrice <> "0" Then
                                                        'oDBDSDetail2.SetValue("U_AvgPrice", pVal.Row - 1, Trim(oDataTable.GetValue("AvgPrice", 0)))
                                                        oDBDSDetail2.SetValue("U_AvgPrice", pVal.Row - 1, GetPrice)
                                                        oDBDSDetail2.SetValue("U_Total", pVal.Row - 1, CDbl(GetPrice))
                                                    End If
                                                End If
                                                oMatrix2.LoadFromDataSource()
                                                Me.CalculateGrandTotal()
                                                oMatrix2.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                            Case "dimcode"
                                                oMatrix2.FlushToDataSource()
                                                oDBDSDetail2.SetValue("U_Costcenter", pVal.Row - 1, Trim(oDataTable.GetValue("PrcCode", 0)))
                                                oMatrix2.LoadFromDataSource()
                                        End Select
                                        oMatrix2.AutoResizeColumns()
                                    Case "Matrix3"
                                        Select Case pVal.ColUID
                                            Case "empname"
                                                oMatrix3.FlushToDataSource()
                                                oDBDSDetail3.SetValue("U_empId", pVal.Row - 1, Trim(oDataTable.GetValue("empID", 0)))
                                                oDBDSDetail3.SetValue("U_empName", pVal.Row - 1, Trim(oDataTable.GetValue("lastName", 0)) & "," & Trim(oDataTable.GetValue("firstName", 0)))
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
                                                oMatrix3.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                        End Select
                                        oMatrix3.AutoResizeColumns()
                                End Select
                            End If

                        Catch ex As Exception
                            oGFun.Msg("Choose From List Event Failed:" & ex.Message)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS
                        Try
                            Select Case pVal.ItemUID
                                Case "Matrix1"
                                    Select Case pVal.ColUID
                                        Case "actcode", "actname"
                                            oGFun.SetNewLine(oMatrix1, oDBDSDetail1, pVal.Row, pVal.ColUID)
                                        Case "ItemCode"
                                            'select T1.ItemCode,(Select ItemName from OITM where ItemCode=T1.ItemCode) as ItemName,T1.LineTotal 
                                            'from OPRQ T0 join PRQ1 T1 on T0.DocEntry=T1.DocEntry where T0.DocEntry=$[@MIPL_PM_JOC1.U_PREntry]
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
                                                    frmJobCard.Update()
                                                    'oDBDSDetail1.SetValue("U_ItemName", oDBDSDetail1.Offset, rsetVal.Fields.Item(1).Value.ToString)
                                                    'oDBDSDetail1.SetValue("U_Linetot", oDBDSDetail1.Offset, rsetVal.Fields.Item(2).Value.ToString)
                                                    'oMatrix1.SetLineData(pVal.Row)
                                                End If
                                            End If
                                    End Select
                                Case "Matrix2"
                                    Select Case pVal.ColUID
                                        Case "itemcode"
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, pVal.Row, pVal.ColUID)
                                    End Select
                                Case "Matrix3"
                                    Select Case pVal.ColUID
                                        Case "empname"
                                            oGFun.SetNewLine(oMatrix3, oDBDSDetail3, pVal.Row, pVal.ColUID)
                                    End Select
                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Lost Focus Event Failed : " & ex.Message)
                        Finally
                        End Try
                        'Case SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                        '    Try
                        '        Select Case pVal.ItemUID
                        '            Case "t_drivenam"
                        '                If pVal.BeforeAction = False Then
                        '                    'oGFun.ChooseFromListFilteration(frmJobCard, "DVRCFL", "empID", "select empID from OHEM where dept='13' ")
                        '                End If
                        '            Case "t_prebynam"
                        '                If pVal.BeforeAction = False Then
                        '                    'oGFun.ChooseFromListFilteration(frmJobCard, "PRECFL", "empID", "select empID from OHEM where dept='13' ")
                        '                End If
                        '            Case "t_macnam"
                        '                If pVal.BeforeAction = False Then
                        '                    'oGFun.ChooseFromListFilteration(frmJobCard, "MECH_CFL", "empID", "select empID from OHEM where dept='13' ")
                        '                End If
                        '            Case "Matrix2"
                        '                Select Case pVal.ColUID
                        '                    Case "itemcode"
                        '                        If pVal.BeforeAction = False Then
                        '                            'oGFun.ChooseFromListFilteration(frmJobCard, "ITMCFL", "ItemCode", "Exec [_IND_Sp_PMD_Got_ItemCode]")
                        '                        End If
                        '                End Select
                        '            Case "Matrix3"
                        '                Select Case pVal.ColUID
                        '                    Case "empid"
                        '                        If pVal.BeforeAction = False Then
                        '                            oGFun.ChooseFromListFilteration(frmJobCard, "MAN_CFL", "empID", "select empID from OHEM where dept='13' ")
                        '                        End If
                        '                End Select
                        '            Case "t_brkdwno"
                        '                If HANA Then
                        '                    SQuery = " SELECT ""DocNum"" FROM ""@MIPL_PM_OBDS"" WHERE ""Status"" = 'O'"
                        '                Else
                        '                    SQuery = " SELECT DocNum FROM [@MIPL_PM_OBDS] WHERE Status = 'O'"
                        '                End If

                        '                oGFun.ChooseFromListFilteration(frmJobCard, "BDSCFL", "DocNum", SQuery)
                        '            Case "t_mainplno"
                        '                If HANA Then
                        '                    SQuery = " SELECT ""Code"" FROM ""@MIPL_PM_OPCL"" where ""U_Type""='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                        '                Else
                        '                    SQuery = " SELECT Code FROM [@MIPL_PM_OPCL] where U_Type='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                        '                End If
                        '                oGFun.ChooseFromListFilteration(frmJobCard, "PMCFL", "Code", SQuery)
                        '            Case "t_vehicno"
                        '                Dim oTxt As SAPbouiCOM.EditText = frmJobCard.Items.Item(pVal.ItemUID).Specific

                        '                If Trim(oDBDSHeader.GetValue("U_VHLType", 0)).Equals("VH") Then
                        '                    If HANA Then
                        '                        StrQuery = "SELECT ""U_ItemCode"" FROM ""@MIPL_PM_OVHL"" "
                        '                    Else
                        '                        StrQuery = "SELECT U_ItemCode FROM [@MIPL_PM_OVHL] "
                        '                    End If

                        '                    oTxt.ChooseFromListUID = "OVHL_CFL"
                        '                    oTxt.ChooseFromListAlias = "U_ItemCode"
                        '                    oGFun.ChooseFromListFilteration(frmJobCard, "OVHL_CFL", "U_ItemCode", StrQuery)
                        '                Else
                        '                    If HANA Then
                        '                        StrQuery = "select ""Code"" from ""@MIPL_PM_OMAC"" Where ""U_InsType""='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                        '                    Else
                        '                        StrQuery = "select Code from [@MIPL_PM_OMAC] Where U_InsType='" & oDBDSHeader.GetValue("U_VHLType", 0).Trim & "'"
                        '                    End If

                        '                    oTxt.ChooseFromListUID = "OMAC_CFL"
                        '                    oTxt.ChooseFromListAlias = "Code"
                        '                    oGFun.ChooseFromListFilteration(frmJobCard, "OMAC_CFL", "Code", StrQuery)
                        '                End If
                        '        End Select
                        '    Catch ex As Exception
                        '        oGFun.Msg("Got Focus Event Failed : " & ex.Message)
                        '    Finally
                        '    End Try

                    Case SAPbouiCOM.BoEventTypes.et_VALIDATE
                        Try
                            Select Case pVal.ItemUID
                                Case "t_prebydat"
                                    If pVal.BeforeAction = True Then
                                        Dim StartDate As String = frmJobCard.Items.Item("t_isudate").Specific.value
                                        Dim EndDate As String = frmJobCard.Items.Item("t_prebydat").Specific.value
                                        If Not oGFun.isValidFrAndToDate(StartDate, EndDate) Then
                                            oApplication.StatusBar.SetText("PreparedBy Date Should be Greater Than Or Equal to Issue By  Date ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        End If
                                    End If
                                Case "t_analdate"
                                    If pVal.BeforeAction = True Then
                                        Dim StartDate As String = frmJobCard.Items.Item("t_isudate").Specific.value
                                        Dim EndDate As String = frmJobCard.Items.Item("t_analdate").Specific.value
                                        If Not oGFun.isValidFrAndToDate(StartDate, EndDate) Then
                                            oApplication.StatusBar.SetText("Analysis By Date Should be Greater Than Or Equal to Issue By   Date ...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        End If
                                    End If
                                Case "Matrix2"
                                    Select Case pVal.ColUID
                                        Case "quantity", "avgprice"
                                            If pVal.BeforeAction = False And pVal.ActionSuccess = True Then
                                                oMatrix2.FlushToDataSource()
                                                Dim dblValue As Double = CDbl(oDBDSDetail2.GetValue("U_Quantity", pVal.Row - 1)) * CDbl(oDBDSDetail2.GetValue("U_AvgPrice", pVal.Row - 1))
                                                oDBDSDetail2.SetValue("U_Total", pVal.Row - 1, dblValue)
                                                oMatrix2.LoadFromDataSource()
                                                oMatrix2.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                                Me.CalculateGrandTotal()
                                            End If
                                    End Select
                                Case "Matrix3"
                                    Select Case pVal.ColUID
                                        Case "manhrs", "hrcost"
                                            If pVal.BeforeAction = False And pVal.ItemChanged Then
                                                oMatrix3.FlushToDataSource()
                                                Dim dblValue As Double = CDbl(oDBDSDetail3.GetValue("U_ManHrs", pVal.Row - 1)) * CDbl(oDBDSDetail3.GetValue("U_HrCost", pVal.Row - 1))
                                                oDBDSDetail3.SetValue("U_TotCost", pVal.Row - 1, dblValue)
                                                oMatrix3.LoadFromDataSource()
                                                oMatrix3.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Click()
                                                Me.CalculateGrandTotal()
                                            End If
                                    End Select
                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Validate Event Failed:")
                        Finally
                        End Try

                    Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                        Try
                            Select Case pVal.ItemUID
                                Case "Matrix1"
                                    Select Case pVal.ColUID
                                        Case "actcode"
                                            If pVal.BeforeAction = False Then
                                                oGFun.DoOpenLinkedObjectForm("OACT", "OACT", "txt_code", oDBDSDetail1.GetValue("U_ActCode", pVal.Row - 1))
                                            End If
                                    End Select

                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Lost Focus Event Failed:")
                        Finally
                        End Try
                    Case (SAPbouiCOM.BoEventTypes.et_COMBO_SELECT)
                        Try
                            Select Case pVal.ItemUID
                                Case "c_series"
                                    If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE And pVal.BeforeAction = False Then
                                        'Get the Serial Number Based On Series...
                                        Dim oCmbSerial As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_series").Specific
                                        Dim strSerialCode As String = oCmbSerial.Selected.Value
                                        Dim strDocNum As Long = frmJobCard.BusinessObject.GetNextSerialNumber(strSerialCode, UDOID)
                                        oDBDSHeader.SetValue("DocNum", 0, strDocNum)
                                    End If
                                Case "c_type"
                                    If pVal.BeforeAction = False Then
                                        Try
                                            Dim oCmbType As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_type").Specific
                                            frmJobCard.Freeze(True)
                                            If oCmbType.Selected.Value = "B" Then
                                                frmJobCard.Items.Item("t_brkdwno").Enabled = True
                                                frmJobCard.Items.Item("t_servtype").Enabled = False
                                                frmJobCard.Items.Item("t_mainplno").Enabled = False
                                                frmJobCard.Items.Item("c_vhltype").Enabled = False
                                                frmJobCard.Items.Item("t_vehicno").Enabled = False
                                                oMatrix1.Clear()
                                                oMatrix2.Clear()
                                                oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
                                                oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                                oDBDSHeader.SetValue("U_VHLNo", 0, "")
                                                oDBDSHeader.SetValue("U_ServType", 0, "")
                                                oDBDSHeader.SetValue("U_MainPlNo", 0, "")
                                                oDBDSHeader.SetValue("U_PrjName", 0, "")
                                                oDBDSHeader.SetValue("U_PrjCode", 0, "")
                                                oDBDSHeader.SetValue("U_BrkDwNo", 0, "")
                                            Else
                                                frmJobCard.Items.Item("t_brkdwno").Enabled = False
                                                frmJobCard.Items.Item("t_servtype").Enabled = True
                                                frmJobCard.Items.Item("t_mainplno").Enabled = True
                                                frmJobCard.Items.Item("c_vhltype").Enabled = True
                                                frmJobCard.Items.Item("t_vehicno").Enabled = True
                                                oMatrix1.Clear()
                                                oMatrix2.Clear()
                                                oGFun.SetNewLine(oMatrix1, oDBDSDetail1)
                                                oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
                                                oDBDSHeader.SetValue("U_VHLNo", 0, "")
                                                oDBDSHeader.SetValue("U_ServType", 0, "")
                                                oDBDSHeader.SetValue("U_MainPlNo", 0, "")
                                                oDBDSHeader.SetValue("U_PrjName", 0, "")
                                                oDBDSHeader.SetValue("U_PrjCode", 0, "")
                                                oDBDSHeader.SetValue("U_BrkDwNo", 0, "")
                                            End If
                                            frmJobCard.Freeze(False)
                                        Catch ex As Exception
                                            frmJobCard.Freeze(False)
                                        End Try
                                        
                                    End If
                                Case "c_vhltype"
                                    If pVal.BeforeAction = False And pVal.ItemChanged Then
                                        oDBDSHeader.SetValue("U_VHLNo", 0, "")
                                    End If
                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Combo Select Event Failed:")
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Try
                            Select Case pVal.ItemUID
                                Case "1"
                                    If frmJobCard.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                        'If (frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                        '    Dim HStatus As String = frmJobCard.Items.Item("c_status").Specific.Selected.Description
                                        '    If oDBDSHeader.GetValue("Status", 0) = "C" Or oDBDSHeader.GetValue("Status", 0) = "L" Then
                                        '        If oApplication.MessageBox(HStatus & " a document is irreversible.Document status will be changed to " & HStatus & ".Do you want to Continue?", 1, "Yes", "No") <> 1 Then oDBDSHeader.SetValue("Status", 0, "O") : BubbleEvent = False : Exit Sub
                                        '        'frmJobCard.Items.Item("1").Click()
                                        '        oApplication.Menus.Item("1304").Activate()
                                        '        'frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                                        '    End If
                                        'End If
                                        If Me.ValidateAll() = False Then
                                            System.Media.SystemSounds.Asterisk.Play()
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If
                                    If pVal.BeforeAction And (frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                        If Trim(oDBDSHeader.GetValue("Status", 0)) = "C" And boolstatus = False Then
                                            'oApplication.MessageBox("Are you sure. Do you want to Close The Document. Continue?", 1, "Ok", "Cancel")
                                            boolstatus = True
                                        End If
                                    End If
                                Case "BtnJE"
                                    Dim Flag As Boolean = False
                                    If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        For i As Integer = 1 To oMatrix3.RowCount
                                            If oMatrix3.Columns.Item("empname").Cells.Item(i).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                                                If oMatrix3.Columns.Item("manhrs").Cells.Item(i).Specific.String = "" Or oMatrix3.Columns.Item("hrcost").Cells.Item(i).Specific.String = "" Then
                                                    Flag = True
                                                    oGFun.Msg("Please update the Line Level Data in ManHours Cost Tab...", "S", "E")
                                                End If
                                            End If
                                        Next

                                        If Flag = False Then
                                            If frmJobCard.Items.Item("txtJE").Specific.String = "" Then
                                                JournalEntry()
                                            Else
                                                oGFun.Msg("Journal Entry Already Created...", "S", "E")
                                                Exit Sub
                                            End If
                                        End If

                                    End If
                                Case "BtnGI"
                                    Dim Flag As Boolean = False
                                    If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        If frmJobCard.Items.Item("c_status").Specific.Selected.Value = "O" Then
                                            For i As Integer = 1 To oMatrix2.RowCount
                                                If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                                                    If oMatrix2.Columns.Item("quantity").Cells.Item(i).Specific.String = "" Or oMatrix2.Columns.Item("avgprice").Cells.Item(i).Specific.String = "" Or oMatrix2.Columns.Item("dimcode").Cells.Item(i).Specific.String = "" Then
                                                        Flag = True
                                                        oGFun.Msg("Please update the Line Level Data in Repair Parts Tab...", "S", "E")
                                                    End If
                                                End If
                                            Next
                                        End If
                                       
                                        If Flag = False Then
                                            'If frmJobCard.Items.Item("txtGI").Specific.String = "" Then
                                            If frmJobCard.Items.Item("c_status").Specific.Selected.Value = "O" Then
                                                GoodsIssue()
                                            Else
                                                cmb_status = frmJobCard.Items.Item("c_status").Specific
                                                oGFun.Msg("Document Status is " & cmb_status.Selected.Description & ".Goods Issue will not be posted...", "S", "E")
                                                Exit Sub
                                            End If
                                            'Else
                                            '    oGFun.Msg("Goods Issue Already Posted...", "S", "E")
                                            '    Exit Sub
                                            'End If                                       
                                        End If

                                    End If
                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Click Event Failed:" & ex.ToString)
                        Finally
                        End Try
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        Try
                            Dim breakdownvalue As String
                            Select Case pVal.ItemUID
                                Case "1"
                                    If pVal.ActionSuccess And frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        Me.InitForm()
                                    End If
                                Case "lk_bds"
                                    If pVal.BeforeAction = False Then
                                        breakdownvalue = oGFun.getSingleValue("select ""DocNum"" from ""@MIPL_PM_OBDS"" where ""DocEntry""=" & oDBDSHeader.GetValue("U_BrkDwNo", 0).Trim & "")
                                        oGFun.DoOpenLinkedObjectForm("OBDS", "OBDS", "t_docnum", breakdownvalue)
                                    End If
                                Case "lk_vhl"
                                    If pVal.BeforeAction = False Then
                                        Dim ocmb As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
                                        Dim a As String = ocmb.Value
                                        If ocmb.Value = "VH" Then
                                            oGFun.DoOpenLinkedObjectForm("OVHL", "OVHL", "t_pecidno", oDBDSHeader.GetValue("U_VHLNo", 0).Trim)
                                        Else
                                            oGFun.DoOpenLinkedObjectForm("OMAC", "OMAC", "t_code", oDBDSHeader.GetValue("U_VHLNo", 0).Trim)
                                        End If
                                    End If
                                Case "lk_pm"
                                    If pVal.BeforeAction = False Then
                                        oGFun.DoOpenLinkedObjectForm("OPCL", "OPCL", "t_name", oDBDSHeader.GetValue("U_MainPlNo", 0).Trim)
                                    End If
                                Case "f_activity"
                                    If pVal.BeforeAction = False Then
                                        frmJobCard.PaneLevel = 1
                                        frmJobCard.Items.Item("f_activity").AffectsFormMode = False
                                        frmJobCard.Settings.MatrixUID = "Matrix1"
                                    End If
                                Case "f_reprpart"
                                    If pVal.BeforeAction = False Then
                                        frmJobCard.PaneLevel = 2
                                        frmJobCard.Items.Item("f_reprpart").AffectsFormMode = False
                                        frmJobCard.Settings.MatrixUID = "Matrix2"
                                        Field_Disable()
                                        If oDBDSHeader.GetValue("Status", 0).Trim = "O" Then
                                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, , "itemcode")
                                        End If

                                    End If
                                Case "f_manhrs"
                                    If pVal.BeforeAction = False Then
                                        frmJobCard.PaneLevel = 3
                                        frmJobCard.Items.Item("f_manhrs").AffectsFormMode = False
                                        frmJobCard.Settings.MatrixUID = "Matrix3"
                                        oGFun.SetNewLine(oMatrix3, oDBDSDetail3, , "empname")
                                    End If
                            End Select
                        Catch ex As Exception
                            oGFun.Msg("Item Pressed Event Failed:")
                        Finally
                        End Try
                End Select
            End If
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
                    Case "1284" 'Cancel
                        If oApplication.MessageBox("Cancelling of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                    Case "1286" 'Close
                        If oApplication.MessageBox("Closing of an entry cannot be reversed.Do you want to Continue?", 1, "Yes", "No") <> 1 Then BubbleEvent = False : Exit Sub
                End Select
            Else
                Select Case pVal.MenuUID
                    Case "1281" 'Find
                        frmJobCard.Items.Item("c_status").Enabled = True
                        frmJobCard.Items.Item("txtGI").Enabled = True
                        frmJobCard.Items.Item("t_brkdwno").Enabled = True
                        frmJobCard.Items.Item("t_mainplno").Enabled = True
                        frmJobCard.Items.Item("t_regexpdt").Enabled = True
                        frmJobCard.Items.Item("t_prebycd").Enabled = True
                        frmJobCard.Items.Item("BtnGI").Enabled = False
                        frmJobCard.Items.Item("BtnView").Enabled = False
                        oMatrix1.Item.Enabled = False
                        oMatrix2.Item.Enabled = False
                        oMatrix3.Item.Enabled = False
                    Case "1284"
                        If pVal.BeforeAction = False Then
                            'Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            'If frmJobCard.Items.Item("c_type").Specific.value = "B" And frmJobCard.Items.Item("c_vhltype").Specific.value = "VH" Then
                            '    If HANA Then
                            '        rset.DoQuery("Exec _IND_Sp_PMD_Machine_Activity_Updation '" & frmJobCard.Items.Item("t_docnum").Specific.value & "' ")
                            '    Else
                            '        rset.DoQuery("Exec _IND_Sp_PMD_Machine_Activity_Updation '" & frmJobCard.Items.Item("t_docnum").Specific.value & "' ")
                            '    End If
                            'End If
                        End If
                    Case "1282"
                        If pVal.BeforeAction = False Then
                            Me.InitForm()
                        End If
                    Case "1293"
                        Select Case sDelRowMatrix
                            Case "Matrix1"
                                oGFun.DeleteRow(oMatrix1, oDBDSDetail1)
                            Case "Matrix2"
                                oGFun.DeleteRow(oMatrix2, oDBDSDetail2)
                            Case "Matrix3"
                                oGFun.DeleteRow(oMatrix3, oDBDSDetail3)
                        End Select
                        Me.CalculateGrandTotal()
                    Case "1287"
                        oGFun.LoadComboBoxSeries(frmJobCard.Items.Item("c_series").Specific, UDOID) ' Load the Combo Box Series
                        oGFun.LoadDocumentDate(frmJobCard.Items.Item("t_docdate").Specific)
                        cmb_status = frmJobCard.Items.Item("c_status").Specific
                        cmb_status.Select("O", SAPbouiCOM.BoSearchKey.psk_ByValue)
                        frmJobCard.Items.Item("txtGI").Specific.String = ""
                    Case "1292"
                        If Folder2.Selected Then
                            oGFun.SetNewLine(oMatrix2, oDBDSDetail2)
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
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix1, "actname", oDBDSDetail1)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix2, "itemcode", oDBDSDetail2)
                        oGFun.DeleteEmptyRowInFormDataEvent(oMatrix3, "empname", oDBDSDetail3)

                    End If
                    If BusinessObjectInfo.ActionSuccess Then
                        If Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") = False And Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("B") Then
                            If HANA Then
                                SQuery = "UPDATE ""@MIPL_PM_OBDS"" SET  ""Status"" = 'C' WHERE ""Status"" = 'O' AND ""DocNum"" = '" & Trim(oDBDSHeader.GetValue("U_BrkDwNo", 0)) & "' "
                            Else
                                SQuery = "UPDATE [@MIPL_PM_OBDS] SET  Status = 'C' WHERE Status = 'O' AND DocNum = '" & Trim(oDBDSHeader.GetValue("U_BrkDwNo", 0)) & "' "
                            End If
                            Dim rsetUpdate As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                            oGFun.oApplication.StatusBar.SetText("Breakdown status has been closed...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                        'If Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") = False Then
                        '    For i As Integer = 0 To oMatrix1.VisualRowCount - 1
                        '        If Trim(oDBDSDetail1.GetValue("U_Status", i)) <> "P" Then
                        '            If i = oMatrix1.VisualRowCount Then
                        '                sQuery = "UPDATE [@MIPL_PM_OBDS] SET  U_CmpDate ='" & frmJobCard.Items.Item("t_docdate").Specific.value & "' WHERE  DocNum = '" & Trim(oDBDSHeader.GetValue("U_BrkDwNo", 0)) & "'  "
                        '                Dim rsetUpdate As SAPbobsCOM.Recordset = oGFun.DoQuery(sQuery)

                        '                Dim ocmb As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
                        '                Dim a As String = ocmb.Value
                        '                If ocmb.Value = "MC" Then
                        '                    Dim str As String = "Update [@MIPL_PM_OMAC] set U_status='A' where Code='" & frmJobCard.Items.Item("t_vehicno").Specific.value & "'"
                        '                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        '                    rset.DoQuery(str)
                        '                Else
                        '                    Dim str As String = "Update [@MIPL_PM_OVHL] set U_status='A' where Code='" & frmJobCard.Items.Item("t_vehicno").Specific.value & "'"
                        '                    Dim rset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        '                    rset.DoQuery(str)
                        '                End If
                        '            End If
                        '        End If
                        '    Next
                        'End If
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                    If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                        'If oDBDSHeader.GetValue("U_GINo", 0).Trim = "" Then
                        '    frmJobCard.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_Default)
                        '    oDBDSHeader.SetValue("Status", 0, "O")
                        'Else
                        '    frmJobCard.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
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
                        If Trim(oDBDSHeader.GetValue("Status", 0)).Equals("O") = False And Trim(oDBDSHeader.GetValue("U_Type", 0)).Equals("B") Then
                            If HANA Then
                                SQuery = "UPDATE ""@MIPL_PM_OBDS"" SET  ""Status"" = 'C' WHERE ""Status"" = 'O' AND ""DocNum"" = '" & Trim(oDBDSHeader.GetValue("U_BrkDwNo", 0)) & "' "
                            Else
                                SQuery = "UPDATE [@MIPL_PM_OBDS] SET  Status = 'C' WHERE Status = 'O' AND DocNum = '" & Trim(oDBDSHeader.GetValue("U_BrkDwNo", 0)) & "' "
                            End If
                            Dim rsetUpdate As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
                            'oGFun.oApplication.StatusBar.SetText("Breakdown status has been closed...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
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
                        If EntryFlag Then If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE : frmJobCard.Items.Item("1").Click()
                        Field_Disable()
                        If oDBDSHeader.GetValue("Status", 0).Trim.Equals("C") Or oDBDSHeader.GetValue("Status", 0).Trim.Equals("L") Then
                            'frmJobCard.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE
                        Else
                            frmJobCard.Items.Item("BtnGI").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_Ok, SAPbouiCOM.BoModeVisualBehavior.mvb_Default)
                            frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                        End If
                        frmJobCard.Items.Item("BtnView").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, SAPbouiCOM.BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        frmJobCard.EnableMenu("1282", True)
                    End If
                    'If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
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
                    sDelRowMatrix = EventInfo.ItemUID
                    Dim Matrix3, Matrix1, Matrix2 As SAPbouiCOM.Matrix
                    Matrix1 = frmJobCard.Items.Item("Matrix1").Specific
                    Matrix2 = frmJobCard.Items.Item("Matrix2").Specific
                    Matrix3 = frmJobCard.Items.Item("Matrix3").Specific
                    If EventInfo.BeforeAction = True Then
                        frmJobCard.EnableMenu("1284", False)
                        frmJobCard.EnableMenu("1285", False)
                        frmJobCard.EnableMenu("1286", False)
                        If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            frmJobCard.EnableMenu("1287", True)  'Duplicate
                        Else
                            frmJobCard.EnableMenu("1287", False)
                        End If

                        If Folder2.Selected = True Then
                            Try
                                If EventInfo.ColUID = "lineid" Then
                                    If Matrix2.Columns.Item("itemcode").Cells.Item(EventInfo.Row).Specific.String <> "" Then
                                        frmJobCard.EnableMenu("1292", True) 'Add Row Menu
                                    End If
                                End If
                                If Matrix2.Columns.Item("itemcode").Cells.Item(EventInfo.Row).Specific.String <> "" And Matrix2.Columns.Item("GIEntry").Cells.Item(EventInfo.Row).Specific.String <> "" Then
                                    frmJobCard.EnableMenu("1293", False) 'Remove Row Menu
                                Else
                                    frmJobCard.EnableMenu("1293", True) 'Remove Row Menu
                                End If
                            Catch ex As Exception
                            End Try

                        End If
                        If EventInfo.ItemUID = "Matrix1" Then
                            frmJobCard.EnableMenu("1293", False)
                        ElseIf EventInfo.ItemUID = "Matrix2" Then
                            If EventInfo.Row = oMatrix1.VisualRowCount Then
                                frmJobCard.EnableMenu("1293", False)
                            Else
                                frmJobCard.EnableMenu("1293", True)
                            End If
                        ElseIf EventInfo.ItemUID = "Matrix3" Then
                            If EventInfo.Row = oMatrix1.VisualRowCount Then
                                frmJobCard.EnableMenu("1293", False)
                            Else
                                frmJobCard.EnableMenu("1293", True)
                            End If
                        Else
                            frmJobCard.EnableMenu("1293", False) 'Remove Row Menu
                        End If
                        If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If Trim(oDBDSHeader.GetValue("U_GINo", 0)) = "" Then
                                frmJobCard.EnableMenu("1283", True) 'Remove
                                'frmJobCard.EnableMenu("1286", True) 'Close
                                frmJobCard.EnableMenu("1284", True) 'Cancel
                            End If
                            frmJobCard.EnableMenu("1286", True) 'Close
                        End If
                    Else
                        frmJobCard.EnableMenu("1292", False)
                        frmJobCard.EnableMenu("1293", False) 'Remove Row Menu
                        frmJobCard.EnableMenu("1283", False) 'Remove
                        frmJobCard.EnableMenu("1286", False) 'Close
                        frmJobCard.EnableMenu("1284", False) 'Cancel
                    End If
                    
            End Select
        Catch ex As Exception
            oGFun.Msg("Right Click Event Failed:")
        Finally
        End Try
    End Sub

    Sub LoadMaintenancePlanDets()
        Try
            If HANA Then
                SQuery = " SELECT b.""U_ActCode"" ,b.""U_ActName"" FROM ""@MIPL_PM_OACP"" a INNER JOIN ""@MIPL_PM_ACP1"" b ON a.""DocEntry"" = b.""DocEntry"" AND  b.""U_ActCode"" IS NOT NULL AND a.""DocNum"" =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            Else
                SQuery = " SELECT b.U_ActCode ,b.U_ActName FROM [@MIPL_PM_OACP] a INNER JOIN [@MIPL_PM_ACP1] b ON a.DocEntry = b.DocEntry AND  b.U_ActCode IS NOT NULL AND a.DocNum =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            End If

            Dim rsetAct As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix1.Clear()
            oDBDSDetail1.Clear()

            rsetAct.MoveFirst()
            For i As Integer = 1 To rsetAct.RecordCount
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_ActCode").Value)
                oDBDSDetail1.SetValue("U_ActName", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_ActName").Value)
                rsetAct.MoveNext()
            Next
            oMatrix1.LoadFromDataSource()
            oMatrix1.AutoResizeColumns()
            Dim oCmbType As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
            Dim WhsCode As String
            If oCmbType.Selected.Value = "VH" Then
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            Else
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            End If
            If WhsCode <> "" Then
                If HANA Then
                    SQuery = "SELECT B.""U_ItemCode"" ,B.""U_ItemName"",B.""U_UOM"",B.""U_Quantity"",W.""AvgPrice"" , IFNULL(B.""U_Quantity"",0) * IFNULL(W.""AvgPrice"",0) ""Total""  FROM ""@MIPL_PM_OACP"" A,""@MIPL_PM_ACP2"" B , OITM i,OITW W WHERE A.""DocEntry"" =B.""DocEntry"" AND B.""U_ItemCode"" IS NOT NULL AND b.""U_ItemCode"" = i.""ItemCode"" and i.""ItemCode""=w.""ItemCode"" and W.""WhsCode""='" & WhsCode & "' AND A.""DocNum"" ='" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                Else
                    SQuery = "SELECT B.U_ItemCode ,B.U_ItemName,B.U_UOM,B.U_Quantity,W.AvgPrice , ISNULL(B.U_Quantity,0) * ISNULL(W.AvgPrice,0) Total  FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP2] B , OITM i,OITW W WHERE A.DocEntry =B.DocEntry AND B.U_ItemCode IS NOT NULL AND b.U_ItemCode = i.ItemCode and i.ItemCode=w.ItemCode and W.WhsCode='" & WhsCode & "' AND A.DocNum ='" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                End If

            End If
           
            'sQuery = "SELECT b.U_ItemCode ,b.U_ItemName,b.U_UOM,b.U_Quantity FROM [@MIPL_PM_OACP] a INNER JOIN [@MIPL_PM_ACP2] b ON a.DocEntry = b.DocEntry AND  b.U_ItemCode IS NOT NULL AND a.DocNum = '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            Dim rsetItem As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix2.Clear()
            oDBDSDetail2.Clear()

            rsetItem.MoveFirst()
            For i As Integer = 1 To rsetItem.RecordCount
                oDBDSDetail2.InsertRecord(oDBDSDetail2.Size)
                oDBDSDetail2.Offset = oDBDSDetail2.Size - 1
                oDBDSDetail2.SetValue("LineID", oDBDSDetail2.Offset, i)
                oDBDSDetail2.SetValue("U_ItemCode", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_ItemCode").Value)
                oDBDSDetail2.SetValue("U_ItemName", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_ItemName").Value)
                oDBDSDetail2.SetValue("U_UOM", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_UOM").Value)
                oDBDSDetail2.SetValue("U_Quantity", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_Quantity").Value)
                oDBDSDetail2.SetValue("U_AvgPrice", oDBDSDetail2.Offset, rsetItem.Fields.Item("AvgPrice").Value)
                oDBDSDetail2.SetValue("U_Total", oDBDSDetail2.Offset, rsetItem.Fields.Item("Total").Value)
                rsetItem.MoveNext()
            Next
            oMatrix2.LoadFromDataSource()
            oMatrix2.AutoResizeColumns()
            Me.CalculateGrandTotal()
            oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.RowCount, "actcode")
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.RowCount, "itemcode")
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("" & ex.Message)
        Finally
        End Try
    End Sub

    Sub LoadPMChecklistDets()
        Try
            If HANA Then
                'SQuery = " SELECT b.""U_ActCode"" ,b.""U_ActName"" FROM ""@MIPL_PM_OACP"" a INNER JOIN ""@MIPL_PM_ACP1"" b ON a.""DocEntry"" = b.""DocEntry"" AND  b.""U_ActCode"" IS NOT NULL AND a.""DocNum"" =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                SQuery = "SELECT b.""U_actcode"" ,b.""U_activity"" FROM ""@MIPL_PM_OPCL"" a INNER JOIN ""@MIPL_PM_PCL1"" b ON a.""Code"" = b.""Code"" AND  b.""U_actcode"" IS NOT NULL AND a.""Code"" =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            Else
                'SQuery = " SELECT b.U_ActCode ,b.U_ActName FROM [@MIPL_PM_OACP] a INNER JOIN [@MIPL_PM_ACP1] b ON a.DocEntry = b.DocEntry AND  b.U_ActCode IS NOT NULL AND a.DocNum =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                SQuery = "SELECT b.U_actcode ,b.U_activity FROM [@MIPL_PM_OPCL] a INNER JOIN [@MIPL_PM_PCL1] b ON a.Code = b.Code AND  b.U_actcode IS NOT NULL AND a.Code =  '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            End If

            Dim rsetAct As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix1.Clear()
            oDBDSDetail1.Clear()

            rsetAct.MoveFirst()
            For i As Integer = 1 To rsetAct.RecordCount
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_actcode").Value)
                oDBDSDetail1.SetValue("U_ActName", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_activity").Value)
                rsetAct.MoveNext()
            Next
            oMatrix1.LoadFromDataSource()
            oMatrix1.AutoResizeColumns()
            Dim oCmbType As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
            Dim WhsCode As String
            If oCmbType.Selected.Value = "VH" Then
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            Else
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            End If
            If WhsCode <> "" Then
                If HANA Then
                    SQuery = "SELECT B.""U_ItemCode"" ,B.""U_ItemName"",B.""U_UOM"",B.""U_Quantity"",W.""AvgPrice"" , IFNULL(B.""U_Quantity"",0) * IFNULL(W.""AvgPrice"",0) ""Total""  FROM ""@MIPL_PM_OACP"" A,""@MIPL_PM_ACP2"" B , OITM i,OITW W WHERE A.""DocEntry"" =B.""DocEntry"" AND B.""U_ItemCode"" IS NOT NULL AND b.""U_ItemCode"" = i.""ItemCode"" and i.""ItemCode""=w.""ItemCode"" and W.""WhsCode""='" & WhsCode & "' AND A.""DocNum"" ='" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                Else
                    SQuery = "SELECT B.U_ItemCode ,B.U_ItemName,B.U_UOM,B.U_Quantity,W.AvgPrice , ISNULL(B.U_Quantity,0) * ISNULL(W.AvgPrice,0) Total  FROM [@MIPL_PM_OACP] A,[@MIPL_PM_ACP2] B , OITM i,OITW W WHERE A.DocEntry =B.DocEntry AND B.U_ItemCode IS NOT NULL AND b.U_ItemCode = i.ItemCode and i.ItemCode=w.ItemCode and W.WhsCode='" & WhsCode & "' AND A.DocNum ='" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
                End If

            End If

            'sQuery = "SELECT b.U_ItemCode ,b.U_ItemName,b.U_UOM,b.U_Quantity FROM [@MIPL_PM_OACP] a INNER JOIN [@MIPL_PM_ACP2] b ON a.DocEntry = b.DocEntry AND  b.U_ItemCode IS NOT NULL AND a.DocNum = '" & oDBDSHeader.GetValue("U_MnPlnCd", 0).Trim & "'"
            Dim rsetItem As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix2.Clear()
            oDBDSDetail2.Clear()

            rsetItem.MoveFirst()
            For i As Integer = 1 To rsetItem.RecordCount
                oDBDSDetail2.InsertRecord(oDBDSDetail2.Size)
                oDBDSDetail2.Offset = oDBDSDetail2.Size - 1
                oDBDSDetail2.SetValue("LineID", oDBDSDetail2.Offset, i)
                oDBDSDetail2.SetValue("U_ItemCode", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_ItemCode").Value)
                oDBDSDetail2.SetValue("U_ItemName", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_ItemName").Value)
                oDBDSDetail2.SetValue("U_UOM", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_UOM").Value)
                oDBDSDetail2.SetValue("U_Quantity", oDBDSDetail2.Offset, rsetItem.Fields.Item("U_Quantity").Value)
                oDBDSDetail2.SetValue("U_AvgPrice", oDBDSDetail2.Offset, rsetItem.Fields.Item("AvgPrice").Value)
                oDBDSDetail2.SetValue("U_Total", oDBDSDetail2.Offset, rsetItem.Fields.Item("Total").Value)
                rsetItem.MoveNext()
            Next
            oMatrix2.LoadFromDataSource()
            oMatrix2.AutoResizeColumns()
            Me.CalculateGrandTotal()
            oGFun.SetNewLine(oMatrix1, oDBDSDetail1, oMatrix1.RowCount, "actcode")
            oGFun.SetNewLine(oMatrix2, oDBDSDetail2, oMatrix2.RowCount, "itemcode")
        Catch ex As Exception
            oGFun.StatusBarErrorMsg("" & ex.Message)
        Finally
        End Try
    End Sub

    Sub LoadActionSuggest()
        Try
            If HANA Then
                SQuery = " select ""U_ActSugg"" from ""@MIPL_PM_BDS1"" where ""DocEntry""= '" & oDBDSHeader.GetValue("U_BrkDwNo", 0).Trim & "'"
            Else
                SQuery = " select U_ActSugg from [@MIPL_PM_BDS1] where DocEntry= '" & oDBDSHeader.GetValue("U_BrkDwNo", 0).Trim & "'"
            End If

            Dim rsetAct As SAPbobsCOM.Recordset = oGFun.DoQuery(SQuery)
            oMatrix1.Clear()
            oDBDSDetail1.Clear()

            rsetAct.MoveFirst()
            For i As Integer = 1 To rsetAct.RecordCount
                oDBDSDetail1.InsertRecord(oDBDSDetail1.Size)
                oDBDSDetail1.Offset = oDBDSDetail1.Size - 1
                oDBDSDetail1.SetValue("LineID", oDBDSDetail1.Offset, i)
                'oDBDSDetail1.SetValue("U_ActCode", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_ActCode").Value)
                oDBDSDetail1.SetValue("U_ActName", oDBDSDetail1.Offset, rsetAct.Fields.Item("U_ActSugg").Value)
                rsetAct.MoveNext()
            Next
            oMatrix1.LoadFromDataSource()

        Catch ex As Exception
            oGFun.StatusBarErrorMsg("" & ex.Message)
        Finally
        End Try
    End Sub

    Sub CalculateGrandTotal()
        Try
            Dim dblActCost = 0, dblSpareCost As Double = 0
            frmJobCard.Freeze(True)
            oMatrix2.FlushToDataSource()
            For i As Integer = 1 To oMatrix2.VisualRowCount
                dblActCost += CDbl(oDBDSDetail2.GetValue("U_Total", i - 1))
            Next

            oMatrix3.FlushToDataSource()
            For j As Integer = 1 To oMatrix3.VisualRowCount
                dblSpareCost += CDbl(oDBDSDetail3.GetValue("U_TotCost", j - 1))
            Next
            oDBDSHeader.SetValue("U_GrandTot", 0, dblActCost + dblSpareCost)
            frmJobCard.Freeze(False)
        Catch ex As Exception
            frmJobCard.Freeze(False)
            oGFun.StatusBarErrorMsg(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub Field_Disable()
        Try
            frmJobCard.Freeze(True)
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
            frmJobCard.Freeze(False)
        Catch ex As Exception
            frmJobCard.Freeze(False)
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
                If oMatrix2.Columns.Item("quantity").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("avgprice").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("dimcode").Cells.Item(i).Specific.String <> "" Then
                    Flag = True
                End If
            End If
        Next
        If Flag = False Then oGFun.Msg("No more Data for posting the Goods Issue ...", "S", "E") : Exit Sub
        oGFun.oApplication.StatusBar.SetText("Goods Issue Creating Please wait...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Try
            If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            Dim oCmbType As SAPbouiCOM.ComboBox = frmJobCard.Items.Item("c_vhltype").Specific
            If oCmbType.Selected.Value = "VH" Then
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_Whse"" from ""@MIPL_PM_OVHL"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_Whse from [@MIPL_PM_OVHL] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            Else
                If HANA Then
                    WhsCode = oGFun.getSingleValue("select Top 1 ""U_DefWhse"" from ""@MIPL_PM_OMAC"" where ""Code""='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                Else
                    WhsCode = oGFun.getSingleValue("select Top 1 U_DefWhse from [@MIPL_PM_OMAC] where Code='" & oDBDSHeader.GetValue("U_VHLNo", 0) & "'")
                End If
            End If
            'If HANA Then
            '    AcctCode = oGFun.getSingleValue("select Top 1 ""U_GIGL"" from ""@MIPL_PM_GL"" where  ""Code""<>''")
            'Else
            '    AcctCode = oGFun.getSingleValue("select Top 1 U_GIGL from [@MIPL_PM_GL] where  Code<>''")
            'End If

            'If AcctCode = "" Then oGFun.oApplication.StatusBar.SetText("Please update the Accountcode in GL UDT...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            If WhsCode = "" Then oGFun.oApplication.StatusBar.SetText("Warehouse Code is missing for the specified entry in Machine Master...Please check", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            If Not oCompany.InTransaction Then oCompany.StartTransaction()
            frmJobCard.Freeze(True)
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmJobCard.Items.Item("t_docdate").Specific
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            'Dim DocDate As Date = Date.ParseExact(GetDate, "dd/MM/yy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            objGoodsIssue.DocDate = Now.Date ' changed from docdate to system date on March 10 2023
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
            objGoodsIssue.Comments = "AutoGen thro' PM Addon-JobCard Posted on ->" & Now.ToString 'frmJobCard.Items.Item("txtremark").Specific.string 
            objGoodsIssue.UserFields.Fields.Item("U_JobNo").Value = frmJobCard.Items.Item("t_docnum").Specific.string
            For i As Integer = 1 To oMatrix2.VisualRowCount
                If oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(i).Specific.String = "O" Then
                    Quantity = oMatrix2.Columns.Item("quantity").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.ItemCode = oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String
                    objGoodsIssue.Lines.UnitPrice = oMatrix2.Columns.Item("avgprice").Cells.Item(i).Specific.String
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
                        StrSql = "SELECT distinct I1.""BatchNum"" ""BatchSerial"", I1.""Quantity"",I1.""DocDate"""
                        StrSql += vbCrLf + " from IBT1 I1 join OIBT T4 on T4.""ItemCode""=I1.""ItemCode"" and I1.""BatchNum""=T4.""BatchNum"" and I1.""WhsCode"" = T4.""WhsCode"""
                        StrSql += vbCrLf + " where T4.""Quantity"">0 and I1.""ItemCode""='" & oMatrix2.Columns.Item("itemcode").Cells.Item(i).Specific.String & "' and I1.""WhsCode""='" & WhsCode & "' order by I1.""DocDate"" "
                        objrs.DoQuery(StrSql)
                        Dim BQty As Double = 0
                        BQty = Quantity '11
                        If objrs.RecordCount > 0 Then
                            For j As Integer = 0 To objrs.RecordCount - 1
                                'objGoodsIssue.Lines.BatchNumbers.SetCurrentLine(j)
                                objGoodsIssue.Lines.BatchNumbers.BatchNumber = CStr(objrs.Fields.Item("BatchSerial").Value)
                                objGoodsIssue.Lines.BatchNumbers.Quantity = Quantity ' CDbl(objrs.Fields.Item("Quantity").Value) 'Quantity
                                objGoodsIssue.Lines.BatchNumbers.Add()
                                BQty = BQty - CDbl(objrs.Fields.Item("Quantity").Value)
                                If BQty > 0 Then
                                    BQty = BQty
                                    If BQty <= 0 Then
                                        Exit For
                                    End If
                                    objrs.MoveNext()
                                Else
                                    Exit For
                                End If
                            Next
                        End If
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
                If frmJobCard.Items.Item("txtGI").Specific.String = "" Then
                    frmJobCard.Items.Item("txtGI").Specific.String = GINo
                Else
                    frmJobCard.Items.Item("txtGI").Specific.String = frmJobCard.Items.Item("txtGI").Specific.String & "," & GINo
                End If

                For j = 1 To oMatrix2.RowCount
                    If oMatrix2.Columns.Item("itemcode").Cells.Item(j).Specific.String <> "" And oMatrix2.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                        oMatrix2.Columns.Item("Stat").Cells.Item(j).Specific.String = "C"
                        oMatrix2.Columns.Item("GIEntry").Cells.Item(j).Specific.String = GINo
                        oMatrix2.CommonSetting.SetRowEditable(j, False)
                    End If
                Next
                If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    frmJobCard.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                End If
                oGFun.oApplication.StatusBar.SetText("Goods Issue Created Successfully...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            End If
            frmJobCard.Freeze(False)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objGoodsIssue)
            GC.Collect()
            objrs = Nothing
        Catch ex As Exception
            frmJobCard.Freeze(False)
            If oCompany.InTransaction Then oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            oGFun.oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub JournalEntry()
        Try
            Dim DocEntry, Branch, BranchCode As String
            Dim objrecset As SAPbobsCOM.Recordset
            Dim objjournalentry As SAPbobsCOM.JournalEntries
            objjournalentry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            If oMatrix3.Columns.Item("empname").Cells.Item(1).Specific.String = "" Then
                oGFun.oApplication.SetStatusBarMessage("Please update the data in ManHours Cost Tab...", SAPbouiCOM.BoMessageTime.bmt_Medium, True) : Exit Sub
            End If
            If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            objrecset = oGFun.DoQuery("Select Top 1 ""U_CreditGL"",""U_DebitGL"" from ""@MIPL_PM_GL"" where ""Code""<>'' ")
            If objrecset.RecordCount = 0 Then oGFun.oApplication.StatusBar.SetText("Please update the GL UDT...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub
            oGFun.oApplication.StatusBar.SetText("Journal Entry Creating Please wait...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            If Not oCompany.InTransaction Then oCompany.StartTransaction()
            Dim oEdit As SAPbouiCOM.EditText
            oEdit = frmJobCard.Items.Item("t_docdate").Specific
            Dim DocDate As Date = Date.ParseExact(oEdit.Value, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            objjournalentry.ReferenceDate = DocDate 'ConvertDate.ToString("dd/MM/yy") 'DocDate 'Now.Date.ToString("yyyyMMdd") 
            'objjournalentry.DueDate = Now.Date.ToString("yyyyMMdd") 'DocDate
            objjournalentry.TaxDate = DocDate  ' ConvertDate.ToString("dd/MM/yy") 'DocDate 'Now.Date.ToString("yyyyMMdd") 
            objjournalentry.UserFields.Fields.Item("U_JobNo").Value = frmJobCard.Items.Item("t_docnum").Specific.string
            objjournalentry.Memo = "JobCard JE"
            objjournalentry.Reference = "Auto Posted through PM JobCard:" & Now.ToString

            For j = 1 To oMatrix3.RowCount
                If oMatrix3.Columns.Item("empname").Cells.Item(j).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                    Branch = oGFun.getSingleValue("select ""MltpBrnchs"" from OADM")
                    If Branch = "Y" Then
                        BranchCode = oGFun.getSingleValue("select ""BPLId"" from OBPL where ifnull(""MainBPL"",'Y')='Y'")
                        objjournalentry.Lines.BPLID = BranchCode
                    End If
                    If CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String) <> 0 Then
                        objjournalentry.Lines.AccountCode = objrecset.Fields.Item("U_CreditGL").Value
                        objjournalentry.Lines.Credit = CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String)
                        objjournalentry.Lines.Debit = 0
                        objjournalentry.Lines.Add()
                        objjournalentry.Lines.AccountCode = objrecset.Fields.Item("U_DebitGL").Value
                        objjournalentry.Lines.Debit = CDbl(oMatrix3.Columns.Item("totcost").Cells.Item(j).Specific.String)
                        objjournalentry.Lines.Credit = 0
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
                frmJobCard.Items.Item("txtJE").Specific.String = DocEntry
                For j = 1 To oMatrix3.RowCount
                    If oMatrix3.Columns.Item("empname").Cells.Item(j).Specific.String <> "" And oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "O" Then
                        oMatrix3.Columns.Item("Stat").Cells.Item(j).Specific.String = "C"
                        'oMatrix3.CommonSetting.SetRowEditable(j, False)
                    End If
                Next
                If frmJobCard.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    frmJobCard.Items.Item("1").Click()
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
            frmJobCard = oGFun.oApplication.Forms.Item(eventInfo.FormUID)
            'eventInfo.LayoutKey = frmJobCard.Items.Item("t_docnum").Specific.string
            eventInfo.LayoutKey = frmJobCard.DataSources.DBDataSources.Item("@MIPL_PM_OJOC").GetValue("DocEntry", 0)
        Catch ex As Exception
        End Try
        
    End Sub

End Class

