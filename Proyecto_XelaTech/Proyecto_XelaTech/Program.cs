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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El teléfono no puede estar vacío.");
                }

                string texto = value.Trim();
                string digitos = texto;

                if (texto.StartsWith("+502"))
                {
                    digitos = texto.Substring(4);
                }

                if (digitos.Length != 8)
                {
                    throw new ArgumentException("Ingrese 8 dígitos, con +502 opcional al inicio.");
                }

                foreach (char caracter in digitos)
                {
                    if (caracter < '0' || caracter > '9')
                    {
                        throw new ArgumentException("El teléfono solo admite dígitos y el prefijo opcional +502.");
                    }
                }

                telefono = texto;
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

                string texto = value.Trim();

                foreach (char caracter in texto)
                {
                    if (char.IsWhiteSpace(caracter))
                    {
                        throw new ArgumentException("El correo no puede contener espacios.");
                    }
                }

                int arroba = texto.IndexOf('@');
                int punto = texto.LastIndexOf('.');

                if (arroba <= 0 || arroba != texto.LastIndexOf('@') || punto <= arroba + 1 || punto == texto.Length - 1 || texto.StartsWith(".") || texto.Contains("..") || texto.Contains(".@"))
                {
                    throw new ArgumentException("Ingrese un correo como nombre@dominio.com.");
                }

                try
                {
                    System.Net.Mail.MailAddress direccion = new System.Net.Mail.MailAddress(texto);

                    if (direccion.Address != texto)
                    {
                        throw new ArgumentException("Ingrese solamente la dirección de correo.");
                    }
                }
                catch (FormatException)
                {
                    throw new ArgumentException("El formato del correo no es válido.");
                }

                correo = texto;
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
        public int ContarSolicitudes(List<SolicitudServicio> solicitudes)
        {
            int cantidad = 0;

            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (solicitud.Cliente == this)
                    cantidad++;
            }

            return cantidad;
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
        public int CalcularCarga(List<SolicitudServicio> solicitudes)
        {
            int cantidad = 0;

            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (solicitud.TecnicoAsignado == this &&
                    solicitud.EsActiva())
                {
                    cantidad++;
                }
            }

            return cantidad;
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

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    estadoFisico = value.Trim();
                }
                else
                {
                    throw new ArgumentException("El estado físico no puede estar vacío.");
                }
            }
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

            EstadoFisico = estado;
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
    public class Repuesto
    {
        private string codigo;
        private string nombre;
        private string categoria;
        private int existencia;
        private int existenciaMinima;
        private decimal costoUnitario;
        private decimal precioVenta;
        private int cantidadUtilizada;

        public string Codigo
        {
            get { return codigo; }
        }

        public string Nombre
        {
            get { return nombre; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre del repuesto es obligatorio.");
                }

                nombre = value.Trim();
            }
        }

        public string Categoria
        {
            get { return categoria; }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La categoría es obligatoria.");
                }

                categoria = value.Trim();
            }
        }

        public int Existencia
        {
            get { return existencia; }
        }

        public int ExistenciaMinima
        {
            get { return existenciaMinima; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("La existencia mínima no puede ser negativa.");
                }

                existenciaMinima = value;
            }
        }

        public decimal CostoUnitario
        {
            get { return costoUnitario; }

            set
            {
                if (value < 0 || value > precioVenta)
                {
                    throw new ArgumentException("El costo debe estar entre cero y el precio de venta.");
                }

                costoUnitario = value;
            }
        }

        public decimal PrecioVenta
        {
            get { return precioVenta; }

            set
            {
                if (value < 0 || value < costoUnitario)
                {
                    throw new ArgumentException("El precio de venta no puede ser menor al costo.");
                }

                precioVenta = value;
            }
        }

        public int CantidadUtilizada
        {
            get { return cantidadUtilizada; }
        }

        public Repuesto(string codigo, string nombre, string categoria, int existencia, int minimo, decimal costo, decimal precio)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código es obligatorio.");
            }

            if (existencia < 0)
            {
                throw new ArgumentException("La existencia no puede ser negativa.");
            }

            if (costo < 0 || precio < costo)
            {
                throw new ArgumentException("El costo no puede ser negativo ni superar el precio.");
            }

            this.codigo = codigo.Trim();

            Nombre = nombre;
            Categoria = categoria;
            ExistenciaMinima = minimo;

            PrecioVenta = precio;
            CostoUnitario = costo;

            this.existencia = existencia;
            cantidadUtilizada = 0;
        }

        public void ActualizarDatos(string nombre, string categoria, int minimo, decimal costo, decimal precio)
        {
            Repuesto validado = new Repuesto(Codigo, nombre, categoria, Existencia, minimo, costo, precio);

            Nombre = validado.Nombre;
            Categoria = validado.Categoria;
            ExistenciaMinima = validado.ExistenciaMinima;

            costoUnitario = validado.CostoUnitario;
            precioVenta = validado.PrecioVenta;
        }

        public void RegistrarEntrada(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La entrada debe ser mayor que cero.");
            }

            if (cantidad > int.MaxValue - existencia)
            {
                throw new ArgumentException("La cantidad supera la capacidad del inventario.");
            }

            existencia = existencia + cantidad;
        }

        public void Consumir(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad utilizada debe ser mayor que cero.");
            }

            if (cantidad > existencia)
            {
                throw new ArgumentException("No hay existencia suficiente.");
            }

            if (cantidad > int.MaxValue - cantidadUtilizada)
            {
                throw new ArgumentException("Se alcanzó el límite del contador de consumo.");
            }

            existencia = existencia - cantidad;
            cantidadUtilizada = cantidadUtilizada + cantidad;
        }

        public bool EstaEnMinimo()
        {
            return existencia <= existenciaMinima;
        }
    }
    public class DatosSistema
    {
        private List<Cliente> clientes;
        private List<Tecnico> tecnicos;
        private List<Equipo> equipos;
        private List<Repuesto> repuestos;
        private List<SolicitudServicio> solicitudes;
        private int[] secuencias;

        public List<Cliente> Clientes
        {
            get { return clientes; }
        }
        public List<Tecnico> Tecnicos
        {
            get { return tecnicos; }
        }
        public List<Equipo> Equipos
        {
            get { return equipos; }
        }
        public List<Repuesto> Repuestos
        {
            get { return repuestos; }
        }
        public List<SolicitudServicio> Solicitudes
        {
            get { return solicitudes; }
        }
        public DatosSistema()
        {
            clientes = new List<Cliente>();
            tecnicos = new List<Tecnico>();
            equipos = new List<Equipo>();
            repuestos = new List<Repuesto>();
            solicitudes = new List<SolicitudServicio>();
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

            foreach (Tecnico tecnico in tecnicos)
            {
                if (tecnico.Codigo.ToUpper() == codigoBuscado)
                {
                    return true;
                }
            }
            foreach (Equipo equipo in equipos)
            {
                if (equipo.Codigo.ToUpper() == codigoBuscado)
                {
                    return true;
                }
            }

            foreach (Repuesto repuesto in repuestos)
            {
                if (repuesto.Codigo.ToUpper() == codigoBuscado)
                {
                    return true;
                }
            }

            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (solicitud.Codigo.ToUpper() == codigoBuscado)
                {
                    return true;
                }
            }
            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (solicitud.Diagnostico != null)
                {
                    if (solicitud.Diagnostico.Codigo.ToUpper() == codigoBuscado)
                    {
                        return true;
                    }

                    foreach (AccionRealizada accion in solicitud.Diagnostico.Acciones)
                    {
                        if (accion.Codigo.ToUpper() == codigoBuscado)
                        {
                            return true;
                        }
                    }
                }

                foreach (DetalleRepuesto detalle in solicitud.Detalles)
                {
                    if (detalle.Codigo.ToUpper() == codigoBuscado)
                    {
                        return true;
                    }
                }

                foreach (CambioEstado cambio in solicitud.ObtenerHistorial())
                {
                    if (cambio.Codigo.ToUpper() == codigoBuscado)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public class GestorAtencion
    {
        private DatosSistema datos;
        private Queue<SolicitudServicio> colaUrgente;
        private Queue<SolicitudServicio> colaNormal;

        public GestorAtencion(DatosSistema datos)
        {
            if (datos == null)
            {
                throw new ArgumentException("Debe proporcionar los datos del sistema.");
            }

            this.datos = datos;
            colaUrgente = new Queue<SolicitudServicio>();
            colaNormal = new Queue<SolicitudServicio>();
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
        public Cliente BuscarClientePorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("Debe proporcionar el código del cliente.");
            }

            string codigoBuscado = codigo.Trim().ToUpper();

            foreach (Cliente cliente in datos.Clientes)
            {
                if (cliente.Codigo.ToUpper() == codigoBuscado)
                {
                    return cliente;
                }
            }

            return null;
        }
        public void ActualizarCliente(string codigo, string nombre, string telefono, string correo, string tipo)
        {
            Cliente cliente = BuscarClientePorCodigo(codigo);

            if (cliente == null)
            {
                throw new ArgumentException("No existe un cliente con ese código.");
            }

            cliente.ActualizarDatos(nombre, telefono, correo, tipo);
        }

        public void RegistrarTecnico(Tecnico tecnico)
        {
            if (tecnico == null)
            {
                throw new ArgumentException("Debe proporcionar un técnico.");
            }

            if (!tecnico.ValidarDatosBasicos())
            {
                throw new ArgumentException("El técnico tiene datos básicos incompletos.");
            }

            if (datos.ExisteCodigo(tecnico.Codigo))
            {
                throw new ArgumentException("Ya existe un registro con ese código.");
            }

            datos.Tecnicos.Add(tecnico);
        }
        public Tecnico BuscarTecnicoPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("Debe proporcionar el código del técnico.");
            }

            string codigoBuscado = codigo.Trim().ToUpper();

            foreach (Tecnico tecnico in datos.Tecnicos)
            {
                if (tecnico.Codigo.ToUpper() == codigoBuscado)
                {
                    return tecnico;
                }
            }

            return null;
        }

        public void HabilitarTecnico(string codigo, bool habilitado)
        {
            Tecnico tecnico = BuscarTecnicoPorCodigo(codigo);

            if (tecnico == null)
            {
                throw new ArgumentException("No existe un técnico con ese código.");
            }

            tecnico.Habilitado = habilitado;
        }

        public void ActualizarNombreTecnico(string codigo, string nombre)
        {
            Tecnico tecnico = BuscarTecnicoPorCodigo(codigo);

            if (tecnico == null)
            {
                throw new ArgumentException("No existe un técnico con ese código.");
            }

            tecnico.ActualizarNombre(nombre);
        }
        public Equipo BuscarEquipoPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("Debe proporcionar el código del equipo.");
            }

            string buscado = codigo.Trim().ToUpper();

            foreach (Equipo equipo in datos.Equipos)
            {
                if (equipo.Codigo.ToUpper() == buscado)
                {
                    return equipo;
                }
            }

            return null;
        }

        public Equipo BuscarEquipoPorSerie(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie))
            {
                throw new ArgumentException("Debe proporcionar el número de serie.");
            }

            string buscada = serie.Trim().ToUpper();

            foreach (Equipo equipo in datos.Equipos)
            {
                if (equipo.NumeroSerie.ToUpper() == buscada)
                {
                    return equipo;
                }
            }

            return null;
        }

        public void RegistrarEquipo(Equipo equipo)
        {
            if (equipo == null)
            {
                throw new ArgumentException("Debe proporcionar un equipo.");
            }

            if (datos.ExisteCodigo(equipo.Codigo))
            {
                throw new ArgumentException("Ya existe un registro con ese código.");
            }

            if (BuscarEquipoPorSerie(equipo.NumeroSerie) != null)
            {
                throw new ArgumentException("Ya existe un equipo con ese número de serie.");
            }

            Cliente propietario = BuscarClientePorCodigo(equipo.Propietario.Codigo);

            if (propietario == null || propietario != equipo.Propietario)
            {
                throw new ArgumentException("El propietario debe ser un cliente registrado.");
            }

            datos.Equipos.Add(equipo);
        }

        public Repuesto BuscarRepuestoPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("Debe proporcionar el código del repuesto.");
            }

            string buscado = codigo.Trim().ToUpper();

            foreach (Repuesto repuesto in datos.Repuestos)
            {
                if (repuesto.Codigo.ToUpper() == buscado)
                {
                    return repuesto;
                }
            }

            return null;
        }

        public void RegistrarRepuesto(Repuesto repuesto)
        {
            if (repuesto == null)
            {
                throw new ArgumentException("Debe proporcionar un repuesto.");
            }

            if (datos.ExisteCodigo(repuesto.Codigo))
            {
                throw new ArgumentException("Ya existe un registro con ese código.");
            }

            datos.Repuestos.Add(repuesto);
        }

        public void RegistrarEntradaInventario(string codigo, int cantidad)
        {
            Repuesto repuesto = BuscarRepuestoPorCodigo(codigo);

            if (repuesto == null)
            {
                throw new ArgumentException("No existe un repuesto con ese código.");
            }

            repuesto.RegistrarEntrada(cantidad);
        }

        public void ActualizarRepuesto(string codigo, string nombre, string categoria, int minimo, decimal costo, decimal precio)
        {
            Repuesto repuesto = BuscarRepuestoPorCodigo(codigo);

            if (repuesto == null)
            {
                throw new ArgumentException("No existe un repuesto con ese código.");
            }

            repuesto.ActualizarDatos(nombre, categoria, minimo, costo, precio);
        }
        public SolicitudServicio CrearSolicitud(string codigoCliente, string codigoEquipo, string problema, string prioridad)
        {
            Cliente cliente = BuscarClientePorCodigo(codigoCliente);
            Equipo equipo = BuscarEquipoPorCodigo(codigoEquipo);

            if (cliente == null || equipo == null)
            {
                throw new ArgumentException("El cliente y el equipo deben estar registrados.");
            }

            if (equipo.Propietario != cliente)
            {
                throw new ArgumentException("El equipo no pertenece al cliente indicado.");
            }

            if (string.IsNullOrWhiteSpace(problema))
            {
                throw new ArgumentException("Debe describir el problema.");
            }

            if (prioridad != "Normal" && prioridad != "Urgente")
            {
                throw new ArgumentException("La prioridad no es válida.");
            }

            foreach (SolicitudServicio registrada in datos.Solicitudes)
            {
                if (registrada.Equipo == equipo && registrada.EsActiva())
                {
                    throw new ArgumentException("El equipo ya tiene una solicitud activa.");
                }
            }

            SolicitudServicio solicitud = new SolicitudServicio(datos.GenerarCodigo("SOL"), cliente, equipo, problema, prioridad, datos.Solicitudes.Count + 1);

            solicitud.PasarAEspera(datos.GenerarCodigo("CAM"));
            datos.Solicitudes.Add(solicitud);

            if (prioridad == "Urgente")
            {
                colaUrgente.Enqueue(solicitud);
            }
            else
            {
                colaNormal.Enqueue(solicitud);
            }

            return solicitud;
        }

        public SolicitudServicio[] ObtenerColaUrgente()
        {
            return colaUrgente.ToArray();
        }

        public SolicitudServicio[] ObtenerColaNormal()
        {
            return colaNormal.ToArray();
        }

        public SolicitudServicio AtenderSiguiente()
        {
            Queue<SolicitudServicio> cola;

            if (colaUrgente.Count > 0)
            {
                cola = colaUrgente;
            }
            else if (colaNormal.Count > 0)
            {
                cola = colaNormal;
            }
            else
            {
                return null;
            }

            SolicitudServicio solicitud = cola.Peek();

            Tecnico elegido = null;
            int menorCarga = int.MaxValue;

            foreach (Tecnico tecnico in datos.Tecnicos)
            {
                int carga = tecnico.CalcularCarga(datos.Solicitudes);

                if (tecnico.Habilitado && tecnico.Especialidad == solicitud.Equipo.Tipo && carga < tecnico.LimiteActivos)
                {
                    if (elegido == null || carga < menorCarga)
                    {
                        elegido = tecnico;
                        menorCarga = carga;
                    }
                    else if (carga == menorCarga && string.CompareOrdinal(tecnico.Codigo, elegido.Codigo) < 0)
                    {
                        elegido = tecnico;
                    }
                }
            }

            if (elegido == null)
            {
                throw new ArgumentException("La solicitud " + solicitud.Codigo + " sigue en cola: no hay técnico habilitado de " + solicitud.Equipo.Tipo + " con cupo disponible.");
            }

            solicitud.AsignarTecnico(elegido, datos.GenerarCodigo("CAM"));
            cola.Dequeue();

            return solicitud;
        }
        public SolicitudServicio BuscarSolicitudPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código de solicitud es obligatorio.");
            }

            string buscado = codigo.Trim().ToUpper();

            foreach (SolicitudServicio solicitud in datos.Solicitudes)
            {
                if (solicitud.Codigo.ToUpper() == buscado)
                {
                    return solicitud;
                }
            }

            return null;
        }

        private SolicitudServicio ExigirSolicitud(string codigo)
        {
            SolicitudServicio solicitud = BuscarSolicitudPorCodigo(codigo);

            if (solicitud == null)
            {
                throw new ArgumentException("La solicitud no existe.");
            }

            return solicitud;
        }

        public void CambiarEstado(string codigo, string destino)
        {
            SolicitudServicio solicitud = ExigirSolicitud(codigo);

            solicitud.CambiarEstado(destino, datos.GenerarCodigo("CAM"));

            ReconstruirColas();
        }

        public void RegistrarDiagnostico(string codigo, string hallazgos, int minutos)
        {
            SolicitudServicio solicitud = ExigirSolicitud(codigo);

            string codigoDiagnostico;

            if (solicitud.Diagnostico == null)
            {
                codigoDiagnostico = datos.GenerarCodigo("DIA");
            }
            else
            {
                codigoDiagnostico = solicitud.Diagnostico.Codigo;
            }

            solicitud.RegistrarDiagnostico(codigoDiagnostico, hallazgos, minutos);
        }

        public void RegistrarAccion(string codigo, string descripcion)
        {
            ExigirSolicitud(codigo).RegistrarAccion(datos.GenerarCodigo("ACC"), descripcion);
        }

        public void UsarRepuesto(string codigo, string codigoRepuesto, int cantidad)
        {
            SolicitudServicio solicitud = ExigirSolicitud(codigo);
            Repuesto repuesto = BuscarRepuestoPorCodigo(codigoRepuesto);

            if (repuesto == null)
            {
                throw new ArgumentException("El repuesto no existe.");
            }

            solicitud.UsarRepuesto(datos.GenerarCodigo("DET"), repuesto, cantidad);
        }

        public void RegistrarManoObra(string codigo, decimal importe)
        {
            ExigirSolicitud(codigo).RegistrarManoObra(importe);
        }

        public void Entregar(string codigo, int? nota)
        {
            ExigirSolicitud(codigo).Entregar(datos.GenerarCodigo("CAM"), nota);
        }

        public void Cancelar(string codigo, string motivo)
        {
            ExigirSolicitud(codigo).Cancelar(datos.GenerarCodigo("CAM"), motivo);

            ReconstruirColas();
        }

        public void DeshacerEstado(string codigo)
        {
            ExigirSolicitud(codigo).DeshacerEstado();
            ReconstruirColas();
        }

        private void ReconstruirColas()
        {
            SolicitudServicio[] copia = datos.Solicitudes.ToArray();

            for (int i = 1; i < copia.Length; i++)
            {
                SolicitudServicio temporal = copia[i];
                int j = i - 1;

                while (j >= 0 && copia[j].OrdenLlegada > temporal.OrdenLlegada)
                {
                    copia[j + 1] = copia[j];
                    j--;
                }

                copia[j + 1] = temporal;
            }

            colaUrgente.Clear();
            colaNormal.Clear();

            foreach (SolicitudServicio solicitud in copia)
            {
                if (solicitud.Estado == "EnEspera")
                {
                    if (solicitud.Prioridad == "Urgente")
                    {
                        colaUrgente.Enqueue(solicitud);
                    }
                    else
                    {
                        colaNormal.Enqueue(solicitud);
                    }
                }
            }
        }
    }
    public static class Algoritmos
    {
        private static string Normalizar(string texto)
        {
            if (texto == null || texto.Trim() == "")
            {
                throw new ArgumentException("Ingrese un texto para buscar.");
            }

            return texto.Trim().ToUpperInvariant();
        }

        public static List<Cliente> BuscarClientes(List<Cliente> clientes, string texto)
        {
            string filtro = Normalizar(texto);
            List<Cliente> resultado = new List<Cliente>();

            foreach (Cliente cliente in clientes)
            {
                if (Normalizar(cliente.NombreCompleto).Contains(filtro))
                {
                    resultado.Add(cliente);
                }
            }

            return resultado;
        }

        public static List<SolicitudServicio> BuscarPorEstado(List<SolicitudServicio> solicitudes, string estado)
        {
            string filtro = Normalizar(estado);
            List<SolicitudServicio> resultado = new List<SolicitudServicio>();

            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (Normalizar(solicitud.Estado) == filtro)
                {
                    resultado.Add(solicitud);
                }
            }

            return resultado;
        }

        public static List<Equipo> BuscarEquipos(List<Equipo> equipos, string marca)
        {
            string filtro = Normalizar(marca);
            List<Equipo> resultado = new List<Equipo>();

            foreach (Equipo equipo in equipos)
            {
                if (Normalizar(equipo.Marca).Contains(filtro))
                {
                    resultado.Add(equipo);
                }
            }

            return resultado;
        }

        public static int ContarFinalizados(Tecnico tecnico, List<SolicitudServicio> solicitudes)
        {
            int cantidad = 0;

            foreach (SolicitudServicio solicitud in solicitudes)
            {
                if (solicitud.TecnicoAsignado == tecnico && solicitud.Estado == "Entregada")
                {
                    cantidad++;
                }
            }

            return cantidad;
        }

        private static bool FueraDeOrden(decimal primero, decimal segundo, bool ascendente)
        {
            if (ascendente)
            {
                return primero > segundo;
            }

            return primero < segundo;
        }

        public static void Burbuja(SolicitudServicio[] arreglo, bool ascendente)
        {
            if (arreglo.Length < 2)
            {
                return;
            }
            for (int i = 0; i < arreglo.Length - 1; i++)
            {
                bool cambio = false;

                for (int j = 0; j < arreglo.Length - 1 - i; j++)
                {
                    if (FueraDeOrden(arreglo[j].Total, arreglo[j + 1].Total, ascendente))
                    {
                        SolicitudServicio temporal = arreglo[j];
                        arreglo[j] = arreglo[j + 1];
                        arreglo[j + 1] = temporal;
                        cambio = true;
                    }
                }

                if (!cambio)
                {
                    break;
                }
            }
        }

        public static void Seleccion(Tecnico[] arreglo, List<SolicitudServicio> solicitudes, bool ascendente)
        {
            if (arreglo.Length < 2)
            {
                return;
            }
            int[] cantidades = new int[arreglo.Length];

            for (int i = 0; i < arreglo.Length; i++)
            {
                cantidades[i] = ContarFinalizados(arreglo[i], solicitudes);
            }

            for (int i = 0; i < arreglo.Length - 1; i++)
            {
                int elegido = i;

                for (int j = i + 1; j < arreglo.Length; j++)
                {
                    if (FueraDeOrden(cantidades[elegido], cantidades[j], ascendente))
                    {
                        elegido = j;
                    }
                }

                Tecnico temporal = arreglo[i];
                arreglo[i] = arreglo[elegido];
                arreglo[elegido] = temporal;

                int cantidadTemporal = cantidades[i];
                cantidades[i] = cantidades[elegido];
                cantidades[elegido] = cantidadTemporal;
            }
        }

        public static void Shell(Repuesto[] arreglo, bool ascendente)
        {
            if (arreglo.Length < 2)
            {
                return;
            }
            int salto = arreglo.Length / 2;

            while (salto > 0)
            {
                for (int i = salto; i < arreglo.Length; i++)
                {
                    Repuesto temporal = arreglo[i];
                    int j = i;

                    while (j >= salto && FueraDeOrden(arreglo[j - salto].Existencia, temporal.Existencia, ascendente))
                    {
                        arreglo[j] = arreglo[j - salto];
                        j = j - salto;
                    }

                    arreglo[j] = temporal;
                }

                salto = salto / 2;
            }
        }

        private static int Comparar(SolicitudServicio primera, SolicitudServicio segunda, bool porCodigo)
        {
            if (porCodigo)
            {
                return string.CompareOrdinal(Normalizar(primera.Codigo), Normalizar(segunda.Codigo));
            }

            return DateTime.Compare(primera.FechaRecepcion, segunda.FechaRecepcion);
        }

        public static void QuickSort(SolicitudServicio[] arreglo, bool ascendente, bool porCodigo)
        {
            if (arreglo.Length < 2)
            {
                return;
            }

            QuickSort(arreglo, 0, arreglo.Length - 1, ascendente, porCodigo);
        }

        private static void QuickSort(SolicitudServicio[] arreglo, int izquierda, int derecha, bool ascendente, bool porCodigo)
        {
            int i = izquierda;
            int j = derecha;

            SolicitudServicio pivote = arreglo[izquierda + (derecha - izquierda) / 2];

            while (i <= j)
            {
                while (i <= derecha && FueraDeOrden(0, Comparar(arreglo[i], pivote, porCodigo), ascendente))
                {
                    i++;
                }

                while (j >= izquierda && FueraDeOrden(Comparar(arreglo[j], pivote, porCodigo), 0, ascendente))
                {
                    j--;
                }

                if (i <= j)
                {
                    SolicitudServicio temporal = arreglo[i];
                    arreglo[i] = arreglo[j];
                    arreglo[j] = temporal;

                    i++;
                    j--;
                }
            }

            if (izquierda < j)
            {
                QuickSort(arreglo, izquierda, j, ascendente, porCodigo);
            }

            if (i < derecha)
            {
                QuickSort(arreglo, i, derecha, ascendente, porCodigo);
            }
        }

        public static SolicitudServicio PrepararBusquedaCodigo(List<SolicitudServicio> solicitudes, string codigo)
        {
            string buscado = Normalizar(codigo);

            SolicitudServicio[] copia = solicitudes.ToArray();

            QuickSort(copia, true, true);

            return BusquedaBinaria(copia, buscado);
        }

        public static SolicitudServicio BusquedaBinaria(SolicitudServicio[] arreglo, string codigo)
        {
            string buscado = Normalizar(codigo);

            for (int i = 1; i < arreglo.Length; i++)
            {
                if (string.CompareOrdinal(Normalizar(arreglo[i - 1].Codigo), Normalizar(arreglo[i].Codigo)) > 0)
                {
                    throw new ArgumentException("Primero ordene los códigos de forma ascendente.");
                }
            }

            int inicio = 0;
            int fin = arreglo.Length - 1;

            while (inicio <= fin)
            {
                int centro = inicio + (fin - inicio) / 2;

                int comparacion = string.CompareOrdinal(Normalizar(arreglo[centro].Codigo), buscado);

                if (comparacion == 0)
                {
                    return arreglo[centro];
                }

                if (comparacion < 0)
                {
                    inicio = centro + 1;
                }
                else
                {
                    fin = centro - 1;
                }
            }

            return null;
        }
    }
    public class GeneradorDatos
    {
        private DatosSistema datos;
        private GestorAtencion gestor;
        private Random random;

        public GeneradorDatos(DatosSistema datos, GestorAtencion gestor)
        {
            if (datos == null || gestor == null)
            {
                throw new ArgumentException("Faltan los datos o el gestor.");
            }

            this.datos = datos;
            this.gestor = gestor;
            random = new Random();
        }

        public string Generar(int cantidad, int? semilla)
        {
            if (cantidad < 1 || cantidad > 50)
            {
                throw new ArgumentException("Genere entre 1 y 50 solicitudes por vez.");
            }

            if (semilla.HasValue)
            {
                random = new Random(semilla.Value);
            }
            else
            {
                random = new Random();
            }

            string[] nombres =
            {
                "Ana", "Carlos", "María", "Luis", "Sofía", "Pedro"
            };

            string[] tiposCliente =
            {
                "Estudiante", "Profesional", "Negocio", "Institucion"
            };

            string[] especialidades =
            {
                "Portatil", "Escritorio", "Impresora"
            };

            string[] repuestos =
            {
                "Memoria RAM", "Disco SSD", "Cartucho de tinta"
            };

            string[] categorias =
            {
                "Memoria", "Almacenamiento", "Consumible"
            };

            string[] marcas =
            {
                "HP", "Dell", "Epson"
            };

            string[] problemas =
            {
                "No enciende",
                "Se apaga durante el uso", 
                "Funcionamiento intermitente"
            };

            for (int i = 0; i < especialidades.Length; i++)
            {
                string codigo = datos.GenerarCodigo("TEC");

                Tecnico tecnico = new Tecnico(codigo, "Técnico de prueba " + codigo, especialidades[i]);

                gestor.RegistrarTecnico(tecnico);
            }

            for (int i = 0; i < repuestos.Length; i++)
            {
                decimal costo = random.Next(25, 201);
                decimal precio = costo + random.Next(10, 76);

                Repuesto repuesto = new Repuesto(datos.GenerarCodigo("REP"), repuestos[i], categorias[i], random.Next(5, 21), random.Next(1, 5), costo, precio);

                gestor.RegistrarRepuesto(repuesto);
            }

            int urgentes = 0;
            int normales = 0;
            string primerCodigo = "";
            string ultimoCodigo = "";

            for (int i = 0; i < cantidad; i++)
            {

                string codigoCliente = datos.GenerarCodigo("CLI");

                Cliente cliente = new Cliente(codigoCliente, nombres[random.Next(nombres.Length)] + " Prueba " + codigoCliente, random.Next(20000000, 80000000).ToString(), codigoCliente.ToLowerInvariant() + "@ejemplo.com", tiposCliente[random.Next(tiposCliente.Length)]);

                gestor.RegistrarCliente(cliente);

                string codigoEquipo = datos.GenerarCodigo("EQU");
                string serieBase = "PRUEBA-" + codigoEquipo;
                string serie = serieBase;
                int intento = 1;

                while (gestor.BuscarEquipoPorSerie(serie) != null)
                {
                    serie = serieBase + "-" + intento;
                    intento++;
                }

                List<string> accesorios = new List<string>();
                Equipo equipo;
                int tipoEquipo = random.Next(3);

                if (tipoEquipo == 0)
                {
                    accesorios.Add("Cargador");

                    equipo = new Portatil(codigoEquipo, serie, marcas[random.Next(2)], "Modelo de prueba", cliente, "Sin golpes visibles", accesorios);
                }
                else if (tipoEquipo == 1)
                {
                    accesorios.Add("Cable de corriente");

                    equipo = new Escritorio(codigoEquipo, serie, marcas[random.Next(2)], "Modelo de prueba", cliente, "Sin golpes visibles", accesorios, "Torre");
                }
                else
                {
                    accesorios.Add("Cable USB");

                    equipo = new Impresora(codigoEquipo, serie, marcas[2], "Modelo de prueba", cliente, "Sin golpes visibles", accesorios, "Inyección de tinta");
                }

                gestor.RegistrarEquipo(equipo);

                string prioridad = "Normal";

                if (random.Next(2) == 1)
                {
                    prioridad = "Urgente";
                }

                SolicitudServicio solicitud = gestor.CrearSolicitud(cliente.Codigo, equipo.Codigo, problemas[random.Next(problemas.Length)], prioridad);

                if (i == 0)
                {
                    primerCodigo = solicitud.Codigo;
                }

                ultimoCodigo = solicitud.Codigo;

                if (prioridad == "Urgente")
                {
                    urgentes++;
                }
                else
                {
                    normales++;
                }
            }

            return "Clientes creados: " + cantidad + "\nTécnicos creados: 3" + "\nRepuestos creados: 3" + "\nEquipos creados: " + cantidad + "\nSolicitudes creadas: " + cantidad + "\nUrgentes: " + urgentes + "\nNormales: " + normales + "\nPrimera solicitud: " + primerCodigo + "\nÚltima solicitud: " + ultimoCodigo + "\nLas solicitudes quedaron en espera. " + "Atiéndalas desde el menú de colas.";
        }
    }
    public class MenuConsola
    {
        private DatosSistema datos;
        private GestorAtencion gestor;

        public MenuConsola(DatosSistema datos, GestorAtencion gestor)
        {
            if (datos == null || gestor == null)
            {
                throw new ArgumentException("El menú necesita los datos y el gestor.");
            }

            this.datos = datos;
            this.gestor = gestor;
        }

        public void Ejecutar()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("        XELATECH SOPORTE");
                Console.WriteLine("================================");
                Console.WriteLine("1. Gestión de clientes");
                Console.WriteLine("2. Gestión de técnicos");
                Console.WriteLine("3. Gestión de equipos");
                Console.WriteLine("4. Gestión de repuestos");
                Console.WriteLine("5. Solicitudes de servicio");
                Console.WriteLine("6. Colas de atención");
                Console.WriteLine("7. Ordenamientos");
                Console.WriteLine("8. Búsquedas");
                Console.WriteLine("9. Reportes");
                Console.WriteLine("10. Generar datos de prueba");
                Console.WriteLine("0. Salir");

                opcion = LeerEntero("Seleccione una opción: ", 0, 10);

                switch (opcion)
                {
                    case 1:
                        MenuClientes();
                        break;
                    case 2:
                        MenuTecnicos();
                        break;
                    case 3:
                        MenuEquipos();
                        break;
                    case 4:
                        MenuRepuestos();
                        break;
                    case 5:
                        MenuSolicitudes();
                        break;
                    case 6:
                        MenuColas();
                        break;
                    case 7:
                        MenuOrdenamientos();
                        break;
                    case 8:
                        MenuBusquedas();
                        break;
                    case 9:
                        MenuReportes();
                        break;
                    case 10:
                        GenerarDatosPrueba();
                        break;
                    case 0:
                        Console.WriteLine("Los datos están en memoria y se perderán al cerrar.");

                        int confirmarSalida = LeerEntero("¿Salir? 1 Sí / 2 No: ", 1, 2);

                        if (confirmarSalida == 1)
                        {
                            Console.WriteLine("Saliendo del programa...");
                        }
                        else
                        {
                            opcion = -1;
                        }

                        break;
                }

            } while (opcion != 0);
        }
        private void MenuOrdenamientos()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("ORDENAMIENTOS");
                Console.WriteLine("1. Costo total - Burbuja");
                Console.WriteLine("2. Servicios entregados por técnico - Selección");
                Console.WriteLine("3. Existencias - Shell");
                Console.WriteLine("4. Fecha de recepción - Quick Sort");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ", 0, 4);

                if (opcion == 0)
                    return;

                int sentido = LeerEntero(
                    "1. Ascendente / 2. Descendente: ", 1, 2);

                bool ascendente = sentido == 1;

                if (ascendente)
                    Console.WriteLine("Orden: ascendente");
                else
                    Console.WriteLine("Orden: descendente");

                int cantidad = 0;

                if (opcion == 1 || opcion == 4)
                {
                    SolicitudServicio[] copia = datos.Solicitudes.ToArray();

                    if (opcion == 1)
                    {
                        Algoritmos.Burbuja(copia, ascendente);
                        Console.WriteLine("Burbuja: costo total");
                    }
                    else
                    {
                        Algoritmos.QuickSort(copia, ascendente, false);
                        Console.WriteLine("Quick Sort: fecha de recepción");
                    }

                    cantidad = copia.Length;

                    foreach (SolicitudServicio solicitud in copia)
                    {
                        Console.WriteLine(
                            solicitud.Codigo + " | " +
                            solicitud.FechaRecepcion.ToString("dd/MM/yyyy HH:mm:ss") +
                            " | " + solicitud.Estado +
                            " | Q " + solicitud.Total.ToString("F2"));
                    }
                }
                else if (opcion == 2)
                {
                    Tecnico[] copia = datos.Tecnicos.ToArray();

                    Algoritmos.Seleccion(copia, datos.Solicitudes, ascendente);

                    Console.WriteLine("Selección: servicios entregados");
                    cantidad = copia.Length;

                    foreach (Tecnico tecnico in copia)
                    {
                        Console.WriteLine(tecnico.Codigo + " | " + tecnico.NombreCompleto + " | Entregados: " + Algoritmos.ContarFinalizados(tecnico, datos.Solicitudes));
                    }
                }
                else
                {
                    Repuesto[] copia = datos.Repuestos.ToArray();

                    Algoritmos.Shell(copia, ascendente);

                    Console.WriteLine("Shell: existencia");
                    cantidad = copia.Length;

                    foreach (Repuesto repuesto in copia)
                    {
                        Console.WriteLine(repuesto.Codigo + " | " + repuesto.Nombre + " | Existencia: " + repuesto.Existencia);
                    }
                }

                Console.WriteLine("Registros: " + cantidad);

                if (cantidad == 0)
                    Console.WriteLine("No hay datos para ordenar.");

                Pausar();

            } while (opcion != 0);
        }

        private void MenuBusquedas()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("BÚSQUEDAS");
                Console.WriteLine("1. Clientes por nombre - Secuencial");
                Console.WriteLine("2. Solicitudes por estado - Secuencial");
                Console.WriteLine("3. Equipos por marca - Secuencial");
                Console.WriteLine("4. Solicitud por código - Binaria");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ", 0, 4);

                if (opcion == 0)
                {
                    return;
                }

                int cantidad = 0;

                if (opcion == 1)
                {
                    string texto = LeerTexto("Nombre o parte del nombre: ");

                    List<Cliente> encontrados = Algoritmos.BuscarClientes(datos.Clientes, texto);

                    Console.WriteLine("Búsqueda secuencial. Nombre: " + texto);
                    cantidad = encontrados.Count;

                    foreach (Cliente cliente in encontrados)
                    {
                        int solicitudesCliente = 0;

                        foreach (SolicitudServicio solicitud in datos.Solicitudes)
                        {
                            if (solicitud.Cliente == cliente)
                            {
                                solicitudesCliente++;
                            }
                        }

                        Console.WriteLine(cliente.Codigo + " | " + cliente.NombreCompleto + " | Teléfono: " + cliente.Telefono + " | Correo: " + cliente.Correo + " | Tipo: " + cliente.Tipo + " | Solicitudes: " + solicitudesCliente);
                    }
                }
                else if (opcion == 2)
                {
                    string[] estados =
                    {
                        "Recibida", "EnEspera", "Asignada", "EnDiagnostico", "EnReparacion", "ListaParaEntregar", "Entregada", "Cancelada"
                    };

                    for (int i = 0; i < estados.Length; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + estados[i]);
                    }

                    int seleccion = LeerEntero("Estado: ", 1, estados.Length);
                    string estado = estados[seleccion - 1];

                    List<SolicitudServicio> encontrados = Algoritmos.BuscarPorEstado(datos.Solicitudes, estado);

                    Console.WriteLine("Búsqueda secuencial. Estado: " + estado);
                    cantidad = encontrados.Count;

                    foreach (SolicitudServicio solicitud in encontrados)
                    {
                        MostrarSolicitud(solicitud);
                    }
                }
                else if (opcion == 3)
                {
                    string marca = LeerTexto("Marca o parte de la marca: ");

                    List<Equipo> encontrados = Algoritmos.BuscarEquipos(datos.Equipos, marca);

                    Console.WriteLine("Búsqueda secuencial. Marca: " + marca);
                    cantidad = encontrados.Count;

                    foreach (Equipo equipo in encontrados)
                    {
                        MostrarEquipo(equipo);
                    }
                }
                else
                {
                    string codigo = LeerTexto("Código de solicitud: ");

                    SolicitudServicio encontrada = Algoritmos.PrepararBusquedaCodigo(datos.Solicitudes, codigo);

                    Console.WriteLine("Búsqueda binaria. Código: " + codigo);

                    if (encontrada != null)
                    {
                        cantidad = 1;
                        MostrarSolicitud(encontrada);
                    }
                }

                Console.WriteLine("Coincidencias: " + cantidad);

                if (cantidad == 0)
                {
                    Console.WriteLine("No se encontraron coincidencias.");
                }

                Pausar();

            } while (opcion != 0);
        }
        private void MenuReportes()
        {
            GeneradorReportes reportes = new GeneradorReportes(datos, gestor);
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("REPORTES");
                Console.WriteLine("1. Solicitudes en espera por prioridad");
                Console.WriteLine("2. Solicitudes activas, entregadas y canceladas");
                Console.WriteLine("3. Disponibilidad de técnicos");
                Console.WriteLine("4. Ranking de técnicos");
                Console.WriteLine("5. Equipos atendidos por tipo");
                Console.WriteLine("6. Solicitudes por costo total");
                Console.WriteLine("7. Solicitudes por fecha de recepción");
                Console.WriteLine("8. Repuestos por existencia");
                Console.WriteLine("9. Repuestos en mínimo o agotados");
                Console.WriteLine("10. Ingresos por servicios entregados");
                Console.WriteLine("11. Resultados de búsquedas");
                Console.WriteLine("12. Matriz de carga semanal");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ", 0, 12);

                if (opcion == 0)
                {
                    return;
                }

                if (opcion == 11)
                {
                    MenuBusquedas();
                    continue;
                }

                try
                {
                    bool ascendente = true;

                    if (opcion == 6 || opcion == 7)
                    {
                        ascendente = LeerEntero("1. Ascendente / 2. Descendente: ", 1, 2) == 1;
                    }

                    switch (opcion)
                    {
                        case 1:
                            reportes.SolicitudesEnEspera();
                            break;

                        case 2:
                            reportes.SolicitudesPorSituacion();
                            break;

                        case 3:
                            reportes.DisponibilidadTecnicos();
                            break;

                        case 4:
                            reportes.RankingTecnicos();
                            break;

                        case 5:
                            reportes.EquiposPorTipo();
                            break;

                        case 6:
                            reportes.SolicitudesOrdenadas(true, ascendente);
                            break;

                        case 7:
                            reportes.SolicitudesOrdenadas(false, ascendente);
                            break;

                        case 8:
                            reportes.Inventario(false);
                            break;

                        case 9:
                            reportes.Inventario(true);
                            break;

                        case 10:
                            reportes.Ingresos();
                            break;

                        case 12:
                            string texto = LeerTexto("Fecha del lunes (dd/MM/yyyy): ");

                            DateTime lunes;

                            if (!DateTime.TryParseExact(texto,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out lunes))
                            {
                                throw new ArgumentException("Ingrese una fecha válida con formato dd/MM/yyyy.");
                            }

                            reportes.CargaSemanal(lunes);
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine("Error: " + error.Message);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("No se pudo completar el total: " + "los importes exceden el límite numérico.");
                }

                Pausar();

            } while (opcion != 0);
        }
        private void GenerarDatosPrueba()
        {
            Console.Clear();
            Console.WriteLine("GENERAR DATOS DE PRUEBA");
            Console.WriteLine("Cada solicitud tendrá un cliente y un equipo nuevos.");
            Console.WriteLine("También se agregarán 3 técnicos y 3 repuestos.");
            Console.WriteLine("Los registros existentes se conservarán.");

            int cantidad = LeerEntero("Cantidad de solicitudes (1 a 50): ", 1, 50);

            int? semilla = null;

            int opcion = LeerEntero("1. Aleatorio / 2. Elegir semilla: ", 1, 2);

            if (opcion == 2)
            {
                semilla = LeerEntero("Semilla (0 a 1000000): ", 0, 1000000);
            }    

            try
            {
                GeneradorDatos generador = new GeneradorDatos(datos, gestor);

                Console.WriteLine(generador.Generar(cantidad, semilla));
            }
            catch (ArgumentException error)
            {
                Console.WriteLine("No se completó la generación: " + error.Message);

                Console.WriteLine("Los registros creados antes del error permanecen guardados en memoria.");
            }

            Pausar();
        }
        private void MenuClientes()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("        GESTIÓN DE CLIENTES");
                Console.WriteLine("================================");
                Console.WriteLine("1. Registrar cliente");
                Console.WriteLine("2. Mostrar clientes");
                Console.WriteLine("3. Actualizar cliente");
                Console.WriteLine("0. Volver al menú principal");

                opcion = LeerEntero("Seleccione una opción: ", 0, 3);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarCliente();
                            break;

                        case 2:
                            MostrarClientes();
                            break;
                        case 3:
                            ActualizarCliente();
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 0);
        }

        private void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR CLIENTE");
            Console.WriteLine();

            string nombre = LeerTexto("Nombre completo: ");
            string telefono = LeerTexto("Teléfono: ");
            string correo = LeerTexto("Correo: ");

            Console.WriteLine();
            Console.WriteLine("Tipo de cliente:");
            Console.WriteLine("1. Estudiante");
            Console.WriteLine("2. Profesional");
            Console.WriteLine("3. Negocio");
            Console.WriteLine("4. Institucion");

            int opcionTipo = LeerEntero("Seleccione el tipo: ", 1, 4);

            string tipo;

            switch (opcionTipo)
            {
                case 1:
                    tipo = "Estudiante";
                    break;

                case 2:
                    tipo = "Profesional";
                    break;

                case 3:
                    tipo = "Negocio";
                    break;

                default:
                    tipo = "Institucion";
                    break;
            }

            string codigo = datos.GenerarCodigo("CLI");

            Cliente cliente = new Cliente(codigo, nombre, telefono, correo, tipo);

            gestor.RegistrarCliente(cliente);

            Console.WriteLine();
            Console.WriteLine("Cliente registrado correctamente.");
            Console.WriteLine("Código asignado: " + cliente.Codigo);
        }

        private void MostrarClientes()
        {
            Console.Clear();
            Console.WriteLine("CLIENTES REGISTRADOS");
            Console.WriteLine();

            if (datos.Clientes.Count == 0)
            {
                Console.WriteLine("Todavía no hay clientes registrados.");
                return;
            }

            foreach (Cliente cliente in datos.Clientes)
            {
                Console.WriteLine("Código: " + cliente.Codigo);
                Console.WriteLine("Nombre: " + cliente.NombreCompleto);
                Console.WriteLine("Teléfono: " + cliente.Telefono);
                Console.WriteLine("Correo: " + cliente.Correo);
                Console.WriteLine("Tipo: " + cliente.Tipo);
                Console.WriteLine("Solicitudes registradas: " + cliente.ContarSolicitudes(datos.Solicitudes));
                Console.WriteLine("--------------------------------");
            }

            Console.WriteLine("Total de clientes: " + datos.Clientes.Count);
        }

        private void ActualizarCliente()
        {
            Console.Clear();
            Console.WriteLine("ACTUALIZAR CLIENTE");
            Console.WriteLine();

            if (datos.Clientes.Count == 0)
            {
                Console.WriteLine("Todavía no hay clientes registrados.");
                return;
            }

            string codigo = LeerTexto("Código del cliente: ");

            Cliente cliente = gestor.BuscarClientePorCodigo(codigo);

            if (cliente == null)
            {
                Console.WriteLine("No existe un cliente con ese código.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Datos actuales:");
            Console.WriteLine("Nombre: " + cliente.NombreCompleto);
            Console.WriteLine("Teléfono: " + cliente.Telefono);
            Console.WriteLine("Correo: " + cliente.Correo);
            Console.WriteLine("Tipo: " + cliente.Tipo);
            Console.WriteLine();

            Console.WriteLine("Ingrese los datos que desea guardar.");
            Console.WriteLine("Si un dato no cambia, escriba su valor actual.");
            Console.WriteLine();

            string nombre = LeerTexto("Nombre completo: ");
            string telefono = LeerTexto("Teléfono: ");
            string correo = LeerTexto("Correo: ");

            Console.WriteLine();
            Console.WriteLine("Tipo de cliente:");
            Console.WriteLine("1. Estudiante");
            Console.WriteLine("2. Profesional");
            Console.WriteLine("3. Negocio");
            Console.WriteLine("4. Institucion");

            int opcionTipo = LeerEntero("Seleccione el tipo: ", 1, 4);

            string tipo;

            switch (opcionTipo)
            {
                case 1:
                    tipo = "Estudiante";
                    break;

                case 2:
                    tipo = "Profesional";
                    break;

                case 3:
                    tipo = "Negocio";
                    break;

                default:
                    tipo = "Institucion";
                    break;
            }

            gestor.ActualizarCliente(cliente.Codigo, nombre, telefono, correo, tipo);

            Console.WriteLine();
            Console.WriteLine("Cliente actualizado correctamente.");
        }
        private void MenuTecnicos()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("        GESTIÓN DE TÉCNICOS");
                Console.WriteLine("================================");
                Console.WriteLine("1. Registrar técnico");
                Console.WriteLine("2. Mostrar técnicos");
                Console.WriteLine("3. Habilitar o deshabilitar técnico");
                Console.WriteLine("4. Actualizar nombre del técnico");
                Console.WriteLine("0. Volver al menú principal");

                opcion = LeerEntero("Seleccione una opción: ", 0, 4);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarTecnico();
                            break;

                        case 2:
                            MostrarTecnicos();
                            break;

                        case 3:
                            CambiarHabilitacionTecnico();
                            break;

                        case 4:
                            ActualizarNombreTecnico();
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 0);
        }
        private void RegistrarTecnico()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR TÉCNICO");
            Console.WriteLine();

            string nombre = LeerTexto("Nombre completo: ");

            Console.WriteLine();
            Console.WriteLine("Especialidad:");
            Console.WriteLine("1. Portatil");
            Console.WriteLine("2. Escritorio");
            Console.WriteLine("3. Impresora");

            int opcionEspecialidad = LeerEntero("Seleccione la especialidad: ", 1, 3);

            string especialidad;

            switch (opcionEspecialidad)
            {
                case 1:
                    especialidad = "Portatil";
                    break;

                case 2:
                    especialidad = "Escritorio";
                    break;

                default:
                    especialidad = "Impresora";
                    break;
            }

            string codigo = datos.GenerarCodigo("TEC");

            Tecnico tecnico = new Tecnico(codigo, nombre, especialidad);

            gestor.RegistrarTecnico(tecnico);

            Console.WriteLine();
            Console.WriteLine("Técnico registrado correctamente.");
            Console.WriteLine("Código asignado: " + tecnico.Codigo);
        }
        private void MostrarTecnicos()
        {
            Console.Clear();
            Console.WriteLine("TÉCNICOS REGISTRADOS");
            Console.WriteLine();

            if (datos.Tecnicos.Count == 0)
            {
                Console.WriteLine("Todavía no hay técnicos registrados.");
                return;
            }

            foreach (Tecnico tecnico in datos.Tecnicos)
            {
                Console.WriteLine("Código: " + tecnico.Codigo);
                Console.WriteLine("Nombre: " + tecnico.NombreCompleto);
                Console.WriteLine("Especialidad: " + tecnico.Especialidad);

                if (tecnico.Habilitado)
                {
                    Console.WriteLine("Habilitado: Sí");
                }
                else
                {
                    Console.WriteLine("Habilitado: No");
                }

                Console.WriteLine("Límite de trabajos activos: " + tecnico.LimiteActivos);
                Console.WriteLine("Trabajos activos: " + tecnico.CalcularCarga(datos.Solicitudes));

                Console.WriteLine("Servicios entregados: " + Algoritmos.ContarFinalizados(tecnico, datos.Solicitudes));

                if (tecnico.PromedioCalificacion == null)
                {
                    Console.WriteLine("Promedio: Sin calificaciones");
                }
                else
                {
                    Console.WriteLine("Promedio: " + tecnico.PromedioCalificacion);
                }

                Console.WriteLine("--------------------------------");
            }

            Console.WriteLine("Total de técnicos: " + datos.Tecnicos.Count);
        }
        private void CambiarHabilitacionTecnico()
        {
            Console.Clear();
            Console.WriteLine("HABILITAR O DESHABILITAR TÉCNICO");
            Console.WriteLine();

            if (datos.Tecnicos.Count == 0)
            {
                Console.WriteLine("Todavía no hay técnicos registrados.");
                return;
            }

            string codigo = LeerTexto("Código del técnico: ");

            Tecnico tecnico = gestor.BuscarTecnicoPorCodigo(codigo);

            if (tecnico == null)
            {
                Console.WriteLine("No existe un técnico con ese código.");
                return;
            }

            Console.WriteLine("Nombre: " + tecnico.NombreCompleto);
            Console.WriteLine("Especialidad: " + tecnico.Especialidad);

            if (tecnico.Habilitado)
            {
                Console.WriteLine("Estado actual: Habilitado");
            }
            else
            {
                Console.WriteLine("Estado actual: Deshabilitado");
            }

            Console.WriteLine();
            Console.WriteLine("1. Habilitar");
            Console.WriteLine("2. Deshabilitar");
            Console.WriteLine("0. Cancelar");

            int opcion = LeerEntero("Seleccione una opción: ", 0, 2);

            if (opcion == 0)
            {
                Console.WriteLine("Operación cancelada.");
                return;
            }

            bool habilitado;

            if (opcion == 1)
            {
                habilitado = true;
            }
            else
            {
                habilitado = false;
            }

            gestor.HabilitarTecnico(tecnico.Codigo, habilitado);

            if (habilitado)
            {
                Console.WriteLine("El técnico quedó habilitado.");
            }
            else
            {
                Console.WriteLine("El técnico quedó deshabilitado.");
            }
        }
        private void ActualizarNombreTecnico()
        {
            Console.Clear();
            Console.WriteLine("ACTUALIZAR NOMBRE DEL TÉCNICO");
            Console.WriteLine();

            if (datos.Tecnicos.Count == 0)
            {
                Console.WriteLine("Todavía no hay técnicos registrados.");
                return;
            }

            string codigo = LeerTexto("Código del técnico: ");

            Tecnico tecnico = gestor.BuscarTecnicoPorCodigo(codigo);

            if (tecnico == null)
            {
                Console.WriteLine("No existe un técnico con ese código.");
                return;
            }

            Console.WriteLine("Nombre actual: " + tecnico.NombreCompleto);

            string nombre = LeerTexto("Nuevo nombre: ");

            gestor.ActualizarNombreTecnico(tecnico.Codigo, nombre);

            Console.WriteLine();
            Console.WriteLine("Nombre actualizado correctamente.");
            Console.WriteLine("Nombre: " + tecnico.NombreCompleto);
        }

        private void MenuEquipos()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("GESTIÓN DE EQUIPOS");
                Console.WriteLine("1. Registrar portátil");
                Console.WriteLine("2. Registrar escritorio");
                Console.WriteLine("3. Registrar impresora");
                Console.WriteLine("4. Mostrar equipos");
                Console.WriteLine("5. Buscar por número de serie");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Seleccione: ", 0, 5);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                        case 2:
                        case 3:
                            RegistrarEquipo(opcion);
                            break;

                        case 4:
                            MostrarEquipos();
                            break;

                        case 5:
                            string serie = LeerTexto("Número de serie: ");
                            Equipo encontrado = gestor.BuscarEquipoPorSerie(serie);

                            if (encontrado == null)
                            {
                                Console.WriteLine("No se encontró el equipo.");
                            }
                            else
                            {
                                MostrarEquipo(encontrado);
                            }
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Pausar();
                }

            } while (opcion != 0);
        }

        private void RegistrarEquipo(int tipo)
        {
            if (datos.Clientes.Count == 0)
            {
                Console.WriteLine("Primero debe registrar al propietario.");
                return;
            }

            MostrarClientes();

            string codigoCliente = LeerTexto("Código del propietario: ");
            Cliente propietario = gestor.BuscarClientePorCodigo(codigoCliente);

            if (propietario == null)
            {
                throw new ArgumentException("El cliente no está registrado.");
            }

            string serie = LeerTexto("Número de serie: ");

            if (gestor.BuscarEquipoPorSerie(serie) != null)
            {
                throw new ArgumentException("El número de serie ya está registrado.");
            }

            string marca = LeerTexto("Marca: ");
            string modelo = LeerTexto("Modelo: ");
            string estado = LeerTexto("Estado físico recibido: ");

            List<string> accesorios = new List<string>();

            if (tipo == 1)
            {
                int cargador = LeerEntero("¿Incluye cargador? 1 Sí / 2 No: ", 1, 2);

                if (cargador == 1)
                {
                    accesorios.Add("Cargador");
                }

                Console.WriteLine("Registre los demás accesorios, si existen.");
            }

            int cantidad = LeerEntero("Cantidad de accesorios a agregar (0 si ninguno): ", 0, 30);

            for (int i = 0; i < cantidad; i++)
            {
                accesorios.Add(LeerTexto("Accesorio " + (i + 1) + ": "));
            }

            string codigo = datos.GenerarCodigo("EQU");
            Equipo equipo;

            if (tipo == 1)
            {
                equipo = new Portatil(codigo, serie, marca, modelo, propietario, estado, accesorios);
            }
            else if (tipo == 2)
            {
                string gabinete = LeerTexto("Formato del gabinete: ");

                equipo = new Escritorio(codigo, serie, marca, modelo, propietario, estado, accesorios, gabinete);
            }
            else
            {
                string tecnologia = LeerTexto("Tecnología de impresión: ");

                equipo = new Impresora(
                    codigo, serie, marca, modelo,
                    propietario, estado, accesorios, tecnologia
                );
            }

            gestor.RegistrarEquipo(equipo);

            Console.WriteLine("Equipo registrado: " + equipo.Codigo);
        }

        private void MostrarEquipos()
        {
            Console.WriteLine("EQUIPOS REGISTRADOS");

            if (datos.Equipos.Count == 0)
            {
                Console.WriteLine("Sin equipos registrados.");
            }

            foreach (Equipo equipo in datos.Equipos)
            {
                MostrarEquipo(equipo);
            }

            Console.WriteLine("Total de equipos: " + datos.Equipos.Count);
        }

        private void MostrarEquipo(Equipo equipo)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Código: " + equipo.Codigo);
            Console.WriteLine("Tipo: " + equipo.Tipo);
            Console.WriteLine("Serie: " + equipo.NumeroSerie);
            Console.WriteLine("Marca: " + equipo.Marca);
            Console.WriteLine("Modelo: " + equipo.Modelo);
            Console.WriteLine("Propietario: " + equipo.Propietario.NombreCompleto);
            Console.WriteLine("Estado físico: " + equipo.EstadoFisico);

            List<string> accesorios = equipo.Accesorios;

            if (accesorios.Count == 0)
            {
                Console.WriteLine("Accesorios: Ninguno");
            }
            else
            {
                Console.WriteLine("Accesorios:");

                foreach (string accesorio in accesorios)
                {
                    Console.WriteLine("- " + accesorio);
                }
            }

            if (equipo is Portatil)
            {
                Portatil portatil = (Portatil)equipo;
                Console.WriteLine("Incluye cargador: " + portatil.IncluyeCargador);
            }
            else if (equipo is Escritorio)
            {
                Escritorio escritorio = (Escritorio)equipo;
                Console.WriteLine("Gabinete: " + escritorio.FormatoGabinete);
            }
            else if (equipo is Impresora)
            {
                Impresora impresora = (Impresora)equipo;
                Console.WriteLine("Tecnología: " + impresora.Tecnologia);
            }

            Console.WriteLine("Guía: " + equipo.ObtenerGuiaDiagnostico());
        }
        private void MenuRepuestos()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("GESTIÓN DE REPUESTOS");
                Console.WriteLine("1. Registrar repuesto");
                Console.WriteLine("2. Mostrar repuestos");
                Console.WriteLine("3. Entrada a inventario");
                Console.WriteLine("4. Actualizar datos y precios");
                Console.WriteLine("5. Mostrar mínimos y agotados");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Seleccione: ", 0, 5);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarRepuesto();
                            break;

                        case 2:
                            MostrarRepuestos(false);
                            break;

                        case 3:
                            EntradaInventario();
                            break;

                        case 4:
                            ActualizarRepuesto();
                            break;

                        case 5:
                            MostrarRepuestos(true);
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Pausar();
                }

            } while (opcion != 0);
        }

        private void RegistrarRepuesto()
        {
            string nombre = LeerTexto("Nombre: ");
            string categoria = LeerTexto("Categoría: ");

            int existencia = LeerEntero("Existencia inicial: ", 0, int.MaxValue);

            int minimo = LeerEntero("Existencia mínima: ", 0, int.MaxValue);

            decimal costo = LeerDecimal("Costo unitario Q: ", 0);
            decimal precio = LeerDecimal("Precio de venta Q: ", costo);

            Repuesto repuesto = new Repuesto(datos.GenerarCodigo("REP"), nombre, categoria, existencia, minimo, costo, precio);

            gestor.RegistrarRepuesto(repuesto);

            Console.WriteLine("Repuesto registrado: " + repuesto.Codigo);
            MostrarRepuesto(repuesto);
        }

        private void MostrarRepuestos(bool soloMinimos)
        {
            if (soloMinimos)
            {
                Console.WriteLine("REPUESTOS EN MÍNIMO O AGOTADOS");
            }
            else
            {
                Console.WriteLine("REPUESTOS REGISTRADOS");
            }

            int cantidad = 0;

            foreach (Repuesto repuesto in datos.Repuestos)
            {
                if (!soloMinimos || repuesto.EstaEnMinimo())
                {
                    MostrarRepuesto(repuesto);
                    cantidad++;
                }
            }

            if (cantidad == 0)
            {
                Console.WriteLine("Sin registros para mostrar.");
            }

            Console.WriteLine("Total mostrado: " + cantidad);
        }

        private void MostrarRepuesto(Repuesto repuesto)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Código: " + repuesto.Codigo);
            Console.WriteLine("Nombre: " + repuesto.Nombre);
            Console.WriteLine("Categoría: " + repuesto.Categoria);
            Console.WriteLine("Existencia: " + repuesto.Existencia);
            Console.WriteLine("Existencia mínima: " + repuesto.ExistenciaMinima);
            Console.WriteLine("Costo Q" + repuesto.CostoUnitario.ToString("F2"));
            Console.WriteLine("Precio Q" + repuesto.PrecioVenta.ToString("F2"));
            Console.WriteLine("Cantidad utilizada: " + repuesto.CantidadUtilizada);

            if (repuesto.EstaEnMinimo())
            {
                Console.WriteLine("ALERTA: existencia mínima o agotada.");
            }
        }

        private void EntradaInventario()
        {
            string codigo = LeerTexto("Código del repuesto: ");
            Repuesto repuesto = gestor.BuscarRepuestoPorCodigo(codigo);

            if (repuesto == null)
            {
                throw new ArgumentException("El repuesto no existe.");
            }

            MostrarRepuesto(repuesto);

            int cantidad = LeerEntero("Cantidad que ingresa: ", 1, int.MaxValue);

            gestor.RegistrarEntradaInventario(codigo, cantidad);

            Console.WriteLine("Entrada registrada.");
            MostrarRepuesto(repuesto);
        }

        private void ActualizarRepuesto()
        {
            string codigo = LeerTexto("Código del repuesto: ");
            Repuesto repuesto = gestor.BuscarRepuestoPorCodigo(codigo);

            if (repuesto == null)
            {
                throw new ArgumentException("El repuesto no existe.");
            }

            MostrarRepuesto(repuesto);

            Console.WriteLine("Escriba todos los datos; repita el valor actual si no cambia.");

            string nombre = LeerTexto("Nombre: ");
            string categoria = LeerTexto("Categoría: ");

            int minimo = LeerEntero("Existencia mínima: ", 0, int.MaxValue);

            decimal costo = LeerDecimal("Costo Q: ", 0);
            decimal precio = LeerDecimal("Precio Q: ", costo);

            gestor.ActualizarRepuesto(codigo, nombre, categoria, minimo, costo, precio);

            Console.WriteLine("Repuesto actualizado correctamente.");
        }
        private void MenuSolicitudes()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("SOLICITUDES DE SERVICIO");
                Console.WriteLine("1. Crear solicitud");
                Console.WriteLine("2. Mostrar solicitudes");
                Console.WriteLine("3. Iniciar diagnóstico");
                Console.WriteLine("4. Registrar o actualizar diagnóstico");
                Console.WriteLine("5. Iniciar reparación");
                Console.WriteLine("6. Registrar acción realizada");
                Console.WriteLine("7. Usar repuesto");
                Console.WriteLine("8. Confirmar costo de mano de obra");
                Console.WriteLine("9. Marcar lista para entregar");
                Console.WriteLine("10. Entregar");
                Console.WriteLine("11. Cancelar solicitud");
                Console.WriteLine("12. Consultar historial");
                Console.WriteLine("13. Deshacer último cambio de estado");
                Console.WriteLine("14. Enviar recibida a espera");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Seleccione: ", 0, 14);

                try
                {
                    if (opcion == 1)
                    {
                        CrearSolicitud();
                    }
                    else if (opcion == 2)
                    {
                        if (datos.Solicitudes.Count == 0)
                        {
                            Console.WriteLine("Sin solicitudes.");
                        }

                        foreach (SolicitudServicio solicitud in datos.Solicitudes)
                        {
                            MostrarSolicitud(solicitud);
                        }
                    }
                    else if (opcion >= 3)
                    {
                        OperarSolicitud(opcion);
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Pausar();
                }

            } while (opcion != 0);
        }
        private void OperarSolicitud(int opcion)
        {
            string codigo = LeerTexto("Código de solicitud: ");

            SolicitudServicio solicitud = gestor.BuscarSolicitudPorCodigo(codigo);

            if (solicitud == null)
            {
                throw new ArgumentException("La solicitud no existe.");
            }

            MostrarSolicitud(solicitud);

            if (opcion == 10 || opcion == 11 || opcion == 13)
            {
                if (opcion == 10)
                {
                    Console.WriteLine("Se registrará la entrega y el total mostrado.");
                }

                if (opcion == 11)
                {
                    Console.WriteLine("Se cancelará. Los repuestos usados no se devolverán automáticamente.");
                }

                if (opcion == 13)
                {
                    List<CambioEstado> cambios = solicitud.ObtenerHistorial();

                    if (cambios.Count == 0)
                    {
                        throw new ArgumentException("El historial está vacío.");
                    }

                    Console.WriteLine("Se intentará volver a: " + cambios[0].Anterior);
                }

                int confirmar = LeerEntero("¿Confirmar? 1 Sí / 2 No: ", 1, 2);

                if (confirmar == 2)
                {
                    return;
                }
            }

            switch (opcion)
            {
                case 3:
                    gestor.CambiarEstado(codigo, "EnDiagnostico");
                    Console.WriteLine(solicitud.Equipo.ObtenerGuiaDiagnostico());
                    break;

                case 4:
                    string hallazgos = LeerTexto("Hallazgos y repuestos que se necesitan: ");

                    int minutos = LeerEntero("Tiempo estimado en minutos: ", 1, int.MaxValue);

                    gestor.RegistrarDiagnostico(codigo, hallazgos, minutos);
                    break;

                case 5:
                    gestor.CambiarEstado(codigo, "EnReparacion");
                    break;

                case 6:
                    gestor.RegistrarAccion(codigo, LeerTexto("Acción realizada: "));
                    break;

                case 7:
                    MostrarRepuestos(false);

                    string repuesto = LeerTexto("Código del repuesto: ");

                    int cantidad = LeerEntero("Cantidad utilizada: ", 1, int.MaxValue);

                    gestor.UsarRepuesto(codigo, repuesto, cantidad);

                    MostrarRepuesto(gestor.BuscarRepuestoPorCodigo(repuesto));
                    break;

                case 8:
                    decimal importe = LeerDecimal("Mano de obra Q (0 si no se cobra): ", 0);

                    gestor.RegistrarManoObra(codigo, importe);
                    break;

                case 9:
                    gestor.CambiarEstado(codigo, "ListaParaEntregar");
                    break;

                case 10:
                    int valor = LeerEntero("Calificación 1 a 5, o 0 para omitir: ", 0, 5);

                    int? nota = null;

                    if (valor != 0)
                    {
                        nota = valor;
                    }

                    gestor.Entregar(codigo, nota);
                    break;

                case 11:
                    gestor.Cancelar(codigo, LeerTexto("Motivo de cancelación: "));
                    break;

                case 12:
                    List<CambioEstado> historial = solicitud.ObtenerHistorial();

                    if (historial.Count == 0)
                    {
                        Console.WriteLine("Historial vacío.");
                    }

                    foreach (CambioEstado cambio in historial)
                    {
                        Console.WriteLine(cambio.Describir());
                    }
                    return;

                case 13:
                    gestor.DeshacerEstado(codigo);
                    break;

                case 14:
                    gestor.CambiarEstado(codigo, "EnEspera");
                    break;
            }

            Console.WriteLine("Operación realizada correctamente.");
            MostrarSolicitud(solicitud);
        }

        private void CrearSolicitud()
        {
            if (datos.Clientes.Count == 0 || datos.Equipos.Count == 0)
            {
                Console.WriteLine("Primero registre un cliente y su equipo.");
                return;
            }

            MostrarEquipos();

            string cliente = LeerTexto("Código del cliente: ");
            string equipo = LeerTexto("Código del equipo: ");
            string problema = LeerTexto("Descripción del problema: ");

            int opcion = LeerEntero("Prioridad: 1 Normal / 2 Urgente: ", 1, 2);

            string prioridad;

            if (opcion == 1)
            {
                prioridad = "Normal";
            }
            else
            {
                prioridad = "Urgente";
            }

            SolicitudServicio solicitud = gestor.CrearSolicitud(cliente, equipo, problema, prioridad);

            Console.WriteLine("Solicitud creada y encolada correctamente.");
            MostrarSolicitud(solicitud);
        }

        private void MostrarSolicitud(SolicitudServicio solicitud)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Código: " + solicitud.Codigo);
            Console.WriteLine("Cliente: " + solicitud.Cliente.NombreCompleto);
            Console.WriteLine("Equipo: " + solicitud.Equipo.Codigo + " - " + solicitud.Equipo.Tipo);
            Console.WriteLine("Problema: " + solicitud.Problema);
            Console.WriteLine("Recepción: " + solicitud.FechaRecepcion);
            Console.WriteLine("Prioridad: " + solicitud.Prioridad);
            Console.WriteLine("Estado: " + solicitud.Estado);

            if (solicitud.TecnicoAsignado == null)
            {
                Console.WriteLine("Técnico: Sin asignar");
            }
            else
            {
                Console.WriteLine("Técnico: " + solicitud.TecnicoAsignado.Codigo + " - " + solicitud.TecnicoAsignado.NombreCompleto);
            }

            if (solicitud.FechaAsignacion != null)
            {
                Console.WriteLine("Fecha de asignación: " + solicitud.FechaAsignacion);
            }
            if (solicitud.Diagnostico != null)
            {
                Console.WriteLine("Diagnóstico: " + solicitud.Diagnostico.Hallazgos);

                Console.WriteLine("Tiempo estimado: " + solicitud.Diagnostico.TiempoEstimadoMin + " minutos");

                foreach (AccionRealizada accion in solicitud.Diagnostico.Acciones)
                {
                    Console.WriteLine("Acción " + accion.Codigo + ": " + accion.Descripcion);
                }
            }

            foreach (DetalleRepuesto detalle in solicitud.Detalles)
            {
                Console.WriteLine(detalle.Repuesto.Codigo + " | cantidad: " + detalle.Cantidad + " | precio Q" + detalle.PrecioAplicado.ToString("F2") + " | subtotal Q" + detalle.CalcularSubtotal().ToString("F2"));
            }

            Console.WriteLine("Mano de obra: Q" + solicitud.CostoManoObra.ToString("F2"));

            Console.WriteLine("Repuestos: Q" + solicitud.CostoRepuestos.ToString("F2"));

            Console.WriteLine("TOTAL: Q" + solicitud.Total.ToString("F2"));

            if (!solicitud.ManoObraConfirmada)
            {
                Console.WriteLine("Mano de obra pendiente de confirmar.");
            }

            if (solicitud.FechaEntrega != null)
            {
                Console.WriteLine("Entrega: " + solicitud.FechaEntrega);
            }

            if (solicitud.Calificacion != null)
            {
                Console.WriteLine("Calificación: " + solicitud.Calificacion);
            }

            if (solicitud.Estado == "Cancelada")
            {
                Console.WriteLine("Motivo: " + solicitud.MotivoCancelacion);
            }
        }
        private void MenuColas()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("COLAS DE ATENCIÓN");
                Console.WriteLine("1. Ver urgentes");
                Console.WriteLine("2. Ver normales");
                Console.WriteLine("3. Atender siguiente");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Seleccione: ", 0, 3);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            MostrarCola(gestor.ObtenerColaUrgente());
                            break;

                        case 2:
                            MostrarCola(gestor.ObtenerColaNormal());
                            break;

                        case 3:
                            SolicitudServicio atendida = gestor.AtenderSiguiente();

                            if (atendida == null)
                            {
                                Console.WriteLine("No hay solicitudes en espera.");
                            }
                            else
                            {
                                Console.WriteLine("Asignación realizada correctamente.");

                                MostrarSolicitud(atendida);
                            }
                            break;
                    }
                }
                catch (ArgumentException error)
                {
                    Console.WriteLine("Error: " + error.Message);
                }

                if (opcion != 0)
                {
                    Pausar();
                }

            } while (opcion != 0);
        }

        private void MostrarCola(SolicitudServicio[] cola)
        {
            if (cola.Length == 0)
            {
                Console.WriteLine("Esta cola está vacía.");
            }

            for (int i = 0; i < cola.Length; i++)
            {
                Console.WriteLine("Turno: " + (i + 1));
                MostrarSolicitud(cola[i]);
            }
        }
        private int LeerEntero(string mensaje, int minimo, int maximo)
        {
            int valor;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();

                esValido = int.TryParse(entrada, out valor);

                if (!esValido)
                {
                    Console.WriteLine("Debe ingresar un número entero.");
                }
                else if (valor < minimo || valor > maximo)
                {
                    Console.WriteLine("Ingrese un número entre " + minimo + " y " + maximo + ".");

                    esValido = false;
                }

            } while (!esValido);

            return valor;
        }

        private string LeerTexto(string mensaje)
        {
            string texto;

            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine("Este dato no puede estar vacío.");
                }

            } while (string.IsNullOrWhiteSpace(texto));

            return texto.Trim();
        }
        private decimal LeerDecimal(string mensaje, decimal minimo)
        {
            decimal valor;
            bool valido;

            do
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();

                if (entrada == null)
                {
                    throw new ArgumentException("No se recibió un importe.");
                }

                entrada = entrada.Trim().Replace(',', '.');

                valido = decimal.TryParse(
                    entrada,
                    System.Globalization.NumberStyles.AllowLeadingSign |
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out valor
                );

                if (!valido || valor < minimo)
                {
                    Console.WriteLine(
                        "Ingrese un importe mayor o igual a " + minimo +
                        ", sin separadores de miles."
                    );

                    valido = false;
                }

            } while (!valido);

            return valor;
        }

        private void Pausar()
        {
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }
    }
    public class SolicitudServicio
    {
        private string codigo;
        private Cliente cliente;
        private Equipo equipo;
        private string problema;
        private DateTime fechaRecepcion;
        private string prioridad;
        private string estado;
        private Tecnico tecnicoAsignado;
        private DateTime? fechaAsignacion;
        private int ordenLlegada;
        private Diagnostico diagnostico;
        private List<DetalleRepuesto> detalles;
        private Stack<CambioEstado> historial;
        private decimal costoManoObra;
        private bool manoObraConfirmada;
        private DateTime? fechaEntrega;
        private int? calificacion;
        private string motivoCancelacion;

        public string Codigo
        {
            get { return codigo; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
        }

        public Equipo Equipo
        {
            get { return equipo; }
        }

        public string Problema
        {
            get { return problema; }
        }

        public DateTime FechaRecepcion
        {
            get { return fechaRecepcion; }
        }

        public string Prioridad
        {
            get { return prioridad; }
        }

        public string Estado
        {
            get { return estado; }
        }

        public Tecnico TecnicoAsignado
        {
            get { return tecnicoAsignado; }
        }

        public DateTime? FechaAsignacion
        {
            get { return fechaAsignacion; }
        }

        public int OrdenLlegada
        {
            get { return ordenLlegada; }
        }
        public Diagnostico Diagnostico
        {
            get { return diagnostico; }
        }

        public List<DetalleRepuesto> Detalles
        {
            get { return new List<DetalleRepuesto>(detalles); }
        }

        public decimal CostoManoObra
        {
            get { return costoManoObra; }
        }

        public bool ManoObraConfirmada
        {
            get { return manoObraConfirmada; }
        }

        public DateTime? FechaEntrega
        {
            get { return fechaEntrega; }
        }

        public int? Calificacion
        {
            get { return calificacion; }
        }

        public string MotivoCancelacion
        {
            get { return motivoCancelacion; }
        }

        public decimal CostoRepuestos
        {
            get
            {
                decimal suma = 0;

                foreach (DetalleRepuesto detalle in detalles)
                {
                    suma = suma + detalle.CalcularSubtotal();
                }

                return suma;
            }
        }

        public decimal Total
        {
            get { return costoManoObra + CostoRepuestos; }
        }

        public SolicitudServicio(string codigo, Cliente cliente, Equipo equipo, string problema, int ordenLlegada) : this(codigo, cliente, equipo, problema, "Normal", ordenLlegada)
        {
        }

        public SolicitudServicio(string codigo, Cliente cliente, Equipo equipo, string problema, string prioridad, int ordenLlegada)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código es obligatorio.");
            }

            if (cliente == null || equipo == null)
            {
                throw new ArgumentException("Debe indicar un cliente y un equipo.");
            }

            if (equipo.Propietario != cliente)
            {
                throw new ArgumentException("El cliente debe ser propietario del equipo.");
            }

            if (string.IsNullOrWhiteSpace(problema))
            {
                throw new ArgumentException("Debe describir el problema.");
            }

            if (prioridad != "Normal" && prioridad != "Urgente")
            {
                throw new ArgumentException("La prioridad debe ser Normal o Urgente.");
            }

            if (ordenLlegada <= 0)
            {
                throw new ArgumentException("El orden de llegada debe ser positivo.");
            }

            this.codigo = codigo.Trim();
            this.cliente = cliente;
            this.equipo = equipo;
            this.problema = problema.Trim();
            this.prioridad = prioridad;
            this.ordenLlegada = ordenLlegada;

            fechaRecepcion = DateTime.Now;
            estado = "Recibida";
            tecnicoAsignado = null;
            fechaAsignacion = null;
            diagnostico = null;
            detalles = new List<DetalleRepuesto>();
            historial = new Stack<CambioEstado>();

            costoManoObra = 0;
            manoObraConfirmada = false;
            fechaEntrega = null;
            calificacion = null;
            motivoCancelacion = null;
        }

        public bool EsActiva()
        {
            return estado != "Entregada" && estado != "Cancelada";
        }

        internal void PasarAEspera(string codigoCambio)
        {
            CambiarEstado("EnEspera", codigoCambio);
        }

        internal void AsignarTecnico(Tecnico tecnico, string codigoCambio)
        {
            if (estado != "EnEspera")
            {
                throw new ArgumentException("La solicitud no está en espera.");
            }

            if (tecnico == null || !tecnico.Habilitado || tecnico.Especialidad != equipo.Tipo)
            {
                throw new ArgumentException("El técnico no está habilitado o no es compatible.");
            }

            DateTime fecha = DateTime.Now;

            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new ArgumentException("Las asignaciones se realizan de lunes a sábado.");
            }

            if (fecha < fechaRecepcion)
            {
                throw new ArgumentException("La asignación no puede ser anterior a la recepción.");
            }

            GuardarCambio(codigoCambio, "Asignada");

            tecnicoAsignado = tecnico;
            fechaAsignacion = fecha;
        }
        private void ExigirActiva()
        {
            if (!EsActiva())
            {
                throw new ArgumentException("Una solicitud entregada o cancelada no se puede modificar.");
            }
        }

        private void GuardarCambio(string codigoCambio, string destino)
        {
            CambioEstado cambio = new CambioEstado(codigoCambio, estado, destino, tecnicoAsignado, fechaAsignacion);

            historial.Push(cambio);
            estado = destino;
        }

        public List<CambioEstado> ObtenerHistorial()
        {
            return new List<CambioEstado>(historial);
        }

        internal void CambiarEstado(string destino, string codigoCambio)
        {
            ExigirActiva();

            bool permitido = false;

            if (estado == "Recibida" && destino == "EnEspera")
            {
                permitido = true;
            }
            else if (estado == "Asignada" && destino == "EnDiagnostico")
            {
                if (tecnicoAsignado == null || !tecnicoAsignado.Habilitado)
                {
                    throw new ArgumentException("El técnico debe estar asignado y habilitado.");
                }

                permitido = true;
            }
            else if (estado == "EnDiagnostico" && destino == "EnReparacion")
            {
                if (diagnostico == null)
                {
                    throw new ArgumentException("Primero registre el diagnóstico y su tiempo estimado.");
                }

                permitido = true;
            }
            else if (estado == "EnReparacion" && destino == "ListaParaEntregar")
            {
                if (diagnostico == null || diagnostico.Acciones.Count == 0)
                {
                    throw new ArgumentException("Debe registrar al menos una acción realizada.");
                }

                if (!manoObraConfirmada)
                {
                    throw new ArgumentException("Debe confirmar la mano de obra, aunque sea Q0.");
                }

                permitido = true;
            }

            if (!permitido)
            {
                throw new ArgumentException("No se permite pasar de " + estado + " a " + destino + ".");
            }

            GuardarCambio(codigoCambio, destino);
        }

        internal void RegistrarDiagnostico(string codigo, string hallazgos, int minutos)
        {
            ExigirActiva();

            if (estado != "EnDiagnostico")
            {
                throw new ArgumentException("La solicitud debe estar en diagnóstico.");
            }

            if (diagnostico == null)
            {
                diagnostico = new Diagnostico(codigo, hallazgos, minutos);
            }
            else
            {
                diagnostico.ActualizarHallazgos(hallazgos, minutos);
            }
        }

        internal void RegistrarAccion(string codigo, string descripcion)
        {
            ExigirActiva();

            if (estado != "EnReparacion" || diagnostico == null)
            {
                throw new ArgumentException("Las acciones se registran durante la reparación.");
            }

            diagnostico.AgregarAccion(new AccionRealizada(codigo, descripcion));
        }

        internal void UsarRepuesto(string codigoDetalle, Repuesto repuesto, int cantidad)
        {
            ExigirActiva();

            if (estado != "EnReparacion")
            {
                throw new ArgumentException("Los repuestos se utilizan durante la reparación.");
            }

            DetalleRepuesto detalle = new DetalleRepuesto(codigoDetalle, repuesto, cantidad);

            if (detalle.CalcularSubtotal() > decimal.MaxValue - Total)
            {
                throw new ArgumentException("El total supera el importe permitido.");
            }
            repuesto.Consumir(cantidad);
            detalles.Add(detalle);
        }

        internal void RegistrarManoObra(decimal importe)
        {
            ExigirActiva();

            if (estado != "EnReparacion")
            {
                throw new ArgumentException("La mano de obra se confirma durante la reparación.");
            }

            if (importe < 0 || importe > decimal.MaxValue - CostoRepuestos)
            {
                throw new ArgumentException("El importe de mano de obra no es válido.");
            }

            costoManoObra = importe;
            manoObraConfirmada = true;
        }

        internal void Entregar(string codigoCambio, int? nota)
        {
            ExigirActiva();

            if (estado != "ListaParaEntregar" || tecnicoAsignado == null || fechaAsignacion == null)
            {
                throw new ArgumentException("La solicitud debe estar lista para entregar.");
            }

            if (nota != null && (nota < 1 || nota > 5))
            {
                throw new ArgumentException("La calificación debe estar entre 1 y 5, o no registrarse.");
            }

            DateTime fecha = DateTime.Now;

            if (fecha < fechaAsignacion.Value)
            {
                throw new ArgumentException("La entrega no puede ser anterior a la asignación.");
            }

            GuardarCambio(codigoCambio, "Entregada");

            fechaEntrega = fecha;
            calificacion = nota;

            if (nota != null)
            {
                tecnicoAsignado.RegistrarCalificacion(nota.Value);
            }
        }

        internal void Cancelar(string codigoCambio, string motivo)
        {
            ExigirActiva();

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("Debe indicar el motivo de cancelación.");
            }

            GuardarCambio(codigoCambio, "Cancelada");
            motivoCancelacion = motivo.Trim();
        }

        internal void DeshacerEstado()
        {
            ExigirActiva();

            if (historial.Count == 0)
            {
                throw new ArgumentException("El historial está vacío.");
            }

            CambioEstado cambio = historial.Peek();

            if (cambio.Nuevo != estado)
            {
                throw new ArgumentException("El historial no coincide con el estado actual.");
            }

            if (estado == "EnReparacion" && (detalles.Count > 0 || (diagnostico != null && diagnostico.Acciones.Count > 0)))
            {
                throw new ArgumentException("No puede volver a diagnóstico si ya hay acciones o repuestos usados.");
            }

            if (estado == "EnDiagnostico" && diagnostico != null)
            {
                throw new ArgumentException("No puede volver a asignada si ya guardó un diagnóstico.");
            }

            estado = cambio.Anterior;
            tecnicoAsignado = cambio.TecnicoAnterior;
            fechaAsignacion = cambio.FechaAsignacionAnterior;

            historial.Pop();
        }
    }
    public class AccionRealizada
    {
        private string codigo;
        private string descripcion;
        private DateTime fecha;

        public string Codigo { get { return codigo; } }
        public string Descripcion { get { return descripcion; } }
        public DateTime Fecha { get { return fecha; } }

        public AccionRealizada(string codigo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("La acción necesita código y descripción.");
            }

            this.codigo = codigo.Trim();
            this.descripcion = descripcion.Trim();
            fecha = DateTime.Now;
        }
    }

    public class Diagnostico
    {
        private string codigo;
        private DateTime fecha;
        private string hallazgos;
        private int tiempoEstimadoMin;
        private List<AccionRealizada> acciones;

        public string Codigo { get { return codigo; } }
        public DateTime Fecha { get { return fecha; } }
        public string Hallazgos { get { return hallazgos; } }
        public int TiempoEstimadoMin { get { return tiempoEstimadoMin; } }

        public List<AccionRealizada> Acciones
        {
            get { return new List<AccionRealizada>(acciones); }
        }

        public Diagnostico(string codigo, string hallazgos, int minutos)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El diagnóstico necesita un código.");
            }

            this.codigo = codigo.Trim();
            ActualizarHallazgos(hallazgos, minutos);

            fecha = DateTime.Now;
            acciones = new List<AccionRealizada>();
        }

        internal void ActualizarHallazgos(string texto, int minutos)
        {
            if (string.IsNullOrWhiteSpace(texto) || minutos <= 0)
            {
                throw new ArgumentException("Escriba hallazgos y un tiempo mayor que cero.");
            }

            hallazgos = texto.Trim();
            tiempoEstimadoMin = minutos;
        }

        internal void AgregarAccion(AccionRealizada accion)
        {
            if (accion == null)
            {
                throw new ArgumentException("La acción es obligatoria.");
            }

            acciones.Add(accion);
        }
    }

    public class DetalleRepuesto
    {
        private string codigo;
        private Repuesto repuesto;
        private int cantidad;
        private decimal precioAplicado;
        private DateTime fechaUso;

        public string Codigo { get { return codigo; } }
        public Repuesto Repuesto { get { return repuesto; } }
        public int Cantidad { get { return cantidad; } }
        public decimal PrecioAplicado { get { return precioAplicado; } }
        public DateTime FechaUso { get { return fechaUso; } }

        public DetalleRepuesto(string codigo, Repuesto repuesto, int cantidad)
        {
            if (string.IsNullOrWhiteSpace(codigo) || repuesto == null || cantidad <= 0)
            {
                throw new ArgumentException("El detalle necesita código, repuesto y cantidad positiva.");
            }

            if (repuesto.PrecioVenta > decimal.MaxValue / cantidad)
            {
                throw new ArgumentException("El subtotal supera el importe permitido.");
            }

            this.codigo = codigo.Trim();
            this.repuesto = repuesto;
            this.cantidad = cantidad;
            precioAplicado = repuesto.PrecioVenta;
            fechaUso = DateTime.Now;
        }

        public decimal CalcularSubtotal()
        {
            return cantidad * precioAplicado;
        }
    }

    public class CambioEstado
    {
        private string codigo;
        private string anterior;
        private string nuevo;
        private DateTime fecha;
        private Tecnico tecnicoAnterior;
        private DateTime? fechaAsignacionAnterior;

        public string Codigo { get { return codigo; } }
        public string Anterior { get { return anterior; } }
        public string Nuevo { get { return nuevo; } }
        public DateTime Fecha { get { return fecha; } }
        public Tecnico TecnicoAnterior { get { return tecnicoAnterior; } }

        public DateTime? FechaAsignacionAnterior
        {
            get { return fechaAsignacionAnterior; }
        }

        public CambioEstado(string codigo, string anterior, string nuevo, Tecnico tecnicoAnterior, DateTime? fechaAnterior)
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(anterior) || string.IsNullOrWhiteSpace(nuevo))
            {
                throw new ArgumentException("El cambio necesita código y estados.");
            }

            this.codigo = codigo.Trim();
            this.anterior = anterior;
            this.nuevo = nuevo;
            this.tecnicoAnterior = tecnicoAnterior;
            fechaAsignacionAnterior = fechaAnterior;
            fecha = DateTime.Now;
        }

        public string Describir()
        {
            return Codigo + " | " + Fecha + " | " + Anterior + " -> " + Nuevo;
        }
    }
    public class GeneradorReportes
    {
        private DatosSistema datos;
        private GestorAtencion gestor;

        public GeneradorReportes(DatosSistema datos, GestorAtencion gestor)
        {
            if (datos == null || gestor == null)
            {
                throw new ArgumentException("Faltan los datos o el gestor.");
            }

            this.datos = datos;
            this.gestor = gestor;
        }

        private void Encabezado(string titulo)
        {
            Console.WriteLine("\n" + titulo);
            Console.WriteLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        }

        private string NombreTecnico(SolicitudServicio solicitud)
        {
            if (solicitud.TecnicoAsignado == null)
            {
                return "Sin asignar";
            }

            return solicitud.TecnicoAsignado.Codigo + " - " + solicitud.TecnicoAsignado.NombreCompleto;
        }

        private void MostrarCola(string titulo, SolicitudServicio[] cola)
        {
            Console.WriteLine("\n" + titulo + " | Total: " + cola.Length);

            for (int i = 0; i < cola.Length; i++)
            {
                SolicitudServicio s = cola[i];

                Console.WriteLine((i + 1) + " | " + s.Codigo + " | " + s.FechaRecepcion + " | " + s.Cliente.NombreCompleto + " | " + s.Equipo.Codigo + " | " + s.Problema);
            }

            if (cola.Length == 0)
            {
                return;
            }

            bool disponible = false;

            foreach (Tecnico t in datos.Tecnicos)
            {
                if (t.Habilitado && t.Especialidad == cola[0].Equipo.Tipo && t.CalcularCarga(datos.Solicitudes) < t.LimiteActivos)
                {
                    disponible = true;
                }
            }

            if (!disponible)
            {
                Console.WriteLine("Frente bloqueado: no hay técnico habilitado de " + cola[0].Equipo.Tipo + " con cupo.");
            }
            else
            {
                Console.WriteLine("Hay técnico compatible con cupo para el frente.");
            }
        }

        public void SolicitudesEnEspera()
        {
            Encabezado("SOLICITUDES EN ESPERA - FIFO");

            MostrarCola("URGENTES", gestor.ObtenerColaUrgente());
            MostrarCola("NORMALES", gestor.ObtenerColaNormal());

            Console.WriteLine("Se atienden primero las urgentes; no se salta un frente bloqueado.");
        }

        public void SolicitudesPorSituacion()
        {
            Encabezado("SOLICITUDES POR SITUACIÓN");

            string[] grupos = { "ACTIVAS", "ENTREGADAS", "CANCELADAS" };

            for (int i = 0; i < grupos.Length; i++)
            {
                int cantidad = 0;
                Console.WriteLine("\n" + grupos[i]);

                foreach (SolicitudServicio s in datos.Solicitudes)
                {
                    if ((i == 0 && s.EsActiva()) || (i == 1 && s.Estado == "Entregada") || (i == 2 && s.Estado == "Cancelada"))
                    {
                        Console.WriteLine(s.Codigo + " | " + s.Cliente.NombreCompleto + " | " + s.Equipo.Codigo + " | " + s.Prioridad + " | " + s.Estado +  " | " + NombreTecnico(s) +  " | Q " + s.Total.ToString("F2"));

                        cantidad++;
                    }
                }

                Console.WriteLine("Total: " + cantidad);
            }
        }

        public void DisponibilidadTecnicos()
        {
            Encabezado("DISPONIBILIDAD DE TÉCNICOS");

            foreach (Tecnico t in datos.Tecnicos)
            {
                int carga = t.CalcularCarga(datos.Solicitudes);
                string estado = "Disponible";

                if (!t.Habilitado)
                {
                    estado = "Deshabilitado";
                }
                else if (carga >= t.LimiteActivos)
                {
                    estado = "Carga completa";
                }

                Console.WriteLine(t.Codigo + " | " + t.NombreCompleto + " | " + t.Especialidad + " | " + estado + " | Carga: " + carga + "/" + t.LimiteActivos + " | Cupos: " + (t.LimiteActivos - carga));
            }

            Console.WriteLine("Técnicos registrados: " + datos.Tecnicos.Count);
        }

        public void RankingTecnicos()
        {
            Encabezado("TÉCNICOS POR SERVICIOS ENTREGADOS - SELECCIÓN DESCENDENTE");

            Tecnico[] copia = datos.Tecnicos.ToArray();
            Algoritmos.Seleccion(copia, datos.Solicitudes, false);

            int maximo = 0;

            if (copia.Length > 0)
            {
                maximo = Algoritmos.ContarFinalizados(copia[0], datos.Solicitudes);
            }

            for (int i = 0; i < copia.Length; i++)
            {
                Tecnico t = copia[i];
                string promedio = "Sin calificaciones";

                if (t.PromedioCalificacion.HasValue)
                {
                    promedio = t.PromedioCalificacion.Value.ToString("F2");
                }

                Console.WriteLine((i + 1) + " | " + t.Codigo + " | " + t.NombreCompleto + " | Entregados: " + Algoritmos.ContarFinalizados(t, datos.Solicitudes) + " | Promedio: " + promedio);
            }

            if (maximo == 0)
            {
                Console.WriteLine("Aún no hay servicios entregados.");
            }
            else
            {
                Console.WriteLine("Líderes con " + maximo + " servicios:");

                foreach (Tecnico t in copia)
                {
                    if (Algoritmos.ContarFinalizados(t, datos.Solicitudes) == maximo)
                    {
                        Console.WriteLine(t.Codigo + " - " + t.NombreCompleto);
                    }
                }
            }
        }

        public void EquiposPorTipo()
        {
            Encabezado("EQUIPOS ATENDIDOS POR TIPO - SOLO ENTREGADAS");

            string[] tipos = { "Portatil", "Escritorio", "Impresora" };

            foreach (string tipo in tipos)
            {
                int servicios = 0;
                List<string> equipos = new List<string>();

                foreach (SolicitudServicio s in datos.Solicitudes)
                {
                    if (s.Estado == "Entregada" && s.Equipo.Tipo == tipo)
                    {
                        servicios++;

                        if (!equipos.Contains(s.Equipo.Codigo))
                        {
                            equipos.Add(s.Equipo.Codigo);
                        }
                    }
                }

                Console.WriteLine(tipo + " | Equipos distintos: " + equipos.Count + " | Servicios: " + servicios);
            }
        }

        public void SolicitudesOrdenadas(bool porCosto, bool ascendente)
        {
            SolicitudServicio[] copia = datos.Solicitudes.ToArray();

            if (porCosto)
            {
                Encabezado("SOLICITUDES POR COSTO - BURBUJA");
                Algoritmos.Burbuja(copia, ascendente);

                Console.WriteLine("Código | Estado | Cliente | Mano de obra | Repuestos | Total");
            }
            else
            {
                Encabezado("SOLICITUDES POR FECHA DE RECEPCIÓN - QUICK SORT");
                Algoritmos.QuickSort(copia, ascendente, false);

                Console.WriteLine("Código | Recepción | Prioridad | Estado | Técnico");
            }

            if (ascendente)
            {
                Console.WriteLine("Orden ascendente");
            }
            else
            {
                Console.WriteLine("Orden descendente");
            }

            decimal suma = 0;

            foreach (SolicitudServicio s in copia)
            {
                if (porCosto)
                {
                    Console.WriteLine(s.Codigo + " | " + s.Estado + " | " + s.Cliente.NombreCompleto + " | Q " + s.CostoManoObra.ToString("F2") + " | Q " + s.CostoRepuestos.ToString("F2") + " | Q " + s.Total.ToString("F2"));

                    suma += s.Total;
                }
                else
                {
                    Console.WriteLine(s.Codigo + " | " + s.FechaRecepcion + " | " + s.Prioridad + " | " + s.Estado + " | " + NombreTecnico(s));
                }
            }

            Console.WriteLine("Registros: " + copia.Length);

            if (porCosto)
            {
                Console.WriteLine("Suma de importes: Q " + suma.ToString("F2"));
                Console.WriteLine("Los importes de solicitudes activas son provisionales. " + "Esta suma no es ingreso.");
            }
        }

        public void Inventario(bool soloMinimos)
        {
            Encabezado("INVENTARIO - SHELL POR EXISTENCIA ASCENDENTE");

            Repuesto[] copia = datos.Repuestos.ToArray();
            Algoritmos.Shell(copia, true);

            int cantidad = 0;

            foreach (Repuesto r in copia)
            {
                if (!soloMinimos || r.EstaEnMinimo())
                {
                    string alerta = "Disponible";

                    if (r.Existencia == 0)
                    {
                        alerta = "Agotado";
                    }
                    else if (r.EstaEnMinimo())
                    {
                        alerta = "En mínimo o por debajo";
                    }

                    Console.WriteLine(r.Codigo + " | " + r.Nombre + " | " + r.Categoria + " | Existencia: " + r.Existencia + " | Mínimo: " + r.ExistenciaMinima + " | Precio: Q " + r.PrecioVenta.ToString("F2") + " | Utilizados: " + r.CantidadUtilizada + " | " + alerta);

                    cantidad++;
                }
            }

            Console.WriteLine("Registros: " + cantidad);

            if (cantidad == 0)
            {
                Console.WriteLine("No hay repuestos que mostrar con este filtro.");
            }
        }

        public void Ingresos()
        {
            Encabezado("INGRESOS POR SERVICIOS ENTREGADOS");

            int cantidad = 0;
            decimal manoObra = 0;
            decimal repuestos = 0;

            foreach (SolicitudServicio s in datos.Solicitudes)
            {
                if (s.Estado == "Entregada")
                {
                    Console.WriteLine(s.Codigo + " | " + s.FechaEntrega + " | " + s.Cliente.NombreCompleto + " | Mano de obra: Q " + s.CostoManoObra.ToString("F2") + " | Repuestos: Q " + s.CostoRepuestos.ToString("F2") + " | Total: Q " + s.Total.ToString("F2"));

                    manoObra += s.CostoManoObra;
                    repuestos += s.CostoRepuestos;
                    cantidad++;
                }
            }

            Console.WriteLine("Servicios entregados: " + cantidad);
            Console.WriteLine("Mano de obra: Q " + manoObra.ToString("F2"));
            Console.WriteLine("Repuestos: Q " + repuestos.ToString("F2"));
            Console.WriteLine("Ingreso total: Q " + (manoObra + repuestos).ToString("F2"));

            Console.WriteLine("Representa importes de servicios entregados; " + "no utilidad neta ni control de pagos.");
        }

        public int[,] ConstruirCargaSemanal(DateTime lunes)
        {
            lunes = lunes.Date;

            if (lunes > DateTime.MaxValue.Date.AddDays(-5))
            {
                throw new ArgumentException("La semana excede el rango de fechas permitido.");
            }

            if (lunes.DayOfWeek != DayOfWeek.Monday)
            {
                throw new ArgumentException("La fecha inicial debe ser lunes.");
            }

            int[,] matriz = new int[datos.Tecnicos.Count, 6];

            for (int fila = 0; fila < datos.Tecnicos.Count; fila++)
            {
                foreach (SolicitudServicio s in datos.Solicitudes)
                {
                    if (s.TecnicoAsignado == datos.Tecnicos[fila] && s.FechaAsignacion.HasValue)
                    {
                        int dia = (s.FechaAsignacion.Value.Date - lunes).Days;

                        if (dia >= 0 && dia < 6)
                        {
                            matriz[fila, dia]++;
                        }
                    }
                }
            }

            return matriz;
        }

        public void CargaSemanal(DateTime lunes)
        {
            lunes = lunes.Date;
            int[,] matriz = ConstruirCargaSemanal(lunes);

            string[] dias = { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb" };

            int[] totalesTecnicos = new int[datos.Tecnicos.Count];
            int[] totalesDias = new int[6];

            int total = 0;
            int maxTecnico = 0;
            int maxDia = 0;
            int maxCelda = 0;

            Encabezado("CARGA SEMANAL: " + lunes.ToString("dd/MM/yyyy") + " AL " + lunes.AddDays(5).ToString("dd/MM/yyyy"));

            Console.WriteLine("Cantidad de solicitudes asignadas por día.");
            Console.WriteLine("Técnico\tLun\tMar\tMié\tJue\tVie\tSáb\tTotal");

            for (int fila = 0; fila < datos.Tecnicos.Count; fila++)
            {
                Console.Write(datos.Tecnicos[fila].Codigo);

                for (int columna = 0; columna < 6; columna++)
                {
                    int valor = matriz[fila, columna];

                    Console.Write("\t" + valor);

                    totalesTecnicos[fila] += valor;
                    totalesDias[columna] += valor;
                    total += valor;

                    if (valor > maxCelda)
                    {
                        maxCelda = valor;
                    }
                }

                Console.WriteLine("\t" + totalesTecnicos[fila]);

                if (totalesTecnicos[fila] > maxTecnico)
                {
                    maxTecnico = totalesTecnicos[fila];
                }
            }

            Console.Write("Total");

            for (int columna = 0; columna < 6; columna++)
            {
                Console.Write("\t" + totalesDias[columna]);

                if (totalesDias[columna] > maxDia)
                {
                    maxDia = totalesDias[columna];
                }
            }

            Console.WriteLine("\t" + total);

            if (total == 0)
            {
                Console.WriteLine("No hay asignaciones registradas para esta semana.");
                return;
            }

            Console.WriteLine("Técnicos con mayor carga: " + maxTecnico);

            for (int fila = 0; fila < datos.Tecnicos.Count; fila++)
            {
                if (totalesTecnicos[fila] == maxTecnico)
                {
                    Console.WriteLine(datos.Tecnicos[fila].Codigo + " - " + datos.Tecnicos[fila].NombreCompleto);
                }
            }

            Console.WriteLine("Días con mayor carga: " + maxDia);

            for (int columna = 0; columna < 6; columna++)
            {
                if (totalesDias[columna] == maxDia)
                {
                    Console.WriteLine(dias[columna]);
                }
            }

            Console.WriteLine("Mayor carga individual: " + maxCelda);

            for (int fila = 0; fila < datos.Tecnicos.Count; fila++)
            {
                for (int columna = 0; columna < 6; columna++)
                {
                    if (matriz[fila, columna] == maxCelda)
                    {
                        Console.WriteLine(datos.Tecnicos[fila].Codigo + " | " + dias[columna]);
                    }
                }
            }

            Console.WriteLine("Incluye asignaciones conservadas de solicitudes " + "entregadas o canceladas.");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            DatosSistema datos = new DatosSistema();
            GestorAtencion gestor = new GestorAtencion(datos);
            MenuConsola menu = new MenuConsola(datos, gestor);

            menu.Ejecutar();
        }
    }
}
