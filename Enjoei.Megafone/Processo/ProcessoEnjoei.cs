using Enjoei.Megafone.Navegadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enjoei.Megafone.Processo
{
    public class ProcessoEnjoei
    {
        private GoogleChrome Chrome = null;

        public void Megafonar()
        {
            string usuario;
            string senha;
            string navegador;

            Console.WriteLine("### MEGAFONE ENJOEI ###");
            Console.Write("Login: ");
            usuario = Console.ReadLine();

            Console.Write("Senha: ");
            senha = Console.ReadLine();

            Console.Write("Exibir navegador - SIM ou NÃO: ");
            navegador = Console.ReadLine();

            while (!navegador.ToUpper().Equals("SIM") && !navegador.ToUpper().Equals("NÃO"))
            {
                Console.Write("Exibir navegador - SIM ou NÃO: ");
                navegador = Console.ReadLine();
            }

            bool exibirNavegador = navegador.ToUpper().Equals("SIM");
            ProcessoMegafone(usuario, senha, exibirNavegador);

        }

        private void ProcessoMegafone(string usuario, string senha, bool exibirNavegador = false)
        {
            int etapa = 0;
            try
            {
                Chrome = new GoogleChrome(exibirNavegador);
                while (etapa <= 3)
                {
                    switch (etapa)
                    {
                        case 0:
                            Login(usuario, senha);
                            break;
                        case 1:
                            AcessaMegafone();
                            break;
                        case 2:
                            MegafonarTodosItens();
                            break;
                    }
                    etapa++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha ao megafonar: " + ex.Message);
            }
            finally
            {
                Chrome.EncerraDriver();
            }
        }

        private void Login(string usuario, string senha)
        {
            try
            {
                Chrome.Navegador.Navigate().GoToUrl("https://www.enjoei.com.br/");
                Chrome.Navegador.Navigate().GoToUrl("https://www.enjoei.com.br/usuario/identifique-se");
                Chrome.ClicaElementoTexto("button", "entrar com email", 10, 2, true);
                Chrome.EscreveElemento("user_email", usuario, 10, 2, true);
                Chrome.EscreveElemento("user_password", senha, 10, 2, true);
                Chrome.ClicaElementoTexto("button", "entrar", 10, 2, true);

                BUSCA:
                try
                {
                    var elemento = Chrome.LocalizaElemento("icon-megaphone", 25, 2, false);
                    while (elemento == null)
                    {
                        elemento = Chrome.LocalizaElemento("icon-megaphone", 25, 2, false);
                    }
                }
                catch
                {
                    goto BUSCA;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AcessaMegafone()
        {
            try
            {
                Thread.Sleep(3000);
                Chrome.ClicaElementoPropriedade("a","aria-label", "ir para minha lojinha", 10, 2, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MegafonarTodosItens()
        {
            try
            {
                Thread.Sleep(3000);
            MEGAFONAR_TODOS:
                var divsMegafonar = Chrome.LocalizaElementosPropriedade("div", "class", "c-boost-button", 10, 2, true);
                if (divsMegafonar.Any())
                {
                    foreach (var divMegafonar in divsMegafonar)
                    {
                        try
                        {
                            ((OpenQA.Selenium.IJavaScriptExecutor)Chrome.Navegador).ExecuteScript("arguments[0].scrollIntoView(true);", divMegafonar);
                            divMegafonar.Click();
                            Thread.Sleep(1000);
                        }
                        catch
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
                goto MEGAFONAR_TODOS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
