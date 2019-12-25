using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.BLL
{
    private void sCheckWithdrawAccount()
    {
        SqlCommand CMD;
        SqlDataReader dread = null;
        bool flagWithdraw;
        bool isAccMatured;
        double intAmnt;
        try
        {
            if (BDNLoader.OpenConnection == true)
            {
                CMD = new SqlCommand("Select IsNull(SType,0) From SchmDetails SD Inner Join ProductDetail PD On SD.SDID = PD.SDID Inner Join ADetail AD On PD.PID = AD.PID Where SD.Revolving = 1 And AD.IAccNo = " + this.FinanceTB2.AccountId, BDNLoader.Connection);
                dread = CMD.ExecuteReader;
                if (dread.Read)
                {
                    IsRevolving = dread(0);
                }

                dread.Close();
                if (IsRevolving == true)
                {
                    return;
                }

                CMD = new SqlCommand("SELECT Withdrawal FROM ADETAIL,PRODUCTDETAIL WHERE IACCNO=" + FinanceTB2.AccountId + " AND ADETAIL.PID=PRODUCTDETAIL.PID", BDNLoader.Connection);
                dread = CMD.ExecuteReader;
                if (dread.Read == true)
                {
                    flagWithdraw = dread(0);
                }

                CMD.Dispose();
                dread.Close();
                if (flagWithdraw == false)
                {
                    CMD = new SqlCommand("SELECT IACCNO FROM ADUR WHERE IACCNO=" + FinanceTB2.AccountId + " AND MATDAT<='" + BDNLoader.TransactionDate + "'", BDNLoader.Connection);
                    dread = CMD.ExecuteReader;
                    if (dread.Read == true)
                    {
                        isAccMatured = dread(0);
                    }

                    CMD.Dispose();
                    dread.Close();
                    if (FinanceTB2.AccountStatus != 6)
                    {
                        if (isAccMatured == false)
                        {
                            MessageBox.Show("Withdraw is not Allowed. " + ControlChars.CrLf + "Account NOT Matured Yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            CMD = new SqlCommand("SELECT  ISNULL(SUM(cramt),0) - ISNULL(SUM(DrAmt),0) FROM aintpayable WHERE iaccno = " + FinanceTB2.AccountId + "", BDNLoader.Connection);
                            dread = CMD.ExecuteReader;
                            if (dread.Read == true)
                            {
                                intAmnt = dread(0);
                            }

                            CMD.Dispose();
                            dread.Close();
                            if (intAmnt > 0)
                            {
                                grbWithDoc.Enabled = false;
                                grbWithDetail.Enabled = false;
                                sGetPayableAmountInfo();
                            }
                            else
                            {
                                MessageBox.Show("Available Interest Payable Amount = NRS 0", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                sClearForm();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MsgBox(ex.ToString);
        }
        finally
        {
            dread.Close();
            BDNLoader.Connection.Close();
        }

        BDNLoader.CLSchedule c = new BDNLoader.CLSchedule();
    }
    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by Refactoring Essentials.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================

}

