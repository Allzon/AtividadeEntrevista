using System.Collections.Generic;
using System;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.BLL;
using System.Linq;
using System.Data.SqlClient;


namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        [HttpGet]
        public ActionResult BeneficiarioList(long? clienteId)
        {
            BoBeneficiario bo = new BoBeneficiario();
            try
            {
                if (clienteId == null)
                    return PartialView("BeneficiariosModal",
                        new ListBeneficiarioModel()
                        {
                            Beneficiarios = new List<BeneficiarioModel>()
                        });

                List<Beneficiario> ben = bo.Listar(clienteId.Value);

                ListBeneficiarioModel model = new ListBeneficiarioModel();
                model.Beneficiarios = new List<BeneficiarioModel>();

                foreach (var item in ben)
                {
                    model.Beneficiarios.Add(new BeneficiarioModel
                    {
                        Id = item.Id,
                        CPF = String.Format("{0:000\\.000\\.000\\-00}", Convert.ToInt64(item.CPF)),
                        Nome = item.Nome,
                        IdCliente = item.IdCliente
                    });
                }

                return PartialView("BeneficiariosModal", model);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost]
        public JsonResult IncluirBeneficiarios(ListBeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

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
                List<Beneficiario> ben = bo.Listar(model.ClienteId);

                foreach (var item in model.Beneficiarios)
                {                    
                    try
                    {
                        if (ben.Exists(x => x.Id == item.Id))
                        {
                            bo.Alterar(new Beneficiario()
                            {
                                Id = item.Id,
                                CPF = item.CPF.Replace(".", "").Replace("-", ""),
                                Nome = item.Nome,
                            });
                        }
                        else
                        {
                            item.Id = bo.Inlcuir(new Beneficiario()
                            {
                                CPF = item.CPF.Replace(".", "").Replace("-", ""),
                                Nome = item.Nome,
                                IdCliente = model.ClienteId,
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        Response.StatusCode = 400;
                        if (ex.Number == 2627)
                            return Json("CPF ja cadastrado!");
                    }
                }
            }

            return Json("Beneficiarios cadastrados com sucesso");
        }


        public JsonResult DeleteBeneficiarios(List<long> listaId)
        {
            BoBeneficiario bo = new BoBeneficiario();

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

                foreach (var item in listaId)
                {
                    try
                    {
                        bo.Excluir(item);
                    }
                    catch (Exception)
                    {
                        Response.StatusCode = 400;
                        return Json("Erro ao excluir beneficiario");
                    }
                }
            }

            return Json("Beneficiarios cadastrados com sucesso");
        }
    }
}