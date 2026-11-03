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

            SolicitudServicio solicitud = new SolicitudServicio(datos.GenerarCodigo("SOL"),cliente, equipo, problema, prioridad, datos.Solicitudes.Count + 1
            );

            solicitud.PasarAEspera();
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

            solicitud.AsignarTecnico(elegido);
            cola.Dequeue();

            return solicitud;
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
                Console.WriteLine("0. Salir");

                opcion = LeerEntero("Seleccione una opción: ", 0, 6);

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
                    case 0:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                }

            } while (opcion != 0);
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
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Seleccione: ", 0, 2);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            CrearSolicitud();
                            break;
                        case 2:
                            if (datos.Solicitudes.Count == 0)
                            {
                                Console.WriteLine("Sin solicitudes registradas.");
                            }

                            foreach (SolicitudServicio solicitud in datos.Solicitudes)
                            {
                                MostrarSolicitud(solicitud);
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
        }

        public bool EsActiva()
        {
            return estado != "Entregada" && estado != "Cancelada";
        }

        internal void PasarAEspera()
        {
            if (estado != "Recibida")
            {
                throw new ArgumentException("Solo una solicitud recibida puede entrar en espera.");
            }

            estado = "EnEspera";
        }

        internal void AsignarTecnico(Tecnico tecnico)
        {
            if (estado != "EnEspera")
            {
                throw new ArgumentException("La solicitud no está en espera.");
            }

            if (tecnico == null ||
                !tecnico.Habilitado ||
                tecnico.Especialidad != equipo.Tipo)
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

            tecnicoAsignado = tecnico;
            fechaAsignacion = fecha;
            estado = "Asignada";
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
