using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {
        internal long Incluir(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.IdCliente),
                new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome),
                new System.Data.SqlClient.SqlParameter("Cpf", beneficiario.Cpf)
            };

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal void Alterar(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("Id", beneficiario.Id),
                new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome),
                new System.Data.SqlClient.SqlParameter("Cpf", beneficiario.Cpf)
            };

            base.Executar("FI_SP_AltBenef", parametros);
        }

        internal void Excluir(long idCliente, List<long> idsManter)
        {
            // Cria o DataTable compatível com o tipo definido no SQL (dbo.TipoListaID)
            var table = new DataTable();
            table.Columns.Add("ID", typeof(long));

            foreach (var id in idsManter)
            {
                table.Rows.Add(id);
            }

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@IDCLIENTE", idCliente),
                new SqlParameter("@IDsManter", table)
                {
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.TipoListaID"
                }
            };

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }
        

        internal List<Beneficiario> ListarPorCliente(long clienteId)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("IdCliente", clienteId)
            };

            DataSet ds = base.Consultar("FI_SP_ConsultaBeneficiario", parametros);
            return Converter(ds);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario dep = new Beneficiario
                    {
                        Id = row.Field<long>("Id"),
                        IdCliente = row.Field<long>("IdCliente"),
                        Nome = row.Field<string>("Nome"),
                        Cpf = row.Field<string>("Cpf")
                    };
                    lista.Add(dep);
                }
            }

            return lista;
        }

        internal bool VerificarExistencia(string cpf, long id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", id));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }
    }
}

