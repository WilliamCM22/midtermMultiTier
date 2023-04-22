using midtermMultiTier.DataAccess;
using midtermMultiTier.GUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace midtermMultiTier.Business
{
    public class LivresManager
    {
        private LivresForm livresWindow;
        private SqlConnection connection;
        private DataSet dataSet;
        private LivresDAO livresDAO;

        public LivresManager()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.livresWindow = new LivresForm(this);
            this.connection = new SqlConnection("Server=.\\SQL2019EXPRESS;Integrated security=true;Database=db_midterm_07449;");
            this.dataSet = new DataSet();
            this.livresDAO = new LivresDAO(this);
        }

        public void OpenWindow()
        {
            Application.Run(this.livresWindow);
        }

        public void ExitApplication()
        {
            Application.Exit();
        }

        public void LoadData()
        {
            this.livresDAO.FillData();
            this.livresWindow.LinkTableToGridView(this.dataSet.Tables[this.livresDAO.dataSetTableName]);
        }

        public void SaveData()
        {
            this.livresDAO.UpdateData();
        }

        public SqlConnection GetConnection()
        {
            return this.connection;
        }

        public DataSet GetDataSet()
        {
            return this.dataSet;
        }
    }
}
