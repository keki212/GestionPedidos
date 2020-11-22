using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GestionPedidos
{
    class RecursoDb
    {
        SqlConnection conexionDb;
        string conexion;

        public RecursoDb()
        {
            conexion = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            
        }

        public string conectar()
        {
            conexionDb = new SqlConnection(conexion);

            return conexionDb.ToString();
            
        }
       
    }
}
