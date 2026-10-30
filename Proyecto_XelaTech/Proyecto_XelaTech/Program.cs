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
    public class Cliente : Persona
    {
        private string telefono;
        private string correo;
        private string tipo;

        public string Telefono
        {
            get { return telefono; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    telefono = value.Trim();
                }
                else
                {
                    throw new ArgumentException("El teléfono no puede estar vacío.");
                }
            }
        }

        public string Correo
        {
            get { return correo; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El correo no puede estar vacío.");
                }
                else if (!value.Contains("@"))
                {
                    throw new ArgumentException("El correo debe contener @.");
                }
                else
                {
                    correo = value.Trim();
                }
            }
        }

        public string Tipo
        {
            get { return tipo; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El tipo de cliente no puede estar vacío.");
                }

                string tipoIngresado = value.Trim();

                if (tipoIngresado == "Estudiante" || tipoIngresado == "Profesional" || tipoIngresado == "Negocio" || tipoIngresado == "Institucion")
                {
                    tipo = tipoIngresado;
                }
                else
                {
                    throw new ArgumentException("Tipo de cliente no válido.");
                }
            }
        }

        public Cliente(string codigo, string nombreCompleto, string telefono, string correo, string tipo) : base(codigo, nombreCompleto)
        {
            Telefono = telefono;
            Correo = correo;
            Tipo = tipo;
        }

        public void ActualizarDatos(string nombre, string telefono, string correo, string tipo)
        {
            Cliente datosValidados = new Cliente(Codigo, nombre, telefono, correo, tipo);

            ActualizarNombre(datosValidados.NombreCompleto);
            Telefono = datosValidados.Telefono;
            Correo = datosValidados.Correo;
            Tipo = datosValidados.Tipo;
        }

        public override string ObtenerRol()
        {
            return "Cliente";
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
