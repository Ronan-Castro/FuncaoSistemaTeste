using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.CPF = bo.LimparCpf(model.CPF);
                if (!bo.IsCpfValido(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json("Formato de CPF inválido");
                }
                if (bo.VerificarExistencia(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }                

                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });
                if (model.Beneficiarios != null)
                {
                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        beneficiario.Cpf = bo.LimparCpf(beneficiario.Cpf);
                        if (!bo.IsCpfValido(beneficiario.Cpf))
                        {
                            Response.StatusCode = 400;
                            return Json($"Formato de CPF do beneficiário {beneficiario.Nome} é inválido");
                        }

                        //se não existe o beneficiário atual insere, não deixa salvar duplicado
                        if (!boBeneficiario.VerificarExistencia(beneficiario.Cpf, model.Id) && beneficiario.Cpf != model.CPF)
                        {
                            beneficiario.IdCliente = model.Id;
                            boBeneficiario.Incluir(beneficiario);
                        }
                    }
                }
                return Json("Cadastro efetuado com sucesso");
            }
        }
        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                if (model.Beneficiarios != null)
                {
                    var idsExistentes = new List<long>();

                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        if (beneficiario.Id != 0)
                        {
                            idsExistentes.Add(beneficiario.Id);
                        }
                        beneficiario.IdCliente = model.Id;
                        beneficiario.Cpf = bo.LimparCpf(beneficiario.Cpf);
                    }
                    //excluir
                    boBeneficiario.ExcluirBeneficiarios(model.Id, idsExistentes);

                    //update
                    foreach (var beneficiario in model.Beneficiarios.Where(x => idsExistentes.Contains(x.Id)))
                    {
                        if (!bo.IsCpfValido(beneficiario.Cpf))
                        {
                            Response.StatusCode = 400;
                            return Json($"Formato de CPF do beneficiário {beneficiario.Nome} é inválido");
                        }
                        boBeneficiario.Atualizar(beneficiario);
                    }

                    //novos
                    foreach (var beneficiario in model.Beneficiarios.Where(x => !idsExistentes.Contains(x.Id)))
                    {
                        if (!bo.IsCpfValido(beneficiario.Cpf))
                        {
                            Response.StatusCode = 400;
                            return Json($"Formato de CPF do beneficiário {beneficiario.Nome} é inválido");
                        }
                        //se não existe o beneficiário atual insere, não deixa salvar duplicado
                        if (!boBeneficiario.VerificarExistencia(beneficiario.Cpf, model.Id) && beneficiario.Cpf != model.CPF)
                        {
                            beneficiario.IdCliente = model.Id;
                            boBeneficiario.Incluir(beneficiario);
                        }
                    }
                }
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);

            BoBeneficiario boBeneficiario = new BoBeneficiario();
            var beneficiarios = boBeneficiario.Consultar(cliente.Id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiarios
                };
            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}