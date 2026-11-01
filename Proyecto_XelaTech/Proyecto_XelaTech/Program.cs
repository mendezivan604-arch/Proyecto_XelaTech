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
    public class Tecnico : Persona
    {
        private string especialidad;
        private bool habilitado;
        private int limiteActivos;
        private int[] calificaciones;

        public string Especialidad
        {
            get { return especialidad; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La especialidad no puede estar vacía.");
                }

                string especialidadIngresada = value.Trim();

                if (especialidadIngresada == "Portatil" || especialidadIngresada == "Escritorio" || especialidadIngresada == "Impresora")
                {
                    especialidad = especialidadIngresada;
                }
                else
                {
                    throw new ArgumentException("La especialidad debe ser Portatil, Escritorio o Impresora.");
                }
            }
        }

        public bool Habilitado
        {
            get { return habilitado; }
            set { habilitado = value; }
        }

        public int LimiteActivos
        {
            get { return limiteActivos; }
        }

        public decimal? PromedioCalificacion
        {
            get { return CalcularPromedio(); }
        }

        public Tecnico(string codigo, string nombreCompleto, string especialidad) : base(codigo, nombreCompleto)
        {
            Especialidad = especialidad;
            Habilitado = true;
            limiteActivos = 3;

            calificaciones = new int[5];
        }

        public void RegistrarCalificacion(int valor)
        {
            if (valor >= 1 && valor <= 5)
            {
                calificaciones[valor - 1]++;
            }
            else
            {
                throw new ArgumentException("La calificación debe estar entre 1 y 5.");
            }
        }

        public decimal? CalcularPromedio()
        {
            int cantidadCalificaciones = 0;
            int sumaPuntos = 0;

            for (int i = 0; i < calificaciones.Length; i++)
            {
                cantidadCalificaciones = cantidadCalificaciones + calificaciones[i];

                sumaPuntos = sumaPuntos + calificaciones[i] * (i + 1);
            }

            if (cantidadCalificaciones == 0)
            {
                return null;
            }
            else
            {
                return (decimal)sumaPuntos / cantidadCalificaciones;
            }
        }

        public override string ObtenerRol()
        {
            return "Tecnico";
        }
    }
    public abstract class Equipo
    {
        protected string codigo;
        protected string numeroSerie;
        protected string marca;
        protected string modelo;
        protected Cliente propietario;
        protected string estadoFisico;
        protected List<string> accesorios;

        public string Codigo
        {
            get { return codigo; }
        }

        public string NumeroSerie
        {
            get { return numeroSerie; }
        }

        public string Marca
        {
            get { return marca; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    marca = value.Trim();
                }
                else
                {
                    throw new ArgumentException("La marca no puede estar vacía.");
                }
            }
        }

        public string Modelo
        {
            get { return modelo; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    modelo = value.Trim();
                }
                else
                {
                    throw new ArgumentException("El modelo no puede estar vacío.");
                }
            }
        }

        public Cliente Propietario
        {
            get { return propietario; }
        }

        public string EstadoFisico
        {
            get { return estadoFisico; }
        }

        public List<string> Accesorios
        {
            get { return new List<string>(accesorios); }
        }

        public abstract string Tipo
        {
            get;
        }

        protected Equipo(string codigo, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, List<string> accesorios)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del equipo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(numeroSerie))
            {
                throw new ArgumentException("El número de serie es obligatorio.");
            }

            if (propietario == null)
            {
                throw new ArgumentException("El equipo debe tener un propietario.");
            }

            this.codigo = codigo.Trim();
            this.numeroSerie = numeroSerie.Trim();
            this.propietario = propietario;

            Marca = marca;
            Modelo = modelo;

            ActualizarRecepcion(estadoFisico, accesorios);
        }

        public void ActualizarRecepcion(string estado, List<string> accesorios)
        {
            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new ArgumentException("Debe indicar el estado físico del equipo.");
            }

            if (accesorios == null)
            {
                throw new ArgumentException("Debe proporcionar una lista de accesorios.");
            }

            List<string> accesoriosValidados = new List<string>();

            foreach (string accesorio in accesorios)
            {
                if (string.IsNullOrWhiteSpace(accesorio))
                {
                    throw new ArgumentException("El nombre de un accesorio no puede estar vacío.");
                }

                accesoriosValidados.Add(accesorio.Trim());
            }

            estadoFisico = estado.Trim();
            this.accesorios = accesoriosValidados;
        }

        public abstract string ObtenerGuiaDiagnostico();
    }
    public class Portatil : Equipo
    {
        public bool IncluyeCargador
        {
            get { return TieneCargador(); }
        }

        public override string Tipo
        {
            get { return "Portatil"; }
        }

        public Portatil(string codigo, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, List<string> accesorios) : base(codigo, numeroSerie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            
        }

        private bool TieneCargador()
        {
            foreach (string accesorio in accesorios)
            {
                if (accesorio.ToUpper() == "CARGADOR")
                {
                    return true;
                }
            }

            return false;
        }

        public override string ObtenerGuiaDiagnostico()
        {
            return "Revisar encendido, batería, cargador, pantalla y teclado.";
        }
    }
    public class Escritorio : Equipo
    {
        private string formatoGabinete;

        public string FormatoGabinete
        {
            get { return formatoGabinete; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    formatoGabinete = value.Trim();
                }
                else
                {
                    throw new ArgumentException("El formato del gabinete es obligatorio.");
                }
            }
        }

        public override string Tipo
        {
            get { return "Escritorio"; }
        }

        public Escritorio(string codigo, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, List<string> accesorios, string formatoGabinete) : base(codigo, numeroSerie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            FormatoGabinete = formatoGabinete;
        }

        public override string ObtenerGuiaDiagnostico()
        {
            return "Revisar fuente de poder, memoria RAM, disco y ventilación.";
        }
    }
    public class Impresora : Equipo
    {
        private string tecnologia;

        public string Tecnologia
        {
            get { return tecnologia; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    tecnologia = value.Trim();
                }
                else
                {
                    throw new ArgumentException("La tecnología de impresión es obligatoria.");
                }
            }
        }

        public override string Tipo
        {
            get { return "Impresora"; }
        }

        public Impresora(string codigo, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, List<string> accesorios, string tecnologia) : base(codigo, numeroSerie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            Tecnologia = tecnologia;
        }

        public override string ObtenerGuiaDiagnostico()
        {
            return "Revisar alimentación de papel, tinta o tóner y calidad de impresión.";
        }
    }
    public class DatosSistema
    {
        private List<Cliente> clientes;
        private int[] secuencias;

        public List<Cliente> Clientes
        {
            get { return clientes; }
        }

        public DatosSistema()
        {
            clientes = new List<Cliente>();
            secuencias = new int[9];
        }

        public string GenerarCodigo(string prefijo)
        {
            int posicion;

            switch (prefijo)
            {
                case "CLI":
                    posicion = 0;
                    break;

                case "TEC":
                    posicion = 1;
                    break;

                case "EQU":
                    posicion = 2;
                    break;

                case "SOL":
                    posicion = 3;
                    break;

                case "REP":
                    posicion = 4;
                    break;

                case "DIA":
                    posicion = 5;
                    break;

                case "ACC":
                    posicion = 6;
                    break;

                case "DET":
                    posicion = 7;
                    break;

                case "CAM":
                    posicion = 8;
                    break;

                default:
                    throw new ArgumentException("El prefijo del código no es válido.");
            }

            secuencias[posicion]++;

            string codigoGenerado = prefijo + secuencias[posicion].ToString("D3");

            while (ExisteCodigo(codigoGenerado))
            {
                secuencias[posicion]++;

                codigoGenerado = prefijo + secuencias[posicion].ToString("D3");
            }

            return codigoGenerado;
        }

        public bool ExisteCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("Debe proporcionar un código.");
            }

            string codigoBuscado = codigo.Trim().ToUpper();

            foreach (Cliente cliente in clientes)
            {
                if (cliente.Codigo.ToUpper() == codigoBuscado)
                {
                    return true;
                }
            }

            return false;
        }
    }
    public class GestorAtencion
    {
        private DatosSistema datos;

        public GestorAtencion(DatosSistema datos)
        {
            if (datos == null)
            {
                throw new ArgumentException("Debe proporcionar los datos del sistema.");
            }

            this.datos = datos;
        }

        public void RegistrarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentException("Debe proporcionar un cliente.");
            }

            if (!cliente.ValidarDatosBasicos())
            {
                throw new ArgumentException("El cliente tiene datos básicos incompletos.");
            }

            if (datos.ExisteCodigo(cliente.Codigo))
            {
                throw new ArgumentException("Ya existe un registro con ese código.");
            }

            datos.Clientes.Add(cliente);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
