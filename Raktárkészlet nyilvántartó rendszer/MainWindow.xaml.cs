using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raktárkészlet_nyilvántartó_rendszer
{
    public partial class MainWindow : Window
    {
        private ServerConnection con;

        Border cb;



        public MainWindow()
        {
            InitializeComponent();
            con = new ServerConnection();
            clearContent();
        }

        private async void ClickAdd(object sender, EventArgs e)
        {
            try
            {
                bool success = await con.CreateProduct(
    nameInput.Text,
    typeInput.Text,
    Convert.ToInt32(priceInput.Text)
);



                if (success)
                {
                    MessageBox.Show("Termék sikeresen hozzáadva");
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Hiba történt a termék hozzáadásakor");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("error:" + err.Message);
            }



        }

        private async void ClickShowAll(object sender, EventArgs e)
        {
            ClearDisplay();

            List<JsonData> products = await con.GetAllProducts();


            if (products.Count == 0)
            {
                MessageBox.Show("Nincsenek termékek.");
                return;
            }

            Everything.Children.Clear();

            foreach (var p in products)
            {

                cb = new Border
                {
                    Width = 240,
                    Height = 170,
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 75)),
                    CornerRadius = new CornerRadius(20),

                    Child = new TextBlock
                    {
                        Text = $"ID: {p.id}\nNév: {p.name}\nTípus: {p.type}\nÁr: {p.price}",
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 16,
                    },
                };
                Everything.Children.Add(cb);
            }
        }

        private async void ClickShowById(object sender, EventArgs e)
        {
            ClearDisplay();

            if (!int.TryParse(IdInput.Text, out int id))
            {
                MessageBox.Show("Érvénytelen azonosító.");
                return;
            }

            JsonData product = await con.GetProductById(id.ToString());
            if (product != null)
            {

                var tb = new TextBlock
                {
                    Text = $"> ID: {product.id} Név: {product.name} Típus: {product.type} Ár: {product.price}",
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 24,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 30, 0, 0)
                };
                
                SearchRes.Children.Add(tb);
            }
            else
            {
                MessageBox.Show("Nem található ilyen azonosítójú termék.");
            }
        }

        private async void ClickEdit(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(IdInput.Text.Trim(), out int id))
            {
                MessageBox.Show("Érvénytelen termékazonosító.");
                return;
            }

            string newName = nameInput.Text.Trim();
            string newType = typeInput.Text.Trim();
            if (!int.TryParse(priceInput.Text.Trim(), out int newPrice))
            {
                MessageBox.Show("Érvénytelen ár.");
                return;
            }

            bool success = await con.EditProduct(id, newName, newType, newPrice);
            if (success)
            {
                MessageBox.Show("Termék sikeresen módosítva.");
                ClearDisplay();
                ClickShowAll(null, null);
            }
            else
            {
                MessageBox.Show("Hiba történt a termék módosításakor.");
            }
        }

        private async void ClickDelete(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(IdInput.Text.Trim(), out int id))
            {
                MessageBox.Show("Érvénytelen termékazonosító.");
                return;
            }

            bool success = await con.DeleteProduct(id);
            if (success)
            {
                MessageBox.Show("Termék sikeresen törölve.");
                ClearDisplay();
                ClickShowAll(null, null);
            }
            else
            {
                MessageBox.Show("Hiba történt a termék törlésekor.");
            }
        }

        private async void ClickShowByName(object sender, EventArgs e)
        {
            ClearDisplay();

            string name = nameInput.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Érvénytelen név.");
                return;
            }

            JsonData product = await con.GetProductByName(name);
            if (product != null)
            {
                var tb = new TextBlock
                {
                    Text = $"> ID: {product.id} — Név: {product.name} — Típus: {product.type} — Ár: {product.price}",
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 24,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20,30,0,0)
                    
                };
                SearchRes.Children.Add(tb);
            }
            else
            {
                MessageBox.Show("Nem található ilyen nevű termék.");
            }
        }

        private void ClearDisplay()
        {
            SearchRes.Children.Clear();
        }

        private void ClearInputs()
        {
            nameInput.Text = string.Empty;
            typeInput.Text = string.Empty;
            priceInput.Text = string.Empty;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text == (string)tb.Tag)
            {
                tb.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = (string)tb.Tag;
            }
        }

        private void Items_menaggement_Click(object sender, RoutedEventArgs e)
        {
            item_container.Visibility = Visibility.Visible;
            warehouse_container.Visibility = Visibility.Hidden;

            Itemadd.Visibility = Visibility.Visible;
            ShowbynameB.Visibility = Visibility.Visible;
            ShowallB.Visibility = Visibility.Visible;
            showbyidB.Visibility = Visibility.Visible;
            itemedit.Visibility = Visibility.Visible;
            itemdelete.Visibility = Visibility.Visible;
            showbywhB.Visibility = Visibility.Visible;
            showlowB.Visibility = Visibility.Visible;

            WH_add.Visibility = Visibility.Collapsed;
            WH_searchbyID.Visibility = Visibility.Collapsed;
            WH_ShowAll.Visibility = Visibility.Collapsed;
            WH_delStock.Visibility = Visibility.Collapsed;
        }

        private void Warehouse_menaggement_Click(object sender, RoutedEventArgs e)
        {
            item_container.Visibility = Visibility.Hidden;
            warehouse_container.Visibility = Visibility.Visible;

            Itemadd.Visibility = Visibility.Collapsed;
            ShowbynameB.Visibility = Visibility.Collapsed;
            ShowallB.Visibility = Visibility.Collapsed;
            showbyidB.Visibility = Visibility.Collapsed;
            itemedit.Visibility = Visibility.Collapsed;
            itemdelete.Visibility = Visibility.Collapsed;
            showbywhB.Visibility = Visibility.Collapsed;
            showlowB.Visibility = Visibility.Collapsed;

            WH_add.Visibility = Visibility.Visible;
            WH_searchbyID.Visibility = Visibility.Visible;
            WH_ShowAll.Visibility = Visibility.Visible;
            WH_delStock.Visibility = Visibility.Visible;
        }

        void clearContent()
        {
            item_container.Visibility = Visibility.Hidden;
            warehouse_container.Visibility = Visibility.Hidden;
            Itemadd.Visibility = Visibility.Collapsed;
            ShowbynameB.Visibility = Visibility.Collapsed;
            ShowallB.Visibility = Visibility.Collapsed;
            showbyidB.Visibility = Visibility.Collapsed;
            itemedit.Visibility = Visibility.Collapsed;
            itemdelete.Visibility = Visibility.Collapsed;
            WH_add.Visibility = Visibility.Collapsed;
            WH_searchbyID.Visibility = Visibility.Collapsed;
            WH_ShowAll.Visibility = Visibility.Collapsed;
            showbywhB.Visibility = Visibility.Collapsed;
            showlowB.Visibility = Visibility.Collapsed;
            WH_delStock.Visibility = Visibility.Collapsed;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            clearContent();
        }

        private async void WH_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WH_name.Text.Length == 0 || WH_location.Text.Length == 0 || WH_capacity.Text.Length == 0 || WH_manager_name.Text.Length == 0)
                {
                    MessageBox.Show("nem jók a mezők");
                    return;
                }
                int temp;
                if (!int.TryParse(WH_capacity.Text, out temp))
                {
                    MessageBox.Show("csak számot lehet megadni");
                    return;
                }

                bool success = await con.CreateWarehouse(
                    WH_name.Text,
                    WH_location.Text,
                    WH_capacity.Text,
                    WH_manager_name.Text,
                    WH_notes.Text
                );

                if (success)
                {
                    MessageBox.Show("Sikeres létrehozás");
                }
                else
                {
                    MessageBox.Show("Sikertelen létrehozás");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Hiba: " + err.Message);
            }
        }

        private async void WH_searchbyID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(WH_id.Text, out int id))
                {
                    MessageBox.Show("Érvénytelen ID!");
                    return;
                }

                Warehouse warehouse = await con.GetWarehouseById(id);

                if (warehouse == null)
                {
                    MessageBox.Show("Nem található ilyen ID-jű raktár.");
                    return;
                }

                cb = new Border
                {
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 75)),
                    CornerRadius = new CornerRadius(20),
                    Height = 300,

                    Child = new TextBlock
                    {
                        Text = $"ID: {warehouse.id}\nNév: {warehouse.name}\nHelye: {warehouse.location}\nKapacitás: {warehouse.capacity}\nVezető név: {warehouse.manager_name}\nEgyéb megjegyzések: {warehouse.notes}",
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 16,
                    },
                };

                WH_searchResult.Children.Clear();
                WH_searchResult.Children.Add(cb);
            }
            catch (Exception err)
            {
                MessageBox.Show("Hiba: " + err.Message);
            }
        }



        private async void WH_ShowAll_Click(object sender, RoutedEventArgs e)
        {
            WHEverything.Children.Clear();
            List <Warehouse> WH_all = await con.GetAllWarehouses();

            if (WH_all.Count == 0)
            {
                MessageBox.Show("Nincs adatod");
            }

            try
            {
                foreach (var WH in WH_all)
                {

                    cb = new Border
                    {
                        Width = 240,
                        Height = 170,
                        Margin = new Thickness(10),
                        Background = new SolidColorBrush(Color.FromRgb(0, 0, 75)),
                        CornerRadius = new CornerRadius(20),

                        Child = new TextBlock
                        {
                            Text = $"ID: {WH.id}\nNév: {WH.name}\nVezető név: {WH.manager_name}",
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Foreground = Brushes.White,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            FontSize = 16,
                        },
                    };
                    WHEverything.Children.Add(cb);

                }
            }
            catch (Exception err)
            {

                MessageBox.Show("error: " + err.Message);
            }

          
        }

        private async void ClickShowByWh(object s, RoutedEventArgs e)
        {
            ClearDisplay();

            List<Products> products = await con.FilterStock(whNameInput.Text);


            if (products.Count == 0)
            {
                MessageBox.Show("Nincsenek termékek.");
                return;
            }

            foreach (var p in products)
            {

                cb = new Border
                {
                    Width = 240,
                    Height = 170,
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 75)),
                    CornerRadius = new CornerRadius(20),

                    Child = new TextBlock
                    {
                        Text = $"ID: {p.id}\nNév: {p.name}\nRaktár ID: {p.warehouseId}\nDarab: {p.amount}",
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 16,
                    }
                };
                SearchRes.Children.Add(cb);
            }
        }

        private async void ClickShowLow(object s, RoutedEventArgs e)
        {
            ClearDisplay();

            List<Products> products = await con.ListLowStock();


            if (products.Count == 0)
            {
                MessageBox.Show("Nincsenek termékek.");
                return;
            }

            foreach (var p in products)
            {

                cb = new Border
                {
                    Width = 240,
                    Height = 170,
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 75)),
                    CornerRadius = new CornerRadius(20),

                    Child = new TextBlock
                    {
                        Text = $"ID: {p.id}\nNév: {p.name}\nRaktár ID: {p.warehouseId}\nDarab: {p.amount}",
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 16,
                    }
                };
                SearchRes.Children.Add(cb);
            }
        }

        private async void WH_DeleteStock_Click(object s, RoutedEventArgs e)
        {
            if (!int.TryParse(WH_id.Text.Trim(), out int id))
            {
                MessageBox.Show("Érvénytelen ID!");
                return;
            }

            bool success = await con.DeleteStock(id);
            if (success)
            {
                MessageBox.Show("Termékek sikeresen törölve.");
                ClearDisplay();
                ClickShowAll(null, null);
            }
            else
            {
                MessageBox.Show("Hiba történt a termékek törlésekor.");
            }
        }
    }
}
