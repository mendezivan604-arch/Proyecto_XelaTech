using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_XelaTech
{
    public abstract class Persona
    {
        protected string codigo;
        protected string nombreCompleto;

        public string Codigo
        {
            get { return codigo; }
        }

        public string NombreCompleto
        {
            get { return nombreCompleto; }
        }

        protected Persona(string codigo, string nombreCompleto)
        {
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                this.codigo = codigo.Trim();
            }
            else
            {
                throw new ArgumentException("El código no puede estar vacío.");
            }

            ActualizarNombre(nombreCompleto);
        }

        public void ActualizarNombre(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                nombreCompleto = nombre.Trim();
            }
            else
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }
        }

        public bool ValidarDatosBasicos()
        {
            if (!string.IsNullOrWhiteSpace(codigo) && !string.IsNullOrWhiteSpace(nombreCompleto))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract string ObtenerRol();
    }
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
