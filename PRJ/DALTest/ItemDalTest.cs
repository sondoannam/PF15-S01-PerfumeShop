using System;
using Xunit;
using DAL;
using Persistence;
using System.Collections.Generic;

namespace DALTest
{
    public class ItemDalTest
    {
        private ItemDal idal = new ItemDal();
        private Perfume result = new Perfume();
        private List<Perfume> results = new List<Perfume>();

        //Search by ID test
        [Theory]
        [InlineData(1, "Bright Crystal")]
        [InlineData(2, "Light Blue")]
        [InlineData(3, "Jimmy Choo")]
        [InlineData(4, "Eternity")]
        [InlineData(5, "Euphoria")]
        [InlineData(6, "Eternity Cologne")]
        [InlineData(7, "CK One")]
        [InlineData(8, "CK One Shock For Him")]
        [InlineData(9, "Obsession")]
        [InlineData(10, "Obsession Cologne")]
        public void GetItemByIDTest(int itemID, string expected)
        {
            result = idal.GetItemByID(itemID);
            Assert.True(result != null);
            Assert.True(result.Perfume_ID == itemID);
            Assert.True(result.PerfumeName.Equals(expected));
        }    

        //Search by Name test
        [Theory]
        [InlineData("Light", 1)]
        [InlineData("CK", 2)]
        [InlineData("Obsession", 2)]
        [InlineData("Eternity", 2)]
        [InlineData("Jimmy Choo", 1)]
        [InlineData("Crystal", 2)]
        public void GetItemsByNameTest(string searchName, int expected)
        {
            result.PerfumeName = searchName;
            results = idal.GetItems(1, result);
            if (expected == 0)
            {
                Assert.True(results == null);
            }
            else
            {
                Assert.True(results != null);
                Assert.True(results.Count == expected);

                foreach (Perfume p in results)
                {
                    Assert.Contains(searchName.ToLower(), p.PerfumeName.ToLower());
                }
            }
        }

        //Search by Gender test
        [Theory]
        [InlineData("Men", 9)]
        [InlineData("Women", 10)]
        [InlineData("Unisex", 1)]
        public void GetItemsByGenderTest(string gender, int expected)
        {
            result.Gender = gender;
            results = idal.GetItems(2, result);
            Assert.True(results != null);
            Assert.True(results.Count == expected);

            foreach (Perfume p in results)
                {
                    Assert.True(p.Gender.Equals(gender));
                }
        }

        //Search by Brand test
        [Theory]
        [InlineData("Versace", 4)]
        [InlineData("Calvin Klein", 7)]
        [InlineData("Dolce & Gabbana", 4)]
        [InlineData("Mont Blanc", 1)]
        [InlineData("Christian Dior", 3)]
        [InlineData("Jimmy Choo", 1)]
        public void GetItemsByBrandTest(string brand, int expected)
        {
            result.BrandName = brand;
            results = idal.GetItems(3, result);
            Assert.True(results != null);
            Assert.True(results.Count == expected);

            foreach (Perfume p in results)
                {
                    Assert.True(p.BrandName.Equals(brand));
                }
        }

    }
}