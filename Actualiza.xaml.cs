using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace GestionPedidos
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {
        private int z;
        SqlConnection conexionDb;

        public Actualiza(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            conexionDb = new SqlConnection(conexion);

            z = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ActualizarCliente(z);
        }

        private void ActualizarCliente(int id)
        {
            try
            {
                string sql = "UPDATE Clientes SET Nombre = @Nombre, Direccion=@Direccion, Poblacion=@Poblacion, Telefono=@Telefono" +
                            " WHERE Id=" + id;
                SqlCommand sqlComnand = new SqlCommand(sql, conexionDb);

                conexionDb.Open();

                sqlComnand.Parameters.AddWithValue("@Nombre", nombreCliente.Text);
                sqlComnand.Parameters.AddWithValue("@Direccion", direccionCliente.Text);
                sqlComnand.Parameters.AddWithValue("@Poblacion", poblacionCliente.Text);
                sqlComnand.Parameters.AddWithValue("@Telefono", telefonoCliente.Text);

                sqlComnand.ExecuteNonQuery();

                MessageBox.Show("Cliente actualizado con exito!");

                conexionDb.Close();

                this.Close();

                //MainWindow main = new MainWindow();
                

                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente \n" + ex);
            }
        }
    }
}
