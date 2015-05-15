using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductInformationApp
{
    public partial class ProductInfoUI : Form
    {
        Product aProduct = new Product();
        public ProductInfoUI()
        {
            InitializeComponent();
            
        }

        private void ShowProductListView()
        {
            showProductListView.Items.Clear();
            List<Product> aProductList = aProduct.GetAllProductDetails();
            foreach (var products in aProductList)
            {
                ListViewItem item = new ListViewItem(products.code);
                item.SubItems.Add(products.description);
                item.SubItems.Add(products.quantity.ToString());

                showProductListView.Items.Add(item);

            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            aProduct.code = codeTextBox.Text;
            aProduct.description = descriptionTextBox.Text;
            aProduct.quantity = Convert.ToDouble(quantityTextBox.Text);
            string message = aProduct.SaveProductInformation(aProduct);
            MessageBox.Show(message);
            codeTextBox.Clear();
            descriptionTextBox.Clear();
            quantityTextBox.Clear();
            
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            double total = aProduct.GetTotal();
            totalTextBox.Text = total.ToString();
            ShowProductListView();

        }

    }
}
