using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTW.Models
{
    public class Cart
    {
        public IList<CartItems> Item { get; set; }
        public Cart()
        {
            Item = new List<CartItems>();
        }
        public int countCartItems()
        {
            return Item.Sum(i => i.Quantity);
        }
        public void addItems(int proId, int sltQuantity)
        {
            using (var dc = new MobileShopEntities())
            {
                var pro = dc.SanPhams.Where(p => p.ID == proId).FirstOrDefault();
                if (pro != null)
                {
                    var ci = Item.Where(i => i.Product.ID == proId).FirstOrDefault();
                    if (ci == null)
                    {
                        ci = new CartItems { Product = pro, Quantity = sltQuantity };
                        Item.Add(ci);
                    }
                    else
                    {
                        ci.Quantity += sltQuantity;
                    }
                }
            }
        }

        public void RemoveItem(int proId)
        {
            var i = Item.Where(it => it.Product.ID == proId).FirstOrDefault();
            if(i != null)
            {
                Item.Remove(i);
            }
        
        }
        public void Update(int proId, int quantity)
        {
            var i = Item.Where(it => it.Product.ID == proId).FirstOrDefault();

            if (i != null)
            {
                i.Quantity = quantity;
            }
        }

        public void CheckOut()
        {
            Item.Clear();
        }
        
    }
    public class CartItems
    {
        public SanPham Product { get; set; }
        public int Quantity { get; set; }
    } 
}