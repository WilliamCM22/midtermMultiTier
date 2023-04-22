using midtermMultiTier.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midtermMultiTier.DataAccess
{
    public class LivresDAO
    {
        private string dataBaseTableName = "dbo.Livres";
        public string dataSetTableName = "Livres";
        private SqlDataAdapter dataAdapter;
        private LivresManager livresManager;

        public LivresDAO(LivresManager livresManager)
        {
            this.livresManager = livresManager;
            this.SetupDataAdapter();
        }

        public void FillData()
        {
            if (this.livresManager.GetDataSet().Tables.Contains(this.dataSetTableName))
            {
                this.livresManager.GetDataSet().Tables.Remove(this.dataSetTableName);
            }
            this.dataAdapter.Fill(this.livresManager.GetDataSet(), this.dataSetTableName);

            DataTable livresTable = this.livresManager.GetDataSet().Tables[this.dataSetTableName];
            livresTable.Columns["id"].ReadOnly = true;
            livresTable.Columns["id"].AutoIncrementSeed = 0;
            livresTable.Columns["id"].AutoIncrementStep = -1;
            livresTable.Columns["nomAuteur"].AllowDBNull = false;
            livresTable.Columns["nomAuteur"].MaxLength = 50;
            livresTable.Columns["prenomAuteur"].AllowDBNull = false;
            livresTable.Columns["prenomAuteur"].MaxLength = 50;
            livresTable.Columns["titre"].AllowDBNull = false;
            livresTable.Columns["titre"].MaxLength = 128;
            livresTable.Columns["isbn"].MaxLength = 64;
        }

        public void UpdateData()
        {
            this.dataAdapter.Update(this.livresManager.GetDataSet(), this.dataSetTableName);
            DataTable livresTable = this.livresManager.GetDataSet().Tables[this.dataSetTableName];
            livresTable.AcceptChanges();
        }
        public void SetupDataAdapter()
        {
            this.dataAdapter = new SqlDataAdapter();
            this.dataAdapter.MissingSchemaAction = System.Data.MissingSchemaAction.AddWithKey;

            this.dataAdapter.SelectCommand = new SqlCommand($"SELECT * FROM {this.dataBaseTableName}", this.livresManager.GetConnection());

            this.dataAdapter.InsertCommand = new SqlCommand($"INSERT INTO {this.dataBaseTableName} " +
                $"(nomAuteur, prenomAuteur, titre, description, isbn) " +
                $"VALUES (@nomAuteur, @prenomAuteur, @titre, @description, @isbn); " +
                $"SELECT * FROM {this.dataBaseTableName} WHERE id = SCOPE_IDENTITY()", this.livresManager.GetConnection());
            this.dataAdapter.InsertCommand.Parameters.Add("@nomAuteur", SqlDbType.NVarChar, 50, "nomAuteur");
            this.dataAdapter.InsertCommand.Parameters.Add("@prenomAuteur", SqlDbType.NVarChar, 50, "prenomAuteur");
            this.dataAdapter.InsertCommand.Parameters.Add("@titre", SqlDbType.NVarChar, 128, "titre");
            this.dataAdapter.InsertCommand.Parameters.Add("@description", SqlDbType.Text, -1, "description");
            this.dataAdapter.InsertCommand.Parameters.Add("@isbn", SqlDbType.NVarChar, 64, "isbn");
            this.dataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            this.dataAdapter.UpdateCommand = new SqlCommand($"UPDATE {this.dataBaseTableName} SET " +
                $"nomAuteur = @nomAuteur, " +
                $"prenomAuteur = @prenomAuteur, " +
                $"titre = @titre, " +
                $"description = @description, " +
                $"isbn = @isbn " +
                $"WHERE id = @id AND " +
                $"nomAuteur = @oldNomAuteur AND " +
                $"prenomAuteur = @oldPrenomAuteur AND " +
                $"titre = @oldTitre AND " +
                $"isbn = @oldIsbn;", this.livresManager.GetConnection());
            this.dataAdapter.UpdateCommand.Parameters.Add("@nomAuteur", SqlDbType.NVarChar, 50, "nomAuteur");
            this.dataAdapter.UpdateCommand.Parameters.Add("@prenomAuteur", SqlDbType.NVarChar, 50, "prenomAuteur");
            this.dataAdapter.UpdateCommand.Parameters.Add("@titre", SqlDbType.NVarChar, 128, "titre");
            this.dataAdapter.UpdateCommand.Parameters.Add("@description", SqlDbType.Text, -1, "description");
            this.dataAdapter.UpdateCommand.Parameters.Add("@isbn", SqlDbType.NVarChar, 64, "isbn");

            this.dataAdapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");
            this.dataAdapter.UpdateCommand.Parameters.Add("@oldNomAuteur", SqlDbType.NVarChar, 50, "nomAuteur").SourceVersion = DataRowVersion.Original;
            this.dataAdapter.UpdateCommand.Parameters.Add("@oldPrenomAuteur", SqlDbType.NVarChar, 50, "prenomAuteur").SourceVersion = DataRowVersion.Original;
            this.dataAdapter.UpdateCommand.Parameters.Add("@oldTitre", SqlDbType.NVarChar, 128, "titre").SourceVersion = DataRowVersion.Original;
            this.dataAdapter.UpdateCommand.Parameters.Add("@oldIsbn", SqlDbType.NVarChar, 64, "isbn").SourceVersion = DataRowVersion.Original;



            this.dataAdapter.DeleteCommand = new SqlCommand($"DELETE FROM {this.dataBaseTableName} WHERE id = @id;", this.livresManager.GetConnection());
            this.dataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            this.dataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
        }

        private void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.StatementType == StatementType.Insert)
            {
                args.Status = UpdateStatus.SkipCurrentRow;

            }
            else if (args.StatementType == StatementType.Update)
            {
                if (args.RecordsAffected == 0)
                {
                    throw new Exception("No rows affected during update.");
                }
            }
            else if (args.StatementType == StatementType.Delete)
            {
                if (args.RecordsAffected == 0)
                {
                    throw new Exception("No rows affected during delete.");
                }
            }
        }








    }
}
