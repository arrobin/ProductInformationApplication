using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductInformationApp
{
    class Product
    {
        public int id;
        public string code;
        public string description;
        public double quantity;

        SqlConnection aConnection = new SqlConnection();
       
        string connectionString = ConfigurationManager.ConnectionStrings["ProductInfoDBConnectionString"].ConnectionString;
        public string SaveProductInformation(Product aProduct)
        {
            if (aProduct.code.Length>=3 && aProduct.quantity>0)
            {
                if (HasThisProductCodeAlready(aProduct.code))
                {
                    aConnection.ConnectionString = connectionString;
                    aConnection.Open();
                    string query = string.Format("UPDATE t_product SET product_quantity={0} WHERE product_code= '{1}'", aProduct.quantity, aProduct.code);

                    SqlCommand aCommand = new SqlCommand(query, aConnection);
                    int rowAffected = aCommand.ExecuteNonQuery();
                    aConnection.Close();
                    if (rowAffected > 0)
                    {
                        return "Update Successful";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
                else
                {
                    aConnection.ConnectionString = connectionString;
                    aConnection.Open();
                    string query = string.Format("INSERT INTO t_product VALUES('{0}','{1}','{2}')", aProduct.code, aProduct.description, aProduct.quantity);

                    SqlCommand aCommand = new SqlCommand(query, aConnection);
                    int rowAffected = aCommand.ExecuteNonQuery();
                    aConnection.Close();
                    if (rowAffected > 0)
                    {
                        return "Save Successful";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
            }
            else
            {
                string info = "";

                if (aProduct.code.Length<3)
                {
                    info += "Product Code Must be at Least Three Characters Long  ";
                }
                if (aProduct.quantity<0)
                {
                    info += "\nProduct Quantity Must be Positive Number";
                }
                return info;
            }
           
        }


        public List<Product> GetAllProductDetails()
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = string.Format("SELECT * FROM t_product");

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            SqlDataReader aReader = aCommand.ExecuteReader();

            List<Product> aProducts = new List<Product>();
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Product product = new Product();
                    product.code = aReader[1].ToString();
                    product.description = aReader[2].ToString();
                    product.quantity = Convert.ToDouble(aReader[3].ToString());

                    aProducts.Add(product);
                }
            }
            aConnection.Close();
            return aProducts;
        }

        public double GetTotal()
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = "SELECT SUM(product_quantity) FROM t_product";
            SqlCommand aCommand = new SqlCommand(query, aConnection);
            
            object result = aCommand.ExecuteScalar();
            double total = Convert.ToDouble(Convert.ToString(result));

            aConnection.Close();
            return total;

        }

        public bool HasThisProductCodeAlready(string code)
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = string.Format("SELECT * FROM t_product WHERE product_code='{0}'", code);

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            SqlDataReader aReader = aCommand.ExecuteReader();
            bool message = aReader.HasRows;
            aConnection.Close();
            return message;
        }
    }
}
