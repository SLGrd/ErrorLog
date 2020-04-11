using System;
using System.IO;
using System.Windows.Forms;

namespace ErrorLog
{
    public partial class FrmErrorLog : Form
    {
        private readonly string ErrorLogFilePath = Application.StartupPath + "/ErrorLog.Txt";
        readonly string CrLf = Environment.NewLine;

        public FrmErrorLog() { InitializeComponent(); }

        private void FrmErrorLog_Load(object sender, EventArgs e) { }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                txtResultadoNum.Text = Dividir( numNumerador.Value.ToString(), numDenominador.Value.ToString()).ToString("N2");
            }
            catch (Exception ex)
            {
                GravaErrorLog(ex);
                SendTxtMsg(ex);                
                //  Avisa o usuario
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGoBox_Click(object sender, EventArgs e)
        {
            try
            {
                txtResultadoBox.Text = Dividir( txtNumerador.Text, txtDenominador.Text).ToString("N2");
            }
            catch (Exception ex)
            {
                GravaErrorLog(ex);
                SendTxtMsg(ex);                
                //  Avisa o usuario
                MessageBox.Show(ex.Message);
            }
        }

        private double Dividir(string Numerador, string Denominador)
        {
            double Resultado = 0;

            try
            {
                Resultado = Double.Parse( Numerador) / Double.Parse( Denominador);

                if (Double.Parse(Denominador) == 0)
                {
                    throw new DivideByZeroException("Denominador nao pode ser zero.", new DivideByZeroException());
                }
            }
            catch ( DivideByZeroException ex)
            {
                GravaErrorLog(ex);
                txtMsg.Clear();
                SendTxtMsg(ex);
                //  Segue com a exception devolvendo para quem chamou
                throw ex;
            }
            catch (Exception ex)
            {
                GravaErrorLog(ex);
                txtMsg.Clear();
                SendTxtMsg(ex);
                //  Segue com a exception devolvendo para quem chamou
                throw ex;
            }
            finally
            {
                //MessageBox.Show("Fim da Rotina de Calculo");
            }
            return Resultado;
        }

        private void SendTxtMsg( Exception ex)
        {
            txtMsg.Text += "Stack Trace     = " + ex.StackTrace + CrLf;
            txtMsg.Text += "Source          = " + ex.Source + "\r\n";
            txtMsg.Text += "Message         = " + ex.Message + "\r\n";
            txtMsg.Text += "Inner Exception = " + ex.InnerException + CrLf + CrLf;
        }

        private void GravaErrorLog(Exception ex)
        {
            //  Se o arquivo nao existir, cria e logo emseguida fecha o arquivo para que possa ser usado abaixo
            if (!File.Exists( ErrorLogFilePath)) { File.Create( ErrorLogFilePath).Close(); }
            
            using (StreamWriter streamW = new StreamWriter( ErrorLogFilePath, true))
            {
                streamW.WriteLine(DateTime.Now + " - Stack Trace     = " + ex.StackTrace);
                streamW.WriteLine(DateTime.Now + " - Source          = " + ex.Source);
                streamW.WriteLine(DateTime.Now + " - Message         = " + ex.Message);
                streamW.WriteLine(DateTime.Now + " - Inner Exception = " + ex.InnerException);
            }
        }
    }
}