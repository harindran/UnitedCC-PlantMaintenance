Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Net.Mail
'Imports System.Windows.Forms

Public Class ClsMail
    'Dim Payroll_Report_FileName = System.Windows.Forms.Application.StartupPath & "\" & "InnovaPaySlip.rpt"
    Public FromMail_id As String = "", FromMail_Password As String = "", Mail_Host As String = "", Mail_Port As String = ""

    Public Sub AutoEmail()
        Try
            Dim strsql As String = ""
            'Dim objrs As SAPbobsCOM.Recordset
            'Dim objrsupdate As SAPbobsCOM.Recordset
            Dim Mailbody As String = ""

            'strsql = " Select T0.DocEntry,Datepart(MM,T0.U_fromdate)[Month],Datepart(yyyy,T0.U_Fromdate)[Year],DateName(Month,T0.U_fromdate)+' - '+Convert(varchar,Datepart(yyyy,T0.U_Fromdate))[Period],"
            'strsql += vbCrLf + " T2.U_empid[Empid],T2.U_ExtEmpNo,isnull(T2.U_firstNam,'')+' '+isnull(T2.U_lastName,'')[ToName],isnull(T2.U_Email,'')[ToEmail],'N'[OTTA]"
            'strsql += vbCrLf + " from [@SMPR_OPRC] T0 inner join [@SMPR_PRC1] T1 on T0.DOcentry=T1.DocEntry Inner join [@SMPR_OHEM] T2 on T1.U_Empid=T2.U_empid"
            'strsql += vbCrLf + " Where T0.U_Fromdate=(Select Max(U_Fromdate) from [@SMPR_OPRC] Where U_process='Y') and isnull(T2.U_payslip,'')='Y' and isnull(T2.U_Email,'')<>''"
            'strsql += vbCrLf + " and isnull(T1.U_payslip,'N')='N' and isnull(T0.U_Apayslip,'N')='Y'"

            'objrs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            'objrs.DoQuery(strsql)
            'If objrs.RecordCount = 0 Then Exit Sub

            FromMail_id = "chitrakumaresan95@gmail.com"
            FromMail_Password = "Chitrakumaresan1995"
            Mail_Host = "smtp.gmail.com"
            Mail_Port = "465" '"587"

            'Dim cryRpt As New ReportDocument
            'cryRpt.Load(Payroll_Report_FileName)
            'cryRpt.DataSourceConnections(0).SetConnection(ServerName, CompanyDb, False)
            'cryRpt.DataSourceConnections(0).SetLogon(DBUserName, DbPassword)

            'For i As Integer = 0 To objrs.RecordCount - 1
            'If objrs.Fields.Item("ToEmail").Value.ToString = "" Then Continue For

            Dim Email As New System.Net.Mail.MailMessage
            Dim MailServer As New System.Net.Mail.SmtpClient()

            Try
                MailServer.Host = Mail_Host
                MailServer.Port = Mail_Port
                MailServer.Credentials = New System.Net.NetworkCredential(FromMail_id.ToString.Trim, FromMail_Password.ToString.Trim)
                MailServer.EnableSsl = True
                MailServer.UseDefaultCredentials = False
                Email.From = New System.Net.Mail.MailAddress(FromMail_id.ToString.Trim)

                Email.To.Add(New System.Net.Mail.MailAddress(FromMail_id))
                Email.Subject = "Test - " ' & objrs.Fields.Item("ToName").Value.ToString & " - " & objrs.Fields.Item("Period").Value.ToString

                Mailbody = "Dear Chitra " '& objrs.Fields.Item("ToName").Value.ToString & ","
                Mailbody += vbCrLf + " "
                Mailbody += vbCrLf + " Please Find the Attached Payslip for the Month of " '& objrs.Fields.Item("Period").Value.ToString & "."
                Mailbody += vbCrLf + " "
                Mailbody += vbCrLf + "With Regards,"
                Mailbody += vbCrLf + "HR Team"
                Mailbody += vbCrLf + " "
                Mailbody += vbCrLf + " "
                Mailbody += "This is Auto generated E-Mail from SAP Business One . Please do not reply to this message. Thank you! "

                Email.Body = Mailbody
                Email.Priority = Net.Mail.MailPriority.High

                'cryRpt.SetParameterValue("Emp@select empid,FIRSTNAME+'  '+LASTNAME from ohem order by Firstname", objrs.Fields.Item("Empid").Value.ToString)
                'cryRpt.SetParameterValue("Month", objrs.Fields.Item("Month").Value.ToString)
                'cryRpt.SetParameterValue("year@select distinct year(T0.u_todate) year from [@SMPR_OPRC] T0", objrs.Fields.Item("Year").Value.ToString)
                'cryRpt.SetParameterValue("OTTA", "N")


                'Email.Attachments.Add(cryRpt.ExportToStream(ExportFormatType.PortableDocFormat, ""))
                'Email.Attachments.Add(New Attachment(cryRpt.ExportToStream(ExportFormatType.PortableDocFormat), "Pay Slip - " & objrs.Fields.Item("ToName").Value.ToString & " - " & objrs.Fields.Item("Period").Value.ToString & ".PDF"))

                MailServer.Send(Email)

                'strsql = "Update [@SMPR_PRC1] set U_Payslip='Y' where DOcentry='" & objrs.Fields.Item("DocEntry").Value.ToString & "' and U_empid='" & objrs.Fields.Item("Empid").Value.ToString & "'"
                'objrsupdate = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'objrsupdate.DoQuery(strsql)

            Catch ex As Exception
            Finally
                If Not Email Is Nothing Then Email.Dispose()
                MailServer = Nothing
            End Try

            ' objrs.MoveNext()
            'Next

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try






    End Sub
End Class
