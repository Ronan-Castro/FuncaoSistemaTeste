using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.DAL;
using System.Collections.Generic;
using System;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public void Atualizar(Beneficiario beneficiario)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            dao.Alterar(beneficiario);
        }

        public List<Beneficiario> Consultar(long id)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.ListarPorCliente(id);
        }

        public void ExcluirBeneficiarios(long id, List<long> idsManter)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            cli.Excluir(id, idsManter);
        }

        public void Incluir(Beneficiario beneficiario)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            dao.Incluir(beneficiario);
        }

        public bool VerificarExistencia(string cpf, long id)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.VerificarExistencia(cpf, id);
        }
    }
}
