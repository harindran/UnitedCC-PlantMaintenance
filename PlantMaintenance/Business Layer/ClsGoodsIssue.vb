Public Class ClsGoodsIssue
    Dim frmGoodsIssue As SAPbouiCOM.Form
    Public Const Formtype = "720"
    Public Sub LoadGoodsIssue()
        Try
            frmGoodsIssue = oApplication.Forms.GetForm(Formtype, 0)
            Dim oUDFForm As SAPbouiCOM.Form
            oUDFForm = oApplication.Forms.Item(frmGoodsIssue.UDFFormUID)
            oUDFForm.Items.Item("U_MCOutNo").Enabled = False
            oUDFForm.Items.Item("U_JobNo").Enabled = False
        Catch ex As Exception
            oGFun.Msg("Load GoodsIssue Failed")
        Finally
        End Try
    End Sub

    'Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
    '    Try
    '        If pVal.BeforeAction = True Then
    '            frmGoodsIssue = oApplication.Forms.Item(FormUID)
    '        Else
    '            Select Case pVal.EventType
    '                Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST

    '                Case SAPbouiCOM.BoEventTypes.et_CLICK

    '                Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
    '                    Dim oUDFForm As SAPbouiCOM.Form
    '                    oUDFForm = oApplication.Forms.Item(frmGoodsIssue.UDFFormUID)
    '                    oUDFForm.Items.Item("U_MCOutNo").Enabled = False
    '                    oUDFForm.Items.Item("U_JobNo").Enabled = False
    '            End Select
    '        End If

    '    Catch ex As Exception
    '        oGFun.Msg("Item Event Failed:")
    '    Finally
    '    End Try
    'End Sub

    'Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
    '    Try
    '        Select Case pVal.MenuUID
    '            Case "1281"
    '                Dim oUDFForm As SAPbouiCOM.Form
    '                oUDFForm = oApplication.Forms.Item(frmGoodsIssue.UDFFormUID)
    '                oUDFForm.Items.Item("U_MCOutNo").Enabled = True
    '                oUDFForm.Items.Item("U_JobNo").Enabled = True
    '            Case Else
    '        End Select
    '    Catch ex As Exception
    '        oGFun.Msg("Menu Event Failed:")
    '    Finally
    '    End Try
    'End Sub
End Class
