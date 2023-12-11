Public Class ClsViewTranList
    Dim frmViewTranData As SAPbouiCOM.Form
    Dim oDBDSHeader As SAPbouiCOM.DBDataSource
    Dim oGrid As SAPbouiCOM.Grid
    'Dim UDOID As String = "OACT"
    Dim SQuery As String = ""
    Sub LoadViewTranData(ByVal str_sql As String, ByVal LinkedID As String, ByVal DocEntry As String)
        Try
            oGFun.LoadXML(frmViewTranData, TranDataFormID, TranDataFormIDXML)
            frmViewTranData = oApplication.Forms.Item(TranDataFormID)
            frmViewTranData = oApplication.Forms.ActiveForm
            frmViewTranData.Title = "Goods Issue Entries"
            Try
                Dim objrs As SAPbobsCOM.Recordset
                objrs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                objrs.DoQuery(str_sql)
                If objrs.RecordCount = 0 Then oApplication.StatusBar.SetText("No Entries Found...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning) : frmViewTranData.Close() : objrs = Nothing : Exit Sub
                Dim objDT As SAPbouiCOM.DataTable
                frmViewTranData.Freeze(True)
                If frmViewTranData.DataSources.DataTables.Count = 0 Then
                    frmViewTranData.DataSources.DataTables.Add("DT_VIEW")
                End If
                oGrid = frmViewTranData.Items.Item("3").Specific
                objDT = frmViewTranData.DataSources.DataTables.Item("DT_VIEW")
                objDT.ExecuteQuery(str_sql)
                frmViewTranData.DataSources.DataTables.Item("DT_VIEW").ExecuteQuery(str_sql)

                oGrid.DataTable = frmViewTranData.DataSources.DataTables.Item("DT_VIEW")


                For i As Integer = 0 To oGrid.Columns.Count - 1
                    'oGrid.Columns.Item(i).TitleObject.Sortable = True
                    oGrid.Columns.Item(i).Editable = False
                Next
                frmViewTranData.Freeze(False)
                oGrid.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single
                oGrid.Columns.Item(0).Type = SAPbouiCOM.BoGridColumnType.gct_EditText
                'Dim col As SAPbouiCOM.EditTextColumn
                'col = oGrid.Columns.Item(0)
                'col.LinkedObjectType = LinkedID
                frmViewTranData.Visible = True
                frmViewTranData.Update()
                oGrid.CollapseLevel = 1
                'For i As Integer = 0 To oGrid.Rows.Count - 1
                '    Dim val As String = oGrid.DataTable.Columns.Item(0).Cells.Item(i).Value
                '    MsgBox(val)
                'Next
            Catch ex As Exception
                frmViewTranData.Freeze(False)
            End Try
        Catch ex As Exception
            oGFun.Msg("Load ViewTranData Failed" & ex.Message)
        Finally
        End Try
    End Sub

    Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.EventType
                Case SAPbouiCOM.BoEventTypes.et_CLICK
                    Try
                        Select Case pVal.ItemUID
                            Case "101"
                                If pVal.BeforeAction = False Then
                                    frmViewTranData.Close()
                                End If
                            Case "4"
                                If pVal.BeforeAction = False Then
                                    oGrid.Rows.ExpandAll()
                                    oApplication.Menus.Item("1300").Activate()
                                End If
                            Case "5"
                                If pVal.BeforeAction = False Then
                                    oGrid.Rows.CollapseAll()
                                    oApplication.Menus.Item("1300").Activate()
                                End If
                                'Case "3"
                                '    If pVal.BeforeAction = False Then
                                '        oGrid.Rows.SelectedRows.Add(pVal.Row)
                                '    End If

                        End Select
                    Catch ex As Exception
                        oGFun.Msg("Click Event Failed:")
                    Finally
                    End Try
                Case SAPbouiCOM.BoEventTypes.et_MATRIX_COLLAPSE_PRESSED

                Case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                    Select Case pVal.ItemUID
                        Case "3"
                            Select Case pVal.ColUID
                                Case "DocEntry"
                                    Try
                                        Dim ColItem As SAPbouiCOM.EditTextColumn = oGrid.Columns.Item("DocEntry")
                                        If pVal.BeforeAction = True Then
                                            'Dim objlink As SAPbouiCOM.LinkedButton = ColItem.Type.
                                            Dim ActualEntry As String = ""
                                            'Dim Cellpos As SAPbouiCOM.CellPosition
                                            'Dim Row1 As Integer = oGrid.GetCellFocus().rowIndex 'oGrid.Rows.GetParent(pVal.Row)
                                            'Dim Row As Integer = 0  'oGrid.DataTable.GetValue(0, pVal.Row).ToString
                                            ''If EFlag Then
                                            ''Row = Cellpos.rowIndex
                                            'MsgBox(Row1)
                                            'ActualEntry = oGrid.DataTable.GetValue(0, Row).ToString ' pVal.Row - 1
                                            '    Else
                                            '    Row = pVal.Row
                                            'End If
                                            Dim Row As Integer = oGrid.Rows.GetParent(pVal.Row)
                                            If Row = -1 Then
                                                Row = oGrid.Rows.GetParent(pVal.Row + 1)
                                            Else
                                                Row = pVal.Row
                                            End If
                                            'MsgBox(Row)
                                            'If Row > oGrid.DataTable.Rows.Count Then
                                            '    oGrid.Rows.CollapseAll()
                                            'End If
                                            'MsgBox(oGrid.DataTable.GetValue(0, Row))
                                            If HANA Then
                                                ActualEntry = oGFun.getSingleValue("Select T0.""DocEntry"" from ODRF T0 where ""ObjType""=60 and ifnull(T0.""DocStatus"",'')='O' and T0.""DocEntry""='" & oGrid.DataTable.GetValue(0, Row).ToString & "'")
                                            Else
                                                ActualEntry = oGFun.getSingleValue("Select T0.DocEntry from ODRF T0 where ObjType=60 and isnull(T0.DocStatus,'')='O' and T0.DocEntry='" & oGrid.DataTable.GetValue(0, Row).ToString & "'")
                                            End If
                                            If ActualEntry = "" Then
                                                ColItem.LinkedObjectType = "60"
                                                'ColItem.Item.LinkTo = "GIEntry"
                                            Else
                                                ColItem.LinkedObjectType = "112"
                                                'ColItem.Item.LinkTo = "GIEntry"
                                            End If
                                        End If


                                    Catch ex As Exception
                                    End Try
                            End Select
                    End Select
            End Select
        Catch ex As Exception
            oGFun.Msg("Item Event Failed:")
        Finally
        End Try
    End Sub
End Class
