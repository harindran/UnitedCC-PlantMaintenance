Public Class GVariables

    Public oCompany As SAPbobsCOM.Company
    Public ConnectionString As String = Environment.GetCommandLineArgs().GetValue(1).ToString()
    Public addonName As String = "LandedCosts"
    Public v_RetVal, v_ErrCode As Long
    Public v_ErrMsg As String = ""
    'Public HWKEY() As String = New String() {"H0383144080", "K1679825911"}

End Class
