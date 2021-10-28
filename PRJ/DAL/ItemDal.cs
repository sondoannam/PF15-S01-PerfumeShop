using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public static class ItemMatch
    {
        public const int GET_ALL = 0;
        public const int MATCH_BY_NAME = 1;
        public const int MATCH_BY_GENDER = 2;
        public const int MATCH_BY_BRAND = 3;
    }
    public class ItemDal
    {
        private MySqlConnection connection = DbConfig.GetConnection();
        private string query;

        public Perfume GetItemByID(int itemID)
        {
            Perfume item = null;
            
                try
                {
                    connection.Open();
                    query = @"SELECT perfume_ID, perfume_name, fragrance_family, classification,
                                    volume, top_notes, heart_notes, base_notes, gender, ingredients, form,
                                    year_launched, strength, origin, price, total_quantity, product_status,
                                    ifnull(perfume_description, '') as perfume_description,
                                    brand_name
                                    FROM Perfumes, Brands 
                                    WHERE perfume_ID=@itemID AND Perfumes.brand_ID = Brands.brand_ID;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@itemID", itemID);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        item = GetItem(reader);
                    }
                    reader.Close();
                }
                catch{ }
                finally
                {
                    connection.Close();
                }
            
            return item;
        }

        internal Perfume GetItem(MySqlDataReader reader)
        {
            Perfume item = new Perfume();
            item.Perfume_ID = reader.GetInt32("perfume_ID"); 
            item.PerfumeName = reader.GetString("perfume_name");
            item.FragranceFamily = reader.GetString("fragrance_family");
            item.Classification = reader.GetString("classification"); 
            item.Volume = reader.GetString("volume");
            item.TopNotes = reader.GetString("top_notes");
            item.HeartNotes = reader.GetString("heart_notes");
            item.BaseNotes = reader.GetString("base_notes");
            item.Gender = reader.GetString("gender");
            item.Ingredients = reader.GetString("ingredients");
            item.Form = reader.GetString("form");
            item.YearLaunched = reader.GetString("year_launched");
            item.Strength = reader.GetString("strength");
            item.Origin = reader.GetString("origin");
            item.PerfumePrice = reader.GetDecimal("price");
            item.TotalQuantity = reader.GetInt32("total_quantity");
            item.Status = reader.GetInt16("product_status");
            item.Description = reader.GetString("perfume_description");
            item.BrandName = reader.GetString("brand_name");
            return item;
        }

        public List<Perfume> GetItems(int itemMatch, Perfume item)
        {
            List<Perfume> list = null;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(" ", connection);
                switch (itemMatch)
                {
                    case ItemMatch.GET_ALL:
                        query = @"SELECT perfume_ID, perfume_name, fragrance_family, classification,
                                    volume, top_notes, heart_notes, base_notes, gender, ingredients, form,
                                    year_launched, strength, origin, price, total_quantity, product_status,
                                    ifnull(perfume_description, '') as perfume_description,
                                    brand_name
                                FROM Perfumes INNER JOIN Brands 
                                ON Perfumes.brand_ID = Brands.brand_ID;";
                        break;
                    case ItemMatch.MATCH_BY_NAME:
                        query = @"SELECT perfume_ID, perfume_name, fragrance_family, classification,
                                    volume, top_notes, heart_notes, base_notes, gender, ingredients, form,
                                    year_launched, strength, origin, price, total_quantity, product_status,
                                    ifnull(perfume_description, '') as perfume_description,
                                    brand_name
                                FROM Perfumes INNER JOIN Brands 
                                ON Perfumes.brand_ID = Brands.brand_ID AND perfume_name like concat('%',@perfumeName,'%');";
                        command.Parameters.AddWithValue("@perfumeName", item.PerfumeName);
                        break;
                    case ItemMatch.MATCH_BY_GENDER:
                        query = @"SELECT perfume_ID, perfume_name, fragrance_family, classification,
                                    volume, top_notes, heart_notes, base_notes, gender, ingredients, form,
                                    year_launched, strength, origin, price, total_quantity, product_status,
                                    ifnull(perfume_description, '') as perfume_description,
                                    brand_name
                                FROM Perfumes INNER JOIN Brands 
                                ON Perfumes.brand_ID = Brands.brand_ID AND Perfumes.gender = @gender;";
                        command.Parameters.AddWithValue("@gender", item.Gender);
                        break;
                    case ItemMatch.MATCH_BY_BRAND:
                        query = @"SELECT perfume_ID, perfume_name, fragrance_family, classification,
                                    volume, top_notes, heart_notes, base_notes, gender, ingredients, form,
                                    year_launched, strength, origin, price, total_quantity, product_status,
                                    ifnull(perfume_description, '') as perfume_description,
                                    brand_name
                                FROM Perfumes INNER JOIN Brands
                                ON Perfumes.brand_ID = Brands.brand_ID AND Brands.brand_name like concat('%',@brandName,'%');";
                        command.Parameters.AddWithValue("@brandName", item.BrandName);
                        break;
                    default:

                        break;
                }
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                list = new List<Perfume>();
                while (reader.Read())
                {
                    list.Add(GetItem(reader));
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return list;
        }
    }
}