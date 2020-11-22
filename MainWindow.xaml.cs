using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conexionDb;

        public MainWindow()
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            conexionDb = new SqlConnection(conexion);

            MuestraClientes();
            MuestraPedidos();
        }

        private void MuestraClientes()
        {
            try
            {
                string sql = "SELECT * FROM Clientes";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conexionDb);

                using (sqlDataAdapter)
                {
                    DataTable tablaClientes = new DataTable();

                    sqlDataAdapter.Fill(tablaClientes);

                    ListaClientes.DisplayMemberPath = "Nombre";
                    ListaClientes.SelectedValuePath = "Id";
                    ListaClientes.ItemsSource = tablaClientes.DefaultView;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al intentar cargar datos de clientes");
                Console.WriteLine(ex);
            }
        }

        private void MuestraPedidos()
        {
            try
            {
                string sql = "SELECT Id, CONCAT(IdCliente, '   ', FechaPedido, '   ', FormaPago) AS Todos FROM Pedidos";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conexionDb);

                using (sqlDataAdapter)
                {
                    DataTable tablaPedidos = new DataTable();

                    sqlDataAdapter.Fill(tablaPedidos);

                    Pedidos.DisplayMemberPath = "Todos";
                    Pedidos.SelectedValuePath = "Id";
                    Pedidos.ItemsSource = tablaPedidos.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostras información! Comunicarse con el Administrador del Sistema");

                Console.WriteLine(ex);
            }
        }

        private void MuestraPedidosCliente()
        {
            try
            {
                string sql = "SELECT CONCAT(FechaPedido, ' ') AS Fecha FROM Pedidos P INNER JOIN Clientes C ON C.Id = P.IdCliente " +
                "WHERE C.Id =@ClienteId";

                SqlCommand sqlCommand = new SqlCommand(sql, conexionDb);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("ClienteId", ListaClientes.SelectedValue);

                    DataTable tablaPedidos = new DataTable();
                    sqlDataAdapter.Fill(tablaPedidos);
                    PedidosCliente.DisplayMemberPath = "Fecha";
                    PedidosCliente.SelectedValuePath = "Id";
                    PedidosCliente.ItemsSource = tablaPedidos.DefaultView;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al intentar mostrar información de pedidos! Comuniquese con el Administrador del Sistema");

                Console.WriteLine(ex); 
            }

            
        }

        private void BorrarPedido()
        {
            if(Pedidos.SelectedValue != null)
            {
                try
                {
                    string sql = "DELETE FROM Pedidos WHERE Id =@PedidoId";

                    SqlCommand sqlCommand = new SqlCommand(sql, conexionDb);

                    conexionDb.Open();

                    sqlCommand.Parameters.AddWithValue("@PedidoId", Pedidos.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    MessageBox.Show("Pedido Eliminado con exito");

                    conexionDb.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al Eliminar el pedido");

                    Console.WriteLine(ex);

                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un pedido");
            }
            
        }


        private void GuardarClientes()
        {
            try
            {
                string sql = "INSERT INTO Clientes (Nombre, Direccion, Poblacion, Telefono) " +
                    "VALUES(@Nombre, @Direccion, @Poblacion, @Telefono)";
                SqlCommand sqlCommand = new SqlCommand(sql, conexionDb);

                conexionDb.Open();

                sqlCommand.Parameters.AddWithValue("@Nombre", nombreCliente.Text);
                sqlCommand.Parameters.AddWithValue("@Direccion", direccionCliente.Text);
                sqlCommand.Parameters.AddWithValue("@Poblacion", poblacionCliente.Text);
                sqlCommand.Parameters.AddWithValue("@Telefono", telefonoCliente.Text);

                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Cliente guardado con exito");

                nombreCliente.Text = "";
                direccionCliente.Text = "";
                poblacionCliente.Text = "";
                telefonoCliente.Text = "";

                conexionDb.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al guardar el nuevo cliente! Comuniquese con el administrador \n" +
                    ex);
            }
        }

        private void BorrarCliente()
        {
            try
            {
                string sql = "DELETE FROM Clientes WHERE Id =@ClienteId";

                SqlCommand sqlCommand = new SqlCommand(sql, conexionDb);

                conexionDb.Open();

                sqlCommand.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);

                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Cliente Eliminado con exito");

                conexionDb.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al Eliminar el Cliente \n " + ex.ToString());

                Console.WriteLine(ex);

            }
        }
        private void MuestraCliente()
        {
           if(ListaClientes.SelectedValue != null)
            {
                Actualiza ventanaActualizar = new Actualiza((int)ListaClientes.SelectedValue);
                ventanaActualizar.Show();

                try
                {
                    string sql = "SELECT * FROM Clientes WHERE Id = @ClienteId";

                    SqlCommand sqlCommand = new SqlCommand(sql, conexionDb);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    using (sqlDataAdapter)
                    {
                        sqlCommand.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);

                        DataTable tablaCliente = new DataTable();

                        sqlDataAdapter.Fill(tablaCliente);

                        ventanaActualizar.nombreCliente.Text = tablaCliente.Rows[0]["Nombre"].ToString();
                        ventanaActualizar.direccionCliente.Text = tablaCliente.Rows[0]["Direccion"].ToString();
                        ventanaActualizar.poblacionCliente.Text = tablaCliente.Rows[0]["Poblacion"].ToString();
                        ventanaActualizar.telefonoCliente.Text = tablaCliente.Rows[0]["Telefono"].ToString();

                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al cargar información del cliente \n" + ex);
                }

                //ventanaActualizar.ShowDialog();
                //MuestraClientes();
            }
            else
            {
                MessageBox.Show("No has seleccionado un cliente!");
            }
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BorrarPedido();
            MuestraPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuardarClientes();
            MuestraClientes();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BorrarCliente();
            MuestraClientes();
        }

        private void ListaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidosCliente();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MuestraCliente();
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MuestraClientes();
            MuestraPedidos();
        }
    }
}
